using System;
using System.Collections.Generic;
using System.Text;

namespace OmoEReceLib
{

    /// <see cref="ToERCode(this ER_審査支払機関 e)"/>
    public enum ER_審査支払機関
    {
        社会保険診療報酬支払基金 = 1,
        国民健康保険団体連合会 = 2,
    }


    /// <see cref="ToERCode(this ER_都道府県 e)"/>
    public enum ER_都道府県
    {
        北海道 = 1,
        青森 = 2,
        岩手 = 3,
        宮城 = 4,
        秋田 = 5,
        山形 = 6,
        福島 = 7,
        茨城 = 8,
        栃木 = 9,
        群馬 = 10,
        埼玉 = 11,
        千葉 = 12,
        東京 = 13,
        神奈川 = 14,
        新潟 = 15,
        富山 = 16,
        石川 = 17,
        福井 = 18,
        山梨 = 19,
        長野 = 20,
        岐阜 = 21,
        静岡 = 22,
        愛知 = 23,
        三重 = 24,
        滋賀 = 25,
        京都 = 26,
        大阪 = 27,
        兵庫 = 28,
        奈良 = 29,
        和歌山 = 30,
        鳥取 = 31,
        島根 = 32,
        岡山 = 33,
        広島 = 34,
        山口 = 35,
        徳島 = 36,
        香川 = 37,
        愛媛 = 38,
        高知 = 39,
        福岡 = 40,
        佐賀 = 41,
        長崎 = 42,
        熊本 = 43,
        大分 = 44,
        宮崎 = 45,
        鹿児島 = 46,
        沖縄 = 47,
    }



    /// <see cref="ToERCode(this ER_点数表 e)"/>
    public enum ER_点数表
    {
        医科 = 1,
        歯科 = 3,
        調剤 = 4,
    }



    /// <see cref="ToERCode(this ER_年号区分 e)"/>
    public enum ER_年号区分
    {
        明治 = 1,
        大正 = 2,
        昭和 = 3,
        平成 = 4,
        令和 = 5,
    }

    /// <see cref="ToERCode(this ER_施設基準届出 e)"/>
    public enum ER_施設基準届出
    {
        /// <summary>
        /// クラウン・ブリッジ維持管理料
        /// </summary>
        補管 = 1,

        /// <summary>
        /// 在宅療養支援歯科診療所
        /// </summary>
        歯援診 = 2,

        /// <summary>
        /// 歯科外来診療環境体制加算
        /// </summary>
        外来環 = 3,

        /// <summary>
        /// う蝕無痛窩洞形成加算
        /// </summary>
        う蝕無痛 = 4,

        /// <summary>
        /// 歯周組織再生誘導手術
        /// </summary>
        GTR = 5,

        /// <summary>
        /// 歯科治療総合医療管理料
        /// </summary>
        医管 = 6,

        /// <summary>
        /// 在宅患者歯科治療総合医療管理料
        /// </summary>
        在歯管 = 7,

        /// <summary>
        /// 障害者歯科医療連携加算
        /// </summary>
        障連 = 8,

        /// <summary>
        /// 手術時歯根面レーザー応用加算
        /// </summary>
        手術歯根 = 9,

        /// <summary>
        /// 歯科技工加算
        /// </summary>
        歯技工 = 10,

        /// <summary>
        /// 明細書発行体制等加算
        /// </summary>
        明細 = 11,
    }

    /// <summary>
    /// タグで囲われているもの以外は各審査支払機関に共通して使用できる。
    /// 国民健康保険団体連合会 の場合、"医保"を"国保"に読み換える。
    /// </summary>
    public enum ER_レセプト種別_負担者
    {
        医保単独 = 11,
        医保と公費1種 = 12,
        医保と公費2種 = 13,
        医保と公費3種 = 14,
        医保と公費4種 = 15,

        // <社会保険診療報酬支払基金>
        公費単独 = 21,
        公費2種 = 22,
        公費3種 = 23,
        公費4種 = 24,
        // </社会保険診療報酬支払基金>

        高齢単独 = 31,
        高齢と公費1種 = 32,
        高齢と公費2種 = 33,
        高齢と公費3種 = 34,
        高齢と公費4種 = 35,

