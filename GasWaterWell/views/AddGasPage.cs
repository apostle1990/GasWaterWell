using GasWaterWell.Model;
using GasWaterWell.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TabControlWithClose;

namespace GasWaterWell.views
{
    class AddGasPage
    {
        string wellId;
        string inputId;
        string resultId;
        string name;
        WellParamsModel wellParamsModel;

        public AddGasPage(string wellId, string inputId, string resultId, string name)
        {
            this.wellId = wellId;
            this.name = name;
            this.inputId = inputId;
            this.resultId = resultId;
            wellParamsModel = DbRead.GetWellParams(wellId);
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
            //获取井Model
            WellModel wellModel = DbRead.GetWellModelByWellId(this.wellId.ToString());

            // 获取井参数DT
            DataTable WellParmasDt = DbRead.GetWellParmasTableByWellId(this.wellId.ToString());
            // 获取气水相渗
            //DataTable PhaseSeepageDt = DbRead.GetPhaseSeepageByWellid(this.well_id.ToString());
            //获取PvtGas
            DataTable PvtGasDt = DbRead.GetPVTGasTable(wellModel.pvt_gas_name);

            List<DataTable> FinalDataTableList = new List<DataTable>();
            FinalDataTableList.Add(WellParmasDt);
            FinalDataTableList.Add(PvtGasDt);
            FinalDataTableList.Add(dt);
            string[] names = { "井参数", "PvtGas", "结果" };
            ComboBoxItem comboxItem = comBox.SelectedItem as ComboBoxItem;
            if (comboxItem.Tag.ToString() == "excel")
            {
                if (Utils.ExportExcel(FinalDataTableList, names))
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
            grid1.Children.Add(UI_Components.addTextBlock("输入结果", new Thickness(5, 5, 0, 0), 14));
            grid1.Children.Add(UI_Components.addRectangle(new Thickness(70, 12, 5, 160)));

            grid1.Children.Add(UI_Components.addTextBlock("气体密度:", new Thickness(25, 43, 0, 0)));
            grid1.Children.Add(UI_Components.addTextBox(wellParamsModel.rhogsc.ToString(), new Thickness(94, 42, 0, 0), 12, 205));
            grid1.Children.Add(UI_Components.addTextBlock("kg/m3", new Thickness(305, 43, 0, 0)));

            grid1.Children.Add(UI_Components.addTextBlock("水的密度:", new Thickness(456, 43, 0, 0)));
            grid1.Children.Add(UI_Components.addTextBox(wellParamsModel.rhowsc.ToString(), new Thickness(525, 42, 0, 0), 12, 165));
            grid1.Children.Add(UI_Components.addTextBlock("kg/m3", new Thickness(699, 43, 0, 0)));

            grid1.Children.Add(UI_Components.addTextBlock("气井表皮系数:", new Thickness(25, 80, 0, 0)));
            grid1.Children.Add(UI_Components.addTextBox(wellParamsModel.s.ToString(), new Thickness(128, 80, 0, 0), 12, 169));

            grid1.Children.Add(UI_Components.addTextBlock("非达西渗流系数:", new Thickness(434, 79, 0, 0)));
            grid1.Children.Add(UI_Components.addTextBox(wellParamsModel.d.ToString(), new Thickness(525, 79, 0, 0), 12, 165));
            grid1.Children.Add(UI_Components.addTextBlock("/10^4m^3/d", new Thickness(699, 79, 0, 0)));

            grid1.Children.Add(UI_Components.addTextBlock("井筒半径:", new Thickness(25, 120, 0, 0)));
            grid1.Children.Add(UI_Components.addTextBox(wellParamsModel.re.ToString(), new Thickness(100, 119, 0, 0), 12, 165));
            grid1.Children.Add(UI_Components.addTextBlock("m", new Thickness(270, 120, 0, 0)));

            grid1.Children.Add(UI_Components.addTextBlock("井控半径:", new Thickness(459, 120, 0, 0)));
            grid1.Children.Add(UI_Components.addTextBox(wellParamsModel.rw.ToString(), new Thickness(524, 114, 0, 0), 12, 165));
            grid1.Children.Add(UI_Components.addTextBlock("m", new Thickness(695, 120, 0, 0)));

            //grid1.Children.Add(UI_Components.addTextBlock("原始地层压力:", new Thickness(25, 159, 0, 0)));
            //grid1.Children.Add(UI_Components.addTextBox(gasWaterTwoModel.pi.ToString(), new Thickness(111, 159, 0, 0), 12, 184));
            //grid1.Children.Add(UI_Components.addTextBlock("MPa", new Thickness(298, 161, 0, 0)));

            return grid1;
        }

        /// <summary>
        /// 添加pictures
        /// </summary>
        /// <param name="url1"></param>
        /// <param name="url2"></param>
        /// <returns></returns>
        public Grid addPicture(string url1, string url2)
        {
            try
            {
                Grid grid = new Grid();
                if (url1 == null)
                {
                    return grid;
                }
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                Image img1 = new Image()
                {
                    Margin = new Thickness(0, 25, 0, 0),

                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Source = new BitmapImage(new Uri(url1, UriKind.Absolute)),
                };
                grid.Children.Add(img1);
                Grid.SetColumn(img1, 0);
                if (url2 == null)
                {
                    return grid;
                }
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                Image img2 = new Image()
                {
                    Margin = new Thickness(0, 25, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Source = new BitmapImage(new Uri(url2, UriKind.Absolute)),
                };
                grid.Children.Add(img2);
                Grid.SetColumn(img2, 1);
                return grid;
            }
            catch (Exception)
            {
                MessageBox.Show("请检查是否连接python或数据库!");
                return null;
            }

        }

        public Grid addPicture(string url1)
        {
            try
            {
                Grid grid = new Grid();
                if (url1 == null)
                {
                    return grid;
                }
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                Image img1 = new Image()
                {
                    Margin = new Thickness(0, 25, 0, 0),

                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Source = new BitmapImage(new Uri(url1, UriKind.Absolute)),
                };
                grid.Children.Add(img1);
                Grid.SetColumn(img1, 0);
                return grid;
            }
            catch (Exception)
            {
                MessageBox.Show("请检查是否连接python或数据库!");
                return null;
            }

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
            item.Tag = this.inputId;
            item.ToolTip = name;
            item.Margin = new Thickness(0, 0, 1, 0);
            item.Height = 28;
            StackPanel sPanel = new StackPanel();
            sPanel.Children.Add(addInputResult());
            //计算结果 TODO：绑定数据DataTable
            string sql = string.Format("SELECT * FROM gas_result_data " +
                             "where gas_result_index = {0} and gas_product_index = {1}",
                             resultId, inputId);

            Console.WriteLine(sql);

            dt = DbManager.Ins.ExcuteDataTable(sql);
            if(dt == null)
            {
                return item;
            }

            try
            {
                if (dt.Rows[0]["pic_path_2"].ToString() == "")
                {
                    sPanel.Children.Add(addPicture(dt.Rows[0]["pic_path_1"].ToString()));
                }
                else
                {
                    sPanel.Children.Add(addPicture(dt.Rows[0]["pic_path_1"].ToString(), dt.Rows[0]["pic_path_2"].ToString()));
                }

                dt.Columns.Remove("gas_result_id");
                dt.Columns.Remove("gas_result_index");
                dt.Columns.Remove("gas_product_index");
                dt.Columns.Remove("pic_path_1");
                dt.Columns.Remove("pic_path_2");
                dt.Columns.Remove("type");
                dt.Columns["date"].ColumnName = "日期";
                dt.Columns["qaof"].ColumnName = "无阻流量";
                dt.Columns["kh"].ColumnName = "KH/(mD.m)";
                dt.Columns["a"].ColumnName = "A";
                dt.Columns["b"].ColumnName = "B";


                sPanel.Children.Add(UI_Components.addGridView("计算结果", dt));//
                                                                           //按扭区 TODO：添加事件
                sPanel.Children.Add(addButton());
                ScrollViewer myScrollViewer = new ScrollViewer();
                myScrollViewer.Content = sPanel;
                item.Content = myScrollViewer;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "哎呀，崩溃了...");
            }

            
            return item;
        }
    }
}
