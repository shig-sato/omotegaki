namespace OmoOmotegaki.Forms
{
    partial class OmotegakiForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OmotegakiForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabelカルテタイトル = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabel説明 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ファイルToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.データフォルダーの変更ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.最近使ったデータフォルダーToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.閉じるToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.診療情報ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新規フィルターの作成NToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.全てのフィルターを削除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.診療情報LabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.診療情報_印刷ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.診療情報印刷プレビューToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.歯別診療情報ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.歯別診療情報_印刷ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.歯別診療情報_複数カルテ印刷ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ツールToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.診療録作成ソフトのリンクを解除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.アプリの設定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.データフォルダ共通設定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.データフォルダーToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ウィンドウToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新規ウィンドウToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.すべてのウィンドウを閉じるToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.表書きToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.表書き印刷ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.表書き印刷プレビューToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.カルテToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.カルテ印刷ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.カルテ印刷プレビューToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.カルテ一括印刷ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.カルテルール管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ヘルプToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.操作説明ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.メッセージログToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabKikan = new System.Windows.Forms.TabControl();
            this.tabPage初診リスト = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._btnすべての初診リストのチェックを外す = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this._btnすべての初診リストにチェック = new System.Windows.Forms.Button();
            this._chk最新の期間のみチェック = new System.Windows.Forms.CheckBox();
            this._shoshinKikanListElementHost = new System.Windows.Forms.Integration.ElementHost();
            this._shoshinKikanList = new OmoOmotegaki.Controls.ShoshinKikanList();
            this.tabPage期間指定 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this._dateKikanEnd = new System.Windows.Forms.DateTimePicker();
            this.chkKikanEnd = new System.Windows.Forms.CheckBox();
            this._btnKikanYear = new System.Windows.Forms.Button();
            this._btnKikanDay = new System.Windows.Forms.Button();
            this._btnKikanMonth = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this._dateKikanStart = new System.Windows.Forms.DateTimePicker();
            this.chkKikanStart = new System.Windows.Forms.CheckBox();
            this._btnCalendarTekiyou = new System.Windows.Forms.Button();
            this._pnlKarteLoaderWrap = new System.Windows.Forms.Panel();
            this._pnlKarteLoader = new System.Windows.Forms.Panel();
            this._pnlKarteIdInput = new System.Windows.Forms.Panel();
            this._pnlTxtKarteBangouWrap = new System.Windows.Forms.Panel();
            this.txtKarteBangou = new System.Windows.Forms.TextBox();
            this.cmbSinryoujo = new System.Windows.Forms.ComboBox();
            this.btnLoadKarte = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.picPanelRightDark1 = new System.Windows.Forms.PictureBox();
            this.tabInfo = new System.Windows.Forms.TabControl();
            this.tabPage患者情報 = new System.Windows.Forms.TabPage();
            this.karteDataDisp1 = new OmoOmotegaki.Controls.KarteDataDisp();
            this.picPanelBottomDark = new System.Windows.Forms.PictureBox();
            this._splitContainer上部 = new System.Windows.Forms.SplitContainer();
            this._chkSisyuKanzen = new System.Windows.Forms.CheckBox();
            this._toothSelector = new OmoOmotegaki.Controls.ToothSelector();
            this._shinryouDateSelectorElementHost = new System.Windows.Forms.Integration.ElementHost();
            this._shinryouDateSelector = new OmoOmotegaki.Controls.ShinryouDateSelector();
            this._pnlRirekiTableControlBox = new System.Windows.Forms.FlowLayoutPanel();
            this._pnlJogaiType = new System.Windows.Forms.Panel();
            this._cmbJogaiType = new System.Windows.Forms.ComboBox();
            this._chkGishiJogai = new System.Windows.Forms.CheckBox();
            this._chkGJogai = new System.Windows.Forms.CheckBox();
            this._chkPJogai = new System.Windows.Forms.CheckBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this._cmbShinryouTougou = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this._cmbSinryouOrder = new System.Windows.Forms.Integration.ElementHost();
            this.shinryouOrderTypeSelector1 = new OmoOmotegaki.Controls.ShinryouOrderTypeSelector();
            this.label2 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this._cmbViewerSettings = new System.Windows.Forms.ComboBox();
            this.btnAddTemplate = new System.Windows.Forms.Button();
            this.btnRemoveTemplate = new System.Windows.Forms.Button();
            this._syoRirekiListWrap = new System.Windows.Forms.Integration.ElementHost();
            this._shinryouListControlPanel = new System.Windows.Forms.Panel();
            this._pnlBtnフィルター追加Wrap = new System.Windows.Forms.Panel();
            this._btnフィルター追加 = new System.Windows.Forms.Button();
            this.picMenuDark = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this._btnToggleFilterControlPanel = new System.Windows.Forms.Button();
            this.splitContainerRight = new System.Windows.Forms.SplitContainer();
            this._shinryouListPanel = new System.Windows.Forms.Panel();
            this._filterControlPanel = new System.Windows.Forms.Panel();
            this._shinryouCheckDisplayElementHost = new System.Windows.Forms.Integration.ElementHost();
            this._shinryouCheckDisplay = new OmoOmotegaki.Controls.ShinryouCheckDisplay();
            this.データ変換ヤハラtoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabKikan.SuspendLayout();
            this.tabPage初診リスト.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabPage期間指定.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel3.SuspendLayout();
            this._pnlKarteLoaderWrap.SuspendLayout();
            this._pnlKarteLoader.SuspendLayout();
            this._pnlKarteIdInput.SuspendLayout();
            this._pnlTxtKarteBangouWrap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPanelRightDark1)).BeginInit();
            this.tabInfo.SuspendLayout();
            this.tabPage患者情報.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPanelBottomDark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer上部)).BeginInit();
            this._splitContainer上部.Panel1.SuspendLayout();
            this._splitContainer上部.Panel2.SuspendLayout();
            this._splitContainer上部.SuspendLayout();
            this._pnlRirekiTableControlBox.SuspendLayout();
            this._pnlJogaiType.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this._shinryouListControlPanel.SuspendLayout();
            this._pnlBtnフィルター追加Wrap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMenuDark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRight)).BeginInit();
            this.splitContainerRight.Panel1.SuspendLayout();
            this.splitContainerRight.Panel2.SuspendLayout();
            this.splitContainerRight.SuspendLayout();
            this._shinryouListPanel.SuspendLayout();
            this._filterControlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.statusStrip1.Font = new System.Drawing.Font("メイリオ", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabelカルテタイトル,
            this.statusLabel説明});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1316);
            this.statusStrip1.MinimumSize = new System.Drawing.Size(0, 56);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 30, 0);
            this.statusStrip1.Size = new System.Drawing.Size(2666, 56);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabelカルテタイトル
            // 
            this.statusLabelカルテタイトル.Name = "statusLabelカルテタイトル";
            this.statusLabelカルテタイトル.Size = new System.Drawing.Size(395, 46);
            this.statusLabelカルテタイトル.Text = "statusLabelカルテタイトル";
            // 
            // statusLabel説明
            // 
            this.statusLabel説明.BackColor = System.Drawing.Color.Gold;
            this.statusLabel説明.Margin = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.statusLabel説明.Name = "statusLabel説明";
            this.statusLabel説明.Size = new System.Drawing.Size(245, 56);
            this.statusLabel説明.Text = "statusLabel説明";
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルToolStripMenuItem,
            this.診療情報ToolStripMenuItem,
            this.ツールToolStripMenuItem,
            this.データフォルダーToolStripMenuItem,
            this.ウィンドウToolStripMenuItem,
            this.表書きToolStripMenuItem,
            this.カルテToolStripMenuItem,
            this.ヘルプToolStripMenuItem,
            this.データ変換ヤハラtoolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(14, 4, 0, 4);
            this.menuStrip1.Size = new System.Drawing.Size(2666, 44);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ファイルToolStripMenuItem
            // 
            this.ファイルToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.データフォルダーの変更ToolStripMenuItem,
            this.最近使ったデータフォルダーToolStripMenuItem,
            this.toolStripSeparator2,
            this.閉じるToolStripMenuItem});
            this.ファイルToolStripMenuItem.Name = "ファイルToolStripMenuItem";
            this.ファイルToolStripMenuItem.Size = new System.Drawing.Size(128, 36);
            this.ファイルToolStripMenuItem.Text = "ファイル(&F)";
            // 
            // データフォルダーの変更ToolStripMenuItem
            // 
            this.データフォルダーの変更ToolStripMenuItem.Name = "データフォルダーの変更ToolStripMenuItem";
            this.データフォルダーの変更ToolStripMenuItem.Size = new System.Drawing.Size(406, 44);
            this.データフォルダーの変更ToolStripMenuItem.Text = "データフォルダーの変更 (&D)...";
            this.データフォルダーの変更ToolStripMenuItem.Click += new System.EventHandler(this.データフォルダーの変更ToolStripMenuItem_Click);
            // 
            // 最近使ったデータフォルダーToolStripMenuItem
            // 
            this.最近使ったデータフォルダーToolStripMenuItem.Name = "最近使ったデータフォルダーToolStripMenuItem";
            this.最近使ったデータフォルダーToolStripMenuItem.Size = new System.Drawing.Size(406, 44);
            this.最近使ったデータフォルダーToolStripMenuItem.Text = "最近使ったデータフォルダー";
            this.最近使ったデータフォルダーToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.最近使ったデータフォルダーToolStripMenuItem_DropDownItemClicked);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(403, 6);
            // 
            // 閉じるToolStripMenuItem
            // 
            this.閉じるToolStripMenuItem.Name = "閉じるToolStripMenuItem";
            this.閉じるToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.閉じるToolStripMenuItem.Size = new System.Drawing.Size(406, 44);
            this.閉じるToolStripMenuItem.Text = "閉じる (&C)";
            this.閉じるToolStripMenuItem.Click += new System.EventHandler(this.閉じるToolStripMenuItem_Click);
            // 
            // 診療情報ToolStripMenuItem
            // 
            this.診療情報ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新規フィルターの作成NToolStripMenuItem,
            this.toolStripMenuItem1,
            this.全てのフィルターを削除ToolStripMenuItem,
            this.toolStripSeparator1,
            this.診療情報LabelToolStripMenuItem,
            this.診療情報_印刷ToolStripMenuItem,
            this.診療情報印刷プレビューToolStripMenuItem,
            this.toolStripMenuItem7,
            this.歯別診療情報ToolStripMenuItem,
            this.歯別診療情報_印刷ToolStripMenuItem,
            this.歯別診療情報_複数カルテ印刷ToolStripMenuItem});
            this.診療情報ToolStripMenuItem.Name = "診療情報ToolStripMenuItem";
            this.診療情報ToolStripMenuItem.Size = new System.Drawing.Size(157, 36);
            this.診療情報ToolStripMenuItem.Text = "診療情報(&S)";
            // 
            // 新規フィルターの作成NToolStripMenuItem
            // 
            this.新規フィルターの作成NToolStripMenuItem.Name = "新規フィルターの作成NToolStripMenuItem";
            this.新規フィルターの作成NToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.新規フィルターの作成NToolStripMenuItem.Size = new System.Drawing.Size(473, 44);
            this.新規フィルターの作成NToolStripMenuItem.Text = "新規フィルターの作成(&N)...";
            this.新規フィルターの作成NToolStripMenuItem.Click += new System.EventHandler(this.新規フィルターの作成ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(470, 6);
            // 
            // 全てのフィルターを削除ToolStripMenuItem
            // 
            this.全てのフィルターを削除ToolStripMenuItem.Name = "全てのフィルターを削除ToolStripMenuItem";
            this.全てのフィルターを削除ToolStripMenuItem.Size = new System.Drawing.Size(473, 44);
            this.全てのフィルターを削除ToolStripMenuItem.Text = "全てのフィルターを削除(&C)...";
            this.全てのフィルターを削除ToolStripMenuItem.Click += new System.EventHandler(this.全てのフィルターを削除ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(470, 6);
            // 
            // 診療情報LabelToolStripMenuItem
            // 
            this.診療情報LabelToolStripMenuItem.Name = "診療情報LabelToolStripMenuItem";
            this.診療情報LabelToolStripMenuItem.Size = new System.Drawing.Size(473, 44);
            this.診療情報LabelToolStripMenuItem.Text = "診療情報";
            // 
            // 診療情報_印刷ToolStripMenuItem
            // 
            this.診療情報_印刷ToolStripMenuItem.Margin = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.診療情報_印刷ToolStripMenuItem.Name = "診療情報_印刷ToolStripMenuItem";
            this.診療情報_印刷ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.診療情報_印刷ToolStripMenuItem.Size = new System.Drawing.Size(473, 44);
            this.診療情報_印刷ToolStripMenuItem.Text = "印刷(&P)...";
            this.診療情報_印刷ToolStripMenuItem.Click += new System.EventHandler(this.診療情報_印刷ToolStripMenuItem_Click);
            // 
            // 診療情報印刷プレビューToolStripMenuItem
            // 
            this.診療情報印刷プレビューToolStripMenuItem.Margin = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.診療情報印刷プレビューToolStripMenuItem.Name = "診療情報印刷プレビューToolStripMenuItem";
            this.診療情報印刷プレビューToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.P)));
            this.診療情報印刷プレビューToolStripMenuItem.Size = new System.Drawing.Size(473, 44);
            this.診療情報印刷プレビューToolStripMenuItem.Text = "印刷プレビュー(&V)...";
            this.診療情報印刷プレビューToolStripMenuItem.Click += new System.EventHandler(this.診療情報_印刷プレビューToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(470, 6);
            // 
            // 歯別診療情報ToolStripMenuItem
            // 
            this.歯別診療情報ToolStripMenuItem.Name = "歯別診療情報ToolStripMenuItem";
            this.歯別診療情報ToolStripMenuItem.Size = new System.Drawing.Size(473, 44);
            this.歯別診療情報ToolStripMenuItem.Text = "歯別診療情報";
            // 
            // 歯別診療情報_印刷ToolStripMenuItem
            // 
            this.歯別診療情報_印刷ToolStripMenuItem.Margin = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.歯別診療情報_印刷ToolStripMenuItem.Name = "歯別診療情報_印刷ToolStripMenuItem";
            this.歯別診療情報_印刷ToolStripMenuItem.Size = new System.Drawing.Size(473, 44);
            this.歯別診療情報_印刷ToolStripMenuItem.Text = "印刷...";
            this.歯別診療情報_印刷ToolStripMenuItem.Click += new System.EventHandler(this.歯別診療情報_印刷ToolStripMenuItem_Click);
            // 
            // 歯別診療情報_複数カルテ印刷ToolStripMenuItem
            // 
            this.歯別診療情報_複数カルテ印刷ToolStripMenuItem.Margin = new System.Windows.Forms.Padding(18, 0, 0, 0);
            this.歯別診療情報_複数カルテ印刷ToolStripMenuItem.Name = "歯別診療情報_複数カルテ印刷ToolStripMenuItem";
            this.歯別診療情報_複数カルテ印刷ToolStripMenuItem.Size = new System.Drawing.Size(473, 44);
            this.歯別診療情報_複数カルテ印刷ToolStripMenuItem.Text = "複数カルテ印刷...";
            this.歯別診療情報_複数カルテ印刷ToolStripMenuItem.Click += new System.EventHandler(this.歯別診療情報_複数カルテ印刷ToolStripMenuItem_Click);
            // 
            // ツールToolStripMenuItem
            // 
            this.ツールToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.診療録作成ソフトのリンクを解除ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.アプリの設定ToolStripMenuItem,
            this.データフォルダ共通設定ToolStripMenuItem,
            this.toolStripMenuItem8});
            this.ツールToolStripMenuItem.Name = "ツールToolStripMenuItem";
            this.ツールToolStripMenuItem.Size = new System.Drawing.Size(114, 36);
            this.ツールToolStripMenuItem.Text = "ツール(&T)";
            // 
            // 診療録作成ソフトのリンクを解除ToolStripMenuItem
            // 
            this.診療録作成ソフトのリンクを解除ToolStripMenuItem.Enabled = false;
            this.診療録作成ソフトのリンクを解除ToolStripMenuItem.Name = "診療録作成ソフトのリンクを解除ToolStripMenuItem";
            this.診療録作成ソフトのリンクを解除ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.診療録作成ソフトのリンクを解除ToolStripMenuItem.Size = new System.Drawing.Size(569, 44);
            this.診療録作成ソフトのリンクを解除ToolStripMenuItem.Text = "診療録作成ソフトのリンクを解除(&U)";
            this.診療録作成ソフトのリンクを解除ToolStripMenuItem.Click += new System.EventHandler(this.診療録作成ソフトのリンクを解除ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(566, 6);
            // 
            // アプリの設定ToolStripMenuItem
            // 
            this.アプリの設定ToolStripMenuItem.Name = "アプリの設定ToolStripMenuItem";
            this.アプリの設定ToolStripMenuItem.Size = new System.Drawing.Size(569, 44);
            this.アプリの設定ToolStripMenuItem.Text = "アプリの設定(&O)...";
            this.アプリの設定ToolStripMenuItem.Click += new System.EventHandler(this.アプリの設定ToolStripMenuItem_Click);
            // 
            // データフォルダ共通設定ToolStripMenuItem
            // 
            this.データフォルダ共通設定ToolStripMenuItem.Name = "データフォルダ共通設定ToolStripMenuItem";
            this.データフォルダ共通設定ToolStripMenuItem.Size = new System.Drawing.Size(569, 44);
            this.データフォルダ共通設定ToolStripMenuItem.Text = "データフォルダ共通設定(D)...";
            this.データフォルダ共通設定ToolStripMenuItem.Click += new System.EventHandler(this.データフォルダ共通設定ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(566, 6);
            // 
            // データフォルダーToolStripMenuItem
            // 
            this.データフォルダーToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.データフォルダーToolStripMenuItem.Name = "データフォルダーToolStripMenuItem";
            this.データフォルダーToolStripMenuItem.Size = new System.Drawing.Size(172, 36);
            this.データフォルダーToolStripMenuItem.Text = "データフォルダー";
            // 
            // ウィンドウToolStripMenuItem
            // 
            this.ウィンドウToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新規ウィンドウToolStripMenuItem,
            this.toolStripMenuItem3,
            this.すべてのウィンドウを閉じるToolStripMenuItem,
            this.toolStripMenuItem4});
            this.ウィンドウToolStripMenuItem.Name = "ウィンドウToolStripMenuItem";
            this.ウィンドウToolStripMenuItem.Size = new System.Drawing.Size(155, 36);
            this.ウィンドウToolStripMenuItem.Text = "ウィンドウ(&W)";
            // 
            // 新規ウィンドウToolStripMenuItem
            // 
            this.新規ウィンドウToolStripMenuItem.Name = "新規ウィンドウToolStripMenuItem";
            this.新規ウィンドウToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.N)));
            this.新規ウィンドウToolStripMenuItem.Size = new System.Drawing.Size(431, 44);
            this.新規ウィンドウToolStripMenuItem.Text = "新規ウィンドウ(&N)";
            this.新規ウィンドウToolStripMenuItem.Click += new System.EventHandler(this.新規ウィンドウToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(428, 6);
            // 
            // すべてのウィンドウを閉じるToolStripMenuItem
            // 
            this.すべてのウィンドウを閉じるToolStripMenuItem.Name = "すべてのウィンドウを閉じるToolStripMenuItem";
            this.すべてのウィンドウを閉じるToolStripMenuItem.Size = new System.Drawing.Size(431, 44);
            this.すべてのウィンドウを閉じるToolStripMenuItem.Text = "すべてのウィンドウを閉じる(&A)...";
            this.すべてのウィンドウを閉じるToolStripMenuItem.Click += new System.EventHandler(this.すべてのウィンドウを閉じるToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(428, 6);
            // 
            // 表書きToolStripMenuItem
            // 
            this.表書きToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.表書き印刷ToolStripMenuItem,
            this.表書き印刷プレビューToolStripMenuItem});
            this.表書きToolStripMenuItem.Name = "表書きToolStripMenuItem";
            this.表書きToolStripMenuItem.Size = new System.Drawing.Size(132, 36);
            this.表書きToolStripMenuItem.Text = "表書き(&O)";
            // 
            // 表書き印刷ToolStripMenuItem
            // 
            this.表書き印刷ToolStripMenuItem.Name = "表書き印刷ToolStripMenuItem";
            this.表書き印刷ToolStripMenuItem.Size = new System.Drawing.Size(322, 44);
            this.表書き印刷ToolStripMenuItem.Text = "印刷(&P)...";
            this.表書き印刷ToolStripMenuItem.Click += new System.EventHandler(this.表書き印刷ToolStripMenuItem_Click);
            // 
            // 表書き印刷プレビューToolStripMenuItem
            // 
            this.表書き印刷プレビューToolStripMenuItem.Name = "表書き印刷プレビューToolStripMenuItem";
            this.表書き印刷プレビューToolStripMenuItem.Size = new System.Drawing.Size(322, 44);
            this.表書き印刷プレビューToolStripMenuItem.Text = "印刷プレビュー(&V)...";
            this.表書き印刷プレビューToolStripMenuItem.Click += new System.EventHandler(this.表書き印刷プレビューToolStripMenuItem_Click);
            // 
            // カルテToolStripMenuItem
            // 
            this.カルテToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.カルテ印刷ToolStripMenuItem,
            this.カルテ印刷プレビューToolStripMenuItem,
            this.toolStripMenuItem6,
            this.カルテ一括印刷ToolStripMenuItem,
            this.toolStripMenuItem5,
            this.カルテルール管理ToolStripMenuItem});
            this.カルテToolStripMenuItem.Name = "カルテToolStripMenuItem";
            this.カルテToolStripMenuItem.Size = new System.Drawing.Size(118, 36);
            this.カルテToolStripMenuItem.Text = "カルテ(&K)";
            // 
            // カルテ印刷ToolStripMenuItem
            // 
            this.カルテ印刷ToolStripMenuItem.Name = "カルテ印刷ToolStripMenuItem";
            this.カルテ印刷ToolStripMenuItem.Size = new System.Drawing.Size(342, 44);
            this.カルテ印刷ToolStripMenuItem.Text = "印刷(&P)...";
            this.カルテ印刷ToolStripMenuItem.Click += new System.EventHandler(this.カルテ印刷ToolStripMenuItem_Click);
            // 
            // カルテ印刷プレビューToolStripMenuItem
            // 
            this.カルテ印刷プレビューToolStripMenuItem.Name = "カルテ印刷プレビューToolStripMenuItem";
            this.カルテ印刷プレビューToolStripMenuItem.Size = new System.Drawing.Size(342, 44);
            this.カルテ印刷プレビューToolStripMenuItem.Text = "印刷プレビュー(&V)...";
            this.カルテ印刷プレビューToolStripMenuItem.Click += new System.EventHandler(this.カルテ印刷プレビューToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(339, 6);
            // 
            // カルテ一括印刷ToolStripMenuItem
            // 
            this.カルテ一括印刷ToolStripMenuItem.Name = "カルテ一括印刷ToolStripMenuItem";
            this.カルテ一括印刷ToolStripMenuItem.Size = new System.Drawing.Size(342, 44);
            this.カルテ一括印刷ToolStripMenuItem.Text = "カルテ一括印刷(&B)...";
            this.カルテ一括印刷ToolStripMenuItem.Click += new System.EventHandler(this.カルテ一括印刷ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(339, 6);
            // 
            // カルテルール管理ToolStripMenuItem
            // 
            this.カルテルール管理ToolStripMenuItem.Name = "カルテルール管理ToolStripMenuItem";
            this.カルテルール管理ToolStripMenuItem.Size = new System.Drawing.Size(342, 44);
            this.カルテルール管理ToolStripMenuItem.Text = "ルール管理(&R)...";
            this.カルテルール管理ToolStripMenuItem.Click += new System.EventHandler(this.カルテルール管理ToolStripMenuItem_Click);
            // 
            // ヘルプToolStripMenuItem
            // 
            this.ヘルプToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.操作説明ToolStripMenuItem,
            this.メッセージログToolStripMenuItem});
            this.ヘルプToolStripMenuItem.Name = "ヘルプToolStripMenuItem";
            this.ヘルプToolStripMenuItem.Size = new System.Drawing.Size(123, 36);
            this.ヘルプToolStripMenuItem.Text = "ヘルプ(&H)";
            // 
            // 操作説明ToolStripMenuItem
            // 
            this.操作説明ToolStripMenuItem.Name = "操作説明ToolStripMenuItem";
            this.操作説明ToolStripMenuItem.Size = new System.Drawing.Size(313, 44);
            this.操作説明ToolStripMenuItem.Text = "操作説明(&C)";
            this.操作説明ToolStripMenuItem.Click += new System.EventHandler(this.操作説明ToolStripMenuItem_Click);
            // 
            // メッセージログToolStripMenuItem
            // 
            this.メッセージログToolStripMenuItem.Name = "メッセージログToolStripMenuItem";
            this.メッセージログToolStripMenuItem.Size = new System.Drawing.Size(313, 44);
            this.メッセージログToolStripMenuItem.Text = "メッセージ ログ(&M)";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Controls.Add(this.picPanelBottomDark);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 48);
            this.panel1.Margin = new System.Windows.Forms.Padding(6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(500, 1268);
            this.panel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.SlateGray;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(6);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.tabKikan);
            this.splitContainer1.Panel1.Controls.Add(this._pnlKarteLoaderWrap);
            this.splitContainer1.Panel1.Controls.Add(this.panel4);
            this.splitContainer1.Panel1.Controls.Add(this.picPanelRightDark1);
            this.splitContainer1.Panel1MinSize = 242;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel2.Controls.Add(this.tabInfo);
            this.splitContainer1.Size = new System.Drawing.Size(500, 1264);
            this.splitContainer1.SplitterDistance = 902;
            this.splitContainer1.SplitterWidth = 8;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabKikan
            // 
            this.tabKikan.Controls.Add(this.tabPage初診リスト);
            this.tabKikan.Controls.Add(this.tabPage期間指定);
            this.tabKikan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabKikan.Location = new System.Drawing.Point(0, 76);
            this.tabKikan.Margin = new System.Windows.Forms.Padding(6);
            this.tabKikan.Name = "tabKikan";
            this.tabKikan.SelectedIndex = 0;
            this.tabKikan.Size = new System.Drawing.Size(496, 826);
            this.tabKikan.TabIndex = 0;
            // 
            // tabPage初診リスト
            // 
            this.tabPage初診リスト.Controls.Add(this.splitContainer3);
            this.tabPage初診リスト.ForeColor = System.Drawing.Color.Black;
            this.tabPage初診リスト.Location = new System.Drawing.Point(8, 39);
            this.tabPage初診リスト.Margin = new System.Windows.Forms.Padding(6);
            this.tabPage初診リスト.Name = "tabPage初診リスト";
            this.tabPage初診リスト.Padding = new System.Windows.Forms.Padding(6);
            this.tabPage初診リスト.Size = new System.Drawing.Size(480, 779);
            this.tabPage初診リスト.TabIndex = 0;
            this.tabPage初診リスト.Text = "初診リスト";
            this.tabPage初診リスト.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point(6, 6);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(6);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer3.Panel1.Controls.Add(this._chk最新の期間のみチェック);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this._shoshinKikanListElementHost);
            this.splitContainer3.Size = new System.Drawing.Size(468, 767);
            this.splitContainer3.SplitterDistance = 28;
            this.splitContainer3.SplitterWidth = 8;
            this.splitContainer3.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Controls.Add(this._btnすべての初診リストのチェックを外す, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._btnすべての初診リストにチェック, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(273, 28);
            this.tableLayoutPanel1.TabIndex = 21;
            // 
            // _btnすべての初診リストのチェックを外す
            // 
            this._btnすべての初診リストのチェックを外す.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._btnすべての初診リストのチェックを外す.AutoSize = true;
            this._btnすべての初診リストのチェックを外す.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnすべての初診リストのチェックを外す.BackColor = System.Drawing.Color.Azure;
            this._btnすべての初診リストのチェックを外す.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this._btnすべての初診リストのチェックを外す.Location = new System.Drawing.Point(161, 4);
            this._btnすべての初診リストのチェックを外す.Margin = new System.Windows.Forms.Padding(4);
            this._btnすべての初診リストのチェックを外す.Name = "_btnすべての初診リストのチェックを外す";
            this._btnすべての初診リストのチェックを外す.Size = new System.Drawing.Size(68, 20);
            this._btnすべての初診リストのチェックを外す.TabIndex = 17;
            this._btnすべての初診リストのチェックを外す.Text = "☐";
            this.toolTip1.SetToolTip(this._btnすべての初診リストのチェックを外す, "すべてのチェックを外す");
            this._btnすべての初診リストのチェックを外す.UseVisualStyleBackColor = false;
            this._btnすべての初診リストのチェックを外す.Click += new System.EventHandler(this._btnすべての初診リストのチェックを外す_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 2);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 24);
            this.label4.TabIndex = 19;
            this.label4.Text = "すべて";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _btnすべての初診リストにチェック
            // 
            this._btnすべての初診リストにチェック.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._btnすべての初診リストにチェック.AutoSize = true;
            this._btnすべての初診リストにチェック.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnすべての初診リストにチェック.BackColor = System.Drawing.Color.Azure;
            this._btnすべての初診リストにチェック.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this._btnすべての初診リストにチェック.Location = new System.Drawing.Point(85, 4);
            this._btnすべての初診リストにチェック.Margin = new System.Windows.Forms.Padding(4);
            this._btnすべての初診リストにチェック.Name = "_btnすべての初診リストにチェック";
            this._btnすべての初診リストにチェック.Size = new System.Drawing.Size(68, 20);
            this._btnすべての初診リストにチェック.TabIndex = 17;
            this._btnすべての初診リストにチェック.Text = "☑";
            this.toolTip1.SetToolTip(this._btnすべての初診リストにチェック, "すべてにチェックを入れる");
            this._btnすべての初診リストにチェック.UseVisualStyleBackColor = false;
            this._btnすべての初診リストにチェック.Click += new System.EventHandler(this._btnすべての初診リストにチェック_Click);
            // 
            // _chk最新の期間のみチェック
            // 
            this._chk最新の期間のみチェック.AutoSize = true;
            this._chk最新の期間のみチェック.Dock = System.Windows.Forms.DockStyle.Right;
            this._chk最新の期間のみチェック.Location = new System.Drawing.Point(273, 0);
            this._chk最新の期間のみチェック.Margin = new System.Windows.Forms.Padding(4);
            this._chk最新の期間のみチェック.Name = "_chk最新の期間のみチェック";
            this._chk最新の期間のみチェック.Size = new System.Drawing.Size(195, 28);
            this._chk最新の期間のみチェック.TabIndex = 18;
            this._chk最新の期間のみチェック.Text = "最新のみチェック";
            this._chk最新の期間のみチェック.UseVisualStyleBackColor = true;
            this._chk最新の期間のみチェック.CheckedChanged += new System.EventHandler(this._chk最新の期間のみチェック_CheckedChanged);
            // 
            // _shoshinKikanListElementHost
            // 
            this._shoshinKikanListElementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this._shoshinKikanListElementHost.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this._shoshinKikanListElementHost.Location = new System.Drawing.Point(0, 0);
            this._shoshinKikanListElementHost.Margin = new System.Windows.Forms.Padding(6);
            this._shoshinKikanListElementHost.Name = "_shoshinKikanListElementHost";
            this._shoshinKikanListElementHost.Size = new System.Drawing.Size(468, 731);
            this._shoshinKikanListElementHost.TabIndex = 0;
            this._shoshinKikanListElementHost.Text = "_shoshinKikanList";
            this._shoshinKikanListElementHost.Child = this._shoshinKikanList;
            // 
            // tabPage期間指定
            // 
            this.tabPage期間指定.Controls.Add(this.panel2);
            this.tabPage期間指定.Location = new System.Drawing.Point(8, 39);
            this.tabPage期間指定.Margin = new System.Windows.Forms.Padding(6);
            this.tabPage期間指定.Name = "tabPage期間指定";
            this.tabPage期間指定.Padding = new System.Windows.Forms.Padding(6);
            this.tabPage期間指定.Size = new System.Drawing.Size(480, 778);
            this.tabPage期間指定.TabIndex = 1;
            this.tabPage期間指定.Text = "期間指定";
            this.tabPage期間指定.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel8);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this._btnCalendarTekiyou);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel2.Location = new System.Drawing.Point(6, 6);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(468, 766);
            this.panel2.TabIndex = 0;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.label6);
            this.panel8.Controls.Add(this._dateKikanEnd);
            this.panel8.Controls.Add(this.chkKikanEnd);
            this.panel8.Controls.Add(this._btnKikanYear);
            this.panel8.Controls.Add(this._btnKikanDay);
            this.panel8.Controls.Add(this._btnKikanMonth);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 110);
            this.panel8.Margin = new System.Windows.Forms.Padding(6);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(468, 150);
            this.panel8.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 6);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 24);
            this.label6.TabIndex = 9;
            this.label6.Text = "終了日";
            // 
            // _dateKikanEnd
            // 
            this._dateKikanEnd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._dateKikanEnd.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._dateKikanEnd.Location = new System.Drawing.Point(56, 40);
            this._dateKikanEnd.Margin = new System.Windows.Forms.Padding(6);
            this._dateKikanEnd.Name = "_dateKikanEnd";
            this._dateKikanEnd.Size = new System.Drawing.Size(408, 39);
            this._dateKikanEnd.TabIndex = 8;
            // 
            // chkKikanEnd
            // 
            this.chkKikanEnd.AutoSize = true;
            this.chkKikanEnd.Location = new System.Drawing.Point(12, 48);
            this.chkKikanEnd.Margin = new System.Windows.Forms.Padding(6);
            this.chkKikanEnd.Name = "chkKikanEnd";
            this.chkKikanEnd.Size = new System.Drawing.Size(28, 27);
            this.chkKikanEnd.TabIndex = 7;
            this.chkKikanEnd.UseVisualStyleBackColor = true;
            this.chkKikanEnd.CheckedChanged += new System.EventHandler(this.chkKikanEnd_CheckedChanged);
            // 
            // _btnKikanYear
            // 
            this._btnKikanYear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._btnKikanYear.Location = new System.Drawing.Point(314, 98);
            this._btnKikanYear.Margin = new System.Windows.Forms.Padding(6);
            this._btnKikanYear.Name = "_btnKikanYear";
            this._btnKikanYear.Size = new System.Drawing.Size(138, 46);
            this._btnKikanYear.TabIndex = 11;
            this._btnKikanYear.Text = "１年間";
            this._btnKikanYear.UseVisualStyleBackColor = true;
            // 
            // _btnKikanDay
            // 
            this._btnKikanDay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._btnKikanDay.Location = new System.Drawing.Point(54, 98);
            this._btnKikanDay.Margin = new System.Windows.Forms.Padding(6);
            this._btnKikanDay.Name = "_btnKikanDay";
            this._btnKikanDay.Size = new System.Drawing.Size(102, 46);
            this._btnKikanDay.TabIndex = 9;
            this._btnKikanDay.Text = "同日";
            this._btnKikanDay.UseVisualStyleBackColor = true;
            // 
            // _btnKikanMonth
            // 
            this._btnKikanMonth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._btnKikanMonth.Location = new System.Drawing.Point(166, 98);
            this._btnKikanMonth.Margin = new System.Windows.Forms.Padding(6);
            this._btnKikanMonth.Name = "_btnKikanMonth";
            this._btnKikanMonth.Size = new System.Drawing.Size(138, 46);
            this._btnKikanMonth.TabIndex = 10;
            this._btnKikanMonth.Text = "１ヶ月間";
            this._btnKikanMonth.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this._dateKikanStart);
            this.panel3.Controls.Add(this.chkKikanStart);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(468, 110);
            this.panel3.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 6);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 24);
            this.label5.TabIndex = 2;
            this.label5.Text = "開始日";
            // 
            // _dateKikanStart
            // 
            this._dateKikanStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._dateKikanStart.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._dateKikanStart.Location = new System.Drawing.Point(56, 40);
            this._dateKikanStart.Margin = new System.Windows.Forms.Padding(6);
            this._dateKikanStart.Name = "_dateKikanStart";
            this._dateKikanStart.Size = new System.Drawing.Size(408, 39);
            this._dateKikanStart.TabIndex = 1;
            // 
            // chkKikanStart
            // 
            this.chkKikanStart.AutoSize = true;
            this.chkKikanStart.Location = new System.Drawing.Point(12, 48);
            this.chkKikanStart.Margin = new System.Windows.Forms.Padding(6);
            this.chkKikanStart.Name = "chkKikanStart";
            this.chkKikanStart.Size = new System.Drawing.Size(28, 27);
            this.chkKikanStart.TabIndex = 0;
            this.chkKikanStart.UseVisualStyleBackColor = true;
            this.chkKikanStart.CheckedChanged += new System.EventHandler(this.chkKikanStart_CheckedChanged);
            // 
            // _btnCalendarTekiyou
            // 
            this._btnCalendarTekiyou.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnCalendarTekiyou.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._btnCalendarTekiyou.Location = new System.Drawing.Point(146, 326);
            this._btnCalendarTekiyou.Margin = new System.Windows.Forms.Padding(6);
            this._btnCalendarTekiyou.Name = "_btnCalendarTekiyou";
            this._btnCalendarTekiyou.Size = new System.Drawing.Size(178, 70);
            this._btnCalendarTekiyou.TabIndex = 12;
            this._btnCalendarTekiyou.Text = "適用";
            this._btnCalendarTekiyou.UseVisualStyleBackColor = true;
            this._btnCalendarTekiyou.Click += new System.EventHandler(this.btnCalendarTekiyou_Click);
            // 
            // _pnlKarteLoaderWrap
            // 
            this._pnlKarteLoaderWrap.Controls.Add(this._pnlKarteLoader);
            this._pnlKarteLoaderWrap.Dock = System.Windows.Forms.DockStyle.Top;
            this._pnlKarteLoaderWrap.Location = new System.Drawing.Point(0, 0);
            this._pnlKarteLoaderWrap.Margin = new System.Windows.Forms.Padding(6);
            this._pnlKarteLoaderWrap.Name = "_pnlKarteLoaderWrap";
            this._pnlKarteLoaderWrap.Size = new System.Drawing.Size(496, 76);
            this._pnlKarteLoaderWrap.TabIndex = 20;
            // 
            // _pnlKarteLoader
            // 
            this._pnlKarteLoader.Controls.Add(this._pnlKarteIdInput);
            this._pnlKarteLoader.Controls.Add(this.btnLoadKarte);
            this._pnlKarteLoader.Controls.Add(this.btnClear);
            this._pnlKarteLoader.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlKarteLoader.Location = new System.Drawing.Point(0, 0);
            this._pnlKarteLoader.Margin = new System.Windows.Forms.Padding(6);
            this._pnlKarteLoader.Name = "_pnlKarteLoader";
            this._pnlKarteLoader.Size = new System.Drawing.Size(496, 76);
            this._pnlKarteLoader.TabIndex = 14;
            // 
            // _pnlKarteIdInput
            // 
            this._pnlKarteIdInput.Controls.Add(this._pnlTxtKarteBangouWrap);
            this._pnlKarteIdInput.Controls.Add(this.cmbSinryoujo);
            this._pnlKarteIdInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlKarteIdInput.Location = new System.Drawing.Point(0, 0);
            this._pnlKarteIdInput.Margin = new System.Windows.Forms.Padding(0);
            this._pnlKarteIdInput.Name = "_pnlKarteIdInput";
            this._pnlKarteIdInput.Padding = new System.Windows.Forms.Padding(8, 8, 4, 8);
            this._pnlKarteIdInput.Size = new System.Drawing.Size(318, 76);
            this._pnlKarteIdInput.TabIndex = 3;
            // 
            // _pnlTxtKarteBangouWrap
            // 
            this._pnlTxtKarteBangouWrap.Controls.Add(this.txtKarteBangou);
            this._pnlTxtKarteBangouWrap.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlTxtKarteBangouWrap.Location = new System.Drawing.Point(140, 8);
            this._pnlTxtKarteBangouWrap.Margin = new System.Windows.Forms.Padding(0);
            this._pnlTxtKarteBangouWrap.Name = "_pnlTxtKarteBangouWrap";
            this._pnlTxtKarteBangouWrap.Padding = new System.Windows.Forms.Padding(4, 4, 0, 0);
            this._pnlTxtKarteBangouWrap.Size = new System.Drawing.Size(174, 60);
            this._pnlTxtKarteBangouWrap.TabIndex = 2;
            // 
            // txtKarteBangou
            // 
            this.txtKarteBangou.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKarteBangou.Font = new System.Drawing.Font("MS UI Gothic", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtKarteBangou.ForeColor = System.Drawing.Color.Black;
            this.txtKarteBangou.Location = new System.Drawing.Point(4, 4);
            this.txtKarteBangou.Margin = new System.Windows.Forms.Padding(0);
            this.txtKarteBangou.Name = "txtKarteBangou";
            this.txtKarteBangou.Size = new System.Drawing.Size(170, 45);
            this.txtKarteBangou.TabIndex = 1;
            this.txtKarteBangou.Text = "123456";
            this.txtKarteBangou.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtKarteBangou.WordWrap = false;
            this.txtKarteBangou.TextChanged += new System.EventHandler(this.txtKarteBangou_TextChanged);
            this.txtKarteBangou.Enter += new System.EventHandler(this.txtKarteBangou_GotFocus);
            this.txtKarteBangou.Leave += new System.EventHandler(this.txtKarteBangou_LostFocus);
            this.txtKarteBangou.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtKarteBangou_PreviewKeyDown);
            // 
            // cmbSinryoujo
            // 
            this.cmbSinryoujo.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmbSinryoujo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSinryoujo.Enabled = false;
            this.cmbSinryoujo.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbSinryoujo.ForeColor = System.Drawing.Color.Black;
            this.cmbSinryoujo.FormattingEnabled = true;
            this.cmbSinryoujo.Location = new System.Drawing.Point(8, 8);
            this.cmbSinryoujo.Margin = new System.Windows.Forms.Padding(0);
            this.cmbSinryoujo.Name = "cmbSinryoujo";
            this.cmbSinryoujo.Size = new System.Drawing.Size(132, 50);
            this.cmbSinryoujo.TabIndex = 0;
            // 
            // btnLoadKarte
            // 
            this.btnLoadKarte.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnLoadKarte.ForeColor = System.Drawing.Color.Black;
            this.btnLoadKarte.Location = new System.Drawing.Point(318, 0);
            this.btnLoadKarte.Margin = new System.Windows.Forms.Padding(0);
            this.btnLoadKarte.Name = "btnLoadKarte";
            this.btnLoadKarte.Size = new System.Drawing.Size(84, 76);
            this.btnLoadKarte.TabIndex = 2;
            this.btnLoadKarte.Text = "読込";
            this.btnLoadKarte.UseVisualStyleBackColor = true;
            this.btnLoadKarte.Click += new System.EventHandler(this.btnLoadKarte_Click);
            // 
            // btnClear
            // 
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClear.ForeColor = System.Drawing.Color.Black;
            this.btnClear.Location = new System.Drawing.Point(402, 0);
            this.btnClear.Margin = new System.Windows.Forms.Padding(0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(94, 76);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "クリア";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // panel4
            // 
            this.panel4.AutoSize = true;
            this.panel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 902);
            this.panel4.Margin = new System.Windows.Forms.Padding(6);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(496, 0);
            this.panel4.TabIndex = 19;
            // 
            // picPanelRightDark1
            // 
            this.picPanelRightDark1.BackColor = System.Drawing.Color.DarkGray;
            this.picPanelRightDark1.Dock = System.Windows.Forms.DockStyle.Right;
            this.picPanelRightDark1.Location = new System.Drawing.Point(496, 0);
            this.picPanelRightDark1.Margin = new System.Windows.Forms.Padding(6);
            this.picPanelRightDark1.Name = "picPanelRightDark1";
            this.picPanelRightDark1.Size = new System.Drawing.Size(4, 902);
            this.picPanelRightDark1.TabIndex = 10;
            this.picPanelRightDark1.TabStop = false;
            // 
            // tabInfo
            // 
            this.tabInfo.Controls.Add(this.tabPage患者情報);
            this.tabInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabInfo.Location = new System.Drawing.Point(0, 0);
            this.tabInfo.Margin = new System.Windows.Forms.Padding(6);
            this.tabInfo.Name = "tabInfo";
            this.tabInfo.Padding = new System.Drawing.Point(0, 0);
            this.tabInfo.SelectedIndex = 0;
            this.tabInfo.Size = new System.Drawing.Size(500, 354);
            this.tabInfo.TabIndex = 1;
            this.tabInfo.TabStop = false;
            // 
            // tabPage患者情報
            // 
            this.tabPage患者情報.AutoScroll = true;
            this.tabPage患者情報.Controls.Add(this.karteDataDisp1);
            this.tabPage患者情報.Location = new System.Drawing.Point(8, 43);
            this.tabPage患者情報.Margin = new System.Windows.Forms.Padding(6);
            this.tabPage患者情報.Name = "tabPage患者情報";
            this.tabPage患者情報.Padding = new System.Windows.Forms.Padding(6);
            this.tabPage患者情報.Size = new System.Drawing.Size(484, 303);
            this.tabPage患者情報.TabIndex = 0;
            this.tabPage患者情報.Text = "患者情報";
            this.tabPage患者情報.UseVisualStyleBackColor = true;
            // 
            // karteDataDisp1
            // 
            this.karteDataDisp1.BackColor = System.Drawing.Color.White;
            this.karteDataDisp1.KarteData = ((OmoSeitokuEreceipt.SER.KarteData)(resources.GetObject("karteDataDisp1.KarteData")));
            this.karteDataDisp1.Location = new System.Drawing.Point(0, 0);
            this.karteDataDisp1.Margin = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.karteDataDisp1.Name = "karteDataDisp1";
            this.karteDataDisp1.Size = new System.Drawing.Size(1382, 594);
            this.karteDataDisp1.TabIndex = 0;
            this.karteDataDisp1.TabStop = false;
            // 
            // picPanelBottomDark
            // 
            this.picPanelBottomDark.BackColor = System.Drawing.Color.DarkGray;
            this.picPanelBottomDark.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.picPanelBottomDark.Location = new System.Drawing.Point(0, 1264);
            this.picPanelBottomDark.Margin = new System.Windows.Forms.Padding(6);
            this.picPanelBottomDark.Name = "picPanelBottomDark";
            this.picPanelBottomDark.Size = new System.Drawing.Size(500, 4);
            this.picPanelBottomDark.TabIndex = 15;
            this.picPanelBottomDark.TabStop = false;
            // 
            // _splitContainer上部
            // 
            this._splitContainer上部.BackColor = System.Drawing.Color.MediumTurquoise;
            this._splitContainer上部.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer上部.Location = new System.Drawing.Point(0, 0);
            this._splitContainer上部.Margin = new System.Windows.Forms.Padding(6);
            this._splitContainer上部.Name = "_splitContainer上部";
            // 
            // _splitContainer上部.Panel1
            // 
            this._splitContainer上部.Panel1.Controls.Add(this._chkSisyuKanzen);
            this._splitContainer上部.Panel1.Controls.Add(this._toothSelector);
            // 
            // _splitContainer上部.Panel2
            // 
            this._splitContainer上部.Panel2.BackColor = System.Drawing.Color.Black;
            this._splitContainer上部.Panel2.Controls.Add(this._shinryouDateSelectorElementHost);
            this._splitContainer上部.Panel2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this._splitContainer上部.Size = new System.Drawing.Size(1526, 286);
            this._splitContainer上部.SplitterDistance = 1076;
            this._splitContainer上部.SplitterWidth = 8;
            this._splitContainer上部.TabIndex = 2;
            // 
            // _chkSisyuKanzen
            // 
            this._chkSisyuKanzen.AutoSize = true;
            this._chkSisyuKanzen.BackColor = System.Drawing.Color.PowderBlue;
            this._chkSisyuKanzen.Location = new System.Drawing.Point(10, 6);
            this._chkSisyuKanzen.Margin = new System.Windows.Forms.Padding(6);
            this._chkSisyuKanzen.Name = "_chkSisyuKanzen";
            this._chkSisyuKanzen.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this._chkSisyuKanzen.Size = new System.Drawing.Size(146, 28);
            this._chkSisyuKanzen.TabIndex = 0;
            this._chkSisyuKanzen.Text = "完全一致";
            this._chkSisyuKanzen.UseVisualStyleBackColor = false;
            // 
            // _toothSelector
            // 
            this._toothSelector.BackgroundImage = global::OmoOmotegaki.Properties.Resources.background_dark;
            this._toothSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this._toothSelector.IsMultiSelect = false;
            this._toothSelector.Location = new System.Drawing.Point(0, 0);
            this._toothSelector.Margin = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this._toothSelector.Name = "_toothSelector";
            this._toothSelector.Size = new System.Drawing.Size(1076, 286);
            this._toothSelector.TabIndex = 0;
            this._toothSelector.TabStop = false;
            this.toolTip1.SetToolTip(this._toothSelector, "Ctrlキーを押しながらクリックで複数選択");
            // 
            // _shinryouDateSelectorElementHost
            // 
            this._shinryouDateSelectorElementHost.BackColor = System.Drawing.Color.LightSteelBlue;
            this._shinryouDateSelectorElementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this._shinryouDateSelectorElementHost.Location = new System.Drawing.Point(0, 0);
            this._shinryouDateSelectorElementHost.Margin = new System.Windows.Forms.Padding(6);
            this._shinryouDateSelectorElementHost.Name = "_shinryouDateSelectorElementHost";
            this._shinryouDateSelectorElementHost.Size = new System.Drawing.Size(442, 284);
            this._shinryouDateSelectorElementHost.TabIndex = 8;
            this._shinryouDateSelectorElementHost.TabStop = false;
            this._shinryouDateSelectorElementHost.Text = "_shinryouDateSelectorElementHost";
            this._shinryouDateSelectorElementHost.Child = this._shinryouDateSelector;
            // 
            // _pnlRirekiTableControlBox
            // 
            this._pnlRirekiTableControlBox.AutoSize = true;
            this._pnlRirekiTableControlBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._pnlRirekiTableControlBox.BackColor = System.Drawing.Color.Transparent;
            this._pnlRirekiTableControlBox.Controls.Add(this._pnlJogaiType);
            this._pnlRirekiTableControlBox.Controls.Add(this.panel5);
            this._pnlRirekiTableControlBox.Controls.Add(this.panel6);
            this._pnlRirekiTableControlBox.Controls.Add(this.panel7);
            this._pnlRirekiTableControlBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._pnlRirekiTableControlBox.Location = new System.Drawing.Point(0, 286);
            this._pnlRirekiTableControlBox.Margin = new System.Windows.Forms.Padding(6);
            this._pnlRirekiTableControlBox.Name = "_pnlRirekiTableControlBox";
            this._pnlRirekiTableControlBox.Size = new System.Drawing.Size(1526, 50);
            this._pnlRirekiTableControlBox.TabIndex = 3;
            // 
            // _pnlJogaiType
            // 
            this._pnlJogaiType.BackColor = System.Drawing.Color.SkyBlue;
            this._pnlJogaiType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._pnlJogaiType.Controls.Add(this._cmbJogaiType);
            this._pnlJogaiType.Controls.Add(this._chkGishiJogai);
            this._pnlJogaiType.Controls.Add(this._chkGJogai);
            this._pnlJogaiType.Controls.Add(this._chkPJogai);
            this._pnlJogaiType.Location = new System.Drawing.Point(0, 0);
            this._pnlJogaiType.Margin = new System.Windows.Forms.Padding(0);
            this._pnlJogaiType.Name = "_pnlJogaiType";
            this._pnlJogaiType.Padding = new System.Windows.Forms.Padding(0, 0, 8, 4);
            this._pnlJogaiType.Size = new System.Drawing.Size(376, 50);
            this._pnlJogaiType.TabIndex = 1;
            // 
            // _cmbJogaiType
            // 
            this._cmbJogaiType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._cmbJogaiType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbJogaiType.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this._cmbJogaiType.FormattingEnabled = true;
            this._cmbJogaiType.Items.AddRange(new object[] {
            "除外",
            "のみ"});
            this._cmbJogaiType.Location = new System.Drawing.Point(232, 4);
            this._cmbJogaiType.Margin = new System.Windows.Forms.Padding(0);
            this._cmbJogaiType.Name = "_cmbJogaiType";
            this._cmbJogaiType.Size = new System.Drawing.Size(130, 32);
            this._cmbJogaiType.TabIndex = 3;
            // 
            // _chkGishiJogai
            // 
            this._chkGishiJogai.AutoSize = true;
            this._chkGishiJogai.Location = new System.Drawing.Point(136, 10);
            this._chkGishiJogai.Margin = new System.Windows.Forms.Padding(0);
            this._chkGishiJogai.Name = "_chkGishiJogai";
            this._chkGishiJogai.Size = new System.Drawing.Size(90, 28);
            this._chkGishiJogai.TabIndex = 2;
            this._chkGishiJogai.Text = "義歯";
            this._chkGishiJogai.UseVisualStyleBackColor = true;
            // 
            // _chkGJogai
            // 
            this._chkGJogai.AutoSize = true;
            this._chkGJogai.Location = new System.Drawing.Point(72, 10);
            this._chkGJogai.Margin = new System.Windows.Forms.Padding(0);
            this._chkGJogai.Name = "_chkGJogai";
            this._chkGJogai.Size = new System.Drawing.Size(58, 28);
            this._chkGJogai.TabIndex = 1;
            this._chkGJogai.Text = "G";
            this._chkGJogai.UseVisualStyleBackColor = true;
            // 
            // _chkPJogai
            // 
            this._chkPJogai.AutoSize = true;
            this._chkPJogai.Location = new System.Drawing.Point(10, 10);
            this._chkPJogai.Margin = new System.Windows.Forms.Padding(0);
            this._chkPJogai.Name = "_chkPJogai";
            this._chkPJogai.Size = new System.Drawing.Size(57, 28);
            this._chkPJogai.TabIndex = 0;
            this._chkPJogai.Text = "P";
            this._chkPJogai.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.GhostWhite;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this._cmbShinryouTougou);
            this.panel5.Controls.Add(this.label1);
            this.panel5.Location = new System.Drawing.Point(376, 0);
            this.panel5.Margin = new System.Windows.Forms.Padding(0);
            this.panel5.MinimumSize = new System.Drawing.Size(2, 50);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(302, 50);
            this.panel5.TabIndex = 3;
            // 
            // _cmbShinryouTougou
            // 
            this._cmbShinryouTougou.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._cmbShinryouTougou.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbShinryouTougou.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this._cmbShinryouTougou.FormattingEnabled = true;
            this._cmbShinryouTougou.Location = new System.Drawing.Point(76, 6);
            this._cmbShinryouTougou.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this._cmbShinryouTougou.Name = "_cmbShinryouTougou";
            this._cmbShinryouTougou.Size = new System.Drawing.Size(206, 32);
            this._cmbShinryouTougou.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "統合";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.GhostWhite;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this._cmbSinryouOrder);
            this.panel6.Controls.Add(this.label2);
            this.panel6.Location = new System.Drawing.Point(678, 0);
            this.panel6.Margin = new System.Windows.Forms.Padding(0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(352, 50);
            this.panel6.TabIndex = 2;
            // 
            // _cmbSinryouOrder
            // 
            this._cmbSinryouOrder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._cmbSinryouOrder.Location = new System.Drawing.Point(110, 0);
            this._cmbSinryouOrder.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this._cmbSinryouOrder.Name = "_cmbSinryouOrder";
            this._cmbSinryouOrder.Size = new System.Drawing.Size(234, 48);
            this._cmbSinryouOrder.TabIndex = 1;
            this._cmbSinryouOrder.Text = "elementHost1";
            this._cmbSinryouOrder.Child = this.shinryouOrderTypeSelector1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 14);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 24);
            this.label2.TabIndex = 0;
            this.label2.Text = "並び順";
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.GhostWhite;
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.label3);
            this.panel7.Controls.Add(this._cmbViewerSettings);
            this.panel7.Controls.Add(this.btnAddTemplate);
            this.panel7.Controls.Add(this.btnRemoveTemplate);
            this.panel7.Location = new System.Drawing.Point(1030, 0);
            this.panel7.Margin = new System.Windows.Forms.Padding(0);
            this.panel7.MinimumSize = new System.Drawing.Size(2, 50);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(376, 50);
            this.panel7.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 14);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 24);
            this.label3.TabIndex = 7;
            this.label3.Text = "設定";
            // 
            // _cmbViewerSettings
            // 
            this._cmbViewerSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._cmbViewerSettings.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this._cmbViewerSettings.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this._cmbViewerSettings.DisplayMember = "Name";
            this._cmbViewerSettings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbViewerSettings.DropDownWidth = 500;
            this._cmbViewerSettings.FormattingEnabled = true;
            this._cmbViewerSettings.Location = new System.Drawing.Point(68, 4);
            this._cmbViewerSettings.Margin = new System.Windows.Forms.Padding(0);
            this._cmbViewerSettings.Name = "_cmbViewerSettings";
            this._cmbViewerSettings.Size = new System.Drawing.Size(138, 32);
            this._cmbViewerSettings.TabIndex = 4;
            this.toolTip1.SetToolTip(this._cmbViewerSettings, "表示設定を復帰");
            this._cmbViewerSettings.SelectedIndexChanged += new System.EventHandler(this._cmbViewerSettings_SelectedIndexChanged);
            // 
            // btnAddTemplate
            // 
            this.btnAddTemplate.AutoSize = true;
            this.btnAddTemplate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAddTemplate.Location = new System.Drawing.Point(212, 2);
            this.btnAddTemplate.Margin = new System.Windows.Forms.Padding(0);
            this.btnAddTemplate.Name = "btnAddTemplate";
            this.btnAddTemplate.Size = new System.Drawing.Size(68, 34);
            this.btnAddTemplate.TabIndex = 5;
            this.btnAddTemplate.Text = "保存";
            this.btnAddTemplate.UseVisualStyleBackColor = true;
            this.btnAddTemplate.Click += new System.EventHandler(this.BtnAddTemplate_Click);
            // 
            // btnRemoveTemplate
            // 
            this.btnRemoveTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveTemplate.AutoSize = true;
            this.btnRemoveTemplate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRemoveTemplate.Location = new System.Drawing.Point(300, 2);
            this.btnRemoveTemplate.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.btnRemoveTemplate.Name = "btnRemoveTemplate";
            this.btnRemoveTemplate.Size = new System.Drawing.Size(68, 34);
            this.btnRemoveTemplate.TabIndex = 6;
            this.btnRemoveTemplate.Text = "削除";
            this.btnRemoveTemplate.UseVisualStyleBackColor = true;
            this.btnRemoveTemplate.Click += new System.EventHandler(this.BtnRemoveTemplate_Click);
            // 
            // _syoRirekiListWrap
            // 
            this._syoRirekiListWrap.Dock = System.Windows.Forms.DockStyle.Fill;
            this._syoRirekiListWrap.Location = new System.Drawing.Point(4, 102);
            this._syoRirekiListWrap.Margin = new System.Windows.Forms.Padding(6);
            this._syoRirekiListWrap.Name = "_syoRirekiListWrap";
            this._syoRirekiListWrap.Size = new System.Drawing.Size(1522, 812);
            this._syoRirekiListWrap.TabIndex = 0;
            this._syoRirekiListWrap.Text = "elementHost1";
            this._syoRirekiListWrap.Child = null;
            // 
            // _shinryouListControlPanel
            // 
            this._shinryouListControlPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._shinryouListControlPanel.Controls.Add(this._pnlBtnフィルター追加Wrap);
            this._shinryouListControlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._shinryouListControlPanel.Location = new System.Drawing.Point(4, 0);
            this._shinryouListControlPanel.Margin = new System.Windows.Forms.Padding(6);
            this._shinryouListControlPanel.Name = "_shinryouListControlPanel";
            this._shinryouListControlPanel.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this._shinryouListControlPanel.Size = new System.Drawing.Size(1522, 102);
            this._shinryouListControlPanel.TabIndex = 2;
            // 
            // _pnlBtnフィルター追加Wrap
            // 
            this._pnlBtnフィルター追加Wrap.AutoSize = true;
            this._pnlBtnフィルター追加Wrap.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._pnlBtnフィルター追加Wrap.BackColor = System.Drawing.Color.Transparent;
            this._pnlBtnフィルター追加Wrap.Controls.Add(this._btnフィルター追加);
            this._pnlBtnフィルター追加Wrap.Dock = System.Windows.Forms.DockStyle.Left;
            this._pnlBtnフィルター追加Wrap.Location = new System.Drawing.Point(0, 8);
            this._pnlBtnフィルター追加Wrap.Margin = new System.Windows.Forms.Padding(6);
            this._pnlBtnフィルター追加Wrap.Name = "_pnlBtnフィルター追加Wrap";
            this._pnlBtnフィルター追加Wrap.Padding = new System.Windows.Forms.Padding(4, 0, 8, 4);
            this._pnlBtnフィルター追加Wrap.Size = new System.Drawing.Size(182, 94);
            this._pnlBtnフィルター追加Wrap.TabIndex = 1;
            // 
            // _btnフィルター追加
            // 
            this._btnフィルター追加.AutoSize = true;
            this._btnフィルター追加.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnフィルター追加.BackColor = System.Drawing.SystemColors.Control;
            this._btnフィルター追加.Dock = System.Windows.Forms.DockStyle.Left;
            this._btnフィルター追加.FlatAppearance.BorderSize = 0;
            this._btnフィルター追加.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnフィルター追加.Location = new System.Drawing.Point(4, 0);
            this._btnフィルター追加.Margin = new System.Windows.Forms.Padding(6);
            this._btnフィルター追加.Name = "_btnフィルター追加";
            this._btnフィルター追加.Size = new System.Drawing.Size(170, 90);
            this._btnフィルター追加.TabIndex = 0;
            this._btnフィルター追加.Text = "フィルター追加...";
            this.toolTip1.SetToolTip(this._btnフィルター追加, "新規フィルターの作成 (Ctrl + T)");
            this._btnフィルター追加.UseVisualStyleBackColor = false;
            this._btnフィルター追加.Click += new System.EventHandler(this.新規フィルターの作成ToolStripMenuItem_Click);
            // 
            // picMenuDark
            // 
            this.picMenuDark.BackColor = System.Drawing.Color.DarkGray;
            this.picMenuDark.Dock = System.Windows.Forms.DockStyle.Top;
            this.picMenuDark.Location = new System.Drawing.Point(0, 44);
            this.picMenuDark.Margin = new System.Windows.Forms.Padding(6);
            this.picMenuDark.Name = "picMenuDark";
            this.picMenuDark.Size = new System.Drawing.Size(2666, 4);
            this.picMenuDark.TabIndex = 9;
            this.picMenuDark.TabStop = false;
            // 
            // _btnToggleFilterControlPanel
            // 
            this._btnToggleFilterControlPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnToggleFilterControlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._btnToggleFilterControlPanel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this._btnToggleFilterControlPanel.Location = new System.Drawing.Point(0, 336);
            this._btnToggleFilterControlPanel.Margin = new System.Windows.Forms.Padding(0);
            this._btnToggleFilterControlPanel.Name = "_btnToggleFilterControlPanel";
            this._btnToggleFilterControlPanel.Size = new System.Drawing.Size(1526, 14);
            this._btnToggleFilterControlPanel.TabIndex = 13;
            this._btnToggleFilterControlPanel.TabStop = false;
            this.toolTip1.SetToolTip(this._btnToggleFilterControlPanel, "フィルターコントロールパネルの開閉");
            this._btnToggleFilterControlPanel.UseMnemonic = false;
            this._btnToggleFilterControlPanel.UseVisualStyleBackColor = true;
            this._btnToggleFilterControlPanel.Click += new System.EventHandler(this._btnToggleFilterControlPanel_Click);
            // 
            // splitContainerRight
            // 
            this.splitContainerRight.BackColor = System.Drawing.Color.MediumTurquoise;
            this.splitContainerRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerRight.Location = new System.Drawing.Point(500, 48);
            this.splitContainerRight.Margin = new System.Windows.Forms.Padding(6);
            this.splitContainerRight.Name = "splitContainerRight";
            // 
            // splitContainerRight.Panel1
            // 
            this.splitContainerRight.Panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.splitContainerRight.Panel1.Controls.Add(this._shinryouListPanel);
            this.splitContainerRight.Panel1.Controls.Add(this._btnToggleFilterControlPanel);
            this.splitContainerRight.Panel1.Controls.Add(this._filterControlPanel);
            // 
            // splitContainerRight.Panel2
            // 
            this.splitContainerRight.Panel2.Controls.Add(this._shinryouCheckDisplayElementHost);
            this.splitContainerRight.Size = new System.Drawing.Size(2166, 1268);
            this.splitContainerRight.SplitterDistance = 1526;
            this.splitContainerRight.SplitterWidth = 8;
            this.splitContainerRight.TabIndex = 11;
            // 
            // _shinryouListPanel
            // 
            this._shinryouListPanel.BackColor = System.Drawing.Color.SlateGray;
            this._shinryouListPanel.Controls.Add(this._syoRirekiListWrap);
            this._shinryouListPanel.Controls.Add(this._shinryouListControlPanel);
            this._shinryouListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._shinryouListPanel.Location = new System.Drawing.Point(0, 350);
            this._shinryouListPanel.Margin = new System.Windows.Forms.Padding(6);
            this._shinryouListPanel.Name = "_shinryouListPanel";
            this._shinryouListPanel.Padding = new System.Windows.Forms.Padding(4, 0, 0, 4);
            this._shinryouListPanel.Size = new System.Drawing.Size(1526, 918);
            this._shinryouListPanel.TabIndex = 11;
            // 
            // _filterControlPanel
            // 
            this._filterControlPanel.Controls.Add(this._splitContainer上部);
            this._filterControlPanel.Controls.Add(this._pnlRirekiTableControlBox);
            this._filterControlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._filterControlPanel.Location = new System.Drawing.Point(0, 0);
            this._filterControlPanel.Margin = new System.Windows.Forms.Padding(6);
            this._filterControlPanel.Name = "_filterControlPanel";
            this._filterControlPanel.Size = new System.Drawing.Size(1526, 336);
            this._filterControlPanel.TabIndex = 12;
            // 
            // _shinryouCheckDisplayElementHost
            // 
            this._shinryouCheckDisplayElementHost.BackColor = System.Drawing.Color.GhostWhite;
            this._shinryouCheckDisplayElementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this._shinryouCheckDisplayElementHost.Location = new System.Drawing.Point(0, 0);
            this._shinryouCheckDisplayElementHost.Margin = new System.Windows.Forms.Padding(6);
            this._shinryouCheckDisplayElementHost.Name = "_shinryouCheckDisplayElementHost";
            this._shinryouCheckDisplayElementHost.Size = new System.Drawing.Size(632, 1268);
            this._shinryouCheckDisplayElementHost.TabIndex = 0;
            this._shinryouCheckDisplayElementHost.Text = "_shinryouCheckDisplay";
            this._shinryouCheckDisplayElementHost.Child = this._shinryouCheckDisplay;
            // 
            // データ変換ヤハラtoolStripMenuItem
            // 
            this.データ変換ヤハラtoolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.データ変換ヤハラtoolStripMenuItem.Name = "データ変換ヤハラtoolStripMenuItem";
            this.データ変換ヤハラtoolStripMenuItem.Size = new System.Drawing.Size(197, 36);
            this.データ変換ヤハラtoolStripMenuItem.Text = "データ変換 ヤハラ";
            this.データ変換ヤハラtoolStripMenuItem.Click += new System.EventHandler(this.データ変換ヤハラToolStripMenuItem_Click);
            // 
            // OmotegakiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(2666, 1372);
            this.Controls.Add(this.splitContainerRight);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.picMenuDark);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MinimumSize = new System.Drawing.Size(2044, 1345);
            this.Name = "OmotegakiForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabKikan.ResumeLayout(false);
            this.tabPage初診リスト.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabPage期間指定.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this._pnlKarteLoaderWrap.ResumeLayout(false);
            this._pnlKarteLoader.ResumeLayout(false);
            this._pnlKarteIdInput.ResumeLayout(false);
            this._pnlTxtKarteBangouWrap.ResumeLayout(false);
            this._pnlTxtKarteBangouWrap.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPanelRightDark1)).EndInit();
            this.tabInfo.ResumeLayout(false);
            this.tabPage患者情報.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPanelBottomDark)).EndInit();
            this._splitContainer上部.Panel1.ResumeLayout(false);
            this._splitContainer上部.Panel1.PerformLayout();
            this._splitContainer上部.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer上部)).EndInit();
            this._splitContainer上部.ResumeLayout(false);
            this._pnlRirekiTableControlBox.ResumeLayout(false);
            this._pnlJogaiType.ResumeLayout(false);
            this._pnlJogaiType.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this._shinryouListControlPanel.ResumeLayout(false);
            this._shinryouListControlPanel.PerformLayout();
            this._pnlBtnフィルター追加Wrap.ResumeLayout(false);
            this._pnlBtnフィルター追加Wrap.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMenuDark)).EndInit();
            this.splitContainerRight.Panel1.ResumeLayout(false);
            this.splitContainerRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRight)).EndInit();
            this.splitContainerRight.ResumeLayout(false);
            this._shinryouListPanel.ResumeLayout(false);
            this._filterControlPanel.ResumeLayout(false);
            this._filterControlPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabInfo;
        private System.Windows.Forms.TabPage tabPage患者情報;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DateTimePicker _dateKikanStart;
        private System.Windows.Forms.TextBox txtKarteBangou;
        private System.Windows.Forms.ComboBox cmbSinryoujo;
        private System.Windows.Forms.Button btnLoadKarte;
        private System.Windows.Forms.CheckBox chkKikanStart;
        private System.Windows.Forms.DateTimePicker _dateKikanEnd;
        private System.Windows.Forms.Button _btnKikanYear;
        private System.Windows.Forms.Button _btnKikanDay;
        private System.Windows.Forms.Button _btnKikanMonth;
        private System.Windows.Forms.CheckBox chkKikanEnd;
        private System.Windows.Forms.TabControl tabKikan;
        private System.Windows.Forms.TabPage tabPage初診リスト;
        private System.Windows.Forms.TabPage tabPage期間指定;
        private System.Windows.Forms.ToolStripMenuItem 診療情報ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ファイルToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem データフォルダーの変更ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 最近使ったデータフォルダーToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 閉じるToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新規フィルターの作成NToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 全てのフィルターを削除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel説明;
        private System.Windows.Forms.ToolStripMenuItem ヘルプToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel statusLabelカルテタイトル;
        private System.Windows.Forms.ToolStripMenuItem ツールToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 診療録作成ソフトのリンクを解除ToolStripMenuItem;
        private System.Windows.Forms.PictureBox picPanelRightDark1;
        private System.Windows.Forms.PictureBox picMenuDark;
        private System.Windows.Forms.ToolStripMenuItem 操作説明ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem データフォルダーToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem アプリの設定ToolStripMenuItem;
        private System.Windows.Forms.Panel _pnlKarteLoader;
        private System.Windows.Forms.ToolStripMenuItem メッセージログToolStripMenuItem;
        private System.Windows.Forms.PictureBox picPanelBottomDark;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button _btnCalendarTekiyou;
        private Controls.KarteDataDisp karteDataDisp1;
        private System.Windows.Forms.ToolStripMenuItem ウィンドウToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem 新規ウィンドウToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem すべてのウィンドウを閉じるToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem カルテToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem カルテ印刷ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem カルテルール管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem カルテ一括印刷ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem カルテ印刷プレビューToolStripMenuItem;
        private System.Windows.Forms.CheckBox _chkSisyuKanzen;
        private System.Windows.Forms.ToolStripMenuItem 表書きToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 表書き印刷ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 表書き印刷プレビューToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 診療情報_印刷ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 診療情報印刷プレビューToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 歯別診療情報ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 歯別診療情報_印刷ToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox _cmbShinryouTougou;
        private System.Windows.Forms.Integration.ElementHost _cmbSinryouOrder;
        private Controls.ShinryouOrderTypeSelector shinryouOrderTypeSelector1;
        private System.Windows.Forms.Panel _pnlJogaiType;
        private System.Windows.Forms.ComboBox _cmbJogaiType;
        private System.Windows.Forms.CheckBox _chkGishiJogai;
        private System.Windows.Forms.CheckBox _chkGJogai;
        private System.Windows.Forms.CheckBox _chkPJogai;
        private Controls.ToothSelector _toothSelector;
        private System.Windows.Forms.SplitContainer _splitContainer上部;
        private System.Windows.Forms.Integration.ElementHost _shinryouDateSelectorElementHost;
        private Controls.ShinryouDateSelector _shinryouDateSelector;
        private System.Windows.Forms.ToolStripMenuItem 診療情報LabelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 歯別診療情報_複数カルテ印刷ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ComboBox _cmbViewerSettings;
        private System.Windows.Forms.Button btnRemoveTemplate;
        private System.Windows.Forms.Button btnAddTemplate;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Button _btnすべての初診リストのチェックを外す;
        private System.Windows.Forms.Button _btnすべての初診リストにチェック;
        private System.Windows.Forms.SplitContainer splitContainerRight;
        private System.Windows.Forms.CheckBox _chk最新の期間のみチェック;
        private System.Windows.Forms.Integration.ElementHost _syoRirekiListWrap;
        private System.Windows.Forms.Integration.ElementHost _shoshinKikanListElementHost;
        private Controls.ShoshinKikanList _shoshinKikanList;
        private System.Windows.Forms.Integration.ElementHost _shinryouCheckDisplayElementHost;
        private Controls.ShinryouCheckDisplay _shinryouCheckDisplay;
        private System.Windows.Forms.Panel _shinryouListControlPanel;
        private System.Windows.Forms.Button _btnフィルター追加;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel _pnlRirekiTableControlBox;
        private System.Windows.Forms.Panel _pnlBtnフィルター追加Wrap;
        private System.Windows.Forms.Panel _shinryouListPanel;
        private System.Windows.Forms.Panel _filterControlPanel;
        private System.Windows.Forms.Button _btnToggleFilterControlPanel;
        private System.Windows.Forms.Panel _pnlKarteIdInput;
        private System.Windows.Forms.Panel _pnlKarteLoaderWrap;
        private System.Windows.Forms.Panel _pnlTxtKarteBangouWrap;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripMenuItem データフォルダ共通設定ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem データ変換ヤハラtoolStripMenuItem;
    }
}

