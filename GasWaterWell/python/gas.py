# gas_water_two.py
# @author Cabbage
# @description
# @created 2019-05-10T10:29:40.405Z+08:00
# @last-modified 2019-09-04T16:09:24.251Z+08:00
#
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

    product_df[["qg", "pe", "pwf"]] = product_df[[
        "qg", "pe", "pwf"]].replace("", 0).astype(float)

    # 如果pwf>pe, 至零
    product_df.loc[product_df["pwf"] > product_df["pe"], "pwf"] = 0
    product_df.loc[product_df["pwf"] > product_df["pe"], "pe"] = 0

    result["error"] = 0
    result["date"] = product_df.date.values
    result["qg"] = product_df.qg.values * 1e4  # 转换成方
    result["pe"] = product_df.pe.values
    result["pwf"] = product_df.pwf.values

    return result


def cal_qaof(basic_dict, pvt_dict, product_dict):
    result = {
        "error": 1,
    }

    # 赋值基础参数
    basic_re = basic_dict["re"]
    basic_rw = basic_dict["rw"]
    basic_S = basic_dict["S"]
    basic_D = basic_dict["D"]
    basic_rhogsc = basic_dict["rhogsc"]

    # 赋值pvt参数
    pvt_p = pvt_dict["p"]
    pvt_rhog = pvt_dict["pg"]
    pvt_miug = pvt_dict["ug"]
    pvt_bg = pvt_dict["bg"]

    # 赋值生产数据参数
    product_date = product_dict["date"]
    product_pe = product_dict["pe"]
    product_pwf = product_dict["pwf"]
    product_qg = product_dict["qg"]

    # 获取表格中的行数
    Pro_n = len(product_pe) + 1
    Pvt_n = len(pvt_p) + 1

    # matlab中是以列举证来存储，这里采用了行矩阵
    P = np.insert(pvt_p, 0, 0)  # 压力
    rhog = np.insert(pvt_rhog, 0, 0)  # 气体密度
    miug = np.insert(pvt_miug, 0, 0)  # 气体粘度
    Bg = np.insert(pvt_bg, 0, 0)  # 气体体积系数

    Data_time = np.insert(product_date, 0, 0)
    Qgr = np.insert(product_qg, 0, 0)  # 实际气井产量

    Pwf = np.insert(product_pwf, 0, 0)  # 井底流压
    Pe = np.insert(product_pe, 0, 0)  # 地层压力

    rn = Pvt_n
    cn = 12

    def integra_nowater(miug, rhog, P):
        # 运用梯形法进行数值积分
        S = np.zeros((rn, cn))
        for i in range(1, rn):
            for j in range(1, cn):
                S[i][j] = rhog[i] / miug[i]
        V = np.zeros((rn, cn))
        for i in range(1, rn):
            if i == 1:
                V[i] = S[i] * P[i]
            else:
                V[i] = V[i - 1] + (S[i - 1] + S[i]) * (P[i] - P[i - 1]) / 2
        return V

    PSTa_nowater = integra_nowater(miug, rhog, P)  # 拟压力数值表#与原数据相同
    # print(PSTa_nowater)
    # ————————计算累产气——————————%
    # 根据实际生产数据，基于得到的拟压力函数表，插值获得一系列的拟压力值，并计算动态无阻流量
    # ——————————计算A、B系数值—————————————%
    AG1 = 1.842*basic_rhogsc*(np.log(basic_re/basic_rw)-0.75+basic_S)
    BG1 = 1.842*basic_rhogsc**2*basic_D

    KH_nowater = np.zeros(Pro_n)
    PeGS = np.zeros(Pro_n)  # 不考虑产水每一天的地层拟压力
    PwfGS = np.zeros(Pro_n)  # 不考虑产水每一天的井底流压拟压力

    for k in range(1, Pro_n):
        P_temp = P[1:]
        # 无水计算
        PSTa_temp = PSTa_nowater[:, 1][1:]  # 取第一列并将第一个数在拟合的过程中去掉
        interp1_5 = interp1d(P_temp, PSTa_temp)

        PeGS[k] = interp1_5(Pe[k])

        PwfGS[k] = interp1_5(Pwf[k])

        KH_nowater[k] = (AG1*Qgr[k]+BG1*Qgr[k]**2) / \
            (PeGS[k]-PwfGS[k])  # 确定地层系数,mD,m

    deltaPSSRO = PeGS

    AG = AG1/KH_nowater
    BG = BG1/KH_nowater

    def QAOF(A, B, PeS, PwfS):
        # 计算无阻流量
        deltaPs = PeS-PwfS
        Qg = (-A+(A*A+4*B*deltaPs)**0.5)/(2*B)
        Qg = Qg/1E4  # 换算为万方
        return Qg
    # print(BG)

    QgAOF = QAOF(AG, BG, deltaPSSRO, 0)  # 不考虑产水影响的动态无阻流量，万方

    # print("QgAOF: ", QgAOF)

    where_are_inf = np.isinf(AG)
    AG[where_are_inf] = None
    where_are_inf = np.isinf(BG)
    BG[where_are_inf] = None

    result["error"] = 0
    result["qaof"] = QgAOF[1:]
    result["kh"] = KH_nowater[1:]
    result["a"] = AG[1:]
    result["b"] = BG[1:]
    result["pwfgs"] = PwfGS[1:]

    return result


