
using GasWaterWell.Model;
using GasWaterWell.utils;
using GasWaterWell.views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TabControlWithClose;

namespace GasWaterWell
{
    class WellOrFiledPage
    {
        UCTabItemWithClose tab_page = new UCTabItemWithClose();
        string id;
        string name;

        string pr = null;
        string a = null;
        string b = null;
        string qaof = null;
        string r2 = null;
        string fitting_pic_path_1 = null;
        string fitting_pic_path_2 = null;
        List<string> qwf = null;
        List<string> inputPwfList = null;
        List<string> pdQgList = null;
        List<string> outputPwfList = null;
        List<string> qgList = null;

        string pred_a_input = null;
        string pred_b_input = null;
        string pred_pr_input = null;
        string pred_qg_input = null;
        string predict_pic_path_1 = null;
        string predict_pic_path_2 = null;
        List<string> pi = null;
        List<string> pi_p = null;
        List<string> k_ki = null;
        List<string> u_pi = null;
        List<string> z_p = null;
        List<string> ai = null;
        List<string> bi = null;
        List<string> pred_qg_output = null;
        List<string> pred_pwf_output = null;

        FittingModel fittingModel;
        PredictModel predictModel;
        public WellOrFiledPage(string fitid,string predId, string name)
        {
            
            this.name = name;

            this.fittingModel = DbRead.GetFitting(fitid);
            this.predictModel = DbRead.GetPredictByPredId(predId);
            if(fittingModel != null)
            {
                this.id = fitid;
                pr = fittingModel.pr;
                a = fittingModel.a;
                b = fittingModel.b;
                qaof = fittingModel.qaof;
                r2 = fittingModel.r2;
                fitting_pic_path_1 = fittingModel.pic_path_1;
                fitting_pic_path_2 = fittingModel.pic_path_2;
                qwf = fittingModel.qwf;
                inputPwfList = fittingModel.inputPwfList;
                pdQgList = fittingModel.pdQgList;
                outputPwfList = fittingModel.outputPwfList;
                qgList = fittingModel.qgList;
            }
            if(predictModel != null)
            {
                this.id = predId;
                pred_a_input = predictModel.pred_a_input;
                pred_b_input = predictModel.pred_b_input;
                pred_pr_input = predictModel.pred_pr_input;
                pred_qg_input = predictModel.pred_qg_input;
                predict_pic_path_1 = predictModel.pic_path_1;
                predict_pic_path_2 = predictModel.pic_path_2;
                pi = predictModel.pi;
                pi_p = predictModel.pi_p;
                k_ki = predictModel.k_ki;
                u_pi = predictModel.u_pi;
                z_p = predictModel.z_p;
                ai = predictModel.ai;
                bi = predictModel.bi;
                pred_qg_output = predictModel.pred_qg_output;
                pred_pwf_output = predictModel.pred_pwf_output;
            }
        }

        /// <summary>
        /// 预测
        /// </summary>
        /// <returns></returns>
        public Grid addInputResult()
        {
            Grid mainGrid = new Grid();
            TextBlock text1 = UI_Components.addTextBlock("输入结果",  new Thickness(5, 5, 0, 0),14);
            

            TextBlock Pi = new TextBlock() {
                Text = "Pi:",
                FontSize = 12,
                Foreground = Brushes.Black,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(35, 35, 0, 0)     
            };  
            TextBlock Qg = new TextBlock() {
                Text = "Qg:",
                FontSize = 12,
                Foreground = Brushes.Black,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(235, 35, 0, 0)
            };           
            TextBlock a = new TextBlock() {
                Text = "a:",
                FontSize = 12,
                Foreground = Brushes.Black,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(35, 70, 0, 0),
                TextWrapping = TextWrapping.Wrap
            };         
            TextBlock b = new TextBlock()
            {
                Text = "b:",
                FontSize = 12,
                Foreground = Brushes.Black,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(235, 70, 0, 0),
                TextWrapping = TextWrapping.Wrap
            };           

            mainGrid.Children.Add(UI_Components.addRectangle(new Thickness(75, 10, 0, 97)));
            mainGrid.Children.Add(text1);
            mainGrid.Children.Add(Pi);
            mainGrid.Children.Add(UI_Components.addTextBox(this.pred_pr_input, new Thickness(65, 35, 0, 0)));
            mainGrid.Children.Add(Qg);
            mainGrid.Children.Add(UI_Components.addTextBox(this.pred_qg_input, new Thickness(275, 35, 0, 0)));
            mainGrid.Children.Add(a);
            mainGrid.Children.Add(UI_Components.addTextBox(this.pred_a_input, new Thickness(65, 70, 0, 0)));
            mainGrid.Children.Add(b);
            mainGrid.Children.Add(UI_Components.addTextBox(this.pred_b_input, new Thickness(275, 70, 0, 0)));
            return mainGrid;

        }



