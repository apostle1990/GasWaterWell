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
    /// CreateProject.xaml 的交互逻辑
    /// </summary>
    public partial class CreateProject : Window
    {
        public CreateProject()
        {
            InitializeComponent();
        }

        public Action<string, string> GetProIdAndName;
        /// <summary>
        /// 新建项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            if (this.ProjectName==null)
            {
                return;
            }
            else
            {
                if (GetProIdAndName != null)//判断事件是否为空
                {
                    GetProIdAndName(Utils.GetUnixTimeStamp(),ProjectName.Text);//执行委托实例  
                }
                this.Close();
            }

        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelNew_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
