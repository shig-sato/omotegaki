using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;

namespace OmoOmotegaki
{
    public sealed class ShinryouPrintDesign : IDisposable
    {
        private const int SHISHIKI_WIDTH = 200;
        private const int SHISHIKI_HEIGHT = 50;


        // Property

        public PrintDocument PrintDocument
        {
            get;
            private set;
        }


        private readonly KarteId _karteId;
        private readonly KarteData _karteData;
        private readonly SinryouData[] _shinryouData;
        private DrawParams _drawParams;
        private DrawState _drawState;


        // Constructor

        public ShinryouPrintDesign(KarteId karteId, KarteData karteData, SinryouData[] shinryouData)
        {
            _karteId = karteId;
            _karteData = karteData;
            _shinryouData = shinryouData;

            this.PrintDocument = new PrintDocument();
            this.PrintDocument.PrintPage += this.PrintDocument_PrintPage;
        }

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
                _drawState = new DrawState(this.Draw());
            }
            _drawState.PageNumber++;
            _drawState.Y = _drawParams.Bounds.Y;

            // ヘッダー (カルテ番号・氏名・ページ番号)
            {
                string header = _karteId + " " + _karteData.Get氏名(false) + " （ページ: " + _drawState.PageNumber + "）";
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
            if (this.PrintDocument != null)
            {
                this.PrintDocument.Dispose();
                this.PrintDocument = null;
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <returns>次のページに続く場合はtrue</returns>
        private IEnumerator<string> Draw()
        {
            var sb = new StringBuilder();

            // 診療データ
            DateTime prevDate = DateTime.MinValue;
            foreach (SinryouData shinryou in _shinryouData)
            {
                _drawState.X = _drawParams.Bounds.X;
                _drawState.Y += _drawParams.TextHeight;


                // 日付
                if (prevDate != shinryou.診療日)
                {
                    prevDate = shinryou.診療日;

                    _drawState.Y += _drawParams.TextHeight * 0.3f;

                    _drawParams.DrawSeparateLine(_drawState.Y);

                    _drawState.Y += _drawParams.TextHeight;

                    sb.Append("初診日 ").Append(shinryou.診療日.ToLongDateString());
                    _drawParams.DrawString(sb, _drawState, true);
                    sb.Clear();
                    _drawState.Y += _drawParams.TextHeight;
                    if (_drawParams.Bounds.Bottom <= _drawState.Y)
                    {
                        yield return "日付";
                    }

                    _drawState.Y += _drawParams.TextHeight;
                }

                // 歯式
                // 病名
                {
                    const int BYOUMEI_LEFT_MARGIN = 3;

                    OmoSeitokuEreceipt.Drawing.ERObjectDrawer.DrawSisikiFixed(
                        _drawParams.G, Pens.Black,
                        shinryou.歯式, shinryou.病名,
                        new RectangleF(_drawState.X, _drawState.Y, SHISHIKI_WIDTH, SHISHIKI_HEIGHT),
                        _drawParams.Font, true
                        );

                    if (_drawParams.Bounds.Bottom <= (_drawState.Y + SHISHIKI_HEIGHT))
                    {
                        yield return "歯式";
                    }

                    _drawState.X += SHISHIKI_WIDTH + BYOUMEI_LEFT_MARGIN;
                    _drawState.Y += _drawParams.TextHeight;

                    sb.Append("  病名: ");
                    _drawParams.DrawString(sb, _drawState);
                    _drawState.X += _drawParams.MeasureString(sb).Width;
                    sb.Clear();

                    float blockWidth = _drawParams.Width - (_drawState.X - _drawParams.X);
                    string[] textData = shinryou.病名;
                    using (var drawBlock = this.DrawBlock(textData, blockWidth, sb))
                    {
                        while (drawBlock.MoveNext())
                        {
                            yield return "病名";
                        }
                    }
                    sb.Clear();

                    _drawState.X = _drawParams.Bounds.X;
                    _drawState.Y += SHISHIKI_HEIGHT - _drawParams.TextHeight;
                }

                // 処置
                {
                    sb.Append("  処置: ");
                    _drawParams.DrawString(sb, _drawState);
                    _drawState.X += _drawParams.MeasureString(sb).Width;
                    sb.Clear();

                    float blockWidth = _drawParams.Width - (_drawState.X - _drawParams.X);
                    string[] textData = (
                            from syo in shinryou.処置
                            where !syo.IsSystemSyochi
                            //orderby syo.SyochiId
                            select syo.Name + " " + syo.Kaisuu + "回"
                            ).ToArray();
                    using (var drawBlock = this.DrawBlock(textData, blockWidth, sb))
                    {
                        while (drawBlock.MoveNext())
                        {
                            yield return "処置";
                        }
                    }
                    sb.Clear();

                    _drawState.X = _drawParams.Bounds.X;
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

            public int X { get { return this.Bounds.X; } }
            public int Y { get { return this.Bounds.Y; } }
            public int Width { get { return this.Bounds.Width; } }
            public int Height { get { return this.Bounds.Height; } }


            public DrawParams(Graphics g, Rectangle bounds, FontFamily fontFamily, float fontSizeEm, Color brushColor)
            {
                this.G = g;
                this.Bounds = bounds;
                this.Font = new Font(fontFamily, fontSizeEm);
                this.BoldFont = new Font(fontFamily, fontSizeEm, FontStyle.Bold);
                this.Brush = new SolidBrush(brushColor);
                this.TextHeight = this.G.MeasureString("M", this.Font).Height;
            }


            // Method

            public void Dispose()
            {
                this.G = null;

                try { this.Font.Dispose(); }
                catch (Exception) { }
                this.Font = null;

                try { this.BoldFont.Dispose(); }
                catch (Exception) { }
                this.BoldFont = null;

                try { this.Brush.Dispose(); }
                catch (Exception) { }
                this.Brush = null;
            }
            public void DrawSeparateLine(float y)
            {
                this.G.DrawLine(Pens.Black, this.Bounds.X, y, this.Bounds.Right, y);
            }
            public void DrawString(string text, DrawState state, bool bold = false)
            {
                this.G.DrawString(
                    text,
                    (bold ? this.BoldFont : this.Font),
                    this.Brush, state.X, state.Y);
            }
            public void DrawString(object obj, DrawState state, bool bold = false)
            {
                this.G.DrawString(
                    obj.ToString(),
                    (bold ? this.BoldFont : this.Font),
                    this.Brush, state.X, state.Y);
            }
            public void DrawImage(Image image, DrawState state)
            {
                this.G.DrawImage(image, state.X, state.Y);
            }
            public SizeF MeasureString(string text)
            {
                return this.G.MeasureString(text, this.Font);
            }
            public SizeF MeasureString(object obj)
            {
                return this.G.MeasureString(obj.ToString(), this.Font);
            }
        }
    }
}
