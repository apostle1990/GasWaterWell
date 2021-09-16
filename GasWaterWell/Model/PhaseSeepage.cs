using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasWaterWell.Model
{
    class PhaseSeepage
    {
        //气水相渗
        public int PhaseSeepageId { get; set; }
        public int PhaseSeepageIndex { get; set; }
        public string PhaseSeepageDate { get; set; }
        public double sw { get; set; }
        public double krg { get; set; }
        public double krw { get; set; }
        public string PhaseSeepageName { get; set; }
        public int ProjectId { get; set; }
        public PhaseSeepage() { }

        public PhaseSeepage(int phaseSeepageId, int phaseSeepageIndex, double sw, double krg, double krw, string phaseSeepageName,int  projectId)
        {
            PhaseSeepageId = phaseSeepageId;
            PhaseSeepageIndex = phaseSeepageIndex;
            this.sw = sw;
            this.krg = krg;
            this.krw = krw;
            PhaseSeepageId = phaseSeepageId;
            PhaseSeepageName = phaseSeepageName ?? throw new ArgumentNullException(nameof(phaseSeepageName));
        }
        public PhaseSeepage(int phaseSeepageIndex,string phaseSeepageName)
        {
            PhaseSeepageIndex = phaseSeepageIndex;
            PhaseSeepageName = phaseSeepageName;
        }
        public PhaseSeepage(string phaseSeepageName, string PhaseSeepagedate, int phaseSeepageIndex)
        {
            PhaseSeepageDate = PhaseSeepagedate;
            PhaseSeepageName = phaseSeepageName;
            PhaseSeepageIndex = phaseSeepageIndex;
        }
    }
}
