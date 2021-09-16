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


def get_product_data(DBSTR, product_index):
    result = {
        "error": 1,
    }
    engine = create_engine(DBSTR)
    sql = "select * from gaswater_production_data_new where product_index = %d;" % product_index
    product_df = pd.read_sql_query(sql, engine)

    if product_df.empty:
        return result

    product_df[["qg", "qw", "pe", "pwf"]] = product_df[["qg", "qw", "pe", "pwf"]].replace("", 0).astype(float)

    # 如果pwf>pe, 至零
    product_df.loc[product_df["pwf"] > product_df["pe"], "pwf"] = 0
    product_df.loc[product_df["pwf"] > product_df["pe"], "pe"] = 0

    result["error"] = 0
    result["date"] = product_df.date.values
    result["qg"] = product_df.qg.values  # 转换成方
    result["qw"] = product_df.qw.values
    result["pe"] = product_df.pe.values
    result["pwf"] = product_df.pwf.values

    return result


def cal_qaof(basic_dict, pvt_gas_dict, pvt_water_dict, phase_seepage_dict, product_dict):
    result_1 = {
        "error": 1,
    }
    result_2 = {
        "error": 1,
    }
    # try:n

    # 赋值基础参数
    basic_re = basic_dict["re"]
    basic_rw = basic_dict["rw"]
    basic_S = basic_dict["S"]
    basic_D = basic_dict["D"]
    basic_rhogsc = basic_dict["rhogsc"]
    basic_rhowsc = basic_dict["rhowsc"]

    # 赋值pvt参数
    pvt_p = pvt_gas_dict["p"]
    pvt_rhog = pvt_gas_dict["pg"]
    pvt_miug = pvt_gas_dict["ug"]
    pvt_bg = pvt_gas_dict["bg"]
    pvt_rhow = pvt_water_dict["pw"]
    pvt_miuw = pvt_water_dict["uw"]
    pvt_bw = pvt_water_dict["bw"]

    assert len(pvt_rhog) == len(pvt_rhow)

    # 赋值气水相渗
    phase_seepage_sw = phase_seepage_dict["sw"]
    phase_seepage_krg = phase_seepage_dict["krg"]
    phase_seepage_krw = phase_seepage_dict["krw"]

    # 赋值生产数据参数
    product_date = product_dict["date"]
    product_pe = product_dict["pe"]
    product_pwf = product_dict["pwf"]
    product_qg = product_dict["qg"]
    product_qw = product_dict["qw"]

    # 获取表格中的行数
    Pro_n = len(pvt_p) + 1
    Kr_n = len(phase_seepage_sw) + 1
    PrData_n = len(product_date) + 1

    # matlab中是以列举证来存储，这里采用了行矩阵
    P = np.insert(pvt_p, 0, 0)  # 压力
    rhog = np.insert(pvt_rhog, 0, 0)  # 气体密度
    miug = np.insert(pvt_miug, 0, 0)  # 气体粘度
    rhow = np.insert(pvt_rhow, 0, 0)  # 水密度
    miuw = np.insert(pvt_miuw, 0, 0)  # 水粘度
    Bg = np.insert(pvt_bg, 0, 0)  # 气体体积系数
    Bw = np.insert(pvt_bw, 0, 0)  # 水的体积系数

    Sw = np.insert(phase_seepage_sw, 0, 0)  # 水的相渗
    Krg = np.insert(phase_seepage_krg, 0, 0)  # 气体相对密度
    Krw = np.insert(phase_seepage_krw, 0, 0)  # 水的相对密度

    Data_time = np.insert(product_date, 0, 0)
    Qgr = np.insert(product_qg, 0, 0)  # 实际气井产量
    Qwr = np.insert(product_qw, 0, 0)  # 实际产水量

    Pwf = np.insert(product_pwf, 0, 0)  # 井底流压
    Pe = np.insert(product_pe, 0, 0)  # 地层压力
    # Gp = np.insert(
    #     PrData_table.cumulative_gas_production.values, 0, 0)  # 累积产气量

    QWGR = np.zeros(PrData_n, dtype=np.float64)  # QWGR为实际水气比，每万方
    for i in range(1, PrData_n):
        Qgr[i] = Qgr[i] * 1e4  # 转换为方
        if Qgr[i] == 0 or Qwr[i] == 0:
            QWGR[i] = 0
            continue
        QWGR[i] = Qwr[i]/Qgr[i]
    # Qwgr = QWGR * 1e4
    Qwgr = QWGR

    n = len(Qgr)
    # print(PrData_n)
    # ————————根据关系式计算地层压力———————————%
    # 参数输入判断
    NN1 = 0  # 记录井底流压大于地层压力计数
    NN2 = 0  # 记录井底流压大于原始地层压力
    for i in range(1, PrData_n):
        if Pwf[i] > Pe[i]:
            NN1 = NN1 + 1
            # 两数存在错误，互换
            # temp = Pwf[i]
            # Pwf[i] = Pe[i]
            # Pe[i] = temp
            Pwf[i] = 0
            Pe[i] = 0
        # if Pwf[i] > Pi:
        #     NN2 = NN2 + 1
    print(NN1, NN2)

    # 参数处理
    # ——————确定krw，krg，Sw等与压力之间的关系式————————%
    kwratio = np.zeros(Kr_n)
    for i in range(1, Kr_n):
        kwratio[i] = Krw[i]/Krg[i]  # 水相与气相相对渗透率之比
    maxRwg = max(Qwgr)+1.1*1e-4  # 确保最大水气比可以插值获得
    # print(max(QWGR))
    # print(max(Qwgr))
    print("maxRwg:", maxRwg)
    Rwg = np.arange(-1e-7, maxRwg, 1e-5)
    Rwg[0] = 0
    Rwg[1] = 0
    Rwg[2] = 1e-7
    # print(Rwg)


    cn = len(Rwg)
    # print(cn)
    # return


    Rwg = Rwg.reshape((1, cn))

    rn = len(Bw)
    BmBm = np.divide(np.multiply(Bw, miuw),
                     np.multiply(Bg, miug)).reshape(rn, 1)
    BmBm[0] = 0
    KrwKrg = np.matmul(BmBm, Rwg)*1e-4
    KrwP = np.zeros((rn, cn))
    KrgP = np.zeros((rn, cn))

    Krw[0] = 0
    Krg[0] = 0
    interp1_1 = interp1d(kwratio, Krw, fill_value="extrapolate")
    interp1_2 = interp1d(kwratio, Krg, fill_value="extrapolate")
    for i in range(1, rn):
        for j in range(1, cn):
            # print(KrwKrg[i][j])
            KrwP[i][j] = interp1_1(KrwKrg[i][j])
            KrgP[i][j] = interp1_2(KrwKrg[i][j])

    # ————————数值积分过程————————%
    def integra(krgP, krwP, miug, miuw, rhog, rhow, P):
        # 运用梯形法进行数值积分
        [rn, cn] = KrgP.shape  # rn为行数，cn为列数
        S = np.zeros((rn, cn))
        for i in range(1, rn):
            for j in range(1, cn):
                S[i][j] = krgP[i][j]*rhog[i] / \
                    miug[i]+krwP[i][j]*rhow[i]/miuw[i]
        V = np.zeros((rn, cn))
        for i in range(1, rn):
            if i == 1:
                V[i] = S[i]*P[i]
            else:
                V[i] = V[i-1]+(S[i-1]+S[i])*(P[i]-P[i-1])/2
        return V

    def integra_nowater(miug, rhog, P):
        # 运用梯形法进行数值积分
        [rn, cn] = KrgP.shape  # rn为行数，cn为列数
        S = np.zeros((rn, cn))
        for i in range(1, rn):
            for j in range(1, cn):
                S[i][j] = rhog[i] / miug[i]
        V = np.zeros((rn, cn))
        for i in range(1, rn):
            if i == 1:
                V[i] = S[i]*P[i]
            else:
                V[i] = V[i-1]+(S[i-1]+S[i])*(P[i]-P[i-1])/2
        return V

    

    PSTa = integra(KrgP, KrwP, miug, miuw, rhog, rhow, P)  # 拟压力数值表#与原数据相同
    PSTa_nowater = integra_nowater(miug, rhog, P)  # 拟压力数值表#与原数据相同
    # np.savetxt("PSTa.txt", PSTa)
    # np.savetxt("PSTa_nowater.txt", PSTa_nowater)
    # print("P: ", P)
    # print("PSTa: ", PSTa[:,2:3])
    # print("KrgP: ", KrgP[:,2:3])
    # print("KrwP: ", KrwP[:,2:3])
    
    # result_1["error"] = 0
    # result_1["pe"] = P[1:]
    # result_1["psta"] = PSTa[1:,2:3].reshape(1,-1)[0]
    # result_1["psta_nowater"] = PSTa_nowater[1:,2:3].reshape(1,-1)[0]
    # result_1["krgp"] = KrgP[1:,2:3].reshape(1,-1)[0]
    # result_1["krwp"] = KrwP[1:,2:3].reshape(1,-1)[0]

    # print(PSTa_nowater)

    # ————————计算累产气——————————%
    # 根据实际生产数据，基于得到的拟压力函数表，插值获得一系列的拟压力值，并计算动态无阻流量
    # ——————————计算A、B系数值—————————————%
    A1 = 1.842*(basic_rhogsc+basic_rhowsc*Qwgr.T)*(np.log(basic_re/basic_rw)-0.75+basic_S)
    B1 = 1.842*(basic_rhogsc+basic_rhowsc*Qwgr.T)**2*basic_D
    AG1 = 1.842*basic_rhogsc*(np.log(basic_re/basic_rw)-0.75+basic_S)
    BG1 = 1.842*basic_rhogsc**2*basic_D


    KH = np.zeros(n)
    KH_nowater = np.zeros(n)
    PeS = np.zeros(n)  # 每一天的地层压力对应的拟压力
    PwfS = np.zeros(n)  # 每一天的井底流压对应的拟压力
    PeGS = np.zeros(n)  # 不考虑产水每一天的地层拟压力
    PwfGS = np.zeros(n)  # 不考虑产水每一天的井底流压拟压力
    # KHC = 0.0  # 不考虑产水，KH恒定,取前30天的平均KH值
    # KHC_count = 0  # 取30次
    # KHC_sum = 0.0
    print("QWGR: max:%.10f, min:%.10f" % (max(QWGR[1:]), min(QWGR[1:])))
    # print(Rwg.shape)
    # print(PSTa)

    for k in range(1, n):
        PS = np.zeros(rn)
        for i in range(1, rn):
            Rwg_temp = Rwg[0][1:]
            PSTa_temp = PSTa[i][1:]  # .tolist()
            # print(k, i)
            # print("Rwg_temp: ", Rwg_temp)
            # print("PSTa_temp: ", PSTa_temp)
            interp1_3 = interp1d(Rwg_temp, PSTa_temp)
            PS[i] = interp1_3(Qwgr[k])  # QWGR为实际水气比，每万方， 函数拟合有问题
            # print(Qwgr[k])
            # print(PS[i])
            # if PS[i] != PS[i]:
            #     raise "break"
        # print(PS)
        # print(QWGR)
        # print(Rwg_temp)
        # print(PSTa_temp)
        # return
        PS_temp = PS[1:]
        P_temp = P[1:]
        PS_temp[np.isnan(PS_temp)] = 0
        interp1_4 = interp1d(P_temp, PS_temp)  # P_temp 压力，
        # print("P_temp: ", max(P_temp), min(P_temp))
        # print("PS_temp: ", max(PS_temp), min(PS_temp))
        # print("Pe: ", Pe[k])
        if Pe[k] == 0 or Pwf[k] == 0:
            continue

        # 有水计算

        PeS[k] = interp1_4(Pe[k])  # 地层压力,
        # print("PE: ", Pe[k])
        # print("PeS: ", PeS[k])

        PwfS[k] = interp1_4(Pwf[k])  # 井底流压

        # 无水计算
        PSTa_temp = PSTa_nowater[:, 1][1:]  # 取第一列并将第一个数在拟合的过程中去掉
        interp1_5 = interp1d(P_temp, PSTa_temp)

        PeGS[k] = interp1_5(Pe[k])

        PwfGS[k] = interp1_5(Pwf[k])

        # PeGS[k] = Pe[k]
        KH[k] = (A1[k]*Qgr[k]+B1[k]*Qgr[k]**2) / \
            (PeS[k]-PwfS[k])  # 确定地层系数,mD,m
        # print(PeS[k])
        # print(PwfS[k])

        KH_nowater[k] = (AG1*Qgr[k]+BG1*Qgr[k]**2) / \
            (PeGS[k]-PwfGS[k])  # 确定地层系数,mD,m

        # if KH[k] == np.inf:
        #     print(A1[k], Qgr[k], B1[k], PeS[k], PwfS[k])
        # if KH[k] > 0 and KH[k] != np.inf:
        #     KHC_count = KHC_count+1
        #     KHC_sum = KHC_sum+KH[k]
        #     # print("KHC_sum:", KHC_sum)
        #     # print("KHC_count:", KHC_count)
        #     KHC = KHC_sum/KHC_count
    result_1["error"] = 0
    result_1["pe"] = P[1:]
    result_1["psta"] = PS[1:]
    result_1["psta_nowater"] = PSTa_nowater[1:,2:3].reshape(1,-1)[0]
    result_1["krgp"] = KrgP[1:,2:3].reshape(1,-1)[0]
    result_1["krwp"] = KrwP[1:,2:3].reshape(1,-1)[0]


    deltaPSSR = PeS
    deltaPSSRO = PeGS
    A = A1/KH  # KH数组中有零。0原则上不能作为除数，所以会报警告,但理论上不影响
    B = B1/KH

    AG = AG1/KH_nowater
    BG = BG1/KH_nowater

    # print("water: ", A[:5], B[:5])
    # print("nowater: ", AG[:5], BG[:5])

    # print(Qgr)
    # print("KH_nowater:", KH_nowater)

    def QAOF(A, B, PeS, PwfS):
        # 计算无阻流量
        deltaPs = PeS-PwfS
        Qg = (-A+(A*A+4*B*deltaPs)**0.5)/(2*B)
        Qg = Qg/1E4  # 换算为万方
        return Qg
    # print(BG)

    QgwAOF = QAOF(A, B, deltaPSSR, 0)  # 不同水气比对应的产气量,万方
    QgAOF = QAOF(AG, BG, deltaPSSRO, 0)  # 不考虑产水影响的动态无阻流量，万方

    # print("QgwAOF: ", QgwAOF)
    # print(AG, BG)
    # print("QgAOF: ", QgAOF)

    where_are_inf = np.isinf(A)
    A[where_are_inf] = None
    where_are_inf = np.isinf(B)
    B[where_are_inf] = None

    result_2["error"] = 0
    result_2["qwaof"] = QgwAOF[1:]
    result_2["qaof"] = QgAOF[1:]
    result_2["kh"] = KH[1:]
    result_2["a"] = A[1:]
    result_2["b"] = B[1:]
    result_2["pwfs"] = PwfS[1:]
    result_2["pes"] = PeS[1:]

    return result_1, result_2


