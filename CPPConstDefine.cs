using excel2json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel2Json3
{
    /// <summary>
    /// 常量表导出常量宏 快速访问
    /// </summary>
    internal class CPPConstDefine
    {
        //List<FieldDef> m_fieldList;


        public CPPConstDefine(DataTable sheet, Encoding cd)
        {
            // First Row as Column Name
            if (sheet.Rows.Count < 4)
                return;

            // 创建代码字符串
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// Export by 1-常量表.xlsx.ConstCfg");
            sb.AppendLine("\n\n");


            DataRow typeRow = sheet.Rows[2];
            //FieldDef field;

            for(int i = 4; i < sheet.Rows.Count; i++)
            {
                DataRow row = sheet.Rows[i];
                string ID = row[sheet.Columns[0]].ToString();
                if (ID.Length <= 0)
                    continue;

                string constKey = row[sheet.Columns[1]].ToString();
                if(string.IsNullOrEmpty(constKey))
                    continue;

                if (!string.IsNullOrEmpty( row[sheet.Columns[2]].ToString().Trim()))
                {
                    int val;
                    if(!int.TryParse(row[sheet.Columns[2]].ToString(), out val))
                    {
                        continue;
                    }

                    // 注释
                    sb.AppendLine(string.Format("// {0}", row[sheet.Columns[5]].ToString()));
                    // 宏
                    sb.AppendLine(string.Format("#define CONST_VAL_{0} {1}\n", constKey, val));

                    continue;
                }

                if (!string.IsNullOrEmpty(row[sheet.Columns[4]].ToString().Trim()))
                {
                    string val = row[sheet.Columns[4]].ToString().Trim();

                    // 注释
                    sb.AppendLine(string.Format("// {0}", row[sheet.Columns[5]].ToString()));
                    // 宏
                    sb.AppendLine(string.Format("#define CONST_VAL_{0} \"{1}\"\n", constKey, val));

                    continue;
                }

            }

            // 保存文件
            string exportFilePath = "SDef/CfgMgrExConst.h";
            using (FileStream file = new FileStream(exportFilePath, FileMode.Create, FileAccess.Write))
            {
                using (TextWriter writer = new StreamWriter(file, cd))
                    writer.WriteLine(sb.ToString());

                file.Close();
            }
        }

        public void Export()
        {
           

        }
    }
}
