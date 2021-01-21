using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace FormGenerator
{
    public partial class frmEntities : Form
    {
        string classAdi;
        bool ilk = true;
        bool iki = true;
        string textCopy, text, query;
        List<x> lst = new List<x>();
        SqlCommand cmd = new SqlCommand();
        public frmEntities()
        {
            InitializeComponent();
        }

        private void frmEntities_Load(object sender, EventArgs e)
        {
            try
            {
                string serverName = AppConstant.ServerName;
                SqlConnection con = new SqlConnection(AppConstant.GetConnectionString+" Initial Catalog="+AppConstant.DatabaseName+";");
                con.Open();
                ilk = true;
                iki = true;
                lst.Clear();
                query = "";

                if (AppConstant.IsSp)
                {
                    SqlCommand cmd = new SqlCommand(AppConstant.SelSpName, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlCommandBuilder.DeriveParameters(cmd);
                    query = "exec  " + AppConstant.SelSpName;

                    foreach (SqlParameter p in cmd.Parameters)
                    {
                        if (ilk)
                            ilk = false;
                        else if (iki)
                        {
                            query += " null";
                            iki = false;
                        }
                        else
                        {
                            query += ",null";
                        }
                    }
                }
                else
                {
                    query = AppConstant.SelSpName;
                }

                SqlCommand cmd2 = new SqlCommand(query, con);
                cmd2.ExecuteNonQuery();
                SqlDataAdapter sadp = new SqlDataAdapter(cmd2);
                DataTable dt = new DataTable();
                sadp.Fill(dt);

                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.DataType == typeof(Int16) || dc.DataType == typeof(Int32) || dc.DataType == typeof(Int64) || dc.DataType == typeof(Decimal) || dc.DataType == typeof(Double))
                    {

                        lst.Add(new x(dc.ColumnName, dc.ColumnName, dc.DataType, true));
                    }
                    else if (dc.DataType == typeof(DateTime))
                    {
                        lst.Add(new x(dc.ColumnName, dc.ColumnName, dc.DataType, false, true));
                    }

                    else if (dc.DataType == typeof(Single))
                    {
                        lst.Add(new x(dc.ColumnName, dc.ColumnName, dc.DataType, false, false, true));
                    }
                    else
                    {
                        lst.Add(new x(dc.ColumnName, dc.ColumnName, dc.DataType));
                    }
                }

                classAdi = AppConstant.TableName.Substring(2) + "Entity";
                AppConstant.EntityName = classAdi;

                StringBuilder s = new StringBuilder();

                s.AppendLine("using System;");
                s.AppendLine("using System.Collections.Generic;");
                s.AppendLine("using System.Linq;");
                s.AppendLine("using System.Text;");

                s.AppendLine("namespace Entity");
                s.AppendLine("{");

                {

                    s.AppendLine("public class " + classAdi + " : BaseEntity{");

                    //s.AppendLine("private DatabaseProcess databaseProcess;");
                    //s.AppendLine("public DatabaseProcess DatabaseProcess");
                    //s.AppendLine("{");
                    //s.AppendLine("get { return databaseProcess; }");
                    //s.AppendLine("set");
                    //s.AppendLine("{");
                    //s.AppendLine("databaseProcess = value;");
                    //s.AppendLine("}");
                    //s.AppendLine("}");

                    foreach (x x1 in lst)
                    {
                        if (getLower(x1.Isim.ToLower()) == "id" || getLower(x1.Isim.ToLower()) == "addedon" || getLower(x1.Isim.ToLower()) == "updatedon" || getLower(x1.Isim.ToLower()) == "deletedon" || getLower(x1.Isim.ToLower()) == "isactive" || getLower(x1.Isim.ToLower()) == "isdeleted")
                        {
                        }
                        else
                        {
                            text = x1.Isim;
                            //s.AppendLine(" [DataMember]");
                            s.AppendLine("private " + findType(x1.Type) + " " + getLower(x1.Isim.ToLower()) + ";");

                            s.AppendLine("public " + findType(x1.Type) + " " + getUpper(x1.Isim[0].ToString().ToUpper()) + "" + x1.Isim.Substring(1));
                            s.AppendLine("{");
                            s.AppendLine("get { return " + getLower(x1.Isim.ToLower()) + "; }");
                            s.AppendLine("set { " + getLower(x1.Isim.ToLower()) + " = value; ");
                            s.AppendLine("}");
                            s.AppendLine("}");
                        }
                    }

                    s.AppendLine("}");
                }
                s.AppendLine("}");
                s.AppendLine();

                StringBuilder ss = new StringBuilder();


                ss.AppendLine("List<" + classAdi + "> lst = new List<" + classAdi + ">();");
                ss.AppendLine(classAdi + " elist;");
                ss.AppendLine("while(dr.Read())");
                ss.AppendLine("{");
                bool newClass = true;
                foreach (x x1 in lst)
                {
                    if (newClass)
                        ss.AppendLine("elist = new " + classAdi + "();");

                    newClass = false;

                    text = x1.Isim;

                    //textCopy = getFirstCharToUpper(text);
                    textCopy = x1.Isim;
                    if (x1.Isim == "isDeleted" ||x1.Isim == "isDeleted" || x1.Isim == "addedOn" || x1.Isim == "updatedOn" || x1.Isim == "deletedOn")
                    {
                    }
                    else
                    {
                        ss.AppendLine("elist." + getUpper(textCopy[0].ToString().ToUpper()) + "" + textCopy.Substring(1) + " =   GeneralFunctions.GetData<" + findType(x1.Type) + ">(dr[\"" + x1.Isim + "\"]);");
                    }
                }
                ss.AppendLine("lst.Add(elist);");
                ss.AppendLine("}");

                ss.AppendLine();

                txtEntities.Text = s.ToString();
                AppConstant.FillEntitiyCode = ss.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private string getLower(string p)
        {
            return p.Replace("ı", "i");
        }

        private string getUpper(string p)
        {
            if (p == "İ")
                return "I";

            else
                return p;
        }

        private string findTypeDecoraiton(Type type)
        {
            if (type == typeof(Int16))
                return "Integer";
            else if (type == typeof(Int32))
                return "Integer";
            else if (type == typeof(Double))
                return "Decimal3Dig";
            else if (type == typeof(Decimal))
                return "Decimal3Dig";
            else if (type == typeof(DateTime))
                return "DateTime";
            else if (type == typeof(Byte))
                return "Byte";
            else if (type == typeof(String))
                return "String";
            else if (type == typeof(Single))
            {
                return "Single";
            }
            else
            {
                //MessageBox.Show(type.ToString());
            }

            return "String";
        }

        private string findType(Type type)
        {
            string objectType =type.ToString().Replace("System."," ").Trim();
            if (objectType == "String")
                return objectType;
            else
            {
                return objectType + "?";
            }
        }

        private void txtEntities_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control)
            {
                txtEntities.SelectAll();
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
               sw.Write(txtEntities.Text);
               sw.Flush();
               sw.Close();
              
            }

            frmBusiness frm = new frmBusiness();
            frm.Show();
        }

    }

    public class x
    {

        public x(string isim, string header, bool right)
        {
            Isim = isim;
            Header = header;
            Right = right;
        }

        public x(string isim, string header, bool right, bool format)
        {
            Isim = isim;
            Header = header;
            Right = right;
            Format = format;
        }

        public x(string isim, bool right)
        {
            Isim = isim;
            Right = right;
        }

        public x(string p, string p_2, Type type)
        {
            // TODO: Complete member initialization
            this.Isim = p;
            this.Header = p_2;
            this.Type = type;
        }

        public x(string p, string p_2, Type type, bool right)
        {
            // TODO: Complete member initialization
            this.Isim = p;
            this.Header = p_2;
            this.Type = type;
            Right = right;
        }

        public x(string p, string p_2, Type type, bool right, bool date)
        {
            // TODO: Complete member initialization
            this.Isim = p;
            this.Header = p_2;
            this.Type = type;
            Right = right;
            Date = date;
        }

        public x(string p, string p_2, Type type, bool right, bool date, bool oran)
        {
            // TODO: Complete member initialization
            this.Isim = p;
            this.Header = p_2;
            this.Type = type;
            Right = right;
            Date = date;
            Oran = oran;
        }

        public string Isim { get; set; }
        public string Header { get; set; }
        public bool Right { get; set; }
        public bool Format { get; set; }
        public Type Type { get; set; }
        public bool Date { get; set; }
        public bool Oran { get; set; }
    }
}
