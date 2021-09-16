
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
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TabControlWithClose;

namespace GasWaterWell.views
{
    class UI_Components
    {

        public static Rectangle addRectangle( Thickness margin,int height=2)
        {
            Rectangle rect = new Rectangle()
            {
                Height = height,
                Margin = margin,
                Fill = Brushes.DarkGray,
            };
            return rect;
        }
        public static TextBlock addTextBlock(string name,  Thickness margin,int size=12)
        {
            TextBlock text = new TextBlock()
            {
                Text = name,
                FontSize = size,
                Foreground = Brushes.Black,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                TextWrapping = TextWrapping.Wrap,
                Margin = margin
            };
            return text;
        }
        public static TextBox addTextBox(String value, Thickness margin, int size = 12, int w = 88, int h = 23, bool isReadOnly = true)
        {
            TextBox textBox = new TextBox()
            {
                Text = value,
                FontSize = size,
                Foreground = Brushes.Black,
                Width = w,
                Height = h,
                IsReadOnly = isReadOnly,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                TextWrapping = TextWrapping.Wrap,
                Margin = margin,
            };
            return textBox;
        }
        public static Grid addGridView<T>(string label, ObservableCollection<T> table)
        {
            Grid grid = new Grid();
            DataGrid dataGrid = new DataGrid()
            {
                Margin = new Thickness(5, 30, 5, 5),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                IsReadOnly = true,
                Height = 500,
                HorizontalScrollBarVisibility =ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility=ScrollBarVisibility.Auto,
            };
            dataGrid.ItemsSource = table;
            grid.Children.Add(addTextBlock(label, new Thickness(5, 5, 0, 0), 14));
            grid.Children.Add(addRectangle( new Thickness(67, 20, 5, 500)));
            grid.Children.Add(dataGrid);
            return grid;
        }

        public static Grid addGridView<T>(string label, ObservableCollection<T> table, List<string> deleteList)
        {
            Grid grid = new Grid();
            DataGrid dataGrid = new DataGrid()
            {
                Margin = new Thickness(5, 30, 5, 5),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                IsReadOnly = true,
                Height = 500,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,

            };
            dataGrid.ItemsSource = table;
            //在这里删
            //dataGrid.Columns.RemoveAt(0);
            grid.Children.Add(addTextBlock(label, new Thickness(5, 5, 0, 0), 14));
            grid.Children.Add(addRectangle(new Thickness(67, 20, 5, 500)));
            grid.Children.Add(dataGrid);
            return grid;
        }

        public static Grid addGridView(string label, DataTable table)
        {
            Grid grid = new Grid();
            DataGrid dataGrid = new DataGrid()
            {
                Margin = new Thickness(5, 30, 5, 5),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                IsReadOnly = true,
                Height = 500,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            };
            void Copy(object sender, RoutedEventArgs e)
            {
                int count = dataGrid.SelectedItems.Count;
                string res = "";

                foreach (DataRowView rowView in dataGrid.SelectedItems)
                {
                    string line = "";
                    int colCount = 0;
                    while (rowView[colCount].ToString() != "")
                    {
                        if(colCount != 0)
                        {
                            line += ",";
                        }
                        line += rowView[colCount].ToString().Trim();
                        colCount++;
                    }
                    res += line + "\n";
                }
                Clipboard.SetDataObject(res);
            }
            void Export(object sender, RoutedEventArgs e)
            {
                DataRowView rowSelected = dataGrid.SelectedItem as DataRowView;
                DataTable newTable = rowSelected.DataView.Table.Clone();
                foreach (DataRowView rowView in dataGrid.SelectedItems)
                {
                    newTable.Rows.Add(rowView.Row.ItemArray);
                }
                Utils.ExportExcel(newTable);
            }
            // 右键菜单
            ContextMenu aMenu = new ContextMenu();
            // 开始添加右键项目
            // 复制
            MenuItem copyMenu = new MenuItem();
            copyMenu.Header = "复制";
            copyMenu.Click += Copy;
            // 导出
            MenuItem exportMenu = new MenuItem();
            exportMenu.Header = "另存为";
            exportMenu.Click += Export;
            // 结束添加右键项目
            aMenu.Items.Add(copyMenu);
            aMenu.Items.Add(exportMenu);
            dataGrid.ContextMenu = aMenu;
            dataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
            dataGrid.ItemsSource = table.DefaultView;
            grid.Children.Add(addTextBlock(label, new Thickness(5, 5, 0, 0), 14));
            grid.Children.Add(addRectangle(new Thickness(67, 20, 5, 500)));
            grid.Children.Add(dataGrid);
            return grid;
        }

        /// <summary>
        /// 解决特殊字符导致列名出问题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string columnName = e.PropertyName;
            if (e.Column is DataGridBoundColumn &&
              (columnName.Contains(".") ||
              columnName.Contains("\\") ||
              columnName.Contains("/") ||
              columnName.Contains("[") ||
              columnName.Contains("]") ||
              columnName.Contains("(") ||
              columnName.Contains(")") ||
              columnName.Contains("·") ||
              columnName.Contains(" ")))
            {
                DataGridBoundColumn dataGridBoundColumn = e.Column as DataGridBoundColumn;
                dataGridBoundColumn.Binding = new Binding("[" + e.PropertyName + "]");

            }
        }


        public static ComboBox addComboBox()
        {
            ComboBoxItem item1 = new ComboBoxItem()
            {
                IsSelected = true,
                Content = "导出到Excel"
            };
            ComboBoxItem item2 = new ComboBoxItem()
            {
                Content = "导出到CSV"
            };
            ComboBox comBox = new ComboBox()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 120,
                Height = 22,
                Margin = new Thickness(75, 5, 0, 0)
            };
            comBox.Items.Add(item1);
            comBox.Items.Add(item2);
            return comBox;
        }


    }
}