using GasWaterWell.Model;
using GasWaterWell.utils;
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
    /// OpenProjectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OpenProjectWindow : Window
    {
        //保存当前选择project实例,用来打开项目
        private ProjectModel mypm = new ProjectModel();
        internal ProjectModel Mypm { get => mypm; set => mypm = value; }

        public OpenProjectWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载项目名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<ProjectModel> projects = DbRead.getProject();
            ListView prolist = listView;
            foreach(var i in projects)
            {
                prolist.Items.Add(new ProjectModel(i.projectName,i.projectTime,i.projectId));
            }
        }

        private void Button_Click_cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public Action<string, string> GetProjectIdAndName;//之前的定义委托和定义事件由这一句话代替
        private void Button_Click_open(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(Mypm.projectId + " " + Mypm.projectName);
            //TODO:打开窗口
            if (GetProjectIdAndName != null)//判断事件是否为空
            {
                GetProjectIdAndName(Mypm.projectId, Mypm.projectName);//执行委托实例  
                this.Close();
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProjectModel pm = listView.SelectedItem as ProjectModel;
            if (pm != null && pm is ProjectModel)
            {
                Mypm = new ProjectModel(pm.projectName, pm.projectTime, pm.projectId);
                //MessageBox.Show(Mypm.projectName+" "+ Mypm.projectTime);
            }
        }
    }
}
