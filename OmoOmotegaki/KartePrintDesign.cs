using OmoSeitoku;
using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

namespace OmoOmotegaki
{
    public sealed class KartePrintDesign
    {
        // 描画設定 //
        private const string FONT_FAMILY = "MS Mincho";
        private const float LINE_WIDTH = 0.2f;
        private const float ROW_COUNT = 30f; // 行数

        public sealed class Settings
        {
            // UNDONE カルテ印刷 保険診療・未来院請求 これの判定できていない（プロパティ未使用）
            public SER_保険診療? Hoken;
            public SER_未来院請求? Miraiin;

            /// <summary>カルテ番号を表示</summary>
            public bool ShowKarteNumber;

            /// <summary>患者名を表示</summary>
            public bool ShowKarteName;

            /// <summary>カルテ出力対象期間を表示</summary>
            public bool ShowKarteKikan;

            /// <summary>病名を表示</summary>
            public bool ShowByoumei;

            /// <summary>診療録作成ソフトの処置名で印字する</summary>
            public bool SSSSyochiName;

            /// <summary>診療録作成ソフトのチェック印刷を印字する</summary>
            public bool SSSCheckPrint;


            public Settings()
            {
                this.Hoken = null;
                this.Miraiin = null;
                this.ShowKarteNumber = true;
                this.ShowKarteName = true;
                this.ShowKarteKikan = true;
                this.ShowByoumei = true;
                this.SSSSyochiName = true;
                this.SSSCheckPrint = false;
            }

            public Settings(Settings value)
                : this()
            {
                this.Hoken = value.Hoken;
                this.Miraiin = value.Miraiin;
                this.ShowKarteNumber = value.ShowKarteNumber;
                this.ShowKarteName = value.ShowKarteName;
                this.ShowKarteKikan = value.ShowKarteKikan;
                this.ShowByoumei = value.ShowByoumei;
                this.SSSSyochiName = value.SSSSyochiName;
                this.SSSCheckPrint = value.SSSCheckPrint;
            }
        }

        /// <summary>印字継続用データ</summary>
        private struct PrintStatus
        {
            public static readonly PrintStatus Empty = new PrintStatus();

            public int ItemsIndex;
            public int SyochiListIndex;
            public int SyochiIndex;
            public int RecordIndex;

            public int TotalTensuu;

            /// <summary>
            /// 次の印刷アイテムへ移動
            /// </summary>
            public void NextItem()
            {
                this.ItemsIndex++;
                this.SyochiListIndex = 0;
                this.SyochiIndex = 0;
                this.RecordIndex = 0;
            }

            public void NextSyochiList()
            {
                this.SyochiListIndex++;
                this.SyochiIndex = 0;
                this.RecordIndex = 0;
            }

            public void NextSyochi()
            {
                this.SyochiIndex++;
                this.RecordIndex = 0;
            }

            public void NextRecord()
            {
                this.RecordIndex++;
            }
        }

        private struct Column
        {
            public string Text;
            public RectangleF Bounds;

            public Column(string text, RectangleF bounds)
            {
                this.Text = text;
                this.Bounds = bounds;
            }
        }

        private sealed class PrintResources : IDisposable
        {
            public Brush Brush { get; private set; }
            public Pen LinePen { get; private set; }
            public Font LabelFont { get; private set; }
            public Font LabelFontSmall { get; private set; }


            public PrintResources()
            {
                this.Brush = new SolidBrush(Color.Black);
                this.LinePen = new Pen(this.Brush, LINE_WIDTH);
                this.LabelFont = new Font(FONT_FAMILY, 9f);
                this.LabelFontSmall = new Font(FONT_FAMILY, 7f);
            }

            public void Dispose()
            {
                if (this.Brush != null)
                {
                    this.Brush.Dispose();
                    this.Brush = null;
                }

                if (this.LinePen != null)
                {
                    this.LinePen.Dispose();
                    this.LinePen = null;
                }

                if (this.LabelFont != null)
                {
                    this.LabelFont.Dispose();
                    this.LabelFont = null;
                }

                if (this.LabelFontSmall != null)
                {
                    this.LabelFontSmall.Dispose();
                    this.LabelFontSmall = null;
                }
            }
        }




