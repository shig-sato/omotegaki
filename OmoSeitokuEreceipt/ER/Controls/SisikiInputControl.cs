using OmoEReceLib;
using OmoEReceLib.ERObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OmoSeitokuEreceipt.ER.Controls
{
    public partial class SisikiInputControl : UserControl
    {
        private readonly Dictionary<string, string> _selectedList = new Dictionary<string, string>();

        //private bool _canInput = true;

        /// <summary>
        /// (歯種 =&gt; KanniButton)
        /// </summary>
        private readonly Dictionary<string, HaToggleButton> _kanniButtons;

        private HaToggleButton _focusedKanniButton;

        public SisikiInputControl()
        {
            InitializeComponent();

            // 状態・部分リストの構築
            lstJyoutai.DataSource = Enum.GetNames(typeof(ER_状態));
            lstBubun.DataSource = Enum.GetNames(typeof(ER_部分));

            //簡易現症図用ボタンの配置
            _kanniButtons = new Dictionary<string, HaToggleButton>(16 + 16 + 10 + 10);

            tabPageKanni.SuspendLayout();
            {
                string shiretsu;
                string mjs = ER歯式単位.右側上顎中切歯.歯種.Substring(0, 3);
                string hjs = ER歯式単位.左側上顎中切歯.歯種.Substring(0, 3);
                string mks = ER歯式単位.右側下顎中切歯.歯種.Substring(0, 3);
                string hks = ER歯式単位.左側下顎中切歯.歯種.Substring(0, 3);

                // 上顎　永久歯
                for (int i = 0; i < 16; ++i)
                {
                    string ha;

                    if (i < 8)
                    {
                        shiretsu = mjs;
                        ha = (8 - i).ToString();
                    }
                    else
                    {
                        shiretsu = hjs;
                        ha = (i - 7).ToString();
                    }

                    HaToggleButton btn = new HaToggleButton(shiretsu, ha);
                    btn.MouseClick += Kanni_button_Click;
                    tabPageKanni.Controls.Add(btn);

                    _kanniButtons.Add(btn.歯種, btn);
                }

                // 下顎　永久歯
                for (int i = 0; i < 16; ++i)
                {
                    string ha;

                    if (i < 8)
                    {
                        shiretsu = mks;
                        ha = (8 - i).ToString();
                    }
                    else
                    {
                        shiretsu = hks;
                        ha = (i - 7).ToString();
                    }

                    HaToggleButton btn = new HaToggleButton(shiretsu, ha);
                    btn.MouseClick += Kanni_button_Click;
                    tabPageKanni.Controls.Add(btn);

                    _kanniButtons.Add(btn.歯種, btn);
                }

                mjs = ER歯式単位.右側上顎乳中切歯.歯種.Substring(0, 3);
                hjs = ER歯式単位.左側上顎乳中切歯.歯種.Substring(0, 3);
                mks = ER歯式単位.右側下顎乳中切歯.歯種.Substring(0, 3);
                hks = ER歯式単位.左側下顎乳中切歯.歯種.Substring(0, 3);

                // 上顎　乳歯
                for (int i = 0; i < 10; ++i)
                {
                    string ha;

                    if (i < 5)
                    {
                        shiretsu = mjs;
                        ha = (5 - i).ToString();
                    }
                    else
                    {
                        shiretsu = hjs;
                        ha = (i - 4).ToString();
                    }

                    HaToggleButton btn = new HaToggleButton(shiretsu, ha);
                    btn.MouseClick += Kanni_button_Click;
                    tabPageKanni.Controls.Add(btn);

                    _kanniButtons.Add(btn.歯種, btn);
                }

                // 下顎　乳歯
                for (int i = 0; i < 10; ++i)
                {
                    string ha;

                    if (i < 5)
                    {
                        shiretsu = mks;
                        ha = (5 - i).ToString();
                    }
                    else
                    {
                        shiretsu = hks;
                        ha = (i - 4).ToString();
                    }

                    HaToggleButton btn = new HaToggleButton(shiretsu, ha);
                    btn.MouseClick += Kanni_button_Click;
                    tabPageKanni.Controls.Add(btn);

                    _kanniButtons.Add(btn.歯種, btn);
                }
            }
            tabPageKanni.ResumeLayout(true);

            // TODO 現症図
            {
                tabControl1.TabPages.Remove(tabPageGensyo);
                //Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                //using (Graphics g = Graphics.FromImage(bmp))
                //{
                //    RectangleF dest = new RectangleF(PointF.Empty, bmp.Size);
                //    DrawGensyou(g, dest);
                //    pictureBox1.Image = bmp;
                //    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                //}
            }

            Clear();
        }

        private HaToggleButton FocusedKanniButton
        {
            get { return _focusedKanniButton; }
            set
            {
                HaToggleButton old = _focusedKanniButton;
                bool selectedAny = value != null;

                lblCurrentInfo.Text = selectedAny ? value.歯席 : string.Empty;

                if (haSettingController.Enabled != selectedAny)
                    haSettingController.Enabled = selectedAny;

                if (old != value)
                {
                    _focusedKanniButton = value;

                    if (selectedAny)
                    {
                        value.FocusedKanniButton = true;
                    }

                    if (old != null)
                        old.FocusedKanniButton = false;
                }
            }
        }

        //[DefaultValue(true)]
        //[Browsable(true)]
        //public bool CanInput
        //{
        //    get { return _canInput; }
        //    set { _canInput = value; }
        //}

        [Browsable(false)]
        [ReadOnly(true)]
        public ER歯式 歯式
        {
            get
            {
                var sisiki = new ER歯式
                {
                    _selectedList.Values
                        .Select(erCode => ER歯式単位.FromERCode(erCode))
                        .ToArray()
                };
                return sisiki;
            }

            set
            {
                Clear();

                if (value != null)
                {
                    foreach (ER歯式単位 st in value)
                    {
                        SelectShisyu(st);

                        _kanniButtons[st.歯種].IsChecked = true;
                    }
                }
            }
        }

        public bool HasData
        {
            get { return _selectedList.Count != 0; }
        }

        public ER_状態 Selected状態
        {
            get
            {
                if (lstJyoutai.SelectedIndex == -1)
                    return ER_状態.現存歯;
                else
                    return (ER_状態)Enum.Parse(typeof(ER_状態), (string)lstJyoutai.SelectedItem);
            }
            set
            {
                lstJyoutai.SelectedItem = value.ToString();
            }
        }

        public ER_部分 Selected部分
        {
            get
            {
                if (lstBubun.SelectedIndex == -1)
                    return ER_部分.部分指定なし;
                else
                    return (ER_部分)Enum.Parse(typeof(ER_部分), (string)lstBubun.SelectedItem);
            }
            set
            {
                lstBubun.SelectedItem = value.ToString();
            }
        }

        private void Clear()
        {
            foreach (HaToggleButton btn in _kanniButtons.Values)
            {
                btn.IsChecked = false;
                UnselectShisyu(btn.歯種);
            }
            _selectedList.Clear();

            ClearHaSettingController();
        }

        private void ClearHaSettingController()
        {
            if (FocusedKanniButton != null)
            {
                FocusedKanniButton.IsChecked = false;
            }
            FocusedKanniButton = null; // イベント発生用のため上のブロックには入れない

            Selected状態 = ER_状態.現存歯;
            Selected部分 = ER_部分.部分指定なし;
        }

        ///// <summary>
        ///// 現症図の描画
        ///// </summary>
        ///// <param name="g"></param>
        ///// <param name="dest"></param>
        //private void DrawGensyou(Graphics g, RectangleF dest)
        //{
        //    g.DrawEllipse(Pens.Red, dest);

        //    Pen pen = new Pen(Color.Black);

        //    RectangleF eikyuusiRect = new RectangleF(dest.Location, dest.Size);
        //    eikyuusiRect.Inflate(dest.Width * -0.1315f, dest.Height * -0.0815f);

        //    RectangleF nyuusiRect = new RectangleF(dest.Location, dest.Size);
        //    nyuusiRect.Inflate(dest.Width * -0.3092f, dest.Height * -0.2283f);

        //    g.DrawEllipse(pen, eikyuusiRect);
        //    g.DrawEllipse(Pens.Purple, nyuusiRect);

        //    DrawHa(g, new RectangleF(0, 0, 20, 20));
        //    DrawHa(g, new RectangleF(50, 50, 40, 40));

        //    pen.Dispose();

        //    //TODO 現症図の描画
        //}

        //private void DrawHa(Graphics g, RectangleF dest)
        //{
        //    RectangleF inner = new RectangleF(dest.Location, dest.Size);
        //    inner.Inflate(dest.Width * -0.243f, dest.Height * -0.243f);

        //    using (GraphicsPath path = new GraphicsPath())
        //    {
        //        path.AddEllipse(dest);

        //        g.SetClip(path);

        //        //TODO draw ha

        //        path.Reset();
        //        path.AddEllipse(inner);

        //        g.SetClip(path, CombineMode.Exclude);
        //    }

        //    g.DrawLine(Pens.Black, dest.Left, dest.Top, dest.Right, dest.Bottom);
        //    g.DrawLine(Pens.Black, dest.Right, dest.Top, dest.Left, dest.Bottom);
        //    g.ResetClip();

        //    g.DrawEllipse(Pens.Purple, dest);
        //    g.DrawEllipse(Pens.Purple, inner);
        //}

        private void SelectShisyu(ER歯式単位 tanni)
        {
            if (_selectedList.ContainsKey(tanni.歯種))
            {
                // 歯種 選択済み

                _selectedList[tanni.歯種] = tanni.ERCode;
            }
            else
            {
                // 歯周 新規選択

                _selectedList.Add(tanni.歯種, tanni.ERCode);
            }

            // ボタンごとのステータスパネルを更新

            HaToggleButton btn = _kanniButtons[tanni.歯種];
            Panel pnl = btn.Tag as Panel;

            if (pnl == null && (tanni.状態 != ER_状態.現存歯 || tanni.部分 != ER_部分.部分指定なし))
            {
                btn.Tag = pnl = new Panel();
                tabPageKanni.Controls.Add(pnl);

                SetStatusPanelBounds(btn);
            }

            if (pnl != null)
            {
                const string BoxJyoutaiControlKey = "boxJyoutai";

                if (tanni.状態 != ER_状態.現存歯)
                {
                    if (!(pnl.Controls[BoxJyoutaiControlKey] is PictureBox boxJyoutai))
                    {
                        boxJyoutai = new PictureBox
                        {
                            Name = BoxJyoutaiControlKey,
                            BorderStyle = BorderStyle.FixedSingle
                        };
                        boxJyoutai.SetBounds(2, 0, 8, 6);
                        pnl.Controls.Add(boxJyoutai);
                    }

                    Bitmap bmp = new Bitmap(boxJyoutai.Width, boxJyoutai.Height);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.Clear(Color.Aqua);
                    }
                    boxJyoutai.Image = bmp;
                    boxJyoutai.Visible = true;
                }
                else if (pnl.Controls[BoxJyoutaiControlKey] is PictureBox boxJyoutai)
                {
                    boxJyoutai.Visible = false;
                }

                const string BoxBubunControlKey = "boxBubun";

                if (tanni.部分 != ER_部分.部分指定なし)
                {
                    if (!(pnl.Controls[BoxBubunControlKey] is PictureBox boxBubun))
                    {
                        boxBubun = new PictureBox
                        {
                            Name = BoxBubunControlKey,
                            BorderStyle = BorderStyle.FixedSingle
                        };
                        boxBubun.SetBounds(2 + 8 + 2, 0, 8, 6);
                        pnl.Controls.Add(boxBubun);
                    }

                    Bitmap bmp = new Bitmap(boxBubun.Width, boxBubun.Height);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.Clear(Color.Magenta);
                    }
                    boxBubun.Image = bmp;
                    boxBubun.Visible = true;
                }
                else if (pnl.Controls[BoxBubunControlKey] is PictureBox boxBubun)
                {
                    boxBubun.Visible = false;
                }
            }
        }

        private void UpdateShisyu(ER歯式単位 tanni)
        {
            SelectShisyu(tanni);
        }

        private void UnselectShisyu(string shisyu)
        {
            if (_kanniButtons[shisyu].Tag is Panel panel)
            {
                panel.Parent.Controls.Remove(panel);
                if (!panel.Disposing && !panel.IsDisposed) panel.Dispose();
                _kanniButtons[shisyu].Tag = null;
            }

            _selectedList.Remove(shisyu);
        }

        /// <summary>
        /// 簡易現症ボタンをクリックした際に発生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Kanni_button_Click(object sender, MouseEventArgs e)
        {
            HaToggleButton senderBtn = (HaToggleButton)sender;
            HaToggleButton lastFocused = FocusedKanniButton;

            FocusedKanniButton = senderBtn;

            ER歯式単位 st;

            if (senderBtn.IsChecked)
            {
                // ボタン選択済み

                if (e.Button == MouseButtons.Middle)
                {
                    UnselectShisyu(senderBtn.歯種);
                    ClearHaSettingController();
                    return;
                }

                st = ER歯式単位.FromERCode(_selectedList[senderBtn.歯種]);
            }
            else
            {
                // ボタン新規選択

                st = new ER歯式単位(senderBtn.歯種, ER_状態.現存歯);

                SelectShisyu(st);

                senderBtn.IsChecked = true;
            }

            if (lastFocused == FocusedKanniButton)
            {
                // 同じボタンをクリックした場合
                // リスト内の次の項目に移動
                switch (e.Button)
                {
                    case MouseButtons.Right:
                        // 右クリックの場合
                        Selected状態 = st.状態;

                        ListBox listR = lstBubun;
                        if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                            listR.SelectedIndex = (listR.SelectedIndex - 1 < 0 ? listR.Items.Count : listR.SelectedIndex) - 1;
                        else
                            listR.SelectedIndex = (listR.SelectedIndex + 1) % listR.Items.Count;
                        break;

                    default:
                        ListBox list = lstJyoutai;
                        if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
                            list.SelectedIndex = (list.SelectedIndex - 1 < 0 ? list.Items.Count : list.SelectedIndex) - 1;
                        else
                            list.SelectedIndex = (list.SelectedIndex + 1) % list.Items.Count;

                        Selected部分 = st.部分;
                        break;
                }
            }
            else
            {
                Selected状態 = st.状態;
                Selected部分 = st.部分;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            OnResize(e);
        }

        private void SetStatusPanelBounds(HaToggleButton btn)
        {
            if (btn.Tag is Panel panel)
            {
                panel.SetBounds(btn.Left + 1, btn.Bottom + 1, btn.Width - 2, 8);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (_kanniButtons != null)
            {
                using (Dictionary<string, HaToggleButton>.ValueCollection.Enumerator btns =
                                                    _kanniButtons.Values.GetEnumerator())
                {
                    tabPageKanni.SuspendLayout();

                    int vPadding = 8;
                    Size cs = tabPageKanni.ClientSize;
                    int btnX = 0, btnY = 0, btnW = cs.Width / 16, btnH = tabPageKanni.ClientSize.Height / 4 / 6 * 5;
                    int vLineWidth = cs.Width - btnW * 16;

                    // 上顎　永久歯
                    btnY = cs.Height / 2 - btnH - vPadding;
                    for (int i = 0; i < 16; ++i)
                    {
                        if (i < 8)
                            btnX = btnW * i;
                        else
                            btnX = btnW * i + vLineWidth;

                        btns.MoveNext();
                        btns.Current.SetBounds(btnX, btnY, btnW, btnH);
                        SetStatusPanelBounds(btns.Current);
                    }

                    // 下顎　永久歯
                    btnY = cs.Height / 2 + vPadding;
                    for (int i = 0; i < 16; ++i)
                    {
                        if (i < 8)
                            btnX = btnW * i;
                        else
                            btnX = btnW * i + vLineWidth;

                        btns.MoveNext();
                        btns.Current.SetBounds(btnX, btnY, btnW, btnH);
                        SetStatusPanelBounds(btns.Current);
                    }

                    // 上顎　乳歯
                    btnY = cs.Height / 2 - (btnH + vPadding) * 2;
                    for (int i = 0; i < 10; ++i)
                    {
                        if (i < 5)
                            btnX = btnW * (i + 3);
                        else
                            btnX = btnW * (i + 3) + vLineWidth;

                        btns.MoveNext();
                        btns.Current.SetBounds(btnX, btnY, btnW, btnH);
                        SetStatusPanelBounds(btns.Current);
                    }

                    // 下顎　乳歯
                    btnY = cs.Height / 2 + btnH + vPadding * 2;
                    for (int i = 0; i < 10; ++i)
                    {
                        if (i < 5)
                            btnX = btnW * (i + 3);
                        else
                            btnX = btnW * (i + 3) + vLineWidth;

                        btns.MoveNext();
                        btns.Current.SetBounds(btnX, btnY, btnW, btnH);
                        SetStatusPanelBounds(btns.Current);
                    }

                    tabPageKanni.ResumeLayout(false);
                }
            }
        }

        /// <summary>
        /// 歯選択の解除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKaijo_Click(object sender, EventArgs e)
        {
            UnselectShisyu(FocusedKanniButton.歯種);
            ClearHaSettingController();
        }

        private void LstJyoutai_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FocusedKanniButton != null)
            {
                string shisyu = FocusedKanniButton.歯種;

                var tanni = new ER歯式単位(shisyu, Selected状態, Selected部分);

                UpdateShisyu(tanni);
            }
        }

        private void LstBubun_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FocusedKanniButton != null)
            {
                string shisyu = FocusedKanniButton.歯種;

                var tanni = new ER歯式単位(shisyu, Selected状態, Selected部分);

                UpdateShisyu(tanni);
            }
        }
    }
}