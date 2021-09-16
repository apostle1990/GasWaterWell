import json
import time
import logging
from flask import Flask, request
from ast import literal_eval
import matplotlib
import matplotlib.pyplot as plt

from gas_water_two import cal_gaswater
import gas
import gas_water
import production_well
import pressure_square


matplotlib.use('Agg')
plt.rcParams['font.sans-serif'] = ['SimHei']  # 用来正常显示中文标签
plt.rcParams['axes.unicode_minus'] = False  # 用来正常显示负号

app = Flask(__name__)


@app.errorhandler(500)
def internal_server_error(e):
    app.logger.exception('error 500: %s', e)
    return '捕捉异常：%s' % e


@app.route('/')
def hello_world():
    return 'Hello, World!'


@app.route('/prcalc', methods=['POST'])
def fprcalc() -> dict:
    result = {}
    if len(request.form) != 3:
        logging.exception(
            "Invalid request length! Should 3 but ", len(request.form))
    try:
        pr = float(request.form.get('pr'))
        q = literal_eval(request.form.get('q'))
        pwf = literal_eval(request.form.get('pwf'))
    except TypeError:
        logging.exception("Invalid Parameters!")
    try:
        result = production_well.prcalc(pr, q, pwf)
    except Exception as e:
        logging.exception(e)
    return json.dumps(result)


@app.route('/iprcalc', methods=['POST'])
def fiprcalc() -> dict:
    result = {}
    if len(request.form) != 3:
        logging.exception(
            "Invalid request length! Should 3 but ", len(request.form))

    try:
        a = float(request.form.get('a'))
        b = float(request.form.get('b'))
        pr = float(request.form.get('pr'))
    except TypeError:
        logging.exception("Invalid Parameters!")
    try:
        result = production_well.iprcalc(a, b, pr, True)
    except Exception as e:
        logging.exception(e)

    return json.dumps(result)


@app.route('/predict', methods=['POST'])
def fpredict() -> dict:
    result = {}
    if len(request.form) != 4:
        logging.exception(
            "Invalid request length! Should 4 but ", len(request.form))

    try:
        pi = float(request.form.get('pi'))
        a = float(request.form.get('a'))
        b = float(request.form.get('b'))
        Qg = float(request.form.get('Qg'))
        gama = float(request.form.get('gama'))
    except TypeError:
        logging.exception("Invalid Parameters!")

    try:
        result = production_well.predict(pi, a, b, Qg, gama)
    except Exception as e:
        logging.exception(e)

    return json.dumps(result)


@app.route('/gaswater', methods=['POST'])
def fgaswater() -> dict:
    result = {}
    if len(request.form) != 15:
        logging.exception(
            "Invalid request length! Should 15 but ", len(request.form))

    try:
        user = request.form.get('user')
        pwd = request.form.get('pwd')
        ip = request.form.get('ip')
        dbname = request.form.get('dbname')
        rhowsc = float(request.form.get('rhowsc'))
        rhogsc = float(request.form.get('rhogsc'))
        S = float(request.form.get('s'))
        D = float(request.form.get('d'))
        Re = float(request.form.get('re'))
        rw = float(request.form.get('rw'))
        Pi = float(request.form.get('pi'))
        pressure_fluid_index = request.form.get('pressure_fluid_index')
        gaswater_index = request.form.get('gaswater_index')
        product_index = request.form.get('product_index')
        gaswater_input_id = request.form.get('gaswater_input_id')
    except TypeError:
        logging.exception("Invalid Parameters!")

    gaswater_result_index = int(time.time())

    DBSTR = 'mysql+pymysql://%s:%s@%s/%s' % (user, pwd, ip, dbname)
    # print(DBSTR)
    params = (rhowsc, rhogsc, S, D, Re, rw, Pi)
    sql_dict = {
        "pressure_fluid_index": pressure_fluid_index,
        "gaswater_index": gaswater_index,
        "product_index": product_index,
        "gaswater_input_id": gaswater_input_id,
        "gaswater_result_index": gaswater_result_index,
    }
    try:
        result = cal_gaswater(DBSTR, params, sql_dict)
    except Exception as e:
        logging.exception(e)
    if "error" not in result:
        result["error"] = 1
    if result['error'] == 1:
        return json.dumps(result)

    result["gaswater_result_index"] = gaswater_result_index

    print(result)

    return json.dumps(result)