        // <国民健康保険団体連合会>
        退職単独 = 41,
        退職と公費1種 = 42,
        退職と公費2種 = 43,
        退職と公費3種 = 44,
        退職と公費4種 = 45,
        // </国民健康保険団体連合会>
    }

    /// <summary>
    /// タグで囲われているもの以外は各審査支払機関に共通して使用できる。
    /// 国民健康保険団体連合会 の場合、"医保"を"国保"に読み換える。
    /// </summary>
    public enum ER_レセプト種別_本人家族入院区分
    {
        本人_入 = 1, //公費単独_入院
        本人_外 = 2, //公費単独_入院外
        未就学_入 = 3,
        未就学_外 = 4,
        家族_入 = 5,
        家族_外 = 6,
        高齢一般_低所得_入 = 7,
        高齢一般_低所得_外 = 8,
        /// <summary>
        /// 高齢受給者7割
        /// </summary>
        高齢受給者_入 = 9,
        /// <summary>
        /// 高齢受給者7割
        /// </summary>
        高齢受給者_外 = 0,
    }

    /// <see cref="ToERCode(this ER_男女区分 e)"/>
    public enum ER_男女区分
    {
        男 = 1,
        女 = 2,
    }

    /// <see cref="ToERCode(this ER_転帰区分 e)"/>
    public enum ER_転帰区分
    {
        治ゆ_死亡_中止以外 = 1,
        治ゆ = 2,
        死亡 = 3,
        中止_転医 = 4,
    }

    /// <see cref="ToERCode(this ER_病棟区分 e)"/>
    public enum ER_病棟区分
    {
        精神 = 1,
        結核 = 2,
        療養 = 7,
    }

    /// <summary>
    /// 一部負担金・食事療養費・生活療養費標準負担額区分
    /// </summary>
    /// <see cref="ToERCode(this ER_標準負担額区分 e)"/>
    public enum ER_標準負担額区分
    {
        /// <summary>
        /// 入院時負担金額又は外来時一部負担金額並びに
        /// 食事療養又は生活療養に係る標準負担額について、
        /// 限度額適用・標準負担額減額認定証の交付を受けている者
        /// （入院日数が９０日以下の者）
        /// </summary>
        低所得者Ⅱ_1 = 1,

        /// <summary>
        /// 入院時負担金額又は食事療養又は生活療養に係る標準負担額について、
        /// 限度額適用・標準負担額減額認定証の交付を受けている者
        /// （入院日数が９０日を超える者）
        /// </summary>
        低所得者Ⅱ_2 = 2,

        /// <summary>
        /// 入院時負担金額又は外来時一部負担金額並びに
        /// 食事療養又は生活療養に係る標準負担額について､
        /// 限度額適用・標準負担額減額認定証の交付を受けている者
        /// </summary>
        低所得者Ⅰ_1 = 3,

        /// <summary>
        /// 入院時負担金額又は外来時一部負担金額並びに
        /// 食事療養又は生活療養に係る標準負担額について､
        /// 限度額適用・標準負担額減額認定証の交付を受けている者であって､
        /// 老齢福祉年金を受給している者
        /// </summary>
        低所得者Ⅰ_2 = 4,
    }

    /// <summary>
    /// レセプト特記事項
    /// </summary>
    /// <see cref="ToERCode(this ER_レセプト特記事項 e)"/>
    /// <see cref="ToERCode(this List<ER_レセプト特記事項> e)"/>
    /// <example><![CDATA[
    ///     List<ER_レセプト特記事項> a = new List<ER_レセプト特記事項>();
    ///     a.Add(ER_レセプト特記事項.器治); // 12
    ///     a.Add(ER_レセプト特記事項.一般); // 18
    ///     a.Add(ER_レセプト特記事項.後保); // 4
    ///     
    ///     write(a[0].ToERCode()); // "12"
    ///     write(a[1].ToERCode()); // "18"
    ///     write(a[2].ToERCode()); // "04"
    ///     write(a.ToERCode());    // "041218"
    /// ]]></example>
    public enum ER_レセプト特記事項
    {
        公 = 1,
        長 = 2,
        長処 = 3,
        後保 = 4,
        老併 = 7,
        老健 = 8,
        施 = 9,
        第三 = 10,
        薬治 = 11,
        器治 = 12,
        先進 = 13,
        制超 = 14,
        経過 = 15,
        長2 = 16,
        上位 = 17,
        一般 = 18,
        低所 = 19,
        二割 = 20,
        高半 = 21,
        多上 = 22,
        多一 = 23,
        多低 = 24,
        出産 = 25,
        百分の五十 = 40,
    }

