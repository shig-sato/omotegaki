namespace OmoOmotegaki.Forms
{
    partial class KartePrintSettingDialog
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkShowByoumei = new System.Windows.Forms.CheckBox();
            this.grpSinryou = new System.Windows.Forms.GroupBox();
            this.grpOptionsSSS = new System.Windows.Forms.GroupBox();
            this.chkSSSSyochiName = new System.Windows.Forms.CheckBox();
            this.chkSSSCheckPrint = new System.Windows.Forms.CheckBox();
            this.lblMiraiinSeikyuu = new System.Windows.Forms.Label();
            this.cmbMiraiinSeikyuu = new System.Windows.Forms.ComboBox();
            this.lblHokenSinryou = new System.Windows.Forms.Label();
            this.cmbHokenSinryou = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkShowKarteName = new System.Windows.Forms.CheckBox();
            this.grpKarteInfo = new System.Windows.Forms.GroupBox();
            this.chkShowKarteKikan = new System.Windows.Forms.CheckBox();
            this.chkShowKarteNumber = new System.Windows.Forms.CheckBox();
            this.grpSinryou.SuspendLayout();
            this.grpOptionsSSS.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpKarteInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(132, 333);
            this.btnOK.Margin = new System.Windows.Forms.Padding(2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(116, 28);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK (&O)";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(281, 333);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(116, 28);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "キャンセル (&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkShowByoumei
            // 
            this.chkShowByoumei.AutoSize = true;
            this.chkShowByoumei.Location = new System.Drawing.Point(19, 29);
            this.chkShowByoumei.Margin = new System.Windows.Forms.Padding(2);
            this.chkShowByoumei.Name = "chkShowByoumei";
            this.chkShowByoumei.Size = new System.Drawing.Size(100, 16);
            this.chkShowByoumei.TabIndex = 0;
            this.chkShowByoumei.Text = "病名を表示する";
            this.chkShowByoumei.UseVisualStyleBackColor = true;
            // 
            // grpSinryou
            // 
            this.grpSinryou.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSinryou.Controls.Add(this.grpOptionsSSS);
            this.grpSinryou.Controls.Add(this.chkShowByoumei);
            this.grpSinryou.Location = new System.Drawing.Point(5, 213);
            this.grpSinryou.Margin = new System.Windows.Forms.Padding(2);
            this.grpSinryou.Name = "grpSinryou";
            this.grpSinryou.Padding = new System.Windows.Forms.Padding(2);
            this.grpSinryou.Size = new System.Drawing.Size(396, 108);
            this.grpSinryou.TabIndex = 2;
            this.grpSinryou.TabStop = false;
            this.grpSinryou.Text = "診療行為 表示設定";
            // 
            // grpOptionsSSS
            // 
            this.grpOptionsSSS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpOptionsSSS.Controls.Add(this.chkSSSSyochiName);
            this.grpOptionsSSS.Controls.Add(this.chkSSSCheckPrint);
            this.grpOptionsSSS.Location = new System.Drawing.Point(161, 14);
            this.grpOptionsSSS.Margin = new System.Windows.Forms.Padding(2);
            this.grpOptionsSSS.Name = "grpOptionsSSS";
            this.grpOptionsSSS.Padding = new System.Windows.Forms.Padding(2);
            this.grpOptionsSSS.Size = new System.Drawing.Size(230, 87);
            this.grpOptionsSSS.TabIndex = 1;
            this.grpOptionsSSS.TabStop = false;
            this.grpOptionsSSS.Text = "診療録作成ソフト 連携";
            // 
            // chkSSSSyochiName
            // 
            this.chkSSSSyochiName.AutoSize = true;
            this.chkSSSSyochiName.Location = new System.Drawing.Point(23, 28);
            this.chkSSSSyochiName.Margin = new System.Windows.Forms.Padding(2);
            this.chkSSSSyochiName.Name = "chkSSSSyochiName";
            this.chkSSSSyochiName.Size = new System.Drawing.Size(129, 16);
            this.chkSSSSyochiName.TabIndex = 0;
            this.chkSSSSyochiName.Text = "ソフトの処置名で印字";
            this.chkSSSSyochiName.UseVisualStyleBackColor = true;
            // 
            // chkSSSCheckPrint
            // 
            this.chkSSSCheckPrint.AutoSize = true;
            this.chkSSSCheckPrint.Location = new System.Drawing.Point(23, 53);
            this.chkSSSCheckPrint.Margin = new System.Windows.Forms.Padding(2);
            this.chkSSSCheckPrint.Name = "chkSSSCheckPrint";
            this.chkSSSCheckPrint.Size = new System.Drawing.Size(112, 16);
            this.chkSSSCheckPrint.TabIndex = 1;
            this.chkSSSCheckPrint.Text = "チェック印刷を印字";
            this.chkSSSCheckPrint.UseVisualStyleBackColor = true;
            // 
            // lblMiraiinSeikyuu
            // 
            this.lblMiraiinSeikyuu.AutoSize = true;
            this.lblMiraiinSeikyuu.Location = new System.Drawing.Point(27, 60);
            this.lblMiraiinSeikyuu.Name = "lblMiraiinSeikyuu";
            this.lblMiraiinSeikyuu.Size = new System.Drawing.Size(77, 14);
            this.lblMiraiinSeikyuu.TabIndex = 2;
            this.lblMiraiinSeikyuu.Text = "未来院請求";
            // 
            // cmbMiraiinSeikyuu
            // 
            this.cmbMiraiinSeikyuu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbMiraiinSeikyuu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMiraiinSeikyuu.FormattingEnabled = true;
            this.cmbMiraiinSeikyuu.Location = new System.Drawing.Point(127, 57);
            this.cmbMiraiinSeikyuu.Name = "cmbMiraiinSeikyuu";
            this.cmbMiraiinSeikyuu.Size = new System.Drawing.Size(258, 21);
            this.cmbMiraiinSeikyuu.TabIndex = 3;
            // 
            // lblHokenSinryou
            // 
            this.lblHokenSinryou.AutoSize = true;
            this.lblHokenSinryou.Location = new System.Drawing.Point(27, 28);
            this.lblHokenSinryou.Name = "lblHokenSinryou";
            this.lblHokenSinryou.Size = new System.Drawing.Size(63, 14);
            this.lblHokenSinryou.TabIndex = 0;
            this.lblHokenSinryou.Text = "保険診療";
            // 
            // cmbHokenSinryou
            // 
            this.cmbHokenSinryou.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbHokenSinryou.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHokenSinryou.FormattingEnabled = true;
            this.cmbHokenSinryou.Location = new System.Drawing.Point(127, 25);
            this.cmbHokenSinryou.Name = "cmbHokenSinryou";
            this.cmbHokenSinryou.Size = new System.Drawing.Size(258, 21);
            this.cmbHokenSinryou.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblHokenSinryou);
            this.groupBox1.Controls.Add(this.lblMiraiinSeikyuu);
            this.groupBox1.Controls.Add(this.cmbHokenSinryou);
            this.groupBox1.Controls.Add(this.cmbMiraiinSeikyuu);
            this.groupBox1.Location = new System.Drawing.Point(5, 9);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(396, 86);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "印刷対象にする診療";
            // 
            // chkShowKarteName
            // 
            this.chkShowKarteName.AutoSize = true;
            this.chkShowKarteName.Location = new System.Drawing.Point(19, 54);
            this.chkShowKarteName.Margin = new System.Windows.Forms.Padding(2);
            this.chkShowKarteName.Name = "chkShowKarteName";
            this.chkShowKarteName.Size = new System.Drawing.Size(60, 16);
            this.chkShowKarteName.TabIndex = 2;
            this.chkShowKarteName.Text = "患者名";
            this.chkShowKarteName.UseVisualStyleBackColor = true;
            // 
            // grpKarteInfo
            // 
            this.grpKarteInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpKarteInfo.Controls.Add(this.chkShowKarteKikan);
            this.grpKarteInfo.Controls.Add(this.chkShowKarteNumber);
            this.grpKarteInfo.Controls.Add(this.chkShowKarteName);
            this.grpKarteInfo.Location = new System.Drawing.Point(5, 110);
            this.grpKarteInfo.Margin = new System.Windows.Forms.Padding(2);
            this.grpKarteInfo.Name = "grpKarteInfo";
            this.grpKarteInfo.Padding = new System.Windows.Forms.Padding(2);
            this.grpKarteInfo.Size = new System.Drawing.Size(392, 84);
            this.grpKarteInfo.TabIndex = 1;
            this.grpKarteInfo.TabStop = false;
            this.grpKarteInfo.Text = "カルテ情報 表示設定";
            // 
            // chkShowKarteKikan
            // 
            this.chkShowKarteKikan.AutoSize = true;
            this.chkShowKarteKikan.Location = new System.Drawing.Point(127, 28);
            this.chkShowKarteKikan.Margin = new System.Windows.Forms.Padding(2);
            this.chkShowKarteKikan.Name = "chkShowKarteKikan";
            this.chkShowKarteKikan.Size = new System.Drawing.Size(72, 16);
            this.chkShowKarteKikan.TabIndex = 1;
            this.chkShowKarteKikan.Text = "印刷期間";
            this.chkShowKarteKikan.UseVisualStyleBackColor = true;
            // 
            // chkShowKarteNumber
            // 
            this.chkShowKarteNumber.AutoSize = true;
            this.chkShowKarteNumber.Location = new System.Drawing.Point(19, 28);
            this.chkShowKarteNumber.Margin = new System.Windows.Forms.Padding(2);
            this.chkShowKarteNumber.Name = "chkShowKarteNumber";
            this.chkShowKarteNumber.Size = new System.Drawing.Size(76, 16);
            this.chkShowKarteNumber.TabIndex = 0;
            this.chkShowKarteNumber.Text = "カルテ番号";
            this.chkShowKarteNumber.UseVisualStyleBackColor = true;
            // 
            // KartePrintSettingDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(406, 371);
            this.Controls.Add(this.grpKarteInfo);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grpSinryou);
            this.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "KartePrintSettingDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "カルテ 印刷設定";
            this.Load += new System.EventHandler(this.KartePrintSettingDialog_Load);
            this.grpSinryou.ResumeLayout(false);
            this.grpSinryou.PerformLayout();
            this.grpOptionsSSS.ResumeLayout(false);
            this.grpOptionsSSS.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpKarteInfo.ResumeLayout(false);
            this.grpKarteInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkShowByoumei;
        private System.Windows.Forms.GroupBox grpSinryou;
        private System.Windows.Forms.CheckBox chkSSSSyochiName;
        private System.Windows.Forms.GroupBox grpOptionsSSS;
        private System.Windows.Forms.CheckBox chkSSSCheckPrint;
        private System.Windows.Forms.Label lblMiraiinSeikyuu;
        private System.Windows.Forms.ComboBox cmbMiraiinSeikyuu;
        private System.Windows.Forms.Label lblHokenSinryou;
        private System.Windows.Forms.ComboBox cmbHokenSinryou;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkShowKarteName;
        private System.Windows.Forms.GroupBox grpKarteInfo;
        private System.Windows.Forms.CheckBox chkShowKarteNumber;
        private System.Windows.Forms.CheckBox chkShowKarteKikan;
    }
}