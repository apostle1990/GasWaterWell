using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasWaterWell.Model
{
    class WellModel
    {
        public int well_id { get; set; }
        public int xmlbbm { get; set; }
        public int qtflbm { get; set; }
        public int xmlxbm { get; set; }
        public int ytflbm { get; set; }
        public string xmmc { get; set; }
        public string xmdm { get; set; }
        public string szd { get; set; }
        public string gk { get; set; }
        public int project_id { get; set; }
        public string pvt_gas_name { get; set; }
        public string pvt_water_name { get; set; }
        public string pvt_rock_name { get; set; }
        public string phase_seepage_name { get; set; }

        public WellModel(int well_id, int xmlbbm, int qtflbm, int xmlxbm, int ytflbm, string xmmc, string xmdm,
                         string szd, string gk, int project_id, string pvt_gas_name, string pvt_water_name,
                         string pvt_rock_name, string phase_seepage_name)
        {
            this.well_id = well_id;
            this.xmlbbm = xmlbbm;
            this.qtflbm = qtflbm;
            this.xmlxbm = xmlxbm;
            this.ytflbm = ytflbm;
            this.xmmc = xmmc;
            this.xmdm = xmdm;
            this.szd = szd;
            this.gk = gk;
            this.project_id = project_id;
            this.pvt_gas_name = pvt_gas_name;
            this.pvt_water_name = pvt_water_name;
            this.pvt_rock_name = pvt_rock_name;
            this.phase_seepage_name = phase_seepage_name;
        }

        public WellModel(int well_id, int xmlbbm, int qtflbm, int xmlxbm, int ytflbm, string xmmc, string xmdm,
                         string szd, string gk, int project_id, string pvt_gas_name)
        {
            this.well_id = well_id;
            this.xmlbbm = xmlbbm;
            this.qtflbm = qtflbm;
            this.xmlxbm = xmlxbm;
            this.ytflbm = ytflbm;
            this.xmmc = xmmc;
            this.xmdm = xmdm;
            this.szd = szd;
            this.gk = gk;
            this.project_id = project_id;
            this.pvt_gas_name = pvt_gas_name;
        }


        public WellModel(int well_id, string pvt_gas_name, string pvt_water_name, string pvt_rock_name, string phase_seepage_name)
        {
            this.well_id = well_id;
            this.pvt_gas_name = pvt_gas_name;
            this.pvt_water_name = pvt_water_name;
            this.pvt_rock_name = pvt_rock_name;
            this.phase_seepage_name = phase_seepage_name;
        }

        public WellModel()
        {
        }
    }
}
