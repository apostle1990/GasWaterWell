using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using System.Data;
using GasWaterWell.Model;
using System.Windows;

namespace GasWaterWell.utils
{
    class DbRead
    {
        public static WellPropertyModel GetWellProperty(string wellId)
        {
            WellPropertyModel wellPropertyModel = null;
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select * from well where well_id=@well_id;";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            Paramter.Add(new MySqlParameter("well_id", wellId));
            result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            if (result == null)
            {
                return wellPropertyModel;
            }
            wellPropertyModel = new WellPropertyModel(
                result.Rows[0]["well_id"].ToString(),
                result.Rows[0]["xmmc"].ToString(),
                result.Rows[0]["xmdm"].ToString(),
                result.Rows[0]["szd"].ToString(),
                result.Rows[0]["gk"].ToString(),
                result.Rows[0]["pvt_gas_name"].ToString(),
                result.Rows[0]["pvt_water_name"].ToString(),
                result.Rows[0]["pvt_rock_name"].ToString()
                );
            return wellPropertyModel;
        }

        public static WellParamsModel GetWellParams(string wellId)
        {
            WellParamsModel wellParamsModel = null;
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select * from well_params where well_id=@well_id;";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            Paramter.Add(new MySqlParameter("well_id", wellId));
            result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            if (result.Rows.Count==0)
            {
                return wellParamsModel;
            }
            try
            {
               wellParamsModel = new WellParamsModel(
               Convert.ToInt32(result.Rows[0]["well_id"]),
               Convert.ToDouble(result.Rows[0]["s"]),
               Convert.ToDouble(result.Rows[0]["d"]),
               Convert.ToDouble(result.Rows[0]["re"]),
               Convert.ToDouble(result.Rows[0]["rw"]),
               Convert.ToDouble(result.Rows[0]["rhogsc"]),
               Convert.ToDouble(result.Rows[0]["rhowsc"]),
               Convert.ToDouble(result.Rows[0]["pe"])
               );
            }
            catch (ArgumentException ex)
            {

                MessageBox.Show(ex.Message, "哎呀，崩溃了...");

            }

            return wellParamsModel;
            
        }
        public static FittingModel GetFitting(string fitting_id)
        {
            FittingModel fittingModel = null;
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select * from fitting where fitting_id=@fitting_id;";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            Paramter.Add(new MySqlParameter("fitting_id", fitting_id));
            result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            if (result.Rows.Count == 0)
            {
                return fittingModel;
            }

            fittingModel = new FittingModel(
                result.Rows[0]["fitting_id"].ToString(),
                result.Rows[0]["well_id"].ToString(),
                result.Rows[0]["pr"].ToString(),
                result.Rows[0]["a"].ToString(),
                result.Rows[0]["b"].ToString(),
                result.Rows[0]["qaof"].ToString(),
                result.Rows[0]["r2"].ToString(),
                result.Rows[0]["pic_path_1"].ToString(),
                result.Rows[0]["pic_path_2"].ToString(),
                result.Rows[0]["Q_wf"].ToString().Split(',').ToList(),
                result.Rows[0]["fitting_pwf_input"].ToString().Split(',').ToList(),
                result.Rows[0]["pd_qg"].ToString().Split(',').ToList(),
                result.Rows[0]["fitting_pwf_ouput"].ToString().Split(',').ToList(),
                result.Rows[0]["fitting_qg"].ToString().Split(',').ToList()
             );
            return fittingModel;
        }

