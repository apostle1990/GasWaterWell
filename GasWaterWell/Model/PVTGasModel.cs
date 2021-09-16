using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasWaterWell.Model
{
    class PVTGasModel
    {
        public string project_id { get; set; }
        public double p { get; set; }
        public double z1 { get; set; }
        public double z2 { get; set; }
        public double Bg { get; set; }
        public double pg { get; set; }
        public double ug { get; set; }
        public double Cg { get; set; }
        public double Pp { get; set; }
        public double Rcwg { get; set; }

        //输入变量
        public string gasName { get; set; }
        public double yg { get; set; }
        public double yco2 { get; set; }
        public double yh2s { get; set; }
        public double yn2 { get; set; }
        public double nacl { get; set; }
        public double t { get; set; }
        public bool isDryGas { get; set; }
        public string pvt_gas_index { get; set; }

        public PVTGasModel(double p, double z1, double z2, double bg, double pg, double ug, double cg, double pp, double rcwg)
        {
            this.p = p;
            this.z1 = z1;
            this.z2 = z2;
            Bg = bg;
            this.pg = pg;
            this.ug = ug;
            Cg = cg;
            Pp = pp;
            Rcwg = rcwg;
        }

        public PVTGasModel(string project_id, double p, double z1, double z2, double bg, double pg, double ug, double cg, double pp, double rcwg, string gasName, double yg, double yco2, double yh2s, double nacl, double t, bool isDryGas, string pvt_gas_index)
        {
            this.project_id = project_id;
            this.p = p;
            this.z1 = z1;
            this.z2 = z2;
            Bg = bg;
            this.pg = pg;
            this.ug = ug;
            Cg = cg;
            Pp = pp;
            Rcwg = rcwg;
            this.gasName = gasName ?? throw new ArgumentNullException(nameof(gasName));
            this.yg = yg;
            this.yco2 = yco2;
            this.yh2s = yh2s;
            this.nacl = nacl;
            this.t = t;
            this.isDryGas = isDryGas;
            this.pvt_gas_index = pvt_gas_index ?? throw new ArgumentNullException(nameof(pvt_gas_index));
        }

        public PVTGasModel(double p, double z1, double z2, double bg, double pg, double ug, double cg, double pp, double rcwg, string gasName, double yg, double yco2, double yh2s, double yn2, double nacl, double t, bool isDryGas) : this(p, z1, z2, bg, pg, ug, cg, pp, rcwg)
        {
            this.gasName = gasName;
            this.yg = yg;
            this.yco2 = yco2;
            this.yh2s = yh2s;
            this.yn2 = yn2;
            this.nacl = nacl;
            this.t = t;
            this.isDryGas = isDryGas;
        }

        public PVTGasModel(string gasName, string pvt_gas_index)
        {
            this.gasName = gasName;
            this.pvt_gas_index = pvt_gas_index;
        }

        public PVTGasModel()
        {
        }
    }
}
