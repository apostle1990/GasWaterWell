using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasWaterWell.Model
{
    class PVTModel
    {
        //这个模型用来打开PVT项目时使用,水气石打开通用的model

        public string pvtName { get; set; }
        public string pvtTime { get; set; }

        public PVTModel(string pvtName, string pvtTime)
        {
            this.pvtName = pvtName ?? throw new ArgumentNullException(nameof(pvtName));
            this.pvtTime = pvtTime ?? throw new ArgumentNullException(nameof(pvtTime));
        }
    }
}
