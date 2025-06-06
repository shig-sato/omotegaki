namespace OmoOmotegaki.Forms
{
    partial class KarteBatchPrintDialog
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
            this.btnPrintAll = new System.Windows.Forms.Button();
            this.grpPreviewKensuu = new System.Windows.Forms.GroupBox();
            this.lblKensuuMax = new System.Windows.Forms.Label();
            this.btnPreview = new System.Windows.Forms.Button();
            this.lblKensuuKen = new System.Windows.Forms.Label();
            this.chkKensuuZenken = new System.Windows.Forms.CheckBox();
            this.numKensuu = new System.Windows.Forms.NumericUpDown();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpOrderAsc = new System.Windows.Forms.GroupBox();
            this.radOrderDesc = new System.Windows.Forms.RadioButton();
            this.radOrderAsc = new System.Windows.Forms.RadioButton();
            this.cmbOrder = new System.Windows.Forms.ComboBox();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.btnStartToday = new System.Windows.Forms.Button();
            this.btnSetDateD = new System.Windows.Forms.Button();
            this.lblKikanNyoro = new System.Windows.Forms.Label();
            this.radKikanStartIgnore = new System.Windows.Forms.RadioButton();
            this.grpKikanStart = new System.Windows.Forms.GroupBox();
            this.radKikanStartFixed = new System.Windows.Forms.RadioButton();
            this.radKikanStartExpand = new System.Windows.Forms.RadioButton();
            this.lblKikanStartInput = new System.Windows.Forms.Label();
            this.grpKikanEnd = new System.Windows.Forms.GroupBox();
            this.radKikanEndFixed = new System.Windows.Forms.RadioButton();
            this.radKikanEndExpand = new System.Windows.Forms.RadioButton();
            this.radKikanEndIgnore = new System.Windows.Forms.RadioButton();
            this.lblKikanEndInput = new System.Windows.Forms.Label();
            this.pnlAutoSetKikanEnd = new System.Windows.Forms.Panel();
            this.lblAutoSetKikanEnd1 = new System.Windows.Forms.Label();
            this.lblAutoSetKikanEnd2 = new System.Windows.Forms.Label();
            this.grpPreviewKensuu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numKensuu)).BeginInit();
            this.grpOrderAsc.SuspendLayout();
            this.grpKikanStart.SuspendLayout();
            this.grpKikanEnd.SuspendLayout();
            this.pnlAutoSetKikanEnd.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPrintAll
            // 
            this.btnPrintAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintAll.AutoSize = true;
            this.btnPrintAll.Location = new System.Drawing.Point(551, 436);
            this.btnPrintAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrintAll.Name = "btnPrintAll";
            this.btnPrintAll.Size = new System.Drawing.Size(157, 36);
            this.btnPrintAll.TabIndex = 3;
            this.btnPrintAll.Text = "全件印刷 (&A)";
            this.btnPrintAll.UseVisualStyleBackColor = true;
            this.btnPrintAll.Click += new System.EventHandler(this.btnPrintAll_Click);
            // 
            // grpPreviewKensuu
            // 
            this.grpPreviewKensuu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.grpPreviewKensuu.Controls.Add(this.lblKensuuMax);
            this.grpPreviewKensuu.Controls.Add(this.btnPreview);
            this.grpPreviewKensuu.Controls.Add(this.lblKensuuKen);
            this.grpPreviewKensuu.Controls.Add(this.chkKensuuZenken);
            this.grpPreviewKensuu.Controls.Add(this.numKensuu);
            this.grpPreviewKensuu.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpPreviewKensuu.Location = new System.Drawing.Point(261, 361);
            this.grpPreviewKensuu.Margin = new System.Windows.Forms.Padding(4);
            this.grpPreviewKensuu.Name = "grpPreviewKensuu";
            this.grpPreviewKensuu.Padding = new System.Windows.Forms.Padding(4);
            this.grpPreviewKensuu.Size = new System.Drawing.Size(452, 60);
            this.grpPreviewKensuu.TabIndex = 2;
            this.grpPreviewKensuu.TabStop = false;
            this.grpPreviewKensuu.Text = "印刷プレビュー";
            // 
            // lblKensuuMax
            // 
            this.lblKensuuMax.AutoSize = true;
            this.lblKensuuMax.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblKensuuMax.Location = new System.Drawing.Point(16, 28);
            this.lblKensuuMax.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblKensuuMax.Name = "lblKensuuMax";
            this.lblKensuuMax.Size = new System.Drawing.Size(37, 15);
            this.lblKensuuMax.TabIndex = 0;
            this.lblKensuuMax.Text = "最大";
            // 
            // btnPreview
            // 
            this.btnPreview.AutoSize = true;
            this.btnPreview.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnPreview.Location = new System.Drawing.Point(290, 15);
            this.btnPreview.Margin = new System.Windows.Forms.Padding(4);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(157, 36);
            this.btnPreview.TabIndex = 4;
            this.btnPreview.Text = "印刷プレビュー (&P)";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // lblKensuuKen
            // 
            this.lblKensuuKen.AutoSize = true;
            this.lblKensuuKen.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblKensuuKen.Location = new System.Drawing.Point(148, 28);
            this.lblKensuuKen.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblKensuuKen.Name = "lblKensuuKen";
            this.lblKensuuKen.Size = new System.Drawing.Size(22, 15);
            this.lblKensuuKen.TabIndex = 2;
            this.lblKensuuKen.Text = "件";
            // 
            // chkKensuuZenken
            // 
            this.chkKensuuZenken.AutoSize = true;
            this.chkKensuuZenken.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkKensuuZenken.Location = new System.Drawing.Point(196, 26);
            this.chkKensuuZenken.Margin = new System.Windows.Forms.Padding(4);
            this.chkKensuuZenken.Name = "chkKensuuZenken";
            this.chkKensuuZenken.Size = new System.Drawing.Size(56, 19);
            this.chkKensuuZenken.TabIndex = 3;
            this.chkKensuuZenken.Text = "全件";
            this.chkKensuuZenken.UseVisualStyleBackColor = true;
            this.chkKensuuZenken.CheckedChanged += new System.EventHandler(this.chkKensuuZenken_CheckedChanged);
            // 
            // numKensuu
            // 
            this.numKensuu.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numKensuu.Location = new System.Drawing.Point(63, 24);
            this.numKensuu.Margin = new System.Windows.Forms.Padding(4);
            this.numKensuu.Name = "numKensuu";
            this.numKensuu.Size = new System.Drawing.Size(77, 22);
            this.numKensuu.TabIndex = 1;
            this.numKensuu.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numKensuu.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.AutoSize = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(551, 487);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(157, 36);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "キャンセル (&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grpOrderAsc
            // 
            this.grpOrderAsc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grpOrderAsc.Controls.Add(this.radOrderDesc);
            this.grpOrderAsc.Controls.Add(this.radOrderAsc);
            this.grpOrderAsc.Controls.Add(this.cmbOrder);
            this.grpOrderAsc.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Bold);
            this.grpOrderAsc.Location = new System.Drawing.Point(17, 259);
            this.grpOrderAsc.Margin = new System.Windows.Forms.Padding(4);
            this.grpOrderAsc.Name = "grpOrderAsc";
            this.grpOrderAsc.Padding = new System.Windows.Forms.Padding(4);
            this.grpOrderAsc.Size = new System.Drawing.Size(352, 80);
            this.grpOrderAsc.TabIndex = 1;
            this.grpOrderAsc.TabStop = false;
            this.grpOrderAsc.Text = "並び替え";
            // 
            // radOrderDesc
            // 
            this.radOrderDesc.AutoSize = true;
            this.radOrderDesc.Font = new System.Drawing.Font("MS UI Gothic", 11F);
            this.radOrderDesc.Location = new System.Drawing.Point(182, 54);
            this.radOrderDesc.Margin = new System.Windows.Forms.Padding(4);
            this.radOrderDesc.Name = "radOrderDesc";
            this.radOrderDesc.Size = new System.Drawing.Size(55, 19);
            this.radOrderDesc.TabIndex = 2;
            this.radOrderDesc.TabStop = true;
            this.radOrderDesc.Text = "降順";
            this.radOrderDesc.UseVisualStyleBackColor = true;
            // 
            // radOrderAsc
            // 
            this.radOrderAsc.AutoSize = true;
            this.radOrderAsc.Font = new System.Drawing.Font("MS UI Gothic", 11F);
            this.radOrderAsc.Location = new System.Drawing.Point(77, 54);
            this.radOrderAsc.Margin = new System.Windows.Forms.Padding(4);
            this.radOrderAsc.Name = "radOrderAsc";
            this.radOrderAsc.Size = new System.Drawing.Size(55, 19);
            this.radOrderAsc.TabIndex = 1;
            this.radOrderAsc.TabStop = true;
            this.radOrderAsc.Text = "昇順";
            this.radOrderAsc.UseVisualStyleBackColor = true;
            // 
            // cmbOrder
            // 
            this.cmbOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrder.Font = new System.Drawing.Font("MS UI Gothic", 11F);
            this.cmbOrder.FormattingEnabled = true;
            this.cmbOrder.Location = new System.Drawing.Point(24, 23);
            this.cmbOrder.Margin = new System.Windows.Forms.Padding(4);
            this.cmbOrder.Name = "cmbOrder";
            this.cmbOrder.Size = new System.Drawing.Size(297, 23);
            this.cmbOrder.TabIndex = 0;
            // 
            // dtpStart
            // 
            this.dtpStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpStart.Font = new System.Drawing.Font("MS UI Gothic", 11F);
            this.dtpStart.Location = new System.Drawing.Point(79, 118);
            this.dtpStart.Margin = new System.Windows.Forms.Padding(4);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(214, 22);
            this.dtpStart.TabIndex = 4;
            this.dtpStart.ValueChanged += new System.EventHandler(this.dtpStart_ValueChanged);
            this.dtpStart.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dtpStart_KeyUp);
            // 
            // dtpEnd
            // 
            this.dtpEnd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpEnd.Font = new System.Drawing.Font("MS UI Gothic", 11F);
            this.dtpEnd.Location = new System.Drawing.Point(88, 118);
            this.dtpEnd.Margin = new System.Windows.Forms.Padding(4);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(214, 22);
            this.dtpEnd.TabIndex = 4;
            this.dtpEnd.ValueChanged += new System.EventHandler(this.dtpEnd_ValueChanged);
            // 
            // btnStartToday
            // 
            this.btnStartToday.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartToday.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.btnStartToday.Location = new System.Drawing.Point(168, 153);
            this.btnStartToday.Margin = new System.Windows.Forms.Padding(4);
            this.btnStartToday.Name = "btnStartToday";
            this.btnStartToday.Size = new System.Drawing.Size(127, 23);
            this.btnStartToday.TabIndex = 5;
            this.btnStartToday.Text = "　今日の日付";
            this.btnStartToday.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStartToday.UseVisualStyleBackColor = true;
            this.btnStartToday.Click += new System.EventHandler(this.btnStartToday_Click);
            // 
            // btnSetDateD
            // 
            this.btnSetDateD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetDateD.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.btnSetDateD.Location = new System.Drawing.Point(187, 153);
            this.btnSetDateD.Margin = new System.Windows.Forms.Padding(4);
            this.btnSetDateD.Name = "btnSetDateD";
            this.btnSetDateD.Size = new System.Drawing.Size(127, 23);
            this.btnSetDateD.TabIndex = 5;
            this.btnSetDateD.Text = "　開始日と同日";
            this.btnSetDateD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSetDateD.UseVisualStyleBackColor = true;
            this.btnSetDateD.Click += new System.EventHandler(this.btnSetDateD_Click);
            // 
            // lblKikanNyoro
            // 
            this.lblKikanNyoro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblKikanNyoro.AutoSize = true;
            this.lblKikanNyoro.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblKikanNyoro.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblKikanNyoro.Location = new System.Drawing.Point(325, 129);
            this.lblKikanNyoro.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblKikanNyoro.Name = "lblKikanNyoro";
            this.lblKikanNyoro.Size = new System.Drawing.Size(40, 27);
            this.lblKikanNyoro.TabIndex = 1;
            this.lblKikanNyoro.Text = "～";
            // 
            // radKikanStartIgnore
            // 
            this.radKikanStartIgnore.AutoSize = true;
            this.radKikanStartIgnore.Font = new System.Drawing.Font("MS UI Gothic", 11F);
            this.radKikanStartIgnore.Location = new System.Drawing.Point(17, 28);
            this.radKikanStartIgnore.Name = "radKikanStartIgnore";
            this.radKikanStartIgnore.Size = new System.Drawing.Size(91, 19);
            this.radKikanStartIgnore.TabIndex = 0;
            this.radKikanStartIgnore.TabStop = true;
            this.radKikanStartIgnore.Text = "指定しない";
            this.radKikanStartIgnore.UseVisualStyleBackColor = true;
            // 
            // grpKikanStart
            // 
            this.grpKikanStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grpKikanStart.Controls.Add(this.radKikanStartFixed);
            this.grpKikanStart.Controls.Add(this.radKikanStartExpand);
            this.grpKikanStart.Controls.Add(this.radKikanStartIgnore);
            this.grpKikanStart.Controls.Add(this.btnStartToday);
            this.grpKikanStart.Controls.Add(this.dtpStart);
            this.grpKikanStart.Controls.Add(this.lblKikanStartInput);
            this.grpKikanStart.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Bold);
            this.grpKikanStart.Location = new System.Drawing.Point(17, 12);
            this.grpKikanStart.Name = "grpKikanStart";
            this.grpKikanStart.Size = new System.Drawing.Size(309, 207);
            this.grpKikanStart.TabIndex = 0;
            this.grpKikanStart.TabStop = false;
            this.grpKikanStart.Text = "開始日";
            // 
            // radKikanStartFixed
            // 
            this.radKikanStartFixed.AutoSize = true;
            this.radKikanStartFixed.Font = new System.Drawing.Font("MS UI Gothic", 11F);
            this.radKikanStartFixed.Location = new System.Drawing.Point(17, 53);
            this.radKikanStartFixed.Name = "radKikanStartFixed";
            this.radKikanStartFixed.Size = new System.Drawing.Size(70, 19);
            this.radKikanStartFixed.TabIndex = 1;
            this.radKikanStartFixed.TabStop = true;
            this.radKikanStartFixed.Text = "指定日";
            this.radKikanStartFixed.UseVisualStyleBackColor = true;
            // 
            // radKikanStartExpand
            // 
            this.radKikanStartExpand.AutoSize = true;
            this.radKikanStartExpand.Font = new System.Drawing.Font("MS UI Gothic", 11F);
            this.radKikanStartExpand.Location = new System.Drawing.Point(17, 78);
            this.radKikanStartExpand.Name = "radKikanStartExpand";
            this.radKikanStartExpand.Size = new System.Drawing.Size(199, 19);
            this.radKikanStartExpand.TabIndex = 2;
            this.radKikanStartExpand.TabStop = true;
            this.radKikanStartExpand.Text = "指定日の診療期間の初診日";
            this.radKikanStartExpand.UseVisualStyleBackColor = true;
            // 
            // lblKikanStartInput
            // 
            this.lblKikanStartInput.AutoSize = true;
            this.lblKikanStartInput.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.lblKikanStartInput.Location = new System.Drawing.Point(27, 123);
            this.lblKikanStartInput.Name = "lblKikanStartInput";
            this.lblKikanStartInput.Size = new System.Drawing.Size(51, 12);
            this.lblKikanStartInput.TabIndex = 3;
            this.lblKikanStartInput.Text = "指定日：";
            // 
            // grpKikanEnd
            // 
            this.grpKikanEnd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpKikanEnd.Controls.Add(this.pnlAutoSetKikanEnd);
            this.grpKikanEnd.Controls.Add(this.radKikanEndFixed);
            this.grpKikanEnd.Controls.Add(this.radKikanEndExpand);
            this.grpKikanEnd.Controls.Add(this.radKikanEndIgnore);
            this.grpKikanEnd.Controls.Add(this.dtpEnd);
            this.grpKikanEnd.Controls.Add(this.btnSetDateD);
            this.grpKikanEnd.Controls.Add(this.lblKikanEndInput);
            this.grpKikanEnd.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Bold);
            this.grpKikanEnd.Location = new System.Drawing.Point(364, 12);
            this.grpKikanEnd.Name = "grpKikanEnd";
            this.grpKikanEnd.Size = new System.Drawing.Size(326, 207);
            this.grpKikanEnd.TabIndex = 2;
            this.grpKikanEnd.TabStop = false;
            this.grpKikanEnd.Text = "終了日";
            // 
            // radKikanEndFixed
            // 
            this.radKikanEndFixed.AutoSize = true;
            this.radKikanEndFixed.Font = new System.Drawing.Font("MS UI Gothic", 11F);
            this.radKikanEndFixed.Location = new System.Drawing.Point(19, 53);
            this.radKikanEndFixed.Name = "radKikanEndFixed";
            this.radKikanEndFixed.Size = new System.Drawing.Size(70, 19);
            this.radKikanEndFixed.TabIndex = 1;
            this.radKikanEndFixed.TabStop = true;
            this.radKikanEndFixed.Text = "指定日";
            this.radKikanEndFixed.UseVisualStyleBackColor = false;
            // 
            // radKikanEndExpand
            // 
            this.radKikanEndExpand.AutoSize = true;
            this.radKikanEndExpand.Font = new System.Drawing.Font("MS UI Gothic", 11F);
            this.radKikanEndExpand.Location = new System.Drawing.Point(19, 78);
            this.radKikanEndExpand.Name = "radKikanEndExpand";
            this.radKikanEndExpand.Size = new System.Drawing.Size(199, 19);
            this.radKikanEndExpand.TabIndex = 2;
            this.radKikanEndExpand.TabStop = true;
            this.radKikanEndExpand.Text = "指定日の診療期間の最終日";
            this.radKikanEndExpand.UseVisualStyleBackColor = true;
            // 
            // radKikanEndIgnore
            // 
            this.radKikanEndIgnore.AutoSize = true;
            this.radKikanEndIgnore.Font = new System.Drawing.Font("MS UI Gothic", 11F);
            this.radKikanEndIgnore.Location = new System.Drawing.Point(19, 28);
            this.radKikanEndIgnore.Name = "radKikanEndIgnore";
            this.radKikanEndIgnore.Size = new System.Drawing.Size(91, 19);
            this.radKikanEndIgnore.TabIndex = 0;
            this.radKikanEndIgnore.TabStop = true;
            this.radKikanEndIgnore.Text = "指定しない";
            this.radKikanEndIgnore.UseVisualStyleBackColor = true;
            // 
            // lblKikanEndInput
            // 
            this.lblKikanEndInput.AutoSize = true;
            this.lblKikanEndInput.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold);
            this.lblKikanEndInput.Location = new System.Drawing.Point(37, 123);
            this.lblKikanEndInput.Name = "lblKikanEndInput";
            this.lblKikanEndInput.Size = new System.Drawing.Size(51, 12);
            this.lblKikanEndInput.TabIndex = 3;
            this.lblKikanEndInput.Text = "指定日：";
            // 
            // pnlAutoSetKikanEnd
            // 
            this.pnlAutoSetKikanEnd.BackColor = System.Drawing.Color.White;
            this.pnlAutoSetKikanEnd.Controls.Add(this.lblAutoSetKikanEnd2);
            this.pnlAutoSetKikanEnd.Controls.Add(this.lblAutoSetKikanEnd1);
            this.pnlAutoSetKikanEnd.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.pnlAutoSetKikanEnd.ForeColor = System.Drawing.Color.Crimson;
            this.pnlAutoSetKikanEnd.Location = new System.Drawing.Point(6, 147);
            this.pnlAutoSetKikanEnd.Name = "pnlAutoSetKikanEnd";
            this.pnlAutoSetKikanEnd.Size = new System.Drawing.Size(174, 54);
            this.pnlAutoSetKikanEnd.TabIndex = 7;
            // 
            // lblAutoSetKikanEnd1
            // 
            this.lblAutoSetKikanEnd1.AutoSize = true;
            this.lblAutoSetKikanEnd1.Location = new System.Drawing.Point(9, 9);
            this.lblAutoSetKikanEnd1.Name = "lblAutoSetKikanEnd1";
            this.lblAutoSetKikanEnd1.Size = new System.Drawing.Size(131, 14);
            this.lblAutoSetKikanEnd1.TabIndex = 0;
            this.lblAutoSetKikanEnd1.Text = "※ 開始日の変更時に";
            // 
            // lblAutoSetKikanEnd2
            // 
            this.lblAutoSetKikanEnd2.AutoSize = true;
            this.lblAutoSetKikanEnd2.Location = new System.Drawing.Point(26, 31);
            this.lblAutoSetKikanEnd2.Name = "lblAutoSetKikanEnd2";
            this.lblAutoSetKikanEnd2.Size = new System.Drawing.Size(144, 14);
            this.lblAutoSetKikanEnd2.TabIndex = 1;
            this.lblAutoSetKikanEnd2.Text = "月末に自動設定します。";
            // 
            // KarteBatchPrintDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(721, 536);
            this.Controls.Add(this.grpKikanEnd);
            this.Controls.Add(this.grpKikanStart);
            this.Controls.Add(this.grpPreviewKensuu);
            this.Controls.Add(this.btnPrintAll);
            this.Controls.Add(this.grpOrderAsc);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblKikanNyoro);
            this.Font = new System.Drawing.Font("MS UI Gothic", 11F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "KarteBatchPrintDialog";
            this.Text = "カルテ一括印刷";
            this.grpPreviewKensuu.ResumeLayout(false);
            this.grpPreviewKensuu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numKensuu)).EndInit();
            this.grpOrderAsc.ResumeLayout(false);
            this.grpOrderAsc.PerformLayout();
            this.grpKikanStart.ResumeLayout(false);
            this.grpKikanStart.PerformLayout();
            this.grpKikanEnd.ResumeLayout(false);
            this.grpKikanEnd.PerformLayout();
            this.pnlAutoSetKikanEnd.ResumeLayout(false);
            this.pnlAutoSetKikanEnd.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPrintAll;
        private System.Windows.Forms.GroupBox grpPreviewKensuu;
        private System.Windows.Forms.Label lblKensuuMax;
        private System.Windows.Forms.Label lblKensuuKen;
        private System.Windows.Forms.CheckBox chkKensuuZenken;
        private System.Windows.Forms.NumericUpDown numKensuu;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grpOrderAsc;
        private System.Windows.Forms.RadioButton radOrderDesc;
        private System.Windows.Forms.RadioButton radOrderAsc;
        private System.Windows.Forms.ComboBox cmbOrder;
        private System.Windows.Forms.Button btnStartToday;
        private System.Windows.Forms.Button btnSetDateD;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Label lblKikanNyoro;
        private System.Windows.Forms.GroupBox grpKikanEnd;
        private System.Windows.Forms.RadioButton radKikanEndFixed;
        private System.Windows.Forms.RadioButton radKikanEndExpand;
        private System.Windows.Forms.RadioButton radKikanEndIgnore;
        private System.Windows.Forms.GroupBox grpKikanStart;
        private System.Windows.Forms.RadioButton radKikanStartFixed;
        private System.Windows.Forms.RadioButton radKikanStartExpand;
        private System.Windows.Forms.RadioButton radKikanStartIgnore;
        private System.Windows.Forms.Label lblKikanEndInput;
        private System.Windows.Forms.Label lblKikanStartInput;
        private System.Windows.Forms.Panel pnlAutoSetKikanEnd;
        private System.Windows.Forms.Label lblAutoSetKikanEnd2;
        private System.Windows.Forms.Label lblAutoSetKikanEnd1;
    }
}