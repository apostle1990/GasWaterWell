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
using System.Windows.Data;


namespace GasWaterWell
{
    /// <summary>
    /// GasWaterTwoElephants.xaml 的交互逻辑
    /// </summary>
    public partial class GasWaterTwoElephants : Window
    {

        MySqlConnection conn = new MySqlConnection(Const.DBSTR);
        private static int gaswater_input_id = int.Parse(Utils.GetUnixTimeStamp());
        //private static int gaswater_input_id = 1562933456;
        private static int pressure_fluid_index = gaswater_input_id + 1;
        private static int gaswater_index = gaswater_input_id + 2;
        private static int product_index = gaswater_input_id + 3;
        int well_id = -1;

        GasWaterTwoModel gasWaterTwoModel;

        public GasWaterTwoElephants()
        {
            InitializeComponent();
        }

        public GasWaterTwoElephants(int well_id)
        {
            InitializeComponent();
            this.well_id = well_id;
        }

        private void Input_Button(object sender, RoutedEventArgs e)
        {
            //导入csv数据
            DataTable dt = utils.Utils.ImportCSV();
            if (dt == null)
            {
                return;
            }
            DataRow drOperate = dt.Rows[0];
            rhowsc_TextBox.Text = drOperate[0].ToString();
            rhogsc_TextBox.Text = drOperate[1].ToString();
            S_TextBox.Text = drOperate[2].ToString();
            D_TextBox.Text = drOperate[3].ToString();
            Re_TextBox.Text = drOperate[4].ToString();
            rw_TextBox.Text = drOperate[5].ToString();
            //Pi_TextBox.Text = drOperate[6].ToString();
        }

        private void Pressure_Button(object sender, RoutedEventArgs e)
        {
            
            DataTable dtGrid = Utils.ImportExcel(this.pressureDataGrid);
            Pressure_TabItem.IsSelected = true;
        }
            
        private void GasWater_Button(object sender, RoutedEventArgs e)
        {

            DataTable dtGrid = Utils.ImportExcel(this.gasWaterDataGrid);
            GasWater_TabItem.IsSelected = true;
            
        }

        private void Product_Button(object sender, RoutedEventArgs e)
        {
            DataTable dtGrid = Utils.ImportCSV();
            Product_TabItem.IsSelected = true;

            if (dtGrid == null)
            {
                return;
            }

            Save_Product_Direct(dtGrid);
            MessageBox.Show("存储生产数据完成！", "成功");
        }

        private void Save_Pressure()
        {
            DataTable dtGrid = new DataTable();
            dtGrid = (pressureDataGrid.ItemsSource as DataView).ToTable();

            if (dtGrid == null)
            {
                return;
            }

            try
            {
                DataTable dt = dtGrid.Copy();

                //更改列名，对应数据库中的列名
                IDictionary<string, string> ColumnsDict = new Dictionary<string, string>()
                {
                    {"压力", "pressure"},
                    {"气体密度", "gas_density"},
                    {"气体粘度", "gas_viscosity"},
                    {"水的密度", "water_density"},
                    {"水的粘度", "water_viscosity"},
                    {"气体体积系数", "gas_volume_factor"},
                    {"水的体积系数", "water_volume_factor"},
                };
                Utils.ChangeColumnsToRight(dt, ColumnsDict);

                string sql = string.Format("SELECT * FROM gaswater_pressure_fluid " +
                             "where pressure_fluid_index = {0} and gaswater_input_id = {1}",
                             pressure_fluid_index, gaswater_input_id);

                //给Datatable插入key列
                DataColumn dc = new DataColumn("pressure_fluid_index", typeof(string));
                dc.DefaultValue = pressure_fluid_index;
                dt.Columns.Add(dc);
                dc = new DataColumn("gaswater_input_id", typeof(string));
                dc.DefaultValue = gaswater_input_id;
                dt.Columns.Add(dc);

                //执行批量插入更新
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                _ = adapter.Update(dt);
                adapter.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save_Pressure错误\n" + ex, "错误");
            }
        }