def draw_pic(sql_dict, product_dict, qaof_dict):
    # 赋值生产数据参数
    product_date = product_dict["date"]
    

    # 赋值结果数据参数
    QgwAOF = qaof_dict["qwaof"]
    QgAOF = qaof_dict["qaof"]
    PwfS = qaof_dict["pwfs"]
    KH = qaof_dict["kh"]

    # 开始画图

    pic_name_1 = "gaswater_1_%d.png" % int(sql_dict["gaswater_result_index"])
    pic_name_2 = "gaswater_2_%d.png" % int(sql_dict["gaswater_result_index"])

    plt.figure(0)
    plt.plot(np.arange(0, len(product_date)), KH, 'o', label='KH')
    # plt.plot(product_qg/100, PwfS/100, 'o', label='PwfS')
    plt.grid(True)
    plt.legend(loc=0)
    plt.xlabel(r'$time$')
    plt.ylabel(r'$KH(mD.m)$')
    # plt.xlabel(r'$Qgr(104m^3/d)$')
    # plt.ylabel(r'$Pwf(MPa)$')
    plt.title('Relationship between KH and time')
    plt.savefig("./pic/"+pic_name_1, dpi=200)
    plt.close(0)

    plt.figure(1)
    plt.plot(np.arange(0, len(product_date)), QgwAOF/100, 'o', mfc='none', label='QgwAOF')
    plt.plot(np.arange(0, len(product_date)), QgAOF/100, label='QgAOF')
    plt.grid(True)
    plt.legend(loc=0)
    plt.xlabel(r'$time$')
    plt.ylabel(r'$Qg(10^6m^3/d)$$time$')
    # plt.ylabel(r'$Qg(104m^3)$')
    plt.title('Relationship between QgAOF and time')
    plt.savefig("./pic/"+pic_name_2, dpi=200)
    plt.close(1)

    return pic_name_1, pic_name_2

