import os
import time
import numpy as np
import pandas as pd
from scipy.interpolate import interp1d
import matplotlib.pyplot as plt  # 约定俗成的写法plt
from sqlalchemy import create_engine
import utils

import warnings

warnings.filterwarnings("ignore")


def get_product_data(DBSTR, gas_product_index):
    result = {
        "error": 1,
    }
    engine = create_engine(DBSTR)
    sql = "select * from gas_production_data where gas_product_index = %d;" % gas_product_index
    product_df = pd.read_sql_query(sql, engine)

    if product_df.empty:
        return result

    product_df[["qg", "pe", "pwf"]] = product_df[["qg", "pe", "pwf"]].replace("", 0).astype(float)

    # 如果pwf>pe, 至零
    product_df.loc[product_df["pwf"] > product_df["pe"], "pwf"] = 0
    product_df.loc[product_df["pwf"] > product_df["pe"], "pe"] = 0

    result["error"] = 0
    result["date"] = product_df.date.values
    result["qg"] = product_df.qg.values
    result["pe"] = product_df.pe.values
    result["pwf"] = product_df.pwf.values

    return result


def cal_qaof(basic_dict, product_dict, pvt_dict):
    result = {
        "error": 1,
    }

    # 赋值基础参数
    basic_re = basic_dict["re"]
    basic_rw = basic_dict["rw"]
    basic_S = basic_dict["S"]
    basic_D = basic_dict["D"]

    # 赋值生产数据参数
    product_pe = product_dict["pe"]
    product_pwf = product_dict["pwf"]
    product_qg = product_dict["qg"]

    # 赋值pvt参数
    pvt_p = pvt_dict["p"]
    pvt_ug = pvt_dict["ug"]
    pvt_z = pvt_dict["z"]

    # 计算平均压力
    p = np.sqrt((product_pe ** 2 + product_pwf ** 2) / 2)

    # 利用p和ug， p和z计算插值

    interp1_1 = interp1d(pvt_p, pvt_ug)
    interp1_2 = interp1d(pvt_p, pvt_z)

    ug = []
    z = []
    for one_p in p:
        ug.append(interp1_1(one_p))
        z.append(interp1_2(one_p))

    ug = np.array(ug)
    z = np.array(z)

    # 计算a_temp, b_temp
    pvt_t = pvt_dict["t"]  # 气藏温度， K
    a_temp = 12.9 * ug * z * pvt_t * (np.log((0.472*basic_re)/basic_rw) + basic_S)
    b_temp = 12.9 * ug * z * pvt_t * basic_D

    # 利用生产数据和a_temp、b_temp计算kh
    kh = (a_temp * product_qg + b_temp * product_qg**2) / (product_pe**2 - product_pwf**2)

    # 利用kh计算a, b
    a = a_temp / kh
    b = b_temp / kh

    # 利用pe和a、b计算qaof
    qaof = (-a + np.sqrt(a**2+4*b*(product_pe**2-0.101**2))) / (2*b)

    result["error"] = 0
    result["qaof"] = qaof
    result["kh"] = kh
    result["a"] = a
    result["b"] = b

    return result


def draw_pic(sql_dict, product_dict, qaof_dict):
    # 赋值生产数据参数
    product_date = product_dict["date"]

    # 赋值结果数据参数
    qaof = qaof_dict["qaof"]

    # 开始画图

    pic_name = "pressure_square_%d.png" % int(sql_dict["gas_result_index"])

    plt.figure(0)
    plt.plot(np.arange(0, len(product_date)), qaof / 100, 'o', mfc='none', label='QgAOF')
    plt.grid(True)
    plt.legend(loc=0)
    plt.xlabel(r'$time$')
    plt.ylabel(r'$Qg(10^6m^3/d)$')
    plt.title('Relationship between time and QgAOF')
    plt.savefig("./pic/" + pic_name, dpi=200)
    plt.close(0)

    return pic_name


def save_data(DBSTR, sql_dict, product_dict, qaof_dict, pic_name):
    engine = create_engine(DBSTR)

    # 赋值生产数据参数
    product_date = product_dict["date"]

    # 赋值结果数据参数
    QgAOF = qaof_dict["qaof"]
    KH_nowater = qaof_dict["kh"]
    AG = qaof_dict["a"]
    BG = qaof_dict["b"]

    # 保存到数据库

    title = ["date", "qaof", "kh", "a", "b"]
    # print(QgAOF)
    result_data = [product_date, QgAOF, KH_nowater, AG, BG]

    result_df = pd.DataFrame({items[0]: items[1]
                              for items in zip(title, result_data)})

    result_df["well_id"] = sql_dict["well_id"]
    result_df["gas_result_index"] = sql_dict["gas_result_index"]
    result_df["gas_product_index"] = sql_dict["gas_product_index"]
    result_df["pic_path_1"] = os.path.dirname(os.path.dirname(
        os.path.abspath(__file__))) + "\\pic\\" + pic_name
    result_df["pic_path_2"] = ""
    result_df["type"] = 0

    sql = "select * from gas_result_data where \
    gas_result_index = {gas_result_index} and gas_product_index = {gas_product_index};".format(**sql_dict)
    result_table = pd.read_sql_query(sql, engine)  # 生产数据
    if not result_table.empty:
        delete_sql = "delete gas_result_data where \
        gas_result_index = {gas_result_index} and gas_product_index = {gas_product_index};".format(**sql_dict)
        conn = engine.connect()
        conn.execute(delete_sql)
        conn.close()
    result_df.replace(np.inf, 0, inplace=True)
    result_df.to_sql('gas_result_data', engine,
                     index=False, if_exists='append')
    result = {
        "error": 0,
        "pic_name": pic_name,
    }
    # except Exception as ex:
    #     print(ex)
    return result


def main(sql_dict):
    DBSTR, well_id = sql_dict["DBSTR"], int(sql_dict["well_id"])
    gas_product_index = int(sql_dict["gas_product_index"])
    basic_dict = utils.get_basic_parmas(DBSTR, well_id)
    if basic_dict["error"] == 1:
        return basic_dict

    print("basic sucess")

    pvt_dict = utils.get_gas_pvt(DBSTR, well_id)
    if pvt_dict["error"] == 1:
        return pvt_dict

    print("pvt_gas sucess")

    product_dict = get_product_data(DBSTR, gas_product_index)
    if product_dict["error"] == 1:
        return product_dict

    print("product sucess")

    qaof_dict = cal_qaof(basic_dict, product_dict, pvt_dict)
    if qaof_dict["error"] == 1:
        return qaof_dict

    print("qaof sucess")

    pic = draw_pic(sql_dict, product_dict, qaof_dict)
    result = save_data(DBSTR, sql_dict, product_dict, qaof_dict, pic)
    # print(result)
    return result


if __name__ == '__main__':
    DBSTR = 'mysql+pymysql://root:qwer1234@127.0.0.1/waterwell'
    sql_dict = {
        "DBSTR": DBSTR,
        "well_id": 1571542044,
        "gas_product_index": 1571557469,
        "gas_result_index": int(time.time()),
    }
    main(sql_dict)