        private void Save_GasWater()
        {
            DataTable dtGrid = new DataTable();
            dtGrid = (gasWaterDataGrid.ItemsSource as DataView).ToTable();

            if (dtGrid == null)
            {
                return;
            }

            try
            {
                DataTable dt = dtGrid.Copy();

                //更改列名，对应数据库中的列名
                IDictionary<string, string> ColumnsDict = new Dictionary<string, string>()
                {
                    {"Sw", "sw"},
                    {"Krg", "krg"},
                    {"Krw", "krw"},
                };
                Utils.ChangeColumnsToRight(dt, ColumnsDict);

                string sql = string.Format("SELECT * FROM gaswater_phase_seepage " +
                             "where gaswater_index = {0} and gaswater_input_id = {1}",
                             gaswater_index, gaswater_input_id);

                //给Datatable插入key列
                DataColumn dc = new DataColumn("gaswater_index", typeof(string));
                dc.DefaultValue = gaswater_index;
                dt.Columns.Add(dc);
                dc = new DataColumn("gaswater_input_id", typeof(string));
                dc.DefaultValue = gaswater_input_id;
                dt.Columns.Add(dc);

                //执行批量插入更新
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                _ = adapter.Update(dt);
                adapter.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save_GasWater错误\n" + ex, "错误");
            }
        }

        private void Save_Product()
        {
            DataTable dtGrid = new DataTable();
            dtGrid = (productDataGrid.ItemsSource as DataView).ToTable();

            if (dtGrid == null)
            {
                return;
            }

            try
            {
                DataTable dt = dtGrid.Copy();

                //更改列名，对应数据库中的列名
                IDictionary<string, string> ColumnsDict = new Dictionary<string, string>()
                {
                    {"日期", "date"},
                    {"产气量", "gas_production"},
                    {"产水量", "water_production"},
                    {"井底流压", "bottom_hole_pressure"},
                    {"地层压力", "formation_pressure"},
                    //{"累积产气量", "cumulative_gas_production"},
                };
                Utils.ChangeColumnsToRight(dt, ColumnsDict);

                Console.WriteLine("---------"+product_index+"---------");

                string sql = string.Format("SELECT * FROM gaswater_production_data " +
                             "where product_index = {0} and gaswater_input_id = {1}",
                             product_index, gaswater_input_id);

                //给Datatable插入key列
                DataColumn dc = new DataColumn("product_index", typeof(string));
                dc.DefaultValue = product_index;
                dt.Columns.Add(dc);
                dc = new DataColumn("gaswater_input_id", typeof(string));
                dc.DefaultValue = gaswater_input_id;
                dt.Columns.Add(dc);

                //执行批量插入更新
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                _ = adapter.Update(dt);
                adapter.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save_Product错误\n" + ex, "错误");
            }
        }

        private void Save_Product_Direct(DataTable dtGrid)
        {
            //DataTable dtGrid = new DataTable();
            //dtGrid = (productDataGrid.ItemsSource as DataView).ToTable();

            if (dtGrid == null)
            {
                return;
            }

            try
            {
                DataTable dt = dtGrid.Copy();

                //更改列名，对应数据库中的列名
                IDictionary<string, string> ColumnsDict = new Dictionary<string, string>()
                {
                    {"日期", "date"},
                    {"产气量", "gas_production"},
                    {"产水量", "water_production"},
                    {"井底流压", "bottom_hole_pressure"},
                    {"地层压力", "formation_pressure"},
                    //{"累积产气量", "cumulative_gas_production"},
                };
                Utils.ChangeColumnsToRight(dt, ColumnsDict);

                Console.WriteLine("---------" + product_index + "---------");

                string sql = string.Format("SELECT * FROM gaswater_production_data " +
                             "where product_index = {0} and gaswater_input_id = {1}",
                             product_index, gaswater_input_id);

                //给Datatable插入key列
                DataColumn dc = new DataColumn("product_index", typeof(string));
                dc.DefaultValue = product_index;
                dt.Columns.Add(dc);
                dc = new DataColumn("gaswater_input_id", typeof(string));
                dc.DefaultValue = gaswater_input_id;
                dt.Columns.Add(dc);

                //执行批量插入更新
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                _ = adapter.Update(dt);
                adapter.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save_Product错误\n" + ex, "错误");
            }
        }

