using GasWaterWell.utils;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TabControlWithClose;

namespace GasWaterWell.views
{
    class AddPhaseSeepage
    {
        string phaseSeepageIndex;
        string name;
        public AddPhaseSeepage(string phase_seepage_index, string name)
        {
            this.phaseSeepageIndex = phase_seepage_index;
            this.name = name;
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
                Content = "导出到Excel",
                Tag = "excel"
            };
            //ComboBoxItem item2 = new ComboBoxItem()
            //{
            //    Content = "导出到CSV"
            //};
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
            //grid.Children.Add(updateBtn);

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

        DataTable dt;
        /// <summary>
        /// 生成 气水TabItem
        /// </summary>
        /// <returns></returns>
        public UCTabItemWithClose addItem()
        {

            UCTabItemWithClose item = new UCTabItemWithClose();
            item.Header = name;
            item.Tag = this.phaseSeepageIndex;
            item.ToolTip = name;
            item.Margin = new Thickness(0, 0, 1, 0);
            item.Height = 28;
            StackPanel sPanel = new StackPanel();
            //计算结果 TODO：绑定数据DataTable
            string sql = string.Format("SELECT * FROM phase_seepage " +
                             "where phase_seepage_index = {0}",
                             phaseSeepageIndex);

            Console.WriteLine(sql);

            dt = DbManager.Ins.ExcuteDataTable(sql);
            if (dt == null)
            {
                return item;
            }

            try
            {
                //sPanel.Children.Add(addPicture(dt.Rows[0]["pic_path_1"].ToString(), dt.Rows[0]["pic_path_2"].ToString()));

                dt.Columns.Remove("phase_seepage_id");
                dt.Columns.Remove("phase_seepage_index");
                dt.Columns.Remove("phase_seepage_name");
                dt.Columns.Remove("project_id");
                dt.Columns["sw"].ColumnName = "Sw";
                dt.Columns["krg"].ColumnName = "Krg";
                dt.Columns["krw"].ColumnName = "Krw";


                sPanel.Children.Add(UI_Components.addGridView("计算结果", dt));//
                                                                           //按扭区 TODO：添加事件
                sPanel.Children.Add(addButton());
                ScrollViewer myScrollViewer = new ScrollViewer();
                myScrollViewer.Content = sPanel;
                item.Content = myScrollViewer;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "哎呀，崩溃了...");
            }


            return item;
        }
    }   
}
