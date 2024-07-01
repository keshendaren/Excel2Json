using System;
using System.IO;
using System.Data;
using System.Text;
using System.Collections.Generic;

namespace excel2json
{
    /// <summary>
    /// 根据表头，生成C#类定义数据结构
    /// 表头使用三行定义：字段名称、字段类型、注释
    /// </summary>
    class CPPDefineGenerator
    {
        //struct FieldDef
        //{
        //    public string key;
        //    public string type;
        //    public string comment;
        //}

        List<FieldDef> m_fieldList;

        public String ClassComment
        {
            get;
            set;
        }

        public CPPDefineGenerator(DataTable sheet)
        {
            // First Row as Column Name
            if (sheet.Rows.Count < 5)
                return;

            m_fieldList = new List<FieldDef>();
            DataRow commentRow = sheet.Rows[0];
            DataRow KeyRow = sheet.Rows[1];
            DataRow typeRow = sheet.Rows[2];
            DataRow PlatformRow = sheet.Rows[3];

            int row = 0;
            FieldDef field;
            foreach (DataColumn column in sheet.Columns)
            {
                row++;
                
                field.name = KeyRow[column].ToString().Trim(); 
                field.comment = commentRow[column].ToString().Trim();
                field.type = typeRow[column].ToString().Trim();
                string platform = PlatformRow[column].ToString().Trim();

                if (string.IsNullOrEmpty(field.name.Trim()))
                    continue;

                if (string.IsNullOrEmpty(field.comment.Trim()))
                    continue;

                if (string.IsNullOrEmpty(platform))
                    continue;

                if (platform.CompareTo("N") == 0
                    || platform.CompareTo("C") == 0)
                    continue;


                if (field.type == "int")
                {
                    field.type = "int32";
                }
                else if (field.type == "vec2")
                {
                    field.type = "Vec2";                   
                }
                else if(field.type == "vec3")
                {
                    field.type = "Vec3";                   
                }
                else if (field.type == "list")
                {// list 最多支持3层嵌套 
                    field.type = "vector<int>";
                }
                else if (field.type == "list<string>")
                {
                    field.type = "vector<string>";
                }
                else if(field.type == "list<list>")
                {
                    field.type = "vector<vector<int>>";
                }
                else if (field.type == "list<vec2>")
                {
                    field.type = "vector<Vec2>";
                }
                else if (field.type == "list<vec3>")
                {
                    field.type = "vector<Vec3>";
                }

                m_fieldList.Add(field);
            }
        }

  
        public void SaveToDef(string filePath, Encoding encoding, bool _isServer = true)
        {
            if (m_fieldList == null)
                throw new Exception("CSDefineGenerator内部数据为空。");

            string defName = Path.GetFileNameWithoutExtension(filePath);

            // 创建代码字符串
            StringBuilder sb = new StringBuilder();

            if (this.ClassComment != null)
                sb.AppendLine(this.ClassComment);

            sb.AppendFormat("struct {0} : CfgBase\r\n{{\n", defName);
            sb.AppendFormat("\t{0}(int _cid) :CfgBase(_cid) {{}}\n", defName);
            sb.AppendLine("\tvirtual int Parse(json& _jObj);");
            sb.AppendLine();

            foreach (FieldDef field in m_fieldList)
            {
                // 连注释都没有的为休止符
                if (string.IsNullOrEmpty(field.comment.Trim()))
                    break;

                if (string.IsNullOrEmpty(field.name))
                    continue;

                if (field.name.CompareTo("cid") == 0)
                    continue;

                // 注释掉的列
                if (field.name.StartsWith("#"))
                    continue;

                string fieldType = field.type;
                if (string.IsNullOrEmpty(fieldType.Trim()))
                    continue;

                //fieldType = fieldType.Replace("List<", "vector<").Replace("vec2", "Vec2").Replace("vec3", "Vec3");

                //if (field.type.Contains("List<"))
                //    sb.AppendFormat("\t{0} {1};\t\t\t// {2}", field.type.Replace("List<", "vector<"), field.name, field.comment);
                //else

                sb.AppendFormat("\t/// <summary>\n\t/// {0}\n\t/// </summary>\n", field.comment);
                sb.AppendFormat("\t{0} {1};\n", fieldType, field.name);
                sb.AppendLine();
            }

            sb.Append("};\r\n");
            sb.AppendLine();

            // 保存文件
            string exportFilePath = "SDef/ConfigDef.h";
            if (!_isServer)
                exportFilePath = "CDef/ConfigDef.h";

            using (FileStream file = new FileStream(exportFilePath, FileMode.Append, FileAccess.Write))
            {
                using (TextWriter writer = new StreamWriter(file, encoding))
                    writer.WriteLine(sb.ToString());

                file.Close();
            }
        }

