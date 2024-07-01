using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Excel2Json3;


//using Excel;
using ExcelDataReader;
//using System.Threading.Tasks;


namespace excel2json
{
    sealed partial class Program
    {
        static void Main(string[] args)
        {
			ExportExcel();
            //Console.ReadLine();
        }
		
		 /// <summary>
        /// 根据命令行参数，执行Excel数据导出工作
        /// </summary>
        /// <param name="_excelFileName">Excel文件名</param>
        private static void ExportJsonAndDef(string _excelFileName)
        {
            string filePath = Path.GetFileName(_excelFileName);
            if (filePath.Contains("~"))
            {
                Console.WriteLine("忽略隐藏文件:" + filePath);
                return;
            }

            if (filePath.Contains("#"))
            {
                Console.WriteLine("忽略手动注释的文件:" + filePath);
                return;
            }


            string[] spliter = { "." };
            string[] getStrs = filePath.Split(spliter, 2, StringSplitOptions.None);
            if (getStrs[1] != "xlsx")
            {
                throw new Exception(filePath + " 不是xlsx格式, 拒绝导出.");
            }

            int header = 4;

            // 加载Excel文件
            using (FileStream excelFile = File.Open(_excelFileName, FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(excelFile);

                // The result of each spreadsheet will be created in the result.Tables
                //excelReader.IsFirstRowAsColumnNames = true;
                DataSet book = excelReader.AsDataSet();

                // 数据检测
                if (book.Tables.Count <= 0)
                {
                    throw new Exception("Excel中没有找到Sheet: " + _excelFileName);
                }

                // 取得数据
                DataTable sheet = book.Tables[0];
                string sheetName = sheet.TableName;
                if (sheet.Rows.Count <= header)
                {
                    throw new Exception("Excel Sheet中没有数据: " + _excelFileName + ", 或表头格式错误. 至少4行");
                }

                // 确定编码
                Encoding cd = new UTF8Encoding(false);
                string excelName = Path.GetFileName(_excelFileName);

                // 导出客户端JSON文件
                string outClientPath = "CJson/" + sheetName + ".json";
                ExporterClientJson JsonExporterC = new ExporterClientJson(sheet, header, false);
                JsonExporterC.SaveToClientFile(outClientPath, cd);

                // 生成C#结构体定义文件
                CSDefineGenerator CsExporter = new CSDefineGenerator(sheet);
                CsExporter.ClassComment = string.Format("/// <summary>\n/// {0}.{1}\n/// </summary>", excelName,  sheetName);
                CsExporter.SaveToCsMgrFile(outClientPath, cd);
                CsExporter.SaveToCsFile(outClientPath, cd);

                //// 生成客户端C++结构体定义文件 虚幻
                //CPPURDefineGenerator HExporterC = new CPPURDefineGenerator(sheet);
                //HExporterC.ClassComment = string.Format("// Define From {0}\n// {1}配置", excelName, excelName.Replace(".xlsx", ""));
                //HExporterC.SaveToDef(outClientPath, cd);
                //HExporterC.SaveToDefCPP(outClientPath, cd);

                // 生成服务器JSON文件
                string outServerPath = "SJson/" + sheetName + ".json";
                ExporterServerJson JsonServerExporter = new ExporterServerJson(sheet, header, false);
                JsonServerExporter.SaveToServerFile(outServerPath, cd);

                // 生成服务器C++结构体定义文件
                CPPDefineGenerator HExporterS = new CPPDefineGenerator(sheet);
                //HExporterS.ClassComment = string.Format("// Define From {0}\n// {1}配置", excelName, excelName.Replace(".xlsx", ""));
                HExporterS.ClassComment = string.Format("/// <summary>\n/// {0}.{1}\n/// </summary>", excelName, sheetName);
                HExporterS.SaveToDef(outClientPath, cd);
                HExporterS.SaveToDefCPP(outClientPath, cd);
                HExporterS.SaveToMgrFile(outClientPath, cd);

                // 如果是常量表 单独处理 生成常量宏 提升效率
                if(sheetName.CompareTo("ConstCfg") == 0)
                {
                    CPPConstDefine ConstExport = new CPPConstDefine(sheet, cd);
                }

            }
        }


        // 清空CS文件
        private static void CleanCsFile()
        {
            // 保存文件
            // 确定编码
            Encoding encoding = new UTF8Encoding(false);

            // 清空CS文件
            using (FileStream file = new FileStream("CDef/ConfigDef.cs", FileMode.Create, FileAccess.Write))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("using System;\n");
                sb.AppendFormat("using System.Collections.Generic;\n");
                sb.AppendFormat("using UnityEngine;\n");

                sb.AppendLine();
                using (TextWriter writer = new StreamWriter(file, encoding))
                    writer.WriteLine(sb.ToString());
                file.Close();
            }
        }