    /// <see cref="ToERCode(this ER_未来院請求 e)"/>
    public enum ER_未来院請求
    {
        /// <summary>
        /// 患者が理由なく来院しなくなった場合､ 患者の意思により治療を中止した場合､患者が死亡した場合であって
        /// 試適又は装着の予定日より1月待った上で請求を行う場合｡
        /// </summary>
        事由1 = 1,
    }

    /// <see cref="ToERCode(this ER_職務上の事由 e)"/>
    public enum ER_職務上の事由
    {
        職務上 = 1,
        下船後３月以内 = 2,
        通勤災害 = 3,
    }

    /// <see cref="ToERCode(this ER_減免区分 e)"/>
    public enum ER_減免区分
    {
        減額 = 1,
        免除 = 2,
        支払猶予 = 3,
    }

    /// <see cref="ToERCode(this ER_状態 e)"/>
    public enum ER_状態
    {
        現存歯 = 0,
        /// <summary>
        /// 部を示す場合に使用
        /// </summary>
        部 = 1,
        欠損歯 = 2,
        支台歯 = 3,
        /// <summary>
        /// 根
        /// </summary>
        分割抜歯支台 = 4, // 任意コード
        便宜抜髄支台歯 = 5,
        残根 = 6, // 任意コード
        部インプラント = 7, // 任意コード
        部近心隙 = 8,
        近心位に存在 = 9, // 任意コード
    }

    /// <see cref="ToERCode(this ER_部分 e)"/>
    public enum ER_部分
    {
        部分指定なし = 0,
        遠心頬側根 = 1,
        近心頬側根 = 2,
        近心頬側根及び遠心頬側根 = 3,
        舌側_口蓋_根 = 4,
        舌側_口蓋_根及び遠心頬側根 = 5,
        舌側_口蓋_根及び近心頬側根 = 6,
        遠心根 = 7,
        近心根 = 8,
    }

    /// <see cref="ToERCode(this ER_病態移行 e)"/>
    public enum ER_病態移行
    {
        病態移行前 = 1,
        病態移行後 = 2,
    }

    /// <see cref="ToERCode(this ER_主傷病 e)"/>
    public enum ER_主傷病
    {
        主傷病 = 1,
    }

    /// <see cref="ToERCode(this ER_診療識別 e)"/>
    public enum ER_診療識別
    {
        /// <summary>
        /// 欄: 初診<br/>
        /// 記録可能なレコード: SS
        /// </summary>
        初診 = 11,

        /// <summary>
        /// 欄: 再診<br/>
        /// 記録可能なレコード: SS
        /// </summary>
        再診 = 12,

        /// <summary>
        /// 欄: 歯管, 義管, 歯清, 衛実, F局, F洗, 医管, 管理その他<br/>
        /// 記録可能なレコード: SS
        /// </summary>
        管理 = 13,

        /// <summary>
        /// 欄: 投薬_注射, 調, 処方, 情, 処, 注<br/>
        /// 記録可能なレコード: SS, IY, TO
        /// </summary>
        投薬_注射 = 21,

        /// <summary>
        /// 欄: 全顎, 標, パ, 模, 写, S培, 顎運動, 平測,<br/>
        ///     EMR, 基本検査, 精密検査, X線_検査その他<br/>
        /// 記録可能なレコード: SS, SI, IY, TO, CO
        /// </summary>
        X線_検査 = 31,

        /// <summary>
        /// 欄: う蝕, 覆罩, 充塞, 除去, 知覚過敏, 咬調, 抜髄, 感染根処,<br/>
        ///     根管貼薬, 根充, 抜髄即充, 感根即充, 加圧根充, 生切, 失切<br/>
        /// 記録可能なレコード: SS
        /// </summary>
        処置_手術1 = 41,

