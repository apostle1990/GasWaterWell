
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
    class AddGasPVTPage
    {
        string name;
        string id;
        List<PVTGasModel> pVTGasModels;
        ObservableCollection<PVTGasModel> memberData;
        DataTable dt;
        public AddGasPVTPage(string id,string name)
        {
            this.name = name;
            this.id = id;
            pVTGasModels = DbRead.GetPVTGas(name);
            dt = DbRead.GetPVTGasTable(name);
            
            memberData = new ObservableCollection<PVTGasModel>(pVTGasModels);
        }

        ComboBox comBox; 
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
                Tag="excel",
                Content = "导出到Excel"
            };
            ComboBoxItem item2 = new ComboBoxItem()
            {
                Content = "导出到CSV",
                Tag = "csv",
            };
            comBox= new ComboBox()
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
            //grid.Children.Add(updateBtn);

            return grid;
        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem comboxItem = comBox.SelectedItem as ComboBoxItem;
            if (comboxItem.Tag.ToString()=="excel")
            {
                if (Utils.ExportExcel(dt))
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

        public Grid addInputResult()
        {
            Grid grid1 = new Grid();
            //
            grid1.Children.Add(UI_Components.addTextBlock("输入结果",new Thickness(5,5,0,0),14));
            grid1.Children.Add(UI_Components.addRectangle(new Thickness(70, 12, 5, 130)));

            grid1.Children.Add(UI_Components.addTextBlock("二氧化碳摩尔分数:", new Thickness(35, 32, 0, 0)));            
            grid1.Children.Add(UI_Components.addTextBox(pVTGasModels[0].yco2.ToString(), new Thickness(148, 30, 0, 0),12,188));
            grid1.Children.Add(UI_Components.addTextBlock("f", new Thickness(342, 32, 0, 0)));

            grid1.Children.Add(UI_Components.addTextBlock("天然气相对密度:", new Thickness(410, 32, 0, 0)));
            grid1.Children.Add(UI_Components.addTextBox(pVTGasModels[0].yg.ToString(), new Thickness(510, 30, 0, 0),12,188));
            grid1.Children.Add(UI_Components.addTextBlock("Dless", new Thickness(705, 32, 0, 0)));

            grid1.Children.Add(UI_Components.addTextBlock("气藏温度:", new Thickness(35, 67, 0, 0)));
            grid1.Children.Add(UI_Components.addTextBox(pVTGasModels[0].t.ToString(), new Thickness(98, 65, 0, 0),12,188));
            grid1.Children.Add(UI_Components.addTextBlock("Dless", new Thickness(290, 67, 0, 0)));

            grid1.Children.Add(UI_Components.addTextBlock("气藏水矿化度:", new Thickness(410, 67, 0, 0)));
            grid1.Children.Add(UI_Components.addTextBox(pVTGasModels[0].nacl.ToString(), new Thickness(505, 65, 0, 0),12,188));
            grid1.Children.Add(UI_Components.addTextBlock("%", new Thickness(700, 67, 0, 0)));

            grid1.Children.Add(UI_Components.addTextBlock("气藏压力", new Thickness(5, 95, 0, 0)));

            double max = (pVTGasModels[pVTGasModels.Count - 1].p) / 1000000;
            double min = (pVTGasModels[0].p) / 1000000;
            grid1.Children.Add(UI_Components.addTextBlock("MAX(最大值):", new Thickness(35, 125, 0, 0)));            
            grid1.Children.Add(UI_Components.addTextBox(max.ToString(), new Thickness(130, 123, 0, 0)));
            grid1.Children.Add(UI_Components.addTextBlock("MPa", new Thickness(226, 125, 0, 0)));

            grid1.Children.Add(UI_Components.addTextBlock("MIN(最小值):", new Thickness(310, 125, 0, 0)));
            grid1.Children.Add(UI_Components.addTextBox(min.ToString(), new Thickness(405, 123, 0, 0)));
            grid1.Children.Add(UI_Components.addTextBlock("MPa", new Thickness(500, 125, 0, 0)));

            //grid1.Children.Add(UI_Components.addTextBlock("粒度:", new Thickness(630, 125, 0, 0)));
            //grid1.Children.Add(UI_Components.addTextBox(GasPreInterval, new Thickness(675, 123, 0, 0)));
            return grid1;
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
            //
            sPanel.Children.Add(UI_Components.addGridView("计算结果", dt));
            //按扭区 TODO：添加事件
            sPanel.Children.Add(addButton());
            ScrollViewer myScrollViewer = new ScrollViewer();
            myScrollViewer.Content = sPanel;
            item.Content = myScrollViewer;  
            return item;
        }
    }
}