        private IList<KartePrintItem> _items;
        private Settings _settings;
        private int _pageCount;
        private int _lastOutputPageCount;
        private DateTime _prevDate;

        private CheckPrint _checkPrint;

        private PrintStatus _status;

        private PrintResources _resources;


        // 点数 右寄せ用 X位置キャッシュ
        private float[] _tensuuXCache = new[] { -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f };

        // 金額 右寄せ用 X位置キャッシュ
        private float[] _kingakuXCache = new[] { -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f, -1f };


        private RectangleF _tableBounds;
        private float _rowHeight;

        private Column _columnDate;
        private Column _columnBui;
        private Column _columnSyochi;
        private Column _columnTensuu;
        private Column _columnHutan;


        public KartePrintDesign()
        {
            _tableBounds = new RectangleF(9f, 14f, 172f - 9f, 228f);
            _rowHeight = _tableBounds.Height / ROW_COUNT;

            this.InitializeColumns();

            Reset();
        }


        public void Reset()
        {
            _lastOutputPageCount = _pageCount;

            _pageCount = 0;
            _prevDate = DateTime.MinValue;

            _status = PrintStatus.Empty;
        }

        public void Preview(IList<KartePrintItem> items, Settings settings, IWin32Window owner)
        {
            Reset();

            _items = items;
            _settings = settings;

            using (var doc = new PrintDocument())
            using (SinryouDataLoader.ConnectMaster())
            using (_resources = new PrintResources())
            using (var pp = new PrintPreviewDialog())
            {
                doc.PrintPage += new PrintPageEventHandler(pd_PrintKarte);

                _checkPrint = new CheckPrint();

                try
                {
                    pp.Document = doc;
                    pp.DesktopBounds = Screen.FromHandle(owner.Handle).WorkingArea;
                    pp.PrintPreviewControl.Zoom = 1.0;
                    pp.PrintPreviewControl.AutoZoom = false;
                    pp.ShowDialog(owner);
                }
                finally
                {
                    _checkPrint = null;
                }
            }

            if (_lastOutputPageCount == 0)
            {
                MessageBox.Show("出力できるデータがありません。");
            }

            Reset();
        }

        public void Print(IList<KartePrintItem> items, Settings settings, IWin32Window owner)
        {
            Reset();

            _items = items;
            _settings = settings;

            using (var doc = new PrintDocument())
            using (SinryouDataLoader.ConnectMaster())
            using (_resources = new PrintResources())
            using (var pp = new PrintDialog())
            {
                doc.PrintPage += new PrintPageEventHandler(pd_PrintKarte);

                _checkPrint = new CheckPrint();

                try
                {
                    pp.Document = doc;
                    if (pp.ShowDialog(owner) == DialogResult.OK)
                    {
                        doc.Print();
                    }
                }
                finally
                {
                    _checkPrint = null;
                }
            }

            if (_lastOutputPageCount == 0)
            {
                MessageBox.Show("出力できるデータがありません。");
            }

            Reset();
        }


        private void pd_PrintKarte(object sender, PrintPageEventArgs e)
        {
            PrintUtil.SetScaleB5JIS(e.Graphics, e.PageSettings);

            bool hasMorePages = this.Draw(e.Graphics);

            if (hasMorePages)
            {
                // 終端ページではない
                e.HasMorePages = true;
            }
            else
            {
                if (NextItem())
                {
                    // 終端ページではない （次のカルテへ移動）
                    e.HasMorePages = true;
                }
                else
                {
                    // 終端ページ
                    e.HasMorePages = false;

                    Reset();
                }
            }
        }

        /// <summary>
        /// 次の印刷アイテムに移動
        /// </summary>
        /// <returns>終端ではない場合 true</returns>
        private bool NextItem()
        {
            _status.NextItem();

            _prevDate = DateTime.MinValue;

            return _status.ItemsIndex < _items.Count;
        }


        private float GetTopTextLineY(Graphics g)
        {
            return _tableBounds.Y + (_rowHeight - g.MeasureString("M", _resources.LabelFont).Height) * 0.5f;
        }


