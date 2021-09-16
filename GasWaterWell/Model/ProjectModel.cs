using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasWaterWell.Model
{
    class ProjectModel
    {
        public string projectName { get; set; }
        public string projectId { get; set; }
        public string projectTime { get; set; }

        public ProjectModel(string projectName, string projectTime, string projectId)
        {
            this.projectName = projectName;
            this.projectId = projectId;
            this.projectTime = projectTime;
        }

        public ProjectModel(string projectName, string projectTime)
        {
            this.projectName = projectName;
            this.projectTime = projectTime;
        }

        public ProjectModel()
        {
        }
    }
}
