using GasWaterWell.Model;
using GasWaterWell.utils;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GasWaterWell
{
    
    /// <summary>
    /// Predict.xaml 的交互逻辑
    /// </summary>
    public partial class Predict : Window
    {
        PredictModel predictModel;
        string fitting_id = null;
        string well_id = null;
        string pred_id = null;
        public Predict()
        {
            InitializeComponent();
        }

        public Predict(string well_id)
        {
            InitializeComponent();
            this.well_id = well_id;

        }

        public Action<string, string> GetPredictId;
        public Predict(string fitting_id, string well_id, string pred_id)
        {
            InitializeComponent();
            this.fitting_id = fitting_id;
            this.well_id = well_id;
            this.pred_id = pred_id;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (fitting_id != null)
            {
                FittingModel fittingModel = DbRead.GetFitting(fitting_id);
                a_TextBox.Text = Utils.FormatString(fittingModel.a, 0);
                b_TextBox.Text = Utils.FormatString(fittingModel.b, 0);
                Pi_TextBox.Text = fittingModel.pr;
            }
        }

        private void Cal_Click(object sender, RoutedEventArgs e)
        {
            Cal_Button.IsEnabled = false;

            //预测
            #region
            string url, jsonText;

            //判断空值
            TextBox[] txt = { this.Pi_TextBox, this.a_TextBox, this.b_TextBox, this.Qg_TextBox };
            if (Utils.IsTextBoxEmpty(txt))
            {
                Cal_Button.IsEnabled = true;
                return;
            }

            string[] postName = { "pi", "a", "b", "Qg", "gama" };//参数列表
            string[] strArr = new string[5];//参数列表
            strArr[0] = Pi_TextBox.Text;
            strArr[1] = a_TextBox.Text;
            strArr[2] = b_TextBox.Text;
            strArr[3] = Qg_TextBox.Text;
            string gama = OperateDB.GetRcokGamaByWellId(this.well_id);
            if(gama == null)
            {
                MessageBox.Show("渗透率模量查询失败，井未绑定PvtRock或PvtRock被删除", "错误");
                Cal_Button.IsEnabled = true;
                return;
            }
            strArr[4] = gama;

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            for (int i = 0; i < postName.Length; i++)
            {
                parameters.Add(postName[i], strArr[i]);
            }

            //发送post请求
            JObject jo = new JObject();
            try
            {
                url = "http://127.0.0.1:5000/predict";
                jsonText = utils.Network.Post(url, parameters);
                jo = JObject.Parse(jsonText);
            }
            catch (Exception ex)
            {
                MessageBox.Show("网络连接出错" + ex, "错误");
                Cal_Button.IsEnabled = true;
                return;
            }
            if (jo["error"].ToString() == "1")
            {
                MessageBox.Show("数据出错，请点击右上角重启服务。");
                Cal_Button.IsEnabled = true;
                return;
            }
            Cal_Button.IsEnabled = true;

            // 写入DataGridViewRow
            DataTable dt = new DataTable();

            List<string> piList = jo["pi"].ToString().Split(',').ToList();
            List<string> theatPList = jo["theat_p"].ToString().Split(',').ToList();
            List<string> kKiList = jo["k_ki"].ToString().Split(',').ToList();
            List<string> miuPList = jo["miu_p"].ToString().Split(',').ToList();
            List<string> zpList = jo["z_p"].ToString().Split(',').ToList();
            List<string> aiList = jo["ai"].ToString().Split(',').ToList();
            List<string> biList = jo["bi"].ToString().Split(',').ToList();
            List<string> qaofList = jo["Qaof"].ToString().Split(',').ToList();
            List<string> thetaList = jo["theta"].ToString().Split(',').ToList();

            dt.Columns.Add(new DataColumn("Pi"));
            dt.Columns.Add(new DataColumn("Pi-P"));
            dt.Columns.Add(new DataColumn("K-Ki"));
            dt.Columns.Add(new DataColumn("μ（P）"));
            dt.Columns.Add(new DataColumn("Z(P)"));
            dt.Columns.Add(new DataColumn("Ai"));
            dt.Columns.Add(new DataColumn("Bi"));
            dt.Columns.Add(new DataColumn("Qg"));
            dt.Columns.Add(new DataColumn("Pwf"));
            dt.Columns.Add(new DataColumn("压差"));

            DataRow dr;
            for (int i = 0; i < piList.Count; i++)
            {
                dr = dt.NewRow();
                dr[0] = Utils.FormatString(piList[i].Trim('[', ']', ' '), 0);
                dr[1] = Utils.FormatString(theatPList[i].Trim('[', ']', ' '), 0);
                dr[2] = Utils.FormatString(kKiList[i].Trim('[', ']', ' '), 0);
                dr[3] = Utils.FormatString(miuPList[i].Trim('[', ']', ' '), 0);
                dr[4] = Utils.FormatString(zpList[i].Trim('[', ']', ' '), 0);
                dr[5] = Utils.FormatString(aiList[i].Trim('[', ']', ' '), 0);
                dr[6] = Utils.FormatString(biList[i].Trim('[', ']', ' '), 0);
                dr[7] = Qg_TextBox.Text;
                dr[8] = Utils.FormatString(qaofList[i].Trim('[', ']', ' '), 0);
                dr[9] = Utils.FormatString(thetaList[i].Trim('[', ']', ' '), 0);
                dt.Rows.Add(dr);
            }

            OutputDataGrid.ItemsSource = dt.DefaultView;

            #endregion

            //TODO:弹出图片
            string pathname1 = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "pic/" + jo["pic_name_1"].ToString();
            //string pathname1 = "E:\\Project\\GasWaterWell\\waterwell\\WindowsFormsApp1\\WindowsFormsApp1\\python\\pic\\" + jo["pic_name_1"].ToString();
            string pathname2 = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "pic/" + jo["pic_name_2"].ToString();
            //string pathname2 = "E:\\Project\\GasWaterWell\\waterwell\\WindowsFormsApp1\\WindowsFormsApp1\\python\\pic\\" + jo["pic_name_2"].ToString();

            Picture pic = new Picture(pathname1);
            pic.Show();
            pic = new Picture(pathname2);
            pic.Show();

            //写入模型
            //TODO:这里获取井ID
            if (pred_id == null)
            {
                pred_id = Utils.GetUnixTimeStamp();
            }
            predictModel = new PredictModel(
                pred_id,
                well_id,
                a_TextBox.Text,
                b_TextBox.Text,
                Pi_TextBox.Text,
                Qg_TextBox.Text,
                pathname1,
                pathname2,
                piList,
                theatPList,
                kKiList,
                miuPList,
                zpList,
                aiList,
                biList,
                qaofList,
                thetaList,
                fitting_id
                );
             

            Output_Button.IsEnabled = true;
            Save_Button.IsEnabled = true;

        }

        
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"REPLACE INTO prediction (pred_id,well_id,pred_a_input,pred_b_input,pred_pr_input,pred_qg_input,pi,pi_p,k_ki,u_pi,z_p,Ai,Bi,pred_qg_output,pred_pwf_output,pic_path_1,pic_path_2,fitting_id)
                           VALUES(@pred_id,@well_id,@pred_a_input,@pred_b_input,@pred_pr_input,@pred_qg_input,@pi,@pi_p,@k_ki,@u_pi,@z_p,@Ai,@Bi,@pred_qg_output,@pred_pwf_output,@pic_path_1,@pic_path_2,@fittingId); ";
            List<MySqlParameter> Paramter = new List<MySqlParameter>
            {
                new MySqlParameter("@pred_id", predictModel.pred_id),
                new MySqlParameter("@well_id", predictModel.well_id),
                new MySqlParameter("@pred_a_input", predictModel.pred_a_input),
                new MySqlParameter("@pred_b_input", predictModel.pred_b_input),
                new MySqlParameter("@pred_pr_input", predictModel.pred_pr_input),
                new MySqlParameter("@pred_qg_input", predictModel.pred_qg_input),
                new MySqlParameter("@pic_path_1", predictModel.pic_path_1),
                new MySqlParameter("@pic_path_2", predictModel.pic_path_2),
                new MySqlParameter("@pi", string.Join(", ", predictModel.pi.ToArray())),
                new MySqlParameter("@pi_p", string.Join(", ", predictModel.pi_p.ToArray())),
                new MySqlParameter("@k_ki", string.Join(", ", predictModel.k_ki.ToArray())),
                new MySqlParameter("@u_pi", string.Join(", ", predictModel.u_pi.ToArray())),
                new MySqlParameter("@z_p", string.Join(", ", predictModel.z_p.ToArray())),
                new MySqlParameter("@Ai", string.Join(", ", predictModel.ai.ToArray())),
                new MySqlParameter("@Bi", string.Join(", ", predictModel.bi.ToArray())),
                new MySqlParameter("@pred_qg_output", string.Join(", ", predictModel.pred_qg_output.ToArray())),
                new MySqlParameter("@pred_pwf_output", string.Join(", ", predictModel.pred_pwf_output.ToArray())),
                new MySqlParameter("@fittingId", predictModel.fittingId)
            };
            int result = DbManager.Ins.ExecuteNonquery(sql, Paramter.ToArray());
            if (result == 0)
            {
                MessageBox.Show("存储失败！", "失败");
                return;
            }
            if (GetPredictId != null)//判断事件是否为空
            {
                GetPredictId(fitting_id, predictModel.pred_id);//执行委托实例
            }
            this.Close();
            //MessageBox.Show("存储成功！", "成功");
        }

        private void Output_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dg = OutputDataGrid;
            if (Output_ComboBox.SelectionBoxItem.ToString() == "导出Excel")
            {
                if (Utils.ExportExcel(dg))
                {
                    MessageBox.Show("导出完成！");
                }
                else
                {
                    MessageBox.Show("导出失败！");
                }
            }
            if (Output_ComboBox.SelectionBoxItem.ToString() == "导出Csv")
            {
                Utils.ExprotCsv(dg);
                MessageBox.Show("导出完成！");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
