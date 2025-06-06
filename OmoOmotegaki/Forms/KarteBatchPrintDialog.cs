using OmoSeitoku;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace OmoOmotegaki.Forms
{
    public partial class KarteBatchPrintDialog : Form
    {
        public KarteBatchSettings Settings
        {
            get { return _settings; }
            set
            {
                _settings = new KarteBatchSettings(value);


                // 期間
                DateRange2 range = value.DateRange.Range;

                if (range.Min.HasValue)
                {
                    DateTime date = range.Min.Value.Date;

                    if (date < dtpStart.MinDate)
                        date = dtpStart.MinDate;
                    else if (dtpStart.MaxDate < date)
                        date = dtpStart.MaxDate;

                    dtpStart.Value = date;
                }
                else
                {
                    dtpStart.Value = dtpStart.MinDate;
                }

                if (range.Max.HasValue)
                {
                    DateTime date = range.Max.Value.Date;

                    if (date < dtpEnd.MinDate)
                        date = dtpEnd.MinDate;
                    else if (dtpEnd.MaxDate < date)
                        date = dtpEnd.MaxDate;

                    dtpEnd.Value = date;
                    _kikanEndUserChanged = false;
                }
                else
                {
                    dtpEnd.Value = dtpEnd.MaxDate;
                    _kikanEndUserChanged = false;
                }

                // 期間 開始日 ラジオボタン
                bool ignoreDate;

                ignoreDate = !range.Min.HasValue;
                radKikanStartIgnore.Checked = ignoreDate;
                if (!ignoreDate)
                {
                    if (value.DateRange.ExpandToSyosinbi)
                    {
                        radKikanStartExpand.Checked = true;
                    }
                    else
                    {
                        radKikanStartFixed.Checked = true;
                    }
                }

                ignoreDate = !range.Max.HasValue;
                radKikanEndIgnore.Checked = ignoreDate;
                if (!ignoreDate)
                {
                    if (value.DateRange.ExpandToLastDate)
                    {
                        radKikanEndExpand.Checked = true;
                    }
                    else
                    {
                        radKikanEndFixed.Checked = true;
                    }
                }


                // 並び替え
                cmbOrder.SelectedItem = value.SortSettings.SortType.ToString();

                if (value.SortSettings.OrderDesc)
                {
                    radOrderDesc.Checked = true;
                }
                else
                {
                    radOrderAsc.Checked = true;
                }

                // プレビュー
                if ((int)numKensuu.Value != value.PreviewLimit)
                {
                    numKensuu.Value = value.PreviewLimit;
                }
            }
        }

        private KarteBatchSettings _settings;
        private Color _kikanRadioDefaultBackColor;

        public bool IsPreview;

        /// <summary>開始日の日付をキーボードから入力中</summary>
        private bool _isStartDateKeyInputting;

        /// <summary>終了日がユーザーによって変更済みかどうか</summary>
        private bool _kikanEndUserChanged
        {
            get { return !pnlAutoSetKikanEnd.Visible; }
            set { pnlAutoSetKikanEnd.Visible = !value; }
        }


        // コンストラクタ
        public KarteBatchPrintDialog()
        {
            InitializeComponent();

            // 期間 開始日
            {
                TextBox kikanDummyTxt = new TextBox();
                kikanDummyTxt.Text = "最小日";
                kikanDummyTxt.TextAlign = HorizontalAlignment.Center;
                kikanDummyTxt.TabStop = false;
                kikanDummyTxt.ReadOnly = true;
                kikanDummyTxt.Bounds = dtpStart.Bounds;
                dtpStart.Parent.Controls.Add(kikanDummyTxt);
                dtpStart.BringToFront();

                radKikanStartIgnore.Checked = true;
                radKikanStartIgnore.CheckedChanged += this.Kikan_CheckedChanged;
                radKikanStartExpand.CheckedChanged += this.Kikan_CheckedChanged;
                radKikanStartFixed.CheckedChanged += this.Kikan_CheckedChanged;
                dtpStart.Value = dtpStart.MinDate;
            }

            // 期間 終了日
            {
                TextBox kikanDummyTxt = new TextBox();
                kikanDummyTxt.Text = "最大日";
                kikanDummyTxt.TextAlign = HorizontalAlignment.Center;
                kikanDummyTxt.TabStop = false;
                kikanDummyTxt.ReadOnly = true;
                kikanDummyTxt.Bounds = dtpEnd.Bounds;
                dtpEnd.Parent.Controls.Add(kikanDummyTxt);
                dtpEnd.BringToFront();

                radKikanEndIgnore.Checked = true;
                radKikanEndIgnore.CheckedChanged += this.Kikan_CheckedChanged;
                radKikanEndExpand.CheckedChanged += this.Kikan_CheckedChanged;
                radKikanEndFixed.CheckedChanged += this.Kikan_CheckedChanged;
                dtpEnd.Value = dtpEnd.MaxDate;
                _kikanEndUserChanged = false;
            }

            // 並び替えコンボボックス
            var orderVals = Enum.GetNames(typeof(KartePrintItemList.SortType));
            var orderItems = new string[orderVals.Length];
            for (int i = orderVals.Length - 1; 0 <= i; --i)
            {
                orderItems[i] = orderVals[i];
            }
            cmbOrder.Items.AddRange(orderItems);


            cmbOrder.SelectedItem = KartePrintItemList.SortSettings.Default.SortType.ToString();
            radOrderAsc.Checked = true;
        }


        private KarteBatchSettings CreateSettings()
        {
            var s = _settings ?? new KarteBatchSettings();

            // 期間
            var range = new DateRange2();

            if (!radKikanStartIgnore.Checked)
            {
                range.Min = dtpStart.Value.Date;
            }

            if (!radKikanEndIgnore.Checked)
            {
                range.Max = dtpEnd.Value.Date;
            }

            s.DateRange = new KarteDateRange(
                range: range,
                expandToSyosinbi: radKikanStartExpand.Checked,
                expandToLastDate: radKikanEndExpand.Checked
                );


            // ソート設定
            var sortSet = new KartePrintItemList.SortSettings();

            sortSet.SortType = (KartePrintItemList.SortType)Enum.Parse(
                                    typeof(KartePrintItemList.SortType),
                                    (string)cmbOrder.SelectedItem);

            sortSet.OrderDesc = radOrderDesc.Checked;

            s.SortSettings = sortSet;


            // プレビュー設定
            s.PreviewLimit = (chkKensuuZenken.Checked ? 0 : (int)numKensuu.Value);

            return s;
        }



        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Kikan_CheckedChanged(null, e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // 開始日がキーボードから入力中の場合
            if (_isStartDateKeyInputting)
            {
                // 開始日のフォーカスを外し、日付の変更を確定させる。
                // （入力中は日付の値が変更されない為。）
                if (dtpStart.Focused)
                {
                    this.ActiveControl = null;
                }
                _isStartDateKeyInputting = false;
            }
        }

        #region 期間

        private void Kikan_CheckedChanged(object sender, EventArgs e)
        {
            // 期間入力コントロールの設定
            bool ignoreStart = radKikanStartIgnore.Checked;
            bool ignoreEnd = radKikanEndIgnore.Checked;

            dtpStart.Enabled = !ignoreStart;
            dtpEnd.Enabled = !ignoreEnd;

            if (ignoreStart)
                dtpStart.SendToBack();
            else
                dtpStart.BringToFront();

            if (ignoreEnd)
                dtpEnd.SendToBack();
            else
                dtpEnd.BringToFront();


            // ラジオボタン
            var rads = new RadioButton[]
            {
                radKikanStartIgnore,
                radKikanStartExpand,
                radKikanStartFixed,
                radKikanEndIgnore,
                radKikanEndExpand,
                radKikanEndFixed
            };
            Color defaultColor = _kikanRadioDefaultBackColor;
            if (defaultColor == null)
            {
                defaultColor = _kikanRadioDefaultBackColor = rads[0].BackColor;
            }
            foreach (var rad in rads)
            {
                rad.BackColor = (rad.Checked ? Color.Gold : defaultColor);
            }
        }

        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            // 開始日が変更されるごとに終了日を変更する。
            if (!_kikanEndUserChanged)
            {
                try
                {
                    dtpEnd.Value = GetMonthLastDate(dtpStart.Value);
                    _kikanEndUserChanged = false;
                }
                catch
                {
                }
            }
        }

        private void dtpStart_KeyUp(object sender, KeyEventArgs e)
        {
            if (((int)Keys.D0 <= e.KeyValue && e.KeyValue <= (int)Keys.D9)
                || ((int)Keys.NumPad0 <= e.KeyValue && e.KeyValue <= (int)Keys.NumPad9))
            {
                // 開始日が変更されるごとに終了日を変更する。
                if (!_kikanEndUserChanged)
                {
                    try
                    {
                        dtpEnd.Value = GetMonthLastDate(dtpStart.Value);
                        _kikanEndUserChanged = false;
                    }
                    catch
                    {
                    }

                    _isStartDateKeyInputting = true;
                }
            }
        }

        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            _kikanEndUserChanged = true;
        }

        private void btnStartToday_Click(object sender, EventArgs e)
        {
            dtpStart.Value = DateTime.Today;
        }

        private void btnSetDateD_Click(object sender, EventArgs e)
        {
            dtpEnd.Value = dtpStart.Value;
        }

        #endregion

        #region 下部コントロールパネル

        private void chkKensuuZenken_CheckedChanged(object sender, EventArgs e)
        {
            numKensuu.Enabled = !chkKensuuZenken.Checked;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            this.Settings = CreateSettings();
            this.IsPreview = true;

            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void btnPrintAll_Click(object sender, EventArgs e)
        {
            this.Settings = CreateSettings();
            this.IsPreview = false;

            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            this.Close();
        }

        #endregion



        /// <summary>
        /// 指定した日付の月の末日を取得する。
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private static DateTime GetMonthLastDate(DateTime dt)
        {
            dt = new DateTime(dt.Year, dt.Month, 1);
            return dt.AddMonths(1).AddDays(-1);
        }
    }
}
