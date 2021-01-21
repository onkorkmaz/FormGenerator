using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace FormGenerator
{
    public partial class frmMain : Form
    {
        #region VARIABLES

        public string sqlConDatabase;
        DataTable dt;
        string databaseName = "";
        string tableName = "";
        StringBuilder sbHtml = new StringBuilder();
        StringBuilder sbCodeBehing = new StringBuilder();
        int number = 0;

        #endregion

        #region CONSTRUCTOR && PAGE_LOAD

        public frmMain()
        {
            InitializeComponent();
            fillTreeViewDatabase();
            fillTreeViewDataBaseTable();
        }

        #endregion

        #region METHODS

        private string getUpper(string p)
        {
            if (p == "İ")
                return "I";

            else
                return p;
        }

        private void fillTreeViewDataBaseTable()
        {
            if (!AppConstant.IsUseRemoteServer)
            {

                foreach (TreeNode tn in treeView1.Nodes[0].Nodes)
                {
                    DataTable tables = new DataTable();
                    sqlConDatabase = AppConstant.GetConnectionString + " Initial Catalog =" + tn.Text;
                    using (SqlConnection con = new SqlConnection(sqlConDatabase))
                    {
                        con.Open();
                        SqlCommand command = new SqlCommand(@"Select table_name as Name from
                    INFORMATION_SCHEMA.Tables where TABLE_TYPE =
                    'BASE TABLE' order by Name", con);

                        SqlDataAdapter sadp = new SqlDataAdapter(command);
                        sadp.Fill(tables);

                        foreach (DataRow drw in tables.Rows)
                        {
                            tn.Nodes.Add(drw[0].ToString());
                        }
                        //tn.ExpandAll();
                        con.Close();
                    }
                }
            }
            else
            {
                DataTable tables = new DataTable();
                sqlConDatabase = AppConstant.GetConnectionString + " Initial Catalog =" + AppConstant.DatabaseName;
                using (SqlConnection con = new SqlConnection(sqlConDatabase))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand(@"Select table_name as Name from INFORMATION_SCHEMA.Tables where TABLE_TYPE ='BASE TABLE' order by Name", con);

                    SqlDataAdapter sadp = new SqlDataAdapter(command);
                    sadp.Fill(tables);

                    foreach (DataRow drw in tables.Rows)
                    {
                        treeView1.Nodes[0].Nodes[0].Nodes.Add(drw[0].ToString());
                    }
                    treeView1.ExpandAll();
                    con.Close();
                }
            }
        }
        private void fillTreeViewDatabase()
        {
            if (!AppConstant.IsUseRemoteServer)
            {
                using (SqlConnection con = new SqlConnection(AppConstant.GetConnectionString))
                {
                    con.Open();
                    dt = con.GetSchema("Databases");
                    cmbDataBase.Items.Add("Database Seçin...");
                    foreach (DataRow drw in dt.Rows)
                    {
                        treeView1.Nodes[0].Nodes.Add(drw[0].ToString());
                        cmbDataBase.Items.Add(drw[0].ToString());
                    }
                    treeView1.Nodes[0].Expand();
                    con.Close();

                }
            }
            else
            {
                cmbDataBase.Items.Add(AppConstant.DatabaseName);
                treeView1.Nodes[0].Nodes.Add(AppConstant.DatabaseName);
            }
        }

        private string getAspxCode(String type, String name, bool isEnabled,string placeHolder)
        {
            string result = "";
            // cmb.Items.Add("TextBox");
            //cmb.Items.Add("ListBox");
            //cmb.Items.Add("ComboBox");
            //cmb.Items.Add("CheckBox");
            //cmb.Items.Add("RadioButton");
            //cmb.Items.Add("PictureBox");
            //cmb.Items.Add("DateTime");
            switch (type)
            {
                case "TextBox":
                    result = "<asp:TextBox ID=\"txt_" + name + "\" runat=\"server\" placeholder=\""+placeHolder+"\" class=\"form-control\"></asp:TextBox>";
                    //result = "<asp:TextBox ID= Enabled=\""+isEnabled+"\" runat=\"server\"></asp:TextBox>";
                    break;
                case "ListBox":
                    result = "<asp:ListBox ID=\"lst_" + name + "\" Enabled=\"" + isEnabled + "\"  runat=\"server\"></asp:ListBox>";
                    break;
                case "ComboBox":
                    result = " <dx:ASPxComboBox ValueField=\"\" TextField=\"\"  ID=\"cmb_" + name + "\" Enabled=\"" + isEnabled + "\"  runat=\"server\"></dx:ASPxComboBox>";
                    //result = "<asp:DropDownList ID=\"drp_" + name + "\" Enabled=\"" + isEnabled + "\"  runat=\"server\"></asp:DropDownList>";
                    break;
                case "RadioButton":
                    result = "<asp:RadioButton ID=\"rdb_" + name + "\" Enabled=\"" + isEnabled + "\"  runat=\"server\"></asp:RadioButton>";
                    break;
                case "PictureBox":
                    result = "";
                    break;
                case "CheckBox":
                    result = "<dx:ASPxCheckBox ID=\"chc_" + name + "\" Enabled=\"" + isEnabled + "\"  runat=\"server\"></dx:ASPxCheckBox>";
                    //result = "<asp:CheckBox ID=\"chc_" + name + "\" Enabled=\"" + isEnabled + "\"  runat=\"server\"></asp:CheckBox>";
                    break;
                case "DateTime":
                    result = "<dx:ASPxDateEdit ID=\"dt_" + name + "\" Enabled=\"" + isEnabled + "\" runat=\"server\"></dx:ASPxDateEdit>";
                    //result = "<asp:Calendar ID=\"clndr_" + name + "\" Enabled=\"" + isEnabled + "\"  runat=\"server\"></asp:Calendar>";
                    break;
                default:
                    break;
            }
            return result;
        }

        private ComboBox fillTypeOfControls(Type type)
        {
            ComboBox cmb = new ComboBox();
            cmb.Name = "cmbControlType";
            cmb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb.Items.Add("TextBox");
            cmb.Items.Add("CheckBox");
            cmb.Items.Add("DateTime");

            switch (type.Name.ToString().Substring(0, 2))
            {
                case "Da":
                  cmb.SelectedIndex = 2;
                    break;

                case "Bo":
                    cmb.SelectedIndex = 1;
                    break;
                case "In":
                    cmb.SelectedIndex = 0;
                    break;

                default:
                    cmb.SelectedIndex = 0;
                    break;
            }

            return cmb;
        }

        private ComboBox fillTableColumns(FlowLayoutPanel flwpnl, string name)
        {
            ComboBox cmb = new ComboBox();
            string text = ((ComboBox)(flwpnl.Controls.Find(name, false)).First()).Text;
            sqlConDatabase = AppConstant.GetConnectionString + " Initial Catalog =" + treeView1.SelectedNode.Parent.Text;
            using (SqlConnection con = new SqlConnection(sqlConDatabase))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM " + text, con))
                {
                    dt = new DataTable();
                    SqlDataAdapter sadp = new SqlDataAdapter(cmd);
                    sadp.Fill(dt);


                    foreach (DataColumn dc in dt.Columns)
                    {
                        cmb.Items.Add(dc.ColumnName.ToString());
                    }
                }
            }
            return cmb;
        }
        private ComboBox fillTable()
        {
            ComboBox cmb = new ComboBox();
            cmb.AutoCompleteMode = AutoCompleteMode.Suggest;

            cmb.AutoCompleteSource = AutoCompleteSource.ListItems;
            foreach (TreeNode tn in treeView1.SelectedNode.Parent.Nodes)
            {
                cmb.Items.Add(tn.Text);
            }
            cmb.SelectedText = treeView1.SelectedNode.Text;

            return cmb;
        }
        private ComboBox fillTable(string name)
        {
            ComboBox cmb = new ComboBox();
            cmb.Name = name;
            cmb.AutoCompleteMode = AutoCompleteMode.Suggest;

            cmb.AutoCompleteSource = AutoCompleteSource.ListItems;
            foreach (TreeNode tn in treeView1.SelectedNode.Parent.Nodes)
            {
                cmb.Items.Add(tn.Text);
            }
            cmb.SelectedText = treeView1.SelectedNode.Text;

            return cmb;
        }

        #endregion

        #region EVENTS

        private void button1_Click(object sender, EventArgs e)
        {
            if (rdbSp.Checked && string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("You must to double click one table");
                return;
            }
            else if (rdbQuery.Checked)
            {
                if (cmbDataBase.SelectedIndex == 0)
                {
                    MessageBox.Show("You must select database");
                    cmbDataBase.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtQuery.Text))
                {

                    MessageBox.Show("You must to write query");
                    txtQuery.Focus();
                    return;
                }
            }

            if (rdbSp.Checked)
            {

                AppConstant.DatabaseName = databaseName;
                AppConstant.TableName = tableName;

                #region Html

                sbHtml.Clear();
                sbHtml.AppendLine("<section class=\"vbox\"> ");
                {
                    sbHtml.AppendLine("<header class=\"header bg-dark bg-gradient\"> ");
                    {
                        sbHtml.AppendLine("<ul class=\"nav nav-tabs\"> ");
                        sbHtml.AppendLine("<li class=\"active\">");
                        sbHtml.AppendLine("<a href=\"#basic\" data-toggle=\"tab\">Profilim</a>");
                        sbHtml.AppendLine("</li> ");

                        sbHtml.AppendLine("</ul> ");
                    }
                    sbHtml.AppendLine("</header> ");

                    sbHtml.AppendLine("<div class=\"tab-content\">");
                    {
                        sbHtml.AppendLine("<section class=\"tab-pane active\" id=\"basic\">");
                        {
                            sbHtml.AppendLine("<form class=\"panel-body\" runat=\"server\" id=\"form1\"> ");
                            {
                                sbHtml.AppendLine(" <asp:TextBox ID=\"txt_id\" runat=\"server\" Visible=\"false\"></asp:TextBox>");
                                number = 0;
                                foreach (Control cc in pnlMain.Controls)
                                {
                                    number++;

                                    foreach (Control c in pnlMain.Controls)
                                    {
                                        if (number.ToString() == ((TextBox)c.Controls[7]).Text)
                                        {

                                            if (((CheckBox)c.Controls[2]).Checked)
                                                sbHtml.AppendLine("<div class=\"form-group\">");
                                            else
                                                sbHtml.Append("<div class=\"form-group\" style=\"display:none;\">");

                                            if (((TextBox)c.Controls[0]).Text != "id" || ((TextBox)c.Controls[0]).Text != "isActive")
                                            {
                                                sbHtml.AppendLine(getAspxCode(((ComboBox)c.Controls[1]).Text, ((ComboBox)c.Controls[1]).Tag.ToString(), ((CheckBox)c.Controls[3]).Checked, ((TextBox)c.Controls[0]).Text));
                                            }


                                            sbHtml.AppendLine("</div> ");
                                        }
                                    }
                                }

                                sbHtml.AppendLine("<div class=\"checkbox\">");
                                sbHtml.AppendLine("Active <asp:CheckBox ID=\"chcActive\" runat=\"server\" />");

                                sbHtml.AppendLine("</div>");
                                sbHtml.AppendLine("<div class=\"form-group\"> ");
                                sbHtml.AppendLine("<%-- <button type=\"submit\" class=\"btn btn-info\">Giriş</button>  --%>");
                                sbHtml.AppendLine("<asp:Button ID=\"btnKaydet\" runat=\"server\" Text=\"Kaydet\" class=\"btn btn-info\" ");
                                sbHtml.AppendLine("onclick=\"btnKaydet_Click\"></asp:Button>");
                                sbHtml.AppendLine("</div>");
                                sbHtml.AppendLine("<div class=\"form-group\" runat=\"server\" id=\"divError\" visible=\"false\">");
                                sbHtml.AppendLine("<div class=\"alert alert-danger\"> ");
                                sbHtml.AppendLine("<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><i class=\"icon-remove\"></i>");
                                sbHtml.AppendLine("</button> ");
                                sbHtml.AppendLine("<i class=\"icon-ban-circle\"></i>");
                                sbHtml.AppendLine("<strong>Hata!</strong> ");
                                sbHtml.AppendLine("<a href=\"#\" class=\"alert-link\"></a><asp:Label ID=\"lblError\" runat=\"server\" Text=\"Label\"></asp:Label>");
                                sbHtml.AppendLine("</div> ");
                                sbHtml.AppendLine("</div>");
                                sbHtml.AppendLine("<div class=\"form-group\" runat=\"server\" id=\"divInformation\" visible=\"false\">");
                                sbHtml.AppendLine("<div class=\"alert alert-info\"> ");
                                sbHtml.AppendLine("<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><i class=\"icon-remove\"></i>");
                                sbHtml.AppendLine("</button> ");
                                sbHtml.AppendLine("<i class=\"icon-info-sign\"></i>");
                                sbHtml.AppendLine("<strong>Bilgi</strong> ");
                                sbHtml.AppendLine("<a href=\"#\" class=\"alert-link\"></a><asp:Label ID=\"lblInformation\" runat=\"server\" Text=\"Label\"></asp:Label>");
                                sbHtml.AppendLine("</div> ");
                                sbHtml.AppendLine("</div>");



                            }
                            sbHtml.AppendLine("</form>");
                        }
                        sbHtml.AppendLine("</section>");
                    }
                }

                


                sbHtml.AppendLine("<dx:ASPxGridView ID=\"ASPxGridView1\" runat=\"server\" align=\"center\" Width=\"800\" Border-BorderWidth=\"2\" EnableRowsCache=\"False\" EnableViewState=\"False\" KeyFieldName=\"Id\" ");
                sbHtml.AppendLine("CssFilePath=\"~/App_Themes/SoftOrange/{0}/styles.css\" CssClass=\"devex_grid_padding\" ");
                sbHtml.AppendLine("CssPostfix=\"SoftOrange\" AutoGenerateColumns=\"False\" Font-Size=\"8pt\">");
                sbHtml.AppendLine("<Settings ShowFilterRow=\"True\" />");
                sbHtml.AppendLine("<Settings ShowGroupPanel=\"True\" />");
                sbHtml.AppendLine("<Settings ShowFilterRow=\"True\" ShowGroupPanel=\"True\"></Settings>");
                sbHtml.AppendLine("<Columns>");

                sbHtml.AppendLine("<dx:GridViewDataColumn Caption=\"Process\" CellStyle-HorizontalAlign=\"Center\">");
                sbHtml.AppendLine("<DataItemTemplate>");

                sbHtml.AppendLine("<table>");
                sbHtml.AppendLine("<tr>");
                sbHtml.AppendLine("<td>");
                sbHtml.AppendLine("<a href=\"~/admin/adminUC.aspx?id=<%#Eval(\"Id\") %>\" target=\"_blank\" border=\"0\"><img border=\"0\" width=\"30\" height=\"30\" src=\"images/update.png\" /></a>");
                sbHtml.AppendLine("<%--<dx:ASPxButton EnableDefaultAppearance=\"False\" EnableViewState=\"False\" Image-Width=\"30\"");
                sbHtml.AppendLine("Image-Height=\"30\" OnCommand=\"btnDelete_Command\" Cursor=\"pointer\" CommandArgument='<%#Eval(\"Id\") %>'");
                sbHtml.AppendLine("CommandName=\"cmmndDelete\" ValidationGroup=\"delete\" ToolTip=\"Delete\" runat=\"server\"  EnableClientSideAPI=\"true\"");
                sbHtml.AppendLine("ID=\"btnDelete\" Image-Url=\"~/admin/images/delete.png\">--%>");
                sbHtml.AppendLine("<ClientSideEvents Click=\"function(s,e) { e.processOnServer = confirm('Are you sure delete current record?'); }\" />");
                sbHtml.AppendLine("</dx:ASPxButton>");
                sbHtml.AppendLine("</td>");
                sbHtml.AppendLine("<td>");
                sbHtml.AppendLine("<dx:ASPxButton EnableDefaultAppearance=\"False\" Image-Width=\"30\" Cursor=\"pointer\" Image-Height=\"30\" ID=\"btnUpdate\"");
                sbHtml.AppendLine("OnCommand=\"btnUpdate_Command\" CommandArgument='<%#Eval(\"Id\") %>' CommandName=\"cmmndUpdate\"");
                sbHtml.AppendLine("AlternateText=\"Update\" runat=\"server\" ValidationGroup=\"update\" Image-Url=\"~/admin/images/update.png\" />");
                sbHtml.AppendLine("</td>");
                sbHtml.AppendLine("</tr>");
                sbHtml.AppendLine("</table>");

                sbHtml.AppendLine("</DataItemTemplate>");
                sbHtml.AppendLine("<CellStyle HorizontalAlign=\"Center\">");
                sbHtml.AppendLine("</CellStyle>");
                sbHtml.AppendLine("</dx:GridViewDataColumn>");

                number = 0;
                foreach (Control cc in pnlMain.Controls)
                {
                    number++;

                    foreach (Control c in pnlMain.Controls)
                    {
                        string text = ((ComboBox)c.Controls[1]).Tag.ToString();

                        if (number.ToString() == ((TextBox)c.Controls[7]).Text)
                        {
                            string result = "";
                            switch (((ComboBox)c.Controls[1]).Text)
                            {
                                case "TextBox":
                                    result = "GridViewDataTextColumn";
                                    break;
                                case "ListBox":
                                    result = "GridViewDataTextColumn";
                                    break;
                                case "ComboBox":
                                    result = "GridViewDataTextColumn";
                                    break;
                                case "RadioButton":
                                    result = "GridViewDataTextColumn";
                                    break;
                                case "PictureBox":
                                    result = "GridViewDataTextColumn";
                                    break;
                                case "CheckBox":
                                    result = "GridViewDataCheckColumn";
                                    break;
                                case "DateTime":
                                    result = "GridViewDataDateColumn";
                                    break;
                                default:
                                    result = "GridViewDataTextColumn";
                                    break;
                            }

                            if (((CheckBox)c.Controls[2]).Checked)
                                sbHtml.AppendLine("<dx:" + result + " Caption=\"" + ((TextBox)c.Controls[0]).Text + "\" FieldName=\"" + getUpper(text.Substring(0, 1).ToUpper().Replace("İ", "I") + "" + text.Substring(1)) + "\"></dx:" + result + ">");
                            else
                                sbHtml.AppendLine("<dx:" + result + " Visible=\"False\" Caption=\"" + ((TextBox)c.Controls[0]).Text + "\" FieldName=\"" + getUpper(text.Substring(0, 1).ToUpper().Replace("İ", "I") + "" + text.Substring(1)) + "\"></dx:" + result + ">");

                        }
                    }
                }
                sbHtml.AppendLine("</Columns>");
                sbHtml.AppendLine("<Settings ShowGroupPanel=\"True\" />");
                sbHtml.AppendLine("<SettingsLoadingPanel ImagePosition=\"Top\" />");
                sbHtml.AppendLine("<Paddings Padding=\"1px\" />");

                sbHtml.AppendLine("<Images SpriteCssFilePath=\"~/App_Themes/SoftOrange/{0}/sprite.css\">");
                sbHtml.AppendLine("<LoadingPanelOnStatusBar Url=\"~/App_Themes/SoftOrange/GridView/gvLoadingOnStatusBar.gif\">");
                sbHtml.AppendLine("</LoadingPanelOnStatusBar>");
                sbHtml.AppendLine("<LoadingPanel Url=\"~/App_Themes/SoftOrange/GridView/Loading.gif\">");
                sbHtml.AppendLine("</LoadingPanel>");
                sbHtml.AppendLine("</Images>");
                sbHtml.AppendLine("<ImagesFilterControl>");
                sbHtml.AppendLine("<LoadingPanel Url=\"~/App_Themes/SoftOrange/GridView/Loading.gif\">");
                sbHtml.AppendLine("</LoadingPanel>");
                sbHtml.AppendLine("</ImagesFilterControl>");
                sbHtml.AppendLine("<Styles CssFilePath=\"~/App_Themes/SoftOrange/{0}/styles.css\"");
                sbHtml.AppendLine(" CssPostfix=\"Office2010Blue\">");
                sbHtml.AppendLine("<Header ImageSpacing=\"5px\" SortingImageSpacing=\"5px\">");
                sbHtml.AppendLine("</Header>");

                sbHtml.AppendLine("<LoadingPanel ImageSpacing=\"5px\">");
                sbHtml.AppendLine("</LoadingPanel>");
                sbHtml.AppendLine("</Styles>");
                sbHtml.AppendLine("<StylesPager>");
                sbHtml.AppendLine("<PageNumber ForeColor=\"#3E4846\">");
                sbHtml.AppendLine("</PageNumber>");
                sbHtml.AppendLine("<Summary ForeColor=\"#1E395B\">");
                sbHtml.AppendLine("</Summary>");
                sbHtml.AppendLine(" </StylesPager>");
                sbHtml.AppendLine("<StylesEditors ButtonEditCellSpacing=\"0\">");
                sbHtml.AppendLine("<ProgressBar Height=\"21px\">");
                sbHtml.AppendLine("</ProgressBar>");
                sbHtml.AppendLine("</StylesEditors>");
                sbHtml.AppendLine("<SettingsText GroupPanel=\"Drag and Drop Column\" />");
                sbHtml.AppendLine("</dx:ASPxGridView>");



                #endregion

                #region CODEBEHIND

                string classAdi = tableName.Substring(2, 1).ToUpper() + "" + tableName.Substring(3).ToLower() + "Business";

                sbCodeBehing.Clear();

                #region VARIABLES

                sbCodeBehing.AppendLine("#region VARIABLES");
                sbCodeBehing.AppendLine("DataResultArgs<List<" + AppConstant.EntityName + "Entities>> resultSet ;");
                sbCodeBehing.AppendLine(classAdi + " business = new " + classAdi + "();");
                sbCodeBehing.AppendLine("List<Entities> lst;");

                sbCodeBehing.AppendLine("#endregion VARIABLES");

                #endregion VARIABLES

                sbCodeBehing.AppendLine("");

                #region PROPERTIES

                sbCodeBehing.AppendLine("#region PROPERTIES");

                sbCodeBehing.AppendLine("private Entities currentRecord;");
                sbCodeBehing.AppendLine("public Entities CurrentRecord");
                {
                    sbCodeBehing.AppendLine("{");
                    sbCodeBehing.AppendLine("set");
                    {
                        sbCodeBehing.AppendLine("{");
                        sbCodeBehing.AppendLine(" currentRecord = value;");
                        {
                            foreach (Control c in pnlMain.Controls)
                            {
                                ComboBox column = ((ComboBox)c.Controls[6]);
                                string type = column.Tag.ToString();

                                switch (((ComboBox)c.Controls[1]).Text)
                                {
                                    case "TextBox":
                                        if (type != "String")
                                            sbCodeBehing.AppendLine("txt_" + ((ComboBox)c.Controls[1]).Tag.ToString() + ".Text= currentRecord." + getUpper(((ComboBox)c.Controls[1]).Tag.ToString().Substring(0, 1).ToUpper()) + "" + ((ComboBox)c.Controls[1]).Tag.ToString().Substring(1) + ".ToString();");
                                        else
                                            sbCodeBehing.AppendLine("txt_" + ((ComboBox)c.Controls[1]).Tag.ToString() + ".Text= currentRecord." + getUpper(((ComboBox)c.Controls[1]).Tag.ToString().Substring(0, 1).ToUpper()) + "" + ((ComboBox)c.Controls[1]).Tag.ToString().Substring(1) + ";");

                                        break;
                                    case "ListBox":
                                        if (type != "String")
                                            MessageBox.Show("yazılacak");
                                        else
                                            MessageBox.Show("yazılacak");
                                        break;
                                    case "ComboBox":
                                        if (type != "String")
                                            sbCodeBehing.AppendLine("cmb_" + ((ComboBox)c.Controls[1]).Tag.ToString() + ".SelectedValue= currentRecord." + getUpper(((ComboBox)c.Controls[1]).Tag.ToString().Substring(0, 1).ToUpper()) + "" + ((ComboBox)c.Controls[1]).Tag.ToString().Substring(1) + ".ToString();");
                                        else
                                            sbCodeBehing.AppendLine("cmb_" + ((ComboBox)c.Controls[1]).Tag.ToString() + ".SelectedValue= currentRecord." + getUpper(((ComboBox)c.Controls[1]).Tag.ToString().Substring(0, 1).ToUpper()) + "" + ((ComboBox)c.Controls[1]).Tag.ToString().Substring(1) + ";");

                                        break;
                                    case "RadioButton":

                                        sbCodeBehing.AppendLine("rdb_" + ((ComboBox)c.Controls[1]).Tag.ToString() + ".Checked = currentRecord." + getUpper(((ComboBox)c.Controls[1]).Tag.ToString().Substring(0, 1).ToUpper()) + "" + ((ComboBox)c.Controls[1]).Tag.ToString().Substring(1) + ";");

                                        break;
                                    case "PictureBox":

                                        break;
                                    case "CheckBox":

                                        sbCodeBehing.AppendLine("chc_" + ((ComboBox)c.Controls[1]).Tag.ToString() + ".Checked = currentRecord." + getUpper(((ComboBox)c.Controls[1]).Tag.ToString().Substring(0, 1).ToUpper()) + "" + ((ComboBox)c.Controls[1]).Tag.ToString().Substring(1) + ";");

                                        break;
                                    case "DateTime":

                                        sbCodeBehing.AppendLine("dt_" + ((ComboBox)c.Controls[1]).Tag.ToString() + ".DateTime = currentRecord." + getUpper(((ComboBox)c.Controls[1]).Tag.ToString().Substring(0, 1).ToUpper()) + "" + ((ComboBox)c.Controls[1]).Tag.ToString().Substring(1) + ";");

                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        sbCodeBehing.AppendLine("}");
                    }
                    sbCodeBehing.AppendLine("}");
                    sbCodeBehing.AppendLine("#endregion PROPERTIES");
                }

                #endregion PROPERTIES

                sbCodeBehing.AppendLine();

                #region CONTRUCTOR && PAGE_LOAD

                sbCodeBehing.AppendLine("#region CONTRUCTOR && PAGE_LOAD");

                sbCodeBehing.AppendLine("protected void Page_Load(object sender, EventArgs e)");
                sbCodeBehing.AppendLine("{");
                {
                    sbCodeBehing.AppendLine(" ASPxGridView1.SettingsText.EmptyDataRow = \"Kayıt Bulunamadı\";");
                    sbCodeBehing.AppendLine("//if(!Page.IsPostBack)");

                    sbCodeBehing.AppendLine("//if(Session[AppConst.ADMININFO] == null)");
                    sbCodeBehing.AppendLine("//return;");

                    sbCodeBehing.AppendLine("setDefaultValuesToControls();");
                    sbCodeBehing.AppendLine("setEnabledToControls();");

                    sbCodeBehing.AppendLine(" getAndSetAllRecord();");
                    sbCodeBehing.AppendLine(" setInformationVisible(false);");
                }



                sbCodeBehing.AppendLine("}");
                sbCodeBehing.AppendLine("#endregion CONTRUCTOR && PAGE_LOAD");

                #endregion CONTRUCTOR && PAGE_LOAD

                sbCodeBehing.AppendLine();

                #region METHODS

                sbCodeBehing.AppendLine("#region METHODS");

                sbCodeBehing.AppendLine(" private void setDefaultValuesToControls()");
                sbCodeBehing.AppendLine("{");
                sbCodeBehing.AppendLine("}");

                sbCodeBehing.AppendLine("");
                sbCodeBehing.AppendLine("private void setEnabledToControls()");
                sbCodeBehing.AppendLine("{");
                sbCodeBehing.AppendLine("}");

                sbCodeBehing.AppendLine("private void getAndSetAllRecord()");
                sbCodeBehing.AppendLine("{");
                {
                    sbCodeBehing.AppendLine("SearchEntity entity = new SearchEntity();");
                    sbCodeBehing.AppendLine("//entity.IsDeleted = false;");
                    sbCodeBehing.AppendLine("//entity.IsActive = true;");

                    sbCodeBehing.AppendLine("resultSet = business.EklenecekGet(entity);");

                    sbCodeBehing.AppendLine("if (resultSet.HasError)");
                    sbCodeBehing.AppendLine("{");
                    sbCodeBehing.AppendLine("divError.Visible  = true;");
                    sbCodeBehing.AppendLine("lblError.Text = resultSet.ErrorDescription;");
                    sbCodeBehing.AppendLine("return;");
                    sbCodeBehing.AppendLine("}");

                    sbCodeBehing.AppendLine("lst = resultSet.Result;");

                    sbCodeBehing.AppendLine("if (lst == null)");
                    sbCodeBehing.AppendLine("throw new Exception(\"Business katmanı eklenmeli\");");
                    sbCodeBehing.AppendLine("");

                    sbCodeBehing.AppendLine("ASPxGridView1.DataSource = lst;");
                    sbCodeBehing.AppendLine("ASPxGridView1.DataBind();");

                    sbCodeBehing.AppendLine("");
                    sbCodeBehing.AppendLine("/*");
                    sbCodeBehing.AppendLine("int sayac = 0;");
                    sbCodeBehing.AppendLine("foreach(" + AppConstant.EntityName + "Entities el in lst)");
                    {
                        sbCodeBehing.AppendLine("{");
                        sbCodeBehing.AppendLine("sayac++;");
                        sbCodeBehing.AppendLine("string bgColor = \"#defdef\";");
                        sbCodeBehing.AppendLine("if (sayac % 2 == 0)");
                        sbCodeBehing.AppendLine("bgColor = \"#edfedf\";");
                        sbCodeBehing.AppendLine("TableRow tr = new TableRow();");
                        sbCodeBehing.AppendLine("tr.Attributes.Add(\"align\", \"center\");");
                        sbCodeBehing.AppendLine(" tr.Attributes.Add(\"bgcolor\", bgColor);");
                        {
                            number = 0;
                            int sayac = 0;
                            foreach (Control cc in pnlMain.Controls)
                            {
                                number++;

                                foreach (Control c in pnlMain.Controls)
                                {
                                    if (number.ToString() == ((TextBox)c.Controls[7]).Text && ((CheckBox)c.Controls[2]).Checked)
                                    {
                                        string text = ((ComboBox)c.Controls[1]).Tag.ToString();

                                        if (sayac == 0)
                                        {
                                            sbCodeBehing.AppendLine("TableCell tc = new TableCell();");
                                            sayac++;
                                        }
                                        else
                                            sbCodeBehing.AppendLine("tc = new TableCell();");

                                        sbCodeBehing.AppendLine("tc.Text = el." + getUpper(text.Substring(0, 1).ToUpper()) + "" + text.Substring(1) + ".ToString();");
                                        sbCodeBehing.AppendLine("tr.Cells.Add(tc);");
                                    }
                                }
                            }
                            sbCodeBehing.AppendLine("tbl.Rows.Add(tr);");

                        }
                        sbCodeBehing.AppendLine("}");
                    }
                    sbCodeBehing.AppendLine("*/");
                    sbCodeBehing.AppendLine("}");
                }
                sbCodeBehing.AppendLine("");
                sbCodeBehing.AppendLine("private bool validate()");
                {
                    sbCodeBehing.AppendLine("{");
                    sbCodeBehing.AppendLine("//write validation code");
                    sbCodeBehing.AppendLine("//lblResult.Text=error");
                    sbCodeBehing.AppendLine("return true;");
                    sbCodeBehing.AppendLine("}");
                }
                sbCodeBehing.AppendLine("");

                sbCodeBehing.AppendLine("private void setInformationVisible(bool visible)");
                sbCodeBehing.AppendLine("{");
                sbCodeBehing.AppendLine(" divError.Visible = visible;");
                sbCodeBehing.AppendLine(" divInformation.Visible = visible;");
                sbCodeBehing.AppendLine("}");

                sbCodeBehing.AppendLine("");

                sbCodeBehing.AppendLine("private void processToDatabase(DatabaseProcess databaseProcess,string labelText=RecordMessage.Default,string id=null)");
                {
                    sbCodeBehing.AppendLine("{");
                    sbCodeBehing.AppendLine("if (!validate())");
                    sbCodeBehing.AppendLine("return;");

                    sbCodeBehing.AppendLine("Entities entity = new Entities();");

                    sbCodeBehing.AppendLine("entity.DatabaseProcess = databaseProcess;");

                    foreach (Control c in pnlMain.Controls)
                    {
                        ComboBox column = ((ComboBox)c.Controls[6]);
                        string type = column.Tag.ToString();
                        string controlName = ((ComboBox)c.Controls[1]).Tag.ToString();

                        switch (((ComboBox)c.Controls[1]).Text)
                        {
                            //getUpper(x1.Isim[0].ToString().ToUpper()) + "" + x1.Isim.Substring(1)
                            case "TextBox":
                                if (type != "String")
                                {
                                    sbCodeBehing.AppendLine("entity." + getUpper(controlName.Substring(0, 1).ToUpper()) + "" + controlName.Substring(1) + " = GeneralFunctions.GetData<" + type + ">(txt_" + controlName + ".Text);");
                                }
                                else
                                    sbCodeBehing.AppendLine("entity." + getUpper(controlName.Substring(0, 1).ToUpper()) + "" + controlName.Substring(1) + " = txt_" + ((ComboBox)c.Controls[1]).Tag.ToString() + ".Text;");

                                break;
                            case "ListBox":
                                if (type != "String")
                                {
                                    sbCodeBehing.AppendLine("entity." + getUpper(controlName.Substring(0, 1).ToUpper()) + "" + controlName.Substring(1) + " =GeneralFunctions.GetData<" + type + ">(lst_" + ((ComboBox)c.Controls[1]).Tag.ToString() + ".SelectedValue);");
                                }
                                else
                                    sbCodeBehing.AppendLine("entity." + getUpper(controlName.Substring(0, 1).ToUpper()) + "" + controlName.Substring(1) + " =lst_" + ((ComboBox)c.Controls[1]).Tag.ToString() + ".SelectedValue;");
                                break;
                            case "ComboBox":
                                if (type != "String")
                                {
                                    sbCodeBehing.AppendLine("entity." + getUpper(controlName.Substring(0, 1).ToUpper()) + "" + controlName.Substring(1) + " =GeneralFunctions.GetData<" + type + ">(cmb_" + ((ComboBox)c.Controls[1]).Tag.ToString() + ".SelectedValue);");
                                }
                                else
                                    sbCodeBehing.AppendLine("entity." + getUpper(controlName.Substring(0, 1).ToUpper()) + "" + controlName.Substring(1) + " =drp_" + ((ComboBox)c.Controls[1]).Tag.ToString() + ".SelectedValue;");

                                break;
                            case "RadioButton":

                                sbCodeBehing.AppendLine("entity." + getUpper(controlName.Substring(0, 1).ToUpper()) + "" + controlName.Substring(1) + " =rdb_" + ((ComboBox)c.Controls[1]).Tag.ToString() + ".Checked;");

                                break;
                            case "PictureBox":
                                sbCodeBehing.AppendLine("entity." + getUpper(controlName.Substring(0, 1).ToUpper()) + "" + controlName.Substring(1) + " =txt_" + ((ComboBox)c.Controls[1]).Tag.ToString() + ".Text;");

                                break;
                            case "CheckBox":

                                sbCodeBehing.AppendLine("entity." + getUpper(controlName.Substring(0, 1).ToUpper()) + "" + controlName.Substring(1) + " =chc_" + ((ComboBox)c.Controls[1]).Tag.ToString() + ".Checked;");

                                break;
                            case "DateTime":

                                sbCodeBehing.AppendLine("entity." + getUpper(controlName.Substring(0, 1).ToUpper()) + "" + controlName.Substring(1) + " =dt_" + ((ComboBox)c.Controls[1]).Tag.ToString() + ".Date;");

                                break;
                            default:
                                break;
                        }
                    }


                    sbCodeBehing.AppendLine("DataResultArgs<bool> resultSet = business.EklenecekSet(entity);");


                    sbCodeBehing.AppendLine("if (resultSet.HasError)");
                    sbCodeBehing.AppendLine("{");
                    sbCodeBehing.AppendLine("divError.Visible = true;");
                    sbCodeBehing.AppendLine("lblError.Text = resultSet.ErrorDescription;");
                    sbCodeBehing.AppendLine("return;");
                    sbCodeBehing.AppendLine("}");
                    sbCodeBehing.AppendLine("else");
                    sbCodeBehing.AppendLine("{");
                    sbCodeBehing.AppendLine("divInformation.Visible = true;");
                    sbCodeBehing.AppendLine("lblInformation.Text = labelText;");
                    sbCodeBehing.AppendLine("getAndSetAllRecord();");
                    sbCodeBehing.AppendLine("CurrentRecord = new Entities();");
                    sbCodeBehing.AppendLine("btnKaydet.Text = ButtonText.Submit;");
                    sbCodeBehing.AppendLine("}");

                    sbCodeBehing.AppendLine("}");
                }

                sbCodeBehing.AppendLine("#endregion METHODS");

                #endregion METHODS

                sbCodeBehing.AppendLine();

                #region EVENTS

                sbCodeBehing.AppendLine("#region EVENTS");
                sbCodeBehing.AppendLine("protected void btnKaydet_Click(object sender, EventArgs e)");
                {
                    sbCodeBehing.AppendLine("{");
                    sbCodeBehing.AppendLine("if (!validate())");
                    sbCodeBehing.AppendLine("return;");

                    sbCodeBehing.AppendLine("Button btn = ((Button)sender);");
                    sbCodeBehing.AppendLine("bool insert = btn.Text == ButtonText.Submit;");

                    sbCodeBehing.AppendLine("processToDatabase((insert) ? DatabaseProcess.Add : DatabaseProcess.Update, (insert) ? RecordMessage.Add : RecordMessage.Update);");
                    sbCodeBehing.AppendLine("setEnabledToControls();");
                    sbCodeBehing.AppendLine("setDefaultValuesToControls();");
                    sbCodeBehing.AppendLine("}");
                }

                sbCodeBehing.AppendLine("protected void btnUpdate_Command(object sender, CommandEventArgs e)");
                {
                    sbCodeBehing.AppendLine("{");

                    sbCodeBehing.AppendLine("String id = e.CommandArgument.ToString();");
                    sbCodeBehing.AppendLine("CurrentRecord = lst.FirstOrDefault(o => o.Id == GeneralFunctions.GetData<Int32>(id));");
                    sbCodeBehing.AppendLine("btnSubmit.Text = ButtonText.Update;");
                    sbCodeBehing.AppendLine("setEnabledToControls();");

                    sbCodeBehing.AppendLine("}");
                }
                sbCodeBehing.AppendLine("");

                //sbCodeBehing.AppendLine("protected void ASPxGridView1_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableDataCellEventArgs e)");
                //{
                //    sbCodeBehing.AppendLine("{");
                //    sbCodeBehing.AppendLine("if (e.DataColumn.Caption == \"Process\")");
                //    sbCodeBehing.AppendLine("{");
                //    sbCodeBehing.AppendLine(" Button btn = (Button)ASPxGridView1.FindRowCellTemplateControl(e.VisibleIndex, e.DataColumn, \"btnDelete\");");
                //    sbCodeBehing.AppendLine(" btn.Attributes.Add(\"onclick\", \"return confirm('Silmek istediğinize emin misiniz?');\");");
                //    sbCodeBehing.AppendLine("}");
                //    sbCodeBehing.AppendLine("  }");
                //}
                //sbCodeBehing.AppendLine("");


                sbCodeBehing.AppendLine("protected void btnCancel_Click(object sender, EventArgs e)");
                {
                    sbCodeBehing.AppendLine("{");
                    {
                        sbCodeBehing.AppendLine("CurrentRecord = new Entities();");
                        sbCodeBehing.AppendLine("btnSubmit.Text = ButtonText.Submit;");
                        sbCodeBehing.AppendLine("setEnabledToControls();");
                        sbCodeBehing.AppendLine("setInformationVisible(false);");
                    }
                    sbCodeBehing.AppendLine("}");
                }



                sbCodeBehing.AppendLine("protected void btnDelete_Command(object sender, CommandEventArgs e)");
                sbCodeBehing.AppendLine("{");
                {
                    sbCodeBehing.AppendLine("//lblResult.Text = e.CommandArgument.ToString();");

                    sbCodeBehing.AppendLine("string id = e.CommandArgument.ToString();");
                    sbCodeBehing.AppendLine("processToDatabase(DatabaseProcess.Delete, RecordMessage.Delete,id);");
                }
                sbCodeBehing.AppendLine("}");

                sbCodeBehing.AppendLine("#endregion EVENTS");

                #endregion EVENTS

                #endregion

                AppConstant.HtmlCode = sbHtml.ToString();
                AppConstant.CodeBehind = sbCodeBehing.ToString();

                frmStoreProcedure frm = new frmStoreProcedure();
                frm.Show();
            }
            else
            {
                MessageBox.Show("Yazılacak...");
            }

        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            

            if (treeView1.SelectedNode==null || e.Node.Parent == null || e.Node.Parent.Parent == null && e.Node.Nodes.Count > 0)
            {
            }
            else
            {
                databaseName = e.Node.Parent.Text;
                tableName = e.Node.Text;
                sqlConDatabase = AppConstant.GetConnectionString + " Initial Catalog =" + databaseName;
                using (SqlConnection con = new SqlConnection(sqlConDatabase))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM " + e.Node.Text, con))
                    {
                        dt = new DataTable();
                        SqlDataAdapter sadp = new SqlDataAdapter(cmd);
                        sadp.Fill(dt);
                        pnlMain.Controls.Clear();
                        int sayac = 0;
                        foreach (DataColumn dc in dt.Columns)
                        {
                            if (dc.ColumnName == "isDeleted" || dc.ColumnName == "addedOn" || dc.ColumnName == "updatedOn" || dc.ColumnName == "deletedOn")
                            {
                            }
                            else
                            {


                                sayac++;
                                FlowLayoutPanel flwpnl = new FlowLayoutPanel();
                                if (sayac % 2 == 0)
                                    flwpnl.BackColor = Color.AliceBlue;
                                else
                                    flwpnl.BackColor = Color.Beige;
                                flwpnl.BorderStyle = BorderStyle.Fixed3D;
                                flwpnl.AutoScroll = true;
                                flwpnl.Height = 50;
                                flwpnl.Dock = DockStyle.Top;

                                TextBox txt = new TextBox();
                                txt.Text = dc.ColumnName;
                                flwpnl.Controls.Add(txt);
                                ComboBox cmb = fillTypeOfControls(dc.DataType);
                                cmb.Tag = dc.ColumnName;


                                flwpnl.Controls.Add(cmb);
                                cmb.SelectedIndexChanged += new EventHandler(cmb_SelectedIndexChanged);

                                CheckBox chcVisible = new CheckBox();
                                chcVisible.Text = "Visible |";
                                chcVisible.Checked = true;
                                flwpnl.Controls.Add(chcVisible);


                                CheckBox chcEnabled = new CheckBox();
                                chcEnabled.Text = "Enabled";
                                chcEnabled.Checked = true;
                                flwpnl.Controls.Add(chcEnabled);

                                CheckBox chcIsRequired = new CheckBox();
                                chcIsRequired.Name = "isRequired";
                                chcIsRequired.Text = "IsRequired";
                                chcIsRequired.Checked = true;
                                flwpnl.Controls.Add(chcIsRequired);

                                ComboBox cmbTable = fillTable();
                                cmbTable.Name = "cmbTable";
                                cmbTable.SelectedIndexChanged += new EventHandler(cmbTable_SelectedIndexChanged);

                                //cmbTable.Enabled = false;

                                flwpnl.Controls.Add(cmbTable);

                                ComboBox cmbColumn = fillTableColumns(flwpnl, "cmbTable");
                                cmbColumn.Name = "cmbColumn";
                                //cmbColumn.Enabled = false;
                                //cmbTable.Enabled = false;
                                flwpnl.Controls.Add(cmbColumn);
                                cmbColumn.Text = dc.ColumnName;
                                cmbColumn.Tag = dc.DataType.ToString().Replace("System.", " ").Trim();
                                cmb_SelectedIndexChanged(cmb, null);
                                pnlMain.Controls.Add(flwpnl);

                                TextBox txtSira = new TextBox();
                                txtSira.Text = sayac.ToString();

                                flwpnl.Controls.Add(txtSira);
                            }
                        }
                    }
                    con.Close();
                }
            }
        }
        private void cmbTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Control c in ((FlowLayoutPanel)(((ComboBox)sender).Parent)).Controls)
            {
                if (c.Name == "cmbColumn" && c is ComboBox)
                {
                    ComboBox cmb = fillTableColumns((FlowLayoutPanel)(((ComboBox)sender).Parent), "cmbTable");
                    ComboBox cc = ((ComboBox)c);

                    cc.Items.Clear();
                    foreach (string s in cmb.Items)
                    {
                        cc.Items.Add(s);
                    }

                }
            }
        }
        private void cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = ((ComboBox)sender);
            if (cmb.Text == "ComboBox" || cmb.Text == "ListBox")
            {
                if (((FlowLayoutPanel)(((ComboBox)sender).Parent)).Controls.Find("cmbColumnCombobox", false).Count() == 0)
                {
                    ComboBox c = fillTable("cmbTableCombobox");
                    c.Name = "cmbTableCombobox";
                    c.SelectedIndexChanged += new EventHandler(c_SelectedIndexChanged);
                    ((FlowLayoutPanel)(((ComboBox)sender).Parent)).Controls.Add(c);
                    ComboBox cc = fillTableColumns(((FlowLayoutPanel)(((ComboBox)sender).Parent)), "cmbTableCombobox");
                    cc.Name = "cmbColumnCombobox";
                    ((FlowLayoutPanel)(((ComboBox)sender).Parent)).Controls.Add(cc);
                }

            }
            else
            {
                if (((FlowLayoutPanel)(((ComboBox)sender).Parent)).Controls.Find("cmbTableCombobox", false).Count() > 0)
                {
                    Control cnrtl = ((FlowLayoutPanel)(((ComboBox)sender).Parent)).Controls.Find("cmbTableCombobox", false).First();
                    ((FlowLayoutPanel)(((ComboBox)sender).Parent)).Controls.Remove(cnrtl);
                }

                if (((FlowLayoutPanel)(((ComboBox)sender).Parent)).Controls.Find("cmbColumnCombobox", false).Count() > 0)
                {
                    Control cnrtl = ((FlowLayoutPanel)(((ComboBox)sender).Parent)).Controls.Find("cmbColumnCombobox", false).First();
                    ((FlowLayoutPanel)(((ComboBox)sender).Parent)).Controls.Remove(cnrtl);
                }
            }

            Control cnrtll = ((FlowLayoutPanel)(((ComboBox)sender).Parent)).Controls.Find("isRequired", false).First();
            if (cmb.Text != "TextBox")
            {
                ((CheckBox)cnrtll).Enabled = false;
                ((CheckBox)cnrtll).Checked = false;
            }
            else
            {
                ((CheckBox)cnrtll).Enabled = true;
                ((CheckBox)cnrtll).Checked = true;
            }


        }
        private void c_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Control c in ((FlowLayoutPanel)(((ComboBox)sender).Parent)).Controls)
            {
                if (c.Name == "cmbColumnCombobox" && c is ComboBox)
                {
                    ComboBox cmb = fillTableColumns((FlowLayoutPanel)(((ComboBox)sender).Parent), "cmbTableCombobox");
                    ComboBox cc = ((ComboBox)c);

                    cc.Items.Clear();
                    foreach (string s in cmb.Items)
                    {
                        cc.Items.Add(s);
                    }

                }
            }
        }
        private void txtResult_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                //txtResult.SelectAll();
            }

        }

        private void rdbSp_CheckedChanged(object sender, EventArgs e)
        {
            bool sp = rdbSp.Checked;
            if (sp)
            {
                cmbDataBase.SelectedIndex = 0;
                txtQuery.Text = "";
                splitContainer4.Visible = false;
                treeView1.Visible = true;
            }
            else
            {
                cmbDataBase.SelectedIndex = 0;
                txtQuery.Text = "";
                splitContainer4.Visible = true;
                treeView1.Visible = false;
            }
        }
        private void txtQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control)
            {
                txtQuery.SelectAll();
            }
        }

        #endregion

    }
}
