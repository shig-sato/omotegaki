using OmoSeitokuEreceipt.SER;
using System.Collections.Generic;

namespace OmoOmotegaki.Models
{
    public interface IShinryouCheckAction
    {
        ShinryouCheckRuleType RuleType { get; }
        ShinryouCheckResult Check(int shochiId, IEnumerable<SinryouData> shinryouDataCollection);

    }
}
