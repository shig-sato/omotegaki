#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

// 患者情報
namespace omotegaki_xml.Libs.Yahara.Entities.PatDataEntities
{
    public class PatData
    {
        public string 内部患者番号 { get; }

        public string カルテ番号 => 内部患者番号;

        /// <summary>半角</summary>
        public string? カナ氏名 { get; set; }

        public string? 漢字氏名 { get; set; }

        /// <summary>"男" or "女"</summary>
        public string? 性別 { get; set; }

        /// <summary>"yyyy/mm/dd"</summary>
        public string? 生年月日 { get; set; }

        /// <summary>半角ハイフンあり</summary>
        public string? 自宅郵便番号 { get; set; }

        public string? 自宅住所 { get; set; }

        /// <summary>半角ハイフンあり</summary>
        public string? 自宅電話番号 { get; set; }

        /// <summary>半角ハイフンあり</summary>
        public string? 連絡先電話番号 { get; set; }

        /// <summary>null</summary>
        public string? 職業 { get; set; }

        public string? 主担当医師 { get; set; }

        /// <summary>"yyyy/mm/dd"</summary>
        public string? 最終来院日 { get; set; }

        /// <summary>半角</summary>
        public string? 保険者番号 { get; set; }

        /// <summary>全角</summary>
        public string? 被保険者記号 { get; set; }

        /// <summary>半角</summary>
        public string? 被保険者番号 { get; set; }

        /// <summary>半角</summary>
        public string? 被保険者枝番 { get; set; }

        /// <summary>"本人" or "家族"</summary>
        public string? 被保険者との続柄 { get; set; }

        /// <summary>全角</summary>
        public string? 被保険者氏名 { get; set; }

        /// <summary>null</summary>
        public string? 医療保険詳細 { get; set; }

        /// <summary>null</summary>
        public string? 職務上の事由 { get; set; }

        /// <summary>"yyyy/mm/dd"</summary>
        public string? 保険有効期限 { get; set; }

        /// <summary>"yyyy/mm/dd"</summary>
        public string? 資格取得日 { get; set; }

        /// <summary>半角</summary>
        public string? 公費負担者番号 { get; set; }

        /// <summary>半角</summary>
        public string? 公費受給者番号 { get; set; }

        /// <summary>"yyyy/mm/dd"</summary>
        public string? 公費有効期限 { get; set; }

        /// <summary>null</summary>
        public string? 特殊保険番号 { get; set; }

        /// <summary>
        /// 特記事項： null または 、 前期高齢者 一般 、 前期高齢者 一般 2 割 、 前期高齢者 一般 低 I" 、 前期高齢者 一般 低 I 2 割 、
        /// 前期高齢者 一般 低 II" 、 前期高齢者 一般 低 II 2 割 、 前期高齢者 一定以上 、 後期高齢者 一般 、
        /// 後期高齢者 一般 2 割 、 後期高齢者 一般 低 I" 、 後期高齢者 一般 低 II" 、 後期高齢者 一定以上
        /// </summary>
        public string? 特記事項 { get; set; }

        /// <summary>null</summary>
        public string? 保険未収過剰金 { get; set; }
        /// <summary>null</summary>
        public string? 自費未収過剰金 { get; set; }
        /// <summary>null</summary>
        public string? 物販未収過剰金 { get; set; }
        /// <summary>null</summary>
        public string? 介護保険者番号 { get; set; }
        /// <summary>null</summary>
        public string? 介護被保険者番号 { get; set; }
        /// <summary>null</summary>
        public string? 介護公費負担者番号 { get; set; }
        /// <summary>null</summary>
        public string? 介護公費受給者番号 { get; set; }
        /// <summary>null</summary>
        public string? 利用者負担割合 { get; set; }
        /// <summary>null</summary>
        public string? 要介護状態区分 { get; set; }
        /// <summary>"yyyy/mm/dd"</summary>
        public string? 介護認定開始日 { get; set; }
        /// <summary>"yyyy/mm/dd"</summary>
        public string? 介護認定終了日 { get; set; }
        /// <summary>null</summary>
        public string? 区分変更申請月 { get; set; }

        public PatData(string karteNo)
        {
            内部患者番号 = karteNo ?? throw new ArgumentNullException(nameof(karteNo));
        }

        private PatData()
        {
        }

        /// <summary>
        /// CSVデータの1行分を作成。改行文字は含まない。
        /// </summary>
        public string ToCSVRow()
        {
            var values = new[]
            {
                内部患者番号,
                カルテ番号,
                カナ氏名,
                漢字氏名,
                性別,
                生年月日,
                自宅郵便番号,
                自宅住所,
                自宅電話番号,
                連絡先電話番号,
                職業,
                主担当医師,
                最終来院日,
                保険者番号,
                被保険者記号,
                被保険者番号,
                被保険者枝番,
                被保険者との続柄,
                被保険者氏名,
                医療保険詳細,
                職務上の事由,
                保険有効期限,
                資格取得日,
                公費負担者番号,
                公費受給者番号,
                公費有効期限,
                特殊保険番号,
                特記事項,
                保険未収過剰金,
                自費未収過剰金,
                物販未収過剰金,
                介護保険者番号,
                介護被保険者番号,
                介護公費負担者番号,
                介護公費受給者番号,
                利用者負担割合,
                要介護状態区分,
                介護認定開始日,
                介護認定終了日,
                区分変更申請月,
            };

            return string.Join(",", values);
        }
    }
}