        private void InitializeColumns()
        {
            _columnDate = new Column(
                "月日",
                new RectangleF(
                    _tableBounds.X,
                    _tableBounds.Y,
                    9.8f,
                    _rowHeight
                    )
                );

            // 部位
            _columnBui = new Column(
                "部　位",
                new RectangleF(
                    _columnDate.Bounds.Right,
                    _tableBounds.Y,
                    24.2f,
                    _rowHeight
                    )
                );

            // 療法・処置
            _columnSyochi = new Column(
                "療　　法　　・　　処　　置",
                new RectangleF(
                    _columnBui.Bounds.Right,
                    _tableBounds.Y,
                    _tableBounds.Right - 36f - _columnBui.Bounds.Right,
                    _rowHeight
                    )
                );

            // 点数
            _columnTensuu = new Column(
                "点 数",
                new RectangleF(
                    _columnSyochi.Bounds.Right,
                    _tableBounds.Y,
                    18f,
                    _rowHeight
                    )
                );

            // 負担金徴収額
            _columnHutan = new Column(
                "負 担 金\n徴 収 額",
                new RectangleF(
                    _columnTensuu.Bounds.Right,
                    _tableBounds.Y,
                    18f,
                    _rowHeight
                    )
                );
        }


        /// <summary>
        /// グラフィックオブジェクトに描き出す
        /// </summary>
        /// <param name="g"></param>
        /// <returns>次ページへ継続する場合は true</returns>
        private bool Draw(Graphics g)
        {
            g.PageUnit = GraphicsUnit.Millimeter;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;


            bool hasMorePage = false;

            KartePrintItem printItem;


            float currentTextY = this.GetTopTextLineY(g);


            // 罫線描画時のクリップ領域から削除する領域のパス
            var excludePath = new System.Drawing.Drawing2D.GraphicsPath();


            while (true)
            {
                printItem = _items[_status.ItemsIndex];

                // (カルテ 行データ)
                float baseY = _tableBounds.Top + _rowHeight;
                int rows = 0;
                bool errorRaised = false;
                float textPaddingTop = currentTextY - _tableBounds.Y;
                // 点数 右寄せ用
                float[] tensuuXCache = _tensuuXCache;
                int prevYear = -1;
                int syochiListLen = printItem.SyochiList.Length;

                for (; _status.SyochiListIndex < syochiListLen; _status.NextSyochiList())
                {
                    SinryouData syochiListItem = printItem.SyochiList[_status.SyochiListIndex];


                    SyochiData[] syochiList = syochiListItem.処置;
                    int syochiLen = (syochiList == null)
                                        ? 0
                                        : syochiList.Length;


                    for (; _status.SyochiIndex < syochiLen; _status.NextSyochi())
                    {
                        SinryouDataLoader.KarteRecord[] records;

                        try
                        {
                            SyochiData item = syochiList[_status.SyochiIndex];

                            if (_settings.SSSSyochiName)
                            {
                                // 診療録作成ソフトの処置名で印字

                                SinryouDataLoader.KarteRecord record;

                                if (SyochiData.Is特殊処置(item.SyochiId))
                                {
                                    continue;
                                }
                                else
                                {
                                    int tensuu = item.Tensuu * item.Kaisuu;

                                    if (item.Kaisuu == 1)
                                    {
                                        record = new SinryouDataLoader.KarteRecord(
                                                    item.Name,
                                                    tensuu.ToString(),
                                                    tensuu);
                                    }
                                    else
                                    {
                                        record = new SinryouDataLoader.KarteRecord(
                                                    item.Name + " (" + item.Kaisuu + "回)",
                                                    tensuu.ToString(),
                                                    tensuu);
                                    }
                                }

                                records = new[] { record };
                            }
                            else
                            {
                                // マスターから処置を取得して印字

                                records = SinryouDataLoader.GetSyochiERCode(
                                                item, syochiListItem.診療日);
                            }

                            errorRaised = false;
                        }
                        catch (IndexOutOfRangeException ex)
                        {
                            // エラーが連続した場合は今回のエラー行をスキップ
                            // ただし今回が最初の処置か歯式がある場合はエラー行を表示する
                            if (errorRaised && (_status.SyochiIndex != 0 || syochiListItem.歯式 == null))
                            {
                                records = new SinryouDataLoader.KarteRecord[0];
                            }
                            else
                            {
                                // エラー行を表示
                                errorRaised = true;

                                var record = new SinryouDataLoader.KarteRecord(
                                     "（エラー: " + ex.Message + "）", "", 0);

                                records = new[] { record };
                            }
                        }

                        // チェック印刷で使用する行数
                        int checkPrintLength = 0;

                        // 処置リスト先頭
                        if (_status.SyochiIndex == 0)
                        {
                            // 病名を処置リストの先頭に追加
                            if (_settings.ShowByoumei)
                            {
                                // レコードの一番目が空の場合はリストを増やさずに一番目に病名を設定する。

                                // 一番目が空ではないのでリストの項目を1つ増やす
                                if (!records[0].IsEmpty)
                                {
                                    // リストの項目を増やす
                                    int insertItemsCount = 1;
                                    var ar = new SinryouDataLoader.KarteRecord[records.Length + insertItemsCount];
                                    Array.Copy(records, 0, ar, insertItemsCount, records.Length);
                                    records = ar;
                                }

                                var byomei = string.Join(", ", syochiListItem.病名);
                                if (byomei.Length != 0) byomei = "（病名: " + byomei + "）";

                                records[0] = new SinryouDataLoader.KarteRecord(byomei, "", 0);
                            }

                            // チェック印刷
                            //   診療録作成ソフトのチェック印刷を印字 行の先頭に追加
                            //   最初の診療レコード又は初診日に印刷
                            if (_settings.SSSCheckPrint && (_status.SyochiListIndex == 0 || syochiListItem.Is初診日))
                            {
                                var items = _checkPrint.GetItems(printItem.KarteId);

                                // リストの項目を増やす
                                int insertItemsCount = items.Length;
                                var ar = new SinryouDataLoader.KarteRecord[records.Length + insertItemsCount];
                                Array.Copy(records, 0, ar, insertItemsCount, records.Length);
                                records = ar;

                                // 印字用レコードを先頭に追加
                                checkPrintLength = items.Length;

                                for (int i = 0; i < checkPrintLength; ++i)
                                {
                                    var text = $"（チェック: {items[i].Comment}）";

                                    records[i] = new SinryouDataLoader.KarteRecord(text, "", 0);
                                }

                                // クリップ領域の設定用のパスを変更
                                checkPrintLength -= _status.RecordIndex;

                                var exRect = new RectangleF(
                                                    _columnTensuu.Bounds.X - _resources.LinePen.Width,
                                                    baseY,
                                                    _tableBounds.Right - _columnTensuu.Bounds.X,
                                                    0);
                                float rowCount = (rows + checkPrintLength < ROW_COUNT)
                                                    ? checkPrintLength
                                                    : (ROW_COUNT - rows - 1);

                                exRect.Height = _rowHeight * rowCount;
                                excludePath.AddRectangle(exRect);
                            }
                        }

                        bool drawDate = _prevDate != syochiListItem.診療日;
                        int recordLen = records.Length;

                        for (; _status.RecordIndex < recordLen; _status.NextRecord())
                        {
                            if (rows == ROW_COUNT - 1)
                            {
                                // 行がいっぱいなので次ページへ

                                hasMorePage = true;

                                // ループを抜ける
                                goto LOOPEND;
                            }

                            SinryouDataLoader.KarteRecord record = records[_status.RecordIndex];

                            // 区切り線　横
                            g.DrawLine(_resources.LinePen, _tableBounds.X, baseY, _tableBounds.Right, baseY);

                            currentTextY = baseY + textPaddingTop;


                            // 月日
                            if (drawDate)
                            {
                                g.DrawString(
                                    syochiListItem.診療日.ToString("M/d"), _resources.LabelFont, _resources.Brush,
                                    _columnDate.Bounds.X + 0.3f, currentTextY);

                                drawDate = false;
                                _prevDate = syochiListItem.診療日;

                                // 年
                                int year = syochiListItem.診療日.Year;
                                if (prevYear != year)
                                {
                                    prevYear = year;

                                    var txt = year.ToString();
                                    var size = g.MeasureString(txt, _resources.LabelFont);
                                    g.DrawString(
                                        txt, _resources.LabelFont, _resources.Brush,
                                        _columnDate.Bounds.X - 0.3f - size.Width, currentTextY);
                                }
                            }

                            // 部位
                            if (_status.RecordIndex == checkPrintLength && _status.SyochiIndex == 0)
                            {
                                if (syochiListItem.歯式 == null)
                                {
                                    // 斜線
                                    g.DrawLine(_resources.LinePen, _columnBui.Bounds.X, baseY + _rowHeight, _columnSyochi.Bounds.X, baseY);
                                }
                                else
                                {
                                    OmoSeitokuEreceipt.Drawing.ERObjectDrawer.DrawSisikiFixed(
                                        g, _resources.LinePen, syochiListItem.歯式, syochiListItem.病名,
                                        new RectangleF(_columnBui.Bounds.X + 0.2f, baseY + 0.2f, _columnBui.Bounds.Width - 0.4f, _rowHeight - 0.4f),
                                        _resources.LabelFontSmall, true);
                                }
                            }

                            // 療法・処置
                            RectangleF syoRect = new RectangleF(
                                                        _columnSyochi.Bounds.X + 1f,
                                                        baseY + 0.8f,
                                                        (string.IsNullOrEmpty(record.Tensuu)
                                                                ? _tableBounds.Right
                                                                : _columnTensuu.Bounds.X
                                                            ) - _columnSyochi.Bounds.X - 3f,
                                                        _rowHeight * 1.5f);

                            g.DrawString(record.Syochi, _resources.LabelFont, _resources.Brush, syoRect);

                            // 点数 (右寄せ)
                            if (!string.IsNullOrEmpty(record.Tensuu))
                            {
                                // 右寄せ
                                int tenLen = record.Tensuu.Length;
                                float tensuuX = tensuuXCache[tenLen];
                                if (tensuuX < 0)
                                {
                                    tensuuXCache[tenLen] = tensuuX = _columnHutan.Bounds.X - g.MeasureString(record.Tensuu, _resources.LabelFont).Width - 1f;
                                }

                                g.DrawString(record.Tensuu, _resources.LabelFont, _resources.Brush, tensuuX, currentTextY);


                                // 合計点数に追加
                                _status.TotalTensuu += record.TensuuNum;
                            }


                            ++rows;

                            baseY += _rowHeight;

                        } // Loop: Record

                    } // Loop: Syochi


                    // 負担金徴収額
                    if (_status.TotalTensuu != 0)
                    {
                        bool drawKingaku = false;

                        // 次の処置リストの日付が今回と異なる場合金額を印字する。
                        if (_status.SyochiListIndex + 1 < syochiListLen)
                        {
                            SinryouData nextSyochi = printItem.SyochiList[_status.SyochiListIndex + 1];

                            drawKingaku = (syochiListItem.診療日 != nextSyochi.診療日);
                        }
                        // 印刷アイテムの最後の処置リストの場合は金額を印字する。
                        else
                        {
                            drawKingaku = true;
                        }

                        if (drawKingaku)
                        {
                            DrawKingaku(g, printItem, _tableBounds.Right, currentTextY);

                            _status.TotalTensuu = 0;
                        }
                    }

                } // Loop: SyochiList

                _status.SyochiListIndex = 0;


                if (rows == 0)
                {
                    // 行がひとつも無かった場合、次の印刷アイテムに移動
                    if (!NextItem())
                    {
                        // 次のアイテムが無い場合、処理を抜ける
                        return false;
                    }
                }
                else
                {
                    // 行が余っていたら区切り線を付け足す
                    for (; rows < ROW_COUNT; ++rows)
                    {
                        // 区切り線　横
                        g.DrawLine(_resources.LinePen, _tableBounds.X, baseY, _tableBounds.Right, baseY);

                        baseY += _rowHeight;
                    }

                    break;
                }

            }

        LOOPEND:

            try
            {
                // クリップ領域を設定
                using (var region = new Region(excludePath))
                {
                    g.ExcludeClip(region);
                }
                excludePath.Dispose();


                // カルテ情報
                this.DrawKarteHeader(g, printItem);


                // (外枠)
                g.DrawRectangle(_resources.LinePen, _tableBounds.X, _tableBounds.Y, _tableBounds.Width, _tableBounds.Height);


                this.DrawAllColumnHeaderText(g);


                // (縦線) 部位
                g.DrawLine(_resources.LinePen, _columnBui.Bounds.X, _tableBounds.Y, _columnBui.Bounds.X, _tableBounds.Bottom);

                // (縦線) 療法・処置
                g.DrawLine(_resources.LinePen, _columnSyochi.Bounds.X, _tableBounds.Y, _columnSyochi.Bounds.X, _tableBounds.Bottom);

                // (縦線) 点数
                g.DrawLine(_resources.LinePen, _columnTensuu.Bounds.X, _tableBounds.Y, _columnTensuu.Bounds.X, _tableBounds.Bottom);

                // (縦線) 負担金徴収額
                g.DrawLine(_resources.LinePen, _columnHutan.Bounds.X, _tableBounds.Y, _columnHutan.Bounds.X, _tableBounds.Bottom);


                // ページ情報
                this.DrawPageInformation(g, _tableBounds);

            }
            finally
            {
                // クリップ領域を解除
                g.ResetClip();
            }

            return hasMorePage;
        }