def save_data_1(DBSTR, sql_dict, middle_dict):
    engine = create_engine(DBSTR)

    # 赋值中间表
    P = middle_dict["pe"]
    PSTa = middle_dict["psta"]
    PSTa_nowater = middle_dict["psta_nowater"]
    KrgP = middle_dict["krgp"]
    KrwP = middle_dict["krwp"]

    # 保存到数据库

    title = ["pe", "psta", "psta_nowater", "krgp", "krwp"]
    # print(QgAOF)
    result_data = [P, PSTa, PSTa_nowater, KrgP, KrwP]
    print(len(P),len(PSTa),len(KrgP))

    result_df = pd.DataFrame({items[0]: items[1]
                              for items in zip(title, result_data)})

    result_df["well_id"] = int(sql_dict["well_id"])
    result_df["middle_index"] = sql_dict["middle_index"]
    result_df["gaswater_input_id"] = sql_dict["gaswater_product_index"]

    sql = "select * from niyali_middle_table where \
    middle_index = {middle_index} and gaswater_input_id = {gaswater_product_index};".format(**sql_dict)
    result_table = pd.read_sql_query(sql, engine)  # 生产数据
    if not result_table.empty:
        delete_sql = "delete niyali_middle_table where \
        middle_index = {middle_index} and gaswater_input_id = {gaswater_product_index};".format(**sql_dict)
        conn = engine.connect()
        conn.execute(delete_sql)
        conn.close()
    result_df.replace(np.inf, 0, inplace=True)
    result_df.to_sql('niyali_middle_table', engine,
                     index=False, if_exists='append')
    result = {
        "error": 0,
    }
    # except Exception as ex:
    #     print(ex)
    return result


