namespace LiveLeakCommentSearch
{
    partial class FrmCommentMonitor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCommentMonitor));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblCurrentComment = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCurrentCommentCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslblTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblProcessedComents = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblProcessedComentsCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnCheckWord = new System.Windows.Forms.Button();
            this.chkUseWordlist = new System.Windows.Forms.CheckBox();
            this.btnEditWordList = new System.Windows.Forms.Button();
            this.chkNotifications = new System.Windows.Forms.CheckBox();
            this.bntClear = new System.Windows.Forms.Button();
            this.chkAutoOpen = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.numCommentId = new System.Windows.Forms.NumericUpDown();
            this.btnFindComment = new System.Windows.Forms.Button();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.btnFindUser = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.grdOutput = new System.Windows.Forms.DataGridView();
            this.clmCommentId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmUserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmwordCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmLink = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Link2 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tmrLabelFlash = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCommentId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdOutput)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblCurrentComment,
            this.lblCurrentCommentCount,
            this.tslblTime,
            this.lblProcessedComents,
            this.lblProcessedComentsCount,
            this.lblStatus,
            this.toolStripStatusLabelVersion});
            this.statusStrip1.Location = new System.Drawing.Point(0, 305);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(973, 24);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblCurrentComment
            // 
            this.lblCurrentComment.Name = "lblCurrentComment";
            this.lblCurrentComment.Size = new System.Drawing.Size(104, 19);
            this.lblCurrentComment.Text = "Current Comment";
            // 
            // lblCurrentCommentCount
            // 
            this.lblCurrentCommentCount.Name = "lblCurrentCommentCount";
            this.lblCurrentCommentCount.Size = new System.Drawing.Size(49, 19);
            this.lblCurrentCommentCount.Text = "0000000";
            this.lblCurrentCommentCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCurrentCommentCount.Click += new System.EventHandler(this.lblCurrentCommentCount_Click);
            // 
            // tslblTime
            // 
            this.tslblTime.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tslblTime.Name = "tslblTime";
            this.tslblTime.Size = new System.Drawing.Size(4, 19);
            // 
            // lblProcessedComents
            // 
            this.lblProcessedComents.Name = "lblProcessedComents";
            this.lblProcessedComents.Size = new System.Drawing.Size(149, 19);
            this.lblProcessedComents.Text = "Total comments processed";
            // 
            // lblProcessedComentsCount
            // 
            this.lblProcessedComentsCount.Name = "lblProcessedComentsCount";
            this.lblProcessedComentsCount.Size = new System.Drawing.Size(13, 19);
            this.lblProcessedComentsCount.Text = "0";
            this.lblProcessedComentsCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStatus
            // 
            this.lblStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblStatus.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedInner;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(32, 19);
            this.lblStatus.Text = "Idle";
            // 
            // toolStripStatusLabelVersion
            // 
            this.toolStripStatusLabelVersion.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabelVersion.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.toolStripStatusLabelVersion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabelVersion.Name = "toolStripStatusLabelVersion";
            this.toolStripStatusLabelVersion.Size = new System.Drawing.Size(607, 19);
            this.toolStripStatusLabelVersion.Spring = true;
            this.toolStripStatusLabelVersion.Text = "Development version";
            this.toolStripStatusLabelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnImport);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnCheckWord);
            this.panel1.Controls.Add(this.chkUseWordlist);
            this.panel1.Controls.Add(this.btnEditWordList);
            this.panel1.Controls.Add(this.chkNotifications);
            this.panel1.Controls.Add(this.bntClear);
            this.panel1.Controls.Add(this.chkAutoOpen);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.numCommentId);
            this.panel1.Controls.Add(this.btnFindComment);
            this.panel1.Controls.Add(this.txtUserName);
            this.panel1.Controls.Add(this.btnFindUser);
            this.panel1.Controls.Add(this.btnStop);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(767, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(206, 305);
            this.panel1.TabIndex = 8;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(103, 200);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(94, 23);
            this.btnImport.TabIndex = 25;
            this.btnImport.Text = "Import Results";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(103, 168);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(94, 23);
            this.btnExport.TabIndex = 24;
            this.btnExport.Text = "Export Results";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnCheckWord
            // 
            this.btnCheckWord.Location = new System.Drawing.Point(103, 136);
            this.btnCheckWord.Name = "btnCheckWord";
            this.btnCheckWord.Size = new System.Drawing.Size(94, 23);
            this.btnCheckWord.TabIndex = 23;
            this.btnCheckWord.Text = "Test Word";
            this.btnCheckWord.UseVisualStyleBackColor = true;
            this.btnCheckWord.Click += new System.EventHandler(this.btnCheckWord_Click);
            // 
            // chkUseWordlist
            // 
            this.chkUseWordlist.AutoSize = true;
            this.chkUseWordlist.Checked = true;
            this.chkUseWordlist.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseWordlist.Location = new System.Drawing.Point(13, 102);
            this.chkUseWordlist.Name = "chkUseWordlist";
            this.chkUseWordlist.Size = new System.Drawing.Size(89, 17);
            this.chkUseWordlist.TabIndex = 22;
            this.chkUseWordlist.Text = "Use Word list";
            this.chkUseWordlist.UseVisualStyleBackColor = true;
            this.chkUseWordlist.Click += new System.EventHandler(this.chkUseWordlist_CheckedChanged);
            // 
            // btnEditWordList
            // 
            this.btnEditWordList.Location = new System.Drawing.Point(103, 104);
            this.btnEditWordList.Name = "btnEditWordList";
            this.btnEditWordList.Size = new System.Drawing.Size(94, 23);
            this.btnEditWordList.TabIndex = 21;
            this.btnEditWordList.Text = "Edit Word List";
            this.btnEditWordList.UseVisualStyleBackColor = true;
            this.btnEditWordList.Click += new System.EventHandler(this.btnEditWordList_Click);
            // 
            // chkNotifications
            // 
            this.chkNotifications.AutoSize = true;
            this.chkNotifications.Checked = true;
            this.chkNotifications.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNotifications.Location = new System.Drawing.Point(13, 81);
            this.chkNotifications.Name = "chkNotifications";
            this.chkNotifications.Size = new System.Drawing.Size(84, 17);
            this.chkNotifications.TabIndex = 18;
            this.chkNotifications.Text = "Notifications";
            this.chkNotifications.UseVisualStyleBackColor = true;
            // 
            // bntClear
            // 
            this.bntClear.Enabled = false;
            this.bntClear.Location = new System.Drawing.Point(103, 72);
            this.bntClear.Name = "bntClear";
            this.bntClear.Size = new System.Drawing.Size(94, 23);
            this.bntClear.TabIndex = 17;
            this.bntClear.Text = "Clear";
            this.bntClear.UseVisualStyleBackColor = true;
            this.bntClear.Click += new System.EventHandler(this.bntClear_Click);
            // 
            // chkAutoOpen
            // 
            this.chkAutoOpen.AutoSize = true;
            this.chkAutoOpen.Location = new System.Drawing.Point(13, 60);
            this.chkAutoOpen.Name = "chkAutoOpen";
            this.chkAutoOpen.Size = new System.Drawing.Size(75, 17);
            this.chkAutoOpen.TabIndex = 16;
            this.chkAutoOpen.Text = "Auto open";
            this.chkAutoOpen.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(6, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(91, 54);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Direction";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(7, 34);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(78, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "Backwards";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(7, 16);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(63, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Forward";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // numCommentId
            // 
            this.numCommentId.Location = new System.Drawing.Point(119, 275);
            this.numCommentId.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.numCommentId.Name = "numCommentId";
            this.numCommentId.Size = new System.Drawing.Size(77, 20);
            this.numCommentId.TabIndex = 13;
            this.numCommentId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnFindComment
            // 
            this.btnFindComment.Location = new System.Drawing.Point(8, 274);
            this.btnFindComment.Name = "btnFindComment";
            this.btnFindComment.Size = new System.Drawing.Size(113, 22);
            this.btnFindComment.TabIndex = 12;
            this.btnFindComment.Text = "Get single comment";
            this.btnFindComment.UseVisualStyleBackColor = true;
            this.btnFindComment.Click += new System.EventHandler(this.btnFindComment_Click);
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(8, 229);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(189, 20);
            this.txtUserName.TabIndex = 11;
            this.txtUserName.TextChanged += new System.EventHandler(this.txtUserName_TextChanged);
            // 
            // btnFindUser
            // 
            this.btnFindUser.Enabled = false;
            this.btnFindUser.Location = new System.Drawing.Point(8, 248);
            this.btnFindUser.Name = "btnFindUser";
            this.btnFindUser.Size = new System.Drawing.Size(189, 20);
            this.btnFindUser.TabIndex = 10;
            this.btnFindUser.Text = "Find users comments";
            this.btnFindUser.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnFindUser.UseVisualStyleBackColor = true;
            this.btnFindUser.Click += new System.EventHandler(this.btnFindUser_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(103, 40);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(94, 23);
            this.btnStop.TabIndex = 9;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(103, 8);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(94, 23);
            this.btnStart.TabIndex = 8;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // grdOutput
            // 
            this.grdOutput.AllowUserToAddRows = false;
            this.grdOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdOutput.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmCommentId,
            this.clmUserName,
            this.clmwordCount,
            this.clmComment,
            this.clmLink,
            this.Link2});
            this.grdOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdOutput.Location = new System.Drawing.Point(0, 0);
            this.grdOutput.Name = "grdOutput";
            this.grdOutput.ReadOnly = true;
            this.grdOutput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdOutput.Size = new System.Drawing.Size(767, 305);
            this.grdOutput.TabIndex = 9;
            this.grdOutput.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdOutput_CellContentClick);
            this.grdOutput.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.grdOutput_RowsAdded);
            this.grdOutput.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.grdOutput_UserDeletedRow);
            this.grdOutput.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.grdOutput_UserDeletingRow);
            this.grdOutput.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdOutput_MouseClick);
            // 
            // clmCommentId
            // 
            this.clmCommentId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.clmCommentId.HeaderText = "ID";
            this.clmCommentId.Name = "clmCommentId";
            this.clmCommentId.ReadOnly = true;
            this.clmCommentId.Width = 43;
            // 
            // clmUserName
            // 
            this.clmUserName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.clmUserName.HeaderText = "User";
            this.clmUserName.Name = "clmUserName";
            this.clmUserName.ReadOnly = true;
            this.clmUserName.Width = 54;
            // 
            // clmwordCount
            // 
            this.clmwordCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.clmwordCount.HeaderText = "Count";
            this.clmwordCount.MinimumWidth = 40;
            this.clmwordCount.Name = "clmwordCount";
            this.clmwordCount.ReadOnly = true;
            this.clmwordCount.Width = 60;
            // 
            // clmComment
            // 
            this.clmComment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.clmComment.HeaderText = "Comment";
            this.clmComment.Name = "clmComment";
            this.clmComment.ReadOnly = true;
            this.clmComment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // clmLink
            // 
            this.clmLink.HeaderText = "Url";
            this.clmLink.Name = "clmLink";
            this.clmLink.ReadOnly = true;
            this.clmLink.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clmLink.Visible = false;
            // 
            // Link2
            // 
            this.Link2.HeaderText = "Link";
            this.Link2.MinimumWidth = 35;
            this.Link2.Name = "Link2";
            this.Link2.ReadOnly = true;
            this.Link2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Link2.Text = "Open Browser";
            this.Link2.Width = 35;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(12, 33);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(739, 162);
            this.panel2.TabIndex = 10;
            this.panel2.Visible = false;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(737, 160);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmrLabelFlash
            // 
            this.tmrLabelFlash.Interval = 1000;
            this.tmrLabelFlash.Tick += new System.EventHandler(this.tmrLabelFlash_Tick);
            // 
            // FrmCommentMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(973, 329);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.grdOutput);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(555, 368);
            this.Name = "FrmCommentMonitor";
            this.Text = "Liveleak Comment Search";
            this.Deactivate += new System.EventHandler(this.FrmCommentMonitor_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCommentId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdOutput)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblCurrentCommentCount;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown numCommentId;
        private System.Windows.Forms.Button btnFindComment;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Button btnFindUser;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.DataGridView grdOutput;
        private System.Windows.Forms.ToolStripStatusLabel lblCurrentComment;
        private System.Windows.Forms.ToolStripStatusLabel lblProcessedComents;
        private System.Windows.Forms.ToolStripStatusLabel lblProcessedComentsCount;
        private System.Windows.Forms.CheckBox chkAutoOpen;
        private System.Windows.Forms.Button bntClear;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer tmrLabelFlash;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelVersion;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chkNotifications;
        private System.Windows.Forms.ToolStripStatusLabel tslblTime;
        private System.Windows.Forms.Button btnEditWordList;
        private System.Windows.Forms.CheckBox chkUseWordlist;
        private System.Windows.Forms.Button btnCheckWord;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.DataGridViewButtonColumn Link2;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLink;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmComment;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmwordCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmUserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCommentId;
    }
}

