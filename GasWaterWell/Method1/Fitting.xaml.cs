using GasWaterWell.Model;
using GasWaterWell.utils;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GasWaterWell
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Fitting : Window
    {
        FittingModel fittingModel;
        string well_id;
        DataTable dt_global;

        public Fitting()
        {
            InitializeComponent();
        }

        public Fitting(string well_id)
        {
            InitializeComponent();
            this.well_id = well_id;
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("该方法失效");
            //return;
            //Utils.ImportExcel(this.InputDataGrid);
            
            dt_global = Utils.ImportCSV();
            if (dt_global == null)
            {
                return;
            }
            this.InputDataGrid.ItemsSource = dt_global.DefaultView;
            AddData_Button.IsEnabled = false;
        }

        private void Cal_Click(object sender, RoutedEventArgs e)
        {
            Cal_Button.IsEnabled = false;
            //拟合
            #region
            int inputRow = 0;
            string url, jsonText;

            if (InputDataGrid.Items.Count <= 0 ||
                Pr_TextBox.Text.Length <= 0)
            {
                MessageBox.Show("请输入数据");
                Cal_Button.IsEnabled = true;
                return;
            }

            string[] postName1 = { "pr", "q", "pwf" };//参数列表
            string[] strArr1 = new string[3];//参数列表
            strArr1[0] = Pr_TextBox.Text;
            strArr1[1] = "[";
            strArr1[2] = "[";
            List<string> QList = new List<string>();
            List<string> inputPwfList = new List<string>();

            if(dt_global == null)
            {
                DataGrid dtgShow = InputDataGrid;
                List<FittingData> list_fit = (List<FittingData>)dtgShow.ItemsSource;

                for (int i = 0; i < list_fit.Count; i++)
                {
                    string str1 = Convert.ToString(list_fit[i].Qg);
                    string str2 = Convert.ToString(list_fit[i].Pwf);
                    if (str1 != null && !String.IsNullOrWhiteSpace(str1))
                    {
                        QList.Add(str1);
                    }
                    if (str2 != null && !String.IsNullOrWhiteSpace(str2))
                    {
                        inputPwfList.Add(str2);
                        inputRow++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < dt_global.Rows.Count; i++)
                {
                    string str1 = dt_global.Rows[i]["Qg"].ToString();
                    string str2 = dt_global.Rows[i]["Pwf"].ToString();
                    if (str1 != null && !String.IsNullOrWhiteSpace(str1))
                    {
                        QList.Add(str1);
                    }
                    if (str2 != null && !String.IsNullOrWhiteSpace(str2))
                    {
                        inputPwfList.Add(str2);
                        inputRow++;
                    }
                }
            }




            var sString = String.Join(",", QList.ToArray());
            strArr1[1] += sString;
            sString = String.Join(",", inputPwfList.ToArray());
            strArr1[2] += sString;

            strArr1[1] += "]";
            strArr1[2] += "]";

            Console.WriteLine(strArr1[1]);
            Console.WriteLine(strArr1[2]);

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            for (int i = 0; i < 3; i++)
            {
                parameters.Add(postName1[i], strArr1[i]);
                //Console.WriteLine(strArr1[i]);
            }


            //发送post请求
            JObject jo1 = new JObject();
            try
            {
                url = "http://127.0.0.1:5000/prcalc";
                jsonText = utils.Network.Post(url, parameters);
                jo1 = JObject.Parse(jsonText);
            }
            catch (Exception ex)
            {
                MessageBox.Show("网络连接出错" + ex, "错误");
                Cal_Button.IsEnabled = true;
                return;
            }
            
            if (jo1.Count == 0 || jo1["error"].ToString() == "1")
            {
                MessageBox.Show("数据出错，请点击右上角重启服务。", "错误");

                Cal_Button.IsEnabled = true;
                return;
            }

            A_TextBox.Text = jo1["a"].ToString();
            B_TextBox.Text = jo1["b"].ToString();

            #endregion

            //计算ipr
            #region

            string[] postName2 = { "pr", "a", "b" };//参数列表
            string[] strArr2 = new string[4];//参数列表
            strArr2[0] = Pr_TextBox.Text;
            strArr2[1] = A_TextBox.Text;
            strArr2[2] = B_TextBox.Text;
            parameters = new Dictionary<string, string>();
            for (int i = 0; i < 3; i++)
            {
                parameters.Add(postName2[i], strArr2[i]);
            }

            //发送post请求
            JObject jo2 = new JObject();
            try
            {
                url = "http://127.0.0.1:5000/iprcalc";
                jsonText = utils.Network.Post(url, parameters);
                jo2 = JObject.Parse(jsonText);
            }
            catch (Exception ex)
            {
                MessageBox.Show("网络连接出错" + ex, "错误");
                Cal_Button.IsEnabled = true;
                return;
            }



            if (jo2["error"].ToString() == "1")
            {
                MessageBox.Show("数据出错，请点击右上角重启服务。");
                Cal_Button.IsEnabled = true;
                return;
            }
            Cal_Button.IsEnabled = true;


            #endregion

            //写入DataGrid
            #region
            //Console.WriteLine(jo2["pwf"].ToString());
            List<string> pdQgList = jo1["pd_qg"].ToString().Split(',').ToList();
            List<string> QgList = jo2["Qg"].ToString().Split(',').ToList();
            List<string> outputPwfList = jo2["pwf"].ToString().Split(',').ToList();

            //Console.WriteLine(QgList.Count);
            //Console.WriteLine(outputPwfList.Count);
            //System.Diagnostics.Debug.Assert(pdQgList.Count == QList.Count);
            System.Diagnostics.Debug.Assert(QgList.Count == outputPwfList.Count);


            DataTable dt= new DataTable();

            //第一部分数据
            dt.Columns.Add(new DataColumn("Pr（Mpa）"));
            dt.Columns.Add(new DataColumn("a"));
            dt.Columns.Add(new DataColumn("b"));
            dt.Columns.Add(new DataColumn("Qaof"));
            dt.Columns.Add(new DataColumn("R2"));

            DataRow dr;
            dr = dt.NewRow();
            dr[0] = Pr_TextBox.Text;
            dr[1] = Utils.FormatString(jo1["a"].ToString(), 0);
            dr[2] = Utils.FormatString(jo1["b"].ToString(), 0);
            dr[3] = Utils.FormatString(jo1["q_aof"].ToString(), 0);
            dr[4] = Utils.FormatString(jo1["r2"].ToString(), 0);
            dt.Rows.Add(dr);

            //第二部分数据
            dr = dt.NewRow();
            dr[0] = "Q（万方）";
            dr[1] = "Pwf";
            dr[2] = "Pd/qg";
            dt.Rows.Add(dr);

            //int[] all_cnt_array = new int[] { QList.Count, pdQgList.Count, outputPwfList.Count };
            //int all_cnt = all_cnt_array.Min();

            for (int i = 0; i < pdQgList.Count; i++)
            {
                dr = dt.NewRow();
                dr[0] = QList[i];
                dr[1] = inputPwfList[i];
                dr[2] = Utils.FormatString(pdQgList[i].Trim('[', ']', ' '), 0);
                dt.Rows.Add(dr);
            }

            //第三部分数据
            dr = dt.NewRow();
            dr[0] = "Qg";
            dr[1] = "Pwf";
            dt.Rows.Add(dr);

            for (int i = 0; i < QgList.Count; i++)
            {
                dr = dt.NewRow();
                dr[0] = Utils.FormatString(QgList[i].Trim('[', ']', ' '), 0);
                dr[1] = Utils.FormatString(outputPwfList[i].Trim('[', ']', ' '), 0);
                dt.Rows.Add(dr);
            }

            OutputDataGrid.ItemsSource = dt.DefaultView;

            #endregion

            //TODO:弹出图片
            string pathname1 = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "pic/" + jo1["pic_name"].ToString();
            //string pathname1 = "F:\\c#\\waterwell\\WindowsFormsApp1\\WindowsFormsApp1\\python\\pic\\" + jo1["pic_name"].ToString();
            string pathname2 = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "pic/" + jo2["pic_name"].ToString();
            //string pathname2 = "F:\\c#\\waterwell\\WindowsFormsApp1\\WindowsFormsApp1\\python\\pic\\" + jo2["pic_name"].ToString();

            Picture pic = new Picture(pathname1);
            pic.Show();
            pic = new Picture(pathname2);
            pic.Show();

            //写入模型
            //TODO:这里获取井ID
            string fitting_id = Utils.GetUnixTimeStamp();
            fittingModel = new FittingModel(
                fitting_id,
                well_id, 
                Pr_TextBox.Text, 
                jo1["a"].ToString(), 
                jo1["b"].ToString(),                       
                jo1["q_aof"].ToString(), 
                jo1["r2"].ToString(),
                pathname1,
                pathname2,
                QList, 
                inputPwfList,
                pdQgList,
                outputPwfList, 
                QgList
                );

            Output_Button.IsEnabled = true;
            Save_Button.IsEnabled = true;
        }

        private void Output_Button_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dg1 = OutputDataGrid;
            DataGrid dg2 = InputDataGrid;
            List<DataGrid> dgvs = new List<DataGrid>();
            dgvs.Add(dg1);
            dgvs.Add(dg2);
            string[] names = { "输入值", "结果" };
            if (Output_ComboBox.SelectionBoxItem.ToString() == "导出Excel")
            {
                if (Utils.ExportExcel(dgvs, names))
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
                //Utils.ExprotCsv(dgvs);
                MessageBox.Show("导出完成！");
            }
        }

        public Action<string,string> GetFittingId;//之前的定义委托和定义事件由这一句话代替
        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"REPLACE INTO fitting (fitting_id,well_id,pr,a,b,Qaof,r2,Q_wf,fitting_pwf_input,pd_qg,fitting_pwf_ouput,fitting_qg,pic_path_1,pic_path_2)
                           VALUES(@fitting_id,@well_id,@pr,@a,@b,@Qaof,@r2,@Q_wf,@fitting_pwf_input,@pd_qg,@fitting_pwf_ouput,@fitting_qg,@pic_path_1,@pic_path_2); ";
            List<MySqlParameter> Paramter = new List<MySqlParameter>
            {
                new MySqlParameter("@fitting_id", fittingModel.fitting_id),
                new MySqlParameter("@well_id", fittingModel.well_id),
                new MySqlParameter("@pr", fittingModel.pr),
                new MySqlParameter("@a", fittingModel.a),
                new MySqlParameter("@b", fittingModel.b),
                new MySqlParameter("@Qaof", fittingModel.qaof),
                new MySqlParameter("@r2", fittingModel.r2),
                new MySqlParameter("@pic_path_1", fittingModel.pic_path_1),
                new MySqlParameter("@pic_path_2", fittingModel.pic_path_2),
                new MySqlParameter("@Q_wf", string.Join(", ", fittingModel.qwf.ToArray())),
                new MySqlParameter("@fitting_pwf_input", string.Join(", ", fittingModel.inputPwfList.ToArray())),
                new MySqlParameter("@pd_qg", string.Join(", ", fittingModel.pdQgList.ToArray())),
                new MySqlParameter("@fitting_pwf_ouput", string.Join(", ", fittingModel.outputPwfList.ToArray())),
                new MySqlParameter("@fitting_qg", string.Join(", ", fittingModel.qgList.ToArray()))
            };
            int result = DbManager.Ins.ExecuteNonquery(sql, Paramter.ToArray());
            if (result == 0)
            {
                MessageBox.Show("存储失败！", "失败");
                return;
            }
            this.Save_Button.IsEnabled = false;
            if (GetFittingId != null)//判断事件是否为空
            {
                Next_Button.IsEnabled = true;
            }
            if (GetFittingId != null)//判断事件是否为空
            {
                GetFittingId(fittingModel.fitting_id, null);//执行委托实例

            }

            //MessageBox.Show("存储成功！", "成功");
        }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {
            string pred_id = Utils.GetUnixTimeStamp();
            Predict predict = new Predict(fittingModel.fitting_id, well_id, pred_id);
            predict.GetPredictId += (fitId,predId) =>
            {
                if (GetFittingId != null)//判断事件是否为空
                {
                    GetFittingId(fittingModel.fitting_id, pred_id);//执行委托实例

                }
            };
            
            predict.Show();
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }

        private void AddData_Button_Click(object sender, RoutedEventArgs e)
        {
            //获取到之前的data
            DataGrid dtgShow = InputDataGrid;
            List<FittingData> list_fit;
            if (dtgShow.ItemsSource != null)
            {
                list_fit = (List<FittingData>)dtgShow.ItemsSource;
            }
            else
            {
                list_fit = new List<FittingData>();
            }

            list_fit.Add(new FittingData(0, 0));

            //ObservableCollection<int[]> showdata = new ObservableCollection<int[]>();

            dtgShow.ItemsSource = null;
            dtgShow.ItemsSource = list_fit;
            
        }

        class FittingData
        {
            public double Qg { set; get; }
            public double Pwf { set; get; }

            public FittingData(double qg, double pwf)
            {
                Qg = qg;
                Pwf = pwf;
            }
        }
    }
    
}
