namespace OmoOmotegaki.Forms
{
    partial class KarteSelectingDialog
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
            this.btnOK = new System.Windows.Forms.Button();
            this._listbox1 = new System.Windows.Forms.ListBox();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnLoadXcllKrbn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.btnOK.Location = new System.Drawing.Point(150, 411);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(106, 54);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // _listbox1
            // 
            this._listbox1.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this._listbox1.FormattingEnabled = true;
            this._listbox1.ItemHeight = 16;
            this._listbox1.Location = new System.Drawing.Point(26, 102);
            this._listbox1.Name = "_listbox1";
            this._listbox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this._listbox1.Size = new System.Drawing.Size(230, 244);
            this._listbox1.TabIndex = 1;
            this._listbox1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this._listbox1_PreviewKeyDown);
            // 
            // txtInput
            // 
            this.txtInput.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.txtInput.Location = new System.Drawing.Point(26, 40);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(230, 23);
            this.txtInput.TabIndex = 2;
            this.txtInput.WordWrap = false;
            this.txtInput.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.textBox1_PreviewKeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "カルテ番号複数選択";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.label2.Location = new System.Drawing.Point(52, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "↓　ENTERキーで追加　↓";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.btnCancel.Location = new System.Drawing.Point(26, 411);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(106, 54);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.label3.Location = new System.Drawing.Point(31, 360);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(223, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "番号を選択してDeleteキーで削除";
            // 
            // btnLoadXcllKrbn
            // 
            this.btnLoadXcllKrbn.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.btnLoadXcllKrbn.Location = new System.Drawing.Point(262, 102);
            this.btnLoadXcllKrbn.Name = "btnLoadXcllKrbn";
            this.btnLoadXcllKrbn.Size = new System.Drawing.Size(132, 69);
            this.btnLoadXcllKrbn.TabIndex = 7;
            this.btnLoadXcllKrbn.Text = "診療録ソフト\r\n出力ファイル\r\nから読み込む";
            this.btnLoadXcllKrbn.UseVisualStyleBackColor = true;
            this.btnLoadXcllKrbn.Click += new System.EventHandler(this.btnLoadXcllKrbn_Click);
            // 
            // KarteSelectingDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(407, 474);
            this.Controls.Add(this.btnLoadXcllKrbn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this._listbox1);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KarteSelectingDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "KarteSelectingDialog";
            this.Shown += new System.EventHandler(this.KarteSelectingDialog_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ListBox _listbox1;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnLoadXcllKrbn;
    }
}