        private void DrawKingaku(Graphics g, KartePrintItem printItem, float rightLineX, float y)
        {
            int kingaku = printItem.CalcHutanKingaku(_status.TotalTensuu);
            string kingakuStr = String.Format(@"\ {0:#,0}", kingaku);

            // 右寄せ
            int kingakuLen = kingakuStr.Length;
            float x = _kingakuXCache[kingakuLen];
            if (x < 0)
            {
                SizeF size = g.MeasureString(kingakuStr, _resources.LabelFont);
                _kingakuXCache[kingakuLen] = x = rightLineX - size.Width - 1f;

                _PrintKingaku_FontHeightHalf = size.Height * 0.5f;
                if (x < _PrintKingaku_MinX) _PrintKingaku_MinX = x;
            }

            g.DrawString(
                kingakuStr,
                _resources.LabelFont, _resources.Brush, x, y - _PrintKingaku_FontHeightHalf);

            g.DrawString(
                "(" + _status.TotalTensuu + "点)",
                _resources.LabelFont, _resources.Brush,
                _PrintKingaku_MinX, y + _PrintKingaku_FontHeightHalf);
        }
        private float _PrintKingaku_FontHeightHalf;
        private float _PrintKingaku_MinX = float.MaxValue;




        private void DrawKarteHeader(Graphics g, KartePrintItem printItem)
        {
            var sb = new StringBuilder(255);

            // カルテID（診療所, カルテ番号）
            if (_settings.ShowKarteNumber)
            {
                sb.Append(' ')
                  .Append(printItem.KarteId);
            }

            // 患者名
            if ((_settings.ShowKarteName) && (!string.IsNullOrEmpty(printItem.KarteName)))
            {
                sb.Append(" 「")
                  .Append(printItem.KarteName)
                  .Append("」");
            }

            // カルテ期間
            if (_settings.ShowKarteKikan)
            {
                var list = printItem.SyochiList;

                sb.Append(" 期間: ").
                   Append(list[0].診療日.ToLongDateString()).
                   Append(" ～ ").
                   Append(list[list.Length - 1].診療日.ToLongDateString());
            }

            if (sb.Length != 0)
            {
                g.DrawString(
                    sb.ToString(1, sb.Length - 1),
                    _resources.LabelFontSmall,
                    _resources.Brush,
                    _tableBounds.X,
                    _tableBounds.Y - _resources.LabelFontSmall.GetHeight(g) * 2f
                    );
            }
        }


