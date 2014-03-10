namespace NetCheatMemoryViewer
{
    partial class OptionsForm
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
            this.optSaveButt = new System.Windows.Forms.Button();
            this.optExitButt = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.relBCNDCheckb = new System.Windows.Forms.CheckBox();
            this.startAddr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // optSaveButt
            // 
            this.optSaveButt.BackColor = System.Drawing.Color.White;
            this.optSaveButt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.optSaveButt.Location = new System.Drawing.Point(12, 62);
            this.optSaveButt.MinimumSize = new System.Drawing.Size(92, 23);
            this.optSaveButt.Name = "optSaveButt";
            this.optSaveButt.Size = new System.Drawing.Size(92, 23);
            this.optSaveButt.TabIndex = 19;
            this.optSaveButt.Text = "Save";
            this.optSaveButt.UseVisualStyleBackColor = false;
            this.optSaveButt.Click += new System.EventHandler(this.optSaveButt_Click);
            // 
            // optExitButt
            // 
            this.optExitButt.BackColor = System.Drawing.Color.White;
            this.optExitButt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.optExitButt.Location = new System.Drawing.Point(180, 62);
            this.optExitButt.Name = "optExitButt";
            this.optExitButt.Size = new System.Drawing.Size(92, 23);
            this.optExitButt.TabIndex = 20;
            this.optExitButt.Text = "Exit";
            this.optExitButt.UseVisualStyleBackColor = false;
            this.optExitButt.Click += new System.EventHandler(this.optExitButt_Click);
            // 
            // relBCNDCheckb
            // 
            this.relBCNDCheckb.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.relBCNDCheckb.Location = new System.Drawing.Point(12, 12);
            this.relBCNDCheckb.Name = "relBCNDCheckb";
            this.relBCNDCheckb.Size = new System.Drawing.Size(260, 17);
            this.relBCNDCheckb.TabIndex = 21;
            this.relBCNDCheckb.Text = "Branch Conditional Relative Addressing";
            this.relBCNDCheckb.UseVisualStyleBackColor = true;
            // 
            // startAddr
            // 
            this.startAddr.Location = new System.Drawing.Point(110, 35);
            this.startAddr.Name = "startAddr";
            this.startAddr.Size = new System.Drawing.Size(162, 20);
            this.startAddr.TabIndex = 22;
            this.startAddr.Text = "00010000";
            this.startAddr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 17);
            this.label1.TabIndex = 23;
            this.label1.Text = "Start Address";
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 97);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.startAddr);
            this.Controls.Add(this.relBCNDCheckb);
            this.Controls.Add(this.optExitButt);
            this.Controls.Add(this.optSaveButt);
            this.MaximumSize = new System.Drawing.Size(300, 300);
            this.Name = "OptionsForm";
            this.Text = "NC Memory Viewer Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button optSaveButt;
        private System.Windows.Forms.Button optExitButt;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.CheckBox relBCNDCheckb;
        private System.Windows.Forms.TextBox startAddr;
        private System.Windows.Forms.Label label1;
    }
}