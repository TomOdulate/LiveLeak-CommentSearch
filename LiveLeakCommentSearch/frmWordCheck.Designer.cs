namespace LiveLeakCommentSearch
{
    partial class frmWordCheck
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
            this.txtTargetWord = new System.Windows.Forms.TextBox();
            this.btnCheck = new System.Windows.Forms.Button();
            this.imgCross = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.imgTick = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.imgCross)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgTick)).BeginInit();
            this.SuspendLayout();
            // 
            // txtTargetWord
            // 
            this.txtTargetWord.Location = new System.Drawing.Point(12, 12);
            this.txtTargetWord.Name = "txtTargetWord";
            this.txtTargetWord.Size = new System.Drawing.Size(275, 20);
            this.txtTargetWord.TabIndex = 0;
            this.txtTargetWord.TextChanged += new System.EventHandler(this.txtTargetWord_TextChanged);
            this.txtTargetWord.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTargetWord_KeyDown);
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(293, 12);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(75, 20);
            this.btnCheck.TabIndex = 1;
            this.btnCheck.Text = "Check";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // imgCross
            // 
            this.imgCross.Location = new System.Drawing.Point(293, 38);
            this.imgCross.Name = "imgCross";
            this.imgCross.Size = new System.Drawing.Size(75, 75);
            this.imgCross.TabIndex = 2;
            this.imgCross.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // imgTick
            // 
            this.imgTick.Location = new System.Drawing.Point(293, 38);
            this.imgTick.Name = "imgTick";
            this.imgTick.Size = new System.Drawing.Size(75, 75);
            this.imgTick.TabIndex = 4;
            this.imgTick.TabStop = false;
            // 
            // frmWordCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(373, 118);
            this.Controls.Add(this.imgTick);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.imgCross);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.txtTargetWord);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmWordCheck";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Word Check";
            ((System.ComponentModel.ISupportInitialize)(this.imgCross)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgTick)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTargetWord;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.PictureBox imgCross;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox imgTick;
    }
}