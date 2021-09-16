using GasWaterWell.utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
    /// Create.xaml 的交互逻辑
    /// </summary>
    public partial class CreateWell : Window
    {
        public CreateWell()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IDictionary<string, string> gpvts = OperateXML.GetAllPVTByName(1);
            IDictionary<string, string> wpvts = OperateXML.GetAllPVTByName(2);
            IDictionary<string, string> rpvts = OperateXML.GetAllPVTByName(3);
            IDictionary<string, string> gwps = OperateXML.GetGwpsByName();

            foreach (var item in gpvts)
            {
                
                ComboBoxItem cmItem = new ComboBoxItem()
                {
                    Tag = item.Key,
                    Content = item.Value,
                };
                Pvt_Gas_ComoBox.Items.Add(cmItem);
            }
            foreach (var item in wpvts)
            {

                ComboBoxItem cmItem = new ComboBoxItem()
                {
                    Tag = item.Key,
                    Content = item.Value,
                };
                Pvt_Water_ComoBox.Items.Add(cmItem);
            }
            foreach (var item in rpvts)
            {

                ComboBoxItem cmItem = new ComboBoxItem()
                {
                    Tag = item.Key,
                    Content = item.Value,
                };
                Pvt_Rock_ComoBox.Items.Add(cmItem);
            }
            foreach (var item in gwps)
            {

                ComboBoxItem cmItem = new ComboBoxItem()
                {
                    Tag = item.Key,
                    Content = item.Value,
                };
                Krwg_ComoBox.Items.Add(cmItem);
            }
        }

        public Action<string, string, string> GetWellIdAndPvtName;//之前的定义委托和定义事件由这一句话代替

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            TextBox[] txt = { Name_TextBox };
            if (Utils.IsTextBoxEmpty(txt))
            {
                return;
            }
            

            string well_id = Utils.GetUnixTimeStamp();
            string well_name = Name_TextBox.Text;
            string code = Code_TextBox.Text;
            string place = Place_TextBox.Text;
            string about = About_TextBox.Text;
            string pvtGasName = null;
            string pvtWaterName = null;
            string pvtRockName = null;
            string gwpsName = null;

            try
            {
                pvtGasName = ((ComboBoxItem)Pvt_Gas_ComoBox.SelectedItem).Content.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("请先或创建选择GasPVT！");
                return;
            }

            try
            {
                pvtWaterName = ((ComboBoxItem)Pvt_Water_ComoBox.SelectedItem).Content.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("请先或创建选择WaterPVT！");
                return;
            }

            try
            {
                pvtRockName = ((ComboBoxItem)Pvt_Rock_ComoBox.SelectedItem).Content.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("请先或创建选择RockPVT！");
                return;
            }

            try
            {
                gwpsName = ((ComboBoxItem)Krwg_ComoBox.SelectedItem).Content.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("请先或创建选择气水相渗！");
                return;
            }

            

            DbManager.Ins.ConnStr = Const.DBSTR;
            string proId = OperateXML.GetProjectId();
            string sql = @"REPLACE INTO well (well_id,xmlbbm,qtflbm,xmlxbm,ytflbm,xmmc,xmdm,szd,gk,project_id,pvt_gas_name,pvt_water_name,pvt_rock_name, phase_seepage_name)
                           VALUES(@well_id,@xmlbbm,@qtflbm,@xmlxbm,@ytflbm,@xmmc,@xmdm,@szd,@gk,@project_id,@pvt_gas_name,@pvt_water_name,@pvt_rock_name, @phase_seepage_name); ";
            List<MySqlParameter> Paramter = new List<MySqlParameter>
            {
                new MySqlParameter("@well_id", well_id),
                new MySqlParameter("@xmlbbm", 1),
                new MySqlParameter("@qtflbm", 1),
                new MySqlParameter("@xmlxbm", 1),
                new MySqlParameter("@ytflbm", 1),
                new MySqlParameter("@xmmc", well_name),
                new MySqlParameter("@xmdm", code),
                new MySqlParameter("@szd", place),
                new MySqlParameter("@gk", about),
                new MySqlParameter("@project_id", proId), 
                new MySqlParameter("@pvt_gas_name", pvtGasName),
                new MySqlParameter("@pvt_water_name", pvtWaterName),
                new MySqlParameter("@pvt_rock_name", pvtRockName),
                new MySqlParameter("@phase_seepage_name", gwpsName),
            };
            int result = DbManager.Ins.ExecuteNonquery(sql, Paramter.ToArray());
            if (result == 0)
            {
                MessageBox.Show("存储失败！", "失败");
                return;
            }
            //MessageBox.Show("存储成功！", "成功");
            
            if (GetWellIdAndPvtName != null)//判断事件是否为空
            {
                GetWellIdAndPvtName(well_id, well_name, pvtGasName);//执行委托实例  
                this.Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}
