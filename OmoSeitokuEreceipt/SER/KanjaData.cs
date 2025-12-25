using OmoEReceLib;
using OmoSeitoku.VB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace OmoSeitokuEreceipt.SER
{
    [Serializable]
    /// <summary>
    /// 診療録患者データレコード (GN88Kr)
    /// </summary>
    public class KanjaData
    {
        public const int SIZE = 202;

        public static readonly KanjaData Empty = new KanjaData();

        #region Constructor

        public KanjaData(BinaryReader input)
        {
            親番号 = VBRandomFile.ReadInt(input);
            孫番号 = VBRandomFile.ReadInt(input);
            氏名カナ = NormalizeString(VBRandomFile.ReadFixedLengthString(input, 14));
            氏名 = NormalizeString(VBRandomFile.ReadFixedLengthString(input, 24));
            (保険種別2, 性別) = Create保険種別2And性別(VBRandomFile.ReadInt(input).ToString());
            生年月日 = TryCatchKanjaDataProp(() => VBRandomFile.ReadDate(input), null, nameof(生年月日));
            本人家族区分 = Create本人家族区分(VBRandomFile.ReadInt(input));
            身障 = VBRandomFile.ReadInt(input) == 1;
            職務上の事由 = Create職務上の事由(VBRandomFile.ReadInt(input));
            保険者番号 = int.TryParse(VBRandomFile.ReadFixedLengthString(input, 8), out int num) ? num : 0;
            被保険者証_記号 = NormalizeString(VBRandomFile.ReadFixedLengthString(input, 24));
            被保険者証_番号 = NormalizeString(VBRandomFile.ReadFixedLengthString(input, 24));
            公費負担者番号 = int.TryParse(VBRandomFile.ReadFixedLengthString(input, 8), out num) ? num : 0;
            公費受給者番号 = int.TryParse(VBRandomFile.ReadFixedLengthString(input, 8), out num) ? num : 0;
            市町村番号 = int.TryParse(VBRandomFile.ReadFixedLengthString(input, 8), out num) ? num : 0;
            老人受給者番号 = NormalizeString(VBRandomFile.ReadFixedLengthString(input, 8));
            保険種別 = VBRandomFile.ReadInt(input);
            患者負担率 = VBRandomFile.ReadInt(input);
            保険給付率 = VBRandomFile.ReadInt(input);
            保険有効期限 = TryCatchKanjaDataProp(() => VBRandomFile.ReadDate(input), null, nameof(保険有効期限));
            保険証使用開始日 = TryCatchKanjaDataProp(() => VBRandomFile.ReadDate(input), null, nameof(保険証使用開始日));
            最終来院日 = TryCatchKanjaDataProp(() => VBRandomFile.ReadDate(input), null, nameof(最終来院日));
            保険診療開始日 = TryCatchKanjaDataProp(() => VBRandomFile.ReadDate(input), null, nameof(保険診療開始日));
            保険最終来院日 = TryCatchKanjaDataProp(() => VBRandomFile.ReadDate(input), null, nameof(保険最終来院日));
            担当医 = VBRandomFile.ReadInt(input);

            // 転帰
            //     10000 < tenki < 20000  右から1桁づつ4つ入ってくる。
            転記 = Create転記(VBRandomFile.ReadInt(input));
            郵便番号 = (int)VBRandomFile.ReadLong(input);
            long denwa = VBRandomFile.ReadLong(input);
            電話番号 = denwa == 0 ? null : denwa.ToString();
            住所番号 = VBRandomFile.ReadLong(input);
            住所 = NormalizeString(VBRandomFile.ReadFixedLengthString(input, 18));
        }

        public KanjaData(KanjaData source)
        {
            if (source is null) { throw new ArgumentNullException(nameof(source)); }

            親番号 = source.親番号;
            孫番号 = source.孫番号;
            氏名カナ = source.氏名カナ;
            氏名 = source.氏名;
            保険種別2 = source.保険種別2;
            性別 = source.性別;
            生年月日 = source.生年月日;
            本人家族区分 = source.本人家族区分;
            身障 = source.身障;
            職務上の事由 = source.職務上の事由;
            保険者番号 = source.保険者番号;
            被保険者証_記号 = source.被保険者証_記号;
            被保険者証_番号 = source.被保険者証_番号;
            公費負担者番号 = source.公費負担者番号;
            公費受給者番号 = source.公費受給者番号;
            市町村番号 = source.市町村番号;
            老人受給者番号 = source.老人受給者番号;
            保険種別 = source.保険種別;
            患者負担率 = source.患者負担率;
            保険給付率 = source.保険給付率;
            保険有効期限 = source.保険有効期限;
            保険証使用開始日 = source.保険証使用開始日;
            最終来院日 = source.最終来院日;
            保険診療開始日 = source.保険診療開始日;
            保険最終来院日 = source.保険最終来院日;
            担当医 = source.担当医;
            転記 = source.転記 is null ? null : new List<ER_転帰区分>(source.転記);
            郵便番号 = source.郵便番号;
            電話番号 = source.電話番号;
            住所番号 = source.住所番号;
            住所 = source.住所;
        }

        private KanjaData()
        {
        }

        #endregion Constructor

        #region Property

        public int? 親番号 { get; }
        public int? 孫番号 { get; }
        public string 氏名カナ { get; }
        public string 氏名 { get; }
        public SER_保険種別2? 保険種別2 { get; }
        public ER_男女区分? 性別 { get; }
        public DateTime? 生年月日 { get; }
        public ER_レセプト種別_本人家族入院区分? 本人家族区分 { get; }
        public bool? 身障 { get; }
        public ER_職務上の事由? 職務上の事由 { get; }
        public int? 保険者番号 { get; }
        public string 被保険者証_記号 { get; }
        public string 被保険者証_番号 { get; }
        public int? 公費負担者番号 { get; }
        public int? 公費受給者番号 { get; }
        public int? 市町村番号 { get; }
        public string 老人受給者番号 { get; }
        public int? 保険種別 { get; }
        public int? 患者負担率 { get; }
        public int? 保険給付率 { get; }
        public DateTime? 保険有効期限 { get; }
        public DateTime? 保険証使用開始日 { get; }
        public DateTime? 最終来院日 { get; }
        public DateTime? 保険診療開始日 { get; }
        public DateTime? 保険最終来院日 { get; }
        public int? 担当医 { get; }

        /// <summary>
        /// 保険変更前
        /// </summary>
        public List<ER_転帰区分> 転記 { get; }

        public int? 郵便番号 { get; }
        public string 電話番号 { get; }

        public long? 電話番号数値
        {
            get
            {
                if (string.IsNullOrEmpty(電話番号)) return 0;
                string s = 数字以外を削除(電話番号);
                if ((s.Length > 0) && (s[0] == '0')) s = '1' + s.Substring(1);
                return long.Parse(s);
            }
            //set
            //{
            //    string s = value.ToString();
            //    if (1000000000L <= value)
            //        //市外局番
            //        s = '0' + s.Substring(1);
            //    this.電話番号 = s;
            //}
        }

        /// <summary>
        /// 千葉県 = 1  青森県 = 34  印旛郡 = 678  八街市 = 457
        /// </summary>
        public long? 住所番号 { get; }

        public string 住所 { get; }

        #endregion Property

        #region Helper

        private static string 数字以外を削除(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            var sb = new StringBuilder(value);
            for (int i = sb.Length - 1; i >= 0; i--)
            {
                char c = sb[i];
                if (c < '0' || '9' < c)
                {
                    sb.Remove(i, 1);
                }
            }
            return sb.ToString();
        }

        private static T TryCatchKanjaDataProp<T>(Func<T> readFunc, T resultOnCatch, string propName)
        {
            try
            {
                return readFunc();
            }
            catch
            {
                Console.WriteLine($"例外: {typeof(KanjaData).Name}: {propName}の取得に失敗");
                return resultOnCatch;
            }
        }

        private static (SER_保険種別2?, ER_男女区分?) Create保険種別2And性別(string hoken2AndSex)
        {
            if (0 == hoken2AndSex.Length) return (null, null);

            if (hoken2AndSex.Length == 1) hoken2AndSex = ' ' + hoken2AndSex;

            SER_保険種別2? hoken;
            switch (hoken2AndSex[0])
            {
                case '1':
                    hoken = SER_保険種別2.単独;
                    break;

                case '2':
                    hoken = SER_保険種別2.一併;
                    break;

                case '3':
                    hoken = SER_保険種別2.二併;
                    break;

                default:
                    hoken = null;
                    //Debug.WriteLine($"[error: 1869cfe0] 未対応の {nameof(SER_保険種別2)}: " + hoken2AndSex[0]);
                    break;
            }

            ER_男女区分? sex;
            if (hoken2AndSex[1] == '1')
                sex = ER_男女区分.男;
            else if (hoken2AndSex[1] == '2')
                sex = ER_男女区分.女;
            else
            {
                sex = null;
                //Debug.WriteLine($"[error: ff356187] 未対応の {nameof(ER_男女区分)}: " + hoken2AndSex[1]);
            }

            return (hoken, sex);
        }

        private static ER_レセプト種別_本人家族入院区分? Create本人家族区分(int value)
        {
            switch (value)
            {
                case 1: return ER_レセプト種別_本人家族入院区分.本人_外;
                case 2: return ER_レセプト種別_本人家族入院区分.家族_外;
                case 3: return ER_レセプト種別_本人家族入院区分.未就学_外;
                case 4: return ER_レセプト種別_本人家族入院区分.高齢一般_低所得_外;
                case 5: return ER_レセプト種別_本人家族入院区分.高齢受給者_外;
                case 6: return ER_レセプト種別_本人家族入院区分.高齢一般_低所得_外;
                default: return null;
            }
        }

        private static ER_職務上の事由? Create職務上の事由(int value)
        {
            if (value == 1) return ER_職務上の事由.職務上;
            if (value == 2) return ER_職務上の事由.下船後３月以内;
            if (value == 3) return ER_職務上の事由.通勤災害;
            return null;
        }

        private static List<ER_転帰区分> Create転記(float tenki)
        {
            var res = new List<ER_転帰区分>();
            if (10000 < tenki)
            {
                while (10f <= tenki)
                {
                    int t = (int)tenki;
                    tenki *= 0.1f;

                    switch (t - (int)tenki * 10)
                    {
                        case 0:
                            break;

                        case 1:
                            res.Add(ER_転帰区分.治ゆ);
                            break;

                        case 2:
                            res.Add(ER_転帰区分.死亡);
                            break;

                        case 3:
                            res.Add(ER_転帰区分.中止_転医);
                            break;

                        default:
                            res.Add(ER_転帰区分.治ゆ_死亡_中止以外);
                            break;
                    }
                }
            }
            return res;
        }

        private static string NormalizeString(string str)
        {
            var s = str.Trim('\0', ' ', '　');
            return (s.Length == 0) || (s == "No accepted") ? null : s;
        }

        #endregion Helper

        /// <summary>
        /// 氏名かカナを取得する。
        /// </summary>
        /// <param name="kana">カナを優先するかどうか</param>
        /// <returns>氏名を優先する場合、氏名が空ではない場合は氏名、空の場合はカナ。</returns>
        public string Get氏名(bool kana)
        {
            string name = (kana ? 氏名カナ : 氏名)?.Trim();

            return string.IsNullOrEmpty(name)
                ? ((kana ? 氏名 : 氏名カナ)?.Trim())
                : name;
        }
    }
}