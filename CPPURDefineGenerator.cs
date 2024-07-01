using System;
using System.IO;
using System.Data;
using System.Text;
using System.Collections.Generic;

namespace excel2json
{
    /// <summary>
    /// 根据表头，生成C#类定义数据结构
    /// 表头使用四行定义：注释、字段名称、字段类型、目标平台
    /// </summary>
    class CPPURDefineGenerator
    {
        struct FieldDef
        {
            public string key;
            public string type;
            public string comment;
        }

        List<FieldDef> m_fieldList;

        public String ClassComment
        {
            get;
            set;
        }

        public CPPURDefineGenerator(DataTable sheet)
        {
            // First Row as Column Name
            if (sheet.Rows.Count < 2)
                return;

            m_fieldList = new List<FieldDef>();
           
            DataRow KeyRow = sheet.Rows[0];
            DataRow typeRow = sheet.Rows[1];
            DataRow dataRow = sheet.Rows[3];

            //Console.WriteLine("call CPPDefineGenerator Texxxxxxxxxxxxxxx");
            int row = 0;
            foreach (DataColumn column in sheet.Columns)
            {
                row++;
                FieldDef field;
                field.key = KeyRow[column].ToString().Trim();
                field.comment = column.ToString().Trim(); 
                field.type = typeRow[column].ToString().Trim();
                string data = dataRow[column].ToString().Trim();

                if (string.IsNullOrEmpty(field.key.Trim()))
                    continue;

                if (string.IsNullOrEmpty(field.comment.Trim()))
                    continue;


                if (field.type == "int")
                {
                    field.type = "int32";
                }
                else if (field.type == "string")
                {
                    field.type = "FString";
                }

                // YU_TODO: 根据数据内容 确定真实的类型 *: Vec只能是数字
                else if (field.type == "vec2")
                {
                    field.type = "FIntVector2";
                    //string str = sampleData.Substring(1, sampleData.Length - 2).Trim();
                    if (!string.IsNullOrEmpty(data.Trim()))// && !sampleData.Equals("{}"))
                    {
                        string[] strAry = data.Trim().Split(',');
                        int count = strAry.Length;
                        if (count != 2)
                        {
                            Console.WriteLine("[Error] vec2 数据过长. 行列[" + row.ToString() + "," + field.comment + "]. target:" + data + ", count:" + count);
                            //continue;
                        }
                    }
                }
                else if (field.type == "vec3")
                {
                    field.type = "FIntVector3";
                    if (!string.IsNullOrEmpty(data.Trim()))// && !sampleData.Equals("{}"))
                    {
                        string[] strAry = data.Split(',');
                        int count = strAry.Length;
                        if (count != 3)
                        {
                            Console.WriteLine("[Error] vec3 数据过长. 行列[" + row.ToString() + "," + field.comment + "]. target:" + data);
                            //continue; 
                        }
                        //else
                        //{
                        //    sampleRow[column] = string.Format("{{{0}}}");
                        //}
                    }
                }
                else if (field.type == "list")
                {// list 最多支持3层嵌套 
                    field.type = "TArray<int32>";
                }
                else if (field.type == "list<string>")
                {
                    field.type = "TArray<FString>";
                }
                else if (field.type == "list<list>")
                {
                    field.type = "TArray<TArray<int32>>";
                }
                else if (field.type == "list<vec2>")
                {
                    field.type = "TArray<FIntVector2>";
                }
                else if (field.type == "list<vec3>")
                {
                    field.type = "TArray<FIntVector3>";
                }


                m_fieldList.Add(field);
            }
        }


