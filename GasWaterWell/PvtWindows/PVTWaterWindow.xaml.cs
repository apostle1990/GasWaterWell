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
    /// PVTWater.xaml 的交互逻辑
    /// </summary>
    public partial class PVTWater : Window
    {
        ObservableCollection<PVTWaterModel> pgm = new ObservableCollection<PVTWaterModel>();
        public PVTWater()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 计算Water
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_calc(object sender, RoutedEventArgs e)
        {
            string tStr = this.T.Text;
            string NaClStr = this.NaCl.Text;
            string PaMaxStr = this.PaMax.Text;
            string PaMinStr = this.PaMin.Text;
            string PaSizeStr = this.PaSize.Text;

            //检查空值异常值
            TextBox[] txt = { this.T, this.NaCl, this.PaMax, this.PaMin, this.PaSize };
            if (Utils.IsTextBoxEmpty(txt))
            {
                return;
            }
            if (!Utils.IsDigit(txt))
            {
                return;
            }

            PvtGas pvtg = new PvtGas();
            PvtWater pvtw = new PvtWater();
            double t = System.Convert.ToDouble(tStr) + 273.15;
            double nacl = System.Convert.ToDouble(NaClStr);
            double PaMax = System.Convert.ToDouble(PaMaxStr);
            double PaMin = System.Convert.ToDouble(PaMinStr);
            double PaSize = System.Convert.ToDouble(PaSizeStr);
            int length = Convert.ToInt32((PaMax - PaMin) / PaSize);

            DataGrid dataGrid = this.dataGrid;

            pgm.Clear();
            double p = PaMin;
            for (int i = 0; i < length + 1; i++)
            {
                double Bw = pvtw.WATERPVTBW(p, t, nacl);
                double pw = pvtw.WATERPVTDEN(p, t, nacl);
                double uw = pvtw.WATERPVTVIS(p, t, nacl);
                double Cw = pvtw.WATERPVTCW(p, t, nacl);
                double Rsw = pvtw.WATERPVTRS(p, t, nacl);
                double Rcwg = pvtg.GASPVTCWGR(p, t, nacl);

                pgm.Add(new PVTWaterModel(p, Bw, pw, uw, Cw, Rsw, Rcwg));
                p += PaSize;
                Console.WriteLine(Bw + " " + pw + " " + uw + " " + Cw + " " + Rsw + " " + Rcwg + " ");
            }
            dataGrid.DataContext = pgm;
            save_btn.IsEnabled = true;
            export_btn.IsEnabled = true;
        }

        /// <summary>
        /// 导入csv数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_import(object sender, RoutedEventArgs e)
        {
            //导入excel数据
            DataTable dt = utils.Utils.ImportCSV();
            if (dt == null)
            {
                return;
            }
            DataRow drOperate = dt.Rows[0];

            this.T.Text = drOperate[0].ToString();
            this.NaCl.Text = drOperate[1].ToString();
            this.PaMax.Text = drOperate[2].ToString();
            this.PaMin.Text = drOperate[3].ToString();
            this.PaSize.Text = drOperate[4].ToString();
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_export(object sender, RoutedEventArgs e)
        {
            DataGrid dg = dataGrid;
            if(output_combox.SelectionBoxItem.ToString() == "导出到Excel")
            {
                utils.Utils.ExportExcel(dg);
                MessageBox.Show("导出完成");
            }
            else if(output_combox.SelectionBoxItem.ToString() == "导出到CSV")
            {
                utils.Utils.ExprotCsv(dg);
                MessageBox.Show("导出完成");
            }
        }


        public Action<string> GetWaterPVTName;//之前的定义委托和定义事件由这一句话代替
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_save(object sender, RoutedEventArgs e)
        {
            //这里可能有项目编码
            string project_id = OperateXML.GetProjectId();
            string waterName = this.WaterName.Text + ".wpvt";
            List<PVTWaterModel> relt = DbRead.GetPVTWater(waterName);
            if (relt!= null&&relt.Count>0)
            {
                MessageBox.Show("PVT名称已存在，请重新命名！");
                return;
            }
            double nacl = Convert.ToDouble(this.NaCl.Text);
            double t = Convert.ToDouble(this.T.Text);

            TextBox[] txt = { this.T, this.NaCl, this.PaMax, this.PaMin, this.PaSize };
            if (!Utils.IsDigit(txt))
            {
                return;
            }
            if (Utils.IsTextBoxEmpty(txt))
            {
                return;
            }
            TextBox[] txt1 = { this.NaCl, this.PaMax, this.PaMin, this.PaSize };
            if (!Utils.IsDigit(txt1))
            {
                return;
            }
            //判断name是否重复
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sqlRe = "select * from pvt_water WHERE water_name = @water_name";
            List<MySqlParameter> Pa = new List<MySqlParameter> {
                   new MySqlParameter("@water_name", waterName.ToString())
            };
            DataTable dt = DbManager.Ins.ExcuteDataTable(sqlRe, Pa.ToArray());
            if (dt.Rows.Count != 0)
            {
                MessageBox.Show("项目名称已经存在，请重新命名");
                return;
            }

            DbManager.Ins.ConnStr = Const.DBSTR;


            string pvt_water_index = Utils.GetUnixTimeStamp();
            foreach (PVTWaterModel model in pgm)
            {
                //model 判定异常计算值

                int result = 0;
                string sql = @"REPLACE INTO pvt_water (project_id,water_name,Pa,K,NaCl,Bw,pw,uw,Cw,Rsw,Rcwg,pvt_water_index)
                           VALUES(@project_id,@water_name,@Pa,@K,@NaCl,@Bw,@pw,@uw,@Cw,@Rsw,@Rcwg,@pvt_water_index); ";
                List<MySqlParameter> Paramter = new List<MySqlParameter>
                {
                    new MySqlParameter("@project_id",project_id),
                    new MySqlParameter("@water_name", waterName.ToString()),
                    new MySqlParameter("@Pa", model.p),
                    new MySqlParameter("@K", t),
                    new MySqlParameter("@NaCl", nacl),
                    new MySqlParameter("@Bw", model.Bw),
                    new MySqlParameter("@pw", model.pw),
                    new MySqlParameter("@uw", model.uw),
                    new MySqlParameter("@Cw", model.Cw),
                    new MySqlParameter("@Rsw", model.Rsw),
                    new MySqlParameter("@Rcwg", model.Rcwg),
                    new MySqlParameter("@pvt_water_index", pvt_water_index)
                };
                result = DbManager.Ins.ExecuteNonquery(sql, Paramter.ToArray());
                if (result == 0)
                {
                    MessageBox.Show("存储失败！");
                    return;
                }
            }
            MessageBox.Show("保存成功！");
            if (GetWaterPVTName != null)//判断事件是否为空
            {
                GetWaterPVTName(waterName.ToString());//执行委托实例  
                this.Close();
            }
            return;
        }



    }
}
