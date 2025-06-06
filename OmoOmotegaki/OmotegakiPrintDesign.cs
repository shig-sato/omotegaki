using Microsoft.International.JapaneseTextAlignment;
using OmoSeitoku;
using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;

namespace OmoOmotegaki
{
    public static class OmotegakiPrintDesign
    {
        // 描画設定 //
        private const string FONT_FAMILY = "MS Mincho";
        private const float LINE_WIDTH = 0.2f;
        private const float 破線のダッシュの長さ = 5f;
        private const float 破線のダッシュの空白の長さ = 2f;

        /// <summary>
        /// グラフィックオブジェクトに描き出す
        /// </summary>
        /// <param name="g"></param>
        public static void DrawOmotegaki(KarteData data, Graphics g, PageSettings pageSettings, SinryouData[] items)
        {
            g.PageUnit = GraphicsUnit.Millimeter;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            PrintUtil.SetScaleB5JIS(g, pageSettings);

            Color writeColor = Color.Black;

            Brush brush = (Brush)Brushes.Red.Clone();
            Brush writeBrush = new SolidBrush(writeColor);

            Pen linePen = new Pen(brush, LINE_WIDTH);
            Pen hutoPen = new Pen(brush, LINE_WIDTH * 1.75f);
            Pen boldPen = new Pen(brush, LINE_WIDTH * 3.5f);
            Pen dashPen = new Pen(brush, LINE_WIDTH);
            dashPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            dashPen.DashPattern = new float[] { 破線のダッシュの長さ, 破線のダッシュの空白の長さ };

            Font labelFontSmall = new Font(FONT_FAMILY, 7f);
            Font labelFont = new Font(FONT_FAMILY, 9f);
            Font labelFont2 = new Font(FONT_FAMILY, 10.5f);
            Font dateFont = new Font(FONT_FAMILY, 7.5f);

            StringFormat vertical = new StringFormat(StringFormatFlags.DirectionVertical);

            // 文字均等割付
            TextAlignmentStyleInfo kintou = new TextAlignmentStyleInfo();
            kintou.Style = TextAlignmentStyle.Justify;

            // 文字中央寄せ
            TextAlignmentStyleInfo center = new TextAlignmentStyleInfo();
            center.Style = TextAlignmentStyle.Center;

            try
            {
                List<PointF> points = new List<PointF>();


                using (Font titleFont = new Font(FONT_FAMILY, 15.5f))
                {
                    Utility.DrawJapaneseString(
                        g, "歯科診療録", titleFont, linePen.Color, new Rectangle(58, 8, 66, 7), kintou);
                }


                //外枠
                points.Clear();
                points.Add(new PointF(9f, 19f));
                points.Add(new PointF(90.1f, 19f));
                points.Add(new PointF(90.1f, 29f));
                points.Add(new PointF(83.1f, 29f));
                points.Add(new PointF(83.1f, 39f));
                points.Add(new PointF(9f, 39f));
                g.DrawPolygon(hutoPen, points.ToArray());


                Utility.DrawJapaneseString(
                    g, "公費負担者番号", labelFont, linePen.Color, new Rectangle(10, 22, 25, 3), kintou);
                g.DrawLine(linePen, 37.8f, 19f, 37.8f, 29f);
                g.DrawLine(dashPen, 44f, 19.5f, 44f, 29f);
                g.DrawLine(boldPen, 50.5f, 19f, 50.5f, 29f);
                g.DrawLine(dashPen, 57f, 19.5f, 57f, 29f);
                g.DrawRectangle(boldPen, 63.2f, 19f, 19.5f, 10f);
                g.DrawLine(dashPen, 70f, 19.5f, 70f, 29f);
                g.DrawLine(dashPen, 76.5f, 19.5f, 76.5f, 29f);
                if (0 < data.公費負担者番号)
                {
                    using (Font font = new Font(FONT_FAMILY, 18f))
                    {
                        string s = data.公費負担者番号.ToString().PadLeft(8, '0');
                        Utility.DrawJapaneseString(
                            g, s, font, writeColor, new Rectangle(38, 21, 49, 10), kintou);
                    }
                }


                g.DrawLine(linePen, 9f, 29f, 74f, 29f);


                Utility.DrawJapaneseString(
                    g, "公費負担医療", labelFont, linePen.Color, new Rectangle(10, 30, 25, 3), kintou);
                Utility.DrawJapaneseString(
                    g, "の受給者番号", labelFont, linePen.Color, new Rectangle(10, 35, 25, 3), kintou);
                g.DrawLine(linePen, 37.8f, 29f, 37.8f, 39f);
                g.DrawLine(dashPen, 44f, 29.5f, 44f, 39f);
                g.DrawLine(dashPen, 50.5f, 29.5f, 50.5f, 39f);
                g.DrawLine(boldPen, 57f, 29f, 57f, 39f);
                g.DrawLine(dashPen, 63.2f, 29.5f, 63.2f, 39f);
                g.DrawLine(dashPen, 70f, 29.5f, 70f, 39f);
                g.DrawLine(boldPen, 76.5f, 29f, 76.5f, 39f);
                if (0 < data.公費受給者番号)
                {
                    using (Font font = new Font(FONT_FAMILY, 18f))
                    {
                        string s = data.公費受給者番号.ToString().PadLeft(7, '0');
                        Utility.DrawJapaneseString(
                            g, s, font, writeColor, new Rectangle(38, 31, 42, 10), kintou);
                    }
                }

                Utility.DrawJapaneseString(
                    g, "保険者番号", labelFont, linePen.Color, new Rectangle(95, 22, 22, 3), kintou);
                g.DrawRectangle(hutoPen, 93.2f, 19f, 78.8f, 10f); //外枠
                g.DrawLine(linePen, 120.2f, 19f, 120.2f, 29f);
                g.DrawLine(dashPen, 126.8f, 19.5f, 126.8f, 29f);
                g.DrawLine(boldPen, 133.3f, 19f, 133.3f, 29f);
                g.DrawLine(dashPen, 140f, 19.5f, 140f, 29f);
                g.DrawRectangle(boldPen, 146.5f, 19f, 19.5f, 10f);
                g.DrawLine(dashPen, 152.9f, 19.5f, 152.9f, 29f);
                g.DrawLine(dashPen, 159f, 19.5f, 159f, 29f);
                if (0 < data.保険者番号)
                {
                    using (Font font = new Font(FONT_FAMILY, 18f))
                    {
                        string s = data.保険者番号.ToString().PadLeft(8, '0');
                        Utility.DrawJapaneseString(
                            g, s, font, writeColor, new Rectangle(121, 21, 49, 10), kintou);
                    }
                }

                //外枠
                points.Clear();
                points.Add(new PointF(9f, 40f));
                points.Add(new PointF(93.2f, 40f));
                points.Add(new PointF(93.2f, 30f));
                points.Add(new PointF(172f, 30f));
                points.Add(new PointF(172f, 89f));
                points.Add(new PointF(9f, 89f));
                g.DrawPolygon(hutoPen, points.ToArray());

                //区切り
                //  縦
                g.DrawLine(linePen, 17.1f, 40f, 17.1f, 89f);
                g.DrawLine(linePen, 37.8f, 40f, 37.8f, 89f);
                g.DrawLine(linePen, 93.2f, 40f, 93.2f, 89f);
                g.DrawLine(linePen, 81.3f, 52f, 81.3f, 64.5f); // 生年月日 | 性別
                g.DrawLine(linePen, 56.2f, 76.6f, 56.2f, 89f); // (職業入力欄) | 被保険者との続柄
                g.DrawLine(linePen, 75f, 76.6f, 75f, 89f);     // 被保険者との続柄 | (入力欄)
                //  横
                g.DrawLine(linePen, 17.1f, 52.0f, 93.2f, 52.0f);
                g.DrawLine(linePen, 17.1f, 64.5f, 93.2f, 64.5f);
                g.DrawLine(linePen, 17.1f, 76.6f, 93.2f, 76.6f);


                //受診者
                g.DrawString("受　 　診 　　者", labelFont2, brush, 10.5f, 49f, vertical);


                Utility.DrawJapaneseString(
                    g, "氏名", labelFont2, linePen.Color, new Rectangle(18, 44, 17, 3), kintou);
                if (!string.IsNullOrEmpty(data.氏名))
                {
                    using (Font font = new Font(FONT_FAMILY, 15.5f))
                    {
                        Utility.DrawJapaneseString(
                            g, data.氏名, font, writeColor, new Rectangle(38, 44, 53, 10), kintou);
                    }
                }

                Utility.DrawJapaneseString(
                    g, "生年月日", labelFont2, linePen.Color, new Rectangle(18, 56, 17, 3), kintou);
                if (data.生年月日.HasValue)
                {
                    string s = OmoEReceLib.ERDateTime.Format(data.生年月日.Value, "g yy年 M月 d日生");
                    Utility.DrawJapaneseString(
                        g, s, labelFont2, writeColor, new Rectangle(38, 57, 42, 10), kintou);

                    // 性別
                    if (data.性別.HasValue)
                        g.DrawString(data.性別.Value.ToString(), labelFont2, writeBrush, 85f, 57f);
                }

                Utility.DrawJapaneseString(
                    g, "住所", labelFont2, linePen.Color, new Rectangle(18, 69, 17, 3), kintou);
                if (!string.IsNullOrEmpty(data.住所))
                {
                    g.DrawString(data.住所, labelFont, writeBrush, 38f, 67f);
                }
                g.DrawString("電話", labelFontSmall, brush, 40f, 73f);
                if (!string.IsNullOrEmpty(data.電話番号))
                {
                    g.DrawString(data.電話番号, labelFontSmall, writeBrush, 48f, 73f);
                }

                Utility.DrawJapaneseString(
                    g, "職業", labelFont2, linePen.Color, new Rectangle(18, 81, 17, 3), kintou);
                if (!string.IsNullOrEmpty(data.職業))
                {
                    Utility.DrawJapaneseString(
                        g, data.職業,
                        labelFont, writeColor, new Rectangle(38, 81, 16, 10), kintou);
                }


                g.TranslateTransform(0.3f, 0f);

                Utility.DrawJapaneseString(
                    g, "被保険者",
                    labelFont2, linePen.Color, new Rectangle(56, 79, 17, 4), kintou);
                Utility.DrawJapaneseString(
                    g, "との続柄",
                    labelFont2, linePen.Color, new Rectangle(56, 83, 17, 4), kintou);

                g.ResetTransform();
                PrintUtil.SetScaleB5JIS(g, pageSettings);



                //区切り
                //  縦
                g.DrawLine(linePen, 103.2f, 30f, 103.2f, 43.2f); // 被保険者手帳 | 記号・番号
                g.DrawLine(linePen, 118.2f, 30f, 118.2f, 89f);   // 記号・番号 | (記号番号入力欄)
                g.DrawLine(linePen, 103.2f, 59f, 103.2f, 89f); // 事業所 | 所在地
                //  横
                g.DrawLine(linePen, 103.2f, 38.2f, 118.2f, 38.2f); // 記号・番号 | 有効期限
                g.DrawLine(linePen, 93.2f, 43.2f, 172f, 43.2f);    // 有効期限 | 被保険者氏名
                g.DrawLine(linePen, 93.2f, 54f, 172f, 54f); // 被保険者氏名 | 資格取得
                g.DrawLine(linePen, 93.2f, 59f, 172f, 59f); // 資格取得 | 事業所
                g.DrawLine(linePen, 103.2f, 69f, 172f, 69f); //
                g.DrawLine(linePen, 93.2f, 74f, 172f, 74f); // 事業所 | 保険者
                g.DrawLine(linePen, 103.2f, 84f, 172f, 84f); //

                // 太い囲み
                g.DrawRectangle(boldPen, 118.2f, 30f, 54f, 8.5f);

                // 記号・番号 の区切り点
                g.FillEllipse(brush, 144.2f, 34f, 1.5f, 1.5f);

                using (Font font = new Font(FONT_FAMILY, 6f))
                {
                    g.DrawString("被保険者証" + Environment.NewLine + "被保険者手帳",
                                 font, brush, 95f, 30f, vertical);
                }


                g.TranslateTransform(0.8f, 0f);

                Utility.DrawJapaneseString(
                    g, "記号・番号", labelFontSmall, linePen.Color, new Rectangle(103, 33, 13, 3), kintou);

                Utility.DrawJapaneseString(
                    g, "有効期限", labelFontSmall, linePen.Color, new Rectangle(103, 39, 13, 3), kintou);

                g.ResetTransform();
                PrintUtil.SetScaleB5JIS(g, pageSettings);


                if (!string.IsNullOrEmpty(data.被保険者証_記号))
                {
                    using (Font font = new Font(FONT_FAMILY, 10f))
                    {
                        Utility.DrawJapaneseString(
                            g, data.被保険者証_記号,
                            font, writeColor, new Rectangle(120, 33, 25, 7), center);
                    }
                }

                if (!string.IsNullOrEmpty(data.被保険者証_番号))
                {
                    using (Font font = new Font(FONT_FAMILY, 12f))
                    {
                        Utility.DrawJapaneseString(
                            g, data.被保険者証_番号,
                            font, writeColor, new Rectangle(147, 33, 23, 7), center);
                    }
                }


                if (data.保険有効期限.HasValue)
                {
                    using (Font font = new Font(FONT_FAMILY, 9f))
                    {
                        DateTime dt = data.保険有効期限.Value;
                        string s = OmoEReceLib.ERDateTime.Format(dt, "gg yy年　M月　d日");
                        g.DrawString(s, font, writeBrush, 131f, 39.5f);
                    }
                }

                Utility.DrawJapaneseString(
                    g, "被保険者氏名", labelFont, linePen.Color, new Rectangle(95, 47, 22, 4), kintou);



                // 現症図
                //g.DrawRectangle(hutoPen, 96.5f, 90f, 75.5f, 92f); //外枠
                g.DrawEllipse(Pens.Beige, new RectangleF(96.5f, 90f, 75.5f, 92f));


                // 部位・傷病名 

                g.DrawRectangle(hutoPen, 9f, 90f, 163f, 121.3f); //外枠


                //区切り
                //  縦
                g.DrawLine(linePen, 29.8f, 90f, 29.8f, 211.3f);
                g.DrawLine(linePen, 65.7f, 90f, 65.7f, 211.3f);
                g.DrawLine(linePen, 70.1f, 90f, 70.1f, 211.3f);
                g.DrawLine(linePen, 80.1f, 90f, 80.1f, 211.3f);
                g.DrawLine(linePen, 90.1f, 90f, 90.1f, 211.3f);
                g.DrawLine(linePen, 96.1f, 90f, 96.1f, 211.3f);
                //  横
                float y = 98f;
                for (int i = 0; i < 12; i++)
                {
                    g.DrawLine(linePen, 9f, y, 96.1f, y);
                    y += 9.5f;

                }
                y = 98f;

                // 同一歯式でグルーピング
                var itemCollection = items.Where(p => (p.歯式 != null)).OrderBy(p => p.診療日);

                foreach (var shinryou in itemCollection)
                {
                    // 部位・傷病名の内容を印字
                    {
                        OmoSeitokuEreceipt.Drawing.ERObjectDrawer.DrawSisikiFixed(
                            g, linePen, shinryou.歯式, shinryou.病名,
                            new RectangleF(9f + 0.4f, y + 0.4f, 20.2f, 8.6f),
                            labelFontSmall, true);

                        string s;

                        s = string.Join(Environment.NewLine, shinryou.病名);
                        g.DrawString(s, labelFont, writeBrush, 29.8f + 2f, y + 2f);


                        // 開始日
                        DateTime dt開始日 = items.Min(p => p.診療日);
                        if (dt開始日 != DateTime.MinValue)
                        {
                            try
                            {
                                s = OmoEReceLib.ERDateTime.GetEraYear(dt開始日, true).Replace(" ", string.Empty) + dt開始日.ToString("年\n  M月\n  d日");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(typeof(OmotegakiPrintDesign).Name + ": GetEraYear: " + ex.Message);
                                s = dt開始日.ToString("y年\n  M月\n  d日");
                            }
                            g.DrawString(s, dateFont, writeBrush, 70.1f, y + 1f);
                        }

                        // 終了日
                        DateTime dt終了日 = (shinryou.処置.Any(p => p.IsFinish)) ?
                                shinryou.診療日 :
                                DateTime.MinValue;

                        if ((30 * 3) < (dt終了日 - dt開始日).TotalDays)
                        {
                            g.DrawString("中止", dateFont, writeBrush, 80.1f, y + 1f);
                        }
                        else
                        {
                            if (dt終了日 != DateTime.MinValue)
                            {
                                try
                                {
                                    s = OmoEReceLib.ERDateTime.GetEraYear(dt終了日, true).Replace(" ", string.Empty) + dt終了日.ToString("年\n  M月\n  d日");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(typeof(OmotegakiPrintDesign).Name + ": GetEraYear: " + ex.Message);
                                    s = dt終了日.ToString("y年\n  M月\n  d日");
                                }
                                g.DrawString(s, dateFont, writeBrush, 80.1f, y + 1f);
                            }
                        }
                    }

                    y += 9.5f;
                }


                Utility.DrawJapaneseString(
                    g, "部位", labelFont2, linePen.Color, new Rectangle(11, 92, 15, 4), kintou);

                Utility.DrawJapaneseString(
                    g, "傷病名", labelFont2, linePen.Color, new Rectangle(36, 92, 23, 4), kintou);

                g.DrawString("職務", labelFont2, brush, 65.5f, 90.5f, vertical);

                g.DrawString("開始", labelFont2, brush, 72.0f, 90.5f, vertical);

                g.DrawString("終了", labelFont2, brush, 82.0f, 90.5f, vertical);

                g.DrawString("転帰", labelFont2, brush, 90.3f, 90.5f, vertical);


                g.DrawRectangle(hutoPen, 9f, 212.5f, 163f, 13.4f); //外枠

                g.DrawRectangle(hutoPen, 9f, 227f, 163f, 13.4f); //外枠
            }
            finally
            {
                brush.Dispose();
                writeBrush.Dispose();

                linePen.Dispose();
                hutoPen.Dispose();
                boldPen.Dispose();
                dashPen.Dispose();

                labelFontSmall.Dispose();
                labelFont.Dispose();
                labelFont2.Dispose();
                dateFont.Dispose();

                vertical.Dispose();
            }
        }

    }



}