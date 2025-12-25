#nullable enable

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using OmoSeitokuEreceipt.SER;
using omotegaki_xml.Libs.Yahara.Entities.PatDataEntities;

namespace omotegaki_xml.Libs.Yahara.Converters
{
    public static partial class KarteToPatDataConverter
    {
        public static PatData Convert(KarteId karteId, KarteData karte)
        {
            var patData = new PatData(karteNo: karteId.KarteNumber.ToString())
            {
                //Name = karte.氏名カナ?.Replace("　", " "),
                //KanjiName = karte.氏名?.Replace("　", " "),
                //Birthday = karte.生年月日?.ToString("yyyy/MM/dd"),
                //Sex = GetSex(karte),
                //郵便番号 = GetPostalCode(karte),
                //住所 = karte.住所,
                //連絡先電話番号 = karte.電話番号,
                //自宅電話番号 = karte.電話番号,
                //主担当医Object = null, // GetPrimaryDoctor(karte),
                //保険者 = CreateHokenshaList(karte),



                //カナ氏名 = karte.氏名カナ?.Replace("　", " "),
                //漢字氏名 = karte.氏名?.Replace(" ", "　"),
                //性別,
                //生年月日,
                //自宅郵便番号,
                //自宅住所,
                //自宅電話番号,
                //連絡先電話番号,
                //職業,
                //主担当医師,
                //最終来院日,
                //保険者番号,
                //被保険者記号,
                //被保険者番号,
                //被保険者枝番,
                //被保険者との続柄,
                //被保険者氏名,
                //医療保険詳細,
                //職務上の事由,
                //保険有効期限,
                //資格取得日,
                //公費負担者番号,
                //公費受給者番号,
                //公費有効期限,
                //特殊保険番号,
                //特記事項,
                //保険未収過剰金,
                //自費未収過剰金,
                //物販未収過剰金,
                //介護保険者番号,
                //介護被保険者番号,
                //介護公費負担者番号,
                //介護公費受給者番号,
                //利用者負担割合,
                //要介護状態区分,
                //介護認定開始日,
                //介護認定終了日,
                //区分変更申請月,
            };

            return patData;
        }
    }
}
