using GasWaterWell.Model;
using GasWaterWell.utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GasWaterWell
{
    /// <summary>
    /// PVTGas.xaml 的交互逻辑
    /// </summary>
    public partial class PVTGas : Window
    {

        ObservableCollection<PVTGasModel> pgm = new ObservableCollection<PVTGasModel>();
        public PVTGas()
        {
            InitializeComponent();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// 导入csv数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_import(object sender, RoutedEventArgs e)
        {
            //导入csv数据
            DataTable dt = utils.Utils.ImportCSV();
            if(dt == null)
            {
                return;
            }
            DataRow drOperate = dt.Rows[0];
            if (drOperate[0].ToString().ToLower() == "true")
            {
                this.GasGan.IsChecked = true;
            }
            else
            {
                this.GasNing.IsChecked = true;
            }
            this.yg.Text = drOperate[1].ToString();
            this.YCO2.Text = drOperate[2].ToString();
            this.YH2S.Text = drOperate[3].ToString();
            this.T.Text = drOperate[4].ToString();
            this.NaCl.Text = drOperate[5].ToString();
            this.GasMax.Text = drOperate[6].ToString();
            this.GasMin.Text = drOperate[7].ToString();
            this.GasSize.Text = drOperate[8].ToString();
            this.N2.Text = drOperate[9].ToString();
        }

        /// <summary>
        /// 计算PVT气
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_calc(object sender, RoutedEventArgs e)
        {
            //点击后计算按钮变成不可点击
            Button calc_btn = this.calc_btn;
            calc_btn.IsEnabled = false;

            string YCO2Str = this.YCO2.Text;
            string ygStr = this.yg.Text;
            string YH2SStr = this.YH2S.Text;
            string YN2Str = this.N2.Text;
            string NaClStr = this.NaCl.Text;
            string TStr = this.T.Text;
            string GasMaxStr = this.GasMax.Text;
            string GasMinStr = this.GasMin.Text;
            string GasSizeStr = this.GasSize.Text;
            
            //判断空值
            TextBox[] txt = { this.GasName, this.YCO2, this.yg, this.YH2S, this.N2, this.NaCl, this.T, this.GasMax, this.GasMin, this.GasSize };
            if (Utils.IsTextBoxEmpty(txt))
            {
                calc_btn.IsEnabled = true;
                return;
            }
            TextBox[] txt1 = { this.YCO2, this.yg, this.YH2S, this.N2, this.NaCl, this.T, this.GasMax, this.GasMin, this.GasSize };
            if (!Utils.IsDigit(txt1))
            {
                calc_btn.IsEnabled = true;
                return;
            }

            PvtGas pvtg = new PvtGas();
            PvtWater pvtw = new PvtWater();

            bool IsDryGas = this.GasGan.IsChecked == true;//感觉这里有问题
            double yg = Convert.ToDouble(ygStr);
            double yco2 = Convert.ToDouble(YCO2Str) / 100;
            double yh2s = Convert.ToDouble(YH2SStr) / 100; //为啥要除100
            double yn2 = Convert.ToDouble(YN2Str) / 100;
            double t = Convert.ToDouble(TStr) + 273.15;
            double nacl = Convert.ToDouble(NaClStr);
            double gasMax = Convert.ToDouble(GasMaxStr) * 1000000;
            double GasMin = Convert.ToDouble(GasMinStr) * 1000000;
            double GasSize = Convert.ToDouble(GasSizeStr) * 1000000;
            int length = Convert.ToInt32((gasMax - GasMin) / GasSize);

            DataGrid dataGrid = this.dataGrid;

            pgm.Clear();
            double p = GasMin;
            for (int i = 0; i < length + 1; i++)
            {
                double z1 = pvtg.GASPVTZ(IsDryGas, yg, yco2, yh2s, yn2, t, p);
                double z2 = pvtg.GASPVTZ1(IsDryGas, yg, yco2, yh2s, yn2, t, p);
                double Bg = pvtg.GASPVTBG(IsDryGas, yg, yco2, yh2s, yn2, t, p);
                double pg = pvtg.GASPVTDEN(IsDryGas, yg, yco2, yh2s, yn2, t, p);
                double ug = pvtg.GASPVTVIS(IsDryGas, yg, yco2, yh2s, yn2, t, p) * 1000; //粘度
                double Cg = pvtg.GASPVTCG(IsDryGas, yg, yco2, yh2s, yn2, t, p) * 1000000; //压缩系数
                double Pp = pvtg.GASPVTPP(IsDryGas, yg, yco2, yh2s, yn2, t, p) / 1E+12;
                double Rcwg = pvtg.GASPVTCWGR(p, t, nacl);

                //Console.WriteLine(z1 + " " + z2 + " " + Bg + " " + pg + " " + ug + " " + Cg + " " + Pp + " " + Rcwg);
                pgm.Add(new PVTGasModel(p, z1, z2, Bg, pg, ug, Cg, Pp, Rcwg));
                p += GasSize;
            }

            dataGrid.Items.Clear();

            dataGrid.DataContext = pgm;

            calc_btn.IsEnabled = true;
            save_btn.IsEnabled = true;
            export_btn.IsEnabled = true;
        }

        private void Button_Click_export(object sender, RoutedEventArgs e)
        {
            DataGrid dg = dataGrid;
            if (mycombox.SelectionBoxItem.ToString() == "导出到Excel")
            {
                //导出到excel
                //1拿到datagrid的计算值
                //2执行导出方法
                utils.Utils.ExportExcel(dg);
                MessageBox.Show("导出完成");
            }
            else if (mycombox.SelectionBoxItem.ToString() == "导出到CSV")
            {
                //导出到csv
                utils.Utils.ExprotCsv(dg);
                MessageBox.Show("导出完成");
            }

        }

        private void Button_Click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        public Action<string> GetGasPVTName;//之前的定义委托和定义事件由这一句话代替
        /// <summary>
        /// 保存到数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_save(object sender, RoutedEventArgs e)
        {
            string gasName = this.GasName.Text + ".gpvt";
            List<PVTGasModel> relt = DbRead.GetPVTGas(gasName);
            int count = relt.Count;
            if (relt!= null && count > 0)
            {
                MessageBox.Show("PVT名称已存在，请重新命名！");
                return;
            }
            //这里可能有项目编码
            string project_id = OperateXML.GetProjectId();
            
            double yg = Convert.ToDouble(this.yg.Text);
            double yco2 = Convert.ToDouble(this.YCO2.Text);
            double nacl = Convert.ToDouble(this.NaCl.Text);
            double t = Convert.ToDouble(this.T.Text);
            double yh2s = Convert.ToDouble(this.YH2S.Text);
            double yn2 = Convert.ToDouble(this.N2.Text);
            bool? gasgan = this.GasGan.IsChecked;
            //判断空值
            TextBox[] txt = { this.GasName, this.YCO2, this.yg, this.YH2S, this.N2, this.NaCl, this.T, this.GasMax, this.GasMin, this.GasSize };
            if (Utils.IsTextBoxEmpty(txt))
            {
                return;
            }
            TextBox[] txt1 = {this.YCO2, this.yg, this.YH2S, this.N2, this.NaCl, this.T, this.GasMax, this.GasMin, this.GasSize };
            if (!Utils.IsDigit(txt1))
            {
                return;
            }
            //判断name是否重复
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sqlRe = "select * from pvt_gas WHERE gas_name = @gas_name";
            List<MySqlParameter> Pa = new List<MySqlParameter> {
                   new MySqlParameter("@gas_name", gasName.ToString())
            };
            DataTable dt = DbManager.Ins.ExcuteDataTable(sqlRe, Pa.ToArray());
            if (dt.Rows.Count != 0)
            {
                MessageBox.Show("项目名称已经存在，请重新命名");
                return;
            }

            string pvt_gas_index = Utils.GetUnixTimeStamp();
            foreach (PVTGasModel model in pgm)
            {
                int result = 0;
                DbManager.Ins.ConnStr = Const.DBSTR;
                string sql = @"REPLACE INTO pvt_gas (project_id,gas_name,yg,YCO2,IsDryGas,YH2S,YN2,T,P,NaCl,z1,z2,Bg,pg,ug,Cg,Pp,Rcwg,pvt_gas_index)
                           VALUES(@project_id,@gas_name,@yg,@YCO2,@IsDryGas,@YH2S,@YN2,@T,@P,@NaCl,@z1,@z2,@Bg,@pg,@ug,@Cg,@Pp,@Rcwg,@pvt_gas_index)";
                List<MySqlParameter> Paramter = new List<MySqlParameter>
                {
                    new MySqlParameter("@project_id",project_id),
                    new MySqlParameter("@gas_name", gasName.ToString()),
                    new MySqlParameter("@yg", yg),
                    new MySqlParameter("@YCO2", yco2),
                    new MySqlParameter("@IsDryGas", gasgan.ToString()),
                    new MySqlParameter("@YH2S", yh2s),
                    new MySqlParameter("@YN2", yn2),
                    new MySqlParameter("@T", t),
                    new MySqlParameter("@P", model.p),
                    new MySqlParameter("@NaCl", nacl),
                    new MySqlParameter("@z1", model.z1),
                    new MySqlParameter("@z2", model.z2),
                    new MySqlParameter("@Bg", model.Bg),
                    new MySqlParameter("@pg", model.pg),
                    new MySqlParameter("@ug", model.ug),
                    new MySqlParameter("@Cg", model.Cg),
                    new MySqlParameter("@Pp", model.Pp),
                    new MySqlParameter("@Rcwg", model.Rcwg),
                    new MySqlParameter("@pvt_gas_index",pvt_gas_index)
                };
                result = DbManager.Ins.ExecuteNonquery(sql, Paramter.ToArray());
                if (result == 0)
                {
                    MessageBox.Show("存储失败！");
                    return;
                }
            }
            MessageBox.Show("保存成功！");
            if (GetGasPVTName != null)//判断事件是否为空
            {
                GetGasPVTName(gasName.ToString());//执行委托实例  
                this.Close();
            }
            return;
        }


    }
}
