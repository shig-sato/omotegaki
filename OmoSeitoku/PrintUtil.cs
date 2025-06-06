using System.Drawing;
using System.Drawing.Printing;

namespace OmoSeitoku
{
    public static class PrintUtil
    {
        /// <summary>
        /// グラフィックのスケールをB5(JIS)用紙に合わせる
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pageSettings">印刷するページ設定</param>
        public static void SetScaleB5JIS(Graphics g, PageSettings pageSettings)
        {
            RectangleF r = pageSettings.Bounds;
            g.ScaleTransform(r.Width / 717f, r.Height / 1012f);
        }
    }


    /// <summary>
    /// プリントドキュメントの拡張メソッドを含むクラス
    /// </summary>
    public static class PrintDocumentUtil
    {
        // 全て大文字で指定する。
        // '_' で区切られた文字が全て含まれる PaperSize.PaperName が選択される。
        public enum ExPaperSize
        {
            A4,
            JIS_B5
        }

        /// <summary>
        /// プリントドキュメントのデフォルトページ設定のページサイズを指定
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="size"></param>
        /// <returns>成功時はtrue</returns>
        public static bool SetPageSize(PrintDocument doc, ExPaperSize size)
        {
            var paper = SearchPaperSize(doc.PrinterSettings, size);

            if (paper != null)
            {
                doc.DefaultPageSettings.PaperSize = paper;
                return true;
            }
            return false;
        }

        /// <summary>
        /// プリンター設定から対応する用紙サイズを取得
        /// </summary>
        /// <param name="printer"></param>
        /// <param name="size"></param>
        /// <returns>見つからない場合は null</returns>
        public static PaperSize SearchPaperSize(PrinterSettings printer, ExPaperSize size)
        {
            // 用紙サイズ名の '_' で区切られた文字が全て含まれる PaperSize.PaperName を探す。

            string[] searchTexts = size.ToString().Split('_');

            foreach (PaperSize paperSize in printer.PaperSizes)
            {
                string paperName = paperSize.PaperName.ToUpper();
                bool containsAll = true;

                foreach (string search in searchTexts)
                {
                    if (!paperName.Contains(search))
                    {
                        containsAll = false;
                        break;
                    }
                }

                if (containsAll)
                {
                    return paperSize;
                }
            }

            return null;
        }
    }
}
