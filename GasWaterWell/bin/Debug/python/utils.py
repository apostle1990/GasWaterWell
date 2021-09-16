import os
import time
import numpy as np
import pandas as pd
from sqlalchemy import create_engine


def get_basic_parmas(DBSTR, well_id):
    result = {
        "error": 1
    }

    engine = create_engine(DBSTR)
    sql = "select * from well_params where well_id = %d;" % well_id
    well_params_df = pd.read_sql_query(sql, engine)
    if well_params_df.empty:
        return result

    result["error"] = 0
    result["re"] = well_params_df.re.values[0]  # 井控半径
    result["rw"] = well_params_df.rw.values[0]  # 井筒半径
    result["D"] = well_params_df.d.values[0]  # 非达西流系数 1/10^4m^3/d
    result["S"] = well_params_df.s.values[0]  # 表皮系数
    result["rhogsc"] = well_params_df.rhogsc.values[0]  # 气体密度,kg/m3
    result["rhowsc"] = well_params_df.rhowsc.values[0]  # 气体密度,kg/m3
    return result


def get_phase_seepage(DBSTR, well_id):
    result = {
        "error": 1
    }

    engine = create_engine(DBSTR)
    sql = "select * from well where well_id = %d;" % well_id
    well_df = pd.read_sql_query(sql, engine)
    if well_df.empty:
        return result

    phase_seepage_name = well_df.phase_seepage_name.values[0]
    sql = "select * from phase_seepage where phase_seepage_name = '%s';" % phase_seepage_name
    phase_seepage_df = pd.read_sql_query(sql, engine)
    if phase_seepage_df.empty:
        return result

    result["error"] = 0
    result["sw"] = phase_seepage_df.sw.values
    result["krg"] = phase_seepage_df.krg.values
    result["krw"] = phase_seepage_df.krw.values
    return result


def get_gas_pvt(DBSTR, well_id):
    result = {
        "error": 1,
    }

    engine = create_engine(DBSTR)
    sql = "select * from well where well_id = %d;" % well_id
    well_df = pd.read_sql_query(sql, engine)
    if well_df.empty:
        return result

    pvt_gas_name = well_df.pvt_gas_name.values[0]
    sql = "select * from pvt_gas where gas_name = '%s';" % pvt_gas_name
    pvt_gas_df = pd.read_sql_query(sql, engine)
    if pvt_gas_df.empty:
        return result

    result["error"] = 0
    result["yg"] = pvt_gas_df.yg.values[0]  # 天然气相对密度
    result["yco2"] = pvt_gas_df.YCO2.values[0]  # CO2摩尔分数
    result["yh2s"] = pvt_gas_df.YH2S.values[0]  # H2S摩尔分数
    result["t"] = pvt_gas_df["T"].values[0] + 273.15  # 气藏温度, K
    result["p"] = pvt_gas_df.P.values / 1E+6  # 气藏压力
    result["nacl"] = pvt_gas_df.NaCl.values[0]  # 地层水矿化度
    # result["z1"] = pvt_gas_df.NaCl.values  # 天然气偏差系数，无因次（DP方法）
    result["z"] = pvt_gas_df.z2.values  # 天然气偏差系数，无因次（DPR方法）
    result["bg"] = pvt_gas_df.Bg.values  # 天然气体积系数
    result["pg"] = pvt_gas_df.pg.values  # 天然气密度
    result["ug"] = pvt_gas_df.ug.values  # 天然气粘度
    result["cg"] = pvt_gas_df.Cg.values  # 天然气压缩系数
    result["pp"] = pvt_gas_df.Pp.values  # 天然气拟压力
    result["rcwg"] = pvt_gas_df.Rcwg.values  # 凝析水气比
    return result


def get_water_pvt(DBSTR, well_id):
    result = {
        "error": 1,
    }

    engine = create_engine(DBSTR)
    sql = "select * from well where well_id = %d;" % well_id
    well_df = pd.read_sql_query(sql, engine)
    if well_df.empty:
        return result

    pvt_water_name = well_df.pvt_water_name.values[0]
    sql = "select * from pvt_water where water_name = '%s';" % pvt_water_name
    pvt_water_df = pd.read_sql_query(sql, engine)
    if pvt_water_df.empty:
        return result

    result["error"] = 0
    result["t"] = pvt_water_df["K"].values[0] + 273.15  # 气藏温度, K
    result["p"] = pvt_water_df.Pa.values / 1E+6  # 地层压力
    result["nacl"] = pvt_water_df.NaCl.values[0]  # 地层水矿化度
    result["bw"] = pvt_water_df.Bw.values  # 地层水体积系数
    result["pw"] = pvt_water_df.pw.values  # 地层水密度
    result["uw"] = pvt_water_df.uw.values * 10*3  # 地层水粘度
    result["cw"] = pvt_water_df.Cw.values  # 地层水压缩系数
    result["rsw"] = pvt_water_df.Rsw.values  # 天然气在水中的溶解度
    result["rcwg"] = pvt_water_df.Rcwg.values  # 凝析水气比
    return result


def get_pvt_params():
    result = {
        "error": 1,
        "p": [],  # 压力
        "z": [],  # 偏差系数
        "bg": [],  # 天然气体积系数/MPa
        "ug": [],  # 天然气粘度/MPa
        "pg": [],  # 天然气密度/MPa
        "bw": [],  # 水体积系数/MPa
        "uw": [],  # 水粘度/MPa
        "pw": [],  # 水密度/MPa
    }
    Pvt_table = pd.read_csv("F:\\Project\\waterwell\\导入数据\\水气两相法导入数据\\压力与流体性质关系.csv",
                            # names=["p", "z", "bg", "ug", "pg", "bw", "uw", "pw"],
                            names=["p", "pg", "ug", "pw", "uw", "bg", "bw"],
                            header=0)

    if Pvt_table.empty:
        return result

    result["error"] = 0
    result["p"] = Pvt_table.p.values
    # result["z"] = Pvt_table.z.values
    result["pg"] = Pvt_table.pg.values
    result["ug"] = Pvt_table.ug.values
    result["t"] = 140 + 273.15  # 气藏温度, K
    return result
