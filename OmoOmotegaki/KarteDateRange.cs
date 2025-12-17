using OmoSeitoku;

namespace OmoOmotegaki
{
    public struct KarteDateRange
    {
        /// <summary>
        /// 期間
        /// </summary>
        public DateRange2 Range;

        /// <summary>
        /// 開始日の診療期間の初診日から対象とする。
        /// </summary>
        public bool ExpandToSyosinbi;

        /// <summary>
        /// 終了日の診療期間の最終日までを対象とする。
        /// </summary>
        public bool ExpandToLastDate;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range">期間</param>
        /// <param name="expandToSyosinbi">開始日の診療期間の初診日から対象とする。</param>
        /// <param name="expandToLastDate">終了日の診療期間の最終日までを対象とする。</param>
        public KarteDateRange(DateRange2 range, bool expandToSyosinbi, bool expandToLastDate)
        {
            this.Range = range;
            this.ExpandToSyosinbi = expandToSyosinbi;
            this.ExpandToLastDate = expandToLastDate;
        }
    }
}