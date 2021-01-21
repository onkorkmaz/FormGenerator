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

namespace FormGenerator
{
    public partial class frmStoreProcedure : Form
    {
        bool ony, ony2;
        string id = "";
        StringBuilder s;
        SqlDataReader dr;
        SqlConnection con;
        SqlCommand cmd;
        string sqlCon = "";
        public frmStoreProcedure()
        {
            InitializeComponent();
            sqlCon = AppConstant.GetConnectionString + " Initial Catalog =" + AppConstant.DatabaseName;
        }

        private void StoreProcedure_Load(object sender, EventArgs e)
        {
            try
            {
                s = new StringBuilder();
                con = new SqlConnection(sqlCon);

                s.Clear();
                con.Open();
                cmd = new SqlCommand(@"SELECT COLUMN_NAME, DATA_TYPE,CHARACTER_OCTET_LENGTH,IS_NULLABLE
                                    FROM INFORMATION_SCHEMA.COLUMNS
                                    WHERE TABLE_NAME ='" + AppConstant.TableName + "'", con);

                dr = cmd.ExecuteReader();

                #region insert update delete
                s.AppendLine("-- =============================================");
                s.AppendLine("-- Author:       <Onur KORKMAZ>");
                s.AppendLine("-- Create date:   <" + DateTime.Now.ToString() + ">");
                s.AppendLine("-- Description:   <Description,,>");
                s.AppendLine("-- =============================================");
                string screenName = AppConstant.TableName.Substring(2);
                AppConstant.InUpDelSpName = "set_" + screenName;
                s.AppendLine("IF EXISTS(SELECT * FROM sys.objects WHERE type = 'P' AND name = 'set_" + screenName + "')");
                s.AppendLine("DROP PROCEDURE set_" + screenName);
                s.AppendLine("GO");

                s.AppendLine("CREATE PROCEDURE dbo.set_" + screenName);


                ony = true;
                s.AppendLine("@DatabaseProcess             varchar(1),");
                //s.AppendLine("@sw_approve_id                 int = 0, ");
                while (dr.Read())
                {

                    if (ony)
                    {
                        id = dr[0].ToString();
                        ony = false;
                    }

                    if (dr[0].ToString() == "isDeleted" || dr[0].ToString() == "addedOn" || dr[0].ToString() == "updatedOn" || dr[0].ToString() == "deletedOn")
                    {
                    }
                    else
                    {

                        s.Append("@" + dr[0].ToString());
                        for (int i = 0; i < 30 - dr[0].ToString().Length; i++)
                        {
                            s.Append(" ");
                        }
                        s.Append(dr[1].ToString());
                        if (dr[2] != null && dr[2].ToString() != "")
                            s.Append("(" + dr[2].ToString() + ")");

                        if (dr[3].ToString() == "YES")
                            s.Append(" = NULL");

                        s.AppendLine(",");

                    }
                }
                dr.Close();

                s.Remove(s.Length - 3, 1);
                s.AppendLine("AS");
                s.AppendLine();
                s.AppendLine("  set @isActive= isnull(@isActive,1)");
                s.AppendLine();
                s.AppendLine("declare @rowcount int,@err int,@trancount int,@tranid int  ");
                s.AppendLine("select @trancount = @@trancount ");
                s.AppendLine("if @@trancount =0  begin transaction ");
                s.AppendLine();
                s.AppendLine("if @DatabaseProcess = '0'  ");
                s.AppendLine("    begin /*--i n s e r t--*/  ");
                s.AppendLine("      insert into " + AppConstant.TableName);
                dr = cmd.ExecuteReader();
                ony = true;
                ony2 = true;
                while (dr.Read())
                {
                    if (dr[0].ToString() == "isDeleted" || dr[0].ToString() == "addedOn" || dr[0].ToString() == "updatedOn" || dr[0].ToString() == "deletedOn")
                    {
                    }
                    else
                    {

                        if (ony)
                        {
                            ony = false;
                        }
                        else
                        {
                            if (ony2)
                            {
                                s.AppendLine("            (" + dr[0].ToString() + ",");
                                ony2 = false;
                            }
                            else
                            {
                                s.AppendLine("             " + dr[0].ToString() + ",");
                            }
                        }
                    }
                }
                s.AppendLine("            isDeleted,");
                s.AppendLine("            addedOn )");



                dr.Close();

                dr = cmd.ExecuteReader();
                ony = true;
                ony2 = true;
                while (dr.Read())
                {
                    if (dr[0].ToString() == "isDeleted" || dr[0].ToString() == "addedOn" || dr[0].ToString() == "updatedOn" || dr[0].ToString() == "deletedOn")
                    {
                    }
                    else
                    {

                        if (ony)
                        {
                            ony = false;
                        }
                        else
                        {
                            if (ony2)
                            {
                                s.AppendLine("      VALUES(");
                                s.AppendLine("             @" + dr[0].ToString() + ",");
                                ony2 = false;
                            }
                            else
                            {
                                s.AppendLine("             @" + dr[0].ToString() + ",");
                            }
                        }
                    }
                }
                s.AppendLine("             0,"); 
                s.AppendLine("             getdate() )");

                dr.Close();

                s.AppendLine();
                s.AppendLine("   select @rowcount = @@rowcount, @err = @@error ");
                s.AppendLine("   if @rowcount <> 1 or @err <> 0  ");
                s.AppendLine("       begin  ");
                s.AppendLine("       if @trancount = 0 rollback tran  ");
                s.AppendLine("          --raiserror 60766 '" + AppConstant.TableName + " RowCount Error!(+set_" + screenName + ")' ");
                s.AppendLine("          return @err");
                s.AppendLine("       end ");

                s.AppendLine("SELECT @tranid = SCOPE_IDENTITY()  ");
                s.AppendLine("   end ");

                s.AppendLine();
                s.AppendLine("else if @DatabaseProcess = '1'   /*--u p d a t e--*/");
                s.AppendLine("     begin  ");
                s.AppendLine("set @tranid = @id");
                s.AppendLine("     update " + AppConstant.TableName);
                dr = cmd.ExecuteReader();
                ony = true;
                ony2 = true;
                while (dr.Read())
                {
                    if (dr[0].ToString() == "isDeleted" || dr[0].ToString() == "addedOn" || dr[0].ToString() == "updatedOn" || dr[0].ToString() == "deletedOn")
                    {
                    }
                    else
                    {

                        if (ony)
                        {
                            ony = false;
                        }
                        else
                        {
                            if (ony2)
                            {
                                //set email= ISNULL(@email,email) , 
                                s.AppendLine("      set " + dr[0].ToString() + "= ISNULL(@" + dr[0].ToString() + " ," + dr[0].ToString() + "),");
                                ony2 = false;
                            }
                            else
                            {
                                s.AppendLine("             " + dr[0].ToString() + "= ISNULL(@" + dr[0].ToString() + " ," + dr[0].ToString() + "),");
                            }
                        }
                    }
                }

                s.AppendLine("             updatedOn = getdate() ");

                dr.Close();


                s.AppendLine("             where " + id + " = @" + id + "  ");
                s.AppendLine("   select @rowcount = @@rowcount , @err = @@error  if @rowcount <> 1 or @err <> 0   begin  ");
                s.AppendLine("   if @trancount = 0 rollback tran  ");
                s.AppendLine("     -- raiserror 60766 '" + AppConstant.TableName + " RowCount Error!(set_" + screenName + ")' ");
                s.AppendLine("      return @err ");
                s.AppendLine("   end");
                s.AppendLine(" end");

                s.AppendLine();
                s.AppendLine(" else if @DatabaseProcess = '2' /*--d e l e t e--*/ ");
                s.AppendLine("      begin  ");
                s.AppendLine("set @tranid = @id  ");

                s.AppendLine("            update " + AppConstant.TableName + " set isDeleted = 1, deletedOn=getdate()  where " + id + " = @" + id + "  ");

                s.AppendLine("        select @rowcount = @@rowcount , @err = @@error  if @rowcount <> 1 or @err <> 0   begin  ");
                s.AppendLine("        if @trancount = 0 rollback tran   ");
                s.AppendLine("           --raiserror 60766 '" + AppConstant.TableName + " RowCount Error!(set_" + screenName + ")' ");
                s.AppendLine("           return @err   ");
                s.AppendLine("        end  ");





                s.AppendLine("end");
                s.AppendLine("if @trancount = 0 commit tran  ");
                s.AppendLine("select @tranid");
                s.AppendLine("GO");
                

                dr.Close();
                string temp = s.ToString().Replace("text(2147483647) ", "varchar(max)");
                txtInUpDel.Text = temp;

                #endregion

                #region select

                dr = cmd.ExecuteReader();
                s.Clear();
                s.AppendLine("-- =============================================");
                s.AppendLine("-- Author:       <Onur KORKMAZ>");
                s.AppendLine("-- Create date:   <" + DateTime.Now.ToString() + ">");
                s.AppendLine("-- Description:   <Description,,>");
                s.AppendLine("-- =============================================");
                s.AppendLine("IF EXISTS(SELECT * FROM sys.objects WHERE type = 'P' AND name = 'get_" + screenName + "')");
                s.AppendLine("DROP PROCEDURE get_" + screenName);
                s.AppendLine("GO");

                AppConstant.SelSpName = "get_" + screenName;
                s.AppendLine("CREATE PROCEDURE dbo.get_" + screenName);


                s.AppendLine("@" + id + "  int = null,");
                s.AppendLine("@isActive bit = null,");
                s.AppendLine("@isDeleted bit = null");
                s.AppendLine("as");

                //s.AppendLine
                s.AppendLine("select ");
                string ilk = screenName.Substring(0, 1).ToLower();
                while (dr.Read())
                {
                    s.AppendLine("     " + ilk + "." + dr[0].ToString() + ",");

                }
                s.Remove(s.Length - 3, 1);
                s.AppendLine();
                s.AppendLine("from " + AppConstant.TableName + " " + ilk + "(nolock)");
                s.AppendLine("where (" + ilk + "." + id + " = @" + id + " or @" + id + " is null) and (" + ilk + ".isActive = @isActive or @isActive is null) and (" + ilk + ".isDeleted = @isDeleted or @isDeleted is null)");
                s.AppendLine("GO");

                txtSelect.Text = s.ToString();
                dr.Close();

                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        private void btnStoreProceudre_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(sqlCon);

            string[] strCmd = Regex.Split(txtInUpDel.Text, "GO");
            con.Open();
            SqlCommand cmd;

            foreach (string s in strCmd)
            {
                string temp = s.Replace("text(2147483647) ", "varchar(max)");

                cmd = new SqlCommand(temp, con);
                cmd.ExecuteNonQuery();
            }

            strCmd = Regex.Split(txtSelect.Text, "GO");
            foreach (string s in strCmd)
            {
                string temp = s.Replace("text(2147483647) ", "varchar(max)");
                cmd = new SqlCommand(temp, con);
                cmd.ExecuteNonQuery();
            }
            con.Close();
            //MessageBox.Show("Database aktarım tamamlandı");
            frmEntities frm = new frmEntities();
            frm.Show();
        }

        private void txtInUpDel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                txtInUpDel.SelectAll();
            }
        }

        private void txtSelect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                txtSelect.SelectAll();
            }
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            frmEntities frm = new frmEntities();
            frm.Show();
        }
    }
}
