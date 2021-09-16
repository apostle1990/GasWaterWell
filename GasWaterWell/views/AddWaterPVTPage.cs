
using GasWaterWell.Model;
using GasWaterWell.utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TabControlWithClose;

namespace GasWaterWell.views
{
    class AddWaterPVTPage
    {
        string id;
        string name;
        List<PVTWaterModel> pVTWaterModels;
        ObservableCollection<PVTWaterModel> memberData;
        DataTable dataTable;
        ComboBox comBox;
        public AddWaterPVTPage(string id, string name)
        {
            this.id = id;
            this.name = name;
            dataTable = DbRead.GetPVTWaterTable(this.name);
            
            pVTWaterModels = DbRead.GetPVTWater(this.name);
            memberData = new ObservableCollection<PVTWaterModel>(pVTWaterModels);

        }
        /// <summary>
        /// 生成 试井TabItem
        /// </summary>
        /// <returns></returns>
        public UCTabItemWithClose addItem()
        {

            UCTabItemWithClose item = new UCTabItemWithClose();
            item.Header = name;
            item.Tag = this.id;
            item.ToolTip = name;
            item.Margin = new Thickness(0, 0, 1, 0);
            item.Height = 28;
            StackPanel sPanel = new StackPanel();
            sPanel.Children.Add(addInputResult());
            
            //计算结果 TODO：绑定数据DataTable
            try
            {
                sPanel.Children.Add(UI_Components.addGridView("计算结果", dataTable));
                //按扭区 TODO：添加事件
                sPanel.Children.Add(addButton());
                ScrollViewer myScrollViewer = new ScrollViewer();
                myScrollViewer.Content = sPanel;
                item.Content = myScrollViewer;
            }
            catch(Exception ex) {
                MessageBox.Show(ex.ToString(), "哎哟，操作出错了");
            }

            return item;
        }

        //string waterSalinity = "2793487";//地层水矿化度
        //string maxPress = "232";  //压力最大值
        //string minPress = "384";//最小值
        //string interval = "12";//粒度
        private Grid addInputResult()
        {
            Grid grid = new Grid();
            grid.Children.Add(UI_Components.addRectangle(new Thickness(67, 20, 5, 110)));
            grid.Children.Add(UI_Components.addTextBlock("输入结果", new Thickness(5, 10, 0, 0), 14));
            grid.Children.Add(UI_Components.addTextBlock("地层水矿化度：", new Thickness(26, 45, 0, 0)));
            grid.Children.Add(UI_Components.addTextBox(pVTWaterModels[0].nacl.ToString(), new Thickness(112, 42, 0, 0)));
            grid.Children.Add(UI_Components.addTextBlock("%", new Thickness(205, 45, 0, 0)));

            grid.Children.Add(UI_Components.addTextBlock("地层温度：", new Thickness(370, 45, 0, 0)));
            grid.Children.Add(UI_Components.addTextBox(pVTWaterModels[0].T.ToString(), new Thickness(428, 42, 0, 0),12,188));
            grid.Children.Add(UI_Components.addTextBlock("摄氏度", new Thickness(629, 45, 0, 0)));

            grid.Children.Add(UI_Components.addTextBlock("地层水压力", new Thickness(26, 79, 0, 0)));

            grid.Children.Add(UI_Components.addTextBlock("MAX(最大压力)：", new Thickness(26, 103, 0, 0)));
            grid.Children.Add(UI_Components.addTextBox(pVTWaterModels[pVTWaterModels.Count-1].p.ToString(), new Thickness(126, 100, 0, 0),12,102));
            grid.Children.Add(UI_Components.addTextBlock("MPa", new Thickness(237, 103, 0, 0)));

            grid.Children.Add(UI_Components.addTextBlock("MIN(最小压力)：", new Thickness(370, 103, 0, 0)));
            grid.Children.Add(UI_Components.addTextBox(pVTWaterModels[0].p.ToString(), new Thickness(467, 100, 0, 0)));
            grid.Children.Add(UI_Components.addTextBlock("MPa", new Thickness(560, 103, 0, 0)));

            //grid.Children.Add(UI_Components.addTextBlock("粒度（间隔）：", new Thickness(640, 103, 0, 0)));
            //grid.Children.Add(UI_Components.addTextBox(interval, new Thickness(725, 100, 0, 0)));
            return grid;
        }

        public Grid addButton()
        {
            Grid grid = new Grid();
            Button exportBtn = new Button()
            {
                Content = "导出",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 65,
                Height = 25,
                Margin = new Thickness(5, 5, 0, 0),
                Foreground = Brushes.White,
                Background = (Brush)new BrushConverter().ConvertFrom("#FF5E5E5E"),
                BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FF5E5E5E")
            };
            exportBtn.Click += new RoutedEventHandler(ExportBtn_Click);
            Button updateBtn = new Button()
            {
                Content = "修改",
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 65,
                Height = 25,
                Margin = new Thickness(0, 5, 5, 0),
                Foreground = Brushes.White,
                Background = (Brush)new BrushConverter().ConvertFrom("#FF5E5E5E"),
                BorderBrush = (Brush)new BrushConverter().ConvertFrom("#FF5E5E5E")
            };
            ComboBoxItem item1 = new ComboBoxItem()
            {
                IsSelected = true,
                Tag = "excel",
                Content = "导出到Excel"
            };
            ComboBoxItem item2 = new ComboBoxItem()
            {
                Content = "导出到CSV",
                Tag = "csv",
            };
            comBox = new ComboBox()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 120,
                Height = 22,
                Margin = new Thickness(75, 5, 0, 0)
            };
            comBox.Items.Add(item1);
            //comBox.Items.Add(item2);

            grid.Children.Add(exportBtn);
            grid.Children.Add(comBox);
            return grid;
        }


        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem comboxItem = comBox.SelectedItem as ComboBoxItem;
            if (comboxItem.Tag.ToString() == "excel")
            {
                if (Utils.ExportExcel(dataTable))
                {
                    MessageBox.Show("导出完成！", "成功");
                }
                else
                {
                    MessageBox.Show("导出失败！", "失败");
                }
            }
            //else if (comboxItem.Tag.ToString() == "csv")
            //{
            //    Utils.ExprotCsv(dg);
            //}
        }
    }
}