        // 清空H文件
        private static void CleanServerHFile()
        {
            // 保存文件
            // 确定编码
            Encoding encoding = new UTF8Encoding(false);

            //// 清空客户端头文件
            //using (FileStream file = new FileStream("CDef/ConfigDef.h", FileMode.Create, FileAccess.Write))
            //{
            //    StringBuilder sb = new StringBuilder();
            //    sb.AppendFormat("// 配置结构体 由配置工具导出\n");
            //    sb.AppendLine("#pragma once");
            //    sb.AppendLine();
            //    sb.AppendLine("#include \"CoreMinimal.h\"");
            //    sb.AppendLine("#include \"GameFramework/Actor.h\"");
            //    sb.AppendLine("#include \"ConfigData.generated.h\"");

            //    sb.AppendLine();
            //    using (TextWriter writer = new StreamWriter(file, encoding))
            //        writer.WriteLine(sb.ToString());

            //    file.Close();
            //}

            // 清空服务器头文件
            using (FileStream file = new FileStream("SDef/ConfigDef.h", FileMode.Create, FileAccess.Write))
            {
                StringBuilder sb = new StringBuilder();       
                sb.AppendFormat("// 配置结构体\n// 此文件由配置工具导出\n\n");
                sb.AppendLine("#pragma once");
                sb.AppendLine("#include <vector>");
                sb.AppendLine("#include <string>");
                sb.AppendLine("#include \"ConfigBase.h\"\n");
                sb.AppendFormat("using namespace std;\n\n");

                sb.AppendLine();
                using (TextWriter writer = new StreamWriter(file, encoding))
                    writer.WriteLine(sb.ToString());

                file.Close();
            }
        }

        // 清空服务器Cpp文件
        private static void CleanServerCPPFile()
        {
            Encoding encoding = new UTF8Encoding(false);

            // 清空服务器头文件
            using (FileStream file = new FileStream("SDef/ConfigDef.cpp", FileMode.Create, FileAccess.Write))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("// 配置结构体从Json解析\n// 此文件由配置工具导出\n\n");
                sb.AppendLine("#include \"ConfigDef.h\"");
                //sb.AppendLine("#include \"../../Framework/CJsonObject.hpp\"");
                sb.AppendLine();
                using (TextWriter writer = new StreamWriter(file, encoding))
                    writer.WriteLine(sb.ToString());

                file.Close();
            }
        }

