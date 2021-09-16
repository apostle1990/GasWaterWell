using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasWaterWell.Model
{
    class GasWaterModel
    {
        public int gaswater_product_index { get; set; }

        public int well_id { get; set; }

        public int gaswater_result_index { get; set; }


        public GasWaterModel(int gaswater_product_index, int well_id, int gaswater_result_index)
        {
            this.gaswater_product_index = gaswater_product_index;
            this.well_id = well_id;
            this.gaswater_result_index = gaswater_result_index;
        }

        public GasWaterModel()
        {

        }
    }
}
