using GasWaterWell.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GasWaterWell.utils
{
    class OperateDB
    {
        /// <summary>
        /// 删除井通过wellId
        /// </summary>
        /// <param name="wellId"></param>
        /// <returns></returns>
        public static int DeleteWellById(string wellId)
        {
            DbManager.Ins.ConnStr = Const.DBSTR;

            //删除气水两相的结果数据
            string sql = @"delete from gaswater_result where exists(select product_id from gaswater_production_data_new where well_id=@wellId and product_id=gaswater_result.gaswater_input_id)";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            Paramter.Add(new MySqlParameter("wellId", wellId));
            int result1 = DbManager.Ins.ExecuteNonquery(sql, Paramter.ToArray());

            //删除气水两相的生产数据
            sql = @"delete from gaswater_production_data_new where well_id=@wellId";
            int result2 = DbManager.Ins.ExecuteNonquery(sql, Paramter.ToArray());

            //删除气相方法-生产数据
            sql = @"delete from gas_production_data where well_id = @wellId";
            int result3 = DbManager.Ins.ExecuteNonquery(sql, Paramter.ToArray());

            //删除气相方法-结果数据
            sql = @"delete from gas_result_data where well_id = @wellId";
            int result4 = DbManager.Ins.ExecuteNonquery(sql, Paramter.ToArray());

            //删除产能试井分析数据
            sql = @"delete from fitting where well_id = @wellId";
            int result5 = DbManager.Ins.ExecuteNonquery(sql, Paramter.ToArray());

            //删除产能试井预测数据
            sql = @"delete from prediction where well_id = @wellId";
            int result6 = DbManager.Ins.ExecuteNonquery(sql, Paramter.ToArray());

            //删除well的基础参数
            sql = @"delete from well_params where well_id = @wellId";
            int result7 = DbManager.Ins.ExecuteNonquery(sql, Paramter.ToArray());

            //删除well
            sql = @"delete from well where well_id = @wellId";
            int result8 = DbManager.Ins.ExecuteNonquery(sql, Paramter.ToArray());

            int result = result1 + result2 + result3 + result4 + result5 + result6 + result7+result8;
            return result;
        }

        public static int DeleteGWPSItem(string phaseName)
        {
            if (SelectWellByPhaseName(phaseName).Rows.Count > 0)
            {
                return 0;
            }
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"delete from phase_seepage where phase_seepage_name=@pvtName";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            Paramter.Add(new MySqlParameter("pvtName", phaseName));
            int result = DbManager.Ins.ExecuteNonquery(sql, Paramter.ToArray());
            return result;
        }

        /// <summary>
        /// 删除RockPVT
        /// </summary>
        /// <param name="pvtName"></param>
        /// <returns></returns>
        public static int DeleteRockPVT(string pvtName)
        {
            if (SelectWellByPVT_Name(pvtName).Rows.Count > 0)
            {
                return 0;
            }
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"delete from pvt_rock where rock_name=@pvtName";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            Paramter.Add(new MySqlParameter("pvtName", pvtName));
            int result = DbManager.Ins.ExecuteNonquery(sql, Paramter.ToArray());
            return result;
        }

        /// <summary>
        /// 删除WaterPVT
        /// </summary>
        /// <param name="pvtName"></param>
        /// <returns></returns>
        public static int DeleteWaterPVT(string pvtName)
        {
            if (SelectWellByPVT_Name(pvtName).Rows.Count > 0)
            {
                return 0;
            }
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"delete from pvt_water where water_name=@pvtName";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            Paramter.Add(new MySqlParameter("pvtName", pvtName));
            int result = DbManager.Ins.ExecuteNonquery(sql, Paramter.ToArray());
            return result;
        }

        /// <summary>
        /// 删除GasPVT
        /// </summary>
        /// <param name="pvtName"></param>
        /// <returns></returns>
        public static int DeleteGasPVT(string pvtName)
        {
            if (SelectWellByPVT_Name(pvtName).Rows.Count > 0)
            {
                return 0;
            }
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"delete from pvt_gas where gas_name=@pvtName";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            Paramter.Add(new MySqlParameter("pvtName", pvtName));
            int result = DbManager.Ins.ExecuteNonquery(sql, Paramter.ToArray());
            return result;
        }

        /// <summary>
        /// 删除产能试井子项
        /// </summary>
        /// <param name="fitId"></param>
        /// <param name="preId"></param>
        /// <returns></returns>
        public static int DeleteTestWellItem(string fitId, string preId)
        {
            DbManager.Ins.ConnStr = Const.DBSTR;
            //删除产能分析
            string sql = @"delete from fitting where fitting_id=@fitId";
            List<MySqlParameter> Paramter1 = new List<MySqlParameter>();
            Paramter1.Add(new MySqlParameter("fitId", fitId));
            int result1 = DbManager.Ins.ExecuteNonquery(sql, Paramter1.ToArray());
            //删除产能预测
            sql = @"delete from prediction where pred_id=@preId";
            List<MySqlParameter> Paramter2 = new List<MySqlParameter>();
            Paramter2.Add(new MySqlParameter("preId", preId));
            int result2 = DbManager.Ins.ExecuteNonquery(sql, Paramter2.ToArray());
            return result2+result1;
        }

        /// <summary>
        /// 删除拟压力平方法/-气相
        /// 删除压力平方法/-气相
        /// </summary>
        /// <param name="proId"></param>
        /// <param name="resultId"></param>
        /// <returns></returns>
        public static int DeletePseudoPressureItem(string proId,string resultId)
        {
            DbManager.Ins.ConnStr = Const.DBSTR;
            //删除拟压力平方法的生产数据
            string sql = @"delete from gas_result_data where gas_result_index=@resultId";
            List<MySqlParameter> Paramter2 = new List<MySqlParameter>();
            Paramter2.Add(new MySqlParameter("resultId", resultId));
            int result2 = DbManager.Ins.ExecuteNonquery(sql, Paramter2.ToArray());

            //删除拟压力平方法的劫夺数据
            sql = @"delete from gas_production_data where gas_product_index=@proId";
            List<MySqlParameter> Paramter1 = new List<MySqlParameter>();
            Paramter1.Add(new MySqlParameter("proId", proId));
            int result1 = DbManager.Ins.ExecuteNonquery(sql, Paramter1.ToArray());
            
            return result2 + result1;
        }

        /// <summary>
        /// 删除 单点二项式-气水两相方法-子项
        /// </summary>
        /// <param name="proId"></param>
        /// <param name="resultId"></param>
        /// <returns></returns>
        public static int DeleteGasWaterItem(string proId,string resultId)
        {
            DbManager.Ins.ConnStr = Const.DBSTR;
            //删除拟压力平方法的生产数据
            string sql = @"delete from gaswater_result where gaswater_result_index=@resultId";
            List<MySqlParameter> Paramter2 = new List<MySqlParameter>();
            Paramter2.Add(new MySqlParameter("resultId", resultId));
            int result2 = DbManager.Ins.ExecuteNonquery(sql, Paramter2.ToArray());

            //删除拟压力平方法的劫夺数据
            sql = @"delete from gaswater_production_data_new where product_index=@proId";
            List<MySqlParameter> Paramter1 = new List<MySqlParameter>();
            Paramter1.Add(new MySqlParameter("proId", proId));
            int result1 = DbManager.Ins.ExecuteNonquery(sql, Paramter1.ToArray());

            return result2 + result1;
        }

        /// <summary>
        /// 查询应用了pvt的well集合
        /// </summary>
        /// <param name="pvtName"></param>
        /// <returns></returns>
        public static DataTable SelectWellByPVT_Name(string pvtName)
        {
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select * from well where pvt_gas_name=@pvtName or pvt_water_name=@pvtName  or pvt_rock_name=@pvtName;";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            Paramter.Add(new MySqlParameter("pvtName", pvtName));
            DataTable result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            return result;
        }

        public static DataTable SelectWellByPhaseName(string phaseName)
        {
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select * from well where phase_seepage_name=@phaseName";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            Paramter.Add(new MySqlParameter("phaseName", phaseName));
            DataTable result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            return result;
        }
        /// <summary>
        /// 插入project
        /// </summary>
        /// <param name="proId"></param>
        /// <param name="proName"></param>
        /// <returns></returns>
        public static int InsertProject(string proId,string proName)
        {
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"insert into project values(@proId,@proName)";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            Paramter.Add(new MySqlParameter("proId", proId));
            Paramter.Add(new MySqlParameter("proName", proName));
            int result = DbManager.Ins.ExecuteNonquery(sql, Paramter.ToArray());
            return result;   
        }

        /// <summary>
        /// 返回项目数据
        /// </summary>
        /// <param name="proId"></param>
        /// <returns></returns>
        /// <returns></returns>
        public static DataTable getDataById(string proId)
        {
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select * from xm_project where xmbm=@xmbm;";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            Paramter.Add(new MySqlParameter("xmbm", proId));
            DataTable result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            return result;
        }

        /// <summary>
        /// 判断是否是油气田
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static bool isFieldById(string proId)
        {
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select xmlbbm from xm_project where xmbm=@xmbm";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            Paramter.Add(new MySqlParameter("@xmbm", proId));
            DataTable data = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            if (data.Rows[0]["xmlbbm"].ToString() == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否存在父节点
        /// </summary>
        /// <param name="proId"></param>
        /// <returns></returns>
        public static DataTable getParentProById(string proId)
        {
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select * from  xm_project where xmbm in (select sjxmbm from xm_project where xmbm=@xmbm);";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            Paramter.Add(new MySqlParameter("xmbm", proId));
            DataTable result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            return result;
        }
        /// <summary>
        /// 获取子项目
        /// </summary>
        /// <param name="proId"></param>
        /// <returns></returns>
        public static DataTable getChildrenProById(string proId)
        {
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select * from xm_project where sjxmbm=@sjxmbm;";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            Paramter.Add(new MySqlParameter("@sjxmbm", proId));
            DataTable result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            return result;
        }
        /// <summary>
        /// 导入气水相渗
        /// </summary>
        public bool InsertToPhaseSeepage(string phase_seepage_index, string project_id, string phaseSeepageName, DataTable dtGrid)
        {
            Console.WriteLine(Const.DBSTR);
            MySqlConnection conn = new MySqlConnection(Const.DBSTR);

            try
            {
                DataTable dt = dtGrid.Copy();

                //更改列名，对应数据库中的列名
                IDictionary<string, string> ColumnsDict = new Dictionary<string, string>()
                {
                    {"Sw", "sw"},
                    {"Krg", "krg"},
                    {"Krw", "krw"},
                };
                Utils.ChangeColumnsToRight(dt, ColumnsDict);

                dt.Columns.Add("phase_seepage_name", typeof(string));
                foreach (DataRow dr in dt.Rows)
                    dr["phase_seepage_name"] = phaseSeepageName;

                string sql = string.Format("SELECT * FROM phase_seepage " +
                             "where phase_seepage_index = {0} and project_id = {1}",
                             phase_seepage_index, project_id);

                //给Datatable插入key列
                DataColumn dc = new DataColumn("phase_seepage_index", typeof(string));
                dc.DefaultValue = phase_seepage_index;
                dt.Columns.Add(dc);
                dc = new DataColumn("project_id", typeof(string));
                dc.DefaultValue = project_id;
                dt.Columns.Add(dc);

                //执行批量插入更新
                MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                _ = adapter.Update(dt);
                adapter.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入气水相渗错误\n" + ex, "错误");
                return false;
            }
        }
        public DataTable ImportGasWaterPhaseSeepage() {
            DataTable dtGrid = Utils.ImportExcel();
            //dtGrid = (gasWaterDataGrid.ItemsSource as DataView).ToTable();
            return dtGrid;
        }


        public static string GetRcokGamaByWellId(string wellId)
        {
            string sql = string.Format("SELECT pvt_rock_name FROM well " +
                             "where well_id = {0}", wellId);
            DataTable result = DbManager.Ins.ExcuteDataTable(sql);
            if(result.Rows.Count > 0)
            {
                string pvtRockName = result.Rows[0]["pvt_rock_name"].ToString();
                sql = string.Format("SELECT gama FROM pvt_rock " +
                             "where rock_name = '{0}'", pvtRockName);
                result = DbManager.Ins.ExcuteDataTable(sql);
                if (result.Rows.Count > 0)
                {
                    return result.Rows[0]["gama"].ToString();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

    }
}
