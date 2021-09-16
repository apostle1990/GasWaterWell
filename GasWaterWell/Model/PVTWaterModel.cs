using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasWaterWell.Model
{
    class PVTWaterModel
    {
        public string project_id { get; set; }
        public string pvt_water_index { get; set; }
        public double p { get; set; }
        public double Bw { get; set; }
        public double pw { get; set; }
        public double uw { get; set; }
        public double Cw { get; set; }
        public double Rsw { get; set; }
        public double Rcwg { get; set; }

        //输入变量
        public double T { get; set; }
        public double nacl { get; set; }
        public string waterName { get; set; }

        public PVTWaterModel(string project_id, string pvt_water_index, double p, double bw, double pw, double uw, double cw, double rsw, double rcwg, double t, double nacl, string waterName) : this(project_id, pvt_water_index)
        {
            this.project_id = project_id;
            this.pvt_water_index = pvt_water_index;
            this.p = p;
            Bw = bw;
            this.pw = pw;
            this.uw = uw;
            Cw = cw;
            Rsw = rsw;
            Rcwg = rcwg;
            T = t;
            this.nacl = nacl;
            this.waterName = waterName;
        }

        public PVTWaterModel(double p,double bw, double pw, double uw, double cw, double rsw, double rcwg)
        {
            this.p = p;
            Bw = bw;
            this.pw = pw;
            this.uw = uw;
            Cw = cw;
            Rsw = rsw;
            Rcwg = rcwg;
        }

        public PVTWaterModel(double p, double bw, double pw, double uw, double cw, double rsw, double rcwg, double t, double nacl, string waterName) : this(p, bw, pw, uw, cw, rsw, rcwg)
        {
            T = t;
            this.nacl = nacl;
            this.waterName = waterName;
        }

        public PVTWaterModel(string waterName, string pvt_water_index)
        {
            this.waterName = waterName;
            this.pvt_water_index = pvt_water_index;
        }

        public PVTWaterModel()
        {
        }

    }
}