@app.route('/gas_ps', methods=['POST'])
def fgasps() -> dict:
    result = {}
    if len(request.form) != 6:
        logging.exception(
            "Invalid request length! Should 6 but ", len(request.form))

    try:
        user = request.form.get('user')
        pwd = request.form.get('pwd')
        ip = request.form.get('ip')
        dbname = request.form.get('dbname')
        gas_product_index = request.form.get('gas_product_index')
        well_id = request.form.get('well_id')
    except TypeError:
        logging.exception("Invalid Parameters!")

    gas_result_index = int(time.time())

    DBSTR = 'mysql+pymysql://%s:%s@%s/%s' % (user, pwd, ip, dbname)
    # print(DBSTR)
    sql_dict = {
        "DBSTR": DBSTR,
        "gas_product_index": gas_product_index,
        "well_id": well_id,
        "gas_result_index": gas_result_index,
    }
    try:
        result = pressure_square.main(sql_dict)
    except Exception as e:
        logging.exception(e)
    if "error" not in result:
        result["error"] = 1
    if result['error'] == 1:
        return json.dumps(result)

    result["gas_result_index"] = gas_result_index

    print(result)

    return json.dumps(result)


@app.route('/gas_pp', methods=['POST'])
def fgaspp() -> dict:
    result = {}
    if len(request.form) != 6:
        logging.exception(
            "Invalid request length! Should 6 but ", len(request.form))

    try:
        user = request.form.get('user')
        pwd = request.form.get('pwd')
        ip = request.form.get('ip')
        dbname = request.form.get('dbname')
        gas_product_index = request.form.get('gas_product_index')
        well_id = request.form.get('well_id')
    except TypeError:
        logging.exception("Invalid Parameters!")

    gas_result_index = int(time.time())

    DBSTR = 'mysql+pymysql://%s:%s@%s/%s' % (user, pwd, ip, dbname)
    # print(DBSTR)
    sql_dict = {
        "DBSTR": DBSTR,
        "gas_product_index": gas_product_index,
        "well_id": well_id,
        "gas_result_index": gas_result_index,
    }
    try:
        result = gas.main(sql_dict)
    except Exception as e:
        logging.exception(e)
    if "error" not in result:
        result["error"] = 1
    if result['error'] == 1:
        return json.dumps(result)

    result["gas_result_index"] = gas_result_index

    print(result)

    return json.dumps(result)


@app.route('/gaswater_new', methods=['POST'])
def fgaswater_new() -> dict:
    result = {}
    if len(request.form) != 7:
        logging.exception(
            "Invalid request length! Should 6 but ", len(request.form))

    try:
        user = request.form.get('user')
        pwd = request.form.get('pwd')
        ip = request.form.get('ip')
        dbname = request.form.get('dbname')
        middle_index = request.form.get('middle_index')
        gaswater_product_index = request.form.get('gaswater_product_index')
        well_id = request.form.get('well_id')
    except TypeError:
        logging.exception("Invalid Parameters!")

    gaswater_result_index = int(gaswater_product_index) + 1

    DBSTR = 'mysql+pymysql://%s:%s@%s/%s' % (user, pwd, ip, dbname)
    # print(DBSTR)
    sql_dict = {
        "DBSTR": DBSTR,
        "gaswater_product_index": gaswater_product_index,
        "middle_index": middle_index,
        "well_id": well_id,
        "gaswater_result_index": gaswater_result_index,
    }
    try:
        result = gas_water.main(sql_dict)
    except Exception as e:
        logging.exception(e)
    if "error" not in result:
        result["error"] = 1
    if result['error'] == 1:
        return json.dumps(result)

    result["gaswater_result_index"] = gaswater_result_index

    print(result)

    return json.dumps(result)


if __name__ == "__main__":
    handler = logging.FileHandler('./log/server.log', encoding='UTF-8')
    handler.setLevel(logging.DEBUG)
    logging_format = logging.Formatter(
        '%(asctime)s - %(levelname)s - %(filename)s - %(funcName)s - %(lineno)s - %(message)s')
    handler.setFormatter(logging_format)
    app.logger.addHandler(handler)
    app.run(debug=True)
