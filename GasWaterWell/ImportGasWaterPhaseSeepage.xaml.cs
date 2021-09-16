using GasWaterWell.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GasWaterWell
{
    /// <summary>
    /// CreateProject.xaml 的交互逻辑
    /// </summary>
    public partial class ImportGasWaterPhaseSeepage : Window
    {

        DataTable dataTable;
        public ImportGasWaterPhaseSeepage()
        {
            InitializeComponent();
        }

        //public Action<string, string> GetProIdAndName;
        ///// <summary>
        ///// 新建项目
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void NewProject_Click(object sender, RoutedEventArgs e)
        //{
        //    if (this.ProjectName==null)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        if (GetProIdAndName != null)//判断事件是否为空
        //        {
        //            GetProIdAndName(Utils.GetUnixTimeStamp(),ProjectName.Text);//执行委托实例  
        //        }
        //        this.Close();
        //    }

        //}

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelNew_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public Action <string,string> GetGasWaterPhaseSeepageId;
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            OperateDB operateDB = new OperateDB();
            string name = this.gasWaterName.Text + ".gwps";
            DataTable relt = DbRead.GetPhaseSeepageByName(name);
            int count = relt.Rows.Count;
            if (count > 0)
            {
                MessageBox.Show("气水相渗名称已存在，请重新命名！");
                return;
            }
            string phase_seepage_index = Utils.GetUnixTimeStamp();
            string projectId = OperateXML.GetProjectId();
            if (name == "")
            {
                MessageBox.Show("名称不能为空，请输入名称后重试！");
                return;
            }
            if (!operateDB.InsertToPhaseSeepage(phase_seepage_index, projectId, name, dataTable))
            {
                MessageBox.Show("存储失败，请重新导入！");
                this.save.IsEnabled = false;
                return;
            }
            if (GetGasWaterPhaseSeepageId != null)
            {
                GetGasWaterPhaseSeepageId(phase_seepage_index, name);
            }
            this.Close();
        }

        private void ImportGasWater_Click(object sender, RoutedEventArgs e)
        {
            OperateDB operateDB = new OperateDB();
            try
            {
                dataTable = operateDB.ImportGasWaterPhaseSeepage();
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入失败，请重新导入！", "错误");
            }

            string name = this.gasWaterName.Text;
            if ( dataTable== null)
            {
                MessageBox.Show("导入失败，请重新导入！", "错误");
                return;
            }
            this.save.IsEnabled = true;
        }
    }
}
