using System.Windows.Forms;

namespace OmoSeitoku.Controls
{
    public static class ScrollableControlExtensions
    {

        /// <summary>
        /// 自動スクロール機能が有効になっているコントロール上で、指定した子コントロールが上端に表示されるようスクロールします。
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        public static void ScrollControlIntoViewTop(this ScrollableControl parent, Control child)
        {
            // 一度最下部までスクロール
            // (取得してあるコントロールを最上部に表示する為)
            VScrollProperties vscr = parent.VerticalScroll;
            vscr.Value = vscr.Maximum;

            // 取得しておいたコントロールを表示
            parent.ScrollControlIntoView(child);
        }


    }
}
