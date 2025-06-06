using OmoEReceLib.ERObjects;
using OmoOmotegaki.Data;
using OmoSeitoku;
using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace OmoOmotegaki.Models.ShinryouCheckActions
{

    [Serializable]
    public class ShinryouCheckAction一定期間算定不可 : IShinryouCheckAction
    {
        public ShinryouCheckRuleType RuleType => ShinryouCheckRuleType.一定期間算定不可;

        [Browsable(true)]
        [TypeConverter(typeof(TermConverter))]
        [Description("期間を指定。")]
        public Term 算定不可期間 { get; set; }

        [Browsable(true)]
        [Description("対象となるブロックを指定。")]
        public 算定ブロック単位 算定ブロック単位 { get; set; }

        [Browsable(true)]
        [Description("前回算定日から現在までに算定済みかどうかチェックする処置IDを指定。")]
        public int[] 算定済チェック処置 { get; set; }


        public ShinryouCheckResult Check(int shochiId, IEnumerable<SinryouData> shinryouDataCollection)
        {
            List<ShinryouCheckResultSingleContentItem> singleContentItems = new List<ShinryouCheckResultSingleContentItem>();

            this.TryGet処置名(shinryouDataCollection, shochiId, out string shochiName, out _);

            List<(string BlockName, SinryouData SinryouData)> shinryouDatas =
                CheckBlocks(shochiId, shinryouDataCollection);

            if (shinryouDatas.Count == 0)
            {
                return new ShinryouCheckResult(string.Empty);
            }

            if (算定ブロック単位 != 算定ブロック単位.全歯)
            {
                singleContentItems.Add(
                    new ShinryouCheckResultSingleContentItem(
                        "算定ブロック", 算定ブロック単位.ToString()));
            }

            foreach ((string blockName, SinryouData shinryouData) in shinryouDatas)
            {
                DateTime 前回算定日 = shinryouData.診療日;

                singleContentItems.Add(
                    new ShinryouCheckResultSingleContentItem(
                        string.Empty, $"[{blockName}]"));

                singleContentItems.Add(
                    new ShinryouCheckResultSingleContentItem(
                        "前回算定日", 前回算定日.ToShortDateString()));

                if (!算定不可期間.IsEmpty)
                {
                    singleContentItems.Add(
                        new ShinryouCheckResultSingleContentItem(
                            "算定不可期間", 算定不可期間.ToString()));
                }

                DateTime offsetDate = 算定不可期間.GetOffsetDate(前回算定日);
                if (DateTime.Now < offsetDate)
                {
                    singleContentItems.Add(
                        new ShinryouCheckResultSingleContentItem(
                            string.Empty, $"{offsetDate.ToShortDateString()} まで算定不可"));
                }

                foreach (int targetShochiId in 算定済チェック処置 ?? Enumerable.Empty<int>())
                {
                    SinryouData target = shinryouDataCollection
                        .FirstOrDefault(p => (p.診療日 > 前回算定日) &&
                                             this.TryGet処置(p, targetShochiId, out _) &&
                                             this.IsSameBlock(算定ブロック単位, p, shinryouData));

                    string content = (target is null)
                        ? $"処置[{targetShochiId}] が 算定できます。"
                        : $"処置[{targetShochiId}] は {target.診療日.ToShortDateString()} に算定済です。";

                    singleContentItems.Add(
                        new ShinryouCheckResultSingleContentItem(
                            "算定済チェック処置", content));
                }
            }

            string title = this.CreateResultTitle(shochiId, shochiName);

            return new ShinryouCheckResult(title, singleContentItems);
        }

        private List<(string BlockName, SinryouData SinryouData)> CheckBlocks(
            int shochiId, IEnumerable<SinryouData> shinryouDataCollection)
        {
            var res = new List<(string BlockName, SinryouData SinryouData)>();

            switch (算定ブロック単位)
            {
                case 算定ブロック単位.全歯:
                    {
                        DateTime max = DateTime.MinValue;
                        foreach (var p in shinryouDataCollection)
                        {
                            if (!this.TryGet処置(p, shochiId, out _)) continue;
                            if (p.診療日 > max)
                            {
                                max = p.診療日;
                                if (res.Count == 0)
                                    res.Add(("全歯", p));
                                else
                                    res[0] = ("全歯", p);
                            }
                        }
                    }
                    break;

                case 算定ブロック単位.顎:
                    {
                        BlockStatus 上顎 = new BlockStatus("上顎");
                        BlockStatus 下顎 = new BlockStatus("下顎");
                        foreach (var p in shinryouDataCollection)
                        {
                            if (!this.TryGet処置(p, shochiId, out _) ||
                                (p.歯式 is null) || (p.歯式.Count == 0))
                            {
                                continue;
                            }

                            BlockStatus blockStatus = p.歯式.First().部位.Is上顎() ? 上顎 : 下顎;

                            if ((blockStatus.前回算定 is null) || (p.診療日 > blockStatus.前回算定.診療日))
                            {
                                blockStatus.前回算定 = p;
                            }
                        }

                        if (上顎.前回算定 is object)
                            res.Add((上顎.BlockName, 上顎.前回算定));

                        if (下顎.前回算定 is object)
                            res.Add((下顎.BlockName, 下顎.前回算定));
                    }
                    break;

                case 算定ブロック単位.歯区分:
                    {
                        Dictionary<ER歯式.歯区分, BlockStatus> blockDict =
                            ((ER歯式.歯区分[])typeof(ER歯式.歯区分).GetEnumValues())
                                        .ToDictionary(
                                                p => p,
                                                p => new BlockStatus(p.ToString()));
                        foreach (var p in shinryouDataCollection)
                        {
                            if (!this.TryGet処置(p, shochiId, out _) ||
                                (p.歯式 is null) || (p.歯式.Count == 0))
                            {
                                continue;
                            }

                            ER歯式.歯区分 block = p.歯式.First().Get歯区分();
                            BlockStatus blockStatus = blockDict[block];

                            if ((blockStatus.前回算定 is null) || (p.診療日 > blockStatus.前回算定.診療日))
                            {
                                blockStatus.前回算定 = p;
                            }
                        }

                        foreach (BlockStatus block in blockDict.Values)
                        {
                            if (block.前回算定 is object)
                                res.Add((block.BlockName, block.前回算定));
                        }
                    }
                    break;

                default:
                    throw new InvalidOperationException("[error: 6312d62c] 未対応の 算定ブロック単位: " + 算定ブロック単位);
            }

            return res;
        }

        public override string ToString()
        {
            return 算定不可期間 + " 算定不可";
        }

        private sealed class BlockStatus
        {
            public BlockStatus(string blockName)
            {
                BlockName = blockName ?? throw new ArgumentNullException(nameof(blockName));
            }

            public string BlockName { get; }

            public SinryouData 前回算定 { get; set; } = null;
        }
    }
}
