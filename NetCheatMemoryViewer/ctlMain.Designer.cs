namespace NetCheatMemoryViewer
{
    partial class ctlMain
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bytesBox = new System.Windows.Forms.RichTextBox();
            this.addrBox = new System.Windows.Forms.RichTextBox();
            this.textBox = new System.Windows.Forms.RichTextBox();
            this.asmBox = new System.Windows.Forms.ListBox();
            this.autoRefCB = new System.Windows.Forms.CheckBox();
            this.refreshButt = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.watchAdd = new System.Windows.Forms.Button();
            this.watchRem = new System.Windows.Forms.Button();
            this.watchSave = new System.Windows.Forms.Button();
            this.watchLoad = new System.Windows.Forms.Button();
            this.watchList = new System.Windows.Forms.DataGridView();
            this.addrCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.typeCol = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.valCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.copyRaw = new System.Windows.Forms.Button();
            this.optionsButt = new System.Windows.Forms.Button();
            this.gotoAddrCB = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.watchList)).BeginInit();
            this.SuspendLayout();
            // 
            // bytesBox
            // 
            this.bytesBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.bytesBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bytesBox.Location = new System.Drawing.Point(86, 33);
            this.bytesBox.Name = "bytesBox";
            this.bytesBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.bytesBox.Size = new System.Drawing.Size(73, 512);
            this.bytesBox.TabIndex = 0;
            this.bytesBox.Tag = "HEX";
            this.bytesBox.Text = "01020304\n10111213\n20202020\n";
            // 
            // addrBox
            // 
            this.addrBox.BackColor = System.Drawing.Color.White;
            this.addrBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.addrBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addrBox.Location = new System.Drawing.Point(3, 33);
            this.addrBox.Name = "addrBox";
            this.addrBox.ReadOnly = true;
            this.addrBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.addrBox.Size = new System.Drawing.Size(83, 512);
            this.addrBox.TabIndex = 1;
            this.addrBox.Tag = "";
            this.addrBox.Text = "01020304\n10111213\n20202020\n";
            // 
            // textBox
            // 
            this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox.Location = new System.Drawing.Point(159, 33);
            this.textBox.Name = "textBox";
            this.textBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(36, 512);
            this.textBox.TabIndex = 2;
            this.textBox.Tag = "TEXT";
            this.textBox.Text = "0102\n1011\n2020";
            // 
            // asmBox
            // 
            this.asmBox.BackColor = System.Drawing.Color.White;
            this.asmBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.asmBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.asmBox.FormattingEnabled = true;
            this.asmBox.ItemHeight = 16;
            this.asmBox.Items.AddRange(new object[] {
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop",
            "nop"});
            this.asmBox.Location = new System.Drawing.Point(195, 32);
            this.asmBox.Name = "asmBox";
            this.asmBox.Size = new System.Drawing.Size(213, 512);
            this.asmBox.TabIndex = 3;
            this.asmBox.DoubleClick += new System.EventHandler(this.asmBox_DoubleClick);
            // 
            // autoRefCB
            // 
            this.autoRefCB.Location = new System.Drawing.Point(509, 524);
            this.autoRefCB.Name = "autoRefCB";
            this.autoRefCB.Size = new System.Drawing.Size(101, 17);
            this.autoRefCB.TabIndex = 7;
            this.autoRefCB.Text = "Auto Refresh";
            this.autoRefCB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.autoRefCB.UseVisualStyleBackColor = true;
            this.autoRefCB.CheckedChanged += new System.EventHandler(this.autoRefCB_CheckedChanged);
            // 
            // refreshButt
            // 
            this.refreshButt.BackColor = System.Drawing.Color.White;
            this.refreshButt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.refreshButt.Location = new System.Drawing.Point(509, 492);
            this.refreshButt.Name = "refreshButt";
            this.refreshButt.Size = new System.Drawing.Size(101, 23);
            this.refreshButt.TabIndex = 8;
            this.refreshButt.Text = "Refresh";
            this.refreshButt.UseVisualStyleBackColor = false;
            this.refreshButt.Click += new System.EventHandler(this.refreshButt_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(411, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(297, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Watch List";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // watchAdd
            // 
            this.watchAdd.BackColor = System.Drawing.Color.White;
            this.watchAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.watchAdd.Location = new System.Drawing.Point(616, 491);
            this.watchAdd.Name = "watchAdd";
            this.watchAdd.Size = new System.Drawing.Size(92, 23);
            this.watchAdd.TabIndex = 11;
            this.watchAdd.Text = "Add Watch";
            this.watchAdd.UseVisualStyleBackColor = false;
            this.watchAdd.Click += new System.EventHandler(this.watchAdd_Click);
            // 
            // watchRem
            // 
            this.watchRem.BackColor = System.Drawing.Color.White;
            this.watchRem.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.watchRem.Location = new System.Drawing.Point(411, 491);
            this.watchRem.Name = "watchRem";
            this.watchRem.Size = new System.Drawing.Size(92, 23);
            this.watchRem.TabIndex = 12;
            this.watchRem.Text = "Delete Watch";
            this.watchRem.UseVisualStyleBackColor = false;
            this.watchRem.Click += new System.EventHandler(this.watchRem_Click);
            // 
            // watchSave
            // 
            this.watchSave.BackColor = System.Drawing.Color.White;
            this.watchSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.watchSave.Location = new System.Drawing.Point(411, 520);
            this.watchSave.Name = "watchSave";
            this.watchSave.Size = new System.Drawing.Size(92, 23);
            this.watchSave.TabIndex = 15;
            this.watchSave.Text = "Save Watch";
            this.watchSave.UseVisualStyleBackColor = false;
            this.watchSave.Click += new System.EventHandler(this.watchSave_Click);
            // 
            // watchLoad
            // 
            this.watchLoad.BackColor = System.Drawing.Color.White;
            this.watchLoad.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.watchLoad.Location = new System.Drawing.Point(616, 520);
            this.watchLoad.Name = "watchLoad";
            this.watchLoad.Size = new System.Drawing.Size(92, 23);
            this.watchLoad.TabIndex = 16;
            this.watchLoad.Text = "Load Watch";
            this.watchLoad.UseVisualStyleBackColor = false;
            this.watchLoad.Click += new System.EventHandler(this.watchLoad_Click);
            // 
            // watchList
            // 
            this.watchList.AllowUserToAddRows = false;
            this.watchList.AllowUserToDeleteRows = false;
            this.watchList.AllowUserToResizeRows = false;
            this.watchList.BackgroundColor = System.Drawing.Color.White;
            this.watchList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.watchList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.watchList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.addrCol,
            this.typeCol,
            this.valCol});
            this.watchList.GridColor = System.Drawing.Color.White;
            this.watchList.Location = new System.Drawing.Point(411, 20);
            this.watchList.Name = "watchList";
            this.watchList.RowHeadersVisible = false;
            this.watchList.RowHeadersWidth = 5;
            this.watchList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.watchList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.watchList.Size = new System.Drawing.Size(297, 465);
            this.watchList.TabIndex = 12;
            this.watchList.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.watchList_CellEndEdit);
            // 
            // addrCol
            // 
            this.addrCol.HeaderText = "Address";
            this.addrCol.Name = "addrCol";
            this.addrCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.addrCol.Width = 60;
            // 
            // typeCol
            // 
            this.typeCol.HeaderText = "Type";
            this.typeCol.Items.AddRange(new object[] {
            "char",
            "short",
            "int",
            "long",
            "string",
            "float"});
            this.typeCol.Name = "typeCol";
            this.typeCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.typeCol.Width = 60;
            // 
            // valCol
            // 
            this.valCol.HeaderText = "Value";
            this.valCol.Name = "valCol";
            this.valCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.valCol.Width = 170;
            // 
            // copyRaw
            // 
            this.copyRaw.BackColor = System.Drawing.Color.White;
            this.copyRaw.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.copyRaw.Location = new System.Drawing.Point(213, 5);
            this.copyRaw.Name = "copyRaw";
            this.copyRaw.Size = new System.Drawing.Size(92, 23);
            this.copyRaw.TabIndex = 17;
            this.copyRaw.Text = "Copy Raw";
            this.copyRaw.UseVisualStyleBackColor = false;
            this.copyRaw.Click += new System.EventHandler(this.copyRaw_Click);
            // 
            // optionsButt
            // 
            this.optionsButt.BackColor = System.Drawing.Color.White;
            this.optionsButt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.optionsButt.Location = new System.Drawing.Point(311, 5);
            this.optionsButt.Name = "optionsButt";
            this.optionsButt.Size = new System.Drawing.Size(92, 23);
            this.optionsButt.TabIndex = 18;
            this.optionsButt.Text = "Options";
            this.optionsButt.UseVisualStyleBackColor = false;
            this.optionsButt.Click += new System.EventHandler(this.optionsButt_Click);
            // 
            // gotoAddrCB
            // 
            this.gotoAddrCB.FormattingEnabled = true;
            this.gotoAddrCB.Location = new System.Drawing.Point(3, 7);
            this.gotoAddrCB.Name = "gotoAddrCB";
            this.gotoAddrCB.Size = new System.Drawing.Size(121, 21);
            this.gotoAddrCB.TabIndex = 19;
            this.gotoAddrCB.SelectedIndexChanged += new System.EventHandler(this.gotoAddrCB_SelectedIndexChanged);
            this.gotoAddrCB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gotoAddrCB_KeyDown);
            // 
            // ctlMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gotoAddrCB);
            this.Controls.Add(this.optionsButt);
            this.Controls.Add(this.copyRaw);
            this.Controls.Add(this.watchList);
            this.Controls.Add(this.watchLoad);
            this.Controls.Add(this.watchSave);
            this.Controls.Add(this.watchRem);
            this.Controls.Add(this.watchAdd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.refreshButt);
            this.Controls.Add(this.autoRefCB);
            this.Controls.Add(this.asmBox);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.addrBox);
            this.Controls.Add(this.bytesBox);
            this.Name = "ctlMain";
            this.Size = new System.Drawing.Size(711, 549);
            this.Load += new System.EventHandler(this.ctlMain_Load);
            this.Resize += new System.EventHandler(this.ctlMain_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.watchList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox bytesBox;
        private System.Windows.Forms.RichTextBox addrBox;
        private System.Windows.Forms.RichTextBox textBox;
        private System.Windows.Forms.ListBox asmBox;
        private System.Windows.Forms.CheckBox autoRefCB;
        private System.Windows.Forms.Button refreshButt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button watchAdd;
        private System.Windows.Forms.Button watchRem;
        private System.Windows.Forms.Button watchSave;
        private System.Windows.Forms.Button watchLoad;
        private System.Windows.Forms.DataGridView watchList;
        private System.Windows.Forms.DataGridViewTextBoxColumn addrCol;
        private System.Windows.Forms.DataGridViewComboBoxColumn typeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn valCol;
        private System.Windows.Forms.Button copyRaw;
        private System.Windows.Forms.Button optionsButt;
        private System.Windows.Forms.ComboBox gotoAddrCB;
    }
}
