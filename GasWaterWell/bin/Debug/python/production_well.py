import time
import numpy as np
import matplotlib.pyplot as plt
from sklearn import linear_model


def cal_r2(y: list, y_pred: list) -> float:
    SSres = sum(map(lambda x: (x[0]-x[1])**2, zip(y, y_pred)))
    SStot = sum([(x - np.mean(y))**2 for x in y])
    return 1-(SSres/SStot)

# 无水pr平方计算法


def prcalc(pr: float, q: list, pwf: list) -> dict:
    '''
    输入：pr值（底层压力，prr值（存储中部，q值（试采，pwf值（井底流压, fig_name图片名称
    '''
    # TODO:判断入口值合法
    if len(q) == 0:
        return {"error": 1}

    if len(pwf) == 0:
        return {"error": 1}

    if len(q) != len(pwf):
        return {"error": 1}

    q = np.array(q)
    q[np.isnan(q)] = 0

    q_raw = q.copy()

    pwf = np.array(pwf)
    pwf[np.isnan(pwf)] = 0

    # q = q[q_raw != 0]
    # pwf = pwf[q_raw != 0]

    theta_p = pr - pwf
    pd_qg = (pr*pr - pwf**2)/(q+1e-8)

    print("q: ", q.shape)
    print("pd_qg: ", pd_qg.shape)

    regr = linear_model.LinearRegression()
    regr.fit(q.reshape(-1, 1), pd_qg.reshape(-1, 1))

    a, b = regr.intercept_, regr.coef_
    label = 'y = ' + str(round(b[0][0], 5)) + 'x %+f' % (round(a[0], 5))
    # 计算无阻流量
    q_aof = (np.sqrt(a*a+4*b*(pr*pr-0.101*0.101))-a)/2/b

    # 计算r2
    score_r2 = cal_r2(pd_qg, regr.predict(q.reshape(-1, 1)))
    pred_y = regr.predict(q.reshape(-1, 1))
    fig = plt.figure(0)
    pic_name = "fitting_%d.png" % time.time()

    plt.scatter(q, pd_qg)
    plt.plot(q, pred_y, label=label)
    plt.xlabel("Qg(10^4 m^3/d)")
    plt.ylabel("(Pr2-Pwf2)/Qg(MPa2/104m3/d)")
    plt.legend()
    plt.savefig("../pic/"+pic_name, dpi=200)
    plt.close(0)

    result = {
        "error": 0,
        "label": label,
        "a": str(a[0]),
        "b": str(b[0][0]),
        "q_aof": str(q_aof[0][0]),
        "r2": str(score_r2[0]),
        "pd_qg": str(pd_qg.tolist()),
        "pred_y": str(pred_y.tolist()),
        "pic_name": pic_name,
    }
    return result

# 计算IPR曲线


def iprcalc(a: float, b: float, pr: float, is_save: bool, fig=None) -> dict:

    Qg = []
    pwf = []

    for i in range(1, 25):
        if((0.05*i) <= 0.95):
            pwf.append((0.05*i)*pr)
        else:
            pwf.append((0.95+(i-19)*0.01)*pr)
    pwf.insert(0, 0.101)
    pwf = np.array(pwf)

    Qg = (np.sqrt(a*a+4*b*(pr*pr-pwf*pwf))-a)/2/b

    print("Qg: ", Qg.shape)
    print("pwf: ", pwf.shape)

    pic_name = "ipr_%d.png" % time.time()

    if(fig == None):
        fig = plt.figure(0)
    plt.plot(Qg, pwf, label="IPR曲线")
    plt.xlabel("Qg(104m3/d)")
    plt.ylabel('Pwf(MPa)')
    plt.legend()
#     plt.show()

    p1 = pr*pr - pwf*pwf
    p2 = pr - pwf
    p3 = p2 / Qg

    plt.plot(Qg, p2)
    plt.scatter(Qg, p2)
    plt.xlabel("Qg(104m3/d)")
    plt.ylabel('thetaP(Mpa)')

    # TODO:差一条直线。不知道怎么画
    if is_save:
        plt.savefig("../pic/"+pic_name, dpi=200)
        plt.close(0)

    result = {
        "error": 0,
        "pwf": str(pwf.tolist()),
        "Qg": str(Qg.tolist()),
        "pic_name": pic_name,
    }

    return result


