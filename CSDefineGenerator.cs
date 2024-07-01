using System;
using System.IO;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace excel2json
{
    struct FieldDef
    {
        public string name;
        public string type;
        public string comment;
    }

    /// <summary>
    /// 根据表头，生成C#类定义数据结构
    /// 表头使用三行定义：字段名称、字段类型、注释
    /// </summary>
    class CSDefineGenerator
    {


        List<FieldDef> m_fieldList;

        public String ClassComment
        {
            get;
            set;
        }

        public CSDefineGenerator(DataTable sheet)
        {
            // First Row as Column Name
            if (sheet.Rows.Count < 4)
                return;

            m_fieldList = new List<FieldDef>();          
            DataRow commentRow = sheet.Rows[0];
            DataRow nameRow = sheet.Rows[1];
            DataRow typeRow = sheet.Rows[2];
            FieldDef field;

            int rowIdx = 0;
            foreach (DataColumn column in sheet.Columns)
            {
                rowIdx++;
               
                field.name = nameRow[column].ToString().Trim();
                field.comment = commentRow[column].ToString().Trim();
                field.type = typeRow[column].ToString().Trim();

                if (string.IsNullOrEmpty(field.name.Trim()))
                    continue;

                if (string.IsNullOrEmpty(field.comment.Trim()))
                    continue;

                // 确定真实的类型 *: Vec只能是数字
                if (field.type == "vec2")
                {
                    field.type = "Vec2";
                }
                else if (field.type == "vec3")
                {
                    field.type = "Vec3";
                }
                else if (field.type == "list")
                {// list 最多支持3层嵌套 
                    field.type = "List<int>";
                }
                else if (field.type == "list<string>")
                {
                    field.type = "List<string>";
                }
                else if (field.type == "list<list>")
                {
                    field.type = "List<List<int>>";
                }
                else if (field.type == "list<vec2>")
                {
                    field.type = "List<Vec2>";
                }
                else if (field.type == "list<vec3>")
                {
                    field.type = "List<Vec3>";
                }


                m_fieldList.Add(field);
            }
        }

        public void SaveToCsFile(string filePath, Encoding encoding)
        {
            if (m_fieldList == null)
                throw new Exception("CSDefineGenerator内部数据为空。");

            string defName = Path.GetFileNameWithoutExtension(filePath);

            // 创建配置结构定义代码
            StringBuilder sb = new StringBuilder();

            if (this.ClassComment != null)
                sb.AppendLine(this.ClassComment);

            
            sb.AppendFormat("public class {0} : CfgBase\r\n{{", defName);
            sb.AppendLine();

            foreach (FieldDef field in m_fieldList)
            {
                if (field.name.CompareTo("cid") == 0)
                    continue;

                // 注释掉的列
                if (field.name.StartsWith("#"))
                    continue;
                //int countLen = field.type.Length + field.name.Length;

                //string tabFormatStr = "";
                //int defaultTCount = 9 - (countLen / 4);
                //if ((countLen + 1) % 4 == 0)
                //    defaultTCount--;

                //if (defaultTCount <= 0)
                //    defaultTCount = 1;

                //for (int i = 0; i < defaultTCount; i++)
                //{
                //    tabFormatStr += "\t";
                //}
                sb.AppendFormat("\t/// <summary>\n\t/// {0}\n\t/// </summary>\n", field.comment);
                sb.AppendFormat("\tpublic {0} {1}{{get;set;}}\n", field.type, field.name);
                sb.AppendLine();
            }

            sb.Append("}");
            sb.AppendLine();

            // 保存文件
            using (FileStream file = new FileStream("CDef/ConfigDef.cs", FileMode.Append, FileAccess.Write))
            {
                using (TextWriter writer = new StreamWriter(file, encoding))
                    writer.WriteLine(sb.ToString());

                file.Close();
            }
        }

        static int SaveToCsMgrFileCallCount = 0;
        public void SaveToCsMgrFile(string filePath, Encoding encoding)
        {
            string defName = Path.GetFileNameWithoutExtension(filePath);

            // 写入ConfigMgr
            string configMgrFilePath = "CDef/CfgMgr.cs";
            byte[] buf = File.ReadAllBytes(configMgrFilePath);
            string oldFileText = Encoding.UTF8.GetString(buf);




            string TagCfgMapDefine = "////////////////////////////////////CfgTools Map Define///////////////////////////////////////////////////";
            if (SaveToCsMgrFileCallCount == 0)
                Program.ReplaceFileCtrlText(ref oldFileText, "\n", TagCfgMapDefine);

            StringBuilder sbCfgMapDefine = new StringBuilder();
            sbCfgMapDefine.AppendLine(TagCfgMapDefine);
            sbCfgMapDefine.AppendLine(Program.CutOutStr(oldFileText, TagCfgMapDefine, TagCfgMapDefine).Replace("\r\n\r\n", ""));
            //sbCfgMapDefine.AppendFormat("\tpublic static Dictionary<int, CfgBase> {0}Map;\n", defName);
            sbCfgMapDefine.AppendFormat("\tpublic static CfgMap<{0}> {1}Map = new CfgMap<{2}>();\n", defName, defName, defName);
            sbCfgMapDefine.AppendLine(TagCfgMapDefine);
            string strCfgMapDefine = sbCfgMapDefine.ToString();
            //strCfgMapDefine.Replace("\n\n", "\n");
            Program.ReplaceFileCtrlText(ref oldFileText, strCfgMapDefine, TagCfgMapDefine);


            string TagCfgMapLoad = "////////////////////////////////////CfgTools Map Load///////////////////////////////////////////////////";
            if (SaveToCsMgrFileCallCount == 0)
                Program.ReplaceFileCtrlText(ref oldFileText, "\n", TagCfgMapLoad);
            StringBuilder sbCfgMapLoad = new StringBuilder();
            sbCfgMapLoad.AppendLine(TagCfgMapLoad);
            sbCfgMapLoad.AppendLine(Program.CutOutStr(oldFileText, TagCfgMapLoad, TagCfgMapLoad).Replace("\r\n\r\n", ""));
            //sbCfgMapLoad.AppendFormat("\t\t{0}Map = LoadCfg<{1}>();\n", defName, defName);
            sbCfgMapLoad.AppendFormat("\t\t{0}Map.LoadCfg();\n", defName);
            sbCfgMapLoad.AppendFormat("\t\tAllCfgs.Add(typeof({0}), {1}Map);\n", defName, defName);
            sbCfgMapLoad.AppendLine(TagCfgMapLoad);
            string strCfgMapLoad = sbCfgMapLoad.ToString();
            Program.ReplaceFileCtrlText(ref oldFileText, strCfgMapLoad, TagCfgMapLoad);

            //string TagCfgMapGet = "////////////////////////////////////CfgTools Map Get///////////////////////////////////////////////////";
            //if (SaveToCsMgrFileCallCount == 0)
            //    Program.ReplaceFileCtrlText(ref oldFileText, "\n", TagCfgMapGet);
            //StringBuilder sbCfgMapGet = new StringBuilder();
            //sbCfgMapGet.AppendLine(TagCfgMapGet);
            //sbCfgMapGet.AppendLine(Program.CutOutStr(oldFileText, TagCfgMapGet, TagCfgMapGet).Replace("\r\n\r\n\r\n", ""));
            //sbCfgMapGet.AppendFormat("\tpublic static {0} Get{1}(int _cid, bool _isShowErr = true)\n", defName, defName);
            //sbCfgMapGet.AppendLine("\t{");
            //sbCfgMapGet.AppendFormat("\t\treturn Get<{0}>(_cid, {1}Map, _isShowErr);\n", defName, defName);
            //sbCfgMapGet.AppendLine("\t}");
            //sbCfgMapGet.AppendLine(TagCfgMapGet);
            //string strCfgMapGet = sbCfgMapGet.ToString();
            //Program.ReplaceFileCtrlText(ref oldFileText, strCfgMapGet, TagCfgMapGet);

            File.WriteAllBytes(configMgrFilePath, Encoding.UTF8.GetBytes(oldFileText));

            SaveToCsMgrFileCallCount++;
        }

    }
}
