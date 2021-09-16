using GasWaterWell.Model;
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
    /// OpenPhaseSeepage.xaml 的交互逻辑
    /// </summary>
    public partial class OpenPhaseSeepage : Window
    {
        Dictionary<string, string> PSmodel;

        private PhaseSeepage psm = new PhaseSeepage();


        public OpenPhaseSeepage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ListView list = listView;
            PSmodel = utils.DbRead.getPhaseSeepage();
            foreach (var i in PSmodel)
            {  
                listView.Items.Add(new Model.PhaseSeepage(i.Key.ToString(),
                    utils.Utils.UnixTimeStampToDateTime(Convert.ToDouble(i.Value)).ToString(), Convert.ToInt32(i.Value)));
            }

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PhaseSeepage psm_t = listView.SelectedItem as PhaseSeepage;
            if (psm_t != null && psm_t is PhaseSeepage)
            {
                psm = psm_t;
                //MessageBox.Show(psm.PhaseSeepageName + " " + psm.PhaseSeepageDate+" "+psm.PhaseSeepageIndex);
            }
        }

        private void Button_Click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public Action<string, string> GetPhaseSeepageIdAndName;//之前的定义委托和定义事件由这一句话代替
        private void Button_Click_open(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(psm.PhaseSeepageIndex + " " + psm.PhaseSeepageName);
            //TODO:打开窗口
            if (GetPhaseSeepageIdAndName != null)//判断事件是否为空
            {
                GetPhaseSeepageIdAndName(psm.PhaseSeepageIndex.ToString(), psm.PhaseSeepageName);//执行委托实例  
                this.Close();
            }
        }
    }
}