        private void DrawAllColumnHeaderText(Graphics g)
        {
            float middleY = _tableBounds.Y + (_rowHeight - g.MeasureString("M", _resources.LabelFont).Height) * 0.5f;
            Column col;

            // 月日
            col = _columnDate;
            this.DrawColumnHeaderText(
                    g: g,
                    text: col.Text,
                    x: col.Bounds.X,
                    y: middleY,
                    width: col.Bounds.Width,
                    height: col.Bounds.Height,
                    font: _resources.LabelFont,
                    alignment: ContentAlignment.TopCenter);


            // 部位
            col = _columnBui;
            this.DrawColumnHeaderText(
                    g: g,
                    text: col.Text,
                    x: col.Bounds.X,
                    y: middleY,
                    width: col.Bounds.Width,
                    height: col.Bounds.Height,
                    font: _resources.LabelFont,
                    alignment: ContentAlignment.TopCenter);


            // 療法・処置
            col = _columnSyochi;
            this.DrawColumnHeaderText(
                    g: g,
                    text: col.Text,
                    x: col.Bounds.X,
                    y: middleY,
                    width: col.Bounds.Width,
                    height: col.Bounds.Height,
                    font: _resources.LabelFont,
                    alignment: ContentAlignment.TopCenter);

            // 点数
            col = _columnTensuu;
            this.DrawColumnHeaderText(
                    g: g,
                    text: col.Text,
                    x: col.Bounds.X,
                    y: middleY,
                    width: col.Bounds.Width,
                    height: col.Bounds.Height,
                    font: _resources.LabelFont,
                    alignment: ContentAlignment.TopCenter);

            // 負担金徴収額
            col = _columnHutan;
            this.DrawColumnHeaderText(
                    g: g,
                    text: col.Text,
                    x: col.Bounds.X,
                    y: _tableBounds.Y,
                    width: col.Bounds.Width,
                    height: col.Bounds.Height,
                    font: _resources.LabelFont,
                    alignment: ContentAlignment.MiddleCenter);
        }

