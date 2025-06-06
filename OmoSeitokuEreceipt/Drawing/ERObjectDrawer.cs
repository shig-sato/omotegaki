using OmoEReceLib.ERObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OmoSeitokuEreceipt.Drawing
{
    public static class ERObjectDrawer
    {
        /// <summary>
        /// 歯式全体を描画する。
        /// </summary>
        /// <param name="g"></param>
        /// <param name="linePen"></param>
        /// <param name="sisiki"></param>
        /// <param name="byoumei"></param>
        /// <param name="destRect"></param>
        /// <param name="font"></param>
        /// <param name="separate">永久歯と乳歯の段を分けるかどうか</param>
        public static void DrawSisikiFixed(Graphics g, Pen linePen, ER歯式 sisiki, string[] byoumei, RectangleF destRect, Font font, bool separate)
        {
            float hX = destRect.Width * 0.5f;
            float hY = destRect.Height * 0.5f;

            {
                float x = destRect.X + hX;
                float y = destRect.Y + hY;
                // 縦線
                g.DrawLine(linePen, x, destRect.Top, x, destRect.Bottom);
                // 横線
                g.DrawLine(linePen, destRect.Left, y, destRect.Right, y);
            }


            bool syouryaku = false;
            //foreach (var byomei in byoumei)
            //{
            //    ////UNDONE 歯式省略 病名 ハードコード (省略判定は歯式クラスに移動したい)
            //    //if (SyochiRirekiList.P病名名称.Equals(byomei) ||
            //    //    SyochiRirekiList.G病名名称.Equals(byomei))
            //    //{
            //    //    syouryaku = true;
            //    //    break;
            //    //}
            //}

            Dictionary<ER歯式.部位, IER歯式表示単位[]> dict = sisiki.Get表示用リスト(syouryaku);
            float linePenWH = linePen.Width * 0.5f;

            using (var kigouPen = (Pen)linePen.Clone())
            {
                kigouPen.Color = Color.Black;

                foreach (var bui in dict.Keys)
                {
                    RectangleF rect = destRect; // Copy

                    rect.Width = hX - linePenWH;
                    rect.Height = hY - linePenWH;

                    switch (bui)
                    {
                        case ER歯式.部位.右側上顎:
                            break;
                        case ER歯式.部位.左側上顎:
                            rect.X += hX + linePenWH;
                            break;
                        case ER歯式.部位.左側下顎:
                            rect.X += hX + linePenWH;
                            rect.Y += hY + linePenWH;
                            break;
                        case ER歯式.部位.右側下顎:
                            rect.Y += hY + linePenWH;
                            break;
                        default:
                            throw new Exception("未対応の部位");
                    }

                    DrawSisiki(g, font, kigouPen, rect, dict[bui], bui, separate);
                }
            }
        }

        /// <summary>
        /// グラフィックの指定した範囲に歯式を描画する。
        /// </summary>
        /// <param name="g">グラフィック</param>
        /// <param name="font">描画フォント</param>
        /// <param name="kigouPen">記号用ペン</param>
        /// <param name="destRect">描画範囲</param>
        /// <param name="sisikiTanniList"></param>
        /// <param name="bui"></param>
        /// <param name="separate">永久歯と乳歯の段を分けるかどうか</param>
        private static void DrawSisiki__(Graphics g, Font font, Pen kigouPen, RectangleF destRect,
                                       IEnumerable<IER歯式表示単位> sisikiTanniList, ER歯式.部位 bui,
                                       bool separate
            )
        {
            // TODO ER歯式の並び順間違ってる。 ( 実際はこう: 記録順は右上遠心から右上近心、左上近心から左上遠心、右下遠心から右下近心、左下近心から左下遠心となります )
            // TODO 乳歯の入力位置についても間違っている。 全面書き直し.

            bool reverseH;
            bool reverseV;

            switch (bui)
            {
                case ER歯式.部位.右側上顎:
                    reverseH = true;
                    reverseV = false;
                    break;
                case ER歯式.部位.左側上顎:
                    reverseH = false;
                    reverseV = false;
                    break;
                case ER歯式.部位.左側下顎:
                    reverseH = false;
                    reverseV = true;
                    break;
                case ER歯式.部位.右側下顎:
                    reverseH = true;
                    reverseV = true;
                    break;
                default:
                    throw new Exception("未対応の部位");
            }


            PointF posEikyuusi;
            PointF posNyuusi;

            {
                SizeF fontSize = g.MeasureString("M", font);
                float fontWHalf = fontSize.Width * 0.5f;

                posEikyuusi = new PointF(
                    x: reverseH
                            ? destRect.Right - fontWHalf
                            : destRect.Left + fontWHalf,

                    y: reverseV
                            ? destRect.Bottom - fontSize.Height
                            : destRect.Top
                );

                posNyuusi = new PointF(
                    x: reverseH
                            ? destRect.Right - fontWHalf
                            : destRect.Left + fontWHalf,

                    y: reverseV
                            ? destRect.Top
                            : destRect.Bottom - fontSize.Height
                );
            }

            IEnumerable<IER歯式表示単位> list = (reverseH)
                                                    ? sisikiTanniList.Reverse()
                                                    : sisikiTanniList;
            float xDif = 0;
            float prevFontW = 0;
            bool nyuusiLoop = true;

            foreach (var sisikiTanni in list)
            {
                if (nyuusiLoop)
                {
                    if (!sisikiTanni.Is乳歯)
                    {
                        nyuusiLoop = false;
                        xDif = 0;
                        prevFontW = 0;
                    }
                }

                string str = sisikiTanni.歯席表示;
                bool isZenkaku = _sjisEncoding.GetByteCount(str) == 2; // 2byte なら全角

                SizeF fontSize = g.MeasureString(str, font);
                PointF pos = (separate && sisikiTanni.Is乳歯)
                                ? posNyuusi
                                : posEikyuusi;

                if (reverseH)
                    // 全角の場合 余計な余白分を詰める
                    xDif -= fontSize.Width * (isZenkaku ? 0.8f : 1f);
                else
                    xDif += prevFontW;

                pos.X += xDif;


                // 歯席文字の表示
                float strX = pos.X + (fontSize.Height - fontSize.Width) * 0.5f;
                float strY = pos.Y;

                float padding = 1.8f;

                // 歯席マークの表示
                switch (sisikiTanni.状態)
                {
                    case OmoEReceLib.ER_状態.現存歯:
                        g.DrawString(str, font, Brushes.Black, strX, strY);
                        break;
                    case OmoEReceLib.ER_状態.支台歯:
                        g.DrawString(str, font, Brushes.Black, strX, strY);
                        g.DrawEllipse(kigouPen, pos.X + padding, pos.Y + padding, fontSize.Height - padding * 2f, fontSize.Height - padding * 2f);
                        break;
                    case OmoEReceLib.ER_状態.欠損歯:
                        g.DrawString(str, font, Brushes.Black, strX, strY);
                        g.DrawLine(kigouPen, pos.X, pos.Y, pos.X + fontSize.Width, pos.Y + fontSize.Height);
                        g.DrawLine(kigouPen, pos.X, pos.Y + fontSize.Height, pos.X + fontSize.Width, pos.Y);
                        break;

                    case OmoEReceLib.ER_状態.部近心隙:
                        isZenkaku = false;

                        float x2 = pos.X + fontSize.Width * 0.15f;
                        float tpX = x2 + fontSize.Width * 0.5f;
                        float tpY = pos.Y + fontSize.Height * 0.25f;
                        float btm = pos.Y + fontSize.Height * 0.8f;
                        g.DrawLine(kigouPen, tpX, tpY, x2 + fontSize.Width - padding, btm);
                        g.DrawLine(kigouPen, x2 + fontSize.Width - padding, btm, x2 + padding, btm);
                        g.DrawLine(kigouPen, x2 + padding, btm, tpX, tpY);
                        break;

                    // UNDONE 他の歯状態の表示

                    default:
                        Console.WriteLine("未実装：他の歯の状態の表示");

                        // EReceLib.ER_状態.現存歯
                        g.DrawString(str, font, Brushes.Black, strX, strY);

                        break;
                }


                prevFontW = (isZenkaku)
                                ? fontSize.Width * 0.7f // 全角の場合 余計な余白分を詰める
                                : fontSize.Width;

            }
        }
        private static void DrawSisiki(Graphics g, Font font, Pen kigouPen, RectangleF destRect,
                                       IEnumerable<IER歯式表示単位> sisikiTanniList, ER歯式.部位 bui,
                                       bool separate
            )
        {
            // TODO ER歯式の並び順間違ってる。 ( 実際はこう: 記録順は右上遠心から右上近心、左上近心から左上遠心、右下遠心から右下近心、左下近心から左下遠心となります )
            // TODO 乳歯の入力位置についても間違っている。 全面書き直し.

            const float HORIZONTAL_PADDING = 2f;

            bool reverseH;
            bool reverseV;

            switch (bui)
            {
                case ER歯式.部位.右側上顎:
                    reverseH = true;
                    reverseV = false;
                    break;
                case ER歯式.部位.左側上顎:
                    reverseH = false;
                    reverseV = false;
                    break;
                case ER歯式.部位.左側下顎:
                    reverseH = false;
                    reverseV = true;
                    break;
                case ER歯式.部位.右側下顎:
                    reverseH = true;
                    reverseV = true;
                    break;
                default:
                    throw new Exception("未対応の部位");
            }


            using (var stringFormat = new StringFormat())
            {
                PointF posEikyuusi;
                PointF posNyuusi;
                {
                    posEikyuusi = new PointF(
                        x: reverseH
                                ? destRect.Right - HORIZONTAL_PADDING
                                : destRect.Left + HORIZONTAL_PADDING,

                        y: reverseV
                                ? destRect.Bottom + 3f
                                : destRect.Top
                    );

                    posNyuusi = new PointF(
                        x: posEikyuusi.X,

                        y: reverseV
                                ? destRect.Top + 1f
                                : destRect.Bottom + 1f
                    );
                }

                IEnumerable<IER歯式表示単位> list = (reverseH)
                                                        ? sisikiTanniList.Reverse()
                                                        : sisikiTanniList;
                float xDif = 0;
                float prevFontW = 0;
                bool nyuusiLoop = true;

                foreach (var sisikiTanni in list)
                {
                    if (nyuusiLoop)
                    {
                        if (!sisikiTanni.Is乳歯)
                        {
                            nyuusiLoop = false;
                            xDif = 0;
                            prevFontW = 0;
                        }
                    }

                    string str = sisikiTanni.歯席表示;
                    bool isZenkaku = _sjisEncoding.GetByteCount(str) == 2; // 2byte なら全角
                    SizeF fontSize = g.MeasureString(str, font);
                    PointF pos = (separate && sisikiTanni.Is乳歯)
                                    ? posNyuusi
                                    : posEikyuusi;

                    if (reverseV ^ sisikiTanni.Is乳歯)
                        stringFormat.LineAlignment = StringAlignment.Far;
                    else
                        stringFormat.LineAlignment = StringAlignment.Near;

                    if (reverseH)
                        // 全角の場合 余計な余白分を詰める
                        xDif -= fontSize.Width * (isZenkaku ? 0.8f : 1f);
                    else
                        xDif += prevFontW;

                    pos.X += xDif;


                    // 歯席文字の表示
                    float strX = pos.X + (fontSize.Height - fontSize.Width) * 0.5f;
                    float strY = pos.Y;

                    float padding = 1.8f;

                    // 歯席マークの表示
                    switch (sisikiTanni.状態)
                    {
                        case OmoEReceLib.ER_状態.現存歯:
                            g.DrawString(str, font, Brushes.Black, strX, strY, stringFormat);
                            break;
                        case OmoEReceLib.ER_状態.支台歯:
                            g.DrawString(str, font, Brushes.Black, strX, strY, stringFormat);
                            g.DrawEllipse(kigouPen, pos.X + padding, pos.Y + padding, fontSize.Height - padding * 2f, fontSize.Height - padding * 2f);
                            break;
                        case OmoEReceLib.ER_状態.欠損歯:
                            g.DrawString(str, font, Brushes.Black, strX, strY, stringFormat);
                            g.DrawLine(kigouPen, pos.X, pos.Y, pos.X + fontSize.Width, pos.Y + fontSize.Height);
                            g.DrawLine(kigouPen, pos.X, pos.Y + fontSize.Height, pos.X + fontSize.Width, pos.Y);
                            break;

                        case OmoEReceLib.ER_状態.部近心隙:
                            isZenkaku = false;

                            float x2 = pos.X + fontSize.Width * 0.15f;
                            float tpX = x2 + fontSize.Width * 0.5f;
                            float tpY = pos.Y + fontSize.Height * 0.25f;
                            float btm = pos.Y + fontSize.Height * 0.8f;
                            g.DrawLine(kigouPen, tpX, tpY, x2 + fontSize.Width - padding, btm);
                            g.DrawLine(kigouPen, x2 + fontSize.Width - padding, btm, x2 + padding, btm);
                            g.DrawLine(kigouPen, x2 + padding, btm, tpX, tpY);
                            break;

                        // UNDONE 他の歯状態の表示

                        default:
                            Console.WriteLine("未実装：他の歯の状態の表示");

                            // EReceLib.ER_状態.現存歯
                            g.DrawString(str, font, Brushes.Black, strX, strY, stringFormat);

                            break;
                    }


                    prevFontW = (isZenkaku)
                                    ? fontSize.Width * 0.7f // 全角の場合 余計な余白分を詰める
                                    : fontSize.Width;
                }
            }
        }

        /// <summary>
        /// 全角・半角判断用エンコード
        /// </summary>
        private static Encoding _sjisEncoding = Encoding.GetEncoding("Shift_JIS");
    }
}
