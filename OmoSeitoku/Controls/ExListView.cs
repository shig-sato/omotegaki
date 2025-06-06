using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OmoSeitoku.Controls
{
    /*
     * BackgroundImage
     * http://blogs.commentor.dk/post/ListView-Background-Image-in-NETCF.aspx
     * 
     */

    public sealed class ExListView : ListView
    {
        private SolidBrush _bgTextBrush = new SolidBrush(Color.FromArgb(110, 0, 0, 0));


        #region BackgroundImage

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref LVBKIMAGE lParam);

        struct LVBKIMAGE
        {
            public int ulFlags;
            public IntPtr hbm;
            public IntPtr pszImage; // not supported
            public int cchImageMax;
            public int xOffsetPercent;
            public int yOffsetPercent;
        }

        const int LVM_FIRST = 0x1000;
        const int LVM_SETBKIMAGE = (LVM_FIRST + 138);
        const int LVM_GETBKIMAGE = (LVM_FIRST + 139);
        const int LVBKIF_SOURCE_NONE = 0x00000000;
        const int LVBKIF_SOURCE_HBITMAP = 0x00000001;
        const int LVBKIF_STYLE_TILE = 0x00000010;
        const int LVBKIF_STYLE_NORMAL = 0x00000000;

        #endregion


        private Bitmap _bgTextBitmap;

        private string _bgText = string.Empty;

        [DefaultValue("")]
        [Description("背景に表示する文字列")]
        public string BGText
        {
            get
            {
                return _bgText;
            }
            set
            {
                _bgText = value;
                UpdateBGTextBitmap();
            }
        }

        private Font _bgTextFont;

        [Description("背景に表示する文字列のフォント")]
        public Font BGTextFont
        {
            get
            {
                return _bgTextFont;
            }
            set
            {
                _bgTextFont = value;
                UpdateBGTextBitmap();
            }
        }

        [DefaultValue(typeof(Color), "110, 0, 0, 0")]
        [Description("背景に表示する文字列の色")]
        public Color BGTextColor
        {
            get
            {
                return _bgTextBrush.Color;
            }
            set
            {
                _bgTextBrush = new SolidBrush(value);
                UpdateBGTextBitmap();
            }
        }


        public ExListView()
        {
            _bgTextFont = this.Font;


            UpdateBGTextBitmap();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_bgTextBrush != null)
            {
                _bgTextBrush.Dispose();
                _bgTextBrush = null;
            }

            ClearBGTextBitmap();
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            UpdateBGTextBitmap();
        }


        private void ClearBGTextBitmap()
        {
            Bitmap bmp = _bgTextBitmap;
            if (bmp != null)
            {
                bmp.Dispose();
                _bgTextBitmap = null;
            }
        }

        private void UpdateBGTextBitmap()
        {
            Bitmap bmp;

            string t = _bgText;

            if (t != null && 0 < t.Length)
            {
                SizeF bgSize;
                using (Graphics g = this.CreateGraphics())
                {
                    bgSize = g.MeasureString(t, _bgTextFont);
                }

                bmp = new Bitmap((int)Math.Ceiling(bgSize.Width), (int)Math.Ceiling(bgSize.Height));
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(this.BackColor);
                    g.DrawString(t, _bgTextFont, _bgTextBrush, PointF.Empty);
                }
            }
            else
            {
                bmp = null;
            }

            ClearBGTextBitmap();
            _bgTextBitmap = bmp;


            LVBKIMAGE lvBkImage = new LVBKIMAGE();
            if (bmp == null)
                lvBkImage.ulFlags = LVBKIF_SOURCE_NONE;
            else
            {
                lvBkImage.ulFlags = LVBKIF_SOURCE_HBITMAP | (this.BackgroundImageTiled ? LVBKIF_STYLE_TILE : LVBKIF_STYLE_NORMAL);
                lvBkImage.hbm = bmp.GetHbitmap();
                lvBkImage.xOffsetPercent = 50;
                lvBkImage.yOffsetPercent = 50;
            }

            SendMessage(this.Handle, LVM_SETBKIMAGE, 0, ref lvBkImage);
        }
    }
}
