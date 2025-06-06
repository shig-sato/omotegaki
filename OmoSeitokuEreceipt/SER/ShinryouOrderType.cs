namespace OmoSeitokuEreceipt.SER
{
    public enum ShinryouOrderType
    {
        日付順 = 0,
        /// <summary>
        /// 右上8to1 → 左上1to8 → 右下8to1 → 左下1to8
        /// </summary>
        歯順A = 1,
        /// <summary>
        /// 右上8to1 → 左上1to8 → 左下8to1 → 右下1to8
        /// </summary>
        歯順B = 2,
    }
}