        public static PredictModel GetPredictByFitId(string fitting_id)
        {
            PredictModel predictModel = null;
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select * from prediction where fitting_id=@fitting_id;";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            Paramter.Add(new MySqlParameter("fitting_id", fitting_id));
            result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            if(result.Rows.Count == 0)
            {
                return predictModel;
            }

            predictModel = new PredictModel(
                result.Rows[0]["pred_id"].ToString(),
                result.Rows[0]["well_id"].ToString(),
                result.Rows[0]["pred_a_input"].ToString(),
                result.Rows[0]["pred_b_input"].ToString(),
                result.Rows[0]["pred_pr_input"].ToString(),
                result.Rows[0]["pred_qg_input"].ToString(),
                result.Rows[0]["pic_path_1"].ToString(),
                result.Rows[0]["pic_path_2"].ToString(),
                result.Rows[0]["pi"].ToString().Split(',').ToList(),
                result.Rows[0]["pi_p"].ToString().Split(',').ToList(),
                result.Rows[0]["k_ki"].ToString().Split(',').ToList(),
                result.Rows[0]["u_pi"].ToString().Split(',').ToList(),
                result.Rows[0]["z_p"].ToString().Split(',').ToList(),
                result.Rows[0]["Ai"].ToString().Split(',').ToList(),
                result.Rows[0]["Bi"].ToString().Split(',').ToList(),
                result.Rows[0]["pred_qg_output"].ToString().Split(',').ToList(),
                result.Rows[0]["pred_pwf_output"].ToString().Split(',').ToList(),
                result.Rows[0]["fitting_id"].ToString()
             );
            return predictModel;
        }

        public static PredictModel GetPredictByPredId(string pred_id)
        {
            PredictModel predictModel = null;
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select * from prediction where pred_id=@pred_id;";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            Paramter.Add(new MySqlParameter("pred_id", pred_id));
            result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            if (result.Rows.Count == 0)
            {
                return predictModel;
            }

            predictModel = new PredictModel(
                result.Rows[0]["pred_id"].ToString(),
                result.Rows[0]["well_id"].ToString(),
                result.Rows[0]["pred_a_input"].ToString(),
                result.Rows[0]["pred_b_input"].ToString(),
                result.Rows[0]["pred_pr_input"].ToString(),
                result.Rows[0]["pred_qg_input"].ToString(),
                result.Rows[0]["pic_path_1"].ToString(),
                result.Rows[0]["pic_path_2"].ToString(),
                result.Rows[0]["pi"].ToString().Split(',').ToList(),
                result.Rows[0]["pi_p"].ToString().Split(',').ToList(),
                result.Rows[0]["k_ki"].ToString().Split(',').ToList(),
                result.Rows[0]["u_pi"].ToString().Split(',').ToList(),
                result.Rows[0]["z_p"].ToString().Split(',').ToList(),
                result.Rows[0]["Ai"].ToString().Split(',').ToList(),
                result.Rows[0]["Bi"].ToString().Split(',').ToList(),
                result.Rows[0]["pred_qg_output"].ToString().Split(',').ToList(),
                result.Rows[0]["pred_pwf_output"].ToString().Split(',').ToList(),
                result.Rows[0]["fitting_id"].ToString()
             );
            return predictModel;
        }

        public static GasWaterTwoModel GetGasWater(string gaswater_input_id)
        {
            GasWaterTwoModel gasWaterTwoModel = null;
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select * from gaswater_input where gaswater_input_id=@gaswater_input_id;";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            Paramter.Add(new MySqlParameter("gaswater_input_id", gaswater_input_id));
            result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            if (result.Rows.Count == 0)
            {
                return gasWaterTwoModel;
            }

            gasWaterTwoModel = new GasWaterTwoModel(
                double.Parse(result.Rows[0]["rhowsc"].ToString()),
                double.Parse(result.Rows[0]["rhogsc"].ToString()),
                double.Parse(result.Rows[0]["s"].ToString()),
                double.Parse(result.Rows[0]["d"].ToString()),
                double.Parse(result.Rows[0]["re"].ToString()),
                double.Parse(result.Rows[0]["rw"].ToString()),
                double.Parse(result.Rows[0]["pi"].ToString()),
                int.Parse(result.Rows[0]["gaswater_input_id"].ToString()),
                int.Parse(result.Rows[0]["well_id"].ToString()),
                ""
             );
            return gasWaterTwoModel;
        }


        /// <summary>
        /// 通过项目打开气水相渗
        /// </summary>
        /// <param name="proId"></param>
        /// <returns></returns>
        public static List <PhaseSeepage> GetPhaseSeepageByProject(string proId)
        {
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"SELECT DISTINCT phase_seepage_name, phase_seepage_index from phase_seepage WHERE project_id = @proId;";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            List<PhaseSeepage> models = new List<PhaseSeepage>();


            if (proId != null)
            {
                Paramter.Add(new MySqlParameter("proId", proId));
                result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            }
            if (result != null && result.Rows.Count != 0)
            {
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    models.Add(new PhaseSeepage(                        
                        Convert.ToInt32(result.Rows[i]["phase_seepage_index"]),
                        result.Rows[i]["phase_seepage_name"].ToString()
                        ));
                }
            }
            else
            {
                return null;
            }
            return models;
        }

