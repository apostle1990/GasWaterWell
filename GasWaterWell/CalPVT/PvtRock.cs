﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasWaterWell
{
    class PvtRock
    {
        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算砂岩岩石压缩系数，1/Pa
        // 参数说明：Porosity：岩石孔隙度，小数
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double ROCKSANDCP(double Porosity)
        {
            double A;
            double B;
            // Newman,1973
            double C;
            A = 1.4115 * Math.Pow(10, (-2));
            B = 0.7;
            C = 79.8181;
            return A / Math.Pow((1 + B * C * Porosity), (1 / B)) / (Math.Pow(10, 6));
        }
        // 
        // *******************************************************************************************************************************************************************************************************************************************
        // 函数作用：计算灰岩岩石压缩系数，1/Pa
        // 参数说明：Porosity：岩石孔隙度，小数
        // *******************************************************************************************************************************************************************************************************************************************
        // 
        public double ROCKLIMECP(double Porosity)
        {
            double A;
            double B;
            // Newman,1973
            double C;
            A = 123.7899;
            B = 1.075;
            C = 2.202 * Math.Pow(10, 6);
            return A / Math.Pow((1 + B * C * Porosity), (1 / B)) / (Math.Pow(10, 6));
        }
    }
}