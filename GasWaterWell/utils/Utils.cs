using ClosedXML.Excel;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace GasWaterWell.utils
{
    class Utils
    {
        /// <summary>
        /// timestamp to datetime
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            DateTime dtDateTime = startTime.AddSeconds(unixTimeStamp);
            return dtDateTime;
        }

        /// <summary>
        /// 返回当前时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetUnixTimeStamp()
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return unixTimestamp.ToString();
        }

        /// <summary>
        /// 保留四位小数，1为科学技术法
        /// </summary>
        /// <param name="s"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static string FormatString(string s, int flag)
        {
            if (s.ToLower().Contains("nan"))
                return "0.0000";
            if (flag == 0)
                return Convert.ToDouble(s).ToString("0.0000");
            else
                return Convert.ToDouble(s).ToString("0.####E+0");
        }

        private static void Copy(DataGrid dgv)
        {
            dgv.SelectAllCells();
            dgv.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, dgv);
        }


        public static void ExprotCsv(DataGrid dgv)
        {
            string path = "";
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "保存"; // Default file name
            dlg.DefaultExt = "csv";
            dlg.Filter = "CSV|*.csv"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                path = dlg.FileName;
                Copy(dgv);
                string data = Clipboard.GetText(TextDataFormat.CommaSeparatedValue);
                File.WriteAllText(path, data);
            }
        }

        public static bool ExportExcel(List<DataGrid> dgvs,string[] names)
        {
            Excel.Application excel = new Excel.Application();
            string excelVersion = excel.Version;//获取你使用的excel 的版本号 

            //声明保存对话框 
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //默然文件后缀 
            saveFileDialog.DefaultExt = "xlsx";

            if (Convert.ToDouble(excelVersion) < 12)//You use Excel 97-2003
            {
                //文件后缀列表 
                saveFileDialog.Filter = "Excel(*.xls)|*.xls";
            }
            else//you use excel 2007 or later
            {
                //文件后缀列表 
                saveFileDialog.Filter = "Excel(*.xls)|*.xls|Excel(2007-2016)(*.xlsx)|*.xlsx";
            }
            //默然路径是系统当前路径 
            saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            //打开保存对话框 
            if (saveFileDialog.ShowDialog() == false)
                return false;
            //返回文件路径 
            string path = saveFileDialog.FileName;
            if (string.IsNullOrEmpty(path.Trim()))
            { return false; }

            Excel.Application app = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;
            var process = Process.GetProcessesByName("EXCEL");

            Array.Reverse(names);
            try
            {
                app = new Excel.Application();
                app.DisplayAlerts = false;

                Excel.Workbooks workbooks = app.Workbooks;
                Excel.Workbook workbook = workbooks.Add();
                

                //wb = app.Workbooks.Add();
                //ws = wb.ActiveSheet;

                //遍历sheet
                for (int i = 0; i < dgvs.Count; i++)
                {
                    Excel.Worksheet worksheet = workbook.Sheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    worksheet.Name = names[i];
                    Copy(dgvs[i]);
                    worksheet.Paste();
                    //workbook.Sheets.Add(worksheet);
                }

                //Copy(dgv);
                //ws.Paste();
                workbook.SaveAs(path);
            }
            finally
            {
                wb?.Close(false);
                app?.Quit();
                Process.GetProcessesByName("EXCEL").Except(process).FirstOrDefault()?.Kill();
            }
            return true;
        }


        public static bool ExportExcel(DataGrid dgv)
        {
            Excel.Application excel = new Excel.Application();
            string excelVersion = excel.Version;//获取你使用的excel 的版本号 

            //声明保存对话框 
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //默然文件后缀 
            saveFileDialog.DefaultExt = "xlsx";

            if (Convert.ToDouble(excelVersion) < 12)//You use Excel 97-2003
            {
                //文件后缀列表 
                saveFileDialog.Filter = "Excel(*.xls)|*.xls";
            }
            else//you use excel 2007 or later
            {
                //文件后缀列表 
                saveFileDialog.Filter = "Excel(*.xls)|*.xls|Excel(2007-2016)(*.xlsx)|*.xlsx";
            }
            //默然路径是系统当前路径 
            saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            //打开保存对话框 
            if (saveFileDialog.ShowDialog() == false)
                return false;
            //返回文件路径 
            string path = saveFileDialog.FileName;
            if (string.IsNullOrEmpty(path.Trim()))
            { return false; }

            Excel.Application app = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;
            var process = Process.GetProcessesByName("EXCEL");
            try
            {
                app = new Excel.Application();
                app.DisplayAlerts = false;
                wb = app.Workbooks.Add();
                ws = wb.ActiveSheet;
                Copy(dgv);
                ws.Paste();
                wb.SaveAs(path);
            }
            finally
            {
                wb?.Close(false);
                app?.Quit();
                Process.GetProcessesByName("EXCEL").Except(process).FirstOrDefault()?.Kill();
            }
            return true;
        }

        public static bool ExportExcel(DataTable dt)
        {
            Excel.Application excel = new Excel.Application();

            //声明保存对话框 
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //默然文件后缀 
            saveFileDialog.DefaultExt = "xlsx";

            saveFileDialog.Filter = "Excel(2007-2016)(*.xlsx)|*.xlsx";
            //默然路径是系统当前路径 
            saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            //打开保存对话框 
            if (saveFileDialog.ShowDialog() == false)
                return false;
            //返回文件路径 
            string path = saveFileDialog.FileName;
            if (string.IsNullOrEmpty(path.Trim()))
            { return false; }

            try
            {
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(dt, "Sheet1");
                wb.SaveAs(path);
            }
            catch(Exception ex)
            {
                MessageBox.Show("错误\n"+ex, "哎呀，出错了...");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 多个datatable导出成多个sheet表
        /// </summary>
        /// <param name="dts"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        public static bool ExportExcel(List<DataTable> dts, string[] names)
        {
            Excel.Application excel = new Excel.Application();
            //声明保存对话框 
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //默然文件后缀 
            saveFileDialog.DefaultExt = "xlsx";

            saveFileDialog.Filter = "Excel(2007-2016)(*.xlsx)|*.xlsx";
            //默然路径是系统当前路径 
            saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            //打开保存对话框 
            if (saveFileDialog.ShowDialog() == false)
                return false;
            //返回文件路径 
            string path = saveFileDialog.FileName;
            if (string.IsNullOrEmpty(path.Trim()))
            { return false; }

            XLWorkbook wb = new XLWorkbook();
            for (int i=0;i< dts.Count; i++)
            {
                try
                {
                    wb.Worksheets.Add(dts[i], names[i]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误\n" + ex, "哎呀，出错了...");
                    return false;
                }
            }
            wb.SaveAs(path);
            return true;
        }




        public static DataTable ImportExcel(DataGrid dgv)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.DefaultExt = ".xlsx";
            openfile.Filter = "(.xlsx)|*.xlsx|(.xls)|*.xls";
            //openfile.ShowDialog();

            DataTable dt = null;
            var browsefile = openfile.ShowDialog();

            if (browsefile == true)
            {
                string path = openfile.FileName;

                Excel.Application excelApp = new Excel.Application();
                //Static File From Base Path...........
                //Excel.Workbook excelBook = excelApp.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + "TestExcel.xlsx", 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                //Dynamic File Using Uploader...........
                Excel.Workbook excelBook = excelApp.Workbooks.Open(path, 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                Excel.Worksheet excelSheet = (Excel.Worksheet)excelBook.Worksheets.get_Item(1); ;
                Excel.Range excelRange = excelSheet.UsedRange;

                string strCellData = "";
                double douCellData;
                int rowCnt = 0;
                int colCnt = 0;

                dt = new DataTable();
                for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                {
                    string strColumn = "";
                    strColumn = (string)(excelRange.Cells[1, colCnt] as Excel.Range).Value2;
                    dt.Columns.Add(strColumn, typeof(string));
                }

                for (rowCnt = 2; rowCnt <= excelRange.Rows.Count; rowCnt++)
                {
                    string strData = "";
                    for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                    {
                        try
                        {
                            strCellData = (string)(excelRange.Cells[rowCnt, colCnt] as Excel.Range).Value2;
                            if (string.IsNullOrWhiteSpace(strCellData))
                            {
                                strCellData = null;
                            }
                            strData += strCellData + "|";
                        }
                        catch (Exception ex)
                        {
                            douCellData = (excelRange.Cells[rowCnt, colCnt] as Excel.Range).Value2;
                            strData += douCellData.ToString() + "|";
                        }
                    }
                    strData = strData.Remove(strData.Length - 1, 1);
                    dt.Rows.Add(strData.Split('|'));
                }

                dgv.ItemsSource = dt.DefaultView;

                excelBook.Close(true, null, null);
                excelApp.Quit();
            }
            return dt;
        }

        public static DataTable ImportExcel()
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.DefaultExt = ".xlsx";
            openfile.Filter = "(.xlsx)|*.xlsx|(.xls)|*.xls";
            //openfile.ShowDialog();

            DataTable dt = null;
            var browsefile = openfile.ShowDialog();

            if (browsefile == true)
            {
                string path = openfile.FileName;

                Excel.Application excelApp = new Excel.Application();
                //Static File From Base Path...........
                //Excel.Workbook excelBook = excelApp.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + "TestExcel.xlsx", 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                //Dynamic File Using Uploader...........
                Excel.Workbook excelBook = excelApp.Workbooks.Open(path, 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                Excel.Worksheet excelSheet = (Excel.Worksheet)excelBook.Worksheets.get_Item(1); ;
                Excel.Range excelRange = excelSheet.UsedRange;

                string strCellData = "";
                double douCellData;
                int rowCnt = 0;
                int colCnt = 0;

                dt = new DataTable();
                for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                {
                    string strColumn = "";
                    strColumn = (string)(excelRange.Cells[1, colCnt] as Excel.Range).Value2;
                    dt.Columns.Add(strColumn, typeof(string));
                }

                for (rowCnt = 2; rowCnt <= excelRange.Rows.Count; rowCnt++)
                {
                    string strData = "";
                    for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                    {
                        try
                        {
                            strCellData = (string)(excelRange.Cells[rowCnt, colCnt] as Excel.Range).Value2;
                            if (string.IsNullOrWhiteSpace(strCellData))
                            {
                                strCellData = null;
                            }
                            strData += strCellData + "|";
                        }
                        catch (Exception ex)
                        {
                            douCellData = (excelRange.Cells[rowCnt, colCnt] as Excel.Range).Value2;
                            strData += douCellData.ToString() + "|";
                        }
                    }
                    strData = strData.Remove(strData.Length - 1, 1);
                    dt.Rows.Add(strData.Split('|'));
                }

                excelBook.Close(true, null, null);
                excelApp.Quit();
            }
            return dt;
        }

        public static DataTable ImportCSV()//从csv读取数据返回table
        {
            string filePath = "";
            DataTable dt = new DataTable();
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "打开文件";
            fdlg.DefaultExt = "csv";
            fdlg.Filter = "CSV|*.csv";
            fdlg.InitialDirectory = Directory.GetCurrentDirectory();

            /*
             * FilterIndex 属性用于选择了何种文件类型,缺省设置为0,系统取Filter属性设置第一项
             * ,相当于FilterIndex 属性设置为1.如果你编了3个文件类型，当FilterIndex ＝2时是指第2个.
             */
            fdlg.FilterIndex = 2;
            /*
             *如果值为false，那么下一次选择文件的初始目录是上一次你选择的那个目录，
             *不固定；如果值为true，每次打开这个对话框初始目录不随你的选择而改变，是固定的  
             */
            fdlg.RestoreDirectory = true;
            var result = fdlg.ShowDialog();

            if (result == true)
            {
                filePath = fdlg.FileName;
            }
            else
            {
                return null;
            }
            Console.WriteLine(filePath);


            System.Text.Encoding encoding = GetTypeByString(filePath); //Encoding.ASCII;//

            System.IO.FileStream fs = null;

            try
            {
                fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开文件出错" + ex, "错误");
                return dt;
            }

            System.IO.StreamReader sr = new System.IO.StreamReader(fs, encoding);

            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (aryLine != null && aryLine.Length > 0)
            {
                dt.DefaultView.Sort = tableHead[0] + " " + "asc";
            }

            sr.Close();
            fs.Close();
            return dt;
        }

        public static System.Text.Encoding GetTypeByString(string FILE_NAME)
        {
            try
            {
                System.IO.FileStream fs = new System.IO.FileStream(FILE_NAME, System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
                System.Text.Encoding r = GetTypeByFile(fs);
                fs.Close();
                return r;
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开文件出错" + ex, "错误");
                System.Text.Encoding r = null;
                return r;
            }
        }


        public static System.Text.Encoding GetTypeByFile(System.IO.FileStream fs)
        {
            byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM
            System.Text.Encoding reVal = System.Text.Encoding.Default;

            System.IO.BinaryReader r = new System.IO.BinaryReader(fs, System.Text.Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                reVal = System.Text.Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = System.Text.Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = System.Text.Encoding.Unicode;
            }
            r.Close();
            return reVal;
        }

        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1;  //计算当前正分析的字符应还有的字节数
            byte curByte; //当前分析的字节.
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X　
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }

        public static IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }


        public static bool IsTextBoxEmpty(params TextBox[] txt)
        {
            bool flag = false;
            for (int i = 0; i < txt.Length; i++)
            {
                if (txt[i].Text.Trim() == "")
                {
                    MessageBox.Show("输入数据不能为空，请重新输入！");
                    txt[i].Focus();
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        public static bool IsDigit(params TextBox[] txt)
        {
            for(int i = 0; i < txt.Length; i++)
            {
                try
                {
                    Convert.ToDouble(txt[i].Text);
                }
                catch
                {
                    MessageBox.Show("请输入正确数字！");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 根据datatable获得列名
        /// </summary>
        /// <param name="dt">表对象</param>
        /// <returns>返回结果的数据列数组</returns>
        public static string[] GetColumnsByDataTable(DataTable dt)
        {
            string[] strColumns = null;


            if (dt.Columns.Count > 0)
            {
                int columnNum = 0;
                columnNum = dt.Columns.Count;
                strColumns = new string[columnNum];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    strColumns[i] = dt.Columns[i].ColumnName;
                }
            }


            return strColumns;
        }

        /// <summary>
        /// 将DataTable的列名按照Dict的映射进行重命名
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ColumnsDict"></param>
        /// <returns></returns>
        public static DataTable ChangeColumnsToRight(DataTable dt, IDictionary<string, string> ColumnsDict)
        {
            string[] ColumnsArray = GetColumnsByDataTable(dt);
            int[] IsUsed = new int[ColumnsArray.Length];

            for (int i = 0; i < ColumnsArray.Length; i++)
            {
                int k = 0;
                foreach (var item in ColumnsDict)
                {
                    Console.WriteLine(ColumnsArray[i]);
                    if (ColumnsArray[i].Contains(item.Key) && IsUsed[k] != 1)
                    {
                        IsUsed[k] = 1;
                        dt.Columns[ColumnsArray[i]].ColumnName = item.Value;
                        break;
                    }
                    k++;
                }
            }
            return dt;
        }

        /// <summary>
        /// 读取Excel单元格中的日期
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ConvertExcelDateTimeIntoCLRDateTime(object value)
        {
            if (value is DateTime)
            {
                return DateTime.Parse(value.ToString());
            }
            else
            {
                string dt = DateTime.FromOADate(Convert.ToInt32(value)).ToString("d");
                return DateTime.Parse(dt);
            }
        }

        public static DataTable ReplaceDataInDataTable(DataTable dt, Char ch1, Char ch2)
        {
            dt = dt.AsEnumerable().Select(a =>
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    a[dc] = a[dc].ToString().Replace(ch1, ch2);//替换
                }
                return a;
            }).CopyToDataTable<DataRow>();
            return dt;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }

        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>  
        /// Excel数据导入Datable  
        /// </summary>  
        /// <param name="fileUrl"></param>  
        /// <param name="table"></param>  
        /// <returns></returns>  
        public static DataTable GetExcelDatatable()
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.DefaultExt = ".xlsx";
            openfile.Filter = "(.xlsx)|*.xlsx|(.xls)|*.xls";
            //openfile.ShowDialog();

            DataTable dt = null;
            var browsefile = openfile.ShowDialog();

            if (browsefile == true)
            {
                string path = openfile.FileName;

                Excel.Application excelApp = new Excel.Application();
                //Static File From Base Path...........
                //Excel.Workbook excelBook = excelApp.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + "TestExcel.xlsx", 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                //Dynamic File Using Uploader...........
                Excel.Workbook excelBook = excelApp.Workbooks.Open(path, 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                Excel.Worksheet excelSheet = (Excel.Worksheet)excelBook.Worksheets.get_Item(1); ;
                Excel.Range excelRange = excelSheet.UsedRange;

                string strCellData = "";
                double douCellData;
                int rowCnt = 0;
                int colCnt = 0;

                dt = new DataTable();
                for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                {
                    string strColumn = "";
                    strColumn = (string)(excelRange.Cells[1, colCnt] as Excel.Range).Value2;
                    dt.Columns.Add(strColumn, typeof(string));
                }

                for (rowCnt = 2; rowCnt <= excelRange.Rows.Count; rowCnt++)
                {
                    string strData = "";
                    for (colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                    {
                        try
                        {
                            strCellData = (string)(excelRange.Cells[rowCnt, colCnt] as Excel.Range).Value2;
                            if (string.IsNullOrWhiteSpace(strCellData))
                            {
                                strCellData = null;
                            }
                            strData += strCellData + "|";
                        }
                        catch (Exception ex)
                        {
                            douCellData = (excelRange.Cells[rowCnt, colCnt] as Excel.Range).Value2;
                            strData += douCellData.ToString() + "|";
                        }
                    }
                    strData = strData.Remove(strData.Length - 1, 1);
                    dt.Rows.Add(strData.Split('|'));
                }

                excelBook.Close(true, null, null);
                excelApp.Quit();
            }
            return dt;
        }

    }
}


