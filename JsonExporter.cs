using System;
using System.IO;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace excel2json
{
    /// <summary>
    /// 将DataTable对象，转换成JSON string，并保存到文件中
    /// </summary>
    class ExporterJson
    {
        public List<Dictionary<string, object>> m_data_lst;


        private JObject ParseVec2(string jstr, ref bool _isSuccess)
        {
            jstr = jstr.Trim();
            if (string.IsNullOrEmpty(jstr))
            {
                _isSuccess = true;
                JObject jobj = new JObject();
                jobj.Add("x", 0);
                jobj.Add("y", 0);
                return jobj;
            }

            JArray ary = JArray.Parse(jstr);
            if (ary.Count != 2)
            {
                _isSuccess = false;
                JObject jobj = new JObject();
                jobj.Add("x", 0);
                jobj.Add("y", 0);
                return jobj;
            }
            {
                _isSuccess = true;
                JObject jobj = new JObject();
                jobj.Add("x", ary[0]);
                jobj.Add("y", ary[1]);
                return jobj;
            }
        }

        private JObject ParseVec2(JToken jToken)
        {
            {
                JObject jobj = new JObject();
                jobj.Add("x", jToken[0]);
                jobj.Add("y", jToken[1]);
                return jobj;
            }
        }

        private JObject ParseVec3(string jstr, ref bool _isSuccess)
        {
            jstr = jstr.Trim();
            if (string.IsNullOrEmpty(jstr))
            {
                _isSuccess = true;
                JObject jobj = new JObject();
                jobj.Add("x", 0);
                jobj.Add("y", 0);
                jobj.Add("z", 0);
                return jobj;
            }

            JArray ary = JArray.Parse(jstr);
            if (ary.Count != 3)
            {
                _isSuccess = false;
                JObject jobj = new JObject();
                jobj.Add("x", 0);
                jobj.Add("y", 0);
                jobj.Add("z", 0);
                return jobj;
            }
            {
                _isSuccess = true;
                JObject jobj = new JObject();
                jobj.Add("x", ary[0]);
                jobj.Add("y", ary[1]);
                jobj.Add("z", ary[2]);
                return jobj;
            }
        }

        private JObject ParseVec3(JToken jToken)
        {
            {
                JObject jobj = new JObject();
                jobj.Add("x", jToken[0]);
                jobj.Add("y", jToken[1]);
                jobj.Add("z", jToken[2]);
                return jobj;
            }
        }

        // 从表格导出数据
        public bool ExportJson(DataTable sheet, DataRow KeyRow, DataRow PlatformRow, DataRow TypeRow, DataRow row, string target, int rowIdx, bool lowcase = false)
        {
            int rowNum = 0;
            var rowData = new Dictionary<string, object>();
            foreach (DataColumn column in sheet.Columns)
            {
                rowNum++;
                object apartValue = PlatformRow[column];
                string apartStr = apartValue.ToString().Trim();
                if (apartStr == "N" || string.IsNullOrEmpty(apartStr.Trim()))
                {// 跳过不导出的的
                    continue;
                }

                if (!apartStr.Equals("A"))
                {
                    if (apartStr != target)
                    {// 跳过非目标平台的
                        continue;
                    }
                }


                object val = row[column];
                string typeStr = TypeRow[column].ToString().Trim();

                string keyStr = KeyRow[column].ToString().Trim();

                if (string.IsNullOrEmpty(val.ToString()))
                {// 默认值

                    if (typeStr == "int")
                    {
                        val = 0;
                        rowData[keyStr] = val;
                        continue;
                    }
                    else if (typeStr == "string")
                    {
                        val = "";
                        rowData[keyStr] = val;
                        continue;
                    }
                    else if (typeStr == "vec2")
                    {
                        val = "";
                    }
                    else if (typeStr == "vec3")
                    {
                        val = "";
                    }
                    else
                    {
                        val = "[]";
                    }
                    //rowData[keyStr] = val;
                    //continue;
                }


                // 去掉数值字段的“.0”
                if (val.GetType() == typeof(double))
                {
                    double num = (double)val;
                    if ((int)num == num)
                        val = (int)num;
                    //Console.WriteLine(string.Format("{0}.行(id:{1}).列({}) 数据有错", sheet.TableName, TypeRow.))
                    //continue;
                }

                string fieldName = column.ToString();
                try
                {
                    // 表头自动转换成小写
                    if (lowcase)
                        fieldName = fieldName.ToLower();

                    if (!string.IsNullOrEmpty(fieldName))
                    {
                        rowData[keyStr] = val;
                        if (typeStr == "int")
                        {
                            rowData[keyStr] = val;
                        }
                        else if (typeStr == "string")
                        {
                            rowData[keyStr] = val;
                        }
                        else if (typeStr == "list")
                        {
                            string jstr = val as string;
                            if (!string.IsNullOrEmpty(jstr.Trim()))
                            {
                                string ch1 = jstr.Substring(0, 1);
                                if (ch1 != "[")
                                {
                                    Console.WriteLine("[Error] format error. not List. 行列[" + rowIdx + "," + keyStr + "]. target:" + jstr);

                                    continue;
                                }
                            }

                            //Console.WriteLine("jstr:" + jstr);
                            JArray ary = JArray.Parse(jstr);
                            rowData[keyStr] = ary;
                        }
                        else if (typeStr == "list<string>")
                        {
                            string jstr = val as string;
                            if (!string.IsNullOrEmpty(jstr.Trim()))
                            {
                                string ch1 = jstr.Substring(0, 1);
                                if (ch1 != "[")
                                {
                                    Console.WriteLine("[Error] format error. not List. 行列[" + rowIdx + "," + keyStr + "]. target:" + jstr);

                                    continue;
                                }
                            }

                            JArray ary = JArray.Parse(jstr);
                            rowData[keyStr] = ary;
                        }
                        else if (typeStr == "vec2")
                        {
                            bool isSuccess = false;
                            rowData[keyStr] = ParseVec2(val as string, ref isSuccess);
                            if (!isSuccess)
                            {
                                Console.WriteLine("[Error] format error. not vec2. 行列[" + rowIdx + "," + keyStr + "]. target:" + val);
                            }

                        }
                        else if (typeStr == "vec3")
                        {
                            bool isSuccess = false;
                            rowData[keyStr] = ParseVec3(val as string, ref isSuccess);
                            if (!isSuccess)
                            {
                                Console.WriteLine("[Error] format error. not vec3. 行列[" + rowIdx + "," + keyStr + "]. target:" + val);
                            }
                        }
                        else if (typeStr == "list<list>")
                        {
                            string jstr = val as string;
                            if (!string.IsNullOrEmpty(jstr.Trim()))
                            {
                                string ch1 = jstr.Substring(0, 1);
                                if (ch1 != "[")
                                {
                                    Console.WriteLine("[Error] format error. not List. 行列[" + rowIdx + "," + keyStr + "]. target:" + jstr);

                                    continue;
                                }
                            }

                            JArray ary = JArray.Parse(jstr);
                            rowData[keyStr] = ary;

                        }
                        else if (typeStr == "list<vec2>")
                        {
                            string jstr = val as string;
                            if (!string.IsNullOrEmpty(jstr.Trim()))
                            {
                                string ch1 = jstr.Substring(0, 1);
                                if (ch1 != "[")
                                {
                                    Console.WriteLine("[Error] format error. not List. 行列[" + rowIdx + "," + keyStr + "]. target:" + jstr);

                                    continue;
                                }
                            }

                            //Console.WriteLine("jstr:" + jstr);
                            JArray ary = JArray.Parse(jstr);



                            JArray aryRet = new JArray();
                            foreach (var aryi in ary)
                            {
                                if (aryi.Count() != 0 && aryi.Count() != 2)
                                {
                                    Console.WriteLine("[Error] format error. not List vec2. 行列[" + rowIdx + "," + keyStr + "]. target:" + jstr);
                                    continue;
                                }

                                aryRet.Add(ParseVec2(aryi));
                                //JObject jObjVec2 = new JObject();
                                //jObjVec2.Add("X", aryi[0]);
                                //jObjVec2.Add("Y", aryi[1]);
                                //aryRet.Add(jObjVec2);
                            }

                            rowData[keyStr] = aryRet;

                        }
                        else if (typeStr == "list<vec3>")
                        {
                            string jstr = val as string;
                            if (!string.IsNullOrEmpty(jstr.Trim()))
                            {
                                string ch1 = jstr.Substring(0, 1);
                                if (ch1 != "[")
                                {
                                    Console.WriteLine("[Error] format error. not List. 行列[" + rowIdx + "," + keyStr + "]. target:" + jstr);
                                    continue;
                                }
                            }

                            //Console.WriteLine("jstr:" + jstr);
                            JArray ary = JArray.Parse(jstr);


                            JArray aryRet = new JArray();
                            foreach (var aryi in ary)
                            {
                                if (aryi.Count() != 0 && aryi.Count() != 3)
                                {
                                    Console.WriteLine("[Error] format error. not List vec3. 行列[" + rowIdx + "," + keyStr + "]. target:" + jstr);
                                    continue;
                                }
                                aryRet.Add(ParseVec3(aryi));
                            }

                            rowData[keyStr] = aryRet;
                        }
                        else
                        {
                            Console.WriteLine("不支持的数据类型:" + typeStr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("[Error] 异常. 行列[{0},{1}], target:{2}\nException:{3}\n", rowNum, fieldName, val, ex.ToString()));
                    return false;
                }

            }



            m_data_lst.Add(rowData);

            return true;
        }

    }




    /// <summary>
    /// 将DataTable对象，转换成JSON string，并保存到文件中
    /// </summary>
    class ExporterClientJson : ExporterJson
    {

        /// <summary>
        /// 构造函数：完成内部数据创建
        /// </summary>
        /// <param name="sheet">ExcelReader创建的一个表单</param>
        /// <param name="headerRows">表单中的那几行是表头</param>
        public ExporterClientJson(DataTable sheet, int headerRows, bool lowcase)
        {
            if (sheet.Columns.Count <= 0)
                return;
            if (sheet.Rows.Count <= 0)
                return;

            m_data_lst = new List<Dictionary<string, object>>();

            DataRow KeyRow = sheet.Rows[headerRows - 3];
            DataRow TypeRow = sheet.Rows[headerRows - 2];
            DataRow PlatformRow = sheet.Rows[headerRows - 1];

            string sheetName = sheet.TableName;

            //--以第一列为ID，转换成ID->Object的字典
            int firstDataRow = headerRows;
            for (int i = firstDataRow; i < sheet.Rows.Count; i++)
            {
                DataRow row = sheet.Rows[i];
                string ID = row[sheet.Columns[0]].ToString();
                if (ID.Length <= 0)
                    continue;

                bool isSeccuss = ExportJson(sheet, KeyRow, PlatformRow, TypeRow, row, "C", i + 1, lowcase);
                if (!isSeccuss)
                {
                    Console.WriteLine(string.Format("导出错误. 数据{0}.id({1})", sheetName, ID));
                }
            }
        }

        /// <summary>
        /// 将内部数据转换成Json文本，并保存至文件
        /// </summary>
        /// <param name="jsonPath">输出文件路径</param>
        public void SaveToClientFile(string filePath, Encoding encoding)
        {
            if (m_data_lst == null)
                throw new Exception("JsonExporter内部数据为空。");

            // 转换为JSON字符串      
            string json = JsonConvert.SerializeObject(m_data_lst, Formatting.Indented);

            // 保存文件
            using (FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (TextWriter writer = new StreamWriter(file, encoding))
                    writer.Write(json);
            }
        }
    }


    /// <summary>
    /// 将DataTable对象，转换成JSON string，并保存到文件中
    /// </summary>
    class ExporterServerJson : ExporterJson
    {
        //List<Dictionary<string, object>> m_data_lst;

        /// <summary>
        /// 构造函数：完成内部数据创建
        /// </summary>
        /// <param name="sheet">ExcelReader创建的一个表单</param>
        /// <param name="headerRows">表单中的那几行是表头</param>
        public ExporterServerJson(DataTable sheet, int headerRows, bool lowcase)
        {
            if (sheet.Columns.Count <= 0)
                return;
            if (sheet.Rows.Count <= 0)
                return;

            m_data_lst = new List<Dictionary<string, object>>();

            DataRow KeyRow = sheet.Rows[headerRows - 3];
            DataRow TypeRow = sheet.Rows[headerRows - 2];
            DataRow PlatformRow = sheet.Rows[headerRows - 1];

            //DataRow PlatformRow = sheet.Rows[headerRows - 2];
            //DataRow TypeRow = sheet.Rows[headerRows - 3];
            //DataRow KeyRow = sheet.Rows[headerRows - 4];
            //--以第一列为ID，转换成ID->Object的字典

            string sheetName = sheet.TableName;

            int firstDataRow = headerRows;
            for (int i = firstDataRow; i < sheet.Rows.Count; i++)
            {
                DataRow row = sheet.Rows[i];
                string ID = row[sheet.Columns[0]].ToString();
                if (ID.Length <= 0)
                    continue;

                bool isSeccuss = ExportJson(sheet, KeyRow, PlatformRow, TypeRow, row, "S", i + 1, lowcase);
                if (!isSeccuss)
                {
                    Console.WriteLine(string.Format("导出错误. 数据{0}.id({1})", sheetName, ID));
                }
            }
        }

        /// <summary>
        /// 将内部数据转换成Json文本，并保存至文件
        /// </summary>
        /// <param name="jsonPath">输出文件路径</param>
        public void SaveToServerFile(string filePath, Encoding encoding)
        {
            if (m_data_lst == null)
                throw new Exception("JsonExporter内部数据为空。");

            // 转换为JSON字符串      
            string json = JsonConvert.SerializeObject(m_data_lst, Formatting.Indented);

            // 保存文件
            using (FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (TextWriter writer = new StreamWriter(file, encoding))
                    writer.Write(json);
            }
        }
    }
}
