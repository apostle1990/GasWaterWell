using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasWaterWell.Model
{
    class GasModel
    {
        public int gas_product_index { get; set; }

        public int well_id { get; set; }

        public int gas_result_index { get; set; }


        public GasModel(int gas_product_index, int well_id, int gas_result_index)
        {
            this.gas_product_index = gas_product_index;
            this.well_id = well_id;
            this.gas_result_index = gas_result_index;
        }

        public GasModel(int gas_result_index,int gas_product_index)
        {
            this.gas_result_index = gas_result_index;
            this.gas_product_index = gas_product_index;
        }

        public GasModel()
        {

        }
    }
}
