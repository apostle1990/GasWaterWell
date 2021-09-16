
using GasWaterWell.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace GasWaterWell.utils
{
    class OperateXML
    {
        static XmlDocument doc = new XmlDocument();
        static string xmlPath = @"Package/rightMenuData.xml";
        static string itemName = "Name";
        static string type = "Type";
        static string Id = "Id";
        static string itemClass = "Class";
        public DataSet getXML(String xmlPath)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(xmlPath);
            return ds;
        }

        /// <summary>
        /// 读取xml文档并返回一个节点:适用于一级节点
        /// </summary>
        /// <param name="XmlPath">xml路径</param>
        /// <param name="NodeName">节点</param>
        /// <returns></returns>
        public static string ReadXmlReturnNode(string Node)
        {
            XmlDocument docXml = new XmlDocument();
            docXml.Load(xmlPath);
            XmlNodeList xn = docXml.GetElementsByTagName(Node);
            string nodeInf = xn.Item(0).InnerText.ToString();
            return nodeInf;
        }

        /// <summary>
        /// 删除XML节点和此节点下的子节点
        /// </summary>
        /// <param name="xmlPath">xml文档路径</param>
        /// <param name="Node">节点路径</param>
        public static void XmlNodeDelete(XmlElement xmle)
        {
            try
            {
                doc.Load(xmlPath);
                string type = xmle.GetAttribute("Type");
                string id = xmle.GetAttribute("Id");
                XmlNodeList childs = doc.GetElementsByTagName(type);
                Console.WriteLine(xmle.LocalName);
                Console.WriteLine(xmle.Name);
                XmlNode root = childs[0].ParentNode;
                foreach (XmlNode node in childs)
                {
                    XmlElement xe = (XmlElement)node;
                    if (xe.GetAttribute("Id") == id)
                    {
                        root.RemoveChild(node);//删除该节点的全部内容
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "哎呀，崩溃了。。。");
            }
            finally
            {
                doc.Save(xmlPath);
            }
            

        }

        /// <summary>
        /// 通过Id删除well
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        public static void XmlNodeDeleteById(string id,string type="Well")
        {
            try
            {
                doc.Load(xmlPath);
                XmlNodeList childs = doc.GetElementsByTagName(type);
                XmlNode root = childs[0].ParentNode;
                foreach (XmlNode node in childs)
                {
                    XmlElement xe = (XmlElement)node;
                    if (xe.GetAttribute("Id") == id)
                    {
                        root.RemoveChild(node);//删除该节点的全部内容
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "哎呀，崩溃了。。。");
            }
            finally
            {
                doc.Save(xmlPath);
            }


        }

        public static void DeleteAnalsisItemById(string id)
        {
            try
            {
                doc.Load(xmlPath);
                XmlNodeList childs = doc.GetElementsByTagName("TWItem");
                foreach (XmlNode node in childs)
                {
                    XmlElement xe = (XmlElement)node;
                    if (xe.GetAttribute("Id") == id)
                    {
                        XmlNode root = node.ParentNode;
                        root.RemoveChild(node);//删除该节点的全部内容
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "哎呀，崩溃了。。。");
            }
            finally
            {
                doc.Save(xmlPath);
            }
        }

        /// <summary>
        /// 查找结点
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static XmlNode SelectNode(string type, string id)
        {
            doc.Load(xmlPath);
            XmlNodeList xnl = doc.GetElementsByTagName(type);

            if (type == "PVTItem")
            {
                type = "Packages/Package/PVT";
            }
            else if (type == "GWPaseSeepage")
            {
                type = "Packages/Package/GWPaseSeepage";
            }
            else if (type == "MethodItem")
            {
                type = "Packages/Package/Well";
            }
            else if (type == "Well")
            {
                type = "Packages/Package";
            }
            XmlNode root = doc.SelectSingleNode(type);
            foreach (XmlNode xn in xnl)
            {
                XmlElement xe = (XmlElement)xn;
                //MessageBox.Show(xe.GetAttribute("Id"));
                if (xe.GetAttribute("Id") == id)
                {
                    return xn;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取所有的PVT
        /// </summary>
        /// <returns></returns>
        public static IDictionary<string, string> GetAllPVT()
        {
            doc.Load(xmlPath);
            IDictionary<string, string> pvts = new Dictionary<string, string>();
            XmlNodeList xnl = doc.GetElementsByTagName("PVTItem");
            foreach (XmlNode xn in xnl)
            {
                XmlElement xe = (XmlElement)xn;
                pvts.Add(xe.GetAttribute("Id"), xe.GetAttribute("Name"));
            };
            return pvts;
        }

        /// <summary>
        /// 获取所有的指定PVT
        /// </summary>
        /// <param name="name">
        /// name=1, gas
        /// name=2, water
        /// name=3, rock
        /// </param>
        /// <returns></returns>
        public static IDictionary<string, string> GetAllPVTByName(int name)
        {
            string containsStr;
            if(name == 1)
            {
                containsStr = ".gpvt";
            }
            else if (name == 2)
            {
                containsStr = ".wpvt";
            }
            else if (name == 3)
            {
                containsStr = ".rpvt";
            }
            else
            {
                containsStr = ".gwps";
            }
            doc.Load(xmlPath);
            IDictionary<string, string> pvts = new Dictionary<string, string>();
            XmlNodeList xnl = doc.GetElementsByTagName("PVTItem");
            foreach (XmlNode xn in xnl)
            {
                XmlElement xe = (XmlElement)xn;
                if (xe.GetAttribute("Name").Contains(containsStr))
                {
                    pvts.Add(xe.GetAttribute("Id"), xe.GetAttribute("Name"));
                }
            };
            return pvts;
        }

        public static IDictionary<string, string> GetGwpsByName()
        {
            string containsStr;
            containsStr = ".gwps";
            doc.Load(xmlPath);
            IDictionary<string, string> pvts = new Dictionary<string, string>();
            XmlNodeList xnl = doc.GetElementsByTagName("GWPSItem");
            foreach (XmlNode xn in xnl)
            {
                XmlElement xe = (XmlElement)xn;
                if (xe.GetAttribute("Name").Contains(containsStr))
                {
                    pvts.Add(xe.GetAttribute("Id"), xe.GetAttribute("Name"));
                }
            };
            return pvts;
        }

        /// <summary>
        /// 插入PVTItem
        /// </summary>
        /// <param name="xmlPath">Xml文档路径</param>
        /// <param name="nodeName">当前节点路径</param>
        /// <param name="name">新节点</param>
        /// <param name="name">name属性</param>
        /// <param name="id">id属性</param>
        /// <param name="clss">clss属性</param>
        public static void XmlInsertPVTEle(string id, string name, string clss = "###")
        {
            doc.Load(xmlPath);
            XmlNode objNode = doc.SelectSingleNode("Packages/Package/PVT");
            XmlElement objElement;         
            objElement = doc.CreateElement("PVTItem");
            objElement.SetAttribute(type, "PVTItem");
            objElement.SetAttribute(itemClass, clss);
            objElement.SetAttribute(itemName, name);
            objElement.SetAttribute(Id, id);
            //TODO: 检查问题
            objNode.AppendChild(objElement);
            doc.Save(xmlPath);
        }

        /// <summary>
        /// 插入气水相渗Item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public static void XmlInsertGWPhaseSeepageItem(string id,string name)
        {
            doc.Load(xmlPath);
            XmlNode objNode = doc.SelectSingleNode("Packages/Package/GWPhaseSeepage");
            XmlElement objElement;
            objElement = doc.CreateElement("GWPSItem");
            objElement.SetAttribute(type, "GWPSItem");
            objElement.SetAttribute(itemName, name);
            objElement.SetAttribute(Id, id);
            //TODO: 检查问题
            objNode.AppendChild(objElement);
            doc.Save(xmlPath);
        }

        /// <summary>
        /// 插入Well
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public static void XmlInsertWell(string id, string name)
        {
            doc.Load(xmlPath);
            XmlNode objNode = doc.SelectSingleNode("Packages/Package");
            XmlElement well;
            well = doc.CreateElement("Well");
            well.SetAttribute(type, "Well");
            well.SetAttribute(itemName, name);
            well.SetAttribute(Id, id);

            XmlElement basicPara;
            basicPara = doc.CreateElement("BasicPara");
            basicPara.SetAttribute(type,"Para");
            basicPara.SetAttribute(itemName, "基础参数");
            basicPara.SetAttribute("WellId", id);

            XmlElement testWell;
            testWell = doc.CreateElement("TestWell");
            testWell.SetAttribute(type, "TestWell");
            testWell.SetAttribute(itemName, "产能试井");
            testWell.SetAttribute("WellId", id);

            XmlElement spbGas;
            spbGas = doc.CreateElement("SPBGas");
            spbGas.SetAttribute(type, "SPBG");
            spbGas.SetAttribute(itemName, "单点二项式法-气相");

            XmlElement pressMethod;
            pressMethod = doc.CreateElement("PressMethod");
            pressMethod.SetAttribute(type, "PressMethod");
            pressMethod.SetAttribute(itemName, "压力平方法");
            pressMethod.SetAttribute("WellId",id);

            XmlElement pseudoPress;//拟压力法
            pseudoPress = doc.CreateElement("PseudoPress");
            pseudoPress.SetAttribute(type, "PPress");
            pseudoPress.SetAttribute(itemName, "拟压力法");
            pseudoPress.SetAttribute("WellId", id);

            spbGas.AppendChild(pressMethod);
            spbGas.AppendChild(pseudoPress);

            XmlElement spbGasWater;
            spbGasWater = doc.CreateElement("SPBGasWater");
            spbGasWater.SetAttribute(type, "SPBGW");
            spbGasWater.SetAttribute(itemName, "单点二项式法-气水两相");
            spbGasWater.SetAttribute("WellId", id);


            well.AppendChild(basicPara);
            well.AppendChild(testWell);
            well.AppendChild(spbGas);
            well.AppendChild(spbGasWater);
            objNode.AppendChild(well);
            doc.Save(xmlPath);
        }


        /// <summary>
        /// 插入产能试井
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public static void XmlInsertTestWellItem(string fitId,string predId, string name,string wellId)
        {
            doc.Load(xmlPath);
            XmlNode objNode = doc.SelectSingleNode("Packages/Package");
            string id;
            XmlElement objElement;
            if (predId == "")  //如果说产能预测为空，则指插入成分析
            {
                name = "产能分析"+name + ".aly";
                id = fitId;
            }
            else
            {
                name = "产能预测"+name + ".pre";
                id = predId;
            }
            objNode = SelectNode("Well", wellId);
            XmlNode pressNode = objNode.ChildNodes[1];

            objElement = doc.CreateElement("TWItem");
            objElement.SetAttribute(type, "TWItem");
            objElement.SetAttribute(itemName, name);
            objElement.SetAttribute(Id, id);
            objElement.SetAttribute("FitId", fitId);
            objElement.SetAttribute("PredId", predId);


            pressNode.AppendChild(objElement);
            doc.Save(xmlPath);
        }

        public static string XmlInsertTestWellItem2(string fitId, string predId, string name, string wellId)
        {
            doc.Load(xmlPath);
            XmlNode objNode = doc.SelectSingleNode("Packages/Package");
            string id;
            XmlElement objElement;
            if (predId == null)  //如果说产能预测为空，则指插入成分析
            {
                name = "产能分析" + name + ".aly";
                id = fitId;
            }
            else
            {
                name = "产能预测" + name + ".pre";
                id = predId;
            }
            objNode = SelectNode("Well", wellId);
            XmlNode pressNode = objNode.ChildNodes[1];

            objElement = doc.CreateElement("TWItem");
            objElement.SetAttribute(type, "TWItem");
            objElement.SetAttribute(itemName, name);
            objElement.SetAttribute(Id, id);
            objElement.SetAttribute("FitId", fitId);
            objElement.SetAttribute("PredId", predId);


            pressNode.AppendChild(objElement);
            doc.Save(xmlPath);
            return name;
        }

        /// <summary>
        /// 在well中插入压力平方法
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="clss"></param>
        public static void XmlInsertPressMethod(string wellId,string id,string resultId, string name)
        {
            doc.Load(xmlPath);
            name = "单点二项式-气相-压力平方法" + name + ".psm";
            XmlNode objNode = doc.SelectSingleNode("Packages/Package");
            objNode = SelectNode("Well", wellId);
            XmlNode pressNode = objNode.ChildNodes[2].FirstChild;
            XmlElement pressMethodItem;
            pressMethodItem = doc.CreateElement("PressMethodItem");
            pressMethodItem.SetAttribute(type, "PressMethodItem");
            pressMethodItem.SetAttribute(itemName, name);
            pressMethodItem.SetAttribute(Id, id);
            pressMethodItem.SetAttribute("WellId", wellId);
            pressMethodItem.SetAttribute("ResultId", resultId);

            pressNode.AppendChild(pressMethodItem);
            doc.Save(xmlPath);
        }

        public static string XmlInsertPressMethod2(string wellId, string id, string resultId, string name)
        {
            doc.Load(xmlPath);
            name = "单点二项式-气相-压力平方法" + name + ".psm";
            XmlNode objNode = doc.SelectSingleNode("Packages/Package");
            objNode = SelectNode("Well", wellId);
            XmlNode pressNode = objNode.ChildNodes[2].FirstChild;
            XmlElement pressMethodItem;
            pressMethodItem = doc.CreateElement("PressMethodItem");
            pressMethodItem.SetAttribute(type, "PressMethodItem");
            pressMethodItem.SetAttribute(itemName, name);
            pressMethodItem.SetAttribute(Id, id);
            pressMethodItem.SetAttribute("WellId", wellId);
            pressMethodItem.SetAttribute("ResultId", resultId);

            pressNode.AppendChild(pressMethodItem);
            doc.Save(xmlPath);
            return name;
        }
        /// <summary>
        /// 在well中插入拟压力法
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="clss"></param>
        public static void XmlInsertPseudoPressMethod(string wellId, string id,string resultId,string name)
        {
            doc.Load(xmlPath);
            name = "单点二项式-气相-拟压力" + name + ".ppm";
            XmlNode objNode = doc.SelectSingleNode("Packages/Package");
            objNode = SelectNode("Well", wellId);
            XmlNode pressNode = objNode.ChildNodes[2].LastChild;
            XmlElement pressMethodItem;
            pressMethodItem = doc.CreateElement("PPressItem");
            pressMethodItem.SetAttribute(type, "PPressItem");
            pressMethodItem.SetAttribute(itemName, name);
            pressMethodItem.SetAttribute(Id, id);
            pressMethodItem.SetAttribute("WellId", wellId);
            pressMethodItem.SetAttribute("ResultId", resultId);

            pressNode.AppendChild(pressMethodItem);
            doc.Save(xmlPath);
        }

        public static string XmlInsertPseudoPressMethod2(string wellId, string id, string resultId, string name)
        {
            doc.Load(xmlPath);
            name = "单点二项式-气相-拟压力" + name + ".ppm";
            XmlNode objNode = doc.SelectSingleNode("Packages/Package");
            objNode = SelectNode("Well", wellId);
            XmlNode pressNode = objNode.ChildNodes[2].LastChild;
            XmlElement pressMethodItem;
            pressMethodItem = doc.CreateElement("PPressItem");
            pressMethodItem.SetAttribute(type, "PPressItem");
            pressMethodItem.SetAttribute(itemName, name);
            pressMethodItem.SetAttribute(Id, id);
            pressMethodItem.SetAttribute("WellId", wellId);
            pressMethodItem.SetAttribute("ResultId", resultId);

            pressNode.AppendChild(pressMethodItem);
            doc.Save(xmlPath);
            return name;
        }

        /// <summary>
        /// 插入水气两相法
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="clss"></param>
        public static void XmlInsertGasWaterMethod(string wellId,string id, string resultId, string name)
        {
            doc.Load(xmlPath);
            name = "单点法-气水两相" + name + ".gwp";
            XmlNode objNode = doc.SelectSingleNode("Packages/Package");
            XmlElement objElement;
            objNode = SelectNode("Well", wellId);
            XmlNode pressNode = objNode.ChildNodes[3];
            objElement = doc.CreateElement("SPBGWItem");
            objElement.SetAttribute(type, "SPBGWItem");
            objElement.SetAttribute(itemName, name);
            objElement.SetAttribute(Id, id);
            objElement.SetAttribute("WellId", wellId);
            objElement.SetAttribute("ResultId", resultId);
            //TODO:插入ProductId

            pressNode.AppendChild(objElement);
            doc.Save(xmlPath);
        }

        public static string XmlInsertGasWaterMethod2(string wellId, string id, string resultId, string name)
        {
            doc.Load(xmlPath);
            name = "单点法-气水两相" + name + ".gwp";
            XmlNode objNode = doc.SelectSingleNode("Packages/Package");
            XmlElement objElement;
            objNode = SelectNode("Well", wellId);
            XmlNode pressNode = objNode.ChildNodes[3];
            objElement = doc.CreateElement("SPBGWItem");
            objElement.SetAttribute(type, "SPBGWItem");
            objElement.SetAttribute(itemName, name);
            objElement.SetAttribute(Id, id);
            objElement.SetAttribute("WellId", wellId);
            objElement.SetAttribute("ResultId", resultId);
            //TODO:插入ProductId

            pressNode.AppendChild(objElement);
            doc.Save(xmlPath);
            return name;
        }

        /// <summary>
        /// 创建新的项目
        /// </summary>
        /// <param name="proId"></param>
        /// <param name="proName"></param>
        public static void XMLCreateProject(string proId,string proName)
        {
            doc.Load(xmlPath);
            XmlNode objNode = doc.SelectSingleNode("Packages");
            //MessageBox.Show(((XmlElement)objNode).GetAttribute("Name"));
            //TODO 没有获取到所有节点  只有PVT节点
            objNode.RemoveAll();
            XmlElement package = doc.CreateElement("Package");
            package.SetAttribute("Name", proName);
            package.SetAttribute("Id", proId);
            package.SetAttribute("Type", "Package");

            XmlElement PVT=doc.CreateElement("PVT");
            PVT.SetAttribute("Name", "PVT");
            PVT.SetAttribute("Type", "PVT");
            PVT.SetAttribute("Id", proId);

            XmlElement GWPaseSeepage;
            GWPaseSeepage = doc.CreateElement("GWPhaseSeepage");
            GWPaseSeepage.SetAttribute(type, "GWPS");
            GWPaseSeepage.SetAttribute(itemName, "气水相渗");

            package.AppendChild(PVT);
            package.AppendChild(GWPaseSeepage);
            objNode.AppendChild(package);
            doc.Save(xmlPath);
        }

        /// <summary>
        /// 打开一个项目，写目录结构
        /// </summary>
        /// <param name="proId"></param>
        /// <param name="proName"></param>
        public static void XMLOpenProject(String proId, String proName)
        {
            //Console.WriteLine(proId, proName);
            // 首先新建一个
            XMLCreateProject(proId, proName);
            // 然后遍历PVT
            List<PVTGasModel> pVTGasModels = DbRead.GetPVTGasByProject(proId);
            if (pVTGasModels != null)
            {
                foreach (PVTGasModel pVTGasModel in pVTGasModels)
                {
                    XmlInsertPVTEle(pVTGasModel.gasName, pVTGasModel.gasName, "gas");
                }
            }
            List<PVTWaterModel> pVTWaterModels = DbRead.GetPVTWaterByProject(proId);
            if (pVTWaterModels != null)
            {
                foreach (PVTWaterModel pVTWaterModel in pVTWaterModels)
                {
                    XmlInsertPVTEle(pVTWaterModel.waterName, pVTWaterModel.waterName, "water");
                }
            }
            List<PVTRockModel> PVTRockModels = DbRead.GetPVTRockByProject(proId);
            if(PVTRockModels != null)
            {
                foreach (PVTRockModel pVTRockModel in PVTRockModels)
                {
                    XmlInsertPVTEle(pVTRockModel.rock_id.ToString(), pVTRockModel.rockName, "rock");
                }
            }
            // 遍历气水相渗
            List<PhaseSeepage> phaseSeepages = DbRead.GetPhaseSeepageByProject(GetProjectId());
            if (phaseSeepages != null)
            {
                foreach (PhaseSeepage phaseSeepage in phaseSeepages)
                {
                    XmlInsertGWPhaseSeepageItem(phaseSeepage.PhaseSeepageIndex.ToString(), phaseSeepage.PhaseSeepageName);
                }
            }

            // 然后遍历井
            List<WellModel> wellModels = DbRead.GetWellByProject(proId);
            if(wellModels != null)
            {
                foreach (WellModel wellModel in wellModels)
                {
                    string well_id = wellModel.well_id.ToString();
                    XmlInsertWell(well_id, wellModel.xmmc);


                    //遍历产能试井
                    List<FittingModel> fittingModels = DbRead.GetFittingByWellId(well_id);
                    if(fittingModels != null)
                    {
                        foreach (FittingModel fittingModel in fittingModels)
                        {
                            string name = Utils.UnixTimeStampToDateTime(double.Parse(fittingModel.fitting_id)).ToString();
                            PredictModel predictModel = DbRead.GetPredictByFitId(fittingModel.fitting_id);
                            if(predictModel == null)
                            {
                                XmlInsertTestWellItem(fittingModel.fitting_id, "", name, well_id);
                            }
                        }
                    }
                    List<PredictModel> predictModels = DbRead.GetPredictByWellId(well_id);
                    if (predictModels != null)
                    {
                        foreach (PredictModel predictModel in predictModels)
                        {
                            string name = Utils.UnixTimeStampToDateTime(double.Parse(predictModel.pred_id)).ToString();
                            XmlInsertTestWellItem(predictModel.fittingId, predictModel.pred_id, name, well_id);
                        }
                    }

                    //遍历压力平方法
                    List<GasModel> gasModels = DbRead.GetPressureByWellId(well_id,0);
                    if (gasModels != null)
                    {
                        foreach (GasModel gasModel in gasModels)
                        {
                            string name = Utils.UnixTimeStampToDateTime(double.Parse(gasModel.gas_result_index.ToString())).ToString();
                            XmlInsertPressMethod(well_id,gasModel.gas_product_index.ToString(),gasModel.gas_result_index.ToString(), name);
                        }
                    }
                    //遍历拟压力法
                    List<GasModel> gasPseudoModels = DbRead.GetPressureByWellId(well_id,1);
                    if (gasModels != null)
                    {
                        foreach (GasModel gasModel in gasModels)
                        {
                            string name = Utils.UnixTimeStampToDateTime(double.Parse(gasModel.gas_result_index.ToString())).ToString();
                            XmlInsertPseudoPressMethod(well_id, gasModel.gas_product_index.ToString(), gasModel.gas_result_index.ToString(), name);
                        }
                    }

                    // 然后遍历气水两项法
                    List<GasWaterModel> gasWaterModels = DbRead.GetGasWaterByWellId(well_id);
                    if(gasWaterModels != null)
                    {
                        foreach (GasWaterModel gasWaterModel in gasWaterModels)
                        {
                            string name = Utils.UnixTimeStampToDateTime(gasWaterModel.gaswater_result_index).ToString();
                            XmlInsertGasWaterMethod(well_id, gasWaterModel.gaswater_product_index.ToString(), gasWaterModel.gaswater_result_index.ToString(), name);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取项目Id
        /// </summary>
        /// <returns></returns>
        public static string GetProjectId()
        {
            doc.Load(xmlPath);
            XmlNode objNode = doc.SelectSingleNode("Packages/Package");
            XmlElement package = (XmlElement)objNode;
            return package.GetAttribute("Id");
        }
    }
}