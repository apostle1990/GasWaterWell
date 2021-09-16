using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasWaterWell.Model
{
    class GasWaterTwoModel
    {
        public double rhowsc { get; set; }
        public double rhogsc { get; set; }
        public double s { get; set; }
        public double d { get; set; }
        public double re { get; set; }
        public double rw { get; set; }
        public double pi { get; set; }
        public int gaswater_input_id { get; set; }
        public int well_id { get; set; }
        public string gaswater_result_index { get; set; }

        public GasWaterTwoModel(double rhowsc, double rhogsc, double s, double d, double re, double rw, double pi, int gaswater_input_id, int well_id, string gaswater_result_index)
        {
            this.rhowsc = rhowsc;
            this.rhogsc = rhogsc;
            this.s = s;
            this.d = d;
            this.re = re;
            this.rw = rw;
            this.pi = pi;
            this.gaswater_input_id = gaswater_input_id;
            this.well_id = well_id;
            this.gaswater_result_index = gaswater_result_index;
        }

        public GasWaterTwoModel(){

        }
    }
}
