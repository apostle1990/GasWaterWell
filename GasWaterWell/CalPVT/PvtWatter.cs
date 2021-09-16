using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasWaterWell
{
    class PvtWater
    {
        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算地层水溶解水汽比，sm^3/sm^3
        // 参数说明：Temperature：体系温度，K
        // Pressure：体系压力，Pa
        // Salinity：地层水矿化度，%（1%=10000ppm=10000mg/L)
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double WATERPVTRS(double Pressure, double Temperature, double Salinity)
        {
            double T;
            double P;
            // 替换变量名称
            double NaCl;
            T = Temperature; P = Pressure; NaCl = Salinity;
            double A;
            double B;
            double C;
            double Sc;
            double MidVar;

            // 执行相应计算
            double Rsw;
            MidVar = 5.625 * Math.Pow(10, (-2)) * (T - 273.15) + 1;
            A = 2.12 + 0.1104 * MidVar - 0.03676 * Math.Pow(MidVar, 2);
            B = 1.5519 - 0.2441 * MidVar + 0.02198 * Math.Pow(MidVar, 2);
            C = -1.8406 * Math.Pow(10, (-2)) + 2.6253 * Math.Pow(10, (-3)) * MidVar - 2.1972 * Math.Pow(10, (-4)) * Math.Pow(MidVar, 2);
            Sc = 1 - (0.0753 - 5.536 * Math.Pow(10, (-3)) * MidVar) * NaCl;
            Rsw = 0.178 * (A + B * (P / Math.Pow(10, 6)) + C * Math.Pow((P / Math.Pow(10, 6)), 2)) * Sc;

            // 计算结果输出
            return Rsw;
        }
        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算地层水体积系数，rm^3/sm^3
        // 参数说明：Temperature：体系温度，K
        // Pressure：体系压力，Pa
        // Salinity：地层水矿化度，%（1%=10000ppm=10000mg/L)
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double WATERPVTBW(double Pressure, double Temperature, double Salinity)
        {
            double T;
            double P;
            // 替换变量名称
            double NaCl;
            T = Temperature; P = Pressure; NaCl = Salinity;
            double A;
            double B;
            double C;
            double Sc;
            double MidVar;

            // 执行相应计算
            double Bw;
            MidVar = 5.625 * Math.Pow(10, (-2)) * (T - 273.15) + 1;
            A = 0.9911 + 2.032 * Math.Pow(10, (-3)) * MidVar + 8.704 * Math.Pow(10, (-4)) * Math.Pow(MidVar, 2);
            B = -1.5853 * Math.Pow(10, (-4)) - 1.623 * Math.Pow(10, (-5)) * MidVar + 6.7873 * Math.Pow(10, (-7)) * Math.Pow(MidVar, 2);
            C = -1.0518 * Math.Pow(10, (-6)) + 4.3277 * Math.Pow(10, (-7)) * MidVar - 3.0803 * Math.Pow(10, (-8)) * Math.Pow(MidVar, 2);
            double MidVar1;
            MidVar1 = 5.625 * Math.Pow(10, (-2)) * (T - 273.15) - 0.875;
            Sc = 1 + NaCl * (7.397 * Math.Pow(10, (-6)) * (P / Math.Pow(10, 6)) + (1.75 * Math.Pow(10, (-4)) - 9.05 * Math.Pow(10, (-7)) * (P / Math.Pow(10, 6))) * MidVar1 - (3.308 * Math.Pow(10, (-5)) - 1.262 * Math.Pow(10, (-7)) * (P / Math.Pow(10, 6))) * Math.Pow(MidVar1, 2));
            Bw = Sc * (A + B * (P / Math.Pow(10, 6)) + C * Math.Pow((P / Math.Pow(10, 6)), 2));

            // 计算结果输出
            return Bw;
        }
        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算地层水压缩系数，1/Pa
        // 参数说明：Temperature：体系温度，K
        // Pressure：体系压力，Pa
        // Salinity：地层水矿化度，%（1%=10000ppm=10000mg/L)
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double WATERPVTCW(double Pressure, double Temperature, double Salinity)
        {
            double T;
            double P;
            // 替换变量名称
            double NaCl;
            T = Temperature; P = Pressure; NaCl = Salinity;
            double A;
            double B;
            double C;
            double Sc;
            double MidVar;

            // 执行相应计算
            double Cw;
            MidVar = 5.625 * Math.Pow(10, (-2)) * (T - 273.15) + 1;
            A = 3.8546 - 1.9435 * Math.Pow(10, (-2)) * (P / Math.Pow(10, 6));
            B = -0.3366 + 2.2138 * Math.Pow(10, (-3)) * (P / Math.Pow(10, 6));
            C = 4.0209 * Math.Pow(10, (-2)) - 1.3069 * Math.Pow(10, (-4)) * (P / Math.Pow(10, 6));
            Sc = 1 + (-0.052 + 8.64 * Math.Pow(10, (-3)) * MidVar - 1.167 * Math.Pow(10, (-3)) * Math.Pow(MidVar, 2) + 3.965 * Math.Pow(10, (-5)) * Math.Pow(MidVar, 3)) * Math.Pow(NaCl, 0.7);
            Cw = 1.4504 * Math.Pow(10, (-10)) * Sc * (A + B * MidVar + C * Math.Pow(MidVar, 2)) * (1 + 4.9974 * Math.Pow(10, (-2)) * WATERPVTRS(P, T, NaCl));

            // 计算结果输出
            return Cw;
        }
        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算地层水密度，kg/m^3,
        // 参数说明：Temperature：体系温度，K
        // Pressure：体系压力，Pa
        // Salinity：地层水矿化度，%（1%=10000ppm=10000mg/L)
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double WATERPVTDEN(double Pressure, double Temperature, double Salinity)
        {
            double T;
            double P;
            // 替换变量名称
            double NaCl;
            T = Temperature; P = Pressure; NaCl = Salinity;

            // 执行相应计算
            double Rouw;
            Rouw = (0.998 + 7.0258 * Math.Pow(10, (-3)) * NaCl + 2.5641 * Math.Pow(10, (-5)) * Math.Pow(NaCl, 2)) / WATERPVTBW(P, T, NaCl) * Math.Pow(10, 3);

            // 计算结果输出
            return Rouw;
        }
        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算地层水粘度，Pa.s,mPa.s
        // 参数说明：Temperature：体系温度，K
        // Pressure：体系压力，Pa
        // Salinity：地层水矿化度，%（1%=10000ppm=10000mg/L)
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double WATERPVTVIS(double Pressure, double Temperature, double Salinity)
        {
            double T;
            double P;
            // 替换变量名称
            double NaCl;
            T = Temperature; P = Pressure; NaCl = Salinity;
            double Sc;
            double Sp;
            double MidVar;

            // 执行相应计算
            double Muw;
            MidVar = 5.625 * Math.Pow(10, (-2)) * (T - 273.15) + 1;
            Sc = 1 - 0.00187 * Math.Pow(NaCl, 0.5) + 0.000218 * Math.Pow(NaCl, 2.5) + (5.657 * Math.Pow(MidVar, 0.5) - 0.432 * MidVar) * (0.000276 * NaCl - 0.000344 * Math.Pow(NaCl, 1.5));
            Sp = 1 + 2.356 * Math.Pow(10, (-6)) * Math.Pow((P / Math.Pow(10, 6)), 2) * (5.625 * Math.Pow(10, (-2)) * (T - 273.15) - 0.25);
            Muw = Sc * Sp * 0.02414 * Math.Pow(10, (247.8 / ((T - 273.15) + 133))) * Math.Pow(10, (-3));

            // 计算结果输出
            return Muw;
        }
    }
}
