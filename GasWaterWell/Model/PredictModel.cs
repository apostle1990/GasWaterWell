using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasWaterWell.Model
{
    class PredictModel
    {
        public string pred_id { get; set; }
        public string well_id { get; set; }
        public string pred_a_input { get; set; }
        public string pred_b_input { get; set; }
        public string pred_pr_input { get; set; }
        public string pred_qg_input { get; set; }
        public string pic_path_1 { get; set; }
        public string pic_path_2 { get; set; }
        public List<string> pi { get; set; }
        public List<string> pi_p { get; set; }
        public List<string> k_ki { get; set; }
        public List<string> u_pi { get; set; }
        public List<string> z_p { get; set; }
        public List<string> ai { get; set; }
        public List<string> bi { get; set; }
        public List<string> pred_qg_output { get; set; }
        public List<string> pred_pwf_output { get; set; }
        public string fittingId { get; set; }

        public PredictModel(string pred_id, string well_id, string pred_a_input, string pred_b_input, string pred_pr_input, string pred_qg_input, string pic_path_1, string pic_path_2, List<string> pi, List<string> pi_p, List<string> k_ki, List<string> u_pi, List<string> z_p, List<string> ai, List<string> bi, List<string> pred_qg_output, List<string> pred_pwf_output, string fittingId)
        {
            this.pred_id = pred_id;
            this.well_id = well_id;
            this.pred_a_input = pred_a_input;
            this.pred_b_input = pred_b_input;
            this.pred_pr_input = pred_pr_input;
            this.pred_qg_input = pred_qg_input;
            this.pic_path_1 = pic_path_1;
            this.pic_path_2 = pic_path_2;
            this.pi = pi;
            this.pi_p = pi_p;
            this.k_ki = k_ki;
            this.u_pi = u_pi;
            this.z_p = z_p;
            this.ai = ai;
            this.bi = bi;
            this.pred_qg_output = pred_qg_output;
            this.pred_pwf_output = pred_pwf_output;
            this.fittingId = fittingId;
        }

        public PredictModel()
        {

        }
    }
}