        private void Save_Input()
        {
            double rhowsc = double.Parse(rhowsc_TextBox.Text);
            double rhogsc = double.Parse(rhogsc_TextBox.Text);
            double s = double.Parse(S_TextBox.Text);
            double d = double.Parse(D_TextBox.Text) / 10000; // 非达西渗流系数
            double re = double.Parse(Re_TextBox.Text);
            double rw = double.Parse(rw_TextBox.Text);
            double pi = 76.05;
            //TODO:井ID硬编码
            gasWaterTwoModel = new GasWaterTwoModel(
                rhowsc,
                rhogsc,
                s,
                d,
                re,
                rw,
                pi,
                gaswater_input_id,
                well_id,
                "");
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"REPLACE INTO gaswater_input (rhowsc,rhogsc,s,d,re,rw,pi,gaswater_input_id,well_id)
                           VALUES(@rhowsc,@rhogsc,@s,@d,@re,@rw,@pi,@gaswater_input_id,@well_id); ";
            List<MySqlParameter> Paramter = new List<MySqlParameter>
            {
                new MySqlParameter("@rhowsc", gasWaterTwoModel.rhowsc),
                new MySqlParameter("@rhogsc", gasWaterTwoModel.rhogsc),
                new MySqlParameter("@s", gasWaterTwoModel.s),
                new MySqlParameter("@d", gasWaterTwoModel.d),
                new MySqlParameter("@re", gasWaterTwoModel.re),
                new MySqlParameter("@rw", gasWaterTwoModel.rw),
                new MySqlParameter("@pi", gasWaterTwoModel.pi),
                new MySqlParameter("@gaswater_input_id", gasWaterTwoModel.gaswater_input_id),
                new MySqlParameter("@well_id", gasWaterTwoModel.well_id),
            };
            int result = DbManager.Ins.ExecuteNonquery(sql, Paramter.ToArray());
            if (result == 0)
            {
                MessageBox.Show("存储参数失败！", "失败");
                return;
            }
            MessageBox.Show("存储参数完成！", "成功");
        }

