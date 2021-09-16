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
    public partial class MainWindowCopy : Window
    {
        public object GetPvtByName { get; private set; }
        int PID = -1;

        public MainWindowCopy()
        {
            InitializeComponent();
            LoadTreeView();
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
                if (tp.Header == tabItem.Header)
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
                mnuItem1.Header = "新建项目";
                mnuItem1.Click += new RoutedEventHandler(CreateWell_Click);
                MenuItem mnuItem2 = new MenuItem();
                mnuItem2.Header = "打开";

                ContextMenu menu = new ContextMenu() { };
                menu.Items.Add(mnuItem1);
                menu.Items.Add(mnuItem2);

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

                MenuItem mnuItem3 = new MenuItem();
                mnuItem3.Header = "导入";
                menu.Items.Add(mnuItem1);
                menu.Items.Add(mnuItem2);
                menu.Items.Add(mnuItem3);
                (sender as TreeViewItem).ContextMenu = menu;
            }
            #region PVTItem
            else if (type.ToUpper() == "PVTITEM")
            {
                string clss = selectedElement.Attributes["Class"].Value;
                ContextMenu menu = new ContextMenu() { };
                MenuItem mnuItem1 = new MenuItem();
                mnuItem1.Header = "移除";
                mnuItem1.Click += new RoutedEventHandler(MovePVT_Click);
                MenuItem mnuItem2 = new MenuItem();
                mnuItem2.Header = "删除";
                mnuItem1.Click += new RoutedEventHandler(DeletePVT_Click);
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
                MenuItem mnuItem1 = new MenuItem();
                mnuItem1.Header = "移除";
                MenuItem mnuItem2 = new MenuItem();
                mnuItem2.Header = "删除";
                MenuItem mnuItem3 = new MenuItem();
                mnuItem3.Header = "重命名";
                menu.Items.Add(mnuItem1);
                menu.Items.Add(mnuItem2);
                menu.Items.Add(mnuItem3);
                (sender as TreeViewItem).ContextMenu = menu;
            }
            else if (type.ToUpper() == "PRESSMETHOD")
            {
                ContextMenu menu = new ContextMenu() { };
                MenuItem mnuItem1 = new MenuItem();
                mnuItem1.Header = "添加";
                MenuItem child1 = new MenuItem();
                child1.Header = "试井分析";
                child1.Click += new RoutedEventHandler(WellAnalysis_CLick);
                MenuItem child2 = new MenuItem();
                child2.Header = "产能预测";
                child2.Click += new RoutedEventHandler(WellPredict_CLick);
                mnuItem1.Items.Add(child1);
                mnuItem1.Items.Add(child2);
                menu.Items.Add(mnuItem1);
                (sender as TreeViewItem).ContextMenu = menu;
            }
            else if (type.ToUpper() == "GASWATERTWOMETHOD")
            {
                ContextMenu menu = new ContextMenu() { };
                MenuItem mnuItem1 = new MenuItem();
                mnuItem1.Header = "添加";
                MenuItem child1 = new MenuItem();
                child1.Header = "水气两项方法";
                child1.Click += new RoutedEventHandler(GasWater_Click);
                mnuItem1.Items.Add(child1);
                menu.Items.Add(mnuItem1);
                (sender as TreeViewItem).ContextMenu = menu;
            }
            else if (type.ToUpper() == "PRESSMETHODITEM")
            {
                ContextMenu menu = new ContextMenu() { };
                MenuItem mnuItem1 = new MenuItem();
                mnuItem1.Header = "移除";
                MenuItem mnuItem2 = new MenuItem();
                mnuItem2.Header = "删除";
                MenuItem mnuItem3 = new MenuItem();
                mnuItem3.Header = "重命名";
                menu.Items.Add(mnuItem1);
                menu.Items.Add(mnuItem2);
                menu.Items.Add(mnuItem3);
                (sender as TreeViewItem).ContextMenu = menu;
            }
            else if(type.ToUpper() == "GASWATERMETHODITEM")
            {
                ContextMenu menu = new ContextMenu() { };
                MenuItem mnuItem1 = new MenuItem();
                mnuItem1.Header = "移除";
                MenuItem mnuItem2 = new MenuItem();
                mnuItem2.Header = "删除";
                MenuItem mnuItem3 = new MenuItem();
                mnuItem3.Header = "重命名";
                menu.Items.Add(mnuItem1);
                menu.Items.Add(mnuItem2);
                menu.Items.Add(mnuItem3);
                (sender as TreeViewItem).ContextMenu = menu;
            }
        }


        public Action<string,string>GetPVTTypeAndParentId;//str1 parentId,str2 clss 
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenGasPVT_CLick(object sender, RoutedEventArgs e)
        {
            OpenPVTWindow openPVT = new OpenPVTWindow(OperateXML.GetProjectId(),"gas");
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
            System.Xml.XmlElement xmlElement = (XmlElement)proSourceTree.SelectedItem;
            //string id = xmlElement.Attributes["Id"].Value.ToString();
            XmlElement parentNode = (XmlElement)xmlElement.ParentNode;
            string parentId = parentNode.GetAttribute("Id");
            Predict predict = new Predict(parentId);
            predict.ShowDialog();
        }

        /// <summary>
        /// 试井分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WellAnalysis_CLick(object sender, RoutedEventArgs e)
        {
            XmlElement xmlElement = (XmlElement)proSourceTree.SelectedItem;
            //string id = xmlElement.Attributes["Id"].Value.ToString();
            XmlElement parentNode =(XmlElement) xmlElement.ParentNode;
            string parentId = parentNode.GetAttribute("Id");
            string type = xmlElement.Attributes["Type"].Value.ToString();
            Fitting fit = new Fitting(parentId);
            fit.GetFittingId += (fitId) =>
            {
                string name = "压力平方法" + Utils.UnixTimeStampToDateTime(Convert.ToDouble(fitId)) + ".prm";
                OperateXML.XmlInsertPressMethod(parentId,fitId, name);
                WellOrFiledPage tabItem = new WellOrFiledPage(fitId, name);
                LoadTreeView();
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
            newWell.GetWellIdAndPvtName += (wellId, wellName, pvtName) => {
                OperateXML.XmlInsertWell(wellId, wellName);
                LoadTreeView();
            };
            newWell.ShowDialog();
        }

        /// <summary>
        /// 删除PVT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeletePVT_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 移除PVT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MovePVT_Click(object sender, RoutedEventArgs e)
        {
            System.Xml.XmlElement xmlElement = (XmlElement)proSourceTree.SelectedItem;
            string id = xmlElement.Attributes["Id"].Value.ToString();
            string type = xmlElement.Attributes["Type"].Value.ToString();
            OperateXML.XmlNodeDelete(type, id);
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
                OperateXML.XmlInsertPVTEle( pvtName, pvtName, "water");
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
                OperateXML.XmlInsertPVTEle(id, pvtName, "stone");
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
                    Console.WriteLine(e.Message);
                    return null;
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
            #region Well
            else if (type.ToUpper() == "PRESSMETHODITEM")
            {                             
                WellOrFiledPage tabItem = new WellOrFiledPage(selectedElement.Attributes["Id"].Value, attName);
                if (!IsCopiedTabItem(tabItem.addItem()))
                {
                    mainTab.Items.Add(tabItem.addItem());
                }
                getTabPageIndex(tabItem.addItem());
                
            }else if (type.ToUpper() == "GASWATERTWOMETHODITEM")
            {
                string gasWaterMId = selectedElement.Attributes["Id"].Value;
                string gasWaterMResultId = selectedElement.Attributes["ResultId"].Value;
                //TODO 添加气水两相
                AddGasWaterPage tabItem = new AddGasWaterPage(gasWaterMId, gasWaterMResultId, attName);
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
                if(OperateDB.InsertProject(proId, proName) > 0)
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
            XmlElement xmlElement = (XmlElement)proSourceTree.SelectedItem;
            XmlElement parentNode = (XmlElement)xmlElement.ParentNode;
            string parentId = parentNode.GetAttribute("Id");
            if (parentId != null)
            {
                GasWaterTwoElephants gasWaterTwoElephants = new GasWaterTwoElephants(int.Parse(parentId));
                gasWaterTwoElephants.GetInputIdAndResultIndex += (gasWaterInputId, gasWaterResultId) =>
                {
                    string name = "水气两相法" + Utils.UnixTimeStampToDateTime(Convert.ToDouble(gasWaterInputId)) + ".gwm";
                    OperateXML.XmlInsertGasWaterMethod(parentId, gasWaterResultId, gasWaterInputId, name);
                    LoadTreeView();
                    AddGasWaterPage tabItem = new AddGasWaterPage(gasWaterInputId, gasWaterResultId, name);
                    if (!IsCopiedTabItem(tabItem.addItem()))
                    {
                        mainTab.Items.Add(tabItem.addItem());
                    }
                    getTabPageIndex(tabItem.addItem());
                };
                gasWaterTwoElephants.ShowDialog();
            }

            
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
                Const.DBSTR = String.Format(Const.DBSTR, ip, username, password, dbName);
                Const.PYTHONPATH = pythonPath;
            }
            else
            {
                if (MessageBox.Show("请填写设置", "错误", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
    }
}