        private void DrawColumnHeaderText(Graphics g, string text, float x, float y, float width, float height, Font font, ContentAlignment alignment)
        {
            if (alignment != ContentAlignment.TopLeft)
            {
                SizeF textSize = g.MeasureString(text, font);

                switch (alignment)
                {
                    case ContentAlignment.TopCenter:
                        x += (width - textSize.Width) * 0.5f;
                        break;
                    case ContentAlignment.TopRight:
                        x += (width - textSize.Width);
                        break;
                    case ContentAlignment.MiddleLeft:
                        y += (height - textSize.Height) * 0.5f;
                        break;
                    case ContentAlignment.MiddleCenter:
                        x += (width - textSize.Width) * 0.5f;
                        y += (height - textSize.Height) * 0.5f;
                        break;
                    case ContentAlignment.MiddleRight:
                        x += (width - textSize.Width);
                        y += (height - textSize.Height) * 0.5f;
                        break;
                    case ContentAlignment.BottomLeft:
                        y += (height - textSize.Height);
                        break;
                    case ContentAlignment.BottomCenter:
                        x += (width - textSize.Width) * 0.5f;
                        y += (height - textSize.Height);
                        break;
                    case ContentAlignment.BottomRight:
                        x += (width - textSize.Width);
                        y += (height - textSize.Height);
                        break;
                }
            }

            g.DrawString(text, font, _resources.Brush, x, y);
        }