        /// <summary>
        /// 欄: SC, SRP, Pcur, SPT, P処<br/>
        /// 記録可能なレコード: SS
        /// </summary>
        処置_手術2 = 42,

        /// <summary>
        /// 欄: 抜歯, 切開<br/>
        /// 記録可能なレコード: SS
        /// </summary>
        処置_手術3 = 43,

        /// <summary>
        /// 欄: 処置_手術その他, 特定薬剤<br/>
        /// 記録可能なレコード: SS, SI, IY, TO, CO
        /// </summary>
        処置_手術その他 = 44,

        /// <summary>
        /// 欄: 伝麻, 浸麻, 麻酔その他<br/>
        /// 記録可能なレコード: SS, SI, IY, TO, CO
        /// </summary>
        麻酔 = 54,

        /// <summary>
        /// 欄: 補診, 維持管理, 印象, 歯冠形成, 充形, 修形, 咬合, 試適, 支台築造<br/>
        /// 記録可能なレコード: SS
        /// </summary>
        修復_補綴1 = 61,

        /// <summary>
        /// 欄: 鋳造歯冠修復, TEK, 硬ジ, ジ, 乳, 修理, 装着, 装着材料, 充填, 充填材料, リテイナー, 仮着<br/>
        /// 記録可能なレコード: SS
        /// </summary>
        修復_補綴2 = 62,

        /// <summary>
        /// 欄: ポンティック, Br装着, バー, 有床義歯, 床裏装, 鋳造鉤, 線鉤, 床修理, 人工歯<br/>
        /// 記録可能なレコード: SS, TO
        /// </summary>
        修復_補綴3 = 63,

        /// <summary>
        /// 欄: 歯冠修復及び欠損補綴その他<br/>
        /// 記録可能なレコード: SS, IY, TO, CO
        /// </summary>
        修復_補綴その他 = 64,

        /// <summary>
        /// 欄: 全体のその他<br/>
        /// 記録可能なレコード: SS, SI, IY, TO, CO
        /// </summary>
        全体のその他 = 80,

        /// <summary>
        /// 欄: 摘要<br/>
        /// 記録可能なレコード: CO
        /// </summary>
        摘要 = 99,
    }

    /// <summary>
    /// 請求点数のある管掌（法別）は true を設定する。
    /// 国民健康保険、退職者医療又は後期高齢者医療については、医保を国民健康保険、退職者医療又は後期高齢者医療と読み替える。
    /// </summary>
    public struct ER_負担区分
    {
        public bool 医保;
        public bool 公費1;
        public bool 公費2;
        public bool 公費3;
        public bool 公費4;

