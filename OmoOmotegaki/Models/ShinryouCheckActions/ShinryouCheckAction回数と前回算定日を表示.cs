using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OmoOmotegaki.Models.ShinryouCheckActions
{
    public class ShinryouCheckAction回数と前回算定日を表示 : IShinryouCheckAction
    {
        public static readonly IShinryouCheckAction Instance = new ShinryouCheckAction回数と前回算定日を表示();

        public ShinryouCheckRuleType RuleType => ShinryouCheckRuleType.回数と前回算定日を表示;

        public ShinryouCheckResult Check(int shochiId, IEnumerable<SinryouData> shinryouDataCollection)
        {
            List<ShinryouCheckResultSingleContentItem> singleContentItems = new List<ShinryouCheckResultSingleContentItem>();

            string shochiName = null;
            DateTime 前回算定日 = DateTime.MinValue;
            int total = 0;

            foreach (var item in shinryouDataCollection)
            {
                if (!Contains処置(item, shochiId, out SyochiData? targetShochi)) continue;

                if (item.診療日 > 前回算定日) 前回算定日 = item.診療日;

                total += targetShochi.Value.Kaisuu;

                if (shochiName is null) shochiName = targetShochi.Value.Name;
            }

            if (total == 0)
            {
                return new ShinryouCheckResult(string.Empty);
            }

            singleContentItems.Add(
                new ShinryouCheckResultSingleContentItem(
                    "回数", $"{total} 回"));

            if (前回算定日 > DateTime.MinValue)
            {
                singleContentItems.Add(
                    new ShinryouCheckResultSingleContentItem(
                        "前回算定日", 前回算定日.ToShortDateString()));
            }

            string title = this.CreateResultTitle(shochiId, shochiName);

            return new ShinryouCheckResult(title, singleContentItems);


            static bool Contains処置(SinryouData shinryouData, int shochiId, out SyochiData? targetShochi)
            {
                SyochiData shochi = shinryouData.処置.FirstOrDefault(s => s.SyochiId == shochiId);
                if ((shochi != null) && (shochi.Kaisuu > 0))
                {
                    targetShochi = shochi;
                    return true;
                }
                else
                {
                    targetShochi = default;
                    return false;
                }
            }
        }

        public override string ToString()
        {
            return RuleType.ToString();
        }
    }
}
