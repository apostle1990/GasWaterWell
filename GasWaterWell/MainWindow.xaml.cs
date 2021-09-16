using GasWaterWell.Method2;
using GasWaterWell.Model;
using GasWaterWell.utils;
using GasWaterWell.views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using TabControlWithClose;

namespace GasWaterWell
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public object GetPvtByName { get; private set; }
        int PID = -1;

        public MainWindow()
        {
            InitializeComponent();
            LoadTreeView();
            CreateFolder(); // 新建文件夹
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReadConfig();
            Connect_Button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent, Connect_Button));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("是否关闭油气井产能计算预测系统", "关闭窗口", MessageBoxButton.YesNo) == MessageBoxResult.No)
                e.Cancel = true;
            else
                KillProcess(PID);
        }

        private void LoadTreeView()
        {
            string xmlpath = "Package/rightMenuData.xml";

            XmlDocument xml = new XmlDocument();
            xml.Load(xmlpath);

            XmlDataProvider xdp = new XmlDataProvider();
            xdp.Document = xml;
            xdp.XPath = @"/Packages/Package";

            this.proSourceTree.DataContext = xdp;
            this.proSourceTree.SetBinding(TreeView.ItemsSourceProperty, new Binding());
        }


        /// <summary>
        /// 设置选项卡为选中状态
        /// </summary>
        /// <param name="tabpage"></param>
        public void getTabPageIndex(UCTabItemWithClose tabItem)
        {
            foreach (UCTabItemWithClose tp in mainTab.Items)
            {
                if (tp.Header.ToString() == tabItem.Header.ToString())
                {
                    this.mainTab.SelectedItem = tp;
                    break;
                }
            }
        }

        /// <summary>
        /// 判断是否存在重复的选项卡
        /// </summary>
        /// <param name="tabItem"></param>
        /// <returns></returns>
        public bool IsCopiedTabItem(UCTabItemWithClose tabItem)
        {
            bool isExit = false;
            foreach (UCTabItemWithClose item in mainTab.Items)
            {
                if (tabItem.Tag.ToString() == item.Tag.ToString())
                {
                    isExit = true;
                    break;
                }
            }
            return isExit;
        }
        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness(0);
            }
        }

        /// <summary>
        /// TreeView 右键生成菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject obj = e.OriginalSource as DependencyObject;
            if (obj == null) return;
            TreeViewItem item = GetDependencyObjectFromVisualTree(obj, typeof(TreeViewItem)) as TreeViewItem;
            XmlElement selectedElement = (XmlElement)item.Header;

            string header = selectedElement.Name;

            string attName = selectedElement.Attributes["Name"].Value;
            string type = selectedElement.Attributes["Type"].Value;
            //string id = selectedElement.Attributes["Id"].Value;
            if (type.ToUpper() == "PACKAGE")
            {
                MenuItem mnuItem1 = new MenuItem();
                mnuItem1.Header = "新建井";
                mnuItem1.Click += new RoutedEventHandler(CreateWell_Click);

                ContextMenu menu = new ContextMenu() { };
                menu.Items.Add(mnuItem1);


                (sender as TreeViewItem).ContextMenu = menu;
            }
            else if (type.ToUpper() == "PVT")
            {
                ContextMenu menu = new ContextMenu() { };
                MenuItem mnuItem1 = new MenuItem();
                mnuItem1.Header = "添加";
                MenuItem chlMenuItem1 = new MenuItem();
                chlMenuItem1.Header = "天然气性质";
                chlMenuItem1.Click += new RoutedEventHandler(AddGasPVT_Click);
                MenuItem chlMenuItem2 = new MenuItem();
                chlMenuItem2.Header = "岩石性质";
                chlMenuItem2.Click += new RoutedEventHandler(AddStonePVT_Click);
                MenuItem chlMenuItem3 = new MenuItem();
                chlMenuItem3.Header = "地层水性质";
                chlMenuItem3.Click += new RoutedEventHandler(AddWaterPVT_Click);
                mnuItem1.Items.Add(chlMenuItem1);
                mnuItem1.Items.Add(chlMenuItem2);
                mnuItem1.Items.Add(chlMenuItem3);

                MenuItem mnuItem2 = new MenuItem();
                mnuItem2.Header = "打开";

                MenuItem openGas = new MenuItem();
                openGas.Header = "天然气性质";
                openGas.Click += new RoutedEventHandler(OpenGasPVT_CLick);
                MenuItem openRock = new MenuItem();
                openRock.Header = "岩石性质";
                openRock.Click += new RoutedEventHandler(OpenRockPVT_CLick);
                MenuItem openWater = new MenuItem();
                openWater.Header = "地层水性质";
                openWater.Click += new RoutedEventHandler(OpenWaterPVT_CLick);
                mnuItem2.Items.Add(openGas);
                mnuItem2.Items.Add(openRock);
                mnuItem2.Items.Add(openWater);

                //MenuItem mnuItem3 = new MenuItem();
                //mnuItem3.Header = "导入";
                menu.Items.Add(mnuItem1);
                menu.Items.Add(mnuItem2);
                //menu.Items.Add(mnuItem3);
                (sender as TreeViewItem).ContextMenu = menu;
            }
            #region PVTItem
            else if (type.ToUpper() == "PVTITEM")
            {
                string clss = selectedElement.Attributes["Class"].Value;
                ContextMenu menu = new ContextMenu() { };
                MenuItem mnuItem1 = new MenuItem();
                mnuItem1.Header = "移除";
                mnuItem1.Click += new RoutedEventHandler(MoveNode_Click);
                MenuItem mnuItem2 = new MenuItem();
                mnuItem2.Header = "删除";
                mnuItem2.Click += new RoutedEventHandler(DeletePVT_Click);
                MenuItem mnuItem3 = new MenuItem();
                mnuItem3.Header = "重命名";
                menu.Items.Add(mnuItem1);
                menu.Items.Add(mnuItem2);
                menu.Items.Add(mnuItem3);
                (sender as TreeViewItem).ContextMenu = menu;

            }
            #endregion
            #region GasWaterPhaseSeepage
            else if (type.ToUpper() == "GWPS")
            {
                ContextMenu menu = new ContextMenu() { };
                MenuItem mnuItem1 = new MenuItem();
                mnuItem1.Header = "打开";
                mnuItem1.Click += new RoutedEventHandler(OpenGasWaterPhaseSeepage_Click);
                MenuItem mnuItem2 = new MenuItem();
                mnuItem2.Header = "导入";
                mnuItem2.Click += new RoutedEventHandler(ImportGasWaterPhaseSeepage_Click);

                menu.Items.Add(mnuItem1);
                menu.Items.Add(mnuItem2);
                //menu.Items.Add(mnuItem3);
                (sender as TreeViewItem).ContextMenu = menu;
            }
            #endregion
            #region GasWaterPhaseSeepageItem
            else if (type.ToUpper() == "GWPSITEM")
            {
                ContextMenu menu = new ContextMenu() { };
                MenuItem mnuItem1 = new MenuItem();
                mnuItem1.Header = "移除";
                mnuItem1.Click += new RoutedEventHandler(MoveNode_Click);
                MenuItem mnuItem2 = new MenuItem();
                mnuItem2.Header = "删除";
                mnuItem2.Click += new RoutedEventHandler(DeleteGasWaterPhaseSeepage_Click);
                MenuItem mnuItem3 = new MenuItem();
                mnuItem3.Header = "重命名";
                menu.Items.Add(mnuItem1);
                menu.Items.Add(mnuItem2);
                menu.Items.Add(mnuItem3);
                (sender as TreeViewItem).ContextMenu = menu;
            }
            #endregion
            else if (type.ToUpper() == "WELL")
            {
                ContextMenu menu = new ContextMenu() { };
                MenuItem mnuItem2 = new MenuItem();
                mnuItem2.Header = "删除";
                mnuItem2.Click += new RoutedEventHandler(DeleteWell_Click);
                MenuItem mnuItem3 = new MenuItem();
                mnuItem3.Header = "重命名";
                menu.Items.Add(mnuItem2);
                menu.Items.Add(mnuItem3);
                (sender as TreeViewItem).ContextMenu = menu;
            }

            else if(type.ToUpper() == "PARA")
            {
                ContextMenu menu = new ContextMenu() { };
                MenuItem mnuItem2 = new MenuItem();
                mnuItem2.Header = "修改";
                mnuItem2.Click += new RoutedEventHandler(ModifyBtn_Click);
                menu.Items.Add(mnuItem2);
                (sender as TreeViewItem).ContextMenu = menu;
            }

            #region TestWell
            else if (type.ToUpper() == "TESTWELL")
            {
                ContextMenu menu = new ContextMenu() { };
                MenuItem mnuItem2 = new MenuItem();
                mnuItem2.Header = "新建产能试井";
                MenuItem child1 = new MenuItem();
                child1.Header = "产能分析";
                MenuItem child2 = new MenuItem();
                child2.Header = "产能预测";
                mnuItem2.Items.Add(child1);
                mnuItem2.Items.Add(child2);
                child1.Click += new RoutedEventHandler(WellAnalysis_CLick);
                child2.Click += new RoutedEventHandler(WellPredict_CLick);
                menu.Items.Add(mnuItem2);
                (sender as TreeViewItem).ContextMenu = menu;
            }
            #endregion
            #region TestWellItem            
            else if (type.ToUpper() == "TWITEM")
            {
                ContextMenu menu = new ContextMenu() { };
                MenuItem child1 = new MenuItem();
                child1.Header = "删除";
                child1.Click += new RoutedEventHandler(DelateTestWell_Click);
                menu.Items.Add(child1); 
                (sender as TreeViewItem).ContextMenu = menu;
            }
            #endregion
            #region SPB-Gas PressMethod            
            else if (type.ToUpper() == "PRESSMETHOD")
            {
                ContextMenu menu = new ContextMenu() { };
                MenuItem child1 = new MenuItem();
                child1.Header = "新建压力平方法";
                child1.Click += new RoutedEventHandler(CreatePressMethod_CLick);
                menu.Items.Add(child1);
                (sender as TreeViewItem).ContextMenu = menu;
            }
            #endregion
            #region SPB-Gas PressMethodItem            
            else if (type.ToUpper() == "PRESSMETHODITEM")
            {
                ContextMenu menu = new ContextMenu() { };
                MenuItem child1 = new MenuItem();
                child1.Header = "删除";
                child1.Click += new RoutedEventHandler(DeletePressMethod_CLick);
                menu.Items.Add(child1);
                (sender as TreeViewItem).ContextMenu = menu;
            }
            #endregion
            #region SPB-Gas PseudoPressMethod 拟压力法        
            else if (type.ToUpper() == "PPRESS")
            {
                ContextMenu menu = new ContextMenu() { };
                MenuItem child1 = new MenuItem();
                child1.Header = "新建拟压力法";
                child1.Click += new RoutedEventHandler(CreatePseudoPressMethod_CLick);
                menu.Items.Add(child1);
                (sender as TreeViewItem).ContextMenu = menu;
            }
            #endregion

            #region SPB-Gas PseudoPressMethodItem  删除拟压力平方法            
            else if (type.ToUpper() == "PPRESSITEM")
            {
                ContextMenu menu = new ContextMenu() { };
                MenuItem child1 = new MenuItem();
                child1.Header = "删除";
                child1.Click += new RoutedEventHandler(DeletePseudPressMethod_CLick);
                menu.Items.Add(child1);
                (sender as TreeViewItem).ContextMenu = menu;
            }
            #endregion

            #region SPB-Gaswater
            else if (type.ToUpper() == "SPBGW")
            {
                ContextMenu menu = new ContextMenu() { };
                MenuItem mnuItem1 = new MenuItem();
                mnuItem1.Header = "新建气水两相";
                mnuItem1.Click += new RoutedEventHandler(GasWater_Click);
                menu.Items.Add(mnuItem1);
                (sender as TreeViewItem).ContextMenu = menu;
            }
            #endregion
            else if (type.ToUpper() == "SPBGWITEM")
            {
                ContextMenu menu = new ContextMenu() { };
                //MenuItem mnuItem1 = new MenuItem();
                //mnuItem1.Header = "移除";
                //mnuItem1.Click += MoveNode_Click;
                MenuItem mnuItem2 = new MenuItem();
                mnuItem2.Header = "删除";
                mnuItem2.Click += new RoutedEventHandler(DeleteGasWaterItem_Click);
                //menu.Items.Add(mnuItem1);
                menu.Items.Add(mnuItem2);
                (sender as TreeViewItem).ContextMenu = menu;
            }
            else
            {
                ContextMenu menu = new ContextMenu() { };
                (sender as TreeViewItem).ContextMenu = null;
                return;
            }
        }

        private void DeleteGasWaterItem_Click(object sender, RoutedEventArgs e)
        {
            System.Xml.XmlElement xmlElement = (XmlElement)proSourceTree.SelectedItem;
            if (xmlElement == null)
            {
                return;
            }
            if (MessageBox.Show("确定要永久删除此文件吗？", "警告", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                return;
            }
            string name = xmlElement.Attributes["Name"].Value.ToString();
            string type = xmlElement.Attributes["Type"].Value.ToString();
            if (type.ToUpper() == "SPBGWITEM") 
            {
                string proId = xmlElement.Attributes["Id"].Value.ToString();
                string resultId = xmlElement.Attributes["ResultId"].Value.ToString();
                int i = OperateDB.DeleteGasWaterItem(proId, resultId);
                if (i > 0)
                {
                    MoveNode();
                }
                else
                {
                    MessageBox.Show("删除失败", "ERROR");
                }
            }
            else
            {
                return;
            }
        }

        private void DelateTestWell_Click(object sender, RoutedEventArgs e)
        {
            System.Xml.XmlElement xmlElement = (XmlElement)proSourceTree.SelectedItem;
            if (xmlElement == null)
            {
                return;
            }
            if (MessageBox.Show("确定要永久删除此文件吗？", "警告", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                return;
            }
            string name = xmlElement.Attributes["Name"].Value.ToString();
            string type = xmlElement.Attributes["Type"].Value.ToString();
            if (type.ToUpper() == "TWITEM")
            {
                string fitId = xmlElement.Attributes["FitId"].Value.ToString();
                string predId = xmlElement.Attributes["PredId"].Value.ToString();
                int i = OperateDB.DeleteTestWellItem(fitId, predId);
                if (i > 0)
                {
                    MoveNode();
                }
                else
                {
                    MessageBox.Show("删除失败", "ERROR");
                }
            }
            else
            {
                return;
            }
        }


        /// <summary>
        /// 在井中删除一个拟压力平方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeletePseudPressMethod_CLick(object sender, RoutedEventArgs e)
        {
            System.Xml.XmlElement xmlElement = (XmlElement)proSourceTree.SelectedItem;
            if (xmlElement == null)
            {
                return;
            }
            if (MessageBox.Show("确定要永久删除此文件吗？", "警告", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                return;
            }
            string type = xmlElement.Attributes["Type"].Value.ToString();
            if (type.ToUpper() == "PPRESSITEM")
            {
                string proId = xmlElement.Attributes["Id"].Value.ToString();
                string resultId = xmlElement.Attributes["ResultId"].Value.ToString();
                int i = OperateDB.DeletePseudoPressureItem(proId, resultId);
                if (i > 0)
                {
                    MoveNode();
                }
                else
                {
                    MessageBox.Show("删除失败", "ERROR");
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 在井中新建一个拟压力法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatePseudoPressMethod_CLick(object sender, RoutedEventArgs e)
        {
            AddGasPseudoPress();
            //throw new NotImplementedException();
        }

        public void AddGasPseudoPress()
        {
            XmlElement xmlElement = null;
            //string id = xmlElement.Attributes["Id"].Value.ToString();
            try
            {
                xmlElement = (XmlElement)proSourceTree.SelectedItem;
            }
            catch (Exception ex)
            {
                MessageBox.Show("你可能没有选中井哦~\n" + ex, "哎呀，出错了...");
            }
            string parentId = xmlElement.GetAttribute("WellId");
            if (parentId != null)
            {
                GasPseudoPressure gasPseudoPressure = new GasPseudoPressure(int.Parse(parentId));
                gasPseudoPressure.GetInputIdAndResultIndex += (gasWaterInputId, gasWaterResultId) =>
                {
                    string name = Utils.UnixTimeStampToDateTime(Convert.ToDouble(gasWaterInputId)).ToString();
                    name = OperateXML.XmlInsertPseudoPressMethod2(parentId, gasWaterInputId, gasWaterResultId, name);
                    LoadTreeView();
                    AddGasPage tabItem = new AddGasPage(parentId, gasWaterInputId, gasWaterResultId, name);
                    if (!IsCopiedTabItem(tabItem.addItem()))
                    {
                        mainTab.Items.Add(tabItem.addItem());
                    }
                    getTabPageIndex(tabItem.addItem());
                };
                gasPseudoPressure.ShowDialog();
            }
        }

        /// <summary>
        /// 删除压力平方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeletePressMethod_CLick(object sender, RoutedEventArgs e)
        {
            System.Xml.XmlElement xmlElement = (XmlElement)proSourceTree.SelectedItem;
            if (xmlElement == null)
            {
                return;
            }
            if (MessageBox.Show("确定要永久删除此文件吗？", "警告", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                return;
            }
            string type = xmlElement.Attributes["Type"].Value.ToString();
            if (type.ToUpper() == "PRESSMETHODITEM")
            {
                string proId = xmlElement.Attributes["Id"].Value.ToString();
                string resultId = xmlElement.Attributes["ResultId"].Value.ToString();
                int i = OperateDB.DeletePseudoPressureItem(proId, resultId);
                if (i > 0)
                {
                    MoveNode();
                }
                else
                {
                    MessageBox.Show("删除失败", "ERROR");
                }
            }
            else
            {
                return;
            }
        }



        /// <summary>
        /// 在井中新建一个压力平方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatePressMethod_CLick(object sender, RoutedEventArgs e)
        {
            AddGasPressureMethod();
            //throw new NotImplementedException();
        }

        public void AddGasPressureMethod()
        {
            XmlElement xmlElement = null;
            //XmlElement parentNode = null;
            //string id = xmlElement.Attributes["Id"].Value.ToString();
            try
            {
                xmlElement = (XmlElement)proSourceTree.SelectedItem;
                //parentNode = (XmlElement)xmlElement.ParentNode;
            }
            catch (Exception ex)
            {
                MessageBox.Show("你可能没有选中井哦~\n" + ex, "哎呀，出错了...");
            }
            string parentId = xmlElement.GetAttribute("WellId");
            if (parentId != null)
            {
                //Console.WriteLine(parentId);
                PressureSquare pressureSquare = new PressureSquare(int.Parse(parentId));
                pressureSquare.GetInputIdAndResultIndex += (gasWaterInputId, gasWaterResultId) =>
                {
                    string name = Utils.UnixTimeStampToDateTime(Convert.ToDouble(gasWaterInputId)).ToString();
                    name = OperateXML.XmlInsertPressMethod2(parentId, gasWaterInputId, gasWaterResultId, name);
                    LoadTreeView();
                    AddGasPage tabItem = new AddGasPage(parentId, gasWaterInputId, gasWaterResultId, name);
                    if (!IsCopiedTabItem(tabItem.addItem()))
                    {
                        mainTab.Items.Add(tabItem.addItem());
                    }
                    getTabPageIndex(tabItem.addItem());
                };
                pressureSquare.ShowDialog();
            }
        }
 
        /// <summary>
        /// 从项目中删除气水相渗项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteGasWaterPhaseSeepage_Click(object sender, RoutedEventArgs e)
        {
            System.Xml.XmlElement xmlElement = (XmlElement)proSourceTree.SelectedItem;
            if (xmlElement == null)
            {
                return;
            }
            if (MessageBox.Show("确定要永久删除吗？", "警告", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                return;
            }
            try
            {
                string id = xmlElement.Attributes["Id"].Value.ToString();
                string type = xmlElement.Attributes["Type"].Value.ToString();
                string name = xmlElement.Attributes["Name"].Value.ToString();
                if (type.ToUpper() == "GWPSITEM") 
                {
                    if (OperateDB.DeleteGWPSItem(name) > 0)
                    {
                        MoveNode();
                    }
                    else
                    {
                        MessageBox.Show("存在项目引用该文件不能删除！");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "ERROR");
            }
        }

        /// <summary>
        /// 导入气水相渗项目  import GasPhaseSeepage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportGasWaterPhaseSeepage_Click(object sender, RoutedEventArgs e)
        {
            ImportGasWaterPhaseSeepage importGasWaterPhase = new ImportGasWaterPhaseSeepage();
            importGasWaterPhase.GetGasWaterPhaseSeepageId += (id, name) =>
            {
                OperateXML.XmlInsertGWPhaseSeepageItem(id, name);
                LoadTreeView();
                //TODO: 写一个AddGasWaterPhaseSeepagePage
                AddPhaseSeepage tabItem = new AddPhaseSeepage(id,name);
                if (!IsCopiedTabItem(tabItem.addItem()))
                {
                    mainTab.Items.Add(tabItem.addItem());
                }
                getTabPageIndex(tabItem.addItem());
            };
            importGasWaterPhase.ShowDialog();

        }

        /// <summary>
        /// 打开气水相渗项目  open GasPhaseSeepage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenGasWaterPhaseSeepage_Click(object sender, RoutedEventArgs e)
        {
            OpenPhaseSeepage openPhaseSeepage = new OpenPhaseSeepage();
            openPhaseSeepage.GetPhaseSeepageIdAndName += ( phaseId,phaseName) =>
            {
                if (OperateXML.SelectNode("GWPSItem", phaseName) != null)
                {
                    MessageBox.Show("已存在在项目中,请重新选择！");
                    return;
                }
                OperateXML.XmlInsertGWPhaseSeepageItem(phaseId, phaseName);
                LoadTreeView();
                AddPhaseSeepage tabItem = new AddPhaseSeepage(phaseId, phaseName);
                mainTab.Items.Add(tabItem.addItem());
                getTabPageIndex(tabItem.addItem());
            };
            openPhaseSeepage.ShowDialog();

        }

        /// <summary>
        /// 删除井
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteWell_Click(object sender, RoutedEventArgs e)
        {
            System.Xml.XmlElement xmlElement = (XmlElement)proSourceTree.SelectedItem;
            if (xmlElement == null)
            {
                return;
            }
            if (MessageBox.Show("确定要永久删除吗？", "警告", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                return;
            }
            try
            {
                string wellId = xmlElement.Attributes["Id"].Value.ToString();
                string type = xmlElement.Attributes["Type"].Value.ToString();
                if (type.ToUpper() == "WELL")
                {
                    if (OperateDB.DeleteWellById(wellId) > 0)
                    {
                        MoveNode();
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "ERROR");
            }
            
        }


        public Action<string, string> GetPVTTypeAndParentId;//str1 parentId,str2 clss 
        private void OpenWaterPVT_CLick(object sender, RoutedEventArgs e)
        {
            OpenPVTWindow openPVT = new OpenPVTWindow(OperateXML.GetProjectId(), "water");
            openPVT.GetpvtName += (pvtName, pvtTime) =>
            {
                if (OperateXML.SelectNode("PVTItem", pvtName) != null)
                {
                    MessageBox.Show("PVT已存在在项目中！");
                    return;
                }
                OperateXML.XmlInsertPVTEle(pvtName, pvtName, "water");
                LoadTreeView();
                AddWaterPVTPage tabItem = new AddWaterPVTPage(pvtName, pvtName);
                mainTab.Items.Add(tabItem.addItem());
                getTabPageIndex(tabItem.addItem());
            };
            openPVT.ShowDialog();
        }

        private void OpenRockPVT_CLick(object sender, RoutedEventArgs e)
        {
            OpenPVTWindow openPVT = new OpenPVTWindow(OperateXML.GetProjectId(), "rock");
            openPVT.GetpvtName += (pvtName, pvtTime) =>
            {
                if (OperateXML.SelectNode("PVTItem", pvtName) != null)
                {
                    MessageBox.Show("PVT已存在在项目中！");
                    return;
                }
                OperateXML.XmlInsertPVTEle(pvtName, pvtName, "rock");
                LoadTreeView();
                AddStonePVTPage tabItem = new AddStonePVTPage(pvtName, pvtName);
                mainTab.Items.Add(tabItem.addItem());
                getTabPageIndex(tabItem.addItem());
            };
            openPVT.ShowDialog();
        }


        /// <summary>
        /// 打开天然气性质PVT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenGasPVT_CLick(object sender, RoutedEventArgs e)
        {
            OpenPVTWindow openPVT = new OpenPVTWindow(OperateXML.GetProjectId(), "gas");
            openPVT.GetpvtName += (pvtName, pvtTime) =>
            {
                if (OperateXML.SelectNode("PVTItem", pvtName) != null)
                {
                    MessageBox.Show("PVT已存在在项目中！");
                    return;
                }
                OperateXML.XmlInsertPVTEle(pvtName, pvtName, "gas");
                LoadTreeView();
                AddGasPVTPage tabItem = new AddGasPVTPage(pvtName, pvtName);
                mainTab.Items.Add(tabItem.addItem());
                getTabPageIndex(tabItem.addItem());
            };
            openPVT.ShowDialog();
        }


        /// <summary>
        /// 产能预测
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WellPredict_CLick(object sender, RoutedEventArgs e)
        {
            XmlElement xmlElement = null;
            try
            {
                xmlElement = (XmlElement)proSourceTree.SelectedItem;
            }
            catch (Exception ex)
            {
                MessageBox.Show("你可能没有选中井哦~\n" + ex, "哎呀，出错了...");
            }
            string wellId;

            wellId = xmlElement.GetAttribute("WellId"); 
            Predict predict = new Predict(wellId);
            predict.GetPredictId += (fitId, predId) =>
            {
                RemoveTabItem(predId);
                string name = Utils.UnixTimeStampToDateTime(Convert.ToDouble(predId)).ToString();
                name = OperateXML.XmlInsertTestWellItem2(fitId, predId, name, wellId);
                WellOrFiledPage tabItem = new WellOrFiledPage(fitId,predId, name);               
                LoadTreeView();
                mainTab.Items.Add(tabItem.addItem());
                getTabPageIndex(tabItem.addItem());
            };
            predict.ShowDialog();
        }

        /// <summary>
        /// 试井分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WellAnalysis_CLick(object sender, RoutedEventArgs e)
        {
            XmlElement xmlElement = null;
          
            try
            {
                xmlElement = (XmlElement)proSourceTree.SelectedItem;
            }
            catch (Exception ex)
            {
                MessageBox.Show("你可能没有选中井哦~\n" + ex, "哎呀，出错了...");
            }
            string wellId;
            wellId = xmlElement.GetAttribute("WellId");          
            Fitting fit = new Fitting(wellId);
            fit.GetFittingId += (fitId, predictId) =>
            {
                RemoveTabItem(fitId);
                OperateXML.DeleteAnalsisItemById(fitId);
                string name = Utils.UnixTimeStampToDateTime(Convert.ToDouble(fitId)).ToString();
                name = OperateXML.XmlInsertTestWellItem2(fitId, predictId, name, wellId);
                LoadTreeView();
                WellOrFiledPage tabItem = new WellOrFiledPage(fitId, predictId, name);
                mainTab.Items.Add(tabItem.addItem());
                getTabPageIndex(tabItem.addItem());  
            };
            fit.ShowDialog();
        }

        /// <summary>
        /// 新建well
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateWell_Click(object sender, RoutedEventArgs e)
        {
            CreateWell newWell = new CreateWell();
            string Id = null;
            newWell.GetWellIdAndPvtName += (wellId, wellName, pvtName) =>
            {
                OperateXML.XmlInsertWell(wellId, wellName);
                Id = wellId;
                LoadTreeView();
            };
            newWell.ShowDialog();
            if (Id!=null)
            {
                WellBasicParams wellBasicParams = new WellBasicParams(Id);
                wellBasicParams.ShowDialog();
            }
            else
            {
                return;
            }
            

        }

        /// <summary>
        /// 删除PVT事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeletePVT_Click(object sender, RoutedEventArgs e)
        {
            System.Xml.XmlElement xmlElement = (XmlElement)proSourceTree.SelectedItem;
            if (xmlElement == null)
            {
                return;
            }
            if (MessageBox.Show("确定要永久删除此文件吗？", "警告", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
            {
                return;
            }
            string name = xmlElement.Attributes["Name"].Value.ToString();
            string type = xmlElement.Attributes["Type"].Value.ToString();
            if (type.ToUpper() == "PVTITEM")
            {
                string clss = xmlElement.Attributes["Class"].Value.ToString();
                if (clss == "gas")
                {
                    int i = OperateDB.DeleteGasPVT(name);
                    if (i > 0)
                    {
                        MoveNode();
                    }
                    else
                    {
                        MessageBox.Show("存在其他项目引用此文件，不能删除！", "ERROR");
                    }
                }
                else if (clss == "water")
                {
                    int i = OperateDB.DeleteWaterPVT(name);
                    if (i > 0)
                    {
                        MoveNode();
                    }
                    else
                    {
                        MessageBox.Show("存在其他项目引用此文件，不能删除！", "ERROR");
                    }
                }
                else if (clss == "rock")
                {
                    int i = OperateDB.DeleteRockPVT(name);
                    if (i > 0)
                    {
                        MoveNode();
                    }
                    else
                    {
                        MessageBox.Show("存在其他项目引用此文件，不能删除！", "ERROR");
                    }
                }
            }
            else
            {
                return;
            }


        }

        /// <summary>
        /// 移除PVT、well、press、gasWater Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveNode_Click(object sender, RoutedEventArgs e)
        {
            MoveNode();
        }

        public void MoveNode()
        {
            System.Xml.XmlElement xmlElement = (XmlElement)proSourceTree.SelectedItem;
            string id = xmlElement.Attributes["Id"].Value.ToString();
            OperateXML.XmlNodeDelete(xmlElement);
            RemoveTabItem(id);
            LoadTreeView();
        }
        
        private void RemoveTabItem(string id)
        {
            if (mainTab.Items == null)
            {
                return;
            }
            foreach (UCTabItemWithClose item in mainTab.Items)
            {
                if (id == item.Tag.ToString())
                {
                    mainTab.Items.Remove(item);
                    break;
                }
            }
        }


        /// <summary>
        /// 右键菜单栏添加地层水性质PVT事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddWaterPVT_Click(object sender, RoutedEventArgs e)
        {
            PVTWater wPvt = new PVTWater();
            wPvt.GetWaterPVTName += (pvtName) =>
            {
                OperateXML.XmlInsertPVTEle(pvtName, pvtName, "water");
                LoadTreeView();
                AddWaterPVTPage tabItem = new AddWaterPVTPage(pvtName, pvtName);
                mainTab.Items.Add(tabItem.addItem());
                getTabPageIndex(tabItem.addItem());
            };
            wPvt.ShowDialog();
        }


        /// <summary>
        /// 右键菜单栏添加岩石性质PVT单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddStonePVT_Click(object sender, RoutedEventArgs e)
        {
            PVTStone sPvt = new PVTStone();
            sPvt.GetRockPVTName += (id, pvtName) =>
            {
                OperateXML.XmlInsertPVTEle(id, pvtName, "rock");
                LoadTreeView();
                AddStonePVTPage tabItem = new AddStonePVTPage(id, pvtName);
                mainTab.Items.Add(tabItem.addItem());
                getTabPageIndex(tabItem.addItem());
            };
            sPvt.ShowDialog();
        }

        /// <summary>
        /// 右键菜单栏添加天然气性质PVT单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddGasPVT_Click(object sender, EventArgs e)
        {
            PVTGas gPvt = new PVTGas();
            gPvt.GetGasPVTName += (pvtName) =>
            {
                OperateXML.XmlInsertPVTEle(pvtName, pvtName, "gas");
                LoadTreeView();
                AddGasPVTPage tabItem = new AddGasPVTPage(pvtName, pvtName);
                mainTab.Items.Add(tabItem.addItem());
                getTabPageIndex(tabItem.addItem());
            };
            gPvt.ShowDialog();
        }

        private static DependencyObject GetDependencyObjectFromVisualTree(DependencyObject startObject, Type type)
        {
            var parent = startObject;
            while (parent != null)
            {
                if (type.IsInstanceOfType(parent))
                    break;
                try
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }
                catch (Exception e)
                {
                    MessageBox.Show("请关闭所有窗口。\n\n" + e, "哎呀，出错了...");
                }

            }
            return parent;
        }

        /// <summary>
        /// treeViewItem 双击显示在右边TabConTrol中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject obj = e.OriginalSource as DependencyObject;
            TreeViewItem item = GetDependencyObjectFromVisualTree(obj, typeof(TreeViewItem)) as TreeViewItem;
            XmlElement selectedElement = (XmlElement)item.Header;
            string attName = selectedElement.Attributes["Name"].Value;
            string type = selectedElement.Attributes["Type"].Value;
            

            #region PVT
            if (type.ToUpper() == "PVTITEM")
            {
                string clss = selectedElement.Attributes["Class"].Value;
                if (clss.ToUpper() == "WATER")
                {
                    AddWaterPVTPage tabItem = new AddWaterPVTPage(selectedElement.Attributes["Id"].Value, attName);
                    if (!IsCopiedTabItem(tabItem.addItem()))
                    {
                        mainTab.Items.Add(tabItem.addItem());
                    }
                    getTabPageIndex(tabItem.addItem());
                }
                else if (clss.ToUpper() == "ROCK")
                {
                    AddStonePVTPage tabItem = new AddStonePVTPage(selectedElement.Attributes["Id"].Value, attName);
                    if (!IsCopiedTabItem(tabItem.addItem()))
                    {
                        mainTab.Items.Add(tabItem.addItem());
                    }
                    getTabPageIndex(tabItem.addItem());
                }
                else if (clss.ToUpper() == "GAS")
                {
                    AddGasPVTPage tabItem = new AddGasPVTPage(selectedElement.Attributes["Id"].Value, attName);
                    if (!IsCopiedTabItem(tabItem.addItem()))
                    {
                        mainTab.Items.Add(tabItem.addItem());
                    }
                    getTabPageIndex(tabItem.addItem());
                }


            }
            #endregion
            #region  气水相渗
            else if (type.ToUpper() == "GWPSITEM")
            {
                AddPhaseSeepage tabItem = new AddPhaseSeepage(selectedElement.Attributes["Id"].Value, attName);
                if (!IsCopiedTabItem(tabItem.addItem()))
                {
                    mainTab.Items.Add(tabItem.addItem());
                }
                getTabPageIndex(tabItem.addItem());
            }
            #endregion
            

            #region 试井分析
            else if (type.ToUpper() == "TWITEM") 
            {
                string fitId = selectedElement.Attributes["FitId"].Value;
                string predId= selectedElement.Attributes["PredId"].Value;
                WellOrFiledPage tabItem = new WellOrFiledPage(fitId,predId, attName);
                if (!IsCopiedTabItem(tabItem.addItem()))
                {
                    mainTab.Items.Add(tabItem.addItem());
                }
                getTabPageIndex(tabItem.addItem());
            }

            #endregion


            #region 压力平方法
            else if (type.ToUpper() == "PRESSMETHODITEM")
            {
                string wellId = selectedElement.ParentNode.Attributes["WellId"].Value;
                //MessageBox.Show(wellId);
                AddGasPage tabItem = new AddGasPage(selectedElement.ParentNode.Attributes["WellId"].Value,selectedElement.Attributes["Id"].Value, selectedElement.Attributes["ResultId"].Value,attName);
                if (!IsCopiedTabItem(tabItem.addItem()))
                {
                    mainTab.Items.Add(tabItem.addItem());
                }
                getTabPageIndex(tabItem.addItem());

            }
            #endregion

            #region 拟压力法
            else if (type.ToUpper() == "PPRESSITEM")
            {
                AddGasPage tabItem = new AddGasPage(selectedElement.ParentNode.Attributes["WellId"].Value, selectedElement.Attributes["Id"].Value, selectedElement.Attributes["ResultId"].Value, attName);
                if (!IsCopiedTabItem(tabItem.addItem()))
                {
                    mainTab.Items.Add(tabItem.addItem());
                }
                getTabPageIndex(tabItem.addItem());

            }
            #endregion
            #region 气水两相

            else if (type.ToUpper() == "SPBGWITEM")
            {
                string wellId = selectedElement.ParentNode.Attributes["WellId"].Value;
                string gasWaterMId = selectedElement.Attributes["Id"].Value;
                string gasWaterMResultId = selectedElement.Attributes["ResultId"].Value;
                //TODO 添加气水两相
                AddGasWaterPage tabItem = new AddGasWaterPage(wellId, gasWaterMId, gasWaterMResultId, attName);
                if (!IsCopiedTabItem(tabItem.addItem()))
                {
                    mainTab.Items.Add(tabItem.addItem());
                }
                getTabPageIndex(tabItem.addItem());
            }
            #endregion



        }

        /// <summary>
        /// 新建项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateProject_Click(object sender, RoutedEventArgs e)
        {
            CreateProject newPro = new CreateProject();
            newPro.GetProIdAndName += (proId, proName) =>
            {
                if (mainTab.Items != null)
                {
                    mainTab.Items.Clear();
                }
                if (OperateDB.InsertProject(proId, proName) > 0)
                {
                    OperateXML.XMLCreateProject(proId, proName);
                    LoadTreeView();
                }
            };
            newPro.ShowDialog();
        }


        /// <summary>
        /// 打开项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenProject_CLick(object sender, RoutedEventArgs e)
        {
            OpenProjectWindow openProjectWindow = new OpenProjectWindow();
            openProjectWindow.GetProjectIdAndName += (proId, proName) =>
            {
                if (mainTab.Items != null)
                {
                    mainTab.Items.Clear();
                }
                OperateXML.XMLOpenProject(proId, proName);
                LoadTreeView();
            };
            openProjectWindow.ShowDialog();
        }


        /// <summary>
        /// 气水两相
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GasWater_Click(object sender, RoutedEventArgs e)
        {
            AddGasWaterMethod();
        }

        /// <summary>
        /// 添加气水两相法
        /// </summary>
        public void AddGasWaterMethod()
        {
            XmlElement xmlElement = null;
            //string id = xmlElement.Attributes["Id"].Value.ToString();
            try
            {
                xmlElement = (XmlElement)proSourceTree.SelectedItem;
            }
            catch (Exception ex)
            {
                MessageBox.Show("你可能没有选中井哦~\n" + ex, "哎呀，出错了...");
            }
            string parentId = xmlElement.GetAttribute("WellId");
            if (parentId != null)
            {
                GasWaterPseudoPressure gasWaterPseudoPressure = new GasWaterPseudoPressure(int.Parse(parentId));
                gasWaterPseudoPressure.GetInputIdAndResultIndex += (gasWaterInputId, gasWaterResultId) =>
                {
                    string name =Utils.UnixTimeStampToDateTime(Convert.ToDouble(gasWaterInputId)).ToString();
                    name = OperateXML.XmlInsertGasWaterMethod2(parentId, gasWaterInputId, gasWaterResultId, name);
                    LoadTreeView();
                    AddGasWaterPage tabItem = new AddGasWaterPage(parentId, gasWaterInputId, gasWaterResultId, name);
                    if (!IsCopiedTabItem(tabItem.addItem()))
                    {
                        mainTab.Items.Add(tabItem.addItem());
                    }
                    getTabPageIndex(tabItem.addItem());
                };
                gasWaterPseudoPressure.ShowDialog();
            }
        }

        /// <summary>
        /// treeView 选中改变时 属性栏也跟着改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProSourceTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            XmlElement xmlElement = (XmlElement)proSourceTree.SelectedItem;
            if (xmlElement == null)
            {
                return;
            }
            string type = xmlElement.GetAttribute("Type");

            if (type.ToUpper() == "WELL")
            {
                string wellId = xmlElement.GetAttribute("Id");
                WellPropertyModel wellPropertyModel = DbRead.GetWellProperty(wellId);
                this.wellName.Text = wellPropertyModel.wellName;
                this.wellCode.Text = wellPropertyModel.wellCode;
                this.wellAddress.Text = wellPropertyModel.wellAddress;
                this.wellDetail.Text = wellPropertyModel.wellDetail;
                this.pvtName.Text = wellPropertyModel.pvtGasName;

                WellParamsModel wellParamsModel = DbRead.GetWellParams(wellId);
                this.wellName1.Text = wellPropertyModel.wellName;
                if (wellParamsModel == null)
                {
                    return;
                }
                this.modifyBtn.IsEnabled = true;
                ValuedWellParams(wellParamsModel);
                
                //this.analysisBtn.IsEnabled = true;
                //this.predictBtn.IsEnabled = true;
                //this.gasWaterBtn.IsEnabled = true;
            }
            else if(type.ToUpper() == "PARA")
            {
                ClearWellProperty();
                string wellId = xmlElement.GetAttribute("WellId");
                WellPropertyModel wellPropertyModel = DbRead.GetWellProperty(wellId);
                this.wellName.Text = wellPropertyModel.wellName;
                this.wellCode.Text = wellPropertyModel.wellCode;
                this.wellAddress.Text = wellPropertyModel.wellAddress;
                this.wellDetail.Text = wellPropertyModel.wellDetail;
                this.pvtName.Text = wellPropertyModel.pvtGasName;

                WellParamsModel wellParamsModel = DbRead.GetWellParams(wellId);
                this.wellName1.Text = wellPropertyModel.wellName;
                if (wellParamsModel == null)
                {
                    return;
                }
                this.modifyBtn.IsEnabled = true;
                ValuedWellParams(wellParamsModel);
            }
            else
            {
                ClearWellProperty();
            }


        }

        private void ValuedWellParams(WellParamsModel wellParamsModel)
        {
            this.well_id.Text = wellParamsModel.well_id.ToString();
            this.params_s.Text = wellParamsModel.s.ToString();
            this.params_d.Text = (wellParamsModel.d * 1E+4).ToString()+" "+ "/10^4m^3/d";
            this.params_re.Text = wellParamsModel.re.ToString() + " " + "m"; 
            this.params_rw.Text = wellParamsModel.rw.ToString() + " " + "m";
            this.params_rhogsc.Text = wellParamsModel.rhogsc.ToString() + " " + "kg/m3";
            this.params_rhowsc.Text = wellParamsModel.rhowsc.ToString() + " " + "kg/m3";
            this.params_pe.Text = wellParamsModel.pe.ToString() + " " + "MPa";
        }

        /// <summary>
        /// 清空well Property
        /// </summary>
        public void ClearWellProperty()
        {
            this.wellName.Text = null;
            this.wellCode.Text = null;
            this.wellAddress.Text = null;
            this.wellDetail.Text = null;
            this.pvtName.Text = null;

            this.wellName1.Text = null;
            this.params_s.Text = null;
            this.params_d.Text = null;
            this.params_re.Text = null;
            this.params_rw.Text = null;
            this.params_rhogsc.Text = null;
            this.params_rhowsc.Text = null;
            this.params_pe.Text = null;
            this.modifyBtn.IsEnabled = false;

        }

        public void KillProcess(int PID)
        {
            if (PID == -1)
            {
                MessageBox.Show("PID不存在", "错误");
                return;
            }

            Process[] process = Process.GetProcesses();

            foreach (Process prs in process)
            {
                if (prs.Id == PID)
                {
                    prs.Kill();
                    break;
                }
            }
        }

        private async void Connect_Click(object sender, EventArgs e)
        {
            if (Const.PYTHONPATH == "")
            {
                MessageBox.Show("未找到Python.exe", "错误");
                return;
            }
            string sArguments = @"python/server.py";//这里是python的文件名字

            if (Connect_Button.Content.ToString() != "未连接")
            {
                KillProcess(PID);
            }
            await Task.Delay(2000);
            Connect_Button.Content = "正在连接...";
            Connect_Button.Background = new SolidColorBrush(Color.FromRgb(30, 159, 255));

            Task<int> task1 = RunPythonScript(sArguments, "-u");

            Task task2 = new Task(() =>
            {
                flaskStatus();
            });

            task2.Start();

            PID = task1.Result;

            Console.WriteLine("PID: " + PID);
        }

        /// <summary>
        /// 外部程序退出事件
        /// </summary>
        public void proc_Exited(object sender, EventArgs e)
        {
            Action action1 = () =>
            {
                Connect_Button.Content = "未连接";
                Connect_Button.Background = new SolidColorBrush(Color.FromRgb(255, 87, 34));
            };
            Connect_Button.Dispatcher.BeginInvoke(action1);
        }

        //调用python核心代码
        public async Task<int> RunPythonScript(string sArgName, string args = "", params string[] teps)
        {
            Console.WriteLine("python: " + Thread.CurrentThread.ManagedThreadId.ToString());
            string strProcessName = "";
            string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + sArgName;// 获得python文件的绝对路径（将文件放在c#的debug文件夹中可以这样操作）
            string sArguments = path;
            foreach (string sigstr in teps)
            {
                sArguments += " " + sigstr;//传递参数
            }
            sArguments += " " + args;

            ProcessStartInfo pInfo = new ProcessStartInfo
            {
                FileName = Const.PYTHONPATH,
                Arguments = sArguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            Process process = new Process();
            try
            {
                process.StartInfo = pInfo;
                process.Start();
                if (process != null)
                {
                    // 监视进程退出
                    process.EnableRaisingEvents = true;
                    // 指定退出事件方法
                    process.Exited += new EventHandler(proc_Exited);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "哎呀，崩溃了...");
            }
            return process.Id;
        }

        public async void flaskStatus()
        {
            Console.WriteLine("flask: " + Thread.CurrentThread.ManagedThreadId.ToString());
            await Task.Delay(8000);
            Console.WriteLine("开始");
            int timeout = 20;
            HttpWebResponse res;
            while (timeout > 0)
            {
                Console.WriteLine(timeout);
                //检查进程是否已经启动，已经启动则显示报错信息退出程序。 
                string url = "http://127.0.0.1:5000";
                res = Network.CreateGetHttpResponse(url, null, null, null);
                if (res != null && (int)res.StatusCode == 200)
                {
                    Action action1 = () =>
                    {
                        Connect_Button.Content = "已连接";
                        Connect_Button.Background = new SolidColorBrush(Color.FromRgb(0, 150, 136));
                    };
                    await Connect_Button.Dispatcher.BeginInvoke(action1);
                    break;

                }
                timeout--;
                await Task.Delay(1000);
            }
        }

        private void ReadConfig()
        {
            if (File.Exists(Const.CONFIGPATH))
            {
                var MyIni = new IniFile(Const.CONFIGPATH);
                string ip = MyIni.Read("IP", "MySQL");
                string username = MyIni.Read("username", "MySQL");
                string password = MyIni.Read("password", "MySQL");
                string dbName = MyIni.Read("dbName", "MySQL");
                string pythonPath = MyIni.Read("pythonPath", "Python");
                Const.IP = ip;
                Const.USER = username;
                Const.PWD = password;
                Const.DBNAME = dbName;
                Const.PYTHONPATH = pythonPath;
            }
            else
            {
                if (MessageBox.Show("请填写设置", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Setting setting = new Setting();
                    setting.ShowDialog();
                }
            }

        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            Setting setting = new Setting();
            setting.ShowDialog();
        }

        private void CreateFolder()
        {
            string basePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string picPath = "pic/";
            string confPath = "conf/";
            string logPath = "log/";
            CreateFile(basePath + picPath);
            CreateFile(basePath + confPath);
            CreateFile(basePath + logPath);
        }

        private void CreateFile(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
        }

        private void CloseProject_CLick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 修改井的基本参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModifyBtn_Click(object sender, RoutedEventArgs e)
        {
            string wellId = well_id.Text;
            string name = wellName1.Text;

            if (wellId == null)
            {
                return;
            }
            WellBasicParams wellBasicParams = new WellBasicParams(wellId, 0);
            wellBasicParams.GetUpdateParams += (wellParams) =>
            {
                this.wellName1.Text = name;
                WellParamsModel wellParamsModel = DbRead.GetWellParams(wellId);
                ValuedWellParams(wellParamsModel);
            };
            wellBasicParams.ShowDialog();
        }
    }
}