        /// <summary>
        /// 读取压力平方法
        /// </summary>
        /// <param name="wellId"></param>
        /// /// <param name="type"> type=0读取压力平方法，type=1读取拟压力法</param>
        /// <returns></returns>
        public static List<GasModel> GetPressureByWellId(string wellId,int type)
        {
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            //TODO:在压力平方表中加入名称，修改下面的sql语句
            string sql = null;
            if (type == 0)
            {
                sql = @"select distinct gas_result_index, gas_product_index from gas_result_data WHERE well_id = @wellId and type=0;";

            }
            else
            {
                sql = @"select distinct gas_result_index, gas_product_index from gas_result_data WHERE well_id = @wellId and type=1;";

            }
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            List<GasModel> models = new List<GasModel>();

            if (wellId != null)
            {
                Paramter.Add(new MySqlParameter("wellId", wellId));
                result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            }
            if (result != null && result.Rows.Count != 0)
            {
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    models.Add(new GasModel(
                        Convert.ToInt32(result.Rows[i]["gas_result_index"]),
                        Convert.ToInt32(result.Rows[i]["gas_product_index"])
                        ));
                }
            }
            else
            {
                return null;
            }
            return models;
        }

        /// <summary>
        /// 读取PVTgas
        /// </summary>
        /// <param name="gasName"></param>
        /// <returns></returns>
        public static List<PVTGasModel> GetPVTGas(string gasName)
        {
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select * from pvt_gas WHERE gas_name = @gas_name;";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            List<PVTGasModel> models = new List<PVTGasModel>();
            if (gasName != null)
            {
                Paramter.Add(new MySqlParameter("gas_name", gasName));
                result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            }
            if (result != null && result.Rows.Count != 0)
            {
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    models.Add(new PVTGasModel(
                        result.Rows[i]["project_id"].ToString(),
                        Convert.ToDouble(result.Rows[i]["P"]),
                        Convert.ToDouble(result.Rows[i]["z1"]),
                        Convert.ToDouble(result.Rows[i]["z2"]),
                        Convert.ToDouble(result.Rows[i]["Bg"]),
                        Convert.ToDouble(result.Rows[i]["pg"]),
                        Convert.ToDouble(result.Rows[i]["ug"]),
                        Convert.ToDouble(result.Rows[i]["Cg"]),
                        Convert.ToDouble(result.Rows[i]["Pp"]),
                        Convert.ToDouble(result.Rows[i]["Rcwg"]),
                        gasName,
                        Convert.ToDouble(result.Rows[i]["yg"]),
                        Convert.ToDouble(result.Rows[i]["YCO2"]),
                        Convert.ToDouble(result.Rows[i]["YH2S"]),
                        Convert.ToDouble(result.Rows[i]["NaCl"]),
                        Convert.ToDouble(result.Rows[i]["T"]),
                        Convert.ToBoolean(result.Rows[i]["IsDryGas"]),
                        result.Rows[i]["pvt_gas_index"].ToString()
                        ));
                }
            }
            return models;
        }

        public static DataTable GetPVTGasTable(string gasName)
        {
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"SELECT P/1000000 as P,project_id,yg,YCO2,IsDryGas,YH2S,T,NaCl,z1,z2,Bg,pg,ug,Cg,Pp,Rcwg,gas_id,pvt_gas_index,gas_name FROM pvt_gas where gas_name = @gas_name;";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            if (gasName != null)
            {
                Paramter.Add(new MySqlParameter("gas_name", gasName));
                result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            }
            if (result != null && result.Rows.Count != 0)
            {
                result.Columns.Remove("project_id");
                result.Columns.Remove("yg");
                result.Columns.Remove("YCO2");
                result.Columns.Remove("YH2S");
                result.Columns.Remove("T");
                result.Columns.Remove("gas_id");
                result.Columns.Remove("pvt_gas_index");
                result.Columns.Remove("gas_name");
                result.Columns.Remove("NaCl");

                result.Columns["P"].ColumnName = "压力";
                result.Columns["z1"].ColumnName = "天然气偏差系数z1";
                result.Columns["z2"].ColumnName = "天然气偏差系数z2";
                result.Columns["bg"].ColumnName = "天然气体积系数";
                result.Columns["pg"].ColumnName = "天然气密度pg";
                result.Columns["ug"].ColumnName = "天然气粘度ug";
                result.Columns["Cg"].ColumnName = "天然气压缩系数Cg";
                result.Columns["Pp"].ColumnName = "天然气拟压力Pp";
                result.Columns["Rcwg"].ColumnName = "凝析水汽比Rcwg";
                return result;
            }
            return null;
        }

        /// <summary>
        /// 读取PVTwater
        /// </summary>
        /// <param name="waterName"></param>
        /// <returns></returns>
        public static List<PVTWaterModel> GetPVTWater(string waterName)
        {
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"SELECT * from pvt_water WHERE water_name = @water_name;";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            List<PVTWaterModel> models = new List<PVTWaterModel>();
            

            if (waterName != null)
            {
                Paramter.Add(new MySqlParameter("water_name", waterName));
                result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            }
            if (result != null && result.Rows.Count != 0)
            {
                for(int i = 0;i< result.Rows.Count; i++)
                {
                    models.Add(new PVTWaterModel(
                        result.Rows[i]["project_id"].ToString(),
                        result.Rows[i]["pvt_water_index"].ToString(),
                        Convert.ToDouble(result.Rows[i]["Pa"]),
                        Convert.ToDouble(result.Rows[i]["Bw"]),
                        Convert.ToDouble(result.Rows[i]["pw"]),
                        Convert.ToDouble(result.Rows[i]["uw"]),
                        Convert.ToDouble(result.Rows[i]["Cw"]),
                        Convert.ToDouble(result.Rows[i]["Rsw"]),
                        Convert.ToDouble(result.Rows[i]["Rcwg"]),
                        Convert.ToDouble(result.Rows[i]["K"]),
                        Convert.ToDouble(result.Rows[i]["NaCl"]),
                        waterName
                        ));
                }
            } else {
                return null;
            }
            return models;
        }
        public static DataTable GetPVTWaterTable(string waterName)
        {
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"SELECT * from pvt_water WHERE water_name = @water_name;";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();

            if (waterName != null)
            {
                Paramter.Add(new MySqlParameter("water_name", waterName));
                result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            }
            if (result != null && result.Rows.Count != 0)
            {
                result.Columns.Remove("project_id");
                result.Columns.Remove("water_id");
                result.Columns.Remove("water_name");
                result.Columns.Remove("pvt_water_index");
                result.Columns.Remove("K");
                result.Columns.Remove("NaCl");

                result.Columns["Pa"].ColumnName = "压力";
                result.Columns["Bw"].ColumnName = "地层体积系数";
                result.Columns["pw"].ColumnName = "地层水密度";
                result.Columns["uw"].ColumnName = "地层水粘度";
                result.Columns["Cw"].ColumnName = "地层水压缩系数";
                result.Columns["Rsw"].ColumnName = "溶解度";
                result.Columns["Rcwg"].ColumnName = "凝析水汽比";
                return result;
            }
            else
            {
                return null;
            }
        }

        public static PVTRockModel GetPVTRock(string rockName)
        {
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select * from pvt_rock WHERE rock_name = @rock_name";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            PVTRockModel model = new PVTRockModel();

            if (rockName != null)
            {
                Paramter.Add(new MySqlParameter("rock_name", rockName));
                result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            }
            if (result != null && result.Rows.Count != 0)
            {
                model.rock = Convert.ToDouble(result.Rows[0]["kxd"]);
                model.Sandstone = Convert.ToDouble(result.Rows[0]["ysysxs1"]);
                model.Limestone = Convert.ToDouble(result.Rows[0]["ysysxs2"]);
                model.rockName = rockName;
                model.project_id = result.Rows[0]["project_id"].ToString();
            }
            return model;
        }

        public static DataTable GetPVTRockTable(string rockName)
        {
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select * from pvt_rock WHERE rock_name = @rock_name";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            if (rockName != null)
            {
                Paramter.Add(new MySqlParameter("rock_name", rockName));
                result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            }
            return result;
        }

        /// <summary>
        /// 读取项目列表
        /// </summary>
        /// <returns></returns>
        public static List<ProjectModel> getProject()
        {
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"SELECT project_id,project_name from project";
            result = DbManager.Ins.ExcuteDataTable(sql);
            if(result == null)
            {
                return null;
            }
            List<ProjectModel> projects = new List<ProjectModel>();

            foreach(DataRow dr in result.Rows)
            {
                //dr["project_id"].ToString()
                projects.Add(
                    new ProjectModel(dr["project_name"].ToString(),
                    utils.Utils.UnixTimeStampToDateTime(Convert.ToDouble(dr["project_id"].ToString())).ToString(), 
                    dr["project_id"].ToString()));
                    
            }
            return projects;
        }
        /// <summary>
        /// 查询所有气水湘渗
        /// 
        /// </summary>
        /// <param name="clss"></param>
        /// <returns></returns>
        public static Dictionary<string, string> getPhaseSeepage()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string sql = @"select distinct phase_seepage_name,phase_seepage_index from phase_seepage";

            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;

            result = DbManager.Ins.ExcuteDataTable(sql);
            foreach (DataRow dr in result.Rows)
            {
                dict.Add(dr["phase_seepage_name"].ToString(), dr["phase_seepage_index"].ToString());
            }
            return dict;
        }

        /// <summary>
        /// 根据名称查气水相渗
        /// </summary>
        /// <param name="phaseSeepageName"></param>
        /// <returns></returns>
        public static DataTable GetPhaseSeepageByName(string phaseSeepageName)
        {
            
            string sql = @"select * from phase_seepage where phase_seepage_name = '" + phaseSeepageName + "'";

            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            result = DbManager.Ins.ExcuteDataTable(sql);
            return result;
        }


        /// <summary>
        /// 根据井ID查气水相渗
        /// </summary>
        /// <param name="phaseSeepageName"></param>
        /// <returns></returns>
        public static DataTable GetPhaseSeepageByWellid(string wellId)
        {
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;

            string sql = @"select phase_seepage_name from well where well_id = '" + wellId + "'";
            result = DbManager.Ins.ExcuteDataTable(sql);
            string phaseSeepageName = result.Rows[0]["phase_seepage_name"].ToString();

            sql = @"select * from phase_seepage where phase_seepage_name = '" + phaseSeepageName + "'";
            result = DbManager.Ins.ExcuteDataTable(sql);

            return result;
        }

        public static DataTable GetMiddleTableByMiddleIndex(string middleIndex)
        {
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;

            string sql = @"select * from niyali_middle_table where middle_index = '" + middleIndex + "'";
            result = DbManager.Ins.ExcuteDataTable(sql);
            result.Columns.Remove("well_id");
            result.Columns.Remove("middle_index");
            result.Columns.Remove("middle_id");
            result.Columns.Remove("gaswater_input_id");

            return result;
        }

        /// <summary>
        /// 传入PVT类型,查询对应集合
        /// </summary>
        /// <param name="clss">只能输入gas,water和rock</param>
        /// <returns></returns>
        public static Dictionary<string,string> getPVTByClss(string clss)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string sql;
            string index;
            string name;
            if (clss == "gas")
            {
                sql = @"SELECT gas_name,pvt_gas_index from pvt_gas GROUP BY pvt_gas_index";
                index = "pvt_gas_index";
                name = "gas_name";
            }
            else if (clss == "water")
            {
                sql = "SELECT water_name,pvt_water_index from pvt_water GROUP BY pvt_water_index";
                index = "pvt_water_index";
                name = "water_name";
            }
            else
            {
                sql = "SELECT rock_name,rock_id from pvt_rock";
                index = "rock_id";
                name = "rock_name";

            }
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;

            result = DbManager.Ins.ExcuteDataTable(sql);
            foreach(DataRow dr in result.Rows)
            {
                dict.Add(dr[index].ToString(), dr[name].ToString());
            }
            return dict;
        }

        /// <summary>
        /// 通过Project读取PVTgas
        /// </summary>
        /// <param name="proId"></param>
        /// <returns></returns>
        public static List<PVTGasModel> GetPVTGasByProject(string proId)
        {
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select DISTINCT gas_name, pvt_gas_index from pvt_gas WHERE project_id = @proId;";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            List<PVTGasModel> models = new List<PVTGasModel>();
            if (proId != null)
            {
                Paramter.Add(new MySqlParameter("proId", proId));
                result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            }
            if (result != null && result.Rows.Count != 0)
            {
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    models.Add(new PVTGasModel(
                        result.Rows[i]["gas_name"].ToString(),
                        result.Rows[i]["pvt_gas_index"].ToString()
                        ));
                }
            }
            return models;
        }

        /// <summary>
        /// 通过Project读取PVTwater
        /// </summary>
        /// <param name="proId"></param>
        /// <returns></returns>
        public static List<PVTWaterModel> GetPVTWaterByProject(string proId)
        {
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"SELECT DISTINCT water_name, pvt_water_index from pvt_water WHERE project_id = @proId;";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            List<PVTWaterModel> models = new List<PVTWaterModel>();


            if (proId != null)
            {
                Paramter.Add(new MySqlParameter("proId", proId));
                result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            }
            if (result != null && result.Rows.Count != 0)
            {
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    models.Add(new PVTWaterModel(
                        result.Rows[i]["water_name"].ToString(),
                        result.Rows[i]["pvt_water_index"].ToString()
                        ));
                }
            }
            else
            {
                return null;
            }
            return models;
        }

        /// <summary>
        /// 通过Project读取PVTrock
        /// </summary>
        /// <param name="proId"></param>
        /// <returns></returns>
        public static List<PVTRockModel> GetPVTRockByProject(string proId)
        {
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select DISTINCT rock_name, rock_id from pvt_rock WHERE project_id = @proId";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            List<PVTRockModel> models = new List<PVTRockModel>();

            if (proId != null)
            {
                Paramter.Add(new MySqlParameter("proId", proId));
                result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            }
            if (result != null && result.Rows.Count != 0)
            {
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    models.Add(new PVTRockModel(
                        int.Parse(result.Rows[i]["rock_id"].ToString()),
                        result.Rows[i]["rock_name"].ToString()
                        ));
                }
            }
            else
            {
                return null;
            }
            return models;
        }

        /// <summary>
        /// 通过Project读取Well
        /// </summary>
        /// <param name="proId"></param>
        /// <returns></returns>
        public static List<WellModel> GetWellByProject(string proId)
        {
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select well_id,xmmc from well WHERE project_id = @proId";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            List<WellModel> models = new List<WellModel>();

            if (proId != null)
            {
                Paramter.Add(new MySqlParameter("proId", proId));
                result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            }
            if (result != null && result.Rows.Count != 0)
            {
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    WellModel wellModel = new WellModel();
                    wellModel.well_id = int.Parse(result.Rows[i]["well_id"].ToString());
                    wellModel.xmmc = result.Rows[i]["xmmc"].ToString();
                    models.Add(wellModel);
                }
            }
            else
            {
                return null;
            }
            return models;
        }


        /// <summary>
        /// 读取WellModelByWellid
        /// </summary>
        /// <param name="proId"></param>
        /// <returns></returns>
        public static WellModel GetWellModelByWellId(string wellId)
        {
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select * from well WHERE well_id = @wellId";
            WellModel wellModel = new WellModel();
            List<MySqlParameter> Paramter = new List<MySqlParameter>();

            if (wellId != null)
            {
                Paramter.Add(new MySqlParameter("wellId", wellId));
                result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            }
            if (result != null && result.Rows.Count != 0)
            {
                wellModel.well_id = int.Parse(result.Rows[0]["well_id"].ToString());
                wellModel.pvt_gas_name = result.Rows[0]["pvt_gas_name"].ToString();
                wellModel.pvt_water_name = result.Rows[0]["pvt_water_name"].ToString();
                wellModel.pvt_rock_name = result.Rows[0]["pvt_rock_name"].ToString();
                wellModel.phase_seepage_name = result.Rows[0]["phase_seepage_name"].ToString();
            }
            else
            {
                return wellModel;
            }
            return wellModel;
        }


        /// <summary>
        /// 通过Well读取压力平方法
        /// </summary>
        /// <param name="wellId"></param>
        /// <returns></returns>
        public static List<FittingModel> GetFittingByWellId(string wellId)
        {
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select fitting_id from fitting WHERE well_id = @wellId";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            List<FittingModel> models = new List<FittingModel>();

            if (wellId != null)
            {
                Paramter.Add(new MySqlParameter("wellId", wellId));
                result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            }
            if (result != null && result.Rows.Count != 0)
            {
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    FittingModel fittingModel = new FittingModel();
                    fittingModel.fitting_id = result.Rows[i]["fitting_id"].ToString();
                    models.Add(fittingModel);
                }
            }
            else
            {
                return null;
            }
            return models;
        }
        /// <summary>
        /// 通过wellid查predid
        /// </summary>
        /// <param name="wellId"></param>
        /// <returns></returns>
        public static List<PredictModel> GetPredictByWellId(string wellId)
        {
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select pred_id from prediction WHERE well_id = @wellId";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            List<PredictModel> models = new List<PredictModel>();

            if (wellId != null)
            {
                Paramter.Add(new MySqlParameter("wellId", wellId));
                result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            }
            if (result != null && result.Rows.Count != 0)
            {
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    PredictModel predictModel = new PredictModel();
                    predictModel.pred_id = result.Rows[i]["pred_id"].ToString();
                    models.Add(predictModel);
                }
            }
            else
            {
                return null;
            }
            return models;
        }

        /// <summary>
        /// 通过Well读取气水两项法
        /// </summary>
        /// <param name="wellId"></param>
        /// <returns></returns>
        public static List<GasWaterModel> GetGasWaterByWellId(string wellId)
        {
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select DISTINCT  gaswater_input_id, gaswater_result_index from "+
                "gaswater_result where well_id = @wellId";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            List<GasWaterModel> models = new List<GasWaterModel>();

            if (wellId != null)
            {
                Paramter.Add(new MySqlParameter("wellId", wellId));
                result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            }
            if (result != null || result.Rows.Count != 0)
            {
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    GasWaterModel gasWaterModel = new GasWaterModel();
                    if (string.IsNullOrWhiteSpace(result.Rows[0]["gaswater_input_id"].ToString()))
                    {
                        continue;
                    }
                    gasWaterModel.gaswater_product_index = int.Parse(result.Rows[i]["gaswater_input_id"].ToString());
                    gasWaterModel.gaswater_result_index = int.Parse(result.Rows[i]["gaswater_result_index"].ToString());
                    models.Add(gasWaterModel);
                }
            }
            else
            {
                return null;
            }
            return models;
        }

        /// <summary>
        /// 通过WellId读取井参数表
        /// </summary>
        /// <param name="wellId"></param>
        /// <returns></returns>
        public static DataTable GetWellParmasTableByWellId(string wellId)
        {
            DataTable result = null;
            DbManager.Ins.ConnStr = Const.DBSTR;
            string sql = @"select * from well_params WHERE well_id = @wellId";
            List<MySqlParameter> Paramter = new List<MySqlParameter>();
            if (wellId != null)
            {
                Paramter.Add(new MySqlParameter("wellId", wellId));
                result = DbManager.Ins.ExcuteDataTable(sql, Paramter.ToArray());
            }
            result.Columns.Remove("well_id");
            result.Columns["s"].ColumnName = "气井表皮系数";
            result.Columns["d"].ColumnName = "非达西渗流系数m^3/d";
            result.Columns["re"].ColumnName = "井控半径m";
            result.Columns["rw"].ColumnName = "井筒半径m";
            result.Columns["rhogsc"].ColumnName = "气体密度kg/m3";
            result.Columns["rhowsc"].ColumnName = "水密度kg/m3";
            return result;
        }



    }
}
