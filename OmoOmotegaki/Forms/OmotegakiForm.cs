using OmoEReceLib.ERObjects;
using OmoOmotegaki.Controls;
using OmoOmotegaki.Data;
using OmoOmotegaki.Models;
using OmoOmotegaki.ViewModels;
using OmoOmotegaki.ViewModels.Controls;
using OmoSeitoku;
using OmoSeitoku.Controls;
using OmoSeitoku.Forms;
using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace OmoOmotegaki.Forms
{
    // （作業順）
    //TODO フィルター　病名・処置のリストを出してそこから選択（そのリストをフィルタリングできるようにする）
    //TODO フィルター　病名・処置の複数設定可能にする。それぞれで一致方法、否定か否かの設定。
    //TODO フィルター　病名・処置の否定項目　プロパティだけ先に作ってしまったので編集コントロールの実装を上のTODOと併せて行う。
    //TODO フィルター　病名・処置はIDもフィルタリングできるように。

    [SuppressMessage("スタイル", "IDE1006:命名スタイル", Justification = "<保留中>")]
    public partial class OmotegakiForm : Form, WindowLink.IWindow
    {
        #region Field

        private const string FORM_TITLE = "カルテ入力";
        private const int 最近使ったデータフォルダーの保存数 = 5;
        // 共通診療フィルター 保存用診療所名
        private const int COMMON_FILTER_KARTE_NO = 1;

        // パラメータ変更時等に再取得されるので、スロットル機構を使用して待機してから実行させている。
        private static readonly TimeSpan _カルテデータ取得の実行待機間隔 = TimeSpan.FromMilliseconds(1000);
        private static readonly Shinryoujo CommonFilterShinryoujo = new Shinryoujo("__common");


        private readonly OmotegakiFormViewModel _vm = new OmotegakiFormViewModel();
        private readonly ShinryouCheckDisplayViewModel _shinryouCheckDisplayVM;

        private int? _initialSettings_ShinryoujoTeisuu;
        private SinryouFilter _commonFilter;
        /// <summary>
        /// フォーム終了時に設定をセーブするかどうか
        /// </summary>
        private bool _saveSettingsOnClose;
        private bool _suspendUpdateKarteData;
        private bool _suspendUpdateCommonFilter;
        private int _filterHistoryLength = 1;
        private readonly KartePrintDesign _kartePrint = new KartePrintDesign();
        private readonly Regex _txtKarteBangouFilter = new Regex("[^0-9]");
        private readonly WindowLink _windowLink;

        #region Disposable

        private SinryouFilterTabPage _filterInfoPage;
        private KarteImage _karteImage = new KarteImage();
        private SyochiRirekiListWpf _syochiRirekiList;
        private TabControl _tabHaByoSyo;
        private TabPage _tabPageHaByoSyoFirst;
        private TabPage _tabPageHaByoSyoPrev;
        private Toast _toast;

        #endregion

        #endregion

        #region Constructor, Destructor

        public OmotegakiForm()
        {
            InitializeComponent();

            _shinryouCheckDisplayVM = new ShinryouCheckDisplayViewModel(_vm);
            _shinryouCheckDisplay.DataContext = _shinryouCheckDisplayVM;

            _cmbShinryouTougou.DataSource = Enum.GetValues(typeof(SinryouDataLoader.診療統合種別));
            _windowLink = new WindowLink(this);
            _cmbJogaiType.SelectedIndex = SinryouFilter.除外種別_除外;
            _shinryouDateSelector.SelectedItemChanged += _shinryouDateSelector_SelectedIndexChanged;

            SetupBindings();
            SetupEventHandlers();

            // ウィンドウタイトルを作成する
            _vm.TitleBase = $"{FORM_TITLE} [Ver. {Program.Version}]";
        }

        ~OmotegakiForm()
        {
            // イベントの解除

            OmotegakiSettings.Instance.SettingsChange -= OmotegakiSettings_SettingsChange;

            global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolderChanged -= CommonSettings_DataFolderChanged;
        }

        #endregion

        private void SetupBindings()
        {
            DataBindings.Add(nameof(Text), _vm, nameof(_vm.Title));

            _shoshinKikanList.SetBinding(
                ShoshinKikanList.ItemsSourceProperty,
                new System.Windows.Data.Binding(nameof(_vm.ShoshinKikanItems))
                {
                    Source = _vm
                });
        }

        private void SetupControls()
        {
            OmotegakiSettings.Instance.PerformSettingsChange();

            Clear();
            _vm.Clear();

            ToolStripMenuItem msgLog = メッセージログToolStripMenuItem;
            msgLog.Enabled = false;
            msgLog.DropDown.Opening += delegate
            {
                ToolStripMenuItem m = メッセージログToolStripMenuItem;
                m.DropDown.MaximumSize = new Size(0, Screen.GetBounds(m.GetCurrentParent()).Height / 5 * 4);
            };
            msgLog.DropDown.ItemClicked += (_, e) =>
            {
                var item = (ToolStripMenuItem)e.ClickedItem;

                メッセージログToolStripMenuItem.HideDropDown();

                var msg = new StringBuilder();

                msg.AppendLine(item.ToolTipText).
                    AppendLine().
                    AppendLine("---------------------------------------------");

                if (item.Checked)
                {
                    msg.AppendLine("(コピー済)");
                }

                msg.AppendLine("メッセージをクリップボードにコピーしますか？");

                if (MessageBox.Show(msg.ToString(), "メッセージ ログ", MessageBoxButtons.YesNo)
                        == DialogResult.Yes)
                {
                    Clipboard.SetText(item.ToolTipText);
                    item.Checked = true;

                    ShowMessage("メッセージをクリップボードにコピーしました。", false, false);
                }
            };


            _toast.ToastPosition = Toast.Position.Top;
            Controls.Add(_toast);



            void ステータスラベルにツールチップの内容を表示(object sender, EventArgs _)
            {
                statusLabel説明.Text = CreateText(sender);

                string CreateText(object sender)
                {
                    switch (sender)
                    {
                        case Control ctrl:
                            return toolTip1.GetToolTip(ctrl);

                        case ToolStripItem toolStripItem:
                            return toolStripItem.ToolTipText;

                        case System.Windows.FrameworkElement element:
                            if (element.ToolTip is object)
                            {
                                element.Tag = element.ToolTip.ToString();
                                element.ToolTip = null;
                            }
                            return element.Tag?.ToString() ?? string.Empty;

                        default:
                            throw new Exception("ツールチップテキストが取得できません。");
                    }
                }
            }

            void ステータスラベルを初期化(object _, EventArgs __)
            {
                statusLabel説明.Text = string.Empty;
            }


            // リンク中のカルテ読み込みパネルの代わりに表示されるパネル
            var pnlKarteLink = new Panel()
            {
                Bounds = _pnlKarteLoader.Bounds,
            };
            {
                Control refCtrl;

                refCtrl = cmbSinryoujo;

                var linkSinryoujo = new Label()
                {
                    Anchor = refCtrl.Anchor,
                    Bounds = refCtrl.Bounds,
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = refCtrl.Font,
                };
                toolTip1.SetToolTip(linkSinryoujo, "診療所　(診療録作成ソフトとリンク中)");
                linkSinryoujo.MouseMove += ステータスラベルにツールチップの内容を表示;
                linkSinryoujo.MouseLeave += ステータスラベルを初期化;
                //
                cmbSinryoujo.SelectedIndexChanged += (sender, evt) =>
                {
                    var item = (KeyValuePair<string, string>)((ComboBox)sender).SelectedItem;

                    linkSinryoujo.Text = item.Key;

                    txtKarteBangou.Focus();
                };


                refCtrl = txtKarteBangou;

                var linkKarteNum = new Label()
                {
                    Anchor = refCtrl.Anchor,
                    Bounds = refCtrl.Bounds,
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = refCtrl.Font,
                };
                toolTip1.SetToolTip(linkKarteNum, "カルテ番号　(診療録作成ソフトとリンク中)");
                linkKarteNum.MouseMove += ステータスラベルにツールチップの内容を表示;
                linkKarteNum.MouseLeave += ステータスラベルを初期化;
                //
                {
                    var binding = new Binding(nameof(txtKarteBangou.Text), _vm, nameof(_vm.CurrentKarteId));
                    binding.Format += (_, e) =>
                    {
                        // KarteId から String へ
                        e.Value = (e.Value is KarteId karteId)
                            ? karteId.KarteNumber.ToString()
                            : "";
                    };
                    binding.Parse += (_, e) =>
                    {
                        // String から KarteId へ
                        if (cmbSinryoujo.SelectedValue is null)
                        {
                            e.Value = null;
                        }
                        else
                        {
                            var shinryoujo = new Shinryoujo(cmbSinryoujo.SelectedValue.ToString());
                            string inputText = (string)e.Value;
                            e.Value = int.TryParse(inputText, out int karteNumber) && (karteNumber > 0)
                                ? new KarteId(shinryoujo, karteNumber)
                                : null;
                        }
                    };
                    txtKarteBangou.DataBindings.Add(binding);
                }
                txtKarteBangou.TextChanged += (sender, _) =>
                {
                    linkKarteNum.Text = ((TextBox)sender).Text;
                };


                refCtrl = btnLoadKarte;

                var btn = new Button()
                {
                    Anchor = refCtrl.Anchor,
                    Bounds = refCtrl.Bounds,
                    Font = refCtrl.Font,
                    Text = "切断",
                };
                btn.Click += delegate
                {
                    var menuItem = 診療録作成ソフトのリンクを解除ToolStripMenuItem;

                    if (menuItem.Enabled)
                        menuItem.PerformClick();
                };
                toolTip1.SetToolTip(btn, "診療録作成ソフトとのリンクを解除");
                btn.MouseMove += ステータスラベルにツールチップの内容を表示;
                btn.MouseLeave += ステータスラベルを初期化;

                toolTip1.SetToolTip(btnAddTemplate, "設定テンプレートを追加");
                btnAddTemplate.MouseMove += ステータスラベルにツールチップの内容を表示;
                btnAddTemplate.MouseLeave += ステータスラベルを初期化;

                toolTip1.SetToolTip(btnRemoveTemplate, "設定テンプレートを削除");
                btnRemoveTemplate.MouseMove += ステータスラベルにツールチップの内容を表示;
                btnRemoveTemplate.MouseLeave += ステータスラベルを初期化;


                toolTip1.SetToolTip(_chkSisyuKanzen, "選択歯式に完全一致する診療のみ表示する場合はチェック");
                _chkSisyuKanzen.MouseMove += ステータスラベルにツールチップの内容を表示;
                _chkSisyuKanzen.MouseLeave += ステータスラベルを初期化;

                pnlKarteLink.Controls.AddRange(new Control[] { linkSinryoujo, linkKarteNum, btn });
            }
            _pnlKarteLoader.Parent.Controls.Add(pnlKarteLink);
            _pnlKarteLoader.BringToFront();


            // 診療リスト コントロールパネル
            {
                // 診療リスト変更用 タブコントロール
                {
                    _tabHaByoSyo.Dock = DockStyle.Fill;

                    _tabHaByoSyo.SelectedIndexChanged += tabHaByoSyo_SelectedIndexChanged;
                    {
                        _tabPageHaByoSyoFirst.BackColor = panel1.BackColor;
                        _tabHaByoSyo.TabPages.Add(_tabPageHaByoSyoFirst);
                    }

                    _shinryouListControlPanel.Controls.Add(_tabHaByoSyo);

                    // 新規フィルター作成ボタンを修正
                    _tabHaByoSyo.BringToFront();
                }

                _shinryouListControlPanel.Height = _tabHaByoSyo.GetTabRect(0).Height + 8;
            }


            _tabPageHaByoSyoFirst.Tag = new TabHaByoSyoPageData();


            CreateKarteImage();

            var sinryoujoDict = new Dictionary<string, string> {
                { "本院", "hon" },
                { "分院", "bun" },
                { "試験", "tes" },
            };
            cmbSinryoujo.SelectedIndexChanged += delegate
            {
                if (0 <= cmbSinryoujo.SelectedIndex)
                {
                    Color[] sinryoujoColors = new[] {
                        Color.SteelBlue, Color.MediumSeaGreen,
                        Color.Tomato, Color.Gold, Color.Violet
                    };

                    _pnlKarteIdInput.BackColor =
                            sinryoujoColors[cmbSinryoujo.SelectedIndex % sinryoujoColors.Length];

                    sinryoujoColors = null;
                }
            };
            cmbSinryoujo.DataSource = new BindingSource(sinryoujoDict, null);
            cmbSinryoujo.DisplayMember = "Key";
            cmbSinryoujo.ValueMember = "Value";


            // 診療一覧テーブル

            _toothSelector.HaButtonClick += (sender, ev) =>
            {
                // 診療リストにフィルターをセットする。
                {
                    var list = _toothSelector.CheckedHaButton歯種.ToList();
                    if (list.Contains(ev.歯種))
                    {
                        list.Remove(ev.歯種);
                    }
                    else
                    {
                        list.Add(ev.歯種);
                    }
                    _toothSelector.CheckedHaButton歯種 = list;
                }
            };
            _toothSelector.CheckedHaButton歯種Changed += delegate { UpdateCommonFilter(); };


            // 診療一覧テーブル コントロールボックス
            void updateCommonFilter(object _, EventArgs __) => UpdateCommonFilter();
            _chkSisyuKanzen.CheckedChanged += updateCommonFilter;
            _chkPJogai.CheckedChanged += updateCommonFilter;
            _chkGJogai.CheckedChanged += updateCommonFilter;
            _chkGishiJogai.CheckedChanged += updateCommonFilter;
            _cmbJogaiType.SelectedIndexChanged += updateCommonFilter;
            _cmbShinryouTougou.SelectedIndexChanged += delegate { UpdateKarteData(false); };
            ((ShinryouOrderTypeSelector)_cmbSinryouOrder.Child).SelectedValueChanged += updateCommonFilter;


            // 診療リスト コントロール

            _syoRirekiListWrap.BackgroundImage = panel1.BackgroundImage;
            _syoRirekiListWrap.BackgroundImageLayout = panel1.BackgroundImageLayout;
            _syoRirekiListWrap.Child = _syochiRirekiList;

            _syochiRirekiList.ErrorMessage += (sender, evt) =>
            {
                _ = Task.Run(() =>
                {
                    Thread.Sleep(300);
                    Invoke((Action)delegate
                    {
                        try
                        {
                            ShowMessage(evt.Message, true, true);
                        }
                        catch (Win32Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }
                    });
                });
            };
            _syochiRirekiList.ItemStartDateClick += (_, e) =>
            {
                DateTime defaultDate = (e.Date == default) ? DateTime.Now : e.Date;

                EditDate(e.Index, defaultDate, true);
            };
            _syochiRirekiList.ItemEndDateClick += (_, e) =>
            {
                DateTime defaultDate = (e.Date == default) ? DateTime.Now : e.Date;

                EditDate(e.Index, defaultDate, false);
            };
            _syochiRirekiList.KeyDown += (_, e) =>
            {
                KeyEventArgs keyEvent = new KeyEventArgs(
                                            (Keys)System.Windows.Input.KeyInterop.VirtualKeyFromKey(e.Key));
                OnKeyDown(keyEvent);
                e.Handled = keyEvent.Handled;
            };



            // 期間指定ボックス

            _btnKikanDay.Click += delegate
            {
                _dateKikanEnd.Value = _dateKikanStart.Value;
            };
            _btnKikanMonth.Click += delegate
            {
                _dateKikanEnd.Value = _dateKikanStart.Value.AddMonths(1);
            };
            _btnKikanYear.Click += delegate
            {
                _dateKikanEnd.Value = _dateKikanStart.Value.AddYears(1);
            };

            _dateKikanStart.ValueChanged += delegate
            {
                _dateKikanStart.CustomFormat =
                    string.Format("({0}) yyyy-MM-dd", OmoEReceLib.ERDateTime.GetEraYear(_dateKikanStart.Value, true).Replace(" ", ""));
            };
            _dateKikanEnd.ValueChanged += delegate
            {
                _dateKikanEnd.CustomFormat =
                    string.Format("({0}) yyyy-MM-dd", OmoEReceLib.ERDateTime.GetEraYear(_dateKikanEnd.Value, true).Replace(" ", ""));
            };


            var kikanDummyTxt = new TextBox()
            {
                Text = "期間指定: 最初",
                TextAlign = HorizontalAlignment.Center,
                TabStop = false,
                ReadOnly = true,
                Bounds = _dateKikanStart.Bounds,
                Cursor = Cursors.Hand,
            };
            kikanDummyTxt.Click += delegate { chkKikanStart.Checked = true; };
            _dateKikanStart.Parent.Controls.Add(kikanDummyTxt);
            _dateKikanStart.Tag = kikanDummyTxt.Text;
            kikanDummyTxt.SendToBack();
            chkKikanStart.Checked = !chkKikanStart.Checked;
            chkKikanStart.Checked = !chkKikanStart.Checked;
            chkKikanStart.Cursor = Cursors.Hand;
            _dateKikanStart.Format = DateTimePickerFormat.Custom;
            _dateKikanStart.Value = DateTime.Today;
            //
            //
            kikanDummyTxt = new TextBox()
            {
                Text = "期間指定: 最後",
                TextAlign = HorizontalAlignment.Center,
                TabStop = false,
                ReadOnly = true,
                Bounds = _dateKikanEnd.Bounds,
                Cursor = Cursors.Hand,
            };
            kikanDummyTxt.Click += delegate { chkKikanEnd.Checked = true; };
            _dateKikanEnd.Parent.Controls.Add(kikanDummyTxt);
            _dateKikanEnd.Tag = kikanDummyTxt.Text;
            kikanDummyTxt.SendToBack();
            chkKikanEnd.Checked = !chkKikanEnd.Checked;
            chkKikanEnd.Checked = !chkKikanEnd.Checked;
            chkKikanEnd.Cursor = Cursors.Hand;
            _dateKikanEnd.Format = DateTimePickerFormat.Custom;
            _dateKikanEnd.Value = DateTime.Today;


            // Menu item

            診療情報ToolStripMenuItem.DropDownOpening += delegate
            {
                全てのフィルターを削除ToolStripMenuItem.Enabled = 1 < _tabHaByoSyo.TabCount;
            };

            データフォルダーToolStripMenuItem.Click += delegate
            {
                try
                {
                    Clipboard.SetText(global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder);
                    ShowMessage("データフォルダー パスをクリップボードにコピーしました。", false, false);
                }
                catch (Exception ex)
                {
                    ShowMessage("フォルダーパス文字列のコピーに失敗しました。 エラー： " + ex.Message, true, false);
                }
            };

            // Setup tooltip
            {
                Control control;

                control = _tabHaByoSyo;
                toolTip1.SetToolTip(control, "診療リストフィルター   (切り替え: Ctrl + Tab,  逆順: Ctrl + Shift + Tab)");
                control.MouseMove += ステータスラベルにツールチップの内容を表示;
                control.MouseLeave += ステータスラベルを初期化;

                control = _dateKikanStart;
                toolTip1.SetToolTip(control, _dateKikanStart.Tag as string);
                control.MouseMove += ステータスラベルにツールチップの内容を表示;
                control.MouseLeave += ステータスラベルを初期化;

                control = _dateKikanEnd;
                toolTip1.SetToolTip(control, _dateKikanEnd.Tag as string);
                control.MouseMove += ステータスラベルにツールチップの内容を表示;
                control.MouseLeave += ステータスラベルを初期化;

                control = txtKarteBangou;
                toolTip1.SetToolTip(control, "カルテ番号   (フォーカス: F1キー)");
                control.MouseMove += ステータスラベルにツールチップの内容を表示;
                control.MouseLeave += ステータスラベルを初期化;

                control = cmbSinryoujo;
                toolTip1.SetToolTip(control, "診療所");
                control.MouseMove += ステータスラベルにツールチップの内容を表示;
                control.MouseLeave += ステータスラベルを初期化;

                control = btnLoadKarte;
                toolTip1.SetToolTip(control, "カルテ読込  (押下: Enterキー)");
                control.MouseMove += ステータスラベルにツールチップの内容を表示;
                control.MouseLeave += ステータスラベルを初期化;

                control = _shoshinKikanListElementHost;
                toolTip1.SetToolTip(control, "チェックを入れた期間のみ診療リスト画面に表示されます。");
                control.MouseMove += ステータスラベルにツールチップの内容を表示;
                control.MouseLeave += ステータスラベルを初期化;

                control = _cmbShinryouTougou;
                toolTip1.SetToolTip(control, "指定した期間内で同一歯式の診療を統合");
                control.MouseMove += ステータスラベルにツールチップの内容を表示;
                control.MouseLeave += ステータスラベルを初期化;

                データフォルダーToolStripMenuItem.ToolTipText = "データフォルダー (クリックでクリップボードにフォルダーパスをコピー)";
                データフォルダーToolStripMenuItem.MouseMove += ステータスラベルにツールチップの内容を表示;
                データフォルダーToolStripMenuItem.MouseLeave += ステータスラベルを初期化;

                // ElementHost
                ElementHost elementHost = _shinryouDateSelectorElementHost;
                elementHost.HostContainer.Tag = "クリックした日付の診療へスクロール";
                elementHost.HostContainer.MouseMove += ステータスラベルにツールチップの内容を表示;
                elementHost.HostContainer.MouseLeave += ステータスラベルを初期化;
            }


            ステータスラベルを初期化(this, EventArgs.Empty);
            _vm.StatusLabelカルテタイトル = "-";
        }

        private void SetupEventHandlers()
        {
            _vm.KarteDataLoaded += delegate { karteDataDisp1.KarteData = _vm.CurrentKarteData; };
            _vm.ShowMessage += (_, e) => { ShowMessage(e.Message, e.IsError, e.IsLogging); };
            _vm.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(_vm.StatusLabelカルテタイトル))
                {
                    statusLabelカルテタイトル.Text = _vm.StatusLabelカルテタイトル;
                }
            };

            Disposed += delegate
            {
                if (_karteImage is object)
                {
                    try { _karteImage.Dispose(); } catch { }
                    _karteImage = null;
                }
            };

            Load += delegate
            {
                // イベントの登録

                OmotegakiSettings.Instance.SettingsChange += OmotegakiSettings_SettingsChange;

                global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolderChanged += CommonSettings_DataFolderChanged;
            };

            // 初診リストアイテムの選択状態が変更された時　（チェック状態ではない）
            _shoshinKikanList.SelectedItemChanged += delegate
            {
                if (_shoshinKikanList.SelectedItem is null) return;

                // 処置履歴リストを選択された初診日にスクロール
                ShoshinKikanItem selectedItem = (ShoshinKikanItem)_shoshinKikanList.SelectedItem;
                _syochiRirekiList.ScrollListTo(selectedItem.Date);
            };

            // 初診リストアイテムのチェック状態が変更された時
            _vm.CheckedShoshinKikanItemsChanged += delegate
            {
                if (_chk最新の期間のみチェック.Checked &&
                    // 初診リストの最新の要素以外でチェックされた要素がある場合
                    (_vm.ShoshinKikanItems.FirstOrDefault(p => p.IsChecked) != _vm.ShoshinKikanItems.LastOrDefault()))
                {
                    _chk最新の期間のみチェック.Checked = false;
                }

                UpdateKarteData(false);
            };
        }

        private void Clear()
        {
            ClearFilter();

            _toothSelector.SetData(null);
            _syochiRirekiList.SetData(null);
            txtKarteBangou.Clear();

            karteDataDisp1.Clear();
        }
        private void CreateKarteImage()
        {
            //TODO カルテ画像ハードコード
            //string imgPath = @"..\..\..\Omotegaki\images\karte_small.png";
            //Image img = Image.FromFile(imgPath);

            //TODO カルテ画像ハードコード
            //imgPath = @"..\..\..\Omotegaki\images\karte_ha.png";
            //pictureBox2.Image = Image.FromFile(imgPath);


            //using (Bitmap srcBmp = (Bitmap)Bitmap.FromFile(imgPath))
            //{
            //    {
            //        Rectangle srcRect = new Rectangle(0, 0, srcBmp.Width, 840);

            //        Bitmap bmp = new Bitmap(srcRect.Width, srcRect.Height);

            //        using (Graphics g = Graphics.FromImage(bmp))
            //        {
            //            g.DrawImage(srcBmp, 0, 0, srcRect, GraphicsUnit.Pixel);
            //        }

            //        _karteImage.KanjaInfo = bmp;
            //    }
            //    {
            //        Rectangle srcRect = new Rectangle(0, 840, 1035, 1450);

            //        Bitmap bmp = new Bitmap(srcRect.Width, srcRect.Height);

            //        using (Graphics g = Graphics.FromImage(bmp))
            //        {
            //            g.DrawImage(srcBmp, 0, 0, srcRect, GraphicsUnit.Pixel);
            //        }

            //        _karteImage.SinryouList = bmp;
            //    }
            //}
        }
        private void EditDate(int receiptIndex, DateTime defaultDate, bool isStart)
        {
            while (true)
            {
                string prompt = (isStart ? "開始日付変更" : "終了日付変更");

                string res = Microsoft.VisualBasic.Interaction.InputBox(prompt, "", defaultDate.ToShortDateString());
                if (!string.IsNullOrEmpty(res))
                {
                    try
                    {
                        DateTime date = DateTime.Parse(res);
                        if (isStart)
                            _syochiRirekiList.EditStartDate(receiptIndex, date);
                        else
                            _syochiRirekiList.EditEndDate(receiptIndex, date);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        continue;
                    }
                }
                break;
            }
        }
        /// <summary>
        /// 期間設定コントロールから期間を取得する。
        /// </summary>
        /// <returns></returns>
        private List<DateRange2> GetKikan()
        {
            if (tabKikan.SelectedTab == tabPage初診リスト)
            {
                return _vm.GetKikanBySyosinList();
            }
            if (tabKikan.SelectedTab == tabPage期間指定)
            {
                return GetKikanByCalendar();
            }
            throw new Exception("未対応の期間設定タブページ");
        }

        /// <summary>
        /// カルテ情報を読み込む
        /// </summary>
        /// <param name="karteNumber">カルテ番号（患者番号）</param>
        /// <param name="isRefresh">同一カルテ番号での再読み込みかどうか</param>
        /// <param name="uncheckSyosinList">初診リストのチェック状態を復帰しない</param>
        /// <returns>成功時はtrue</returns>
        private bool LoadKarteData(int karteNumber, bool isRefresh, bool uncheckSyosinList = false)
        {
            if (karteNumber == 0)
            {
                Clear();
                _vm.Clear();
                return true;
            }

            if (karteNumber < 0)
            {
                ShowMessage("患者番号が範囲外です。", true, true);

                return false;
            }

            if (_vm.IsKarteLoading) return false;

            try
            {
                _suspendUpdateCommonFilter = true;
                _suspendUpdateKarteData = true;
                splitContainer1.Enabled = false;
                Cursor = Cursors.WaitCursor;

                Clear();

                KarteId karteId = new KarteId(
                    new Shinryoujo(cmbSinryoujo.SelectedValue.ToString()),
                    karteNumber);
                var shinryouTougou = (SinryouDataLoader.診療統合種別)_cmbShinryouTougou.SelectedItem;
                DateRange2 allTargetKikan;

                if (!isRefresh || _chk最新の期間のみチェック.Checked)
                {
                    _chk最新の期間のみチェック.Checked = true;

                    _vm.LoadDataLatestShoshinKikan(karteId, isRefresh, shinryouTougou, uncheckSyosinList, out DateRange targetKikan);
                    allTargetKikan = new DateRange2(targetKikan);
                }
                else
                {
                    var targetKikans = isRefresh ? GetKikan() : new List<DateRange2> { DateRange2.Infinite };

                    _vm.LoadData(karteId, isRefresh, targetKikans, shinryouTougou, uncheckSyosinList);

                    if (targetKikans.Count == 0)
                    {
                        allTargetKikan = DateRange2.Infinite;
                    }
                    else
                    {
                        DateTime? min = DateTime.MaxValue;
                        DateTime? max = DateTime.MinValue;
                        // nullが含まれていたら値はnullになる。
                        foreach (DateRange2 kikanInput in targetKikans)
                        {
                            if (min.HasValue)
                            {
                                min = !kikanInput.Min.HasValue
                                    ? null
                                    : (kikanInput.Min < min ? kikanInput.Min : min);
                            }

                            if (max.HasValue)
                            {
                                max = !kikanInput.Max.HasValue
                                    ? null
                                    : (max < kikanInput.Max ? kikanInput.Max : max);
                            }
                        }
                        allTargetKikan = new DateRange2(min, max);
                    }
                }

                // 期間指定コントロールに反映
                {
                    chkKikanStart.Checked = allTargetKikan.Min.HasValue;
                    chkKikanEnd.Checked = allTargetKikan.Max.HasValue;
                    if (allTargetKikan.Min.HasValue) _dateKikanStart.Value = allTargetKikan.Min.Value;
                    if (allTargetKikan.Max.HasValue) _dateKikanEnd.Value = allTargetKikan.Max.Value;
                }


                // 診療リストタブコントロールの情報をリセット
                foreach (TabPage tabPage in _tabHaByoSyo.TabPages)
                {
                    var pageData = (TabHaByoSyoPageData)tabPage.Tag;

                    pageData.Reset();
                }

                // 診療フィルターの復帰
                List<SinryouFilter> filters = OmotegakiSettings.Instance.GetFilterList(karteId);
                if (filters != null)
                {
                    AddFilters(filters);
                }

                _toothSelector.ClickClearButton();

                OnFilterChanged();

                return true;
            }
            finally
            {
                splitContainer1.Enabled = true;
                Cursor = Cursors.Default;
                _suspendUpdateCommonFilter = false;
                _suspendUpdateKarteData = false;
            }
        }

        private void OnFilterChanged()
        {
            ShinryouDataCollection shinryouData = _vm.ShinryouDataCollection;

            _toothSelector.SetData(shinryouData);
            _syochiRirekiList.SetData(shinryouData);

            if (shinryouData is null)
            {
                // 日付選択コントロールの更新
                _shinryouDateSelector.DataContext = null;

                return;
            }

            var pageData = (TabHaByoSyoPageData)_tabHaByoSyo.SelectedTab.Tag;

            // 表示回数 +1
            pageData.ShowCount++;

            shinryouData.OrderFunc = ((ShinryouOrderTypeSelector)_cmbSinryouOrder.Child).SelectedValue switch
            {
                ShinryouOrderType.歯順A => (p => new 歯種Order右左右左(p.歯式)),
                ShinryouOrderType.歯順B => (p => p.歯式),
                ShinryouOrderType.日付順 => null,
                _ => null,
            };
            if (_tabHaByoSyo.SelectedTab == _tabPageHaByoSyoFirst)
            {
                // 共通フィルターのみセットする。
                shinryouData.Filter = _commonFilter;

                tabInfo.SelectedTab = tabPage患者情報;
                tabPage患者情報.AutoScrollPosition = Point.Empty;
            }
            else
            {
                // 診療フィルターと共通フィルターをセットする。
                var filter = new SinryouFilter(pageData.Filter);
                filter.Combine(_commonFilter);
                shinryouData.Filter = filter;

                // フィルター情報リストを表示する。
                ShowFilterInfo();

                if (_filterInfoPage != null)
                {
                    // フィルター情報リストを今回設定されたフィルターまでスクロールする。
                    _filterInfoPage.ScrollToFilter(pageData.Filter);
                }
            }

            if (pageData.ShowCount == 1)
            {
                // 最下部までスクロール
                _syochiRirekiList.ScrollListToBottom();
            }
            else if (pageData.ScrollPercent.HasValue)
            {
                // 表示するタブの前回表示時のスクロール位置を復帰する。
                PointF scr = pageData.ScrollPercent.Value;
                _syochiRirekiList.ScrollListTo(scr.X, scr.Y);
            }

            // 日付選択コントロールの更新
            _shinryouDateSelector.DataContext = shinryouData
                                                    .Select(p => new ShinryouDateSelector.ItemData(p))
                                                    .Distinct();

            // チェック表示を更新
            _shinryouCheckDisplayVM.RefreshCheckDisplay();

            // カルテ番号入力欄にフォーカスをセット
            txtKarteBangou.Focus();
        }
        private string ShowDataFolderDialog(string defaultPath)
        {
            string iniPattern = global::OmoSeitokuEreceipt.Properties.Settings.DEFAULT_INITIAL_FILENAME_PATTERN;
            string defaultIniFile = global::OmoSeitokuEreceipt.Properties.Settings.DEFAULT_INITIAL_FILENAME;
            using OpenFileDialog dlg = new OpenFileDialog
            {
                Title = $"初期設定ファイル ({iniPattern})を選択してください。",
                FileName = (!string.IsNullOrEmpty(defaultPath)) &&
                           File.Exists(Path.Combine(defaultPath, defaultIniFile))
                               ? defaultIniFile
                               : string.Empty,
                Filter = $"初期設定ファイル ({iniPattern})|{iniPattern}|All Files (*.*)|*.*",
                FilterIndex = 1,
                CheckFileExists = true,
                InitialDirectory = defaultPath,
            };

            return (dlg.ShowDialog(this) == DialogResult.OK)
                    ? Path.GetDirectoryName(dlg.FileName)
                    : null;
        }
        private void ShowMessage(string message, bool isError, bool addToLog)
        {
            if (isError)
            {
                _toast.Show(message, TimeSpan.FromSeconds(5), Color.IndianRed);

                message = "**エラー** " + message;
            }
            else
            {
                _toast.Show(message, TimeSpan.FromSeconds(3));
            }

            if (addToLog)
            {
                string shortMsg = (message.Length <= 30 ? message : message.Substring(0, 30) + " ...");

                ToolStripItemCollection items = メッセージログToolStripMenuItem.DropDown.Items;
                items.Insert(0, new ToolStripMenuItem()
                {
                    Text = $"{items.Count + 1} [{DateTime.Now.ToShortTimeString()}] {shortMsg}",
                    ToolTipText = message,
                });

                if (!メッセージログToolStripMenuItem.Enabled)
                {
                    メッセージログToolStripMenuItem.Enabled = true;
                }
            }
        }
        /// <summary>
        /// 現在読み込み済みのカルテデータの再読み込み
        /// </summary>
        /// <param name="uncheckSyosinList">初診リストのチェック状態を復帰しない</param>
        private void UpdateKarteData(bool uncheckSyosinList)
        {
            if (_suspendUpdateKarteData) return;

            if (!(_vm.CurrentKarteId is null) && !_vm.IsKarteLoading)
            {
                _updateKarteDataThrottle.Signal(this, _vm.CurrentKarteId.KarteNumber, uncheckSyosinList);
            }
        }
        private readonly OmoSeitoku.Threadings.Throttle<OmotegakiForm, int, bool> _updateKarteDataThrottle =
            new OmoSeitoku.Threadings.Throttle<OmotegakiForm, int, bool>(
                _カルテデータ取得の実行待機間隔,
                (OmotegakiForm form, int currentKarteNumber, bool uncheckSyosinList) =>
                    form.LoadKarteData(currentKarteNumber, true, uncheckSyosinList));

        private void UpdateShinryoujoSelector()
        {
            if (_initialSettings_ShinryoujoTeisuu.HasValue)
            {
                cmbSinryoujo.SelectedIndex = _initialSettings_ShinryoujoTeisuu.Value - 1;
                cmbSinryoujo.Enabled = false;
            }
            else
            {
                cmbSinryoujo.Enabled = true;
            }
        }
        protected override void WndProc(ref Message m)
        {
            if ((_windowLink != null) && _windowLink.WndProc(ref m))
            {
                return;
            }

            base.WndProc(ref m);
        }

        #region 診療フィルター

        /// <summary>
        /// 新規フィルターエディターを表示
        /// </summary>
        private bool ShowNewShinryouFilterEditor(out SinryouFilter filter)
        {
            using SinryouFilterDialog dlg = new SinryouFilterDialog("フィルター" + _filterHistoryLength);

            if ((dlg.ShowDialog(this) != DialogResult.OK) ||
                (dlg.Filter is null))
            {
                filter = null;
                return false;
            }
            else
            {
                filter = dlg.Filter;
                return true;
            }
        }
        private void AddFilter(SinryouFilter filter)
        {
            string filterKey = filter.GetKey();

            _tabHaByoSyo.SelectedTab = _tabHaByoSyo.TabPages.ContainsKey(filterKey)
                // 既にある場合、そのタブを表示
                ? _tabHaByoSyo.TabPages[filterKey]
                // 新しいタブページを作成して表示
                : AddFilter(filter, filterKey);
        }
        private void AddFilters(IEnumerable<SinryouFilter> collection)
        {
            foreach (var filter in collection)
            {
                string filterKey = filter.GetKey();

                if (!_tabHaByoSyo.TabPages.ContainsKey(filterKey))
                {
                    AddFilter(filter, filterKey);
                }
            }
        }
        private TabPage AddFilter(SinryouFilter filter, string filterKey)
        {
            //新しいタブページを作成
            _tabHaByoSyo.TabPages.Add(filterKey, filter.Title);

            TabPage page = _tabHaByoSyo.TabPages[filterKey];

            page.Tag = new TabHaByoSyoPageData(filter);

            ++_filterHistoryLength;

            // 新しいタブページにファーストタブページのプロパティをコピー
            PropertyInfo[] properties = page.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.CanWrite && !property.IsSpecialName)
                {
                    Type propType = property.PropertyType;
                    if (propType.IsPublic && propType.IsValueType)
                    {
                        property.SetValue(page, property.GetValue(_tabPageHaByoSyoFirst, null), null);
                    }
                }
            }

            return page;
        }
        private void ClearFilter()
        {
            // Save CommonFilter
            {
                var filters = new List<SinryouFilter>
                {
                    new SinryouFilter(_commonFilter) { 歯式 = null },
                };
                OmotegakiSettings.Instance.SetFilterList(
                    new KarteId(CommonFilterShinryoujo, COMMON_FILTER_KARTE_NO),
                    filters);
            }

            // Save Filters
            if (!(_vm.CurrentKarteId is null))
            {
                var list = new List<SinryouFilter>();
                foreach (TabPage page in _tabHaByoSyo.TabPages)
                {
                    if (page.Tag is TabHaByoSyoPageData data && data.Filter != null)
                    {
                        list.Add(data.Filter);
                    }
                }

                OmotegakiSettings.Instance.SetFilterList(
                    _vm.CurrentKarteId,
                    (0 < list.Count) ? list : null);
            }

            // フィルタのクリア (0番目は消さない)
            for (int i = _tabHaByoSyo.TabPages.Count - 1; 0 < i; --i)
            {
                _tabHaByoSyo.TabPages.RemoveAt(i);
            }
        }
        private void EditFilter(SinryouFilter filter)
        {
            if (_tabHaByoSyo.TabPages.ContainsKey(filter.GetKey()))
            {
                TabPage t = _tabHaByoSyo.TabPages[filter.GetKey()];

                if (t != _tabPageHaByoSyoFirst)
                {
                    // 新しいフィルター
                    SinryouFilter newFilter = ((TabHaByoSyoPageData)t.Tag).Filter;
                    // 作成
                    using (var dlg = new SinryouFilterDialog(newFilter))
                    {
                        if (dlg.ShowDialog(this) == DialogResult.OK)
                            newFilter = dlg.Filter;
                        else
                            newFilter = null;
                    }

                    if (newFilter != null)
                    {
                        t.Name = newFilter.GetKey();
                        t.Text = newFilter.Title;
                        ((TabHaByoSyoPageData)t.Tag).Filter = newFilter;
                        OnFilterChanged();
                    }
                }
            }
        }
        private void RemoveFilter(SinryouFilter filter)
        {
            if (_tabHaByoSyo.TabPages.ContainsKey(filter.GetKey()))
            {
                TabPage t = _tabHaByoSyo.TabPages[filter.GetKey()];

                if (t != _tabPageHaByoSyoFirst)
                {
                    _tabHaByoSyo.TabPages.Remove(t);
                }

                OnFilterChanged();
            }
        }
        /// <summary>
        /// フィルター情報を作成
        /// </summary>
        private void ShowFilterInfo()
        {
            var pages = _tabHaByoSyo.TabPages;
            var filters = new SinryouFilter[pages.Count];
            for (int i = filters.Length - 1; 0 < i; --i)
            {
                if (pages[i].Tag != null)
                {
                    filters[i] = ((TabHaByoSyoPageData)pages[i].Tag).Filter;
                }
            }

            if (_filterInfoPage is null)
            {
                _filterInfoPage = new SinryouFilterTabPage("フィルター情報");
                _filterInfoPage.DeleteButtonClick += delegate (object sender, SinryouFilterTabPage.ItemEventArgs ev)
                {
                    RemoveFilter(ev.Filter);
                };
                _filterInfoPage.EditButtonClick += delegate (object sender, SinryouFilterTabPage.ItemEventArgs ev)
                {
                    EditFilter(ev.Filter);
                };
                tabInfo.TabPages.Add(_filterInfoPage);
            }

            _filterInfoPage.SetFilters(filters);


            tabInfo.SelectedTab = _filterInfoPage;


            // カルテ番号入力欄にフォーカスをセット
            txtKarteBangou.Focus();
        }
        private void UpdateCommonFilterControl()
        {
            if (_commonFilter.Has歯式)
            {
                _toothSelector.CheckedHaButton歯種 =
                    from ha in _commonFilter.歯式
                    select ha.歯種;
            }
            else
            {
                _toothSelector.CheckedHaButton歯種 = null;
            }
            _chkSisyuKanzen.Checked = _commonFilter.歯種完全一致;
            _chkPJogai.Checked = _commonFilter.P病名除外;
            _chkGJogai.Checked = _commonFilter.G病名除外;
            _chkGishiJogai.Checked = _commonFilter.義歯除外;
            _cmbJogaiType.SelectedIndex = _commonFilter.除外種別;
            ((ShinryouOrderTypeSelector)_cmbSinryouOrder.Child).SelectedValue = _commonFilter.並び順Type;
        }

        private void UpdateCommonFilter()
        {
            if (_suspendUpdateCommonFilter) return;

            _updateCommonFilterThrottle.Signal(this);
        }
        private readonly OmoSeitoku.Threadings.Throttle<OmotegakiForm> _updateCommonFilterThrottle =
            new OmoSeitoku.Threadings.Throttle<OmotegakiForm>(
                TimeSpan.FromMilliseconds(1000),
                form => form.InternalUpdateCommonFilter());

        private void InternalUpdateCommonFilter()
        {
            { // 歯式
                var list = _toothSelector.CheckedHaButton歯種.ToArray();
                if (0 < list.Length)
                {
                    var haArray = new ER歯式単位[list.Length];
                    int i = 0;
                    foreach (string hasyu in list)
                    {
                        haArray[i++] = new ER歯式単位(hasyu);
                    }
                    _commonFilter.歯式 = new ER歯式(haArray);
                }
                else
                {
                    _commonFilter.歯式 = null;
                }
            }
            _commonFilter.歯種完全一致 = _chkSisyuKanzen.Checked;
            _commonFilter.P病名除外 = _chkPJogai.Checked;
            _commonFilter.G病名除外 = _chkGJogai.Checked;
            _commonFilter.義歯除外 = _chkGishiJogai.Checked;
            _commonFilter.除外種別 = _cmbJogaiType.SelectedIndex;
            _commonFilter.並び順Type = ((ShinryouOrderTypeSelector)_cmbSinryouOrder.Child).SelectedValue;

            OnFilterChanged();
        }

        private void 設定ファイルからコントロールへ反映()
        {
            // ビューワー設定
            _cmbViewerSettings.Items.Clear();
            if (ClientShareSettings.Instance.ShinryouViewerSettings != null)
            {
                _cmbViewerSettings.Items.AddRange(ClientShareSettings.Instance.ShinryouViewerSettings.ToArray());
            }

            // コントロールの初期化
            new Thread((ThreadStart)delegate
            {
                BeginInvoke((Action)delegate
                {
                    _cmbViewerSettings.SelectedItem = null;
                });
            }).Start();
        }

        #endregion

        #region 印刷

        /// <summary>
        /// 印刷前にデータを確認し、正常に印刷できるかどうか取得する。
        /// </summary>
        /// <returns>正常に印刷できる場合 true</returns>
        private bool CheckBeforePrinting()
        {
            if (_vm.ShoshinKikanItems.Count(p => p.IsChecked) == 0)
            {
                var sb = new StringBuilder(1024);
                sb.AppendLine("対象となる診療期間が選択されていません。");
                sb.AppendLine("処理を続行しますか？");

                return MessageBox.Show(sb.ToString(), "処理続行の確認", MessageBoxButtons.YesNo) == DialogResult.Yes;
            }
            return true;
        }
        /// <summary>
        /// カルテ印刷設定ダイアログを表示する。
        /// </summary>
        /// <param name="defaultSettings">新規設定追加時のデフォルト設定</param>
        /// <returns>リストから選択された設定。キャンセル時は null。</returns>
        private KartePrintDesign.Settings ShowKartePrintSettingListDialog(KartePrintDesign.Settings defaultSettings)
        {
            using KartePrintSettingListDialog dlg = new KartePrintSettingListDialog();

            if (defaultSettings != null)
            {
                dlg.Settings = defaultSettings;
            }

            return (dlg.ShowDialog(this) == DialogResult.OK)
                        ? dlg.Settings
                        : null;
        }
        // カルテの印刷
        private void PrintKarte(bool isPreview)
        {
            if (!CheckBeforePrinting())
            {
                return;
            }

            var settings = new KartePrintDesign.Settings();

            settings = ShowKartePrintSettingListDialog(settings);

            if (settings is null)
            {
                return;
            }

            // 患者負担率
            if (!KarteRepository.GetKanjaHutanritu(out double hutanRitu, out string hutanErrMsg, _vm.CurrentKarteData, true))
            {
                ShowMessage(hutanErrMsg, true, true);
                return;
            }

            var printItems = new List<KartePrintItem>();
            SinryouData[] sinryouItems = _vm.ShinryouDataCollection.GetRange(GetKikan().Select(p => p.ToDateRange()));


            var printItem = new KartePrintItem(_vm.CurrentKarteId, _vm.CurrentKarteData.Get氏名(false), sinryouItems, hutanRitu);

            printItems.Add(printItem);


            if (isPreview)
            {
                _kartePrint.Preview(printItems, settings, this);

                var res = MessageBox.Show("このまま印刷を開始しますか？", "印刷",
                                          MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                          MessageBoxDefaultButton.Button2);
                if (res != DialogResult.Yes)
                {
                    return;
                }
            }

            _kartePrint.Print(printItems, settings, this);
        }
        // カルテの連続印刷
        private void PrintKarteBatch()
        {
            KartePrintDesign.Settings kartePrintSettings = ShowKartePrintSettingListDialog(null);

            if (kartePrintSettings is null)
            {
                return;
            }

            var batchSettings = new KarteBatchSettings();

            // カルテ印刷設定
            DateTime dt = DateTime.Now;
            batchSettings.DateRange = new KarteDateRange(
                    range: new DateRange2(
                        new DateTime(dt.Year, 1, 1).AddMonths(dt.Month - 2), // 前月の1日
                        new DateTime(dt.Year, 1, 1).AddMonths(dt.Month - 1).AddDays(-1) // 前月の末日
                    ),
                    expandToSyosinbi: true,
                    expandToLastDate: true
                );
            batchSettings.PreviewLimit = 5;


            bool isPreview;

            // カルテ一括印刷ダイアログを表示
            using (var dlg = new KarteBatchPrintDialog())
            {
                dlg.Settings = batchSettings;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    batchSettings = dlg.Settings;
                    isPreview = dlg.IsPreview;
                }
                else
                {
                    // キャンセル
                    return;
                }
            }


            // 診療所
            Shinryoujo shinryoujo = new Shinryoujo(cmbSinryoujo.SelectedValue.ToString());

            // 対象期間
            DateRange dateRange = batchSettings.DateRange.Range.ToDateRange();


            // 印刷数
            int limit = (isPreview
                            ? batchSettings.PreviewLimit
                            : 0); // 0=Max


            // 各カルテの最終記録日が日付順に入っているので、
            // 指定期間内の日付から、さらにカルテの各診療データの日付を見て
            // 指定期間内のものがあればそれを印刷する。
            var printInfo = new KartePrintInfo(shinryoujo);
            //string kartePath = Path.Combine(global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder, "RgsKrDy."+sinryoujo);// UNDONE カルテ印刷用データ ハードコード
            //using (var fs = File.Open(kartePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            //using (var br = new BinaryReader(fs))
            //{
            //    printInfo.Read(br, dateRange);
            //}

            string karteCheckListFile = Path.Combine(
                                            global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder,
                                            "CHGTSDKR.900");// UNDONE カルテチェック印刷用データ ハードコード
            using (var fs = File.Open(karteCheckListFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            //using (var br = new BinaryReader(fs))
            using (var sr = new StreamReader(fs))
            {
                printInfo.Read(sr, dateRange.Max);
            }

            // UNDONE 診療録データ ハードコード
            string randaPath = Path.Combine(global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder, $"RANDA{shinryoujo.Key}.900");

            bool rePrint = false;
            bool expandToSyosinbi = batchSettings.DateRange.ExpandToSyosinbi;
            bool expandToLastDate = batchSettings.DateRange.ExpandToLastDate;

            do
            {
                rePrint = false;

                var items = new KartePrintItemList();

                using (FileStream karteFS = KarteRepository.OpenKarteDataFile(shinryoujo))
                using (var karteBR = new BinaryReader(karteFS))
                using (FileStream fs = File.Open(randaPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var br = new BinaryReader(fs, Encoding.GetEncoding("Shift_JIS")))
                {
                    var sinryouTougou = (SinryouDataLoader.診療統合種別)_cmbShinryouTougou.SelectedItem;

                    foreach (KartePrintInfoItem item in printInfo.Items
                        .Where(p => !(p.KarteId is null)))
                    {
                        var loader = new SinryouDataLoader(item.KarteId, fs, br);

                        DateRange range = loader.ConvertSinryouDateRange(dateRange, expandToSyosinbi, expandToLastDate);
                        ShinryouDataCollection sinryouData = loader.GetShinryouData(range, sinryouTougou);

                        // リストが期間内に含まれるかどうか
                        if (0 < sinryouData.Count
                            //&& sinryouData[0].開始 <= dateRange.Max && // 先頭が最大日以下
                            //dateRange.Min <= sinryouData[sinryouData.Count - 1].開始 // 終端が最小日以上
                            )
                        {
                            // カルテデータ読み込み
                            long pos = (item.KarteId.KarteNumber - 1) * KanjaData.SIZE;
                            if (0 > pos || pos >= fs.Length)
                            {
                                throw new Exception("患者番号が範囲外です。");
                            }

                            fs.Seek(pos, SeekOrigin.Begin);

                            KarteData karteData = new KarteData(karteBR);

                            // 患者負担率
                            if (!KarteRepository.GetKanjaHutanritu(out double hutanRitu, out string hutanErrMsg, karteData, false))
                            {
                                ShowMessage(hutanErrMsg, true, true);

                                MessageBox.Show("患者負担率が異常です。カルテ番号 = " + item.KarteId.KarteNumber);

                                // キャンセル
                                return;
                            }

                            // 印刷リストに追加
                            items.Add(
                                new KartePrintItem(
                                        karteId: item.KarteId,
                                        karteName: karteData.Get氏名(kana: false),
                                        syochiList: sinryouData.ToArray(),
                                        hutanWariai: hutanRitu
                                        ));
                        }

                        if (0 < limit && limit <= items.Count) break;
                    }
                }

                // 印刷開始
                if (items.Count == 0)
                {
                    MessageBox.Show("印刷できるカルテがありません。");
                }
                else
                {
                    items.Sort(batchSettings.SortSettings);

                    if (isPreview)
                    {
                        _kartePrint.Preview(items, kartePrintSettings, this);


                        var res = MessageBox.Show("このまま全件印刷を開始しますか？", "全件印刷",
                                                  MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                                  MessageBoxDefaultButton.Button2);
                        if (res == DialogResult.Yes)
                        {
                            rePrint = true;
                            limit = 0;
                            isPreview = false;
                        }
                    }
                    else
                    {
                        _kartePrint.Print(items, kartePrintSettings, this);
                    }
                }

            } while (rePrint);
        }

        #endregion

        private List<DateRange2> GetKikanByCalendar()
        {
            var kikan = DateRange2.Infinite;

            if (chkKikanStart.Checked)
            {
                kikan.Min = _dateKikanStart.Value;
            }

            if (chkKikanEnd.Checked)
            {
                kikan.Max = _dateKikanEnd.Value;
            }

            return new List<DateRange2> { kikan };
        }

        #region Event Handler

        #region Form

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // 起動時の設定読込
            {
                OmotegakiSettings.Instance.ShishikiTougou = false;

                // _commonFilter
                {
                    List<SinryouFilter> list = OmotegakiSettings.Instance.GetFilterList(
                                                    new KarteId(CommonFilterShinryoujo, COMMON_FILTER_KARTE_NO));
                    _commonFilter = ((list is null) || (list.Count == 0))
                                        ? new SinryouFilter("共通フィルター")
                                        : list.First();
                    // フィルター
                    // 前回設定を反映させずに初期値に戻す項目のみここで初期化
                    _commonFilter.歯式 = null;
                    _commonFilter.歯種完全一致 = false;
                    _commonFilter.P病名除外 = true;
                    _commonFilter.G病名除外 = true;
                    _commonFilter.義歯除外 = true;
                    _commonFilter.除外種別 = SinryouFilter.除外種別_除外;
                    _commonFilter.並び順Type = ShinryouOrderType.歯順A;
                    UpdateCommonFilterControl();
                }
            }

            _toast = new Toast();
            _tabHaByoSyo = new TabControl();
            _tabPageHaByoSyoFirst = new TabPage("診療情報");
            _tabPageHaByoSyoPrev = _tabPageHaByoSyoFirst;

            _syochiRirekiList = new SyochiRirekiListWpf();

            try
            {
                SuspendLayout();
                SetupControls();
            }
            finally
            {
                ResumeLayout(false);
            }

            // データフォルダー設定
            {
                string dataFolder = global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder;
                string initialFile;

                // データフォルダーの存在確認と、無い場合の選択ダイアログ表示
                #region
                {
                    string errorMessage = null;

                    if (string.IsNullOrEmpty(dataFolder) || !Directory.Exists(dataFolder))
                    {
                        // 最新のデータフォルダーが無い為、フォルダー選択ダイアログを表示する。
                        dataFolder = ShowDataFolderDialog(string.Empty);

                        // キャンセルされた、またはエラー
                        if (string.IsNullOrEmpty(dataFolder) || !Directory.Exists(dataFolder))
                        {
                            errorMessage = "データフォルダーが存在しない為、アプリケーションを終了します。";
                        }

                        if (errorMessage is null)
                        {
                            dataFolder = global::OmoSeitokuEreceipt.Properties.Settings.NormalizeDataFolderPath(dataFolder);

                            try
                            {
                                global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder = dataFolder;

                                if (global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder != dataFolder)
                                {
                                    errorMessage = "アプリケーションを終了します。";
                                }
                            }
                            catch (Exception ex)
                            {
                                errorMessage = new StringBuilder()
                                                .AppendLine("データフォルダーを設定できない為、アプリケーションを終了します。")
                                                .AppendLine("-------------------")
                                                .AppendLine(ex.Message)
                                                .ToString();
                            }
                        }
                    }

                    string[] initFiles = Directory.GetFiles(
                        dataFolder,
                        global::OmoSeitokuEreceipt.Properties.Settings.DEFAULT_INITIAL_FILENAME_PATTERN,
                        SearchOption.TopDirectoryOnly
                        ).ToArray();
                    if (1 == initFiles.Length)
                    {
                        initialFile = initFiles[0];
                    }
                    else if (1 < initFiles.Length)
                    {
                        #region 初期設定ファイル選択ダイアログの表示
                        using Form form = new Form
                        {
                            StartPosition = FormStartPosition.CenterParent,
                            Size = new Size(640, 240),
                            Text = "ファイル選択"
                        };
                        form.Font = new Font(form.Font.FontFamily, 16f);

                        var lbl = new Label()
                        {
                            AutoSize = true,
                            Dock = DockStyle.Top,
                            Padding = new Padding(8, 8, 8, 16),
                            Text = "初期設定ファイルが複数存在します。"
                        };
                        var footer = new Label()
                        {
                            AutoSize = true,
                            Dock = DockStyle.Bottom,
                            Padding = new Padding(8, 16, 8, 8),
                            Text = new StringBuilder()
                                    .AppendLine("ファイル名をダブルクリック、")
                                    .Append("または選択してENTERキーを押してください。")
                                    .ToString()
                        };
                        var listBox = new ListBox()
                        {
                            DataSource = initFiles.OrderBy(f => f.Length).ToArray(),
                            Dock = DockStyle.Fill,
                            SelectionMode = SelectionMode.One,
                            Name = "listBox"
                        };
                        listBox.DoubleClick += (sndr, ev) =>
                        {
                            var frm = ((Control)sndr).FindForm();
                            frm.DialogResult = DialogResult.OK;
                            frm.Close();
                        };
                        listBox.KeyDown += (sndr, ev) =>
                        {
                            if (ev.KeyCode != Keys.Enter) return;
                            var frm = ((Control)sndr).FindForm();
                            frm.DialogResult = DialogResult.OK;
                            frm.Close();
                        };
                        form.Controls.AddRange(new Control[] { lbl, footer, listBox });
                        listBox.BringToFront();
                        form.Shown += (sndr, ev) =>
                        {
                            // 最も短い名前のファイルを初期選択
                            var lb = ((Form)sndr).Controls.Find("listBox", false)[0] as ListBox;
                            var minItem = new String('*', 1024);
                            foreach (object item in lb.Items)
                            {
                                if (((string)item).Length < minItem.Length)
                                {
                                    minItem = (string)item;
                                }
                            }
                            lb.SelectedItem = minItem;
                        };

                        if ((form.ShowDialog(this) == DialogResult.OK)
                            && (listBox.SelectedItems.Count == 1))
                        {
                            initialFile = (string)listBox.SelectedItem;
                        }
                        else
                        {
                            initialFile = null;
                        }
                        #endregion
                    }
                    else
                    {
                        initialFile = null;
                    }
                    if (string.IsNullOrEmpty(initialFile) || !File.Exists(initialFile))
                    {
                        errorMessage = "データフォルダーに初期設定ファイルが存在しない為、アプリケーションを終了します。";
                    }

                    // エラーがある場合はメッセージを表示してアプリを終了。
                    if (errorMessage != null)
                    {
                        MessageBox.Show(errorMessage);
                        _saveSettingsOnClose = false;
                        Close();
                        return;
                    }
                }
                #endregion

                // データフォルダー変更をコントロールの表示に反映。
                _vm.UpdateTitle();
                データフォルダーToolStripMenuItem.Text = dataFolder;

                // 最近使ったデータフォルダーリストへの追加と設定の保存
                #region
                {
                    ToolStripItemCollection toolStripItems = 最近使ったデータフォルダーToolStripMenuItem.DropDown.Items;

                    // 最近使ったデータフォルダーに追加
                    for (int i = 0; i < toolStripItems.Count; ++i)
                    {
                        // リストに既に含まれていたら一旦削除
                        if (toolStripItems[i].Text.Equals(dataFolder, StringComparison.OrdinalIgnoreCase))
                        {
                            toolStripItems.RemoveAt(i);
                            break;
                        }
                    }
                    // リストに追加
                    toolStripItems.Insert(0, new ToolStripMenuItem(dataFolder));
                    // 最大保存数以上のデータを削除
                    for (int i = toolStripItems.Count - 1; 最近使ったデータフォルダーの保存数 <= i; --i)
                    {
                        toolStripItems.RemoveAt(i);
                    }

                    // 現在使用中のデータフォルダーにマークをつける
                    int size = toolStripItems[0].Height;
                    var bmp = new Bitmap(size, size);
                    using (var font = new Font(FontFamily.GenericMonospace, 13))
                    using (var g = Graphics.FromImage(bmp))
                    {
                        g.DrawString("*", font, Brushes.Black, 0, 0);
                    }
                    toolStripItems[0].Image = bmp;


                    // 設定を作成 (最近使ったデータフォルダー)
                    OmotegakiSettings.Instance.DataFolderHistory = toolStripItems.Cast<ToolStripItem>()
                                                    .Select(toolStripItem => toolStripItem.Text)
                                                    .Distinct()
                                                    .ToArray();

                    // 設定をセーブする。
                    OmotegakiSettings.Instance.Save();
                }
                #endregion

                // 診療所選択コンボボックスへの反映 （初期設定ファイルから自動設定）
                #region
                {
                    var db = new OmoSeitokuEreceipt.SeitokuDB(initialFile);
                    int rowIndex = db.Find(db.GetColumn("項目名"), "1", 0);
                    string shinryoujyoTeisuuText = db.GetValue(rowIndex, "定数");

                    if (int.TryParse(shinryoujyoTeisuuText, out int shinryoujyoTeisuu)
                        && (0 < shinryoujyoTeisuu))
                    {
                        _initialSettings_ShinryoujoTeisuu = shinryoujyoTeisuu;
                    }
                    UpdateShinryoujoSelector();
                }
                #endregion
            }



            //// 背景色
            //picMenuDark
            //pictureBox1.BackColor = picPanelTopLight.BackColor =
            //    picPanelRightLight.BackColor = pnlHaByoSyo.BackColor;



            // 
            _tabHaByoSyo.SelectedTab = _tabPageHaByoSyoFirst;


            tabPage患者情報.AutoScrollPosition = Point.Empty;

            if (OmotegakiSettings.Instance.WindowState == FormWindowState.Normal)
            {
                // 非同期でウィンドウ位置・サイズ変更
                _ = Task.Run(() =>
                {
                    Invoke((Action)delegate
                    {
                        WindowState = FormWindowState.Normal;
                        DesktopLocation = OmotegakiSettings.Instance.DesktopLocation;
                        if (!OmotegakiSettings.Instance.WindowSize.IsEmpty)
                        {
                            Size = OmotegakiSettings.Instance.WindowSize;
                        }
                    });
                });
            }
            else
            {
                WindowState = OmotegakiSettings.Instance.WindowState;
            }

            // スクリーンサイズより大きい場合最大化
            if (WindowState == FormWindowState.Normal)
            {
                Screen scr = Screen.FromControl(this);
                if (scr.Bounds.Width < Width || scr.Bounds.Height < Height)
                {
                    WindowState = FormWindowState.Maximized;
                }
            }

            設定ファイルからコントロールへ反映();

            _saveSettingsOnClose = true;

            if (_windowLink != null)
            {
                _windowLink.WaitLink();
            }

            // カルテ番号入力欄にフォーカスをセット
            txtKarteBangou.BeginInvoke((Action)delegate
            {
                DateTime start = DateTime.Now;
                while (!txtKarteBangou.Focus())
                {
                    if (1 <= (DateTime.Now - start).Seconds) break;
                    Thread.Sleep(200);
                }
            });
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            // ウィンドウリンクの解除
            WindowLink wlink = _windowLink;
            if (!e.Cancel && (wlink != null) && wlink.IsLinked)
            {
                if (MessageBox.Show("リンクを切断します。", "リンクの切断確認", MessageBoxButtons.OKCancel)
                    == DialogResult.OK)
                {
                    wlink.UnLink();
                }
                else
                {
                    e.Cancel = true;
                }
            }

            if (!e.Cancel)
            {
                // 設定の保存
                if (_saveSettingsOnClose)
                {
                    // アプリの設定
                    if (OmotegakiSettings.Instance != null)
                    {
                        OmotegakiSettings.IgnoreSettingsChangeEvent = true;
                        try
                        {
                            ClearFilter();

                            OmotegakiSettings.Instance.ShishikiTougou = ((SinryouDataLoader.診療統合種別)_cmbShinryouTougou.SelectedItem != SinryouDataLoader.診療統合種別.統合なし);

                            if (OmotegakiSettings.Instance.RestoreWindowState)
                            {
                                OmotegakiSettings.Instance.WindowState = WindowState;
                                OmotegakiSettings.Instance.WindowSize = Size;
                                OmotegakiSettings.Instance.DesktopLocation = DesktopLocation;
                            }
                        }
                        finally
                        {
                            OmotegakiSettings.IgnoreSettingsChangeEvent = false;
                        }

                        OmotegakiSettings.Instance.Save();
                    }

                    // データフォルダ共通設定
                    if (ClientShareSettings.Instance != null)
                    {
                        ClientShareSettings.Instance.Save();
                    }
                }

                global::OmoSeitokuEreceipt.Properties.Settings.Default.Save();
            }


            base.OnClosing(e);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ControlKey:
                    _toothSelector.IsMultiSelect = true;
                    break;

                case Keys.Home:
                    _syochiRirekiList.ScrollListToTop();
                    e.Handled = true;
                    break;

                case Keys.End:
                    _syochiRirekiList.ScrollListToBottom();
                    e.Handled = true;
                    break;

                case Keys.F1:
                    txtKarteBangou.Focus();
                    txtKarteBangou.SelectAll();
                    e.Handled = true;
                    break;

                case Keys.Tab: // (Ctrl + Tab)
                    if (e.Control)
                    {
                        int ct = _tabHaByoSyo.TabPages.Count;
                        if (0 < ct)
                        {
                            int i = (_tabHaByoSyo.SelectedIndex + (e.Shift ? -1 : 1)) % ct;
                            _tabHaByoSyo.SelectedIndex = (i < 0 ? ct + i : i);
                        }
                        e.Handled = true;
                    }
                    break;
            }

            base.OnKeyDown(e);
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                _toothSelector.IsMultiSelect = false;
            }

            base.OnKeyUp(e);
        }
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);


            WindowLink wlink = _windowLink;
            if ((wlink != null) && wlink.IsLinked)
            {
                // リンク相手にタイトル変更メッセージを送信
                wlink.SendTitleChanged();
            }
        }

        #endregion
        #region _shinryouDateSelector
        void _shinryouDateSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = _shinryouDateSelector.SelectedItem;
            if (selected != null)
            {
                // 選択した診療へスクロール
                _syochiRirekiList.ScrollListTo(selected.SinryouData.診療日);
                _syochiRirekiList.Focus();
            }
        }
        #endregion
        #region カルテ読込ボックス

        private void txtKarteBangou_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLoadKarte.PerformClick();
            }
        }
        private void txtKarteBangou_TextChanged(object sender, EventArgs e)
        {
            string t = txtKarteBangou.Text;

            if (0 < t.Length)
            {
                MatchCollection mc = _txtKarteBangouFilter.Matches(t);
                if (0 < mc.Count)
                {
                    int selStart = txtKarteBangou.SelectionStart;

                    StringBuilder sb = new StringBuilder();
                    int start = 0;
                    foreach (Match m in mc)
                    {
                        sb.Append(t.Substring(start, m.Index - start));
                        start = m.Index + m.Length;
                    }
                    txtKarteBangou.Text = sb.Append(t.Substring(start)).ToString();

                    if (0 < selStart) txtKarteBangou.SelectionStart = selStart - 1;
                }

                if (!btnLoadKarte.Enabled)
                    btnLoadKarte.Enabled = true;
            }
            else
            {
                if (btnLoadKarte.Enabled)
                    btnLoadKarte.Enabled = false;
            }
        }
        private void txtKarteBangou_GotFocus(object sender, EventArgs e)
        {
            txtKarteBangou.BackColor = Color.Yellow;
        }
        private void txtKarteBangou_LostFocus(object sender, EventArgs e)
        {
            txtKarteBangou.BackColor = SystemColors.Window;
        }
        private void chkKikanStart_CheckedChanged(object sender, EventArgs e)
        {
            _dateKikanStart.Visible = chkKikanStart.Checked;
        }
        private void chkKikanEnd_CheckedChanged(object sender, EventArgs e)
        {
            bool flg = chkKikanEnd.Checked;
            _dateKikanEnd.Visible = flg;
            _btnKikanDay.Enabled = flg;
            _btnKikanMonth.Enabled = flg;
            _btnKikanYear.Enabled = flg;
        }
        private void btnLoadKarte_Click(object sender, EventArgs e)
        {
            if (_vm.IsKarteLoading) return;

            if (btnLoadKarte.Enabled && 0 < txtKarteBangou.TextLength)
            {
                if (int.TryParse(txtKarteBangou.Text, out int karteNumber))
                {
                    LoadKarteData(karteNumber, false);
                }
            }

            txtKarteBangou.SelectAll();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (_vm.IsKarteLoading) return;

            Clear();
            _vm.Clear();

            txtKarteBangou.Focus();
        }
        private void btnCalendarTekiyou_Click(object sender, EventArgs e)
        {
            UpdateKarteData(true);
        }

        #endregion
        #region メニュー

        #region ファイル

        private void データフォルダーの変更ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder;

            path = ShowDataFolderDialog(path);

            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder = path;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void 最近使ったデータフォルダーToolStripMenuItem_DropDownItemClicked(
                        object sender, ToolStripItemClickedEventArgs e)
        {
            // イベント中でデータフォルダを変更すると
            // ToolStripMenuItem にアクセスできない例外が出るため、非同期で実行する。
            //
            // 同期で実行する必要がある場合は、事前にドロップダウンを強制的に閉じる方法でも良い。
            Action<string> act = (path =>
            {
                try
                {
                    global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder = path;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            BeginInvoke(act, e.ClickedItem.Text);
        }
        private void 閉じるToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region 診療情報

        private void 新規フィルターの作成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ShowNewShinryouFilterEditor(out SinryouFilter filter))
            {
                AddFilter(filter);
            }
        }
        private void 全てのフィルターを削除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("全てのフィルターを削除します。", "フィルターのクリア",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                ClearFilter();
            }
        }
        private void 診療情報_印刷ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_vm.CurrentKarteId is null) return;

            SinryouData[] data = _vm.ShinryouDataCollection.GetFiltered().ToArray();

            using ShinryouPrintDesign design = new ShinryouPrintDesign(_vm.CurrentKarteId, _vm.CurrentKarteData, data);
            using PrintDialog printDialog = new PrintDialog { Document = design.PrintDocument };

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    design.PrintDocument.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void 診療情報_印刷プレビューToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_vm.CurrentKarteId is null) return;

            SinryouData[] data = _vm.ShinryouDataCollection.GetFiltered().ToArray();

            using var design = new ShinryouPrintDesign(_vm.CurrentKarteId, _vm.CurrentKarteData, data);
            using var previewDialog = new PrintPreviewDialog
            {
                Document = design.PrintDocument,
                WindowState = FormWindowState.Maximized
            };

            previewDialog.ShowDialog();
        }
        private void 歯別診療情報_印刷ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_vm.CurrentKarteId is null) return;

            SinryouData[] data = _vm.ShinryouDataCollection.GetFiltered().ToArray();
            var parameter = new HaRirekiPrintingParameter(_vm.CurrentKarteId, _vm.CurrentKarteData, data);

            using var design = new HaRirekiPrintDesign(parameter);
            using var previewDialog = new PrintPreviewDialog
            {
                Document = design.PrintDocument,
                WindowState = FormWindowState.Maximized
            };

            previewDialog.ShowDialog();
        }
        private void 歯別診療情報_複数カルテ印刷ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //@@ss 不要な歯も印刷されていないか？
            // @@ss 最新の初診期間だけでなく全期間も選べるように選択肢を出す

            var printingKarteNumbers = new int[0];
            using (var dlg = new KarteSelectingDialog())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    printingKarteNumbers = dlg.KarteNumbers;
                }
            }

            if (0 < printingKarteNumbers.Length)
            {
                Shinryoujo shinryoujo = new Shinryoujo(cmbSinryoujo.SelectedValue.ToString());
                var sinryouTougou = (SinryouDataLoader.診療統合種別)_cmbShinryouTougou.SelectedItem;
                HaRirekiPrintingParameter selector(int karteNumber)
                {
                    var karteId = new KarteId(shinryoujo, karteNumber);
                    var loader = KarteRepository.GetShinryouDataLoader(karteId);
                    // 最新の初診期間を指定
                    DateRange kikan = new DateRange(loader.Get初診日リスト().Last(), DateTime.MaxValue);
                    return new HaRirekiPrintingParameter(
                       karteId: karteId,
                       karteData: KarteRepository.GetKarteData(karteId),
                       shinryouDataList: loader.GetShinryouData(kikan, sinryouTougou).GetFiltered().ToArray());
                }
                List<HaRirekiPrintingParameter> parameters = printingKarteNumbers
                    .Select(selector)
                    .ToList();

                using var design = new HaRirekiPrintDesign(parameters);
                using var previewDialog = new PrintPreviewDialog
                {
                    Document = design.PrintDocument,
                    WindowState = FormWindowState.Maximized
                };

                previewDialog.ShowDialog();
            }
        }

        #endregion

        #region ツール

        private void 診療録作成ソフトのリンクを解除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _windowLink.UnLink();
        }
        private void アプリの設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OmotegakiSettings.Instance.ShowForm(this);
            OmotegakiSettings.Instance.Save();

            設定ファイルからコントロールへ反映();
        }

        private void データフォルダ共通設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClientShareSettings.Instance.ShowForm(this);
            ClientShareSettings.Instance.Save();

            設定ファイルからコントロールへ反映();
        }

        #endregion

        #region ウィンドウ

        private void 新規ウィンドウToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.ShowNewForm();
        }
        private void すべてのウィンドウを閉じるToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                    "すべてのウィンドウを閉じます。", "すべてのウィンドウを閉じる",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2
                    ) == DialogResult.OK)
            {
                Program.CloseAllForm();
            }
        }

        #endregion

        #region 表書き

        private void 表書き印刷プレビューToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckBeforePrinting())
            {
                using PrintDocument doc = new PrintDocument();

                doc.PrintPage += pd_PrintOmtegakiPage;

                //doc.SetPageSize(PrintDocumentExtension.ExPaperSize.JIS_B5);

                using var pp = new PrintPreviewDialog { Document = doc };

                pp.ShowDialog();
            }
        }
        private void 表書き印刷ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckBeforePrinting())
            {
                using PrintDocument doc = new PrintDocument();

                doc.PrintPage += pd_PrintOmtegakiPage;

                //doc.SetPageSize(PrintDocumentExtension.ExPaperSize.JIS_B5);

                using var pd = new PrintDialog { Document = doc };

                if (pd.ShowDialog(this) == DialogResult.OK)
                {
                    doc.Print();
                }
            }
        }

        #endregion

        #region カルテ

        private void カルテ印刷プレビューToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintKarte(isPreview: true);
        }
        private void カルテ印刷ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintKarte(isPreview: false);
        }
        private void カルテ一括印刷ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintKarteBatch();
        }
        private void カルテルール管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //UNDONE karteRule.sqlite ファイルパスハードコード
            string rulePath = Path.Combine(global::OmoSeitokuEreceipt.Properties.Settings.Default.EReceFolder, "karteRule.sqlite");
            using var form = new KarteRuleForm(new FileInfo(rulePath));

            form.ShowDialog(this);
        }

        #endregion

        #region ヘルプ

        private void 操作説明ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "操作説明.txt");
            try
            {
                System.Diagnostics.Process.Start(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #endregion
        #region 設定

        // 表書きソフト設定の変更後
        private void OmotegakiSettings_SettingsChange(object sender, EventArgs e)
        {
            if (OmotegakiSettings.Instance is null)
            {
                return;
            }

            _cmbShinryouTougou.SelectedItem = OmotegakiSettings.Instance.ShishikiTougou
                                            ? SinryouDataLoader.診療統合種別.初診期間別
                                            : SinryouDataLoader.診療統合種別.統合なし;

            最近使ったデータフォルダーToolStripMenuItem.DropDown.Items.Clear();
            最近使ったデータフォルダーToolStripMenuItem.DropDown.Items.AddRange(
                OmotegakiSettings.Instance.DataFolderHistory
                    .Select(p => new ToolStripMenuItem(p))
                    .ToArray());
        }
        // 共通設定のデータフォルダーの変更後
        private void CommonSettings_DataFolderChanged(object sender, OmoSeitokuEreceipt.Properties.Settings.ExPropertyChangedEventArgs e)
        {
            bool isFirstTime = string.IsNullOrEmpty(e.OldValue);

            if (!isFirstTime)
            {
                // ソフトを立ち上げなおす

                try
                {
                    Program.Restart();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(new StringBuilder()
                                    .AppendLine("ソフトの再起動に失敗しました。")
                                    .AppendLine("手動で起動してください。")
                                    .AppendLine()
                                    .AppendLine("-- エラーメッセージ --")
                                    .AppendLine(ex.Message)
                                    .ToString());
                    Close();
                }
            }
        }

        #endregion

        // 歯病症タブ 選択インデックス変更時
        private void tabHaByoSyo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // タブページごとのスクロール情報を保存
            if (_tabHaByoSyo.SelectedTab != _tabPageHaByoSyoPrev)
            {
                var pageData = (TabHaByoSyoPageData)_tabPageHaByoSyoPrev.Tag;
                pageData.ScrollPercent = _syochiRirekiList.ScrollPercent;
            }

            _tabPageHaByoSyoPrev = _tabHaByoSyo.SelectedTab;

            OnFilterChanged();
        }

        // 表書き印刷時
        private void pd_PrintOmtegakiPage(object sender, PrintPageEventArgs e)
        {
            KarteData karteData = _vm.CurrentKarteData;
            SinryouData[] sinryouItems = _vm.ShinryouDataCollection.ToArray();
            OmotegakiPrintDesign.DrawOmotegaki(karteData, e.Graphics, e.PageSettings, sinryouItems);

            // 終端ページ
            e.HasMorePages = false;
        }

        // 初診リスト 全期間　チェックボックス
        private void _btnすべての初診リストにチェック_Click(object sender, EventArgs e)
        {
            foreach (var item in _vm.ShoshinKikanItems)
            {
                item.IsChecked = true;
            }
        }

        private void _btnすべての初診リストのチェックを外す_Click(object sender, EventArgs e)
        {
            foreach (var item in _vm.ShoshinKikanItems)
            {
                item.IsChecked = false;
            }
        }

        private void _chk最新の期間のみチェック_CheckedChanged(object sender, EventArgs e)
        {
            if ((_vm.CurrentKarteData is object) &&
                _chk最新の期間のみチェック.Checked)
            {
                UpdateKarteData(false);
            }
        }

        /// <summary>
        /// ビューワー設定コンボボックスの値変更時
        /// </summary>
        private void _cmbViewerSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cmbViewerSettings.SelectedItem is null)
            {
                return;
            }

            // 選択したテンプレートを反映
            try
            {
                _suspendUpdateCommonFilter = true;
                _suspendUpdateKarteData = true;

                var settings = (SinryouViewerSettings)_cmbViewerSettings.SelectedItem;

                _chkSisyuKanzen.Checked = settings.選択歯完全一致;
                _chkPJogai.Checked = settings.Checked病名P;
                _chkGJogai.Checked = settings.Checked病名G;
                _chkGishiJogai.Checked = settings.Checked病名義歯;
                _cmbJogaiType.SelectedIndex = settings.病名FilterType;
                ((ShinryouOrderTypeSelector)_cmbSinryouOrder.Child).SelectedValue = settings.並び順Type;
                _cmbShinryouTougou.SelectedItem = settings.診療統合;
            }
            finally
            {
                _suspendUpdateCommonFilter = false;
                _suspendUpdateKarteData = false;

                UpdateCommonFilter();
            }

            // 選択状態を解除
            new Thread((ThreadStart)delegate
            {
                BeginInvoke((Action)delegate
                {
                    _cmbViewerSettings.SelectedItem = null;
                });
            }).Start();
        }

        private void BtnAddTemplate_Click(object sender, EventArgs e)
        {
            // 現在のビューワー設定状態を保存してテンプレートに追加
            string name = InputDialog.Show("現在の表示設定を保存します。任意の設定名を入力してください。既にある名前を指定した場合は上書きされます。");
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            var settings = new SinryouViewerSettings(name)
            {
                選択歯完全一致 = _chkSisyuKanzen.Checked,
                Checked病名P = _chkPJogai.Checked,
                Checked病名G = _chkGJogai.Checked,
                Checked病名義歯 = _chkGishiJogai.Checked,
                病名FilterType = _cmbJogaiType.SelectedIndex,
                並び順Type = ((ShinryouOrderTypeSelector)_cmbSinryouOrder.Child).SelectedValue,
                診療統合 = (SinryouDataLoader.診療統合種別)_cmbShinryouTougou.SelectedItem,
            };

            var exists = ClientShareSettings.Instance.ShinryouViewerSettings
                .Select((v, index) => new { v.Name, index })
                .Where(a => name.Equals(a.Name, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            if (exists != null)
            {
                // 既にある場合は上書き
                ClientShareSettings.Instance.ShinryouViewerSettings[exists.index] = settings;
            }
            else
            {
                // まだ無いので追加
                ClientShareSettings.Instance.ShinryouViewerSettings.Add(settings);
            }

            ClientShareSettings.Instance.Save();

            設定ファイルからコントロールへ反映();
        }

        private void BtnRemoveTemplate_Click(object sender, EventArgs e)
        {
            var msg = new StringBuilder()
                .AppendLine("削除するテンプレートの番号を入力してください。")
                .AppendLine();
            foreach (var item in ClientShareSettings.Instance.ShinryouViewerSettings.Select((p, i) => new { p.Name, i }))
            {
                msg.AppendLine($"{item.i}: {item.Name}");
            }

            while (true)
            {
                string input = InputDialog.Show(msg.ToString());

                if (string.IsNullOrWhiteSpace(input))
                {
                    return;
                }
                else if (!int.TryParse(input, out int inputNumber))
                {
                    ShowMessage("数値で指定してください。", true, false);
                }
                else
                {
                    try
                    {
                        ClientShareSettings.Instance.ShinryouViewerSettings.RemoveAt(inputNumber);
                        ClientShareSettings.Instance.Save();
                        設定ファイルからコントロールへ反映();
                        return;
                    }
                    catch (Exception ex)
                    {
                        ShowMessage(ex.Message, true, false);
                    }
                }
            }
        }

        #endregion

        #region WindowLink

        bool WindowLink.IWindow.OnPollingMessageReceived(IntPtr callerHandle, string callerTitle)
        {
            // 相手のタイトルにデータフォルダーが含まれている場合は応答する。
            return 相手のタイトルにデータフォルダーが含まれている(callerTitle);
        }
        bool WindowLink.IWindow.OnNewWindowMessageReceived()
        {
            try
            {
                Program.ShowNewForm();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        bool WindowLink.IWindow.OnLinkMessageReceived(IntPtr callerHandle, string callerTitle)
        {
            // 相手のタイトルにデータフォルダーが含まれている場合はリンクを了承する。
            return 相手のタイトルにデータフォルダーが含まれている(callerTitle);
        }
        void WindowLink.IWindow.OnLinkedWindowChanged()
        {
            if (_windowLink != null)
            {
                bool linked = _windowLink.IsLinked;

                診療録作成ソフトのリンクを解除ToolStripMenuItem.Enabled = linked;

                _pnlKarteLoader.Visible = !linked;

                データフォルダーToolStripMenuItem.Enabled = !linked;
                データフォルダーの変更ToolStripMenuItem.Enabled = !linked;
                最近使ったデータフォルダーToolStripMenuItem.Enabled = !linked;

                UpdateShinryoujoSelector();
                Invalidate();
            }
        }
        void WindowLink.IWindow.OnTextReceived(byte[] text)
        {
            //UNDONE @@ss リンク中の診療録ソフトから診療情報テキストを受け取り診療リストを表示
            MessageBox.Show(Encoding.GetEncoding("Shift_JIS").GetString(text));
        }
        IntPtr WindowLink.IWindow.OnValueReceived(IntPtr value)
        {
            // value: [123][1-9][0-9]* の数値。 診療所 + カルテ番号
            try
            {
                string sval = value.ToString();
                if (!int.TryParse(sval.Substring(0, 1), out int sinryoujo))
                {
                    throw new Exception("リンクメッセージの診療所の指定が異常です。");
                }
                sinryoujo--;
                cmbSinryoujo.SelectedIndex = sinryoujo;

                if (!int.TryParse(sval.Substring(1), out int karteNo))
                {
                    throw new Exception("リンクメッセージのカルテ番号の指定が異常です。");
                }

                BeginInvoke((Action<int>)(num =>
                {
                    LoadKarteData(num, false);
                }), karteNo);

                return new IntPtr(1);
            }
            catch (IndexOutOfRangeException ex)
            {
                BeginInvoke((Action<string>)(msg =>
                {
                    MessageBox.Show("未対応の診療所が指定されました。 error: " + msg);
                }), ex.Message);
            }
            catch (Exception ex)
            {
                BeginInvoke((Action<string>)(msg =>
                {
                    MessageBox.Show(msg);
                }), ex.Message);
            }
            return IntPtr.Zero;
        }
        void WindowLink.IWindow.OnCloseAppReceived()
        {
            if (_windowLink != null)
            {
                _windowLink.UnLink();
            }
            Close();
        }

        private static bool 相手のタイトルにデータフォルダーが含まれている(string callerTitle)
        {
            callerTitle = normalizePath(callerTitle);

            if (相手のタイトルにデータフォルダーが含まれている_Cache.TryGetValue(callerTitle, out bool result))
            {
                return result;
            }

            // 場合は応答する。
            var uri = new Uri(global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder);

            if (checkTitle(uri))
            {
                result = true;
            }
            else
            {
                var hostEntry = Dns.GetHostEntry(uri.Host);
                Uri replaced;

                switch (uri.HostNameType)
                {
                    // データフォルダがIPアドレスの場合、ホスト名に変更して再調査
                    case UriHostNameType.IPv4:
                    case UriHostNameType.IPv6:
                        replaced = replaceHost(uri, hostEntry.HostName);
                        if (checkTitle(replaced))
                        {
                            result = true;
                        }
                        else
                        {
                            replaced = replaceHost(uri, hostEntry.HostName.Replace(".local", ""));
                            result = checkTitle(replaced);
                        }
                        break;

                    // データフォルダがホスト名の場合、IPアドレスに変更して再調査
                    default:
                        IPAddress ip = hostEntry.AddressList.Where(o => o.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First();
                        replaced = replaceHost(uri, ip.ToString());
                        result = checkTitle(replaced);
                        break;
                }
            }

            相手のタイトルにデータフォルダーが含まれている_Cache.AddOrUpdate(callerTitle, result, (s, b) => result);

            return result;


            bool checkTitle(Uri path)
            {
                string nm = normalizePath(path.ToString());
                return callerTitle.IndexOf(nm, StringComparison.OrdinalIgnoreCase) >= 0;
                //return path.Segments.All(seg => callerTitle.IndexOf(seg, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            static Uri replaceHost(Uri original, string newHostName)
            {
                var builder = new UriBuilder(original) { Host = newHostName };
                return builder.Uri;
            }

            static string normalizePath(string path)
            {
                return Regex.Replace(path, @"file:|\\|/", "");
            }
        }
        private static readonly ConcurrentDictionary<string, bool>
            相手のタイトルにデータフォルダーが含まれている_Cache = new ConcurrentDictionary<string, bool>();

        #endregion

        private sealed class TabHaByoSyoPageData
        {
            public SinryouFilter Filter;
            public PointF? ScrollPercent;
            /// <summary>表示回数</summary>
            public int ShowCount;

            public TabHaByoSyoPageData(SinryouFilter filter)
            {
                Filter = filter;
            }
            public TabHaByoSyoPageData()
            {
            }

            public void Reset()
            {
                ScrollPercent = null;
                ShowCount = 0;
            }

        }

        private sealed class 歯種Order右左右左 : IComparable<歯種Order右左右左>, IComparable<ER歯式>, IComparable
        {
            private readonly ER歯式 _source;

            public 歯種Order右左右左(ER歯式 source)
            {
                _source = source;
            }

            public int CompareTo(歯種Order右左右左 other)
            {
                CompareHint left = GetCompareHint(_source);
                CompareHint right = GetCompareHint(other._source);

                return left.CompareTo(right);
            }
            public int CompareTo(ER歯式 other)
            {
                CompareHint left = GetCompareHint(_source);
                CompareHint right = GetCompareHint(other);

                return left.CompareTo(right);
            }
            public int CompareTo(object obj)
            {
                return obj switch
                {
                    歯種Order右左右左 hint => ((IComparable<歯種Order右左右左>)this).CompareTo(hint),
                    ER歯式 sisi => ((IComparable<ER歯式>)this).CompareTo(sisi),
                    _ => 1,
                };
            }

            /// <summary>
            /// 歯式の右側から左側にかけて比較用の値を取得する。
            /// </summary>
            /// <returns></returns>
            private static CompareHint GetCompareHint(ER歯式 source)
            {
                var list = source.Get歯列By部位(ER歯式.部位.右側上顎);
                if (list.Count != 0)
                    return new CompareHint(1, list[list.Count - 1]);

                list = source.Get歯列By部位(ER歯式.部位.左側上顎);
                if (list.Count != 0)
                    return new CompareHint(2, list[0]);

                list = source.Get歯列By部位(ER歯式.部位.右側下顎);
                if (list.Count != 0)
                    return new CompareHint(3, list[0]);

                list = source.Get歯列By部位(ER歯式.部位.左側下顎);
                if (list.Count != 0)
                    return new CompareHint(4, list[list.Count - 1]);

                return new CompareHint(CompareHint.EMPTY_HINT, ER歯式単位.Empty);
            }

            private sealed class CompareHint : IComparable<CompareHint>
            {
                public const int EMPTY_HINT = -1;


                private readonly int _hint;
                private readonly ER歯式単位 _value;

                public CompareHint(int hint, ER歯式単位 value)
                {
                    _hint = hint;
                    _value = value;
                }
                public int CompareTo(CompareHint other)
                {
                    int res = _hint.CompareTo(other._hint);

                    if ((res == 0) && (_hint != EMPTY_HINT))
                    {
                        res = _value.CompareTo(other._value);
                        if (_value.部位.Is右側()) res = -res;
                    }

                    return res;
                }
            }
        }

        private void _btnToggleFilterControlPanel_Click(object sender, EventArgs e)
        {
            _filterControlPanel.Visible = !_filterControlPanel.Visible;
        }

        private async void データ変換ヤハラToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dialogResult = InputDialog.Show("変換数を入力 (0で最大数) または k数値 でカルテ番号を1件入力。");
            if (string.IsNullOrWhiteSpace(dialogResult)) return;

            int? karteNumber = null;
            int limit = 0;

            if (dialogResult[0] == 'k')
            {
                if (!int.TryParse(dialogResult.Substring(1), out int res))
                {
                    MessageBox.Show("有効なカルテ番号ではありません。");
                    return;
                }
                karteNumber = res;
            }
            else
            {
                if (!int.TryParse(dialogResult, out int res))
                {
                    MessageBox.Show("有効な数値ではありません。");
                    return;
                }
                limit = res;
            }

            // 診療所
            Shinryoujo shinryoujo = new Shinryoujo(cmbSinryoujo.SelectedValue.ToString());

            try
            {
                this.Cursor = Cursors.WaitCursor;

                List<string> errors;
                try
                {
                    errors = await Task.Run(() =>
                    {
                        Models.Converters.YaharaConverter.Convert(
                            shinryoujo, karteNumber, out List<string> errors, limit);

                        return errors;
                    });
                }
                catch (Exception ex)
                {
                    errors = new List<string> { ex.Message };
                }

                if (errors != null && errors.Count > 0)
                {
                    ShowMessage(string.Join(Environment.NewLine, errors), true, false);
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show("Complete");
            }

        }
    }
}