def predict(pi: float, a1: float, b1: float, Qg: float, gama: float) -> dict:
    pi_list = []
    pii = pi // 10 * 10
    for i in range(1, 8):
        pi_list.append(pii)
        pii = pii - 5
    pi_list = np.array(pi_list)
    theta_p = pi - pi_list


    k1 = 0.5341 * np.exp(0.0078 * pi)
    # k_ki = 0.5341 * np.exp(0.0078 * pi_list) / (0.5341 * np.exp(0.0078 * pi))
    k_ki = np.exp(-gama*theta_p)

    miu1 = -0.00000131*pi*pi+0.000429*pi+0.01
    miu_p = -0.00000131*pi_list*pi_list+0.000429*pi_list+0.01

    z1 = 0.00002144*(pi*pi)+0.0071*pi+0.78365
    z_p = 0.00002144*(pi_list*pi_list)+0.0071*pi_list+0.78365

    ai = a1*miu_p*z_p/(k_ki*miu1*z1)
    bi = b1*miu_p*z_p/(k_ki*miu1*z1)

    Qaof1 = (np.sqrt(a1*a1+4*b1*(pi*pi-0.101*0.101))-a1)/2/b1
    Qaof = (np.sqrt(ai*ai+4*bi*(pi_list*pi_list-0.101*0.101))-ai)/2/bi

    Pwf1 = np.sqrt(pi*pi-a1*Qg-b1*Qg*Qg)
    Pwf = np.sqrt(pi_list*pi_list-ai*Qg-bi*Qg*Qg)

    theta1 = pi - Pwf1
    theta = pi_list - Pwf

    # 绘制Qaof图像
    regr = linear_model.LinearRegression()
    regr.fit(pi_list.reshape(-1, 1), Qaof.reshape(-1, 1))
    aa, bb = regr.intercept_, regr.coef_
    label = 'y='+str(round(bb[0][0], 5))+'x+'+str(round(aa[0], 5))
    fig = plt.figure(1)
    plt.plot(pi_list, regr.predict(pi_list.reshape(-1, 1)), label=label)
    plt.scatter(pi_list, Qaof)
    plt.title('Qaof')
    plt.legend()
    # 计算R2值
    score_r2 = cal_r2(Qaof, regr.predict(pi_list.reshape(-1, 1)))
    pic_name_1 = "predict_1_%d.png" % time.time()
    plt.savefig("../pic/"+pic_name_1, dpi=200)
    plt.close(1)

    pic_name_2 = "predict_2_%d.png" % time.time()

    result = {
        "error": 0,
        "r2": str(score_r2[0]),
        "pi": pi_list.tolist(),
        "theat_p": theta_p.tolist(),
        "k_ki": k_ki.tolist(),
        "miu_p": miu_p.tolist(),
        "z_p": z_p.tolist(),
        "ai": ai.tolist(),
        "bi": bi.tolist(),
        "Qaof": Qaof.tolist(),
        "theta": theta.tolist(),
        "pic_name_1": pic_name_1,
        "pic_name_2": pic_name_2,
    }

    fig = plt.figure(0)
    for i in range(len(pi_list)):
        _ = iprcalc(a=ai[i], b=bi[i], pr=pi_list[i], is_save=False, fig=fig)

    plt.savefig("../pic/"+pic_name_2, dpi=200)
    plt.close('all')

    return result


if __name__ == '__main__':
    Qg = []
    Pwf = []
    with open("F:\\Project\\waterwell\\导入数据\\压力平方法导入数据\\test_data2.csv", encoding="utf-8") as f:
        f.readline()
        for idx, line in enumerate(f):
            q, p = line.strip().split(",")
            Qg.append(float(q))
            Pwf.append(float(p))

    pr = 74
    print(len(Qg))
    print(len(Pwf))
    result = prcalc(pr, Qg, Pwf)
    # iprcalc(float(result["a"]), float(result["b"]), pr, is_save=False)
    # print(predict(pr, float(result["a"]), -float(result["b"]), 50, 0.011))
    # print(prcalc(74, Qg, Pwf))