using OmoOmotegaki.Models.ShinryouCheckActions;
using OmoSeitoku;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OmoOmotegaki.Models
{
    public enum ShinryouCheckRuleType
    {
        回数と前回算定日を表示 = 1,
        前回算定日を表示 = 2,
        一定期間算定不可 = 3,
        算定可能回数 = 4,
        算定可能回数検査 = 5,
    }

    public static class ShinryouCheckRuleCheckExtension
    {
        private static Dictionary<ShinryouCheckRuleType, ConstructorInfo> CheckActionTypeInfos
        {
            get
            {
                var checkActions = __prop_CheckActions;
                if (checkActions is null)
                {
                    // CheckActions 名前空間のICheckAction型をすべて取得
                    __prop_CheckActions = checkActions =
                        ClassUtils.GetDefinedClasses(typeof(ShinryouCheckAction回数と前回算定日を表示).Namespace)
                            .Where(type => typeof(IShinryouCheckAction).IsAssignableFrom(type))
                            .Select(type =>
                            {
                                ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                                IShinryouCheckAction instance = (IShinryouCheckAction)constructor.Invoke(null);
                                return new
                                {
                                    instance.RuleType,
                                    constructor
                                };
                            })
                            .ToDictionary(pair => pair.RuleType, pair => pair.constructor);
                }
                return checkActions;
            }
        }
        private static Dictionary<ShinryouCheckRuleType, ConstructorInfo> __prop_CheckActions;

        public static IShinryouCheckAction GetAction(this ShinryouCheckRuleType ruleType)
        {
            if (CheckActionTypeInfos.TryGetValue(ruleType, out ConstructorInfo checkActionConstructor))
            {
                return (IShinryouCheckAction)checkActionConstructor.Invoke(null);
            }
            else
            {
                throw new InvalidOperationException("[error: 64a754eb] 未対応の RuleType: " + ruleType);
            }
        }
    }
}
