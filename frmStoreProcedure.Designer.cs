namespace FormGenerator
{
    partial class frmStoreProcedure
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnStoreProceudre = new System.Windows.Forms.Button();
            this.txtInUpDel = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSkip = new System.Windows.Forms.Button();
            this.txtSelect = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.txtInUpDel);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Panel2.Controls.Add(this.txtSelect);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Size = new System.Drawing.Size(784, 562);
            this.splitContainer1.SplitterDistance = 386;
            this.splitContainer1.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnStoreProceudre);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 535);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(386, 27);
            this.panel1.TabIndex = 5;
            // 
            // btnStoreProceudre
            // 
            this.btnStoreProceudre.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStoreProceudre.Location = new System.Drawing.Point(0, 0);
            this.btnStoreProceudre.Name = "btnStoreProceudre";
            this.btnStoreProceudre.Size = new System.Drawing.Size(386, 27);
            this.btnStoreProceudre.TabIndex = 2;
            this.btnStoreProceudre.Text = "Add in Database And Continiue";
            this.btnStoreProceudre.UseVisualStyleBackColor = true;
            this.btnStoreProceudre.Click += new System.EventHandler(this.btnStoreProceudre_Click);
            // 
            // txtInUpDel
            // 
            this.txtInUpDel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInUpDel.Location = new System.Drawing.Point(0, 13);
            this.txtInUpDel.Multiline = true;
            this.txtInUpDel.Name = "txtInUpDel";
            this.txtInUpDel.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInUpDel.Size = new System.Drawing.Size(386, 549);
            this.txtInUpDel.TabIndex = 4;
            this.txtInUpDel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtInUpDel_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Insert Update Delete";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSkip);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 535);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(394, 27);
            this.panel2.TabIndex = 5;
            // 
            // btnSkip
            // 
            this.btnSkip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSkip.Location = new System.Drawing.Point(0, 0);
            this.btnSkip.Margin = new System.Windows.Forms.Padding(2);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(394, 27);
            this.btnSkip.TabIndex = 0;
            this.btnSkip.Text = "Skip";
            this.btnSkip.UseVisualStyleBackColor = true;
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // txtSelect
            // 
            this.txtSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSelect.Location = new System.Drawing.Point(0, 13);
            this.txtSelect.Multiline = true;
            this.txtSelect.Name = "txtSelect";
            this.txtSelect.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSelect.Size = new System.Drawing.Size(394, 549);
            this.txtSelect.TabIndex = 4;
            this.txtSelect.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSelect_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Select";
            // 
            // frmStoreProcedure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmStoreProcedure";
            this.Text = "StoreProcedure";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.StoreProcedure_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtInUpDel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSelect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnStoreProceudre;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSkip;
    }
}