using GasWaterWell.utils;
using System;
using System.IO;
using System.Windows;


namespace GasWaterWell
{
    /// <summary>
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : Window
    {
        private readonly string path = "./conf/Config.ini";

        public Setting()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(path))
            {
                var MyIni = new IniFile(path);
                string ip = MyIni.Read("IP", "MySQL");
                string username = MyIni.Read("username", "MySQL");
                string password = MyIni.Read("password", "MySQL");
                string dbName = MyIni.Read("dbName", "MySQL");
                string pythonPath = MyIni.Read("pythonPath", "Python");
                IP_TextBox.Text = ip;
                User_TextBox.Text = username;
                Pwd_TextBox.Text = password;
                DB_TextBox.Text = dbName;
                Python_TextBox.Text = pythonPath;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string ip = IP_TextBox.Text;
            string username = User_TextBox.Text;
            string password = Pwd_TextBox.Text;
            string dbName = DB_TextBox.Text;
            string pythonPath = Python_TextBox.Text;
            var MyIni = new IniFile(path);
            MyIni.Write("IP", ip, "MySQL");
            MyIni.Write("username", username, "MySQL");
            MyIni.Write("password", password, "MySQL");
            MyIni.Write("dbName", dbName, "MySQL");
            MyIni.Write("pythonPath", pythonPath, "Python");
            Const.IP = ip;
            Const.USER = username;
            Const.PWD = password;
            Const.DBNAME = dbName;
            Const.DBSTR = String.Format(Const.DBSTR, ip, username, password, dbName);
            Const.PYTHONPATH = pythonPath;
            MessageBox.Show("保存成功", "成功");
            this.Close();
        }


        private void Test_Click(object sender, RoutedEventArgs e)
        {
            string dbStr = @"server={0};User Id={1};password={2};Database={3}";
            string ip = IP_TextBox.Text;
            string username = User_TextBox.Text;
            string password = Pwd_TextBox.Text;
            string dbName = DB_TextBox.Text;
            string pythonPath = Python_TextBox.Text;
            dbStr = String.Format(dbStr, ip, username, password, dbName);
            Console.WriteLine(dbStr);

            DbManager.Ins.ConnStr = dbStr;
            bool isConn = DbManager.Ins.ConnectDataBase();
            if (isConn && File.Exists(pythonPath))
            {
                MessageBox.Show("数据库与Python都连接成功", "成功");
            }
            else if (!isConn && File.Exists(pythonPath))
            {
                MessageBox.Show("数据库连接失败", "错误");
            }
            else if (isConn && !File.Exists(pythonPath))
            {
                MessageBox.Show("未找到Python.exe", "错误");
            }
            else
            {
                MessageBox.Show("数据库连接失败且未找到Python.exe", "错误");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
