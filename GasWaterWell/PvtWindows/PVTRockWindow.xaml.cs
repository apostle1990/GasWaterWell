using GasWaterWell.Model;
using GasWaterWell.utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// PVTStone.xaml 的交互逻辑
    /// </summary>
    public partial class PVTStone : Window
    {
        ObservableCollection<PVTRockModel> memberData = new ObservableCollection<PVTRockModel>();
        public PVTStone()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = this.dataGrid;
            

            //判断空值
            TextBox[] txt = { this.rock, this.rockName, this.rock_gama, this.rock_pi };
            if (Utils.IsTextBoxEmpty(txt))
            {
                calc_btn.IsEnabled = true;
                return;
            }
            TextBox[] txt1 = { this.rock, this.rock_gama, this.rock_pi };
            if (!Utils.IsDigit(txt1))
            {
                calc_btn.IsEnabled = true;
                return;
            }

            string rockNameStr = this.rockName.Text;
            double rock = System.Convert.ToDouble(this.rock.Text); // 储集层岩石平均孔隙度
            double rockGama = System.Convert.ToDouble(this.rock_gama.Text);// 渗透率模量
            double rockPi = System.Convert.ToDouble(this.rock_pi.Text);// 原始地层压力

            PvtRock pvtr = new PvtRock();

            double Sandstone = pvtr.ROCKSANDCP(rock);
            double Limestone = pvtr.ROCKLIMECP(rock);

            //Console.WriteLine(Sandstone + " " + Limestone);

            memberData.Clear();
            memberData.Add(new PVTRockModel(Sandstone, Limestone));
            dataGrid.DataContext = memberData;
            Save_Button.IsEnabled = true;
            Output_Button.IsEnabled = true;
        }


        public Action<string,string> GetRockPVTName;
        /// <summary>
        /// 保存到数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_save(object sender, RoutedEventArgs e)
        {
            string project_id = OperateXML.GetProjectId();
            string rockName = this.rockName.Text + ".rpvt";
            PVTRockModel relt = DbRead.GetPVTRock(rockName);
            if (relt.rockName != null)
            {
                MessageBox.Show("PVT名称已存在，请重新命名！");
                return;
            }

            TextBox[] txt = { this.rockName ,this.rock, this.rock_gama, this.rock_pi };
            if (Utils.IsTextBoxEmpty(txt))
            {
                return;
            }
            TextBox[] txt1 = { this.rock, this.rock_gama, this.rock_pi };
            if (!Utils.IsDigit(txt1))
            {
                return;
            }
            double rock = Convert.ToDouble(this.rock.Text);
            double rock_gama = Convert.ToDouble(this.rock_gama.Text);
            double rock_pi = Convert.ToDouble(this.rock_pi.Text);

            DbManager.Ins.ConnStr = Const.DBSTR;
            string rockId = Utils.GetUnixTimeStamp();
            foreach (PVTRockModel model in memberData)
            {
                //model判空
                
                string sql = @"REPLACE INTO pvt_rock (project_id,rock_id,rock_name,kxd,ysysxs1,ysysxs2,gama,pi)
                           VALUES(@project_id,@rock_id,@rock_name,@kxd,@ysysxs1,@ysysxs2,@rock_gama,@rock_pi); ";
                List<MySqlParameter> Paramter = new List<MySqlParameter>
                {
                    new MySqlParameter("@project_id",project_id),
                    new MySqlParameter("@rock_id", rockId),
                    new MySqlParameter("@rock_name", rockName),
                    new MySqlParameter("@kxd", rock),
                    new MySqlParameter("@ysysxs1", model.Limestone),
                    new MySqlParameter("@ysysxs2", model.Sandstone),
                    new MySqlParameter("@rock_gama", rock_gama),
                    new MySqlParameter("@rock_pi", rock_pi)
                };
                int result = DbManager.Ins.ExecuteNonquery(sql, Paramter.ToArray());
                if (result == 0)
                {
                    MessageBox.Show("存储失败！");
                    return;
                }
            }
            MessageBox.Show("保存成功！");
            if (GetRockPVTName != null)//判断事件是否为空
            {
                GetRockPVTName(rockId.ToString(),rockName.ToString());//执行委托实例  
                this.Close();
            }
            return;
        }

        private void Button_Click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 导出到文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
    }
}
