using OmoSeitoku.Controls;
using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OmoOmotegaki.Controls
{
    public sealed class SinryouFilterTabPage : TabPage
    {
        public event ItemButtonClickEventHandler DeleteButtonClick;
        public event ItemButtonClickEventHandler EditButtonClick;

        public delegate void ItemButtonClickEventHandler(object sender, ItemEventArgs e);

        public sealed class ItemEventArgs : EventArgs
        {
            public SinryouFilter Filter { get; private set; }

            public ItemEventArgs(SinryouFilter filter)
            {
                this.Filter = filter;
            }
        }

        private FlowLayoutPanel _filtersPanel;

        public SinryouFilterTabPage(string text)
            : base(text)
        {
            this.BackColor = Color.DarkGoldenrod;

            _filtersPanel = new FlowLayoutPanel();
            _filtersPanel.Dock = DockStyle.Fill;
            _filtersPanel.AutoScroll = true;
            _filtersPanel.Padding = Padding.Empty;
            this.Controls.Add(_filtersPanel);
        }

        public SinryouFilterTabPage()
            : this(string.Empty)
        {
        }



        public void SetFilters(SinryouFilter[] filters)
        {
            var size = new Size(_filtersPanel.ClientSize.Width - SystemInformation.VerticalScrollBarWidth, 0);

            var pnls = new FilterDispPanel[filters.Length];
            bool isLast = true;

            for (int i = filters.Length - 1; 0 <= i; --i)
            {
                var pnl = pnls[i] = new FilterDispPanel();
                pnl.SetFilter(filters[i]);
                pnl.DeleteButtonClick += OnItemDeleteButtonClick;
                pnl.EditButtonClick += OnItemEditButtonClick;

                pnl.MinimumSize = size;

                if (isLast)
                {
                    isLast = false;
                    pnl.Margin = new Padding(0, 0, 0, pnl.Height * 2);
                }
                else
                {
                    pnl.Margin = new Padding(0, 0, 0, 12);
                }
            }

            _filtersPanel.Controls.Clear();
            _filtersPanel.Controls.AddRange(pnls);
        }

        public void ScrollToFilter(SinryouFilter filter)
        {
            string targetKey = filter.GetKey();

            foreach (var c in _filtersPanel.Controls)
            {
                var pnl = c as FilterDispPanel;
                if (pnl != null && pnl.Filter != null && targetKey.Equals(pnl.Filter.GetKey()))
                {
                    _filtersPanel.ScrollControlIntoViewTop(pnl);
                }
            }
        }

        private void OnItemDeleteButtonClick(object sender, EventArgs e)
        {
            if (DeleteButtonClick != null)
            {
                DeleteButtonClick(this, new ItemEventArgs(((FilterDispPanel)sender).Filter));
            }
        }

        private void OnItemEditButtonClick(object sender, EventArgs e)
        {
            if (EditButtonClick != null)
            {
                EditButtonClick(this, new ItemEventArgs(((FilterDispPanel)sender).Filter));
            }
        }

        public sealed class FilterDispPanel : Panel
        {
            public event EventHandler DeleteButtonClick;
            public event EventHandler EditButtonClick;


            public SinryouFilter Filter { get; private set; }

            private FlowLayoutPanel _filterPanel;
            private Font _titleFont;
            private Label _lblTitle;



            public FilterDispPanel()
            {
                this.SuspendLayout();

                this.AutoSizeMode = AutoSizeMode.GrowOnly;
                this.AutoSize = true;
                this.BackColor = Color.White;
                this.Font = new Font(this.Font.FontFamily, 13f);
                _titleFont = new Font(this.Font.FontFamily, this.Font.Size - 2f, FontStyle.Bold);


                _filterPanel = new FlowLayoutPanel();
                _filterPanel.Dock = DockStyle.Fill;
                _filterPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                _filterPanel.AutoSize = true;
                _filterPanel.Padding = new Padding(0, 4, 0, 2);
                this.Controls.Add(_filterPanel);



                var footerPanel = new Panel();
                footerPanel.BackColor = Color.Wheat;
                footerPanel.Dock = DockStyle.Bottom;
                footerPanel.Height = 12;
                this.Controls.Add(footerPanel);

                var picFooterBorder = new PictureBox();
                picFooterBorder.BackColor = Color.DarkGoldenrod;
                picFooterBorder.Dock = DockStyle.Top;
                picFooterBorder.Height = 2;
                footerPanel.Controls.Add(picFooterBorder);


                var headerPanel = new Panel();
                headerPanel.BackColor = footerPanel.BackColor;
                headerPanel.Dock = DockStyle.Top;
                {
                    _lblTitle = new Label();
                    _lblTitle.AutoSize = true;
                    _lblTitle.Font = _titleFont;
                    _lblTitle.Location = new Point(8, 1);
                    headerPanel.Controls.Add(_lblTitle);

                    var editButton = new Button();
                    editButton.AutoSize = true;
                    editButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    editButton.Text = "フィルター編集 ...";
                    editButton.Font = new Font(this.Font.FontFamily, 10f);
                    editButton.BackColor = SystemColors.ButtonFace;
                    editButton.Click += this.OnEditButtonClick;
                    headerPanel.Controls.Add(editButton);


                    var deleteButton = new Button();
                    deleteButton.AutoSize = true;
                    deleteButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                    deleteButton.Text = "削除 ...";
                    deleteButton.Font = editButton.Font;
                    deleteButton.BackColor = editButton.BackColor;
                    deleteButton.Click += this.OnDeleteButtonClick;
                    headerPanel.Controls.Add(deleteButton);


                    var picBorder = new PictureBox();
                    picBorder.BackColor = picFooterBorder.BackColor;
                    picBorder.Dock = DockStyle.Bottom;
                    picBorder.Height = 2;
                    headerPanel.Controls.Add(picBorder);


                    int pad = editButton.Height / 2;
                    editButton.Location = new Point(pad, _lblTitle.Bottom + _lblTitle.Margin.Bottom);
                    headerPanel.Height = editButton.Bottom + 4;

                    headerPanel.Resize += delegate
                    {
                        deleteButton.Location = new Point(
                            headerPanel.ClientSize.Width - deleteButton.Width - pad,
                            _lblTitle.Bottom + _lblTitle.Margin.Bottom);
                    };
                }
                this.Controls.Add(headerPanel);



                this.ResumeLayout(false);
            }


            public void ClearFilter()
            {
                this.Controls.Clear();
            }

            public void SetFilter(SinryouFilter filter)
            {
                this.Filter = filter;

                if (filter == null)
                {
                    this.ClearFilter();
                    return;
                }


                // コントロールの作成

                this.SuspendLayout();
                {
                    _filterPanel.Controls.Clear();

                    var addControls = new List<Control>();

                    var dataPad = new Padding(18, 4, 0, 4);

                    if (filter.Has病名)
                    {
                        var pnl = new FlowLayoutPanel();
                        pnl.FlowDirection = FlowDirection.TopDown;
                        pnl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                        pnl.AutoSize = true;


                        var lblType = new Label();
                        lblType.AutoSize = true;
                        lblType.Font = _titleFont;
                        lblType.Text = string.Format("病名 ({0})", filter.病名MatchType);
                        pnl.Controls.Add(lblType);


                        var lbl = new Label();
                        lbl.AutoSize = true;
                        lbl.Text = filter.病名;
                        lbl.Margin = dataPad;
                        pnl.Controls.Add(lbl);


                        addControls.Add(pnl);
                    }

                    if (filter.Has処置)
                    {
                        var pnl = new FlowLayoutPanel();
                        pnl.FlowDirection = FlowDirection.TopDown;
                        pnl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                        pnl.AutoSize = true;


                        var lblType = new Label();
                        lblType.AutoSize = true;
                        lblType.Font = _titleFont;
                        lblType.Text = string.Format("処置 ({0})", filter.処置MatchType);
                        pnl.Controls.Add(lblType);


                        var lbl = new Label();
                        lbl.AutoSize = true;
                        lbl.Text = filter.処置;
                        lbl.Margin = dataPad;
                        pnl.Controls.Add(lbl);


                        addControls.Add(pnl);
                    }

                    if (filter.Has歯式)
                    {
                        var pnl = new FlowLayoutPanel();
                        pnl.FlowDirection = FlowDirection.TopDown;
                        pnl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                        pnl.AutoSize = true;


                        var lblType = new Label();
                        lblType.AutoSize = true;
                        lblType.Font = _titleFont;
                        lblType.Text = filter.歯種完全一致 ? "歯種(完全一致)" : "歯式";
                        pnl.Controls.Add(lblType);


                        var pic = new PictureBox();
                        pic.Margin = dataPad;
                        {
                            var img = new Bitmap(_filterPanel.Width - 32, 96);
                            pic.Size = img.Size;
                            using (var g = Graphics.FromImage(img))
                            using (var font = new Font(this.Font.FontFamily, 14f))
                            {
                                OmoSeitokuEreceipt.Drawing.ERObjectDrawer.DrawSisikiFixed(
                                    g, Pens.Red, filter.歯式, new[] { filter.病名 },
                                    new RectangleF(PointF.Empty, img.Size), font, true);
                            }
                            pic.Image = img;
                        }
                        pnl.Controls.Add(pic);


                        addControls.Add(pnl);
                    }


                    _lblTitle.Text = string.Concat("フィルター名： ", filter.Title);

                    if (addControls.Count != 0)
                    {
                        _filterPanel.Controls.AddRange(addControls.ToArray());
                    }
                    else
                    {
                        // 何もフィルターが無い場合
                        var pnl = new FlowLayoutPanel();
                        pnl.FlowDirection = FlowDirection.TopDown;
                        pnl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                        pnl.AutoSize = true;


                        var lblType = new Label();
                        lblType.AutoSize = true;
                        lblType.Font = _titleFont;
                        pnl.Controls.Add(lblType);


                        var lbl = new Label();
                        lbl.AutoSize = true;
                        lbl.Text = "（フィルターなし）";
                        pnl.Controls.Add(lbl);


                        _filterPanel.Controls.Add(pnl);
                    }
                }

                this.ResumeLayout(true);
            }

            private void OnEditButtonClick(object sender, EventArgs e)
            {
                if (this.EditButtonClick != null)
                {
                    this.EditButtonClick(this, e);
                }
            }

            private void OnDeleteButtonClick(object sender, EventArgs e)
            {
                if (MessageBox.Show(_lblTitle.Text + " 削除します。",
                                    "フィルターの削除", MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2
                                    ) == DialogResult.OK)
                {
                    if (this.DeleteButtonClick != null)
                    {
                        this.DeleteButtonClick(this, e);
                    }
                }
            }
        }
    }
}
