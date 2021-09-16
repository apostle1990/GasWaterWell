using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GasWaterWell
{
    class PvtGas
    {
        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算天然气拟临界温度，K
        // 参数说明：IsDryGas：天然气类型，true为干气，false为凝析气
        // GasSpecificGravity：天然气比重，无量纲
        // CO2MoleFraction：天然气中CO2的摩尔含量，小数
        // H2SMoleFraction：天然气中H2S的摩尔含量，小数
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double GASPVTTPC(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction)
        {
            double Rg;
            double YCO2;
            // 替换变量名称
            double YH2S;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction;

            // 执行相应计算
            // Standing方法，1981
            double Tpc0;
            if (IsDryGas == true)
                Tpc0 = 93.3333 + 180.5556 * Rg - 6.9444 * Math.Pow(Rg, 2);
            else
                Tpc0 = 103.8889 + 183.333 * Rg - 39.72222 * Math.Pow(Rg, 2);
            // Wichert-Aziz方法，1970
            // 王家伟：没有看到公式，实际为修正
            double Tpc1;
            Tpc1 = (Tpc0 - 66.67 * (Math.Pow((YCO2 + YH2S), 0.9) - Math.Pow((YCO2 + YH2S), 1.6)) + 8.33 * (Math.Pow(YH2S, 0.5) - Math.Pow(YH2S, 4)));

            // 计算结果输出
            return Tpc1;
        }
        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算天然气拟临界压力，Pa
        // 参数说明：IsDryGas：天然气类型，true为干气，false为凝析气
        // GasSpecificGravity：天然气比重，无量纲
        // CO2MoleFraction：天然气中CO2的摩尔含量，小数
        // H2SMoleFraction：天然气中H2S的摩尔含量，小数
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double GASPVTPPC(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction)
        {
            double Rg;
            double YCO2;
            // 替换变量名称
            double YH2S;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction;

            // 执行相应计算
            // Standing方法，1981
            double Ppc0;
            if (IsDryGas == true)
                Ppc0 = (4.6677 + 0.1034 * Rg - 0.2586 * Math.Pow(Rg, 2)) * Math.Pow(10, 6);
            else
                Ppc0 = (4.8677 - 0.3565 * Rg - 0.07653 * Math.Pow(Rg, 2)) * Math.Pow(10, 6);
            double Tpc0;
            // Wichert-Aziz方法，1970
            // 王家伟：没有看到公式，实际为修正
            double Tpc1;
            Tpc0 = GASPVTTPC(IsDryGas, Rg, 0, 0);
            Tpc1 = GASPVTTPC(IsDryGas, Rg, YCO2, YH2S);
            double Ppc1;
            Ppc1 = (Ppc0 * Tpc1) / (Tpc0 + (66.67 * (Math.Pow((YCO2 + YH2S), 0.9) - Math.Pow((YCO2 + YH2S), 1.6)) + 8.33 * (Math.Pow(YH2S, 0.5) - Math.Pow(YH2S, 4))) * YH2S * (1 - YH2S));
            // 计算结果输出
            return Ppc1;
        }
        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算天然气拟对比温度，K/K
        // 参数说明：IsDryGas：天然气类型，true为干气，false为凝析气
        // GasSpecificGravity：天然气比重，无量纲
        // CO2MoleFraction：天然气中CO2的摩尔含量，小数
        // H2SMoleFraction：天然气中H2S的摩尔含量，小数
        // Temperature：天然气体系温度，K
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double GASPVTTPR(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction, double Temperature)
        {
            double Rg;
            double YCO2;
            double YH2S;
            // 替换变量名称
            double T;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction; T = Temperature;

            // 执行相应计算
            double Tpc;
            Tpc = GASPVTTPC(IsDryGas, Rg, YCO2, YH2S);
            double Tpr;
            Tpr = T / Tpc;

            // 计算结果输出
            return Tpr;
        }
        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算天然气拟对比压力，Pa/Pa
        // 参数说明：IsDryGas：天然气类型，true为干气，false为凝析气
        // GasSpecificGravity：天然气比重，无量纲
        // CO2MoleFraction：天然气中CO2的摩尔含量，小数
        // H2SMoleFraction：天然气中H2S的摩尔含量，小数
        // Pressure：天然气体系压力，Pa
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double GASPVTPPR(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction, double Pressure)
        {
            double Rg;
            double YCO2;
            double YH2S;
            // 替换变量名称
            double P;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction; P = Pressure;

            // 执行相应计算
            double Ppr;
            Ppr = P / (GASPVTPPC(IsDryGas, Rg, YCO2, YH2S));

            // 计算结果输出
            return Ppr;
        }
        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算天然气偏差系数，无量纲
        // 参数说明：IsDryGas：天然气类型，true为干气，false为凝析气
        // GasSpecificGravity：天然气比重，无量纲
        // CO2MoleFraction：天然气中CO2的摩尔含量，小数
        // H2SMoleFraction：天然气中H2S的摩尔含量，小数
        // Temperature：天然气体系温度，K
        // Pressure：天然气体系压力，Pa
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double GASPVTZ(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction, double Temperature, double Pressure)
        {
            double Rg;
            double YCO2;
            double YH2S;
            double T;
            // 替换变量名称
            double P;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction; T = Temperature; P = Pressure;
            double Ppr;

            // 执行相应计算
            // Dranchuk&Purvis方法，1974
            double Tpr;
            Tpr = GASPVTTPR(IsDryGas, Rg, YCO2, YH2S, T);
            Ppr = GASPVTPPR(IsDryGas, Rg, YCO2, YH2S, P);
            double A1;
            double A2;
            double A3;
            double A4;
            double A5;
            double A6;
            A1 = 0.3151;
            A2 = 1.0467;
            A3 = 0.5783;
            A4 = 0.5353;
            A5 = 0.6123;
            A6 = 0.6815;
            double Z0;
            double RourR;
            double Z1;
            Z0 = 1;
            RourR = 0.27 * Ppr / (Z0 * Tpr);
            Z1 = 1 + (A1 - A2 / Tpr - A3 / Math.Pow(Tpr, 3)) * RourR + (A4 - A5 / Tpr + A6 / Math.Pow(Tpr, 3)) * Math.Pow(RourR, 2);

            int Iterations;
            Iterations = 0;

            while (Math.Abs(Z1 - Z0) > Math.Pow(10, (-4)) & Iterations < 50)
            {
                Z0 = Z1;
                RourR = 0.27 * Ppr / (Z0 * Tpr);
                Z1 = 1 + (A1 - A2 / Tpr - A3 / Math.Pow(Tpr, 3)) * RourR + (A4 - A5 / Tpr + A6 / Math.Pow(Tpr, 3)) * Math.Pow(RourR, 2);
                Iterations = Iterations + 1;
            }

            // 计算结果输出
            return Z1;
        }

        public double GASPVTTPC(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction, double N2MoleFraction)
        {
            double Rg;
            double YCO2;
            double YH2S;
            // 替换变量名称
            double YN2;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction; YN2 = N2MoleFraction;
            // 执行相应计算
            // Standing方法，1981
            double Tpc0;
            if (IsDryGas == true)
                Tpc0 = 93.3333 + 180.5556 * Rg - 6.9444 * Math.Pow(Rg, 2);
            else
                Tpc0 = 103.8889 + 183.333 * Rg - 39.72222 * Math.Pow(Rg, 2);
            // Wichert-Aziz方法，1970
            double Tpc1;
            // Tpc1 = (Tpc0 - 66.67 * ((YCO2 + YH2S) ^ 0.9 - (YCO2 + YH2S) ^ 1.6) + 8.33 * (YH2S ^ 0.5 - YH2S ^ 4))
            Tpc1 = Tpc0 - 44.4 * YCO2 + 72.2 * YH2S - 138.9 * YN2;
            //GASPVTTPC = Tpc1;
            return Tpc1;
        }


        public double GASPVTTPR(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction, double N2MoleFraction, double Temperature)
        {
            double Rg;
            double YCO2;
            double YH2S;
            double YN2;
            // 替换变量名称
            double T;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction; YN2 = N2MoleFraction; T = Temperature;

            // 执行相应计算
            double Tpc;
            Tpc = GASPVTTPC(IsDryGas, Rg, YCO2, YH2S, YN2);
            double Tpr;
            Tpr = T / Tpc;

            // 计算结果输出
            //GASPVTTPR = Tpr;
            return Tpr;
        }

        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算天然气拟临界压力，Pa
        // 参数说明：IsDryGas：天然气类型，true为干气，false为凝析气
        // GasSpecificGravity：天然气比重，无量纲
        // CO2MoleFraction：天然气中CO2的摩尔含量，小数
        // H2SMoleFraction：天然气中H2S的摩尔含量，小数
        // N2MoleFraction：天然气中N2的摩尔含量，小数
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double GASPVTPPC(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction, double N2MoleFraction)
        {
            double Rg;
            double YCO2;
            double YH2S;
            // 替换变量名称
            double YN2;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction; YN2 = N2MoleFraction;

            // 执行相应计算
            // Standing方法，1981
            double Ppc0;
            if (IsDryGas == true)
                Ppc0 = (4.6677 + 0.1034 * Rg - 0.2586 * Math.Pow(Rg, 2)) * Math.Pow(10, 6);
            else
                Ppc0 = (4.8677 - 0.3565 * Rg - 0.07653 * Math.Pow(Rg, 2)) * Math.Pow(10, 6);
            double Tpc0;
            // Wichert-Aziz方法，1970
            double Tpc1;
            Tpc0 = GASPVTTPC(IsDryGas, Rg, 0, 0, 0);
            Tpc1 = GASPVTTPC(IsDryGas, Rg, YCO2, YH2S, YN2);
            double Ppc1;
            // Ppc1 = (Ppc0 * Tpc1) / (Tpc0 + (66.67 * ((YCO2 + YH2S) ^ 0.9 - (YCO2 + YH2S) ^ 1.6) + 8.33 * (YH2S ^ 0.5 - YH2S ^ 4)) * YH2S * (1 - YH2S))
            Ppc1 = Ppc0 + 3.034 * YCO2 + 4.137 * YCO2 - 1.172 * YN2;
            // 计算结果输出
            //GASPVTPPC = Ppc1;
            return Ppc1;
        }


        public double GASPVTPPR(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction, double N2MoleFraction, double Pressure)
        {
            double Rg;
            double YCO2;
            double YH2S;
            double YN2;
            // 替换变量名称
            double P;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction; YN2 = N2MoleFraction; P = Pressure;

            // 执行相应计算
            double Ppr;
            Ppr = P / (GASPVTPPC(IsDryGas, Rg, YCO2, YH2S, YN2));

            // 计算结果输出
            //GASPVTPPR = Ppr;
            return Ppr;
        }



        public double GASPVTZ(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction, double N2MoleFraction, double Temperature, double Pressure)
        {
            double Rg;
            double YCO2;
            double YH2S;
            double YN2;
            double T;
            double P;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction; YN2 = N2MoleFraction; T = Temperature; P = Pressure;
            double Ppr;
            double Tpr;
            Tpr = GASPVTTPR(IsDryGas, Rg, YCO2, YH2S, YN2, T);
            Ppr = GASPVTPPR(IsDryGas, Rg, YCO2, YH2S, YN2, P);
            double A1;
            double A2;
            double A3;
            double A4;
            double A5;
            double A6;
            double A7;
            double A8;
            A1 = 0.31506237;
            A2 = -1.0467099;
            A3 = -0.57832729;
            A4 = 0.53530771;
            A5 = -0.61232032;
            A6 = -0.10488813;
            A7 = 0.68157001;
            A8 = 0.68446549;
            double Z0;
            double RourR;
            double Z1;
            double Z2;
            double Z3;
            Z0 = 1;
            RourR = 0.27 * Ppr / (Z0 * Tpr);
            do
            {
                Z2 = RourR - 0.27 * Ppr / Tpr + (A1 + A2 / Tpr + A3 / (Math.Pow(Tpr, 3))) * Math.Pow(RourR, 2) + (A4 + A5 / Tpr) * (Math.Pow(RourR, 3)) + (A5 * A6 / Tpr) * (Math.Pow(RourR, 6)) + (A7 * (Math.Pow(RourR, 3)) / Math.Pow(Tpr, 3)) * (1 + A8 * Math.Pow(RourR, 2)) * Math.Exp(-A8 * Math.Pow(RourR, 2));
                Z3 = 1 + (A1 + A2 / Tpr + A3 / (Math.Pow(Tpr, 3))) * 2 * RourR + (A4 + A5 / Tpr) * (3 * Math.Pow(RourR, 2)) + (A5 * A6 / Tpr) * (6 * Math.Pow(RourR, 5)) + (A7 / Math.Pow(Tpr, 3)) * (3 * Math.Pow(RourR, 2) + A8 * (3 * Math.Pow(RourR, 4)) - Math.Pow(A8, 2) * (2 * Math.Pow(RourR, 6))) * Math.Exp(-A8 * Math.Pow(RourR, 2));
                RourR = RourR - Z2 / Z3;
            }
            while (!(Math.Abs(Z2 / Z3) < 0.00001));
            
            Z1 = 1 + (A1 + A2 / Tpr + A3 / (Math.Pow(Tpr, 3))) * RourR + (A4 + A5 / Tpr) * (Math.Pow(RourR, 2)) + (A5 * A6 / Tpr) * (Math.Pow(RourR, 5)) + A7 * (1 + A8 * Math.Pow(RourR, 2)) * (Math.Pow(RourR, 2) / (Math.Pow(Tpr, 3))) * Math.Exp(-A8 * Math.Pow(RourR, 2));


            //GASPVTZ = Z1;
            return Z1;
        }


        // 给定资料上的算法
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算天然气偏差系数，无量纲
        // 参数说明：IsDryGas：天然气类型，true为干气，false为凝析气
        // GasSpecificGravity：天然气比重，无量纲
        // CO2MoleFraction：天然气中CO2的摩尔含量，小数
        // H2SMoleFraction：天然气中H2S的摩尔含量，小数
        // Temperature：天然气体系温度，K
        // Pressure：天然气体系压力，Pa
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double GASPVTZ1(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction, double Temperature, double Pressure)
        {
            double Rg;
            double YCO2;
            double YH2S;
            double T;
            // 替换变量名称
            double P;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction; T = Temperature; P = Pressure;
            double Ppr;

            // 执行相应计算
            // Dranchuk-Purvis-Robinson方法，1974
            double Tpr;
            Tpr = GASPVTTPR(IsDryGas, Rg, YCO2, YH2S, T);
            Ppr = GASPVTPPR(IsDryGas, Rg, YCO2, YH2S, P);
            double A;
            double B;
            double C;
            double D;
            double E;
            double F;
            double G;
            A = 0.06432;
            B = 0.5353 * Tpr - 0.6123;
            C = 0.3151 * Tpr - 1.0467 - 0.5783 / Math.Pow(Tpr, 2);
            D = Tpr;
            E = 0.6816 / Math.Pow(Tpr, 2);
            F = 0.6845;
            G = 0.27 * Ppr;
            double Rour0;
            double FRour0;
            double DerivedFRour0;
            double Rour1;
            Rour0 = (0.27 * Ppr) / (1 * Tpr);
            FRour0 = A * Math.Pow(Rour0, 6) + B * Math.Pow(Rour0, 3) + C * Math.Pow(Rour0, 2) + D * Rour0 + E * Math.Pow(Rour0, 3) * (1 + F * Math.Pow(Rour0, 2)) * Math.Exp(-F * Math.Pow(Rour0, 2)) - G;
            DerivedFRour0 = 6 * A * Math.Pow(Rour0, 5) + 3 * B * Math.Pow(Rour0, 2) + 2 * C * Rour0 + D + E * Math.Pow(Rour0, 2) * (3 + F * Math.Pow(Rour0, 2) / (3 - 2 * F * Math.Pow(Rour0, 2))) * Math.Exp(-F * Math.Pow(Rour0, 2));
            Rour1 = Rour0 - FRour0 / DerivedFRour0;

            int Iterations;
            Iterations = 0;

            while (Math.Abs(Rour1 - Rour0) > Math.Pow(10, (-12)) & Iterations < 50)
            {
                Rour0 = Rour1;
                FRour0 = A * Math.Pow(Rour0, 6) + B * Math.Pow(Rour0, 3) + C * Math.Pow(Rour0, 2) + D * Rour0 + E * Math.Pow(Rour0, 3) * (1 + F * Math.Pow(Rour0, 2)) * Math.Exp(-F * Math.Pow(Rour0, 2)) - G;
                DerivedFRour0 = 6 * A * Math.Pow(Rour0, 5) + 3 * B * Math.Pow(Rour0, 2) + 2 * C * Rour0 + D + E * Math.Pow(Rour0, 2) * (3 + F * Math.Pow(Rour0, 2) / (3 - 2 * F * Math.Pow(Rour0, 2))) * Math.Exp(-F * Math.Pow(Rour0, 2));
                Rour1 = Rour0 - FRour0 / DerivedFRour0;
                Iterations = Iterations + 1;
            }
            double Z1;
            Z1 = (0.27 * Ppr) / (Rour1 * Tpr);

            // 计算结果输出
            return Z1;
        }

        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算天然气偏差系数，无量纲
        // 参数说明：IsDryGas：天然气类型，true为干气，false为凝析气
        // GasSpecificGravity：天然气比重，无量纲
        // CO2MoleFraction：天然气中CO2的摩尔含量，小数
        // H2SMoleFraction：天然气中H2S的摩尔含量，小数
        // Temperature：天然气体系温度，K
        // Pressure：天然气体系压力，Pa
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double GASPVTZ1(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction, double N2MoleFraction, double Temperature, double Pressure)
        {
            double Rg;
            double YCO2;
            double YH2S;
            double YN2;
            double T;
            // 替换变量名称
            double P;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction; YN2 = N2MoleFraction; T = Temperature; P = Pressure;
            double Ppr;

            // 执行相应计算
            // Dranchuk-Purvis-Robinson方法，1974
            double Tpr;
            Tpr = GASPVTTPR(IsDryGas, Rg, YCO2, YH2S, YN2, T);
            Ppr = GASPVTPPR(IsDryGas, Rg, YCO2, YH2S, YN2, P);
            double A;
            double B;
            double C;
            double D;
            double E;
            double F;
            double G;
            A = 0.06432;
            B = 0.5353 * Tpr - 0.6123;
            C = 0.3151 * Tpr - 1.0467 - 0.5783 / Math.Pow(Tpr, 2);
            D = Tpr;
            E = 0.6816 / Math.Pow(Tpr, 2);
            F = 0.6845;
            G = 0.27 * Ppr;
            double Rour0;
            double FRour0;
            double DerivedFRour0;
            double Rour1;
            Rour0 = (0.27 * Ppr) / (1 * Tpr);
            FRour0 = A * Math.Pow(Rour0, 6) + B * Math.Pow(Rour0, 3) + C * Math.Pow(Rour0, 2) + D * Rour0 + E * Math.Pow(Rour0, 3) * (1 + F * Math.Pow(Rour0, 2)) * Math.Exp(-F * Math.Pow(Rour0, 2)) - G;
            DerivedFRour0 = 6 * A * Math.Pow(Rour0, 5) + 3 * B * Math.Pow(Rour0, 2) + 2 * C * Rour0 + D + E * Math.Pow(Rour0, 2) * (3 + F * Math.Pow(Rour0, 2) / (3 - 2 * F * Math.Pow(Rour0, 2))) * Math.Exp(-F * Math.Pow(Rour0, 2));
            Rour1 = Rour0 - FRour0 / DerivedFRour0;

            int Iterations;
            Iterations = 0;

            while (Math.Abs(Rour1 - Rour0) > Math.Pow(10, (-12)) & Iterations < 50)
            {
                Rour0 = Rour1;
                FRour0 = A * Math.Pow(Rour0, 6) + B * Math.Pow(Rour0, 3) + C * Math.Pow(Rour0, 2) + D * Rour0 + E * Math.Pow(Rour0, 3) * (1 + F * Math.Pow(Rour0, 2)) * Math.Exp(-F * Math.Pow(Rour0, 2)) - G;
                DerivedFRour0 = 6 * A * Math.Pow(Rour0, 5) + 3 * B * Math.Pow(Rour0, 2) + 2 * C * Rour0 + D + E * Math.Pow(Rour0, 2) * (3 + F * Math.Pow(Rour0, 2) / (3 - 2 * F * Math.Pow(Rour0, 2))) * Math.Exp(-F * Math.Pow(Rour0, 2));
                Rour1 = Rour0 - FRour0 / DerivedFRour0;
                Iterations = Iterations + 1;
            }
            double Z1;
            Z1 = (0.27 * Ppr) / (Rour1 * Tpr);

            // 计算结果输出
            //GASPVTZ1 = Z1;
            return Z1;
        }

        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算天然气体积系数，rm^3/sm^3
        // 参数说明：IsDryGas：天然气类型，true为干气，false为凝析气
        // GasSpecificGravity：天然气比重，无量纲
        // CO2MoleFraction：天然气中CO2的摩尔含量，小数
        // H2SMoleFraction：天然气中H2S的摩尔含量，小数
        // Temperature：天然气体系温度，K
        // Pressure：天然气体系压力，Pa
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double GASPVTBG(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction, double N2MoleFraction, double Temperature, double Pressure)
        {
            double Rg;
            double YCO2;
            double YH2S;
            double YN2;
            double T;
            // 替换变量名称
            double P;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction; YN2 = N2MoleFraction; T = Temperature; P = Pressure;
            // 执行相应计算
            double Z;
            Z = GASPVTZ(IsDryGas, Rg, YCO2, YH2S, YN2, T, P);
            double Psc;
            double Tsc;
            double Zsc;
            Psc = 0.101325 * Math.Pow(10, 6);
            Tsc = 20 + 273.15;
            Zsc = 1;
            double Bg;
            Bg = (Z * T * Psc) / (P * Zsc * Tsc);

            // 计算结果输出
            //GASPVTBG = Bg;
            return Bg;
        }

        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算天然气密度，kg/m^3
        // 参数说明：IsDryGas：天然气类型，true为干气，false为凝析气
        // GasSpecificGravity：天然气比重，无量纲
        // CO2MoleFraction：天然气中CO2的摩尔含量，小数
        // H2SMoleFraction：天然气中H2S的摩尔含量，小数
        // Temperature：天然气体系温度，K
        // Pressure：天然气体系压力，Pa
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double GASPVTDEN(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction, double N2MoleFraction, double Temperature, double Pressure)
        {
            double Rg;
            double YCO2;
            double YH2S;
            double YN2;
            double T;
            // 替换变量名称
            double P;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction; YN2 = N2MoleFraction; T = Temperature; P = Pressure;
            double Z;

            // 执行相应计算
            double Roug;
            Z = GASPVTZ(IsDryGas, Rg, YCO2, YH2S, YN2, T, P);
            Roug = (28.97 * Rg * P) / (Z * 8.315 * T) * Math.Pow(10, (-3));

            // 计算结果输出
            //GASPVTDEN = Roug;
            return Roug;
        }

        // 
        // *********************************************GASPVTVIS**********************************************************************************************************************************************************************************************
        // 函数作用：计算天然气粘度，Pa.s
        // 参数说明：IsDryGas：天然气类型，true为干气，false为凝析气
        // GasSpecificGravity：天然气比重，无量纲
        // CO2MoleFraction：天然气中CO2的摩尔含量，小数
        // H2SMoleFraction：天然气中H2S的摩尔含量，小数
        // Temperature：天然气体系温度，K
        // Pressure：天然气体系压力，Pa
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double GASPVTVIS(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction, double N2MoleFraction, double Temperature, double Pressure)
        {
            double Rg;
            double YCO2;
            double YH2S;
            double YN2;
            double T;
            // 替换变量名称
            double P;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction; YN2 = N2MoleFraction; T = Temperature; P = Pressure;

            // 执行相应计算
            // Lee-Gonzalez方法
            double Roug;
            Roug = GASPVTDEN(IsDryGas, Rg, YCO2, YH2S, YN2, T, P);
            double X;
            double Y;
            double K;
            double Visg;
            X = 0.01 * (350 + 54777.78 / T + 28.97 * Rg);
            Y = 0.2 * (12 - X);
            K = (2.6832 * 0.01 * (470 + 28.97 * Rg) * Math.Pow(T, 1.5)) / (116.1111 + 10.5556 * 28.97 * Rg + T);
            Visg = Math.Pow(10, (-7)) * K * Math.Exp(X * Math.Pow(Roug, Y) * Math.Pow(10, (-3 * Y)));

            // 计算结果输出
            //GASPVTVIS = Visg;
            return Visg;
        }

        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算天然气等温压缩系数，1/Pa
        // 参数说明：IsDryGas：天然气类型，true为干气，false为凝析气
        // GasSpecificGravity：天然气比重，无量纲
        // CO2MoleFraction：天然气中CO2的摩尔含量，小数
        // H2SMoleFraction：天然气中H2S的摩尔含量，小数
        // Temperature：天然气体系温度，K
        // Pressure：天然气体系压力，Pa
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double GASPVTCG(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction, double N2MoleFraction, double Temperature, double Pressure)
        {
            double Rg;
            double YCO2;
            double YH2S;
            double YN2;
            double T;
            // 替换变量名称
            double P;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction; YN2 = N2MoleFraction; T = Temperature; P = Pressure;
            double Ppc;
            // 执行相应计算
            double Z;
            Ppc = GASPVTPPC(IsDryGas, Rg, YCO2, YH2S, YN2);
            Z = GASPVTZ(IsDryGas, Rg, YCO2, YH2S, YN2, T, P);
            double Ppr;
            double Tpr;
            Tpr = GASPVTTPR(IsDryGas, Rg, YCO2, YH2S, YN2, T);
            Ppr = GASPVTPPR(IsDryGas, Rg, YCO2, YH2S, YN2, P);
            double A;
            double B;
            double C;
            double D;
            double E;
            double F;
            double G;
            A = 0.06432;
            B = 0.5353 * Tpr - 0.6123;
            C = 0.3151 * Tpr - 1.0467 - 0.5783 / Math.Pow(Tpr, 2);
            D = Tpr;
            E = 0.6816 / Math.Pow(Tpr, 2);
            F = 0.6845;
            G = 0.27 * Ppr;
            double Rour0;
            double FRour0;
            double DerivedFRour0;
            double Rour1;
            Rour0 = (0.27 * Ppr) / (1 * Tpr);
            FRour0 = A * Math.Pow(Rour0, 6) + B * Math.Pow(Rour0, 3) + C * Math.Pow(Rour0, 2) + D * Rour0 + E * Math.Pow(Rour0, 3) * (1 + F * Math.Pow(Rour0, 2)) * Math.Exp(-F * Math.Pow(Rour0, 2)) - G;
            DerivedFRour0 = 6 * A * Math.Pow(Rour0, 5) + 3 * B * Math.Pow(Rour0, 2) + 2 * C * Rour0 + D + E * Math.Pow(Rour0, 2) * (3 + F * Math.Pow(Rour0, 2) / (3 - 2 * F * Math.Pow(Rour0, 2))) * Math.Exp(-F * Math.Pow(Rour0, 2));
            Rour1 = Rour0 - FRour0 / DerivedFRour0;
            while (Math.Abs(Rour1 - Rour0) >= Math.Pow(10, (-12)))
            {
                Rour0 = Rour1;
                FRour0 = A * Math.Pow(Rour0, 6) + B * Math.Pow(Rour0, 3) + C * Math.Pow(Rour0, 2) + D * Rour0 + E * Math.Pow(Rour0, 3) * (1 + F * Math.Pow(Rour0, 2)) * Math.Exp(-F * Math.Pow(Rour0, 2)) - G;
                DerivedFRour0 = 6 * A * Math.Pow(Rour0, 5) + 3 * B * Math.Pow(Rour0, 2) + 2 * C * Rour0 + D + E * Math.Pow(Rour0, 2) * (3 + F * Math.Pow(Rour0, 2) / (3 - 2 * F * Math.Pow(Rour0, 2))) * Math.Exp(-F * Math.Pow(Rour0, 2));
                Rour1 = Rour0 - FRour0 / DerivedFRour0;
            }

            double DerivedRour1;
            double Cg;
            DerivedRour1 = 1 / (Rour1 * Tpr) * (5 * A * Math.Pow(Rour1, 5) + 2 * B * Math.Pow(Rour1, 2) + C * Rour1 + 2 * E * Math.Pow(Rour1, 2) * (1 + F * Math.Pow(Rour1, 2) - Math.Pow(F, 2) * Math.Pow(Rour1, 4)) * Math.Exp(-F * Rour1));
            Cg = 1 / (Ppc * Ppr * (1 + (Rour1 / Z) * DerivedRour1));

            // 计算结果输出
            //GASPVTCG = Cg;
            return Cg;
        }


        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算天然气拟压力，Pa^2/(Pa.s)
        // 参数说明：IsDryGas：天然气类型，true为干气，false为凝析气
        // GasSpecificGravity：天然气比重，无量纲
        // CO2MoleFraction：天然气中CO2的摩尔含量，小数
        // H2SMoleFraction：天然气中H2S的摩尔含量，小数
        // Temperature：天然气体系温度，K
        // Pressure：天然气体系压力，Pa
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double GASPVTPP(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction, double N2MoleFraction, double Temperature, double Pressure)
        {
            double Rg;
            double YCO2;
            double YH2S;
            double YN2;
            double T;
            // 替换变量名称
            double P;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction; YN2 = N2MoleFraction; T = Temperature; P = Pressure;

            // 执行相应计算
            double Gaspp;
            if (P <= 0)
                Gaspp = 0;
            else
            {
                double DeltaP;
                DeltaP = 100000;
                double PseudoP;
                PseudoP = 0;
                double Mu0;
                double Z0;
                Mu0 = GASPVTVIS(IsDryGas, Rg, YCO2, YH2S, YN2, T, P);
                Z0 = GASPVTZ(IsDryGas, Rg, YCO2, YH2S, YN2, T, P);
                if (P <= DeltaP)
                {
                    PseudoP = PseudoP + (2 * P / (Mu0 * Z0)) * DeltaP / 2;
                    Gaspp = PseudoP;
                }
                else
                {
                    double Mu1;
                    double Z1;
                    do
                    {
                        Mu1 = GASPVTVIS(IsDryGas, Rg, YCO2, YH2S, YN2, T, P - DeltaP);
                        Z1 = GASPVTZ(IsDryGas, Rg, YCO2, YH2S, YN2, T, P - DeltaP);
                        PseudoP = PseudoP + (2 * P / (Mu0 * Z0) + 2 * (P - DeltaP) / (Mu1 * Z1)) * DeltaP / 2;
                        P = P - DeltaP;
                        Mu0 = Mu1;
                        Z0 = Z1;
                    }
                    while (!(P <= DeltaP));
                    PseudoP = PseudoP + (2 * P / (Mu0 * Z0)) * DeltaP / 2;
                    Gaspp = PseudoP;
                }
            }

            // 计算结果输出
            //GASPVTPP = Gaspp;
            return Gaspp;
        }



        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算天然气体积系数，rm^3/sm^3
        // 参数说明：IsDryGas：天然气类型，true为干气，false为凝析气
        // GasSpecificGravity：天然气比重，无量纲
        // CO2MoleFraction：天然气中CO2的摩尔含量，小数
        // H2SMoleFraction：天然气中H2S的摩尔含量，小数
        // Temperature：天然气体系温度，K
        // Pressure：天然气体系压力，Pa
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double GASPVTBG(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction, double Temperature, double Pressure)
        {
            double Rg;
            double YCO2;
            double YH2S;
            double T;
            // 替换变量名称
            double P;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction; T = Temperature; P = Pressure;

            // 执行相应计算
            double Z;
            Z = GASPVTZ(IsDryGas, Rg, YCO2, YH2S, T, P);
            double Psc;
            double Tsc;
            double Zsc;
            Psc = 0.101325 * Math.Pow(10, 6);
            Tsc = 20 + 273.15;
            Zsc = 1;
            double Bg;
            Bg = (Z * T * Psc) / (P * Zsc * Tsc);

            // 计算结果输出
            return Bg;
        }
        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算天然气密度，kg/m^3
        // 参数说明：IsDryGas：天然气类型，true为干气，false为凝析气
        // GasSpecificGravity：天然气比重，无量纲
        // CO2MoleFraction：天然气中CO2的摩尔含量，小数
        // H2SMoleFraction：天然气中H2S的摩尔含量，小数
        // Temperature：天然气体系温度，K
        // Pressure：天然气体系压力，Pa
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double GASPVTDEN(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction, double Temperature, double Pressure)
        {
            double Rg;
            double YCO2;
            double YH2S;
            double T;
            // 替换变量名称
            double P;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction; T = Temperature; P = Pressure;
            double Z;

            // 执行相应计算
            double Roug;
            Z = GASPVTZ1(IsDryGas, Rg, YCO2, YH2S, T, P);
            Roug = (28.97 * Rg * P) / (Z * 8.315 * T) * Math.Pow(10, (-3));

            // 计算结果输出
            return Roug;
        }
        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算天然气粘度，Pa.s
        // 参数说明：IsDryGas：天然气类型，true为干气，false为凝析气
        // GasSpecificGravity：天然气比重，无量纲
        // CO2MoleFraction：天然气中CO2的摩尔含量，小数
        // H2SMoleFraction：天然气中H2S的摩尔含量，小数
        // Temperature：天然气体系温度，K
        // Pressure：天然气体系压力，Pa
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double GASPVTVIS(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction, double Temperature, double Pressure)
        {
            double Rg;
            double YCO2;
            double YH2S;
            double T;
            // 替换变量名称
            double P;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction; T = Temperature; P = Pressure;

            // 执行相应计算
            // Lee-Gonzalez方法
            double Roug;
            Roug = GASPVTDEN(IsDryGas, Rg, YCO2, YH2S, T, P);
            double X;
            double Y;
            double K;
            double Visg;
            X = 0.01 * (350 + 54777.78 / T + 28.97 * Rg);
            Y = 0.2 * (12 - X);
            K = (2.6832 * 0.01 * (470 + 28.97 * Rg) * Math.Pow(T, 1.5)) / (116.1111 + 10.5556 * 28.97 * Rg + T);
            Visg = Math.Pow(10, (-7)) * K * Math.Exp(X * Math.Pow(Roug, Y) * Math.Pow(10, (-3 * Y)));

            // 计算结果输出
            return Visg;
        }
        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算天然气等温压缩系数，1/Pa
        // 参数说明：IsDryGas：天然气类型，true为干气，false为凝析气
        // GasSpecificGravity：天然气比重，无量纲
        // CO2MoleFraction：天然气中CO2的摩尔含量，小数
        // H2SMoleFraction：天然气中H2S的摩尔含量，小数
        // Temperature：天然气体系温度，K
        // Pressure：天然气体系压力，Pa
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double GASPVTCG(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction, double Temperature, double Pressure)
        {
            double Rg;
            double YCO2;
            double YH2S;
            double T;
            // 替换变量名称
            double P;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction; T = Temperature; P = Pressure;
            double Ppc;

            // 执行相应计算
            double Z;
            Ppc = GASPVTPPC(IsDryGas, Rg, YCO2, YH2S);
            Z = GASPVTZ(IsDryGas, Rg, YCO2, YH2S, T, P);
            double Ppr;
            double Tpr;
            Tpr = GASPVTTPR(IsDryGas, Rg, YCO2, YH2S, T);
            Ppr = GASPVTPPR(IsDryGas, Rg, YCO2, YH2S, P);
            double A;
            double B;
            double C;
            double D;
            double E;
            double F;
            double G;
            A = 0.06432;
            B = 0.5353 * Tpr - 0.6123;
            C = 0.3151 * Tpr - 1.0467 - 0.5783 / Math.Pow(Tpr, 2);
            D = Tpr;
            E = 0.6816 / Math.Pow(Tpr, 2);
            F = 0.6845;
            G = 0.27 * Ppr;
            double Rour0;
            double FRour0;
            double DerivedFRour0;
            double Rour1;
            Rour0 = (0.27 * Ppr) / (1 * Tpr);
            FRour0 = A * Math.Pow(Rour0, 6) + B * Math.Pow(Rour0, 3) + C * Math.Pow(Rour0, 2) + D * Rour0 + E * Math.Pow(Rour0, 3) * (1 + F * Math.Pow(Rour0, 2)) * Math.Exp(-F * Math.Pow(Rour0, 2)) - G;
            DerivedFRour0 = 6 * A * Math.Pow(Rour0, 5) + 3 * B * Math.Pow(Rour0, 2) + 2 * C * Rour0 + D + E * Math.Pow(Rour0, 2) * (3 + F * Math.Pow(Rour0, 2) / (3 - 2 * F * Math.Pow(Rour0, 2))) * Math.Exp(-F * Math.Pow(Rour0, 2));
            Rour1 = Rour0 - FRour0 / DerivedFRour0;
            while (Math.Abs(Rour1 - Rour0) >= Math.Pow(10, (-12)))
            {
                Rour0 = Rour1;
                FRour0 = A * Math.Pow(Rour0, 6) + B * Math.Pow(Rour0, 3) + C * Math.Pow(Rour0, 2) + D * Rour0 + E * Math.Pow(Rour0, 3) * (1 + F * Math.Pow(Rour0, 2)) * Math.Exp(-F * Math.Pow(Rour0, 2)) - G;
                DerivedFRour0 = 6 * A * Math.Pow(Rour0, 5) + 3 * B * Math.Pow(Rour0, 2) + 2 * C * Rour0 + D + E * Math.Pow(Rour0, 2) * (3 + F * Math.Pow(Rour0, 2) / (3 - 2 * F * Math.Pow(Rour0, 2))) * Math.Exp(-F * Math.Pow(Rour0, 2));
                Rour1 = Rour0 - FRour0 / DerivedFRour0;
            }

            double DerivedRour1;
            double Cg;
            DerivedRour1 = 1 / (Rour1 * Tpr) * (5 * A * Math.Pow(Rour1, 5) + 2 * B * Math.Pow(Rour1, 2) + C * Rour1 + 2 * E * Math.Pow(Rour1, 2) * (1 + F * Math.Pow(Rour1, 2) - Math.Pow(F, 2) * Math.Pow(Rour1, 4)) * Math.Exp(-F * Rour1));
            Cg = 1 / (Ppc * Ppr * (1 + (Rour1 / Z) * DerivedRour1));

            // 计算结果输出
            return Cg;
        }
        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算天然气拟压力，Pa^2/(Pa.s)
        // 参数说明：IsDryGas：天然气类型，true为干气，false为凝析气
        // GasSpecificGravity：天然气比重，无量纲
        // CO2MoleFraction：天然气中CO2的摩尔含量，小数
        // H2SMoleFraction：天然气中H2S的摩尔含量，小数
        // Temperature：天然气体系温度，K
        // Pressure：天然气体系压力，Pa
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double GASPVTPP(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction, double Temperature, double Pressure)
        {
            double Rg;
            double YCO2;
            double YH2S;
            double T;
            // 替换变量名称
            double P;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction; T = Temperature; P = Pressure;

            // 执行相应计算
            double Gaspp;
            if (P <= 0)
                Gaspp = 0;
            else
            {
                double DeltaP;
                DeltaP = 100000;
                double PseudoP;
                PseudoP = 0;
                double Mu0;
                double Z0;
                Mu0 = GASPVTVIS(IsDryGas, Rg, YCO2, YH2S, T, P);
                Z0 = GASPVTZ(IsDryGas, Rg, YCO2, YH2S, T, P);

                if (P <= DeltaP)
                {
                    PseudoP += (2 * P / (Mu0 * Z0)) * DeltaP / 2;
                    Gaspp = PseudoP;
                }
                else
                {
                    double Mu1;
                    double Z1;
                    do
                    {
                        Mu1 = GASPVTVIS(IsDryGas, Rg, YCO2, YH2S, T, P - DeltaP);
                        Z1 = GASPVTZ(IsDryGas, Rg, YCO2, YH2S, T, P - DeltaP);
                        PseudoP += ((2 * P / (Mu0 * Z0)) + (2 * (P - DeltaP) / (Mu1 * Z1))) * DeltaP / 2;
                        P -= DeltaP;
                        Mu0 = Mu1;
                        Z0 = Z1;
                    }
                    while (P > DeltaP);
                    PseudoP += (2 * P / (Mu0 * Z0)) * DeltaP / 2;
                    Gaspp = PseudoP;
                }
            }
            // 计算结果输出
            return Gaspp;
        }
        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算天然气规整化拟压力，Pa
        // 参数说明：IsDryGas：天然气类型，true为干气，false为凝析气
        // GasSpecificGravity：天然气比重，无量纲
        // CO2MoleFraction：天然气中CO2的摩尔含量，小数
        // H2SMoleFraction：天然气中H2S的摩尔含量，小数
        // Temperature：天然气体系温度，K
        // Pressure：天然气体系压力，Pa
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double GASPVTNPP(bool IsDryGas, double GasSpecificGravity, double CO2MoleFraction, double H2SMoleFraction, double Temperature, double InitialPressure, double Pressure)
        {
            double Rg;
            double YCO2;
            double YH2S;
            double T;
            double P;
            // 替换变量名称
            double Pi;
            Rg = GasSpecificGravity; YCO2 = CO2MoleFraction; YH2S = H2SMoleFraction; T = Temperature; Pi = Pressure; P = Pressure;
            double Mui;
            double Zi;
            // 执行相应计算
            double Pp;
            Mui = GASPVTVIS(IsDryGas, Rg, YCO2, YH2S, T, Pi);
            Zi = GASPVTZ(IsDryGas, Rg, YCO2, YH2S, T, Pi);
            Pp = GASPVTPP(IsDryGas, Rg, YCO2, YH2S, T, P);
            return (Mui * Zi / Pi) * Pp / 2;
        }

        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算天然气凝析水气比，输出结果单位为m^3/m^3
        // 参数说明：Temperature：天然气体系温度，K
        // Pressure：天然气体系压力，Pa
        // Salinity：地层水矿化度，%
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double GASPVTCWGR(double Pressure, double Temperature, double Salinity)
        {
            double T;
            double P;
            // 替换变量名称
            double NaCl;
            T = Temperature; P = Pressure; NaCl = Salinity;
            double A;
            double B;
            double C;

            // 执行相应计算
            double Cwgr;
            //A = 3.4 + 418.0278 / (P / Math.Pow(10, 6));
            //B = 3.2147 + 3.8537 * Math.Pow(10, (-2)) * (P / Math.Pow(10, 6)) - 4.7752 * Math.Pow(10, (-4)) * Math.Pow((P / Math.Pow(10, 6)), 2);
            //C = 1 - 0.4893 * Math.Pow(10, (-2)) * NaCl - 1.757 * Math.Pow(10, (-4)) * Math.Pow(NaCl, 2);
            //Cwgr = 1.6019 * Math.Pow(10, (-4)) * A * Math.Pow((0.32 * (0.05625 * (T - 273.15) + 1)), B) * C / Math.Pow(10, 4);
            A = 3.4 + 418.0278 / (P / Math.Pow(10, 6));
            B = 3.2147 + 3.8537 * Math.Pow(10, (-2)) * (P / Math.Pow(10, 6)) - 4.7752 * Math.Pow(10, (-4)) * Math.Pow((P / Math.Pow(10, 6)), 2);
            C = 1 - 0.4893 * Math.Pow(10, (-2)) * NaCl - 1.757 * Math.Pow(10, (-4)) * Math.Pow(NaCl, 2);
            Cwgr = 1.6019 * Math.Pow(10, (-4)) * A * Math.Pow((0.32 * (0.05625 * (T - 273.15) + 1)), B) * C / Math.Pow(10, 4);



            // 计算结果输出
            return Cwgr;

        }
    }
}