def draw_pic(sql_dict, product_dict, qaof_dict):
    # 赋值生产数据参数
    product_date = product_dict["date"]
    product_pwf = product_dict["pwf"]
    product_qg = product_dict["qg"]

    # 赋值结果数据参数
    QgAOF = qaof_dict["qaof"]
    PwfGS = qaof_dict["pwfgs"]
    KH = qaof_dict["kh"]

    # 开始画图

    pic_name_1 = "gas_1_%d.png" % int(sql_dict["gas_result_index"])
    pic_name_2 = "gas_2_%d.png" % int(sql_dict["gas_result_index"])

    # plt.figure(0)
    # plt.plot(product_qg/100, product_pwf/100, 'o', label='Pwf')
    # plt.plot(product_qg/100, PwfGS/100, 'o', label='PwfS')
    plt.plot(np.arange(0, len(product_date)), KH, 'o', mfc='none', label='KH')
    plt.grid(True)
    plt.legend(loc=0)
    plt.xlabel(r'$time$')
    plt.ylabel(r'$KH(mD.m)$')
    plt.title('Relationship between KH and time')
    plt.savefig("./pic/"+pic_name_1, dpi=200)
    plt.close(0)

    plt.figure(1)
    plt.plot(np.arange(0, len(product_date)), QgAOF /
             100, 'o', mfc='none', label='QgAOF')
    plt.grid(True)
    plt.legend(loc=0)
    plt.xlabel(r'$time$')
    plt.ylabel(r'$Qg(10^6m^3/d)$')
    plt.title('Relationship between QgAOF and time')
    plt.savefig("./pic/"+pic_name_2, dpi=200)
    plt.close(1)

    return pic_name_1, pic_name_2


def save_data(DBSTR, sql_dict, product_dict, qaof_dict, pic_name_1, pic_name_2):
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
        os.path.abspath(__file__))) + "\\pic\\" + pic_name_1
    result_df["pic_path_2"] = os.path.dirname(os.path.dirname(
        os.path.abspath(__file__))) + "\\pic\\" + pic_name_2
    result_df["type"] = 1

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
        "pic_name_1": pic_name_1,
        "pic_name_2": pic_name_2,
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

    print("pvt_water sucess")

    qaof_dict = cal_qaof(basic_dict, pvt_dict, product_dict)
    if qaof_dict["error"] == 1:
        return qaof_dict

    print("qaof sucess")

    pic_1, pic_2 = draw_pic(sql_dict, product_dict, qaof_dict)
    result = save_data(DBSTR, sql_dict, product_dict, qaof_dict, pic_1, pic_2)
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

    # 1571051056 MX11
    # 1571055038 MX8
    # 1571053703 初始
    # 1571191943 正确
    # 1571195852 错误
    # 1571283302 少量数据
    # 1571274624 有水
    # 1571297028 少量新数据
    # 1571298392 五倍水
