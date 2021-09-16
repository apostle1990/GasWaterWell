using GasWaterWell.Model;
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
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace GasWaterWell
{
    /// <summary>
    /// WellBasicParams.xaml 的交互逻辑
    /// </summary>
    public partial class WellBasicParams : Window
    {

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }



        WellParamsModel wellParamsModel = new WellParamsModel();
        int wellId = -1;
        public WellBasicParams()
        {
            InitializeComponent();
        }
        public WellBasicParams(string wellId)
        {
            InitializeComponent();
            this.wellId = int.Parse(wellId);
        }
        public WellBasicParams(string wellId,int flag)
        {
            InitializeComponent();
            this.wellId = int.Parse(wellId);
            WellParamsModel paramsModel = DbRead.GetWellParams(wellId);
            this.rhowsc_TextBox.Text = paramsModel.rhowsc.ToString();
            this.rhogsc_TextBox.Text = paramsModel.rhogsc.ToString();
            this.S_TextBox.Text = paramsModel.s.ToString();
            this.D_TextBox.Text = (paramsModel.d*10000).ToString();
            this.Re_TextBox.Text = paramsModel.re.ToString();
            this.rw_TextBox.Text = paramsModel.rw.ToString();
            this.pe_TextBox.Text = paramsModel.pe.ToString();
        }

        public Action<int> GetUpdateParams; 
        /// <summary>
        /// save BasciParams
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            save_btn.IsEnabled = false;
            string rhowsc_TextBox = this.rhowsc_TextBox.Text;
            string rhogsc_TextBox = this.rhogsc_TextBox.Text;
            string S_TextBox = this.S_TextBox.Text;
            string D_TextBox = this.D_TextBox.Text;
            string Re_TextBox = this.Re_TextBox.Text;
            string rw_TextBox = this.rw_TextBox.Text;
            string pe_TextBox = this.pe_TextBox.Text;

            //判断空值
            TextBox[] txt = { this.rhowsc_TextBox, this.rhogsc_TextBox, this.S_TextBox, this.D_TextBox, this.Re_TextBox, this.rw_TextBox};
            if (Utils.IsTextBoxEmpty(txt))
            {
                save_btn.IsEnabled = true;
                return;
            }
            if (!Utils.IsDigit(txt))
            {
                save_btn.IsEnabled = true;
                return;
            }

            double rhowsc = double.Parse(rhowsc_TextBox);
            double rhogsc = double.Parse(rhogsc_TextBox);
            double s = double.Parse(S_TextBox);
            double d = double.Parse(D_TextBox) / 10000; // 非达西渗流系数
            double re = double.Parse(Re_TextBox);
            double rw = double.Parse(rw_TextBox);
            double pe = double.Parse(pe_TextBox);

            wellParamsModel = new WellParamsModel(
                this.wellId,
                s,
                d,
                re,
                rw,
                rhogsc,
                rhowsc,
                pe);
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"REPLACE INTO well_params (well_id,s,d,re,rw,rhogsc,rhowsc,pe)
                           VALUES(@well_id,@s,@d,@re,@rw,@rhogsc,@rhowsc,@pe); ";
            List<MySqlParameter> Paramter = new List<MySqlParameter>
            {
                new MySqlParameter("@rhowsc", wellParamsModel.rhowsc),
                new MySqlParameter("@rhogsc", wellParamsModel.rhogsc),
                new MySqlParameter("@pe", wellParamsModel.pe),
                new MySqlParameter("@s", wellParamsModel.s),
                new MySqlParameter("@d", wellParamsModel.d),
                new MySqlParameter("@re", wellParamsModel.re),
                new MySqlParameter("@rw", wellParamsModel.rw),
                new MySqlParameter("@well_id", wellParamsModel.well_id),
            };
            int result = DbManager.Ins.ExecuteNonquery(sql, Paramter.ToArray());
            if (result == 0)
            {
                MessageBox.Show("存储参数失败！", "失败");
                return;
            }
            if (GetUpdateParams != null)//判断事件是否为空
            {
                GetUpdateParams(result);//执行委托实例  
            }
            this.Close();
        }

    }
}
