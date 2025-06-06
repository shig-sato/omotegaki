using System;

namespace OmoOmotegaki.Models
{
    public sealed class ShinryouCheckRuleParamInfo
    {
        public ShinryouCheckRuleParamInfo(string name, string description)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public string Name { get; }
        public string Description { get; }
    }
}
