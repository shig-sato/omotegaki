namespace OmoSeitokuEreceipt.SER
{


    public enum SER_保険種別2
    {
        単独 = 1,
        一併 = 2,
        二併 = 3
    }

    public enum SER_保険診療
    {
        保険診療のみ,
        自費診療のみ,
        保険診療と自費診療
    }

    public enum SER_未来院請求
    {
        通常請求のみ,
        未来院請求のみ,
        通常請求と未来院請求
    }




    /// <summary>
    /// SeitokuDBの列インデックス情報等
    /// </summary>
    public sealed class SDB
    {
        /*
         * 歯席DB
         */
        public const int HASEKI_COL_INDEX = 0;
        public const int HASEKI_COL_ERCODE = 6;

        /*
         * 病名DB
         */
        public const int BYOMEI_COL_内部処理病名 = 1;
        public const int BYOMEI_COL_レセプト病名 = 7;


        /*
         * 処置対応DB
         */
        public const int 処置対応DB_COL_カテゴリー = 0;
        public const int 処置対応DB_COL_処置番号 = 1;
    }
}
