namespace AsproRDTool
{
    partial class mainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainWindow));
            mtlbl_Server = new MaterialSkin.Controls.MaterialLabel();
            mtlbl_Db = new MaterialSkin.Controls.MaterialLabel();
            cmbx_Db = new ComboBox();
            mtlbl_timer = new MaterialSkin.Controls.MaterialLabel();
            mtPrgsBr = new MaterialSkin.Controls.MaterialProgressBar();
            mtBtn_Execute = new MaterialSkin.Controls.MaterialButton();
            cmbx_Server = new ComboBox();
            mtlbl_Status = new MaterialSkin.Controls.MaterialLabel();
            SuspendLayout();
            // 
            // mtlbl_Server
            // 
            mtlbl_Server.AutoSize = true;
            mtlbl_Server.Depth = 0;
            mtlbl_Server.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            mtlbl_Server.Location = new Point(82, 139);
            mtlbl_Server.MouseState = MaterialSkin.MouseState.HOVER;
            mtlbl_Server.Name = "mtlbl_Server";
            mtlbl_Server.Size = new Size(45, 19);
            mtlbl_Server.TabIndex = 0;
            mtlbl_Server.Text = "Server";
            // 
            // mtlbl_Db
            // 
            mtlbl_Db.AutoSize = true;
            mtlbl_Db.Depth = 0;
            mtlbl_Db.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            mtlbl_Db.Location = new Point(82, 190);
            mtlbl_Db.MouseState = MaterialSkin.MouseState.HOVER;
            mtlbl_Db.Name = "mtlbl_Db";
            mtlbl_Db.Size = new Size(69, 19);
            mtlbl_Db.TabIndex = 1;
            mtlbl_Db.Text = "Database";
            // 
            // cmbx_Db
            // 
            cmbx_Db.FormattingEnabled = true;
            cmbx_Db.Location = new Point(199, 184);
            cmbx_Db.Name = "cmbx_Db";
            cmbx_Db.Size = new Size(293, 28);
            cmbx_Db.TabIndex = 3;
            // 
            // mtlbl_timer
            // 
            mtlbl_timer.AutoSize = true;
            mtlbl_timer.Depth = 0;
            mtlbl_timer.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            mtlbl_timer.Location = new Point(429, 227);
            mtlbl_timer.MouseState = MaterialSkin.MouseState.HOVER;
            mtlbl_timer.Name = "mtlbl_timer";
            mtlbl_timer.Size = new Size(63, 19);
            mtlbl_timer.TabIndex = 4;
            mtlbl_timer.Text = "00:00:00";
            mtlbl_timer.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // mtPrgsBr
            // 
            mtPrgsBr.Depth = 0;
            mtPrgsBr.Location = new Point(82, 275);
            mtPrgsBr.MouseState = MaterialSkin.MouseState.HOVER;
            mtPrgsBr.Name = "mtPrgsBr";
            mtPrgsBr.Size = new Size(410, 5);
            mtPrgsBr.TabIndex = 5;
            // 
            // mtBtn_Execute
            // 
            mtBtn_Execute.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            mtBtn_Execute.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            mtBtn_Execute.Depth = 0;
            mtBtn_Execute.HighEmphasis = true;
            mtBtn_Execute.Icon = null;
            mtBtn_Execute.Location = new Point(242, 319);
            mtBtn_Execute.Margin = new Padding(4, 6, 4, 6);
            mtBtn_Execute.MouseState = MaterialSkin.MouseState.HOVER;
            mtBtn_Execute.Name = "mtBtn_Execute";
            mtBtn_Execute.NoAccentTextColor = Color.Empty;
            mtBtn_Execute.Size = new Size(84, 36);
            mtBtn_Execute.TabIndex = 6;
            mtBtn_Execute.Text = "Execute";
            mtBtn_Execute.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            mtBtn_Execute.UseAccentColor = false;
            mtBtn_Execute.UseVisualStyleBackColor = true;
            mtBtn_Execute.Click += mtBtn_Execute_Click;
            // 
            // cmbx_Server
            // 
            cmbx_Server.FormattingEnabled = true;
            cmbx_Server.Items.AddRange(new object[] { "Aquila", "Blackbird", "Griffin", "Raven", "Skylark", "Localhost" });
            cmbx_Server.Location = new Point(199, 133);
            cmbx_Server.Name = "cmbx_Server";
            cmbx_Server.Size = new Size(293, 28);
            cmbx_Server.TabIndex = 7;
            cmbx_Server.SelectedIndexChanged += cmbx_Server_SelectedIndexChanged;
            // 
            // mtlbl_Status
            // 
            mtlbl_Status.AutoSize = true;
            mtlbl_Status.Depth = 0;
            mtlbl_Status.Font = new Font("Arial Narrow", 7.8F, FontStyle.Italic, GraphicsUnit.Point);
            mtlbl_Status.ForeColor = Color.RoyalBlue;
            mtlbl_Status.Location = new Point(82, 246);
            mtlbl_Status.MouseState = MaterialSkin.MouseState.HOVER;
            mtlbl_Status.Name = "mtlbl_Status";
            mtlbl_Status.Size = new Size(1, 0);
            mtlbl_Status.TabIndex = 9;
            // 
            // mainWindow
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(572, 421);
            Controls.Add(mtlbl_Status);
            Controls.Add(cmbx_Server);
            Controls.Add(mtPrgsBr);
            Controls.Add(mtlbl_timer);
            Controls.Add(cmbx_Db);
            Controls.Add(mtlbl_Db);
            Controls.Add(mtlbl_Server);
            Controls.Add(mtBtn_Execute);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "mainWindow";
            Sizable = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Aspro RDTool";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ProgressBar progressBar1;
        private MaterialSkin.Controls.MaterialLabel mtlbl_Server;
        private MaterialSkin.Controls.MaterialLabel mtlbl_Db;
        private ComboBox cmbx_Db;
        private MaterialSkin.Controls.MaterialProgressBar mtPrgsBr;
        private MaterialSkin.Controls.MaterialButton mtBtn_Execute;
        private ComboBox cmbx_Server;
        private MaterialSkin.Controls.MaterialLabel mtlbl_Status;
        internal MaterialSkin.Controls.MaterialLabel mtlbl_timer;
    }
}