        public void SaveToDefCPP(string filePath, Encoding encoding)
        {
            if (m_fieldList == null)
                throw new Exception("CPPDefineGenerator内部数据为空。");

            string defName = Path.GetFileNameWithoutExtension(filePath);

            // 创建代码字符串
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("\nint {0}::Parse(json& _jObj)", defName);
            sb.AppendLine("{");
            sb.AppendLine("\tif (_jObj.is_null())");
            sb.AppendLine("\t\treturn -1;\n");
            sb.AppendLine("\ttry {");

            foreach (FieldDef field in m_fieldList)
            {
                // 连注释都没有的为休止符
                if (string.IsNullOrEmpty(field.comment.Trim()))
                    break;

                if (string.IsNullOrEmpty(field.name))
                    continue;

                if (field.name.CompareTo("cid") == 0)
                    continue;

                // 注释掉的列
                if (field.name.StartsWith("#"))
                    continue;

                string fieldType = field.type;
                if (string.IsNullOrEmpty(fieldType.Trim()))
                    continue;

                //sb.AppendFormat("\tif (!_jObj.Get(\"{0}\", {1})) {{ LogErr(\"!_jObj.Get({2})\"); return -1; }}\n", field.name, field.name, field.name);
                sb.AppendFormat("\t\t{0} = _jObj[\"{1}\"].get<{2}>();\n", field.name, field.name, fieldType);
            }

            sb.AppendLine("\t}");
            sb.AppendLine("\tcatch (const std::exception& e) {");
            sb.AppendLine("\t\tLogErr(\"FuncCfg::Parse, cid:%d, e:%s\", cid, e.what());");
            sb.AppendLine("\t\treturn -2;");
            sb.AppendLine("\t}");

            sb.AppendLine("\n\treturn 0;");
            sb.AppendLine("}");
            sb.AppendLine();

            // 保存文件
            string exportFilePath = "SDef/ConfigDef.cpp";

            using (FileStream file = new FileStream(exportFilePath, FileMode.Append, FileAccess.Write))
            {
                using (TextWriter writer = new StreamWriter(file, encoding))
                    writer.WriteLine(sb.ToString());

                file.Close();
            }

        }


        static int SaveToCsMgrFileCallCount = 0;
        public void SaveToMgrFile(string filePath, Encoding encoding)
        {
            string defName = Path.GetFileNameWithoutExtension(filePath);

            // 写入ConfigMgr
            string configMgrFilePath = "SDef/CfgMgr.h";
            byte[] buf = File.ReadAllBytes(configMgrFilePath);
            string oldFileText = Encoding.UTF8.GetString(buf);




            string TagCfgMapDefine = "////////////////////////////////////CfgTools Map Define///////////////////////////////////////////////////";
            if (SaveToCsMgrFileCallCount == 0)
                Program.ReplaceFileCtrlText(ref oldFileText, "\n", TagCfgMapDefine);

            StringBuilder sbCfgMapDefine = new StringBuilder();
            sbCfgMapDefine.AppendLine(TagCfgMapDefine);
            sbCfgMapDefine.AppendLine(Program.CutOutStr(oldFileText, TagCfgMapDefine, TagCfgMapDefine).Replace("\r\n\r\n", ""));
            sbCfgMapDefine.AppendFormat("\tCfgMap<{0}> m{1}s;\n", defName, defName);
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
            sbCfgMapLoad.AppendFormat("\t\tm{0}s.LoadCfg(\"{1}\");\n", defName, defName);
            sbCfgMapLoad.AppendLine(TagCfgMapLoad);
            string strCfgMapLoad = sbCfgMapLoad.ToString();
            Program.ReplaceFileCtrlText(ref oldFileText, strCfgMapLoad, TagCfgMapLoad);


            File.WriteAllBytes(configMgrFilePath, Encoding.UTF8.GetBytes(oldFileText));

            SaveToCsMgrFileCallCount++;
        }



    }
}
