using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasWaterWell.Model
{
    class PVTRockModel
    {
        public string project_id { get; set; }
        public double Sandstone { get; set; }
        public double Limestone { get; set; }

        public double rock { get; set; }

        public double rock_gama { get; set; }

        public double rock_pi { get; set; }

        public int rock_id { get; set; }
        public string rockName { get; set; }

        public PVTRockModel(double sandstone, double limestone)
        {
            Sandstone = sandstone;
            Limestone = limestone;
        }

        public PVTRockModel(int rock_id, string rockName)
        {
            this.rock_id = rock_id;
            this.rockName = rockName;
        }

        public PVTRockModel(string projct_id, double sandstone, double limestone, double rock, int rock_id, string rockName, string project_id)
        {
            this.project_id = projct_id ?? throw new ArgumentNullException(nameof(projct_id));
            Sandstone = sandstone;
            Limestone = limestone;
            this.rock = rock;
            this.rock_id = rock_id;
            this.rockName = rockName ?? throw new ArgumentNullException(nameof(rockName));
            this.project_id = project_id ?? throw new ArgumentNullException(nameof(project_id));
        }

        public PVTRockModel()
        {

        }

    }
}
