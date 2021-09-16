using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasWaterWell.Model
{
    public class FittingModel
    {
        public string fitting_id { get; set; }
        public string well_id { get; set; }
        public string pr { get; set; }
        public string a { get; set; }
        public string b { get; set; }
        public string qaof { get; set; }
        public string r2 { get; set; }
        public string pic_path_1 { get; set; }
        public string pic_path_2 { get; set; }
        public List<string> qwf { get; set; }
        public List<string> inputPwfList { get; set; }
        public List<string> pdQgList { get; set; }
        public List<string> outputPwfList { get; set; }
        public List<string> qgList { get; set; }

        public FittingModel(string fitting_id, string well_id, string pr, string a, string b, string qaof, string r2, string pic_path_1, string pic_path_2, List<string> qwf, List<string> inputPwfList, List<string> pdQgList, List<string> outputPwfList, List<string> qgList)
        {
            this.fitting_id = fitting_id;
            this.well_id = well_id;
            this.pr = pr;
            this.a = a;
            this.b = b;
            this.qaof = qaof;
            this.r2 = r2;
            this.pic_path_1 = pic_path_1;
            this.pic_path_2 = pic_path_2;
            this.qwf = qwf;
            this.inputPwfList = inputPwfList;
            this.pdQgList = pdQgList;
            this.outputPwfList = outputPwfList;
            this.qgList = qgList;
        }

        public FittingModel()
        {

        }
    }
}
