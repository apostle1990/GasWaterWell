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

import warnings

warnings.filterwarnings("ignore")


def cal_gaswater(DBSTR, params, sql_dict):
    result = {
        "error": 1,
    }
    # try:n

    engine = create_engine(DBSTR)
    rhowsc, rhogsc, S, D, Re, rw, Pi = params
    sql = "select * from gaswater_pressure_fluid where \
    pressure_fluid_index = {pressure_fluid_index} and gaswater_input_id = {gaswater_input_id};".format(**sql_dict)
    # print(sql)
    Pro_table = pd.read_sql_query(sql, engine)  # 压力与流体性质关系
    # 后面未提供，导致压力插值越界
    # Pro_table = pd.read_excel("E:\\Project\\GasWaterWell\\导入数据\\水气两相法导入数据\\压力与流体性质关系.xlsx", names=["pressure"])
    if Pro_table.empty:
        return result

    sql = "select * from gaswater_phase_seepage where \
    gaswater_index = {gaswater_index} and gaswater_input_id = {gaswater_input_id};".format(**sql_dict)
    Kr_table = pd.read_sql_query(sql, engine)  # 气水相渗
    # Kr_table = pd.read_excel("E:\\Project\\GasWaterWell\\导入数据\\水气两相法导入数据\\气水相渗.xlsx")
    if Kr_table.empty:
        return result

    sql = "select * from gaswater_production_data where \
    product_index = {product_index} and gaswater_input_id = {gaswater_input_id};".format(**sql_dict)
    PrData_table = pd.read_sql_query(sql, engine)  # 生产数据
    # PrData_table = pd.read_csv("E:\\Project\\GasWaterWell\\导入数据\\水气两相法导入数据\\生产数据11.csv",names=["date","gas_production","water_production","bottom_hole_pressure","formation_pressure","cumulative_gas_production"])
    if PrData_table.empty:
        return result
    # PrData_table = pd.read_excel("E:\Project\GasWaterWell\导入数据\水气两相法导入数据\生产数据1.xlsx", names=["date","gas_production","water_production","bottom_hole_pressure","formation_pressure","cumulative_gas_production"])

    # 数据预处理
    PrData_table.gas_production = PrData_table.gas_production.replace(
        "", 0).astype(float)
    PrData_table.water_production = PrData_table.water_production.replace(
        "", 0).astype(float)
    PrData_table.formation_pressure = PrData_table.formation_pressure.replace(
        "", 0).astype(float)

    PrData_table.bottom_hole_pressure = PrData_table.bottom_hole_pressure.astype(
        float)
    PrData_table.formation_pressure = PrData_table.formation_pressure.astype(
        float)

    # 删除井底流压为0的数据
    # PrData_table = PrData_table[PrData_table.bottom_hole_pressure != 0]
    # PrData_table = PrData_table[PrData_table.formation_pressure != 0]
    # if (PrData_table.bottom_hole_pressure == 0).sum() > 0:
    #     result["error"] = 2
    #     return result
    # if (PrData_table.formation_pressure == 0).sum() > 0:
    #     result["error"] = 2
    #     return result

    # 获取表格中的行数
    Pro_n = len(Pro_table) + 1
    Kr_n = len(Kr_table) + 1
    PrData_n = len(PrData_table) + 1

    # matlab中是以列举证来存储，这里采用了行矩阵
    P = np.insert(Pro_table.pressure.values, 0, 0)  # 压力
    rhog = np.insert(Pro_table.gas_density.values, 0, 0)  # 气体密度
    miug = np.insert(Pro_table.gas_viscosity.values, 0, 0)  # 气体粘度
    rhow = np.insert(Pro_table.water_density.values, 0, 0)  # 水密度
    miuw = np.insert(Pro_table.water_viscosity.values, 0, 0)  # 水粘度
    Bg = np.insert(Pro_table.gas_volume_factor.values, 0, 0)  # 气体体积系数
    Bw = np.insert(Pro_table.water_volume_factor.values, 0, 0)  # 水的体积系数

    Sw = np.insert(Kr_table.sw.values, 0, 0)  # 水的相渗
    Krg = np.insert(Kr_table.krg.values, 0, 0)  # 气体相对密度
    Krw = np.insert(Kr_table.krw.values, 0, 0)  # 水的相对密度

    Data_time = np.insert(PrData_table.date.values, 0, 0)
    Qgr = np.insert(PrData_table.gas_production.values, 0, 0)  # 实际气井产量
    Qwr = np.insert(PrData_table.water_production.values, 0, 0)  # 实际产水量

    Pwf = np.insert(PrData_table.bottom_hole_pressure.values, 0, 0)  # 井底流压
    Pe = np.insert(PrData_table.formation_pressure.values, 0, 0)  # 地层压力
    # Gp = np.insert(
    #     PrData_table.cumulative_gas_production.values, 0, 0)  # 累积产气量

    QWGR = np.zeros(PrData_n)  # QWGR为实际水气比，每万方
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
    Rwg = np.arange(-0.1*1e-4, maxRwg, 0.1*1e-4)
    Rwg[0] = 0

    cn = len(Rwg)

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
    # print(PSTa_nowater)

    # ————————计算累产气——————————%
    # 根据实际生产数据，基于得到的拟压力函数表，插值获得一系列的拟压力值，并计算动态无阻流量
    # ——————————计算A、B系数值—————————————%
    A1 = 1.842*(rhogsc+rhowsc*Qwgr.T)*(np.log(Re/rw)-0.75+S)
    B1 = 1.842*(rhogsc+rhowsc*Qwgr.T)**2*D
    AG1 = 1.842*rhogsc*(np.log(Re/rw)-0.75+S)
    BG1 = 1.842*rhogsc**2*D


    KH = np.zeros(n)
    KH_nowater = np.zeros(n)
    PeS = np.zeros(n)  # 每一天的地层压力对应的拟压力
    PwfS = np.zeros(n)  # 每一天的井底流压对应的拟压力
    PeGS = np.zeros(n)  # 不考虑产水每一天的地层拟压力
    PwfGS = np.zeros(n)  # 不考虑产水每一天的井底流压拟压力
    # KHC = 0.0  # 不考虑产水，KH恒定,取前30天的平均KH值
    # KHC_count = 0  # 取30次
    # KHC_sum = 0.0
    # print("QWGR: max:%f, min:%f" % (max(QWGR), min(QWGR)))
    # print(Rwg.shape)
    # print(PSTa)

    for k in range(1, n):
        PS = np.zeros(rn)
        for i in range(1, rn):
            Rwg_temp = Rwg[0][1:]
            PSTa_temp = PSTa[i][1:]  # .tolist()
            interp1_3 = interp1d(Rwg_temp, PSTa_temp)
            PS[i] = interp1_3(Qwgr[k])  # QWGR为实际水气比，每万方， 函数拟合有问题
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

        PwfS[k] = interp1_4(Pwf[k])  # 井底流压

        # 无水计算
        PSTa_temp = PSTa_nowater[:, 1][1:]  # 取第一列并将第一个数在拟合的过程中去掉
        interp1_5 = interp1d(P_temp, PSTa_temp)

        PeGS[k] = interp1_5(Pe[k])

        PwfGS[k] = interp1_5(Pwf[k])

        # PeGS[k] = Pe[k]
        KH[k] = (A1[k]*Qgr[k]+B1[k]*Qgr[k]**2) / \
            (PeS[k]-PwfS[k])  # 确定地层系数,mD,m

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

    print("QgwAOF: ", QgwAOF)
    # print(AG, BG)
    print("QgAOF: ", QgAOF)

    where_are_inf = np.isinf(A)
    A[where_are_inf] = None
    where_are_inf = np.isinf(B)
    B[where_are_inf] = None

    # 开始画图

    pic_name_1 = "gaswater_1_%d.png" % int(sql_dict["gaswater_result_index"])
    pic_name_2 = "gaswater_2_%d.png" % int(sql_dict["gaswater_result_index"])

    plt.figure(0)
    plt.plot(Qgr[1:]/100, Pwf[1:]/100, 'o', label='Pwf')
    plt.plot(Qgr[1:]/100, PwfS[1:]/100, 'o', label='PwfS')
    plt.grid(True)
    plt.legend(loc=0)
    plt.xlabel(r'$Qgr(10^6m^3/d)$')
    # plt.xlabel(r'$Qgr(104m^3/d)$')
    plt.ylabel(r'$Pwf(MPa)$')
    plt.title('Relationship between Qgr and Pwf')
    plt.savefig("./pic/"+pic_name_1, dpi=200)
    plt.close(0)

    plt.figure(1)
    plt.plot(np.arange(1, n), QgwAOF[1:]/100, 'o', mfc='none', label='QgwAOF')
    plt.plot(np.arange(1, n), QgAOF[1:]/100, label='QgAOF')
    plt.grid(True)
    plt.legend(loc=0)
    plt.xlabel(r'$time$')
    plt.ylabel(r'$Qg(10^6m^3/d)$')
    # plt.ylabel(r'$Qg(104m^3)$')
    plt.title('Relationship between time and QgAOF')
    plt.savefig("./pic/"+pic_name_2, dpi=200)
    plt.close(1)

    # 保存到数据库

    title = ["date", "unimpeded_flow_water",
             "unimpeded_flow_no_water", "kh", "a", "b"]
    # print(QgAOF)
    result_data = [Data_time, QgwAOF, QgAOF, KH, A, B]

    result_df = pd.DataFrame({items[0]: items[1]
                              for items in zip(title, result_data)}).drop(0)

    result_df["gaswater_result_index"] = sql_dict["gaswater_result_index"]
    result_df["gaswater_input_id"] = sql_dict["gaswater_input_id"]
    result_df["pic_path_1"] = os.path.dirname(os.path.dirname(
        os.path.abspath(__file__))) + "\\pic\\" + pic_name_1
    result_df["pic_path_2"] = os.path.dirname(os.path.dirname(
        os.path.abspath(__file__))) + "\\pic\\" + pic_name_2

    sql = "select * from gaswater_result where \
    gaswater_result_index = {gaswater_result_index} and gaswater_input_id = {gaswater_input_id};".format(**sql_dict)
    result_table = pd.read_sql_query(sql, engine)  # 生产数据
    if not result_table.empty:
        delete_sql = "delete gaswater_result where \
        gaswater_result_index = {gaswater_result_index} and gaswater_input_id = {gaswater_input_id};".format(**sql_dict)
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
    print(result)
    return result


if __name__ == '__main__':
    DBSTR = 'mysql+pymysql://root:qwer1234@127.0.0.1/waterwell'
    rhowsc = 1000  # 水的密度,kg/m3
    rhogsc = 0.7048  # 气体密度,kg/m3
    # S = -6.5  # 气井表皮系数
    S = 22
    # D = 4.1E-6  # 非达西渗流系数
    D = 0.51/10000
    Re = 1500  # 井控半径
    rw = 0.1  # 井径
    Pi = 76.05  # 原始地层压力，MPa
    # Pi = 100
    params = (rhowsc, rhogsc, S, D, Re, rw, Pi)
    sql_dict = {
        "pressure_fluid_index": 1571476952,
        "gaswater_index": 1571476953,
        "product_index": 1571476954,
        "gaswater_input_id": 1571476951,
        "gaswater_result_index": time.time(),
    }
    cal_gaswater(DBSTR, params, sql_dict)

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
