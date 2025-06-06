using OmoSeitoku;

namespace OmoOmotegaki
{
    public sealed class KarteBatchSettings
    {
        public KarteDateRange DateRange;

        public KartePrintItemList.SortSettings SortSettings;

        /// <summary>
        /// プレビューの場合の最大件数。
        /// 0以下の場合は全件。
        /// </summary>
        public int PreviewLimit;


        public KarteBatchSettings(KarteBatchSettings value)
        {
            if (value == null)
            {
                this.DateRange = new KarteDateRange(DateRange2.Infinite, true, true);
                this.SortSettings = KartePrintItemList.SortSettings.Default;
                this.PreviewLimit = 5;
            }
            else
            {
                this.DateRange = value.DateRange;
                this.SortSettings = value.SortSettings;
                this.PreviewLimit = value.PreviewLimit;
            }
        }

        public KarteBatchSettings()
            : this(null)
        {
        }
    }
}