        // ページ情報
        private void DrawPageInformation(Graphics g, RectangleF rect)
        {
            string pageText = (++_pageCount).ToString();
            SizeF pageTextSize = g.MeasureString(pageText, _resources.LabelFontSmall);
            float pageTextTop = rect.Bottom + pageTextSize.Height;
            float pageTextLineY = pageTextTop + pageTextSize.Height * 0.5f;
            float pageTextLineW = 2.6f;
            float pageTextPadding = 0.7f;
            float pageTextAreaW = pageTextLineW + pageTextPadding + pageTextSize.Width + pageTextPadding + pageTextLineW;
            float pageTextLineX = rect.X + (rect.Width - pageTextAreaW) * 0.5f;

            g.DrawLine(_resources.LinePen, pageTextLineX, pageTextLineY, pageTextLineX + pageTextLineW, pageTextLineY);
            pageTextLineX += pageTextLineW + pageTextPadding;
            g.DrawString(pageText, _resources.LabelFontSmall, _resources.Brush, pageTextLineX, pageTextTop);
            pageTextLineX += pageTextSize.Width + pageTextPadding;
            g.DrawLine(_resources.LinePen, pageTextLineX, pageTextLineY, pageTextLineX + pageTextLineW, pageTextLineY);
        }
    }
}
