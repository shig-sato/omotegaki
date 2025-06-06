using OmoOmotegaki.Models.ShinryouCheckActions;
using System;
using System.ComponentModel;

namespace OmoOmotegaki.Models
{
    [Serializable]
    public class ShinryouCheckRule
    {
        public ShinryouCheckRule()
        {
        }

        public ShinryouCheckRule(int shochiId)
        {
            ShochiId = shochiId;
        }

        [Browsable(true)]
        [Category("."), DisplayName("処置ID"), Description("指定した処置が含まれる診療データが処理されます。")]
        public int ShochiId { get; set; }

        [Browsable(true)]
        [Category("."), DisplayName("タイプ"), Description("処理の種類を選択します。詳細は「チェック処理」項目で設定できます。")]
        public ShinryouCheckRuleType RuleType
        {
            get => CheckAction.RuleType;
            set
            {
                if ((CheckAction is null) || CheckAction.RuleType != value)
                {
                    CheckAction = value.GetAction();
                }
            }
        }

        [Browsable(true)]
        [Category("."), DisplayName("チェック処理"), Description("処理内容を設定します。")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public IShinryouCheckAction CheckAction { get; set; } = ShinryouCheckAction回数と前回算定日を表示.Instance;

        public override string ToString()
        {
            return $"処置[{ShochiId}] {CheckAction}";
        }
    }
}