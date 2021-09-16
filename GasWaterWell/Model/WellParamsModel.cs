using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasWaterWell.Model
{
    class WellParamsModel
    {
        public int well_id { get; set; }
        public double s { get; set; }
        public double d { get; set; }
        public double re { get; set; }
        public double rw { get; set; }
        public double rhogsc { get; set; }
        public double rhowsc { get; set; }

        public double pe { get; set; }

        public WellParamsModel(int well_id, double s, double d, double re, double rw, double rhogsc, double rhowsc, double pe)
        {
            this.well_id = well_id;
            this.s = s; // 气井表皮系数
            this.d = d; // 非达西渗流系数
            this.re = re; // 井控半径
            this.rw = rw; // 井径
            this.rhogsc = rhogsc; // 气体密度
            this.rhowsc = rhowsc; // 
            this.pe = pe;
        }

        public WellParamsModel()
        {
        }
    }
}
