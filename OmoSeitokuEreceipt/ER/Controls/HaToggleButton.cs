using OmoEReceLib.ERObjects;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace OmoSeitokuEreceipt.ER.Controls
{
    public sealed class HaToggleButton : Label
    {
        private static Color KANNI_BUTTON_FORE_COLOR = Color.Black;
        private static Color KANNI_BUTTON_BACK_COLOR = Color.LightBlue;
        private static Color KANNI_SELECTED_BUTTON_BACK_COLOR = Color.FromArgb(120, Color.Orange);


        public event EventHandler IsCheckedChanged = delegate { };

        private bool _focused;
        private Pen _focusedPen = new Pen(Color.OrangeRed, 2f);

        public override ContentAlignment TextAlign
        {
            get { return ContentAlignment.MiddleCenter; }
        }

        public bool FocusedKanniButton
        {
            get { return _focused; }
            set
            {
                _focused = value;
                Invalidate();
            }
        }

        public bool SemiHidden
        {
            get { return __property_SemiHidden; }
            set
            {
                __property_SemiHidden = value;
                this.UpdateStyle();
            }
        }
        private bool __property_SemiHidden;

        public readonly string 歯種;
        public readonly string 歯席;


        public bool IsChecked
        {
            get { return __property_IsChecked; }
            set
            {
                if (__property_IsChecked != value)
                {
                    __property_IsChecked = value;
                    UpdateStyle();
                    this.IsCheckedChanged(this, EventArgs.Empty);
                }
            }
        }
        private bool __property_IsChecked;


        // Constructor

        public HaToggleButton(string 歯列, string 歯席番号)
        {
            this.歯種 = 歯列 + 歯席番号;

            var st = new ER歯式単位(this.歯種, OmoEReceLib.ER_状態.現存歯);

            this.Text = st.歯席表示;
            this.歯席 = st.歯席;

            this.TextAlign = ContentAlignment.MiddleCenter;
            this.Cursor = Cursors.Hand;
            this.BorderStyle = BorderStyle.FixedSingle;
        }


        // Method

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
                _focusedPen.Dispose();
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            // ダブルクリックのワンクリック化
            base.OnClick(e);
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            // ダブルクリックのワンクリック化
            base.OnMouseClick(e);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);

            if (this.FocusedKanniButton)
            {
                Rectangle rect = this.ClientRectangle;
                rect.Inflate(-3, -3);
                pevent.Graphics.DrawRectangle(_focusedPen, rect);
            }
        }

        private void UpdateStyle()
        {
            Color c;

            if (this.IsChecked)
            {
                c = KANNI_SELECTED_BUTTON_BACK_COLOR;
            }
            else if (this.SemiHidden)
            {
                c = Color.White;
            }
            else
            {
                c = KANNI_BUTTON_BACK_COLOR;
            }

            if (base.BackColor != c)
                base.BackColor = c;
        }
    }
}
