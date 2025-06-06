using OmoSeitoku;
using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OmoOmotegaki.Models.ShinryouCheckActions
{

    [Serializable]
    public class ShinryouCheckAction算定可能回数 : IShinryouCheckAction
    {
        public ShinryouCheckRuleType RuleType => ShinryouCheckRuleType.算定可能回数;


        [Browsable(true)]
        [Description("対象となるブロックを指定。")]
        public 算定ブロック単位 算定ブロック単位 { get; set; }

        [Browsable(true)]
        [Description("同類の算定チェック対象とする処置IDを指定。")]
        public int[] 同類算定処置 { get; set; }


        public ShinryouCheckResult Check(int shochiId, IEnumerable<SinryouData> shinryouDataCollection)
        {
            List<ShinryouCheckResultSingleContentItem> singleContentItems = new List<ShinryouCheckResultSingleContentItem>();
            List<ShinryouCheckResultMultiContentsItem> multiContentsItems = new List<ShinryouCheckResultMultiContentsItem>();

            int[] shochiIds = new int[1 + (同類算定処置?.Length ?? 0)];
            shochiIds[0] = shochiId;
            for (int i = (同類算定処置?.Length ?? 0) - 1; i >= 0; i--)
            {
                shochiIds[i + 1] = 同類算定処置[i];
            }


            // 前回算定日
            {
                DateTime 前回算定日 = shinryouDataCollection
                    .OrderBy(shinryouData => shinryouData.診療日)
                    .Where(shinryouData => this.TryGet処置(shinryouData, shochiId, out _))
                    .Select(shinryouData => shinryouData.診療日)
                    .LastOrDefault();

                if (前回算定日 == default)
                {
                    return new ShinryouCheckResult(string.Empty);
                }

                singleContentItems.Add(
                    new ShinryouCheckResultSingleContentItem(
                        "前回算定日", 前回算定日.ToShortDateString()));
            }


            if (!this.TryGet処置名(shinryouDataCollection, shochiId, out string shochiName, out string errorMessage))
            {
                shochiName = $"[{shochiId}] （{errorMessage}）";
            }

            if (同類算定処置?.Length > 0)
            {
                singleContentItems.Add(
                    new ShinryouCheckResultSingleContentItem(
                        "同類算定処置", string.Join(", ", 同類算定処置)));
            }

            singleContentItems.Add(
                new ShinryouCheckResultSingleContentItem(
                    "算定ブロック", 算定ブロック単位.ToString()));

            List<(DateRange 初診期間, int 残り算定可能回数)> zanList = CheckBlocks(shochiIds, shinryouDataCollection);

            if (zanList.Count > 0)
            {
                singleContentItems.Add(
                    new ShinryouCheckResultSingleContentItem(
                        "残り算定可能回数", zanList.Last().残り算定可能回数.ToString()));

                if (zanList.Count > 1)
                {
                    IEnumerable<string> items = zanList
                        .Select(p => $"({p.初診期間.Min.ToShortDateString()} ～ ) {p.残り算定可能回数} 回");

                    var expandableItem = new ShinryouCheckResultMultiContentsItem("残り算定可能回数 履歴", items);
                    multiContentsItems.Add(expandableItem);
                }
            }


            string title = this.CreateResultTitle(shochiId, shochiName);

            return new ShinryouCheckResult(title, singleContentItems, multiContentsItems);
        }

        private List<(DateRange 初診期間, int 残り算定可能回数)> CheckBlocks(
            IReadOnlyList<int> shochiIds, IEnumerable<SinryouData> shinryouDataCollection)
        {
            var res = new List<(DateRange 初診期間, int 残り算定可能回数)>();

            DateTime startDate = default;
            DateTime? prevDate = null;
            int santeiKanouCount = 0;
            int blockCount = 0;

            foreach (SinryouData shinryouData in shinryouDataCollection
                                                    .OrderBy(p => p.診療日))
            {
                if (shinryouData.Is初診日)
                {
                    if (prevDate.HasValue)
                    {
                        res.Add((
                            初診期間: new DateRange(startDate, prevDate.Value),
                            残り算定可能回数: santeiKanouCount - blockCount));
                    }

                    // リセット
                    santeiKanouCount = 0;
                    blockCount = 0;
                    startDate = shinryouData.診療日;
                }

                foreach (int shochiId in shochiIds)
                {
                    if (
                        // 対象の処置が含まれていない
                        !this.TryGet処置(shinryouData, shochiId, out SyochiData shochi) ||
                        // 歯式が設定されていない
                        (shinryouData.歯式?.Count == 0))
                    {
                        continue;
                    }

                    int ct = shinryouData.歯式.Count算定ブロック(算定ブロック単位);
                    if (ct > santeiKanouCount) santeiKanouCount = ct;

                    blockCount += shochi.Kaisuu;
                }

                prevDate = shinryouData.診療日;
            }

            if (prevDate.HasValue)
            {
                res.Add((
                    初診期間: new DateRange(startDate, prevDate.Value),
                    残り算定可能回数: santeiKanouCount - blockCount));
            }

            return res;
        }

        public override string ToString()
        {
            return RuleType + " チェック: " + 算定ブロック単位;
        }
    }
}