        public void SaveToDef(string filePath, Encoding encoding)
        {
            if (m_fieldList == null)
                throw new Exception("CPPURDefineGenerator内部数据为空。");

            string defName = Path.GetFileNameWithoutExtension(filePath);

            // 创建代码字符串
            StringBuilder sb = new StringBuilder();

            if (this.ClassComment != null)
                sb.AppendLine(this.ClassComment);
            sb.AppendLine("USTRUCT(BlueprintType)");
            sb.AppendFormat("struct {0}\r\n{{\n", defName);
            sb.AppendLine("\tGENERATED_BODY()");
            sb.AppendLine();
            sb.AppendLine("\tstatic void InitConfig();");
            sb.AppendLine("\tstatic void ClearConfig();");
            sb.AppendFormat("\tstatic {0}* FindConfig(const uint32 id);\n", defName);
            sb.AppendLine();

            foreach (FieldDef field in m_fieldList)
            {
                // 连注释都没有的为休止符
                if (string.IsNullOrEmpty(field.comment.Trim()))
                    break;

                if (string.IsNullOrEmpty(field.key))
                    continue;

                // 注释掉的列
                if (field.key.StartsWith("#"))
                    continue;

                string fieldType = field.type;
                if (string.IsNullOrEmpty(fieldType.Trim()))
                    continue;

                sb.AppendFormat("\tUPROPERTY(EditAnywhere, BlueprintReadWrite)\n\t//! {0}\n\t{1} {2};\n", field.comment, fieldType, field.key);
                sb.AppendLine();
            }

            sb.Append("};\r\n");
            sb.AppendLine();

            // 保存文件
            string exportFilePath = "CDef/ConfigDef.h";

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
                throw new Exception("CPPURDefineGenerator内部数据为空。");

            string defName = Path.GetFileNameWithoutExtension(filePath);

            // 创建代码字符串
            StringBuilder sb = new StringBuilder();

            //if (this.ClassComment != null)
            //    sb.AppendLine(this.ClassComment);
            sb.AppendLine("");
            sb.AppendLine("//! 最大配置表数量");
            string hongStr = string.Format("MAX_COUNT_{0}", defName.ToUpper());
            string aryStr = string.Format("__{0}Ary", defName);
            sb.AppendFormat("#define {0} {1}\n", hongStr, m_fieldList.Count + 100);
            sb.AppendFormat("{0}* {1}[{2}] = {{nullptr}}\n", defName, aryStr, hongStr);
            sb.AppendLine();

            sb.AppendFormat("void {0}::InitConfig()\n", defName);
            sb.AppendLine("{");

            string strFunc1 = @"
    // 配置表路径";
            sb.AppendLine(strFunc1);
            sb.AppendFormat("\tFString file_path = FPaths::ProjectContentDir() + TEXT(\"cfg/{0}.json\");", defName);

            string strFunc2 = @"
    // json管理器
    TArray<TSharedPtr<FJsonValue>> json_array;
    // json字符串 
    FString json_contatiner;
    int32 count = 0;
    // 从文件中加载内容到字符串中
    if (FFileHelper::LoadFileToString(json_contatiner, *file_path))
    {
        // 创建json解析对象
        TSharedRef<TJsonReader<TCHAR>> json_reader = TJsonReaderFactory < TCHAR >::Create(json_contatiner);
        // 解析json字符串
        if (FJsonSerializer::Deserialize(json_reader, json_array))
        {
            // 遍历节点
            for (auto & a : json_array)
            { ";
            sb.AppendLine(strFunc2);

            sb.AppendFormat("\t\t\t\t{0}* data = new {1};", defName, defName);
            string strFunc3 = @"
                bool ret = FJsonObjectConverter::JsonObjectToUStruct(a->AsObject().ToSharedRef(), data, 1, 0);
                if (!ret)
                {
                    delete data;
                    continue;
                }";
            sb.AppendLine(strFunc3);


            sb.AppendFormat("\t\t\t\tif (data->id >= {0})", hongStr);
            string strFunc5 = @"
                {
                    delete data;
                    continue;
                }
                count++;
";
            sb.AppendLine(strFunc5);

            sb.AppendFormat("\t\t\t{0}[data->id] = data;", aryStr);
            
            string strFunc4 = @"
            }
        }
    }
    UE_LOG(LogTemp, Log, TEXT(""load[%s] size[%d]""), *file_path, count);";
            sb.AppendLine(strFunc4);
            sb.AppendLine("}\n");

            sb.AppendFormat("void {0}::ClearConfig()\n", defName);
            sb.AppendLine("{");
            sb.AppendFormat("\tfor(auto& it: {0})\n", aryStr);
            sb.AppendLine("\t{"); 
            sb.AppendLine("\t\tSAFE_DELETE(it);");
            sb.AppendLine("\t}");
            sb.AppendFormat("\tUE_LOG(LogTemp, Log, TEXT(\"clear {0}! \"));\n", defName);
            sb.AppendLine("}\n");

            sb.AppendFormat("{0}* {1}::FindConfig(const uint32 id)\n", defName, defName);
            sb.AppendLine("{");
            sb.AppendFormat("\tif(id >= {0}) return nullptr;\n", defName);
            sb.AppendFormat("\treturn {0}[id];\n", aryStr);
            sb.AppendLine("}\n");


            // 保存文件
            string exportFilePath = "CDef/ConfigDef.cpp";

            using (FileStream file = new FileStream(exportFilePath, FileMode.Append, FileAccess.Write))
            {
                using (TextWriter writer = new StreamWriter(file, encoding))
                    writer.WriteLine(sb.ToString());

                file.Close();
            }
        }
    }
}
