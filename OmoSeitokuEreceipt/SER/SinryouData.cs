using OmoEReceLib;
using OmoEReceLib.ERObjects;
using OmoSeitoku;
using OmoSeitoku.VB;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OmoSeitokuEreceipt.SER
{
    [Serializable]
    public sealed class SinryouData
    {
        private int[] _病名番号;

        public int カルテ番号;
        public int 行番号;
        public DateTime 診療日;
        public int 担当医師;
        public ER歯式 歯式;
        public int[] 病名番号
        {
            get { return _病名番号; }
            set
            {
                _病名番号 = value;

                int len = value.Length;
                string[] list = new string[len];
                for (int i = 0; i < len; ++i)
                {
                    list[i] = SinryouDataLoader.Get病名(value[i]);
                }

                this.病名 = list;
            }
        }
        public string[] 病名
        {
            get;
            private set;
        }
        public SyochiData[] 処置;
        public bool Is初診日;


        // Constructor

        public SinryouData()
        {
        }
        public SinryouData(SinryouData source)
        {
            this._病名番号 = (int[])source._病名番号.Clone();
            this.カルテ番号 = source.カルテ番号;
            this.行番号 = source.行番号;
            this.診療日 = source.診療日;
            this.担当医師 = source.担当医師;
            this.歯式 = new ER歯式(source.歯式);
            this.病名 = (string[])source.病名.Clone();
            this.処置 = (SyochiData[])source.処置.Clone();
            this.Is初診日 = source.Is初診日;
        }


        // Method

        /// <summary>
        /// データを統合
        /// </summary>
        public void Combine(SinryouData data)
        {
            // 病名リストの結合
            {
                string[] src = data.病名;
                var dest = new List<string>(this.病名);

                for (int i = 0, len = src.Length; i < len; ++i)
                {
                    if (!dest.Contains(src[i]))
                        dest.Add(src[i]);
                }

                this.病名 = dest.ToArray();
            }

            // 処置リストの結合
            {
                SyochiData[] src = data.処置;
                var dest = new List<SyochiData>(this.処置);

                for (int i = 0, len = src.Length; i < len; ++i)
                {
                    SyochiData srcSD = src[i];

                    int index = dest.FindIndex(destSD => (destSD.Number == srcSD.Number));

                    if (0 <= index)
                    {
                        SyochiData destSD = dest[index];
                        destSD.Kaisuu += srcSD.Kaisuu;
                        dest[index] = destSD;
                    }
                    else
                    {
                        dest.Add(srcSD);
                    }
                }

                this.処置 = dest.ToArray();
            }
        }

        //public string To処置表示用文字列()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (SyochiData s in this.処置)
        //    {
        //        sb.Append(s.Name).Append(" x ").AppendLine(s.Kaisuu.ToString());
        //    }
        //    return sb.ToString();
        //}

        #region 等価演算のオーバーライド

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var p = obj as SinryouData;
            if ((object)p == null) return false;

            return (this == p);
        }
        public bool Equals(SinryouData p)
        {
            if ((object)p == null) return false;
            return (this == p);
        }
        public override int GetHashCode()
        {
            int byoumeiHashCode = 0;
            int syochiHashCode = 0;

            if (this._病名番号 != null)
            {
                var byo = (System.Collections.IStructuralEquatable)this._病名番号;
                byoumeiHashCode = byo.GetHashCode(EqualityComparer<int>.Default);
            }
            if (this.処置 != null)
            {
                var syo = (System.Collections.IStructuralEquatable)this.処置;
                syochiHashCode = syo.GetHashCode(EqualityComparer<SyochiData>.Default);
            }

            return byoumeiHashCode
                 ^ this.カルテ番号.GetHashCode()
                 ^ this.行番号.GetHashCode()
                 ^ this.診療日.GetHashCode()
                 ^ this.担当医師.GetHashCode()
                 ^ ((this.歯式 == null) ? 0 : this.歯式.GetHashCode())
                 ^ syochiHashCode
                 ^ this.Is初診日.GetHashCode();
        }

        public static bool operator ==(SinryouData a, SinryouData b)
        {
            if (Object.ReferenceEquals(a, b)) return true;
            if (((object)a == null) || ((object)b == null)) return false;

            return ((a._病名番号 == b._病名番号) || a._病名番号.SequenceEqual(b._病名番号))
                && (a.カルテ番号 == b.カルテ番号)
                && (a.行番号 == b.行番号)
                && (a.診療日 == b.診療日)
                && (a.担当医師 == b.担当医師)
                && (a.歯式 == b.歯式)
                && ((a.処置 == b.処置) || a.処置.SequenceEqual(b.処置))
                && (a.Is初診日 == b.Is初診日);
        }
        public static bool operator !=(SinryouData a, SinryouData b)
        {
            return !(a == b);
        }

        #endregion
    }

    public sealed class SinryouDataLoader
    {
        public const int SIZE = GBitRstrct3;

        // 必ず偶数
        private const int GochsA = 24;

        /// <summary>
        /// 歯式以外のサイズ = 患者番号 + 行番号 + 前レコード番号 + 後レコード番号 + 日付 + Dr(2 Byte)
        /// </summary>
        private const int GBitRstrct1 = 2 + 2 + 4 + 4 + 4 + 2;

        /// <summary>
        /// 歯式サイズ = 歯式最大表示分(GochsA / 2 * 4 * 4 Byte) + 病名2個分(2 * 4 Byte) + 処置3個分(3 * 4 Byte)
        /// </summary>
        private const int GBitRstrct2 = GochsA / 2 * 4 * 4 + 3 * 4 + 3 * 4;

        /// <summary>
        /// 行記録サイズ
        /// (最後の + 4 は GHaByoSyo の添字指定がVBとの差異を埋めるため VBLong のサイズを足している)
        /// </summary>
        private const int GBitRstrct3 = GBitRstrct1 + GBitRstrct2 + 4;

        /// <summary>
        /// 病名, 歯式, 処置個数制限.
        /// (+ 1 は GHaByoSyo の添字指定がVBとの差異を埋めるため要素数を1つ足している)
        /// </summary>
        private const int GKosRstrct1 = GBitRstrct2 / 4 + 1;



        // 処置対応辞書
        private static Dictionary<string, HashSet<int>> _syochiTaiouDB;
        // 歯席辞書  { index => ERCode }
        private static Dictionary<int, string> _hasekiDB;

        /// <summary>
        /// 診療録レコード行
        /// </summary>
        private sealed class Gyokiro
        {
            /// <summary>
            /// 歯席 識別
            /// </summary>
            private const long OFFSET_HASEKI_DATA = 1000000;
            /// <summary>
            /// 病名 識別
            /// </summary>
            private const long OFFSET_BYOUMEI_DATA = 2000000;
            /// <summary>
            /// 処置・正数回数 識別
            /// </summary>
            private const long OFFSET_SYOCHI_DATA = 3000000;
            /// <summary>
            /// 処置・負数回数 最大 識別
            /// </summary>
            private const long OFFSET_SYOCHI_MMAX_DATA = -3000000;
            /// <summary>
            /// 処置・負数回数 最小 識別
            /// </summary>
            private const long OFFSET_SYOCHI_MMIN_DATA = -4000000;
            /// <summary>
            /// 処置書き換え 識別
            /// </summary>
            private const long OFFSET_KAKIKAE_DATA = 4100000;
            /// <summary>
            /// 歯周検査ファイル記録番号 識別
            /// </summary>
            private const long OFFSET_SISYUU_KENSA_DATA = 4400000;
            /// <summary>
            /// 摘要 識別
            /// </summary>
            private const long OFFSET_TEKIYOU_DATA = 4600000;
            //private const long OFFSET__DATA = 4800000;
            /// <summary>
            /// Pattern 番号(4) 進度(2) 病名(7) 識別
            /// </summary>
            private const long OFFSET_PATTERN_DATA = 1000000000;
            private const long OFFSET_xx_DATA = 1100000000; // 未使用？

            /// <summary>
            /// 処置・回数データを分割する値  (Mhbs10000)
            /// </summary>
            private const int SYOCHI_KAISUU_SIKIBETU = 10000;

            /// <summary>
            /// 歯席データを分割する値  (Mhbs10000)
            /// </summary>
            private const int HASEKI_SPLIT_SIKIBETU = 10000;

            public int GKrBan;
            public int GGyoBan;
            public long GZenBan;
            public long GAtoBan;
            public DateTime GDayBan;
            public int GDrBan;
            public long[] GHaByoSyo = new long[GKosRstrct1];

            //HaByoSyo
            public ER歯式 _歯式;
            public List<int> _病名;
            public List<SyochiData> _処置;


            public bool _Is初診日;


            public SinryouData ToSinryouData()
            {
                return new SinryouData()
                {
                    カルテ番号 = this.GKrBan,
                    行番号 = this.GGyoBan,
                    診療日 = this.GDayBan,
                    担当医師 = this.GDrBan,
                    歯式 = this._歯式,
                    病名番号 = this._病名.ToArray(),
                    処置 = this._処置.ToArray(),
                    Is初診日 = this._Is初診日
                };
            }


            /// <summary>
            /// バイナリから行データを取得する。
            /// </summary>
            /// <param name="input"></param>
            public void Read(BinaryReader input)
            {
                this.GKrBan = VBRandomFile.ReadInt(input);
                this.GGyoBan = VBRandomFile.ReadInt(input);
                this.GZenBan = VBRandomFile.ReadLong(input);
                this.GAtoBan = VBRandomFile.ReadLong(input);

                DateTime? dt = VBRandomFile.ReadDate(input);
                if (dt.HasValue)
                {
                    this.GDayBan = dt.Value.Date;

                    //Console.WriteLine(this.GDayBan.ToShortDateString());
                }

                this.GDrBan = VBRandomFile.ReadInt(input);

                long[] bs = this.GHaByoSyo;
                for (int i = 0, len = bs.Length; i < len; ++i)
                {
                    bs[i] = VBRandomFile.ReadLong(input);
                }

                this.ParseHaByoSyo();
            }

            /// <summary>
            /// 行データ1つ分ストリームの位置を進める
            /// </summary>
            public void SeekNext(BinaryReader input)
            {
                input.BaseStream.Seek(SIZE, SeekOrigin.Current);
            }

            private void ParseHaByoSyo()
            {
                var sisiki = new ER歯式();
                _歯式 = sisiki;

                List<int> byomeiList = new List<int>();
                _病名 = byomeiList;

                List<SyochiData> syochiList = new List<SyochiData>();
                _処置 = syochiList;

                long[] ar = GHaByoSyo;
                int len = ar.Length;
                int index = 1; // 先頭の添え字のズレ用に 予め+1しておく。

                for (; index < len; ++index)
                {
                    long data = ar[index];

                    if (data <= 0)
                    {
                        if (OFFSET_SYOCHI_MMIN_DATA <= data && data <= OFFSET_SYOCHI_MMAX_DATA)
                        {
                            // 処置 負数回数
                            data = -data - OFFSET_SYOCHI_DATA;

                            int kaisuu = -(int)data / SYOCHI_KAISUU_SIKIBETU;
                            int syochiNumber = (int)(data - -kaisuu * SYOCHI_KAISUU_SIKIBETU);

                            //Console.WriteLine("処置(負): {0} x {1} ({2})", syochiNumber, kaisuu, data);


                            syochiList.Add(CreateSyochidata(syochiNumber, kaisuu));

                            // TODO 負数回数処置は開始日・終了日等の判定に反映させるかどうか(現在はさせていない)
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (OFFSET_xx_DATA <= data)
                    {
                        // 
                        data -= OFFSET_xx_DATA;

                        //Console.WriteLine("謎の診療データ: {0}", data);
                    }
                    else if (OFFSET_PATTERN_DATA <= data)
                    {
                        // Pattern
                        data -= OFFSET_PATTERN_DATA;

                        // Console.WriteLine("Pattern: {0}", data);
                    }
                    else if (OFFSET_TEKIYOU_DATA <= data)
                    {
                        // 摘要
                        data -= OFFSET_TEKIYOU_DATA;

                        //Console.WriteLine("摘要: {0}", data);
                    }
                    else if (OFFSET_SISYUU_KENSA_DATA <= data)
                    {
                        // 歯周検査 (P検査)
                        data -= OFFSET_SISYUU_KENSA_DATA;

                        //Console.WriteLine("歯周検査: {0}", data);
                    }
                    else if (OFFSET_KAKIKAE_DATA <= data)
                    {
                        // 書き換え
                        data -= OFFSET_KAKIKAE_DATA;

                        //Console.WriteLine("書き換え: {0}", data);
                    }
                    else if (OFFSET_SYOCHI_DATA <= data)
                    {
                        // 処置 正数回数
                        data -= OFFSET_SYOCHI_DATA;

                        int kaisuu = (int)data / SYOCHI_KAISUU_SIKIBETU;
                        int syochiNumber = (int)(data - kaisuu * SYOCHI_KAISUU_SIKIBETU);

                        //Console.WriteLine("処置: {0} x {1} ({2})", syochi, kaisuu, data);

                        syochiList.Add(CreateSyochidata(syochiNumber, kaisuu));


                        int syochiId = SyochiData.GetSyochiId(syochiNumber);

                        if (_syochiTaiouDB.ContainsKey("初診"))
                        {
                            if (_syochiTaiouDB["初診"].Contains(syochiId))
                            {
                                _Is初診日 = true;
                            }
                        }
                    }
                    else if (OFFSET_BYOUMEI_DATA <= data)
                    {
                        // 病名
                        data -= OFFSET_BYOUMEI_DATA;

                        //Console.WriteLine("病名: {0}, 日時 {1}", data, this.GDayBan);

                        byomeiList.Add((int)data);
                    }
                    else if (OFFSET_HASEKI_DATA <= data)
                    {
                        // 歯席
                        data -= OFFSET_HASEKI_DATA;

                        int haseki = (int)data / HASEKI_SPLIT_SIKIBETU;
                        int haDBIndex = (int)data - haseki * HASEKI_SPLIT_SIKIBETU;

                        //Console.WriteLine("歯式: {0}, {1}, {2}", haseki, haDBIndex, data);


                        if (_hasekiDB.ContainsKey(haDBIndex))
                        {
                            string erCode = _hasekiDB[haDBIndex];
                            __ParseHaByoSyo_HaContainer[0] = ER歯式単位.FromERCode(erCode);
                            sisiki.Add(__ParseHaByoSyo_HaContainer);
                        }
                    }
                    else
                    {
                        // 
                        Console.WriteLine("未確認データ: {0}", data);
                    }
                }
            }
            private ER歯式単位[] __ParseHaByoSyo_HaContainer = new ER歯式単位[1];

            private SyochiData CreateSyochidata(int syochiNumber, int kaisuu)
            {
                string syochiName;
                try
                {
                    syochiName = GetSyochiName(syochiNumber, this.GDayBan);
                }
                catch (Exception ex)
                {
                    syochiName = "(エラー： " + ex.Message + ")";
                }

                int syochiTensuu;
                string tensuu;
                try
                {
                    tensuu = GetSyochiTensuu(syochiNumber, this.GDayBan);
                }
                catch (Exception ex)
                {
                    tensuu = ex.Message;
                }
                if (!int.TryParse(tensuu, out syochiTensuu))
                {
                    syochiName += "(エラー： " + tensuu + ")";
                }

                bool isFinish = false;
                try
                {
                    // 終了日フラグの判定
                    string p = GetSyochi3bites(syochiNumber, this.GDayBan);
                    // 処置ﾃﾞｰﾀﾍﾞｰｽ 4列目 abcdefghijk
                    // 装着ﾌﾗｯｸﾞ
                    //   k = 1 印象
                    //   k = 2 試適
                    //   k = 3 装着
                    char isFinFlag = '3';
                    int isFinFlagPos = 10;
                    if (!string.IsNullOrWhiteSpace(p))
                    {
                        isFinish = (isFinFlagPos < p.Length) && (p[isFinFlagPos] == isFinFlag);
                    }
                }
                catch { }

                return new SyochiData(syochiName, syochiNumber, syochiTensuu, kaisuu, isFinish);
            }
        }


        private SinryouData[] _shinryouList;


        public readonly KarteId KarteId;



        public SinryouDataLoader(KarteId karteId, FileStream fs, BinaryReader br)
        {
            if (karteId is null) { throw new ArgumentNullException(nameof(karteId)); }

            this.KarteId = karteId;




            //UNDONE debug 処置対応DB読み込 ハードコード
            if (_syochiTaiouDB == null)
            {
                _syochiTaiouDB = new Dictionary<string, HashSet<int>>();
                SeitokuDB syoTaiouDB = new SeitokuDB(
                    Path.Combine(global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder, "処置対応DB.txt"));
                string[,] rows = syoTaiouDB.Rows;
                for (int row = 0, len = rows.GetLength(0); row < len; ++row)
                {
                    string category = rows[row, SDB.処置対応DB_COL_カテゴリー];

                    if (!string.IsNullOrEmpty(category))
                    {
                        if (!_syochiTaiouDB.ContainsKey(category))
                        {
                            _syochiTaiouDB.Add(category, new HashSet<int>());
                        }

                        string syochiBangou = rows[row, SDB.処置対応DB_COL_処置番号];

                        _syochiTaiouDB[category].Add(int.Parse(syochiBangou));
                    }
                }
            }



            //UNDONE debug 歯席DB読み込 ハードコード
            if (_hasekiDB == null)
            {
                _hasekiDB = new Dictionary<int, string>();
                SeitokuDB hasekiDB = new SeitokuDB(
                    Path.Combine(global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder, "hasekise.900"));
                string[,] rows = hasekiDB.Rows;
                for (int row = 0, len = hasekiDB.Rows.GetLength(0); row < len; ++row)
                {
                    int index;
                    if (int.TryParse(rows[row, SDB.HASEKI_COL_INDEX], out index) && 0 < index)
                    {
                        string erCode = rows[row, SDB.HASEKI_COL_ERCODE];

                        if (!string.IsNullOrEmpty(erCode))
                        {
                            _hasekiDB.Add(index, erCode);
                        }
                    }
                }
            }



            List<Gyokiro> list = new List<Gyokiro>();

            // 患者テーブルから後番号を取得
            fs.Seek((karteId.KarteNumber - 1) * SIZE, SeekOrigin.Begin);

            Gyokiro gyoData = new Gyokiro();

            gyoData.Read(br);


            // 後番号が0になるまで、指定した患者番号の行データを取得する
            long atoBan;

            while ((atoBan = gyoData.GAtoBan) != 0)
            {
                // 後番号から行データの位置へシーク
                fs.Seek((atoBan - 1) * SIZE, SeekOrigin.Begin);

                gyoData = new Gyokiro();
                gyoData.Read(br);

                list.Add(gyoData);
            }


            var sinryouList = new SinryouData[list.Count];
            for (int i = list.Count - 1; 0 <= i; --i)
            {
                sinryouList[i] = list[i].ToSinryouData();
            }
            _shinryouList = sinryouList;
        }

        /// <summary>
        /// 指定した期間を含む正しい診療期間にコンバートする。
        /// 開始日を、期間内の診療日を含む最小の診療期間の初診日にする。
        /// 終了日を、終了日を含む診療期間の最終日にする。
        /// </summary>
        public DateRange ConvertSinryouDateRange(DateRange kikan,
                                bool expandToSyosinbi, bool expandToLastDate)
        {
            var sinryouList = _shinryouList;
            var sinryouListLen = sinryouList.Length;

            if ((0 == sinryouListLen) ||
                (!expandToSyosinbi && !expandToLastDate))
            {
                return kikan;
            }

            var res = new DateRange2();

            // 開始日
            int lastSyosinbiIndex = 0;
            if (expandToSyosinbi)
            {
                // 開始日の診療期間の初診日を取得
                for (int i = 0; i < sinryouListLen; ++i)
                {
                    var data = sinryouList[i];

                    if (data.Is初診日)
                    {
                        lastSyosinbiIndex = i;
                    }

                    if (kikan.Min <= data.診療日)
                    {
                        if (kikan.Max < data.診療日)
                        {
                            return kikan;
                        }

                        res.Min = sinryouList[lastSyosinbiIndex].診療日;
                        break;
                    }
                }
            }
            else
            {
                res.Min = kikan.Min;
            }

            // 終了日
            if (expandToLastDate)
            {
                // 終了日の診療期間の最終日を取得
                bool overMax = false;
                for (int i = lastSyosinbiIndex + 1; i < sinryouListLen; ++i)
                {
                    var data = sinryouList[i];

                    if (!overMax)
                    {
                        if (kikan.Max <= data.診療日)
                        {
                            overMax = true;
                            --i;
                        }
                        continue;
                    }

                    if (data.Is初診日)
                    {
                        res.Max = sinryouList[i - 1].診療日;
                        break;
                    }
                }
            }
            else
            {
                res.Max = kikan.Max;
            }

            if (!res.Min.HasValue)
            {
                res.Min = DateTime.MaxValue;
                //res.Min = sinryouList[0].診療日;
            }

            if (!res.Max.HasValue)
            {
                res.Max = sinryouList[sinryouListLen - 1].診療日;
            }

            return res.ToDateRange();
        }

        /// <summary>
        /// 同一歯式による診療統合
        /// </summary>
        /// <param name="kikan"></param>
        /// <param name="syosinKikanGoto">初診期間ごとに結合する場合はtrue。falseの場合は全期間。</param>
        /// <returns></returns>
        private List<SinryouData> SinryouTougou(DateRange kikan, bool syosinKikanGoto)
        {
            var list = new List<SinryouData>(_shinryouList.Length);

            // 同一診療期間内の歯式重複チェック用 (歯式 => 行データリスト内のインデックス)
            var haSet = new Dictionary<string, int>();

            foreach (var sinryouData in _shinryouList)
            {
                if (!kikan.Contains(sinryouData.診療日))
                {
                    continue;
                }

                bool add;
                bool contains;

                string ha = sinryouData.歯式.ERCode;

                if (syosinKikanGoto && sinryouData.Is初診日)
                {
                    // 追加済み歯式辞書をクリアし、新たに追加できるようにする。
                    haSet.Clear();

                    contains = false;
                    add = true;
                }
                else
                {
                    contains = haSet.ContainsKey(ha);
                    add = !contains;
                }

                if (add)
                {
                    if (ha.Length != 0)
                    {
                        haSet.Add(ha, list.Count);
                    }

                    list.Add(sinryouData);
                }
                else if (contains && ha.Length != 0)
                {
                    // 行データを統合
                    var comb = new SinryouData(list[haSet[ha]]);
                    comb.Combine(sinryouData);
                    list[haSet[ha]] = comb;
                }
            }

            return list;
        }

        public ShinryouDataCollection GetShinryouData(DateRange kikan, 診療統合種別 shinryouTougou)
        {
            IEnumerable<SinryouData> collection;
            switch (shinryouTougou)
            {
                case 診療統合種別.統合なし:
                    collection = from shinryou in _shinryouList
                                 where kikan.Contains(shinryou.診療日)
                                 select shinryou;
                    break;

                case 診療統合種別.初診期間別:
                    collection = this.SinryouTougou(kikan, true);
                    break;
                case 診療統合種別.全期間:
                    collection = this.SinryouTougou(kikan, false);
                    break;
                default:
                    throw new Exception("未対応の診療統合種別.");
            }
            return new ShinryouDataCollection(collection);
        }


        public IEnumerable<DateTime> Get初診日リスト()
        {
            return from shinryou in _shinryouList
                   where shinryou.Is初診日
                   select shinryou.診療日;
        }


        private static SeitokuDB _byoDB;
        private static Dictionary<int, string> _byoumeiDBCache;

        public static string Get病名(int byoumeiBangou)
        {
            //UNDONE debug 病名DB読み込 ハードコード
            if (_byoDB == null)
            {
                _byoDB = new SeitokuDB(
                    Path.Combine(global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder, "byoumei.900"));

                _byoumeiDBCache = new Dictionary<int, string>();
            }

            // キャッシュされていたらそれを返す
            if (_byoumeiDBCache.ContainsKey(byoumeiBangou))
            {
                return _byoumeiDBCache[byoumeiBangou];
            }

            string result;

            int row = _byoDB.Find(0, byoumeiBangou.ToString(), 0);
            if (0 <= row)
            {
                string[,] rows = _byoDB.Rows;

                result = rows[row, SDB.BYOMEI_COL_レセプト病名];

                if (string.IsNullOrEmpty(result))
                {
                    result = rows[row, SDB.BYOMEI_COL_内部処理病名];

                    StringBuilder sb = _Get病名_sb;
                    sb.Length = 0;

                    foreach (char c in result)
                    {
                        // 異常な文字までのみ結合して返す
                        if (57631 <= (int)c)
                            break;
                        sb.Append(c);
                    }

                    result = sb.ToString();
                }

                result = Microsoft.VisualBasic.Strings.StrConv(result,
                     Microsoft.VisualBasic.VbStrConv.Hiragana | Microsoft.VisualBasic.VbStrConv.Narrow);
            }
            else
            {
                result = null;
            }

            // キャッシュする
            _byoumeiDBCache.Add(byoumeiBangou, result);

            return result;
        }

        private static StringBuilder _Get病名_sb = new StringBuilder(256);


        private struct LastSyochiDb
        {
            public DateTime startDate;
            public DateTime endDate;
            public SeitokuDB syochiDb;
            public string path;

            public bool IsIn(DateTime dt)
            {
                return (syochiDb != null) && (startDate <= dt) && (dt <= endDate);
            }
        }

        private static LastSyochiDb _lastSyochiDb = new LastSyochiDb();
        private static SeitokuDB _syoKikanDb;

        /// <exception cref="System.IndexOutOfRangeException">列が範囲外の場合に発生する。</exception>
        public static string GetSyochiValue(int syochiNumber, DateTime sinryoubi, int column)
        {
            const int COL_開始期日 = 1;
            const int COL_失効期日 = 2;
            const int COL_ファイルパス = 3;


            //点数有効期間DBから対象の点数ファイルを取得する。


            SeitokuDB syoDB;

            if (_lastSyochiDb.IsIn(sinryoubi))
            {
                syoDB = _lastSyochiDb.syochiDb;
            }
            else
            {
                if (_syoKikanDb == null)
                {
                    //UNDONE debug 処置DBリスト読み込 ハードコード
                    _syoKikanDb = new SeitokuDB(Path.Combine(
                        global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder, "S9X0401X.XXX"));
                }

                string[,] rows = _syoKikanDb.Rows;
                string path = null;

                for (int row = 0, len = rows.GetLength(0); row < len; ++row)
                {
                    string startDateStr = rows[row, COL_開始期日];
                    string endDateStr = rows[row, COL_失効期日];

                    DateTime startDate;
                    DateTime endDate;

                    if (DateTime.TryParse(startDateStr, out startDate) &&
                        DateTime.TryParse(endDateStr, out endDate))
                    {
                        if (startDate <= sinryoubi && sinryoubi <= endDate)
                        {
                            path = rows[row, COL_ファイルパス];

                            _lastSyochiDb.startDate = startDate;
                            _lastSyochiDb.endDate = endDate;

                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(path))
                {
                    // throw
                    throw new Exception($"点数ファイルを取得できません。" +
                                        $" 処置番号({syochiNumber})" +
                                        $" 診療日({sinryoubi})");
                }

                path = Path.Combine(global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder, $"{Path.GetFileName(path)}t");

                syoDB = new SeitokuDB(path);

                _lastSyochiDb.path = path;
                _lastSyochiDb.syochiDb = syoDB;
            }


            // 処置DBから指定した列のデータを取得

            const int COL_処置番号 = 0; //ハードコード

            string[,] syoRows = syoDB.Rows;

            for (int row = 0, len = syoRows.GetLength(0); row < len; ++row)
            {
                string numb = syoRows[row, COL_処置番号];

                int number;
                if (int.TryParse(numb, out number) && syochiNumber == number)
                {
                    string result = syoRows[row, column];

                    return Microsoft.VisualBasic.Strings.StrConv(result,
                                Microsoft.VisualBasic.VbStrConv.Hiragana |
                                Microsoft.VisualBasic.VbStrConv.Narrow);
                }
            }

            // throw
            throw new Exception($"処置を取得できません。" +
                                $" 処置番号({syochiNumber})" +
                                $" 処置DB({_lastSyochiDb.path})");
        }

        // 処置DBから名前を取得
        public static string GetSyochiName(int syochiNumber, DateTime sinryoubi)
        {
            const int COL_処置名 = 5;

            return GetSyochiValue(syochiNumber, sinryoubi, COL_処置名);
        }

        public static string GetSyochiTensuu(int syochiNumber, DateTime sinryoubi)
        {
            const int COL_処置点数 = 1;

            return GetSyochiValue(syochiNumber, sinryoubi, COL_処置点数);
        }

        public static string GetSyochi3bites(int syochiNumber, DateTime sinryoubi)
        {
            const int COL_3bites = 4;

            return GetSyochiValue(syochiNumber, sinryoubi, COL_3bites);
        }

        /// <exception cref="System.IndexOutOfRangeException">診療行為リスト列が存在しない場合に発生する。(データが無いのではなく列自体が無い場合)</exception>
        public static KarteRecord[] GetSyochiERCode(SyochiData syochi, DateTime sinryoubi)
        {
            const int COL_診療行為リスト = 6; // UNDONE 診療行為リスト列番号ハードコード


            //UNDONE SQL 歯科診療行為マスター内　列番号等ハードコード

            var res = new List<KarteRecord>();
            string sinList = null;

            try
            {
                sinList = GetSyochiValue(syochi.Number, sinryoubi, COL_診療行為リスト);
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new IndexOutOfRangeException("診療行為リスト列がありません。", ex);
            }

            if (string.IsNullOrEmpty(sinList))
            {
                if (!SyochiData.Is特殊処置(syochi.SyochiId))
                {
                    string tensuu = GetSyochiTensuu(syochi.Number, sinryoubi);

                    int num;

                    KarteRecord data = new KarteRecord(
                        $" (電レセコード無し) {syochi.Name} ({syochi.Kaisuu}回)",
                        tensuu,
                        (int.TryParse(tensuu, out num) ? num : 0)
                    );
                    res.Add(data);
                }
            }
            else
            {
                // 診療行為リストを解釈

                StringBuilder cmdText = _GetSyochiERCode_cmdText;

                foreach (Match m in _GetSyochiERCode_ERListReg.Matches(sinList))
                {
                    cmdText.Length = 0;

                    MasterType masterType;
                    int suuryou = 1;
                    bool scanMaster = true;

                    if (m.Groups["SS"].Success)
                    {
                        masterType = MasterType.SS;
                        string ssCode = m.Groups["SSCode"].Value;

                        cmdText.Append(" OR F3 ='").Append(ssCode).Append("'");
                    }
                    else if (m.Groups["SI"].Success)
                    {
                        masterType = MasterType.SI;
                        string ssCode = m.Groups["SICode"].Value;

                        cmdText.Append(" OR F3 ='").Append(ssCode).Append("'");
                    }
                    else if (m.Groups["KS"].Success)
                    {
                        masterType = MasterType.SS;
                        string ksCode = m.Groups["KSCode"].Value;

                        if (m.Groups["KSSuu"].Success)
                            suuryou = int.Parse(m.Groups["KSSuu"].Value);

                        cmdText.Append(" OR F8 = '").Append(ksCode).Append("'");
                    }
                    else if (m.Groups["IY"].Success)
                    {
                        masterType = MasterType.IY;
                        string ksCode = m.Groups["IYCode"].Value;

                        if (m.Groups["IYSuu"].Success)
                            suuryou = int.Parse(m.Groups["IYSuu"].Value);

                        cmdText.Append(" OR F3 = '").Append(ksCode).Append("'");
                    }
                    else if (m.Groups["TO"].Success)
                    {
                        masterType = MasterType.TO;
                        string ksCode = m.Groups["TOCode"].Value;

                        if (m.Groups["TOSuu"].Success)
                            suuryou = int.Parse(m.Groups["TOSuu"].Value);

                        cmdText.Append(" OR F3 = '").Append(ksCode).Append("'");
                    }
                    else if (m.Groups["CO"].Success)
                    {
                        // コメントレコードをリストに追加
                        var data = new KarteRecord(
                            m.Groups["Comment"].Value, "", 0);

                        if (syochi.Kaisuu != 1)
                        {
                            data.Syochi = $"{data.Syochi} ({syochi.Kaisuu}回)";
                        }

                        res.Add(data);

                        scanMaster = false;
                        masterType = MasterType.SS; // 使わないので適当
                    }
                    else
                    {
                        scanMaster = false;
                        masterType = MasterType.SS; // 使わないので適当
                    }

                    if (scanMaster)
                    {
                        suuryou *= syochi.Kaisuu;

                        // マスターからデータを取得してリストに追加

                        using (var com = _dbConnections[masterType].CreateCommand())
                        {
                            com.CommandText = $"SELECT * FROM [{_csvFileNames[masterType]}] WHERE 0 {cmdText}";

                            using (var reader = com.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    KarteRecord data = new KarteRecord();
                                    float tensuu = 0;

                                    switch (masterType)
                                    {
                                        case MasterType.SS:
                                            GetSyochiSS(ref tensuu, reader, data, suuryou);
                                            break;

                                        case MasterType.SI:
                                            GetSyochiSI(ref tensuu, reader, data, suuryou);
                                            break;

                                        case MasterType.IY:
                                            GetSyochiIY(ref tensuu, reader, data, suuryou);
                                            break;

                                        case MasterType.TO:
                                            GetSyochiTO(ref tensuu, reader, data, suuryou);
                                            break;

                                        default:
                                            throw new Exception($"未対応のマスターファイル種別（{masterType}）");
                                    }

                                    if (string.IsNullOrEmpty(data.Tensuu))
                                    {
                                        int tensuuInt = (int)tensuu;
                                        if (tensuu == 0)
                                        {
                                            data.Tensuu = " -- ";
                                        }
                                        else if (tensuuInt == tensuu)
                                        {
                                            data.Tensuu = tensuuInt.ToString();
                                        }
                                        else
                                        {
                                            data.Tensuu = tensuu.ToString();
                                        }
                                    }

                                    res.Add(data);
                                }
                            }
                        }
                    }
                }
            }

            return res.ToArray();
        }

        // 処置DBの診療リストを解釈
        private static Regex _GetSyochiERCode_ERListReg = new Regex(
                                $@"(?<=^|;)(" +
                                $@"(?<SS>ss(?<SSCode>\d{9});)|" +
                                $@"(?<SI>si(?<SICode>\d{9});)|" +
                                $@"(?<KS>ks(?<KSCode>[A-Z][A-Z]\d{3})(?:\[(?<KSSuu>\d+)\])?;)|" +
                                $@"(?<{RecordIY.識別}>iy{RecordIY.ERCodeRegex}(?:\[(?<IYSuu>\d+)\])?;)|" +
                                $@"(?<{RecordTO.識別}>to{RecordTO.ERCodeRegex}(?:\[(?<TOSuu>\d+)\])?;)|" +
                                $@"(?<CO>co(?<Comment>[^;]+);)" +
                                $@")");
        // SQL作成用
        private static StringBuilder _GetSyochiERCode_cmdText = new StringBuilder(1024);

        private static void GetSyochiSS(ref float tensuu, OleDbDataReader reader, KarteRecord data, int suuryou)
        {
            data.Syochi = reader["F10"].ToString();


            int tensuuSikibetsu = int.Parse(reader["F11"].ToString());

            // UNDONE マスター点数識別ハードコード
            switch (tensuuSikibetsu)
            {
                case 3: // 点数（プラス）
                    tensuu = float.Parse(reader["F12"].ToString()) * suuryou;
                    break;

                case 5: // %加算
                    data.Tensuu = $"+{reader["F12"]}%";
                    break;

                case 6: // %減産
                    data.Tensuu = $"-{reader["F12"]}%";
                    break;

                case 7: // 減点診療行為
                case 8: // 点数（マイナス）
                    tensuu = -float.Parse(reader["F12"].ToString()) * suuryou;
                    break;

                default:
                    Console.WriteLine($"点数識別　点数（プラス）以外 ({data})");
                    break;
            }

            if (suuryou != 1)
            {
                data.Syochi += $" ({suuryou} 回)";
            }
        }

        // 医科診療行為
        private static void GetSyochiSI(ref float tensuu, OleDbDataReader reader, KarteRecord data, int suuryou)
        {
            data.Syochi = reader["F10"].ToString();


            int tensuuSikibetsu = int.Parse(reader["F11"].ToString());

            // UNDONE マスター点数識別ハードコード
            switch (tensuuSikibetsu)
            {
                case 3: // 点数（プラス）
                    tensuu = float.Parse(reader["F12"].ToString()) * suuryou;
                    break;

                case 5: // %加算
                    data.Tensuu = $"+{reader["F12"]}%";
                    break;

                case 6: // %減産
                    data.Tensuu = $"-{reader["F12"]}%";
                    break;

                case 7: // 減点診療行為
                case 8: // 点数（マイナス）
                    tensuu = -float.Parse(reader["F12"].ToString()) * suuryou;
                    break;

                default:
                    Console.WriteLine($"点数識別　点数（プラス）以外 ({data})");
                    break;
            }

            if (suuryou != 1)
            {
                data.Syochi += $" ({suuryou}回)";
            }
        }

        private static void GetSyochiIY(ref float tensuu, OleDbDataReader reader, KarteRecord data, int suuryou)
        {
            data.Syochi = $"{reader["F5"]} ({suuryou} {reader["F10"]})";

            int kingakuSikibetsu = int.Parse(reader["F11"].ToString());

            // UNDONE マスター金額識別ハードコード
            if (kingakuSikibetsu == 1)
            {
                tensuu = OmoEReceLib.RecordIY.CalcTensuu(float.Parse(reader["F12"].ToString())) * suuryou;
            }
            else
            {
                Console.WriteLine($"点数識別　金額以外 ({data})");
            }
        }

        private static void GetSyochiTO(ref float tensuu, OleDbDataReader reader, KarteRecord data, int suuryou)
        {
            data.Syochi = $"{reader["F5"]} ({suuryou} {reader["F10"]})";

            int kingakuSikibetsu = int.Parse(reader["F11"].ToString());

            // UNDONE マスター金額識別ハードコード
            if (kingakuSikibetsu == 1 || kingakuSikibetsu == 2 || kingakuSikibetsu == 4)
            {
                tensuu = OmoEReceLib.RecordTO.CalcTensuu(float.Parse(reader["F12"].ToString())) * suuryou;
            }
            else
            {
                Console.WriteLine($"点数識別　金額以外 ({data})");
            }
        }


        public static DbConnectionCollection ConnectMaster()
        {
            _csvFileNames = new Dictionary<MasterType, string>();
            _dbConnections = new DbConnectionCollection();

            // TODO ハードコード｛　ファイルパス、CSV読み込み・SELECT部分　｝
            var paths = new Dictionary<MasterType, string>();
            paths.Add(
                MasterType.SS,
                @"マスターファイル\歯科診療行為マスター\基本テーブル\h_20130125.csv");
            paths.Add(
                MasterType.SI,
                @"マスターファイル\医科診療行為マスター\s_20130129.csv");
            paths.Add(
                MasterType.IY,
                @"マスターファイル\医薬品マスター\y_20121214.csv");
            paths.Add(
                MasterType.TO,
                @"マスターファイル\特定器材マスター\t_20121228.csv");

            foreach (var kvset in paths)
            {
                var fi_source = new FileInfo(Path.Combine(
                    global::OmoSeitokuEreceipt.Properties.Settings.Default.EReceFolder, kvset.Value));
                var fi = new FileInfo(Path.GetTempFileName());

                // マスターを作業用にコピー
                // TODO このへんの処理　DbConnectionCollection　に移動したい
                File.Delete(fi.FullName);

                File.Copy(fi_source.FullName, fi.FullName);

                string csvDir = fi.DirectoryName;

                _csvFileNames.Add(kvset.Key, fi.Name);

                string conString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={csvDir};Extended Properties=\"text;HDR=No;FMT=Delimited\"";

                var db = new OleDbConnection(conString);

                _dbConnections.Add(kvset.Key, db);

                db.Open();
            }

            _dbConnections._csvFileNames = _csvFileNames;

            return _dbConnections;
        }
        private static Dictionary<MasterType, string> _csvFileNames;
        private static DbConnectionCollection _dbConnections;

        public sealed class DbConnectionCollection : Dictionary<MasterType, OleDbConnection>, IDisposable
        {

            // UNDONE _csvFileNames とか こっちに移動したい
            public Dictionary<MasterType, string> _csvFileNames;

            //public void Add(MasterType masterType, string csvPath)
            //{
            //    _csvFileNames.Add(masterType, csvPath
            //}

            //public string GetCsvFileName(MasterType masterType)
            //{
            //    return _csvFileNames[masterType];
            //}

            public void Dispose()
            {
                foreach (var item in this)
                {
                    try
                    {
                        item.Value.Close();
                        item.Value.Dispose();

                        // 作業用にコピーしたマスターファイルを削除
                        File.Delete(_csvFileNames[item.Key]);

                    }
                    catch
                    {
                    }
                }
            }
        }
        public enum MasterType
        {
            SS,
            SI,
            IY,
            TO,
            CO
        }



        public enum 診療統合種別
        {
            統合なし,
            初診期間別,
            全期間,
        }


        // UNDONE カルテレコードアイテム 一時的に使用
        public sealed class KarteRecord
        {
            public bool IsEmpty
            {
                get
                {
                    return string.IsNullOrEmpty(Syochi) && string.IsNullOrEmpty(Tensuu) && TensuuNum == 0;
                }
            }

            /// <summary>
            /// 処置欄に表示する文字列。
            /// </summary>
            public string Syochi;
            /// <summary>
            /// 点数欄に表示する文字列。
            /// </summary>
            public string Tensuu;
            /// <summary>
            /// 計算に使用する点数。
            /// </summary>
            public int TensuuNum;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="syochi">処置欄に表示する文字列。</param>
            /// <param name="tensuu">点数欄に表示する文字列。</param>
            /// <param name="tensuuNum">計算に使用する点数。</param>
            public KarteRecord(string syochi, string tensuu, int tensuuNum)
            {
                this.Syochi = syochi;
                this.Tensuu = tensuu;
                this.TensuuNum = tensuuNum;
            }


            /// <summary>
            /// コンストラクタ
            /// </summary>
            public KarteRecord()
                : this("", "", 0)
            {
            }
        }
    }
}