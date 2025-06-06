namespace OmoSeitokuEreceipt.ER.Controls
{
    partial class SisikiInputControl
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

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.haSettingController = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnKaijo = new System.Windows.Forms.Button();
            this.lstBubun = new System.Windows.Forms.ListBox();
            this.lblBubun = new System.Windows.Forms.Label();
            this.lstJyoutai = new System.Windows.Forms.ListBox();
            this.lblJyoutai = new System.Windows.Forms.Label();
            this.lblCurrentInfo = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageKanni = new System.Windows.Forms.TabPage();
            this.tabPageGensyo = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.haSettingController.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageGensyo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // haSettingController
            // 
            this.haSettingController.Controls.Add(this.label5);
            this.haSettingController.Controls.Add(this.label4);
            this.haSettingController.Controls.Add(this.label2);
            this.haSettingController.Controls.Add(this.label3);
            this.haSettingController.Controls.Add(this.btnKaijo);
            this.haSettingController.Controls.Add(this.lstBubun);
            this.haSettingController.Controls.Add(this.lblBubun);
            this.haSettingController.Controls.Add(this.lstJyoutai);
            this.haSettingController.Controls.Add(this.lblJyoutai);
            this.haSettingController.Controls.Add(this.lblCurrentInfo);
            this.haSettingController.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.haSettingController.Location = new System.Drawing.Point(0, 193);
            this.haSettingController.Name = "haSettingController";
            this.haSettingController.Size = new System.Drawing.Size(455, 198);
            this.haSettingController.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.ForeColor = System.Drawing.Color.MediumBlue;
            this.label3.Location = new System.Drawing.Point(15, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(316, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "* 歯種ボタンをクリックで ｛ 削除・状態・部分 ｝ 変更";
            // 
            // btnKaijo
            // 
            this.btnKaijo.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnKaijo.Location = new System.Drawing.Point(18, 104);
            this.btnKaijo.Name = "btnKaijo";
            this.btnKaijo.Size = new System.Drawing.Size(117, 49);
            this.btnKaijo.TabIndex = 2;
            this.btnKaijo.Text = "削除 (&D)";
            this.btnKaijo.UseVisualStyleBackColor = true;
            this.btnKaijo.Click += new System.EventHandler(this.BtnKaijo_Click);
            // 
            // lstBubun
            // 
            this.lstBubun.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstBubun.BackColor = System.Drawing.Color.LavenderBlush;
            this.lstBubun.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lstBubun.FormattingEnabled = true;
            this.lstBubun.ItemHeight = 15;
            this.lstBubun.Location = new System.Drawing.Point(281, 22);
            this.lstBubun.Name = "lstBubun";
            this.lstBubun.Size = new System.Drawing.Size(170, 139);
            this.lstBubun.TabIndex = 6;
            this.lstBubun.SelectedIndexChanged += new System.EventHandler(this.LstBubun_SelectedIndexChanged);
            // 
            // lblBubun
            // 
            this.lblBubun.AutoSize = true;
            this.lblBubun.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblBubun.Location = new System.Drawing.Point(279, 2);
            this.lblBubun.Name = "lblBubun";
            this.lblBubun.Size = new System.Drawing.Size(39, 15);
            this.lblBubun.TabIndex = 5;
            this.lblBubun.Text = "部分";
            // 
            // lstJyoutai
            // 
            this.lstJyoutai.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lstJyoutai.BackColor = System.Drawing.Color.PaleTurquoise;
            this.lstJyoutai.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lstJyoutai.FormattingEnabled = true;
            this.lstJyoutai.ItemHeight = 15;
            this.lstJyoutai.Location = new System.Drawing.Point(152, 22);
            this.lstJyoutai.Name = "lstJyoutai";
            this.lstJyoutai.Size = new System.Drawing.Size(121, 139);
            this.lstJyoutai.TabIndex = 4;
            this.lstJyoutai.SelectedIndexChanged += new System.EventHandler(this.LstJyoutai_SelectedIndexChanged);
            // 
            // lblJyoutai
            // 
            this.lblJyoutai.AutoSize = true;
            this.lblJyoutai.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblJyoutai.Location = new System.Drawing.Point(150, 2);
            this.lblJyoutai.Name = "lblJyoutai";
            this.lblJyoutai.Size = new System.Drawing.Size(39, 15);
            this.lblJyoutai.TabIndex = 3;
            this.lblJyoutai.Text = "状態";
            // 
            // lblCurrentInfo
            // 
            this.lblCurrentInfo.AutoSize = true;
            this.lblCurrentInfo.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCurrentInfo.Location = new System.Drawing.Point(3, 21);
            this.lblCurrentInfo.Name = "lblCurrentInfo";
            this.lblCurrentInfo.Size = new System.Drawing.Size(145, 24);
            this.lblCurrentInfo.TabIndex = 0;
            this.lblCurrentInfo.Text = "lblCurrentInfo";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageKanni);
            this.tabControl1.Controls.Add(this.tabPageGensyo);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(455, 193);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageKanni
            // 
            this.tabPageKanni.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tabPageKanni.Location = new System.Drawing.Point(4, 21);
            this.tabPageKanni.Name = "tabPageKanni";
            this.tabPageKanni.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageKanni.Size = new System.Drawing.Size(447, 168);
            this.tabPageKanni.TabIndex = 0;
            this.tabPageKanni.Text = "簡易表示";
            this.tabPageKanni.UseVisualStyleBackColor = true;
            // 
            // tabPageGensyo
            // 
            this.tabPageGensyo.Controls.Add(this.pictureBox1);
            this.tabPageGensyo.Location = new System.Drawing.Point(4, 21);
            this.tabPageGensyo.Name = "tabPageGensyo";
            this.tabPageGensyo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGensyo.Size = new System.Drawing.Size(447, 168);
            this.tabPageGensyo.TabIndex = 1;
            this.tabPageGensyo.Text = "現症図";
            this.tabPageGensyo.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(441, 161);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.MediumBlue;
            this.label2.Location = new System.Drawing.Point(161, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "(* 左クリック)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.MediumBlue;
            this.label4.Location = new System.Drawing.Point(295, 163);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "(* 右クリック)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.MediumBlue;
            this.label5.Location = new System.Drawing.Point(41, 163);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "(* 中クリック)";
            // 
            // SisikiInputControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.haSettingController);
            this.MinimumSize = new System.Drawing.Size(455, 391);
            this.Name = "SisikiInputControl";
            this.Size = new System.Drawing.Size(455, 391);
            this.haSettingController.ResumeLayout(false);
            this.haSettingController.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageGensyo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel haSettingController;
        private System.Windows.Forms.ListBox lstBubun;
        private System.Windows.Forms.Label lblBubun;
        private System.Windows.Forms.ListBox lstJyoutai;
        private System.Windows.Forms.Label lblJyoutai;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageKanni;
        private System.Windows.Forms.TabPage tabPageGensyo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnKaijo;
        private System.Windows.Forms.Label lblCurrentInfo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
    }
}