        /// <summary>
        /// 負担区分コードへ変換する。
        /// </summary>
        /// <seealso cref="オンライン又は光ディスク等による請求に係る記録条件仕様（歯科用）"/>
        /// <seealso cref="別表２１ 負担区分コード [医保と公費又は公費と公費の併用]"/>
        /// <returns>負担区分コード</returns>
        public string ToERCode()
        {
            bool ih = this.医保;
            bool k1 = this.公費1;
            bool k2 = this.公費2;
            bool k3 = this.公費3;
            bool k4 = this.公費4;

            #region 1者

            if (ih && !k1 && !k2 && !k3 && !k4)
                return "1";

            if (!ih && k1 && !k2 && !k3 && !k4)
                return "5";

            if (!ih && !k1 && k2 && !k3 && !k4)
                return "6";

            if (!ih && !k1 && !k2 && k3 && !k4)
                return "B";

            if (!ih && !k1 && !k2 && !k3 && k4)
                return "C";

            #endregion

            #region 2者

            if (ih && k1 && !k2 && !k3 && !k4)
                return "2";

            if (ih && !k1 && k2 && !k3 && !k4)
                return "3";

            if (ih && !k1 && !k2 && k3 && !k4)
                return "E";

            if (ih && !k1 && !k2 && !k3 && k4)
                return "G";

            if (!ih && k1 && k2 && !k3 && !k4)
                return "7";

            if (!ih && k1 && !k2 && k3 && !k4)
                return "H";

            if (!ih && k1 && !k2 && !k3 && k4)
                return "I";

            if (!ih && !k1 && k2 && k3 && !k4)
                return "J";

            if (!ih && !k1 && k2 && !k3 && k4)
                return "K";

            if (!ih && !k1 && !k2 && k3 && k4)
                return "L";

            #endregion

            #region 3者

            if (ih && k1 && k2 && !k3 && !k4)
                return "4";

            if (ih && k1 && !k2 && k3 && !k4)
                return "M";

            if (ih && k1 && !k2 && !k3 && k4)
                return "N";

            if (ih && !k1 && k2 && k3 && !k4)
                return "O";

            if (ih && !k1 && k2 && !k3 && k4)
                return "P";

            if (ih && !k1 && !k2 && k3 && k4)
                return "Q";

            if (!ih && k1 && k2 && k3 && !k4)
                return "R";

            if (!ih && k1 && k2 && !k3 && k4)
                return "S";

            if (!ih && k1 && !k2 && k3 && k4)
                return "T";

            if (!ih && !k1 && k2 && k3 && k4)
                return "U";

            #endregion

            #region 4者

            if (ih && k1 && k2 && k3 && !k4)
                return "V";

            if (ih && k1 && k2 && !k3 && k4)
                return "W";

            if (ih && k1 && !k2 && k3 && k4)
                return "X";

            if (ih && !k1 && k2 && k3 && k4)
                return "Y";

            if (!ih && k1 && k2 && k3 && k4)
                return "Z";

            #endregion

            #region 5者

            if (ih && k1 && k2 && k3 && k4)
                return "9";

            #endregion

            throw new Exception("ERCore.ER_負担区分.ToERCode");
        }
    }

    /// <see cref="ToERCode(this ER_医薬品区分 e)"/>
    public enum ER_医薬品区分
    {
        入院外_内服 = 1,
        入院外_屯服 = 2,
        入院外_外用 = 3,
        入院外_注射 = 4,
        /// <summary>
        /// 歯科麻酔薬剤・特定薬剤 以外
        /// </summary>
        麻酔_処置_手術等で使用する薬剤 = 5,
        歯科麻酔薬剤 = 6,
        特定薬剤 = 7,
    }

    /// <see cref="ToERCode(this ER_単位 e)"/>
    /// <see cref="To特定器材単位コード(this ER_単位 e)"/>
    public enum ER_単位
    {
        分 = 1,
        回 = 2,
        種 = 3,
        箱 = 4,
        巻 = 5,
        枚 = 6,
        本 = 7,
        組 = 8,
        セット = 9,
        個 = 10,
        裂 = 11,
        方向 = 12,
        トローチ = 13,
        アンプル = 14,
        カプセル = 15,
        錠 = 16,
        丸 = 17,
        包 = 18,
        瓶 = 19,
        袋 = 20,
        瓶_袋 = 21,
        管 = 22,
        シリンジ = 23,
        回分 = 24,
        テスト分 = 25,
        ガラス筒 = 26,
        桿錠 = 27,
        単位 = 28,
        万単位 = 29,
        フィート = 30,
        滴 = 31,
        mg = 32,
        g = 33,
        Kg = 34,
        cc = 35,
        mL = 36,
        l = 37,
        mLV = 38,
        バイアル = 39,
        cm = 40,
        cm2 = 41,
        m = 42,
        μCi = 43,
        mCi = 44,
        μg = 45,
        管_瓶 = 46,
        筒 = 47,
        GBq = 48,
        MBq = 49,
        KBq = 50,
        キット = 51,
        国際単位 = 52,
        患者当り = 53,
        気圧 = 54,
        缶 = 55,
        手術当り = 56,
        容器 = 57,
        mL_g = 58,
        ブリスター = 59,
        シート = 60,
        //------------------- ここまで 特定器材単位コード と併用。
        //------------------- 変更する場合 {Is特定器材単位コード(this ER_単位 e)} も修正する。
        分画 = 101,
        染色 = 102,
        種類 = 103,
        株 = 104,
        菌株 = 105,
        照射 = 106,
        臓器 = 107,
        件 = 108,
        部位 = 109,
        肢 = 110,
        局所 = 111,
        種目 = 112,
        スキャン = 113,
        コマ = 114,
        処理 = 115,
        指 = 116,
        歯 = 117,
        面 = 118,
        側 = 119,
        個所 = 120,
        日 = 121,
        椎間 = 122,
        筋 = 123,
        菌種 = 124,
        項目 = 125,
        箇所 = 126,
        椎弓 = 127,
        食 = 128,
        根管 = 129,
        三分の一顎 = 130,
        月 = 131,
        入院初日 = 132,
        入院中 = 133,
        退院時 = 134,
        初回 = 135,
        口腔 = 136,
        顎 = 137,
        週 = 138,
        窩洞 = 139,
    }

