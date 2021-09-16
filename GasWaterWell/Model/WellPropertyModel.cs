using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasWaterWell.Model
{
    class WellPropertyModel
    {
        public string wellId { get ; set; }
        public string wellName { get; set; }
        public string wellCode { get; set; }
        public string wellAddress { get; set; }
        public string wellDetail { get; set; }
        public string pvtId { get; set; }
        public string pvtGasName { get; set; }
        public string pvtWaterName { get; set; }
        public string pvtRockName { get; set; }
        public WellPropertyModel()
        {

        }
        public WellPropertyModel(string wellId, string wellName, string wellCode, string wellAddress, string wellDetail,
                                 string pvtGasName, string pvtWaterName, string pvtRockName)
        {
            this.wellId = wellId;
            this.wellName = wellName;
            this.wellCode = wellCode;
            this.wellAddress = wellAddress;
            this.wellDetail = wellDetail;
            this.pvtGasName = pvtGasName;
            this.pvtWaterName = pvtWaterName;
            this.pvtRockName = pvtRockName;
        }
    }
}