        // 清空H文件
        private static void CleanClientHFile()
        {
            // 保存文件
            // 确定编码
            Encoding encoding = new UTF8Encoding(false);

            // 清空客户端头文件
            using (FileStream file = new FileStream("CDef/ConfigDef.h", FileMode.Create, FileAccess.Write))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("// 配置结构体 由配置工具导出\n");
                sb.AppendLine("#pragma once");
                sb.AppendLine();
                sb.AppendLine("#include \"CoreMinimal.h\"");
                sb.AppendLine("#include \"GameFramework/Actor.h\"");
                sb.AppendLine("#include \"ConfigData.generated.h\"");

                sb.AppendLine();
                using (TextWriter writer = new StreamWriter(file, encoding))
                    writer.WriteLine(sb.ToString());

                file.Close();
            }

        }

        private static void CleanClientCPPFile()
        {
            // 保存文件
            // 确定编码
            Encoding encoding = new UTF8Encoding(false);

            // 清空客户端头文件
            using (FileStream file = new FileStream("CDef/ConfigDef.cpp", FileMode.Create, FileAccess.Write))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("// 配置结构体 由配置工具导出\n");       
                sb.AppendLine("#include \"ConfigDef.h\"");
                sb.AppendLine();
                sb.AppendLine("#define SAFE_DELETE(p) {if(p){delete(p); (p)=nullptr;}}");
                sb.AppendLine("#define SAFE_DELETE_ARY(p) { if(p){delete[](p); (p)=nullptr;}}");
                sb.AppendLine();
                using (TextWriter writer = new StreamWriter(file, encoding))
                    writer.WriteLine(sb.ToString());

                file.Close();
            }
        }

        private static void ExportExcel()
        {
            // 清空结构体文件
            Console.WriteLine("export excel begin");

            // 清空旧配置结构体文件
            CleanCsFile();
            CleanServerHFile();
            CleanServerCPPFile();
            //CleanClientCPPFile();

            string path = "Excel";
            int count = 0;
            var xlsFiles = Directory.GetFiles(path, "*.xlsx");
            Console.WriteLine("导出Excel开始, 共{0}个文件", xlsFiles.Length);

            foreach (var xlsFile in xlsFiles)
            {
                System.DateTime startTime = System.DateTime.Now;

                // 导出CS, C++, Json文件
                Console.WriteLine("导出开始{0}", xlsFile);
                ExportJsonAndDef(xlsFile);
                count++;

                // 程序计时
                System.DateTime endTime = System.DateTime.Now;
                System.TimeSpan dur = endTime - startTime;
                Console.WriteLine(string.Format("导出完成{0}, [耗时{1}毫秒].\n", xlsFile, dur.Milliseconds));
            }
            Console.WriteLine("导出Excel完成, 共{0}个文件", count);

            Console.WriteLine("export excel finish");
        }

        public static string ReplaceFileCtrlText(ref string _oldText, string _newText, string _tag)
        {
            string oldStr = CutOutStr(_oldText, _tag, _tag);
            string newStr = CutOutStr(_newText, _tag, _tag);

            bool isEnmptyOld = string.IsNullOrEmpty(oldStr.Trim());
            bool isEnmptyNew = string.IsNullOrEmpty(newStr.Trim());

            // 两个都是空的 不需要替换
            if (isEnmptyOld && isEnmptyNew)
                return _oldText;

            // 两个都不为空,直接替换
            if (!isEnmptyOld && !isEnmptyNew)
            {
                _oldText = _oldText.Replace(oldStr, newStr);
                return _oldText;
            }

            // 其中有一个为空
            oldStr = string.Format("{0}{1}{2}", _tag, oldStr, _tag);
            newStr = string.Format("{0}{1}{2}", _tag, newStr, _tag);
            _oldText = _oldText.Replace(oldStr, newStr);
            return _oldText;
        }

        public static string CutOutStr(string _res, string _startTag, string _endTag)
        {
            if (string.IsNullOrEmpty(_startTag))
            {
                Console.WriteLine("[Error] string.IsNullOrEmpty(_startTag)");
                return string.Empty;
            }

            if (string.IsNullOrEmpty(_endTag))
            {
                Console.WriteLine("[Error] string.IsNullOrEmpty(_endTag)");
                return string.Empty;
            }

            Regex rg = new Regex("(?<=(" + _startTag + "))[.\\s\\S]*?(?=(" + _endTag + "))"); ;
            return rg.Match(_res).Value;
        }
    }
}