    /// <see cref="ToERCode(this ER_特定器材加算等 e)"/>
    public enum ER_特定器材加算等
    {
        /// <summary>
        /// (1.3)
        /// </summary>
        酸素補正率 = 770020070,
        /// <summary>
        /// 要 治療に係る気圧数
        /// </summary>
        高圧酸素治療加算 = 770030070,
        フィルム料_6歳未満乳幼児加算 = 799990070,
    }

    /// <see cref="ToERCode(this ER_症状詳記区分 e)"/>
    public sealed class ER_症状詳記区分
    {
        public static readonly string[] CodeNames = new string[] {
            "療養の給付及び公費負担医療に関する費用の請求に関する省令第1条第2項の規定に基づく診療報酬明細書の場合",
            "治験概要の添付が必要な診療報酬明細書の場合",
            "疾患別リハビリテーションに係る治療継続の理由等の記載の必要な診療報酬明細書の場合",
            "廃用症候群に係る評価表",
            "上記以外の診療報酬明細書の場合 "
        };

        /// <summary>
        /// Key: コード, Value: 内容
        /// </summary>
        public static readonly Dictionary<string, string> Codes = new Dictionary<string, string>();

        /// <summary>
        /// Key: コード, Value: 対応する CODE_NAMES のインデックス
        /// </summary>
        private static readonly Dictionary<string, int> CodeToName = new Dictionary<string, int>();


        static ER_症状詳記区分()
        {

            // コードと内容の辞書
            Dictionary<string, string> cs = Codes;
            cs["01"] = "患者の主たる疾患（合併症を含む。）の診断根拠となった臨床症状";
            cs["02"] = "患者の主たる疾患（合併症を含む。）の診断根拠となった臨床症状の診察・検査所見";
            cs["03"] = "主な治療行為（手術、処置、薬物治療等）の必要性";
            cs["04"] = "主な治療行為（手術、処置、薬物治療等）の経過";
            cs["05"] = "診療報酬明細書の合計点数が100万点以上の場合における薬剤に係る症状等";
            cs["06"] = "診療報酬明細書の合計点数が100万点以上の場合における処置に係る症状等";
            cs["07"] = "その他";
            cs["50"] = "厚生労働大臣の定める選定療養第7号の規定に基づく薬事法に規定する治験に係る治験概要";
            cs["51"] = "疾患別リハビリテーション（心大血管疾患、脳血管疾患等、運動器及び呼吸器）に係る治療継続の理由等の記載";
            cs["52"] = "廃用症候群に該当するものとして脳血管疾患等リハビリテーション料を算定する場合の、廃用をもたらすに至った要因等の記載";
            cs["90"] = "療養の給付及び公費負担医療に関する費用の請求に関する省令第1条第2項の規定に基づく診療報酬明細書以外の診療報酬明細書の症状詳記";

            //コードとコード名の辞書
            Dictionary<string, int> ctn = CodeToName;
            ctn["01"] = 0;
            ctn["02"] = 0;
            ctn["03"] = 0;
            ctn["04"] = 0;
            ctn["05"] = 0;
            ctn["06"] = 0;
            ctn["07"] = 0;
            ctn["50"] = 1;
            ctn["51"] = 2;
            ctn["52"] = 3;
            ctn["90"] = 4;
        }


        private readonly string _code;

        public ER_症状詳記区分(string code)
        {
            if (!Codes.ContainsKey(code))
                throw new Exception("存在しない症状詳記区分コード(" + code + ")が指定されました。");

            _code = code;
        }