def save_data_2(DBSTR, sql_dict, product_dict, qaof_dict, pic_name_1, pic_name_2):
    engine = create_engine(DBSTR)

    # 赋值生产数据参数
    product_date = product_dict["date"]
    product_pwf = product_dict["pwf"]
    product_pe = product_dict["pe"]

    # 赋值结果数据参数
    QgwAOF = qaof_dict["qwaof"]
    QgAOF = qaof_dict["qaof"]
    KH = qaof_dict["kh"]
    A = qaof_dict["a"]
    B = qaof_dict["b"]
    PwfS = qaof_dict["pwfs"]
    PeS = qaof_dict["pes"]

    # 保存到数据库

    title = ["date", "pwf", "pwfs", "pe", "pes", "unimpeded_flow_water",
             "unimpeded_flow_no_water", "kh", "a", "b"]
    # print(QgAOF)
    result_data = [product_date, product_pwf, PwfS, product_pe, PeS, \
        QgwAOF, QgAOF, KH, A, B]

    result_df = pd.DataFrame({items[0]: items[1]
                              for items in zip(title, result_data)})

    result_df["well_id"] = int(sql_dict["well_id"])
    result_df["gaswater_result_index"] = sql_dict["gaswater_result_index"]
    result_df["gaswater_input_id"] = sql_dict["gaswater_product_index"]
    result_df["pic_path_1"] = os.path.dirname(os.path.dirname(
        os.path.abspath(__file__))) + "\\pic\\" + pic_name_1
    result_df["pic_path_2"] = os.path.dirname(os.path.dirname(
        os.path.abspath(__file__))) + "\\pic\\" + pic_name_2

    sql = "select * from gaswater_result where \
    gaswater_result_index = {gaswater_result_index} and gaswater_input_id = {gaswater_product_index};".format(**sql_dict)
    result_table = pd.read_sql_query(sql, engine)  # 生产数据
    if not result_table.empty:
        delete_sql = "delete gaswater_result where \
        gaswater_result_index = {gaswater_result_index} and gaswater_input_id = {gaswater_product_index};".format(**sql_dict)
        conn = engine.connect()
        conn.execute(delete_sql)
        conn.close()
    result_df.replace(np.inf, 0, inplace=True)
    result_df.to_sql('gaswater_result', engine,
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
    print(sql_dict)
    DBSTR, well_id = sql_dict["DBSTR"], int(sql_dict["well_id"])
    product_index = int(sql_dict["gaswater_product_index"])
    basic_dict = utils.get_basic_parmas(DBSTR, well_id)
    if basic_dict["error"] == 1:
        return basic_dict

    print("basic sucess")

    pvt_gas_dict = utils.get_gas_pvt(DBSTR, well_id)
    if pvt_gas_dict["error"] == 1:
        return pvt_gas_dict

    print("pvt_gas sucess")

    pvt_water_dict = utils.get_water_pvt(DBSTR, well_id)
    if pvt_water_dict["error"] == 1:
        return pvt_water_dict

    print("pvt_water sucess")

    phase_seepage_dict = utils.get_phase_seepage(DBSTR, well_id)
    if phase_seepage_dict["error"] == 1:
        return phase_seepage_dict

    print("phase_seepage sucess")

    product_dict = get_product_data(DBSTR, product_index)
    if product_dict["error"] == 1:
        return product_dict

    print("product sucess")

    middle_dict, qaof_dict = cal_qaof(basic_dict, pvt_gas_dict, pvt_water_dict, phase_seepage_dict, product_dict)
    if middle_dict["error"] == 1:
        return middle_dict
    if qaof_dict["error"] == 1:
        return qaof_dict

    print("qaof sucess")

    pic_1, pic_2 = draw_pic(sql_dict, product_dict, qaof_dict)
    save_data_1(DBSTR, sql_dict, middle_dict)
    result = save_data_2(DBSTR, sql_dict, product_dict, qaof_dict, pic_1, pic_2)
    return result


if __name__ == '__main__':
    DBSTR = 'mysql+pymysql://root:qwer1234@127.0.0.1/waterwell'
    sql_dict = {
        "DBSTR": DBSTR,
        "well_id": 1577948210,
        "gaswater_product_index": 1577951606,
        "middle_index": int(time.time()),
        "gaswater_result_index": int(time.time())+1,
    }
    print(main(sql_dict))

    # 1571051056 MX11
    # 1571055038 MX8
    # 1571053703 初始
    # 1571191943 正确
    # 1571195852 错误
    # 1571283302 少量数据
    # 1571274624 有水
    # 1571297028 少量新数据
    # 1571298392 五倍水
    # 1571476951 无水
