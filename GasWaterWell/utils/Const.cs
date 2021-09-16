using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasWaterWell.utils
{
    class Const
    {
        public static readonly string CONFIGPATH = "./conf/Config.ini"; // 配置文件

        public static string USER = "root";

        public static string PWD = "123456";

        public static string IP = "127.0.0.1";

        public static string DBNAME = "waterwell";

        //public static string DBSTR = ; // 数据库连接
        public static string DBSTR = String.Format(@"server={0};User Id={1};password={2};Database={3};charset=utf8;", IP, USER, PWD, DBNAME);

        //硬编码
        //public static string DBSTR = @"server=192.168.1.130;User Id=lab;password=lablab;Database=waterwell"; // 数据库连接

        public static string PYTHONPATH = "E:/Project/GasWaterWell/GasWaterWell/bin/venv/Scripts/python.exe"; // Python路径

        public static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

    }
}
