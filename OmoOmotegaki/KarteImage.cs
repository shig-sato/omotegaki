using System;
using System.Drawing;

namespace OmoOmotegaki
{

    public sealed class KarteImage : IDisposable
    {
        public Bitmap KanjaInfo;
        public Bitmap SinryouList;
        public Bitmap BuiDisplay;
        public Bitmap Footer;

        public void Dispose()
        {
            if (KanjaInfo != null)
            {
                KanjaInfo.Dispose();
                KanjaInfo = null;
            }

            if (SinryouList != null)
            {
                SinryouList.Dispose();
                SinryouList = null;
            }

            if (BuiDisplay != null)
            {
                BuiDisplay.Dispose();
                BuiDisplay = null;
            }

            if (Footer != null)
            {
                Footer.Dispose();
                Footer = null;
            }
        }
    }
}
