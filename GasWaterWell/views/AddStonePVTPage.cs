
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
    class AddStonePVTPage
    {
        string name;
        string id;
        PVTRockModel pVTRockModel;
        ObservableCollection<PVTRockModel> memberData;
        DataTable dt;
        public AddStonePVTPage(string id,string name)
        {
            this.name = name;
            this.id = id;
            pVTRockModel = DbRead.GetPVTRock(name);
            dt = DbRead.GetPVTRockTable(name);
            dt.Columns.Remove("project_id");
            dt.Columns.Remove("rock_id");
            dt.Columns.Remove("rock_name");
            dt.Columns.Remove("kxd");

            dt.Columns["ysysxs1"].ColumnName = "岩石（孔隙体积）的压缩系数Sandstone";
            dt.Columns["ysysxs2"].ColumnName = "岩石（孔隙体积）的压缩系数Limestone";
            memberData = new ObservableCollection<PVTRockModel>();
            memberData.Add(pVTRockModel);
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
            sPanel.Children.Add(UI_Components.addGridView("计算结果", dt));
            //按扭区 TODO：添加事件
            sPanel.Children.Add(addButton());
            ScrollViewer myScrollViewer = new ScrollViewer();
            myScrollViewer.Content = sPanel;
            item.Content = myScrollViewer;
            return item;
        }

        //string avrPorosity = "2793487";//平均孔隙度
        private Grid addInputResult()
        {
            Grid grid = new Grid();
            grid.Children.Add(UI_Components.addRectangle(new Thickness(67, 20, 5, 60)));
            grid.Children.Add(UI_Components.addTextBlock("输入结果", new Thickness(5,10,0,0),14));
            grid.Children.Add(UI_Components.addTextBlock("储集层岩石平均孔隙度：", new Thickness(23, 48, 0, 0)));
            grid.Children.Add(UI_Components.addTextBox(pVTRockModel.rock.ToString(), new Thickness(162, 46, 0, 0), 12, 188));
            return grid;
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
    }
}
