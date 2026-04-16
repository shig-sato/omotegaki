namespace OmoOmotegaki.Yahara.Views
{
    partial class OptionDialog
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
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.numOneKarte = new System.Windows.Forms.NumericUpDown();
            this.dateStart = new System.Windows.Forms.DateTimePicker();
            this.dateEnd = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.numLimit = new System.Windows.Forms.NumericUpDown();
            this.panel5 = new System.Windows.Forms.Panel();
            this.chkZenKikan = new System.Windows.Forms.CheckBox();
            this.radKarteAll = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.chkShinryoujoBun = new System.Windows.Forms.CheckBox();
            this.chkShinryoujoHon = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.numStartKarteNumber = new System.Windows.Forms.NumericUpDown();
            this.radKarteOne = new System.Windows.Forms.RadioButton();
            this.radKarteLimit = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.btnSelectOutputFolder = new System.Windows.Forms.Button();
            this.btnSelectInputFolder = new System.Windows.Forms.Button();
            this.txtInputFolder = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numOneKarte)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLimit)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStartKarteNumber)).BeginInit();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnStart.Location = new System.Drawing.Point(400, 449);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(169, 62);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "変換開始";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(310, 488);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // numOneKarte
            // 
            this.numOneKarte.Location = new System.Drawing.Point(118, 119);
            this.numOneKarte.Maximum = new decimal(new int[] {
            40000,
            0,
            0,
            0});
            this.numOneKarte.Name = "numOneKarte";
            this.numOneKarte.Size = new System.Drawing.Size(81, 19);
            this.numOneKarte.TabIndex = 2;
            // 
            // dateStart
            // 
            this.dateStart.Location = new System.Drawing.Point(272, 26);
            this.dateStart.Name = "dateStart";
            this.dateStart.Size = new System.Drawing.Size(169, 19);
            this.dateStart.TabIndex = 4;
            // 
            // dateEnd
            // 
            this.dateEnd.Location = new System.Drawing.Point(272, 50);
            this.dateEnd.Name = "dateEnd";
            this.dateEnd.Size = new System.Drawing.Size(169, 19);
            this.dateEnd.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(225, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "開始日";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(225, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "終了日";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "期間指定";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkGray;
            this.panel1.Controls.Add(this.panel7);
            this.panel1.Location = new System.Drawing.Point(114, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1, 239);
            this.panel1.TabIndex = 9;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.DarkGray;
            this.panel7.Location = new System.Drawing.Point(17, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1, 239);
            this.panel7.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Location = new System.Drawing.Point(12, 87);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(475, 2);
            this.panel2.TabIndex = 10;
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackColor = System.Drawing.Color.Black;
            this.panel4.Location = new System.Drawing.Point(12, 12);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(475, 2);
            this.panel4.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 106);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "出力カルテ";
            // 
            // numLimit
            // 
            this.numLimit.Location = new System.Drawing.Point(262, 78);
            this.numLimit.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numLimit.Name = "numLimit";
            this.numLimit.Size = new System.Drawing.Size(81, 19);
            this.numLimit.TabIndex = 12;
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.BackColor = System.Drawing.Color.Black;
            this.panel5.Location = new System.Drawing.Point(12, 251);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(475, 2);
            this.panel5.TabIndex = 14;
            // 
            // chkZenKikan
            // 
            this.chkZenKikan.AutoSize = true;
            this.chkZenKikan.Location = new System.Drawing.Point(124, 25);
            this.chkZenKikan.Name = "chkZenKikan";
            this.chkZenKikan.Size = new System.Drawing.Size(60, 16);
            this.chkZenKikan.TabIndex = 15;
            this.chkZenKikan.Text = "全期間";
            this.chkZenKikan.UseVisualStyleBackColor = true;
            this.chkZenKikan.CheckedChanged += new System.EventHandler(this.chkZenKikan_CheckedChanged);
            // 
            // radKarteAll
            // 
            this.radKarteAll.AutoSize = true;
            this.radKarteAll.Location = new System.Drawing.Point(3, 40);
            this.radKarteAll.Name = "radKarteAll";
            this.radKarteAll.Size = new System.Drawing.Size(52, 16);
            this.radKarteAll.TabIndex = 18;
            this.radKarteAll.TabStop = true;
            this.radKarteAll.Text = "すべて";
            this.radKarteAll.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.panel8);
            this.panel3.Controls.Add(this.chkShinryoujoBun);
            this.panel3.Controls.Add(this.chkShinryoujoHon);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.numStartKarteNumber);
            this.panel3.Controls.Add(this.radKarteOne);
            this.panel3.Controls.Add(this.radKarteLimit);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.radKarteAll);
            this.panel3.Controls.Add(this.numLimit);
            this.panel3.Controls.Add(this.numOneKarte);
            this.panel3.Location = new System.Drawing.Point(124, 103);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(356, 142);
            this.panel3.TabIndex = 19;
            // 
            // panel8
            // 
            this.panel8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel8.BackColor = System.Drawing.Color.DarkGray;
            this.panel8.Controls.Add(this.panel9);
            this.panel8.Location = new System.Drawing.Point(0, 26);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(356, 1);
            this.panel8.TabIndex = 28;
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.DarkGray;
            this.panel9.Location = new System.Drawing.Point(17, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(1, 239);
            this.panel9.TabIndex = 10;
            // 
            // chkShinryoujoBun
            // 
            this.chkShinryoujoBun.AutoSize = true;
            this.chkShinryoujoBun.Checked = true;
            this.chkShinryoujoBun.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShinryoujoBun.Location = new System.Drawing.Point(117, 0);
            this.chkShinryoujoBun.Name = "chkShinryoujoBun";
            this.chkShinryoujoBun.Size = new System.Drawing.Size(48, 16);
            this.chkShinryoujoBun.TabIndex = 27;
            this.chkShinryoujoBun.Text = "分院";
            this.chkShinryoujoBun.UseVisualStyleBackColor = true;
            this.chkShinryoujoBun.CheckedChanged += new System.EventHandler(this.chkShinryoujoBun_CheckedChanged);
            // 
            // chkShinryoujoHon
            // 
            this.chkShinryoujoHon.AutoSize = true;
            this.chkShinryoujoHon.Checked = true;
            this.chkShinryoujoHon.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShinryoujoHon.Location = new System.Drawing.Point(58, 0);
            this.chkShinryoujoHon.Name = "chkShinryoujoHon";
            this.chkShinryoujoHon.Size = new System.Drawing.Size(48, 16);
            this.chkShinryoujoHon.TabIndex = 26;
            this.chkShinryoujoHon.Text = "本院";
            this.chkShinryoujoHon.UseVisualStyleBackColor = true;
            this.chkShinryoujoHon.CheckedChanged += new System.EventHandler(this.chkShinryoujoHon_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(222, 99);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 25;
            this.label10.Text = "（ 0 で無限）";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(215, 80);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 24;
            this.label9.Text = "出力数";
            // 
            // numStartKarteNumber
            // 
            this.numStartKarteNumber.Location = new System.Drawing.Point(108, 78);
            this.numStartKarteNumber.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numStartKarteNumber.Name = "numStartKarteNumber";
            this.numStartKarteNumber.Size = new System.Drawing.Size(81, 19);
            this.numStartKarteNumber.TabIndex = 23;
            this.numStartKarteNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // radKarteOne
            // 
            this.radKarteOne.AutoSize = true;
            this.radKarteOne.Location = new System.Drawing.Point(3, 119);
            this.radKarteOne.Name = "radKarteOne";
            this.radKarteOne.Size = new System.Drawing.Size(109, 16);
            this.radKarteOne.TabIndex = 20;
            this.radKarteOne.TabStop = true;
            this.radKarteOne.Text = "単一のカルテ番号";
            this.radKarteOne.UseVisualStyleBackColor = true;
            this.radKarteOne.CheckedChanged += new System.EventHandler(this.radKarteOne_CheckedChanged);
            // 
            // radKarteLimit
            // 
            this.radKarteLimit.AutoSize = true;
            this.radKarteLimit.Location = new System.Drawing.Point(3, 78);
            this.radKarteLimit.Name = "radKarteLimit";
            this.radKarteLimit.Size = new System.Drawing.Size(99, 16);
            this.radKarteLimit.TabIndex = 19;
            this.radKarteLimit.TabStop = true;
            this.radKarteLimit.Text = "開始カルテ番号";
            this.radKarteLimit.UseVisualStyleBackColor = true;
            this.radKarteLimit.CheckedChanged += new System.EventHandler(this.radKarteLimit_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 22;
            this.label1.Text = "診療所";
            // 
            // panel6
            // 
            this.panel6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel6.Controls.Add(this.panel4);
            this.panel6.Controls.Add(this.dateStart);
            this.panel6.Controls.Add(this.dateEnd);
            this.panel6.Controls.Add(this.panel3);
            this.panel6.Controls.Add(this.label2);
            this.panel6.Controls.Add(this.chkZenKikan);
            this.panel6.Controls.Add(this.label3);
            this.panel6.Controls.Add(this.panel5);
            this.panel6.Controls.Add(this.label4);
            this.panel6.Controls.Add(this.label5);
            this.panel6.Controls.Add(this.panel1);
            this.panel6.Controls.Add(this.panel2);
            this.panel6.Location = new System.Drawing.Point(71, 159);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(498, 271);
            this.panel6.TabIndex = 23;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 169);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 24;
            this.label6.Text = "出力範囲";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 86);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 25;
            this.label7.Text = "出力先";
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputFolder.Location = new System.Drawing.Point(71, 104);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(498, 19);
            this.txtOutputFolder.TabIndex = 26;
            // 
            // btnSelectOutputFolder
            // 
            this.btnSelectOutputFolder.Location = new System.Drawing.Point(71, 81);
            this.btnSelectOutputFolder.Name = "btnSelectOutputFolder";
            this.btnSelectOutputFolder.Size = new System.Drawing.Size(115, 23);
            this.btnSelectOutputFolder.TabIndex = 27;
            this.btnSelectOutputFolder.Text = "フォルダを選択";
            this.btnSelectOutputFolder.UseVisualStyleBackColor = true;
            this.btnSelectOutputFolder.Click += new System.EventHandler(this.btnSelectOutputFolder_Click);
            // 
            // btnSelectInputFolder
            // 
            this.btnSelectInputFolder.Location = new System.Drawing.Point(71, 12);
            this.btnSelectInputFolder.Name = "btnSelectInputFolder";
            this.btnSelectInputFolder.Size = new System.Drawing.Size(115, 23);
            this.btnSelectInputFolder.TabIndex = 30;
            this.btnSelectInputFolder.Text = "フォルダを選択";
            this.btnSelectInputFolder.UseVisualStyleBackColor = true;
            this.btnSelectInputFolder.Click += new System.EventHandler(this.btnSelectInputFolder_Click);
            // 
            // txtInputFolder
            // 
            this.txtInputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputFolder.Location = new System.Drawing.Point(71, 35);
            this.txtInputFolder.Name = "txtInputFolder";
            this.txtInputFolder.Size = new System.Drawing.Size(498, 19);
            this.txtInputFolder.TabIndex = 29;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 28;
            this.label8.Text = "入力元";
            // 
            // OptionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 523);
            this.Controls.Add(this.btnSelectInputFolder);
            this.Controls.Add(this.txtInputFolder);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnSelectOutputFolder);
            this.Controls.Add(this.txtOutputFolder);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnStart);
            this.Name = "OptionDialog";
            this.Text = "ヤハラ変換オプション";
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.OptionDialog_PreviewKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.numOneKarte)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numLimit)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numStartKarteNumber)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.NumericUpDown numOneKarte;
        private System.Windows.Forms.DateTimePicker dateStart;
        private System.Windows.Forms.DateTimePicker dateEnd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numLimit;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.CheckBox chkZenKikan;
        private System.Windows.Forms.RadioButton radKarteAll;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton radKarteOne;
        private System.Windows.Forms.RadioButton radKarteLimit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.Button btnSelectOutputFolder;
        private System.Windows.Forms.Button btnSelectInputFolder;
        private System.Windows.Forms.TextBox txtInputFolder;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numStartKarteNumber;
        private System.Windows.Forms.CheckBox chkShinryoujoBun;
        private System.Windows.Forms.CheckBox chkShinryoujoHon;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel9;
    }
}