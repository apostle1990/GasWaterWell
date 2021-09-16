using GasWaterWell.Model;
using GasWaterWell.utils;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GasWaterWell.Method2
{
    /// <summary>
    /// PressureSquare.xaml 的交互逻辑
    /// </summary>
    public partial class PressureSquare : Window
    {
        MySqlConnection conn = new MySqlConnection(Const.DBSTR);
        GasModel gasModel = new GasModel();
        private static int gas_product_index = -1;
        int well_id = 1571542044;

        public PressureSquare()
        {
            InitializeComponent();
        }

        public PressureSquare(int wellId)
        {
            InitializeComponent();
            this.well_id = wellId;
        }

        private void Product_Click(object sender, RoutedEventArgs e)
        {
            gas_product_index = int.Parse(Utils.GetUnixTimeStamp());
            DataTable dtGrid = Utils.ImportCSV();

            if (dtGrid == null)
            {
                return;
            }

            bool resultFlag = Save_Product_Direct(dtGrid);
            if (resultFlag)
            {
                MessageBox.Show("存储生产数据完成！", "成功");
            }
            else
            {
                MessageBox.Show("存储生产数据失败！", "失败");
            }

            gasModel.gas_product_index = gas_product_index;
            Cal_Button.IsEnabled = true;
        }

        private bool Save_Product_Direct(DataTable dtGrid)
        {
            //DataTable dtGrid = new DataTable();
            //dtGrid = (productDataGrid.ItemsSource as DataView).ToTable();

            if (dtGrid == null)
            {
                return false;
            }

            try
            {
                DataTable dt = dtGrid.Copy();

                //更改列名，对应数据库中的列名
                IDictionary<string, string> ColumnsDict = new Dictionary<string, string>()
                {
                    {"日期", "date"},
                    {"产气量", "qg"},
                    {"井底流压", "pwf"},
                    {"地层压力", "pe"},
                };
                Utils.ChangeColumnsToRight(dt, ColumnsDict);

                Console.WriteLine("---------" + gas_product_index + "---------");

                string sql = string.Format("SELECT * FROM gas_production_data " +
                             "where gas_product_index = {0}",
                             gas_product_index);

                //给Datatable插入key列
                DataColumn dc = new DataColumn("gas_product_index", typeof(string));
                dc.DefaultValue = gas_product_index;
                dt.Columns.Add(dc);

                dc = new DataColumn("well_id", typeof(string));
                dc.DefaultValue = well_id;
                dt.Columns.Add(dc);

                DateTime now = DateTime.Now;
                dc = new DataColumn("date", typeof(DateTime));
                dc.DefaultValue = now;
                dt.Columns.Add(dc);


                string id = well_id.ToString();
                WellParamsModel wellParamsModel = DbRead.GetWellParams(id);
                double pe = wellParamsModel.pe;
                dc = new DataColumn("pe", typeof(double));
                dc.DefaultValue = pe;
                dt.Columns.Add(dc);

                //执行批量插入更新
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                _ = adapter.Update(dt);
                adapter.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save_Product错误\n" + ex, "错误");
                return false;
            }

            
        }

        /// <summary>
        /// 解决特殊字符导致列名出问题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string columnName = e.PropertyName;
            if (e.Column is DataGridBoundColumn &&
              (columnName.Contains(".") ||
              columnName.Contains("\\") ||
              columnName.Contains("/") ||
              columnName.Contains("[") ||
              columnName.Contains("]") ||
              columnName.Contains("(") ||
              columnName.Contains(")") ||
              columnName.Contains("·") ||
              columnName.Contains(" ")))
            {
                DataGridBoundColumn dataGridBoundColumn = e.Column as DataGridBoundColumn;
                dataGridBoundColumn.Binding = new Binding("[" + e.PropertyName + "]");

            }
        }

        private void Cal_Click(object sender, RoutedEventArgs e)
        {
            Cal_Button.IsEnabled = false;
            string url, jsonText;

            DbManager.Ins.ConnStr = Const.DBSTR;
            //封装参数
            #region
            string[] postName = { "user", "pwd", "ip", "dbname", "gas_product_index", "well_id" };//参数列表
            string[] strArr = new string[6];//参数列表
            strArr[0] = Const.USER;
            strArr[1] = Const.PWD;
            strArr[2] = Const.IP;
            strArr[3] = Const.DBNAME;
            strArr[4] = gas_product_index.ToString();
            strArr[5] = well_id.ToString();

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            for (int i = 0; i < postName.Length; i++)
            {
                parameters.Add(postName[i], strArr[i]);
                Console.WriteLine(strArr[i]);
            }
            #endregion

            //发送post请求
            JObject jo = new JObject();
            //try
            //{
            url = "http://127.0.0.1:5000/gas_ps";
            jsonText = utils.Network.Post(url, parameters);
            Console.WriteLine("jsonText: " + jsonText);
            jo = JObject.Parse(jsonText);

            if (jo["error"].ToString() == "1")
            {
                MessageBox.Show("数据出错，请点击右上角重启服务。", "错误");

                Cal_Button.IsEnabled = true;
                return;
            }

            int gas_result_index = Convert.ToInt32(jo["gas_result_index"].ToString());

            gasModel.gas_result_index = gas_result_index;

            string sql = string.Format("SELECT * FROM gas_result_data " +
                             "where gas_result_index = {0} and gas_product_index = {1}",
                             gas_result_index, gas_product_index);

            DataTable dt = DbManager.Ins.ExcuteDataTable(sql);
            dt.Columns.Remove("gas_result_id");
            dt.Columns.Remove("gas_result_index");
            dt.Columns.Remove("well_id");
            dt.Columns.Remove("gas_product_index");
            dt.Columns.Remove("pic_path_1");
            dt.Columns.Remove("pic_path_2");
            dt.Columns.Remove("type");
            dt.Columns["date"].ColumnName = "日期";
            dt.Columns["qaof"].ColumnName = "无阻流量/104m3";
            dt.Columns["kh"].ColumnName = "KH/(mD.m)";
            dt.Columns["a"].ColumnName = "A";
            dt.Columns["b"].ColumnName = "B";

            OutputDataGrid.ItemsSource = dt.DefaultView;

            //TODO:弹出图片
            string pathname1 = System.AppDomain.CurrentDomain.BaseDirectory + "pic\\" + jo["pic_name"].ToString();
            //string pathname1 = "E:\\Project\\GasWaterWell\\waterwell\\WindowsFormsApp1\\WindowsFormsApp1\\python\\pic\\" + jo["pic_name_1"].ToString();
            //string pathname2 = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "pic/" + jo["pic_name_2"].ToString();
            //string pathname2 = "E:\\Project\\GasWaterWell\\waterwell\\WindowsFormsApp1\\WindowsFormsApp1\\python\\pic\\" + jo["pic_name_2"].ToString();

            Picture pic = new Picture(pathname1);
            pic.Show();

            //pic = new Picture(pathname2);
            //pic.Show();

            Cal_Button.IsEnabled = true;
            Save_Button.IsEnabled = true;
            Output_Button.IsEnabled = true;
        }

        private void Output_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dg = OutputDataGrid;
            DataTable outputDt = (dg.ItemsSource as DataView).ToTable(); // 将结果转为DT

            //获取井Model
            WellModel wellModel = DbRead.GetWellModelByWellId(this.well_id.ToString());

            // 获取井参数DT
            DataTable WellParmasDt = DbRead.GetWellParmasTableByWellId(this.well_id.ToString());
            // 获取气水相渗
            //DataTable PhaseSeepageDt = DbRead.GetPhaseSeepageByWellid(this.well_id.ToString());
            //获取PvtGas
            DataTable PvtGasDt = DbRead.GetPVTGasTable(wellModel.pvt_gas_name);

            List<DataTable> FinalDataTableList = new List<DataTable>();
            FinalDataTableList.Add(WellParmasDt);
            FinalDataTableList.Add(PvtGasDt);
            FinalDataTableList.Add(outputDt);
            string[] names = { "井参数", "PvtGas", "结果" };


            if (Output_ComboBox.SelectionBoxItem.ToString() == "导出Excel")
            {
                if (Utils.ExportExcel(FinalDataTableList, names))
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

        public Action<string, string> GetInputIdAndResultIndex;//之前的定义委托和定义事件由这一句话代替
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (GetInputIdAndResultIndex != null)//判断事件是否为空
            {
                GetInputIdAndResultIndex(gasModel.gas_product_index.ToString(), gasModel.gas_result_index.ToString());//执行委托实例  
                this.Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
