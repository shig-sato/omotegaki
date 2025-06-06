using OmoEReceLib.ERObjects;
using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;

namespace OmoOmotegaki
{
    public sealed class HaRirekiPrintingParameter
    {
        public KarteId KarteId { get; }
        public KarteData KarteData { get; }
        public IReadOnlyList<SinryouData> ShinryouDataList { get; }

        public HaRirekiPrintingParameter(KarteId karteId, KarteData karteData, IReadOnlyList<SinryouData> shinryouDataList)
        {
            KarteId = karteId;
            KarteData = karteData;
            ShinryouDataList = shinryouDataList;
        }
    }

    public sealed class HaRirekiPrintDesign : IDisposable
    {
        private readonly IEnumerator<HaRirekiPrintingParameter> _current;

        private DrawParams _drawParams;
        private DrawState _drawState;

        #region Constructor

        public HaRirekiPrintDesign(HaRirekiPrintingParameter parameter)
            : this(new[] { parameter })
        {
        }

        public HaRirekiPrintDesign(IEnumerable<HaRirekiPrintingParameter> parameters)
        {
            _current = parameters.GetEnumerator();

            PrintDocument = new PrintDocument();
            PrintDocument.PrintPage += PrintDocument_PrintPage;
        }

        #endregion

        public PrintDocument PrintDocument { get; }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            _drawParams = new DrawParams(
                g: e.Graphics,
                bounds: e.MarginBounds,
                fontFamily: SystemFonts.DefaultFont.FontFamily,
                fontSizeEm: SystemFonts.DefaultFont.Size,
                brushColor: Color.Black
                );
            _drawParams.G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;

            if (_drawState == null)
            {
                _drawState = new DrawState(Draw());
                _current.MoveNext();
            }

            _drawState.PageNumber++;
            _drawState.Y = _drawParams.Bounds.Y;

            // ヘッダー (カルテ番号・氏名・ページ番号)
            {
                string header = _current.Current.KarteId + " " +
                    _current.Current.KarteData.Get氏名(false) +
                    " （ページ: " + _drawState.PageNumber + "）";
                SizeF size = _drawParams.MeasureString(header);

                _drawState.X = _drawParams.Bounds.Right - size.Width;
                _drawParams.DrawString(header, _drawState);

                _drawState.X = _drawParams.Bounds.X;
                _drawState.Y += _drawParams.TextHeight;
            }

            e.HasMorePages = _drawState.MoveNext();

            if (!e.HasMorePages)
            {
                // 最終フッター
                _drawState.Y += _drawParams.TextHeight;
                _drawParams.DrawSeparateLine(_drawState.Y);
                _drawState.Y += _drawParams.TextHeight;
                string footer = " （総ページ数: " + _drawState.PageNumber + "）";
                SizeF size = _drawParams.MeasureString(footer);
                _drawState.X = _drawParams.Bounds.Right - size.Width;
                _drawParams.DrawString(footer, _drawState);


                _drawState.Dispose();
                _drawState = null;
            }
        }


        // Method

        public void Dispose()
        {
            if (PrintDocument != null)
            {
                PrintDocument.PrintPage -= PrintDocument_PrintPage;
                PrintDocument.Dispose();
            }
            if (_drawParams != null)
            {
                _drawParams.Dispose();
                _drawParams = null;
            }
            if (_drawState != null)
            {
                _drawState.Dispose();
                _drawState = null;
            }
        }