        public string Code
        {
            get { return _code; }
        }

        public string CodeName
        {
            get { return CodeNames[CodeToName[_code]]; }
        }

        public string Contents
        {
            get { return Codes[_code]; }
        }
    }




    public static class ERCoreExtensions
    {
        public static string ToERCode(this ER_審査支払機関 e)
        {
            return ((int)e).ToString();
        }

        public static string ToERCode(this ER_都道府県 e)
        {
            return ((int)e).ToString().PadLeft(2, '0');
        }

        public static string ToERCode(this ER_点数表 e)
        {
            return ((int)e).ToString();
        }

        public static string ToERCode(this ER_年号区分 e)
        {
            return ((int)e).ToString();
        }

        public static string ToERCode(this ER_施設基準届出 e)
        {
            return ((int)e).ToString().PadLeft(2, '0');
        }



        // ここに ER_レセプト種別_***



        public static string ToERCode(this ER_男女区分 e)
        {
            return ((int)e).ToString();
        }

        public static string ToERCode(this ER_転帰区分 e)
        {
            return ((int)e).ToString();
        }

        public static string ToERCode(this ER_病棟区分 e)
        {
            return ((int)e).ToString().PadLeft(2, '0');
        }

        public static string ToERCode(this ER_標準負担額区分 e)
        {
            return ((int)e).ToString();
        }

        public static string ToERCode(this ER_レセプト特記事項 e)
        {
            return ((int)e).ToString().PadLeft(2, '0');
        }

        public static string ToERCode(this List<ER_レセプト特記事項> e)
        {
            ER_レセプト特記事項[] c = e.ToArray();
            //ER_レセプト特記事項[] c = (ER_レセプト特記事項[])e.Clone();

            Array.Sort<ER_レセプト特記事項>(c);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < c.Length; ++i)
            {
                sb.Append(c[i].ToERCode());
            }

            return sb.ToString();
        }

        public static string ToERCode(this ER_未来院請求 e)
        {
            return ((int)e).ToString().PadLeft(2, '0');
        }

        public static string ToERCode(this ER_職務上の事由 e)
        {
            return ((int)e).ToString();
        }

        public static string ToERCode(this ER_減免区分 e)
        {
            return ((int)e).ToString();
        }

        public static string ToERCode(this ER_状態 e)
        {
            return ((int)e).ToString();
        }

        public static void AppendERCode(this ER_状態 e, StringBuilder sb)
        {
            sb.Append((int)e);
        }

        public static string ToERCode(this ER_部分 e)
        {
            return ((int)e).ToString();
        }

        public static void AppendERCode(this ER_部分 e, StringBuilder sb)
        {
            sb.Append((int)e);
        }

        public static string ToERCode(this ER_病態移行 e)
        {
            return ((int)e).ToString();
        }

        public static string ToERCode(this ER_主傷病 e)
        {
            return ((int)e).ToString().PadLeft(2, '0');
        }

        public static string ToERCode(this ER_診療識別 e)
        {
            return ((int)e).ToString().PadLeft(2, '0');
        }

        public static string ToERCode(this ER_医薬品区分 e)
        {
            return ((int)e).ToString();
        }

        //UNDONE 仕様未確認 {ER_単位.ToERCode}
        public static string ToERCode(this ER_単位 e)
        {
            return ((int)e).ToString().PadLeft(3, '0');
        }

        public static string To特定器材単位コード(this ER_単位 e)
        {
            return ((int)e).ToString().PadLeft(3, '0');
        }

        public static bool Is特定器材単位コード(this ER_単位 e)
        {
            return (int)e <= (int)ER_単位.シート;
        }

        public static string ToERCode(this ER_特定器材加算等 e)
        {
            return ((int)e).ToString();
        }
    }


    public struct ER_保険者番号
    {
        public int 法別番号;
        public int 都道府県番号;
        public int 保険者別番号;
        public int 検証番号;
    }

    public struct ER_公費負担者番号
    {
        public int 法別番号;
        public int 都道府県番号;
        public int 実施機関番号;
        public int 検証番号;
    }
}