        private void ImportDB_Click(object sender, RoutedEventArgs e)
        {
            Cal_Button.IsEnabled = true;
            Save_Input();
            //判断空值
            TextBox[] txt = { rhowsc_TextBox, rhogsc_TextBox, S_TextBox, D_TextBox, Re_TextBox, rw_TextBox };
            if (!Utils.IsDigit(txt))
            {
                return;
            }
            if (Utils.IsTextBoxEmpty(txt))
            {
                return;
            }
            //判断DataGrid是否为空
            if (pressureDataGrid.ItemsSource == null)
            {
                //没有绑定任何数据源
                MessageBox.Show("请导入压力与流体性质关系数据", "警告");
                return;
            }

            if (gasWaterDataGrid.ItemsSource == null)
            {
                MessageBox.Show("请导入气水相渗数据", "警告");
                return;
            }

            //if (productDataGrid.ItemsSource == null)
            //{
            //    MessageBox.Show("请导入生产数据数据", "警告");
            //    return;
            //}

            
            Save_Pressure();
            MessageBox.Show("存储压力与流体性质关系完成！", "成功");
            Save_GasWater();
            MessageBox.Show("存储气水相渗完成！", "成功");
            //Save_Product();
            //MessageBox.Show("存储生产数据完成！", "成功");
            Cal_Button.IsEnabled = true;
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

            //封装参数
            #region
            string[] postName = { "rhowsc", "rhogsc", "s", "d", "re", "pi", "rw", "user", "pwd", "ip",
                "dbname", "pressure_fluid_index", "gaswater_index", "product_index", "gaswater_input_id" };//参数列表
            string[] strArr = new string[15];//参数列表
            strArr[0] = rhowsc_TextBox.Text;
            strArr[1] = rhogsc_TextBox.Text;
            strArr[2] = S_TextBox.Text;
            strArr[3] = (double.Parse(D_TextBox.Text) / 10000).ToString();
            strArr[4] = Re_TextBox.Text;
            strArr[5] = "76.05";
            strArr[6] = rw_TextBox.Text;
            strArr[7] = Const.USER;
            strArr[8] = Const.PWD;
            strArr[9] = Const.IP;
            strArr[10] = Const.DBNAME;
            strArr[11] = pressure_fluid_index.ToString();
            strArr[12] = gaswater_index.ToString();
            strArr[13] = product_index.ToString();
            strArr[14] = gaswater_input_id.ToString();
            //strArr[11] = "1562933457";
            //strArr[12] = "1562933458";
            //strArr[13] = "1562933459";
            //strArr[14] = "1562933456";

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
            url = "http://127.0.0.1:5000/gaswater";
            jsonText = utils.Network.Post(url, parameters);
            Console.WriteLine("jsonText: "+jsonText);
            jo = JObject.Parse(jsonText);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("网络连接出错" + ex, "错误");
            //    Cal_Button.IsEnabled = true;
            //    return;
            //}
            if (jo["error"].ToString() == "2")
            {
                MessageBox.Show("数据出错，请检查数据", "错误");

                Cal_Button.IsEnabled = true;
                return;
            }

            if (jo["error"].ToString() == "1")
            {
                MessageBox.Show("数据出错，请点击右上角重启服务。", "错误");

                Cal_Button.IsEnabled = true;
                return;
            }

            string gaswater_result_index = jo["gaswater_result_index"].ToString();

            gasWaterTwoModel.gaswater_result_index = gaswater_result_index;

            string sql = string.Format("SELECT * FROM gaswater_result " +
                             "where gaswater_result_index = {0} and gaswater_input_id = {1}",
                             gaswater_result_index, gaswater_input_id);

            DataTable dt = DbManager.Ins.ExcuteDataTable(sql);
            dt.Columns.Remove("gaswater_result_id");
            dt.Columns.Remove("gaswater_result_index");
            dt.Columns.Remove("gaswater_input_id");
            dt.Columns.Remove("pic_path_1");
            dt.Columns.Remove("pic_path_2");
            dt.Columns["date"].ColumnName = "日期";
            dt.Columns["unimpeded_flow_water"].ColumnName = "考虑产水无阻流量/104m3";
            dt.Columns["unimpeded_flow_no_water"].ColumnName = "不考虑产水无阻流量/104m3";
            dt.Columns["kh"].ColumnName = "KH/(mD.m)";
            dt.Columns["a"].ColumnName = "A";
            dt.Columns["b"].ColumnName = "B";

            OutputDataGrid.ItemsSource = dt.DefaultView;

            //TODO:弹出图片
            string pathname1 = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "pic/" + jo["pic_name_1"].ToString();
            //string pathname1 = "E:\\Project\\GasWaterWell\\waterwell\\WindowsFormsApp1\\WindowsFormsApp1\\python\\pic\\" + jo["pic_name_1"].ToString();
            string pathname2 = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "pic/" + jo["pic_name_2"].ToString();
            //string pathname2 = "E:\\Project\\GasWaterWell\\waterwell\\WindowsFormsApp1\\WindowsFormsApp1\\python\\pic\\" + jo["pic_name_2"].ToString();

            Picture pic = new Picture(pathname1);
            pic.Show();
            pic = new Picture(pathname2);
            pic.Show();

            Cal_Button.IsEnabled = true;
            Save_Button.IsEnabled = true;
            Output_Button.IsEnabled = true;
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

        public Action<string, string> GetInputIdAndResultIndex;//之前的定义委托和定义事件由这一句话代替
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (GetInputIdAndResultIndex != null)//判断事件是否为空
            {
                GetInputIdAndResultIndex(gasWaterTwoModel.gaswater_input_id.ToString(), gasWaterTwoModel.gaswater_result_index);//执行委托实例  
                this.Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}
