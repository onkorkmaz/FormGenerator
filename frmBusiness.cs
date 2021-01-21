using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.IO;

namespace FormGenerator
{
    public partial class frmBusiness : Form
    {
        string sqlCon = "";
        StringBuilder sb;
        string classAdi;
        public frmBusiness()
        {
            InitializeComponent();
            sqlCon = AppConstant.GetConnectionString + " Initial Catalog =" + AppConstant.DatabaseName;
        }

        private void frmBusiness_Load(object sender, EventArgs e)
        {
            sb = new StringBuilder();

            
            classAdi = AppConstant.TableName.Substring(2) + "Business";

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Text;");
            sb.AppendLine("using Entity;");
            sb.AppendLine("using System.Data.SqlClient;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using Common;");

            sb.AppendLine("namespace Business");
            sb.AppendLine("{");
            {
                sb.AppendLine("public class " + classAdi + "{");

                fillBusinessInUpDel(AppConstant.InUpDelSpName);
                fillBusinessSel(AppConstant.SelSpName);

                sb.AppendLine("}");
            }
            sb.AppendLine("}");

            txtBusiness.Text = sb.ToString();

        }

        private void fillBusinessInUpDel(string procedureName)
        {
            try
            {
                SqlConnection con = new SqlConnection(sqlCon);
                con.Open();
                SqlCommand cmd = new SqlCommand(procedureName, con);


                cmd.CommandType = CommandType.StoredProcedure;

                SqlCommandBuilder.DeriveParameters(cmd);


                //ex = new eExecProc("@temsilciref", System.Data.DbType.String, x.Temsilci_ref);
                //lexec.Add(ex);

                cmd.CommandType = CommandType.StoredProcedure;

                bool ilk = true;
                int sayac = 0;

                AppConstant.InUpDelMethodName = procedureName.Substring(0, 1).ToUpper() + "" + procedureName.Substring(1);


                sb.AppendLine("public DataResultArgs<bool> " + AppConstant.InUpDelMethodName + "(" + AppConstant.EntityName + " entity)");
                sb.AppendLine("{");
                {

                    sb.AppendLine("SqlCommand cmd = new SqlCommand();");
                    sb.AppendLine("cmd.CommandType = CommandType.StoredProcedure;");
                    ilk = true;
                    sayac = 0;

                    sb.AppendLine();

                    foreach (SqlParameter p in cmd.Parameters)
                    {
                        sayac++;
                        if (ilk)
                            ilk = false;
                        else
                        {
                            string name = p.ParameterName.Substring(1);
                            name = name.Substring(0, 1).ToUpper() + "" + name.Substring(1);
                            sb.AppendLine("cmd.Parameters.AddWithValue(\"" + p.ParameterName + "\", entity." + name.Replace("İ", "I") + ");");
                        }
                    }
                    sb.AppendLine();

                    sb.AppendLine("DataResultArgs<bool> resultSet = DataProcess.ExecuteProc(cmd, \"" + procedureName + "\"); ");
                    sb.AppendLine("return resultSet ;");
                }
                sb.AppendLine("}");

                sb.AppendLine();
               
                txtBusiness.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void fillBusinessSel(string procedureName)
        {
            try
            {
                SqlConnection con = new SqlConnection(sqlCon);
                con.Open();
                SqlCommand cmd = new SqlCommand(procedureName, con);


                cmd.CommandType = CommandType.StoredProcedure;

                SqlCommandBuilder.DeriveParameters(cmd);


                //ex = new eExecProc("@temsilciref", System.Data.DbType.String, x.Temsilci_ref);
                //lexec.Add(ex);

                cmd.CommandType = CommandType.StoredProcedure;

                bool ilk = true;
                int sayac = 0;

                

                AppConstant.SelectMethodName = procedureName.Substring(0, 1).ToUpper() + "" + procedureName.Substring(1);

                string code = AppConstant.CodeBehind;

                code = code.Substring(0, code.IndexOf("EklenecekGet")) + "" + AppConstant.SelectMethodName + "" + code.Substring(code.IndexOf("EklenecekGet") + 12);
                code = code.Substring(0, code.IndexOf("EklenecekSet")) + "" + AppConstant.InUpDelMethodName + "" + code.Substring(code.IndexOf("EklenecekSet") + 12);

                code = replaceEntityName(code);
                code = replaceEntityName(code);
                code = replaceEntityName(code);
                code = replaceEntityName(code);
                code = replaceEntityName(code);
                code = replaceEntityName(code);
                code = replaceEntityName(code);
                code = replaceEntityName(code);
                code = replaceEntityName(code);

                AppConstant.CodeBehind = code;

                //MessageBox.Show(code);

                sb.AppendLine("public DataResultArgs<List<" + AppConstant.EntityName + ">> " + AppConstant.SelectMethodName + "(SearchEntity entity)");
                sb.AppendLine("{");
                sb.AppendLine("DataResultArgs<List<" + AppConstant.EntityName + ">> resultSet = new DataResultArgs<List<" + AppConstant.EntityName + ">>();");
                sb.AppendLine("SqlCommand cmd = new SqlCommand();");
                sb.AppendLine("cmd.CommandType = CommandType.StoredProcedure;");
                ilk = true;
                sayac = 0;

                sb.AppendLine();

                foreach (SqlParameter p in cmd.Parameters)
                {
                    sayac++;
                    if (ilk)
                        ilk = false;
                    else
                    {
                        string name = p.ParameterName.Substring(1);
                        name = name.Substring(0, 1).ToUpper() + "" + name.Substring(1);
                        sb.AppendLine("cmd.Parameters.AddWithValue(\"" + p.ParameterName + "\", entity." + name.Replace("İ", "I") + ");");
                    }
                }
                sb.AppendLine();

                sb.AppendLine("DataResultArgs<SqlDataReader> result= DataProcess.ExecuteProcDataReader(cmd, \"" + procedureName + "\"); ");
                sb.AppendLine("if (result.HasError)");
                sb.AppendLine("{");

                sb.AppendLine("resultSet.HasError = result.HasError;");
                sb.AppendLine("resultSet.ErrorDescription = result.ErrorDescription;");
                sb.AppendLine("resultSet.ErrorCode = result.ErrorCode;");
                sb.AppendLine("}");
                sb.AppendLine("else");
                sb.AppendLine("{");
                sb.AppendLine(" SqlDataReader dr = result.Result;");
                sb.AppendLine(AppConstant.FillEntitiyCode);
                sb.AppendLine("dr.Close();");
                sb.AppendLine("resultSet.Result = lst;");
                sb.AppendLine("}");
                sb.AppendLine(" return resultSet;");
                sb.AppendLine("}");

                sb.AppendLine();

                txtBusiness.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private string replaceEntityName(string code)
        {
            code = code.Substring(0, code.IndexOf("Entities")) + "" + AppConstant.EntityName + "" + code.Substring(code.IndexOf("Entities") + 8);
            return code;
        }

        private void txtResult_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                txtBusiness.SelectAll();
            }
        }

        private string getReplaceLowerTrCharToEng(string name)
        {
            return name.Replace("ı", "i").Replace("ç", "o").Replace("ü", "u").Replace("ş", "s").Replace("ğ", "g");
        }

        private string getReplaceUpperTrCharToEng(string name)
        {
            return name.Replace("İ", "I").Replace("Ç", "C").Replace("Ü", "U").Replace("Ş", "S").Replace("Ğ", "G");
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control)
            {
                txtBusiness.SelectAll();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sv = new SaveFileDialog();
            sv.FileName = classAdi + ".cs";
            DialogResult result = sv.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {

                //MessageBox.Show(sv.FileName.ToString());
                StreamWriter sw = new StreamWriter(sv.FileName.ToString(), false, System.Text.Encoding.Unicode);
                sw.Write(txtBusiness.Text);
                sw.Flush();
                sw.Close();
            }

            frmAspxCode frm = new frmAspxCode();
            frm.HtmlCode = AppConstant.HtmlCode;
            frm.CodeBehind = AppConstant.CodeBehind;
            frm.Show();
        }
    }
}