        private IEnumerator<string> Draw()
        {
            while (true)
            {
                // 歯別に診療データを辞書登録。
                var dict = new Dictionary<ER歯式単位, List<SinryouData>>();

                foreach (SinryouData shinryou in _current.Current.ShinryouDataList)
                {
                    foreach (ER歯式単位 ha in shinryou.歯式)
                    {
                        List<SinryouData> list;
                        if (!dict.TryGetValue(ha, out list))
                        {
                            list = new List<SinryouData>();
                            dict.Add(ha, list);
                        }
                        list.Add(shinryou);
                    }
                }

                // 印刷開始
                var sb = new StringBuilder();
                foreach (var kv in dict)
                {
                    const float INDENT_1 = 8;

                    ER歯式単位 ha = kv.Key;
                    IEnumerable<SinryouData> shinryouList = kv.Value;

                    _drawState.X = _drawParams.Bounds.X;
                    _drawState.Y += _drawParams.TextHeight;

                    _drawParams.DrawString("◆ " + ha.歯席, _drawState, true);
                    _drawState.X += INDENT_1;

                    foreach (SinryouData shinryou in shinryouList)
                    {
                        const float INDENT_2 = 24;

                        _drawState.Y += _drawParams.TextHeight;

                        _drawParams.DrawString(
                            shinryou.診療日.ToShortDateString() + "   " + string.Join(" ", shinryou.病名),
                            _drawState);
                        _drawState.Y += _drawParams.TextHeight * 1.3f;

                        _drawState.X += INDENT_2;

                        if (shinryou.歯式 != null)
                        {
                            using (var bmp = new Bitmap(Controls.ShishikiImageSourceConverter.ImageWidth, Controls.ShishikiImageSourceConverter.ImageHeight))
                            {
                                Controls.ShishikiImageSourceConverter.DrawShishiki(bmp, shinryou.歯式);
                                _drawParams.DrawImage(bmp, _drawState);
                            }
                            _drawState.Y += Controls.ShishikiImageSourceConverter.ImageHeight;
                            _drawState.Y += _drawParams.TextHeight;
                        }

                        if (_drawParams.Bounds.Bottom <= _drawState.Y)
                        {
                            _drawState.Y = _drawParams.Bounds.Y;
                            yield return DateTime.Now.Ticks.ToString();
                        }

                        // 処置
                        {
                            float blockWidth = _drawParams.Width - (_drawState.X - _drawParams.X);
                            string[] textData = (
                                    from syo in shinryou.処置
                                    where !syo.IsSystemSyochi
                                    //orderby syo.SyochiId
                                    select syo.Name + " " + syo.Kaisuu + "回"
                                    ).ToArray();
                            using (var drawBlock = DrawBlock(textData, blockWidth, sb))
                            {
                                while (drawBlock.MoveNext())
                                {
                                    yield return "処置";
                                }
                            }
                            sb.Clear();
                        }

                        _drawState.X -= INDENT_2;
                    }
                }

                if (_current.MoveNext())
                {
                    // 改ページ

                    _drawState.Y += _drawParams.TextHeight;
                    _drawParams.DrawSeparateLine(_drawState.Y);
                    _drawState.Y += _drawParams.TextHeight;

                    yield return DateTime.Now.Ticks.ToString();
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>途中で用紙をオーバーした場合は再開用インデックスを返す。完了時は-1。</returns>
        public IEnumerator<int> DrawBlock(string[] data, float blockWidth, StringBuilder sb)
        {
            DrawState state = _drawState;
            DrawParams param = _drawParams;
            float x = 0;
            float y = param.TextHeight;

            for (var i = 0; i < data.Length; i++)
            {
                string text = data[i] + "   ";
                var textSize = param.MeasureString(text);
                x += textSize.Width;
                // ブロックの幅を超えたら改行
                if (blockWidth <= x)
                {
                    x = textSize.Width;
                    y += textSize.Height;
                    // 用紙の高さを超えたら処理を一時抜ける
                    if (param.Bounds.Bottom <= y)
                    {
                        param.DrawString(sb, state);
                        sb.Clear();

                        yield return i;

                        y = 0;
                    }
                    else
                    {
                        sb.AppendLine();
                    }
                }
                sb.Append(text);
            }
            param.DrawString(sb, state);
            state.Y += y;
        }


        // Class

        public sealed class DrawState : IDisposable
        {
            private IEnumerator<string> _enumerator;


            public int PageNumber;
            public float X;
            public float Y;


            public DrawState(IEnumerator<string> enumerator)
            {
                _enumerator = enumerator;
            }

            public void Dispose()
            {
                if (_enumerator != null)
                {
                    _enumerator.Dispose();
                    _enumerator = null;
                }
            }

            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }
        }

        private sealed class DrawParams : IDisposable
        {
            public Graphics G;
            public Rectangle Bounds;
            public Font Font;
            public Font BoldFont;
            public Brush Brush;
            public float TextHeight;

            public int X { get { return Bounds.X; } }
            public int Y { get { return Bounds.Y; } }
            public int Width { get { return Bounds.Width; } }
            public int Height { get { return Bounds.Height; } }


            public DrawParams(Graphics g, Rectangle bounds, FontFamily fontFamily, float fontSizeEm, Color brushColor)
            {
                G = g;
                Bounds = bounds;
                Font = new Font(fontFamily, fontSizeEm);
                BoldFont = new Font(fontFamily, fontSizeEm, FontStyle.Bold);
                Brush = new SolidBrush(brushColor);
                TextHeight = G.MeasureString("M", Font).Height;
            }


            // Method

            public void Dispose()
            {
                G = null;

                try { Font.Dispose(); }
                catch (Exception) { }
                Font = null;

                try { BoldFont.Dispose(); }
                catch (Exception) { }
                BoldFont = null;

                try { Brush.Dispose(); }
                catch (Exception) { }
                Brush = null;
            }
            public void DrawSeparateLine(float y)
            {
                G.DrawLine(Pens.Black, Bounds.X, y, Bounds.Right, y);
            }
            public void DrawString(string text, DrawState state, bool bold = false)
            {
                G.DrawString(
                    text,
                    (bold ? BoldFont : Font),
                    Brush, state.X, state.Y);
            }
            public SizeF DrawStringMeasure(string text, DrawState state, bool bold = false)
            {
                DrawString(text, state, bold);
                return MeasureString(text);
            }
            public void DrawString(object obj, DrawState state, bool bold = false)
            {
                G.DrawString(
                    obj.ToString(),
                    (bold ? BoldFont : Font),
                    Brush, state.X, state.Y);
            }
            public void DrawImage(Image image, DrawState state)
            {
                G.DrawImage(image, state.X, state.Y);
            }
            public SizeF MeasureString(string text)
            {
                return G.MeasureString(text, Font);
            }
            public SizeF MeasureString(object obj)
            {
                return G.MeasureString(obj.ToString(), Font);
            }
        }
    }
}
