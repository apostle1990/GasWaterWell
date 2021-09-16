using GasWaterWell.Model;
using GasWaterWell.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GasWaterWell
{
    /// <summary>
    /// OpenPVTWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OpenPVTWindow : Window
    {
        private PVTModel Mypm;
        private string clss;
        private string proId;
        public OpenPVTWindow()
        {
            InitializeComponent();
        }

        public OpenPVTWindow(string proId,string clss)
        {
            this.clss = clss;
            this.proId = proId;
            InitializeComponent();
        }

        public Action<string, string> GetpvtName;
        /// <summary>
        /// 打开pvt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_open(object sender, RoutedEventArgs e)
        {
            //Mypm获取到了当前选择对象属性
            if (this.Mypm == null)
            {
                return;
            }
            else
            {
                if (GetpvtName != null)//判断事件是否为空
                {
                    GetpvtName(Mypm.pvtName, Mypm.pvtTime);//执行委托实例
                    this.Close();
                }
            }
        }

        private void Button_Click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PVTModel pm = listView.SelectedItem as PVTModel;
            if (pm != null && pm is PVTModel)
            {
                Mypm = new PVTModel(pm.pvtName, pm.pvtTime);
                //MessageBox.Show(Mypm.pvtName + " "+ Mypm.pvtTime);
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //TODO这里需要传过来一个PVT类型,并判断类型,这里先假设是Gas
            ListView list = listView;
            Dictionary<string, string> PVTmodels = DbRead.getPVTByClss(this.clss);
            foreach(var i in PVTmodels)
            {
                listView.Items.Add(new PVTModel(i.Value.ToString(),
                    utils.Utils.UnixTimeStampToDateTime(Convert.ToDouble(i.Key)).ToString()));
            }
        }
    }
}