        public Grid addPicture(string url1, string url2)
        {
            Grid grid = new Grid();
            if(url1 == null || url2 == null)
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
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            Image img2 = new Image()
            {
                Margin = new Thickness(0, 25, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Source = new BitmapImage(new Uri(url2, UriKind.Absolute)),
            };
            grid.Children.Add(img1);
            grid.Children.Add(img2);
            Grid.SetColumn(img1, 0);
            Grid.SetColumn(img2, 1);
            return grid;
        }

        ComboBox comBox;
        public Grid addAnalysisButton()
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

        ComboBox comBox2;
        public Grid addPredictButton()
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
            exportBtn.Click += new RoutedEventHandler(ExportBtn2_Click);
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
            //ComboBoxItem item2 = new ComboBoxItem()
            //{
            //    Content = "导出到CSV",
            //    Tag = "csv",
            //};
            comBox2 = new ComboBox()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 120,
                Height = 22,
                Margin = new Thickness(75, 5, 0, 0)
            };
            comBox2.Items.Add(item1);
            //comBox2.Items.Add(item2);

            grid.Children.Add(exportBtn);
            grid.Children.Add(comBox2);
            return grid;
        }

        private void ExportBtn2_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem comboxItem = comBox2.SelectedItem as ComboBoxItem;
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

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem comboxItem = comBox.SelectedItem as ComboBoxItem;
            if (comboxItem.Tag.ToString() == "excel")
            {
                if (Utils.ExportExcel(analyDt))
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
        /// 产能预测
        /// </summary>
        /// <returns></returns>
        public TabItem addProductionItem()
        {
            TabItem item = new TabItem()
            {
                Header="产能预测",
                FontSize=12,
                RenderTransformOrigin=new Point(0.5, 0.5),
                VerticalAlignment=VerticalAlignment.Stretch,
                HorizontalAlignment=HorizontalAlignment.Stretch
            };
            StackPanel sPanel = new StackPanel();
            //数据输入          
            sPanel.Children.Add(addInputResult());
            //结果图
            Grid grid3 = new Grid();
            grid3.Children.Add(UI_Components.addTextBlock("结果图", new Thickness(5, 5, 0, 0),14));
            grid3.Children.Add(UI_Components.addRectangle( new Thickness(70, 12, 5, 450)));
            grid3.Children.Add(addPicture(this.predict_pic_path_1, this.predict_pic_path_2));
            sPanel.Children.Add(grid3);
            //结算结果 TODO：1、这里需要添加数据源参数DataTable类型数据
            dt= new DataTable();
            dt.Columns.Add(new DataColumn("Pi"));
            dt.Columns.Add(new DataColumn("Pi-P"));
            dt.Columns.Add(new DataColumn("K-Ki"));
            dt.Columns.Add(new DataColumn("μ（P）"));
            dt.Columns.Add(new DataColumn("Z(P)"));
            dt.Columns.Add(new DataColumn("Ai"));
            dt.Columns.Add(new DataColumn("Bi"));
            dt.Columns.Add(new DataColumn("Qg"));
            dt.Columns.Add(new DataColumn("Pwf"));
            dt.Columns.Add(new DataColumn("压差"));
            DataRow dr;
            for (int i = 0; i < this.pi.Count; i++)
            {
                dr = dt.NewRow();
                dr[0] = Utils.FormatString(pi[i].Trim('[', ']', ' '), 0);
                dr[1] = Utils.FormatString(pi_p[i].Trim('[', ']', ' '), 0);
                dr[2] = Utils.FormatString(k_ki[i].Trim('[', ']', ' '), 0);
                dr[3] = Utils.FormatString(u_pi[i].Trim('[', ']', ' '), 0);
                dr[4] = Utils.FormatString(z_p[i].Trim('[', ']', ' '), 0);
                dr[5] = Utils.FormatString(ai[i].Trim('[', ']', ' '), 0);
                dr[6] = Utils.FormatString(bi[i].Trim('[', ']', ' '), 0);
                dr[7] = this.pred_qg_input;
                dr[8] = Utils.FormatString(pred_qg_output[i].Trim('[', ']', ' '), 0);
                dr[9] = Utils.FormatString(pred_pwf_output[i].Trim('[', ']', ' '), 0);
                dt.Rows.Add(dr);
            }

            sPanel.Children.Add(UI_Components.addGridView("计算结果", dt));
            //按钮区 TODO：为给按钮添加事件
            sPanel.Children.Add(addPredictButton());

            //添加到tabPage的内容中
            ScrollViewer myScrollViewer = new ScrollViewer();
            myScrollViewer.Content = sPanel;
            item.Content = myScrollViewer;
            return item;
        }

        DataTable analyDt;
        /// <summary>
        /// 试井分析
        /// </summary>
        /// <returns></returns>
        public TabItem addAnalysisItem()
        {
            TabItem item = new TabItem()
            {
                Header = "试井分析",
                FontSize = 12,
                
                RenderTransformOrigin = new Point(0.5, 0.5),
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            StackPanel sPanel = new StackPanel();
            //数据输入  
            Grid grid1 = new Grid();
            grid1.Children.Add(UI_Components.addTextBlock("输入结果",new Thickness(5, 5, 0, 0),14));
            grid1.Children.Add(UI_Components.addRectangle(new Thickness(70, 12, 5, 54)));

            grid1.Children.Add(UI_Components.addTextBlock("Pr:", new Thickness(35, 32, 0, 0)));
            grid1.Children.Add(UI_Components.addTextBox(this.pr, new Thickness(70, 30, 0, 0)));
            grid1.Children.Add(UI_Components.addTextBlock("MPa", new Thickness(165, 32, 0, 0)));
            sPanel.Children.Add(grid1);
            //结算结果
            //sPanel.Children.Add(addGridView());
            Grid grid2 = new Grid();
            grid2.Children.Add(UI_Components.addTextBlock("计算结果", new Thickness(5, 5, 0, 0),14));
            grid2.Children.Add(UI_Components.addRectangle(new Thickness(70, 12, 5, 54)));
            grid2.Children.Add(UI_Components.addTextBlock("a:",new Thickness(35, 32, 0, 0)));
            grid2.Children.Add(UI_Components.addTextBox(Utils.FormatString(a, 0), new Thickness(70, 30, 0, 0)));
            grid2.Children.Add(UI_Components.addTextBlock("b:", new Thickness(208, 32, 0, 0)));
            grid2.Children.Add(UI_Components.addTextBox(Utils.FormatString(b, 0), new Thickness(232, 30, 0, 0)));
            sPanel.Children.Add(grid2);
            //结果图
            Grid grid3 = new Grid();
            grid3.Children.Add(UI_Components.addTextBlock("结果图", new Thickness(5, 5, 0, 0),14));
            grid3.Children.Add(UI_Components.addRectangle( new Thickness(70, 12, 5, 450)));
            grid3.Children.Add(addPicture(this.fitting_pic_path_1, this.fitting_pic_path_2));
            sPanel.Children.Add(grid3);

            //计算结果 TODO：绑定数据源 数据类型：DataTable 记得把默认的DataTable值去掉
            analyDt = new DataTable();

            //第一部分数据
            analyDt.Columns.Add(new DataColumn("Pr（Mpa）"));
            analyDt.Columns.Add(new DataColumn("a"));
            analyDt.Columns.Add(new DataColumn("b"));
            analyDt.Columns.Add(new DataColumn("Qaof"));
            analyDt.Columns.Add(new DataColumn("R2"));

            DataRow dr;
            dr = analyDt.NewRow();
            dr[0] = this.pr;
            dr[1] = Utils.FormatString(this.a.ToString(), 0);
            dr[2] = Utils.FormatString(this.b.ToString(), 0);
            dr[3] = Utils.FormatString(this.qaof.ToString(), 0);
            dr[4] = Utils.FormatString(this.r2.ToString(), 0);
            analyDt.Rows.Add(dr);

            //第二部分数据
            dr = analyDt.NewRow();
            dr[0] = "Q（万方）";
            dr[1] = "Pwf";
            dr[2] = "Pd/qg";
            analyDt.Rows.Add(dr);

            Console.WriteLine(qwf.Count);
            Console.WriteLine(pdQgList.Count);


            for (int i = 0; i < this.qwf.Count; i++)
            {
                dr = analyDt.NewRow();
                dr[0] = this.qwf[i];
                dr[1] = inputPwfList[i];
                dr[2] = Utils.FormatString(pdQgList[i].Trim('[', ']', ' '), 0);
                analyDt.Rows.Add(dr);
            }

            //第三部分数据
            dr = analyDt.NewRow();
            dr[0] = "Qg";
            dr[1] = "Pwf";
            analyDt.Rows.Add(dr);

            for (int i = 0; i < this.qgList.Count; i++)
            {
                dr = analyDt.NewRow();
                dr[0] = Utils.FormatString(this.qgList[i].Trim('[', ']', ' '), 0);
                dr[1] = Utils.FormatString(outputPwfList[i].Trim('[', ']', ' '), 0);
                analyDt.Rows.Add(dr);
            }

            sPanel.Children.Add(UI_Components.addGridView("计算结果", analyDt));
            //按钮区 TODO:添加点击事件 1、导出数据 2、修改
            sPanel.Children.Add(addAnalysisButton());
            //添加到tabPage的内容中

            ScrollViewer myScrollViewer = new ScrollViewer();
            myScrollViewer.Content = sPanel;
            item.Content = myScrollViewer;
            return item;
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

            TabControl tabcontrol = new TabControl()
            {
                TabStripPlacement = Dock.Bottom,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                
            };
            if(fittingModel != null)
            {
                tabcontrol.Items.Add(addAnalysisItem());
            }
            if(predictModel != null)
            {
                tabcontrol.Items.Add(addProductionItem());
            }
            item.Content = tabcontrol;
            return item;
        }
    }
}

