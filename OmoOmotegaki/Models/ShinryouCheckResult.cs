using System;
using System.Collections.Generic;
using System.Linq;

namespace OmoOmotegaki.Models
{
    public sealed class ShinryouCheckResult
    {
        public ShinryouCheckResult(string title)
        {
            Title = title;
            SingleContentItems = Array.Empty<ShinryouCheckResultSingleContentItem>();
            MultiContentsItems = Array.Empty<ShinryouCheckResultMultiContentsItem>();
        }

        public ShinryouCheckResult(
            string title, IEnumerable<ShinryouCheckResultSingleContentItem> singleContentItems)
        {
            Title = title;
            SingleContentItems = (singleContentItems ?? throw new ArgumentNullException(nameof(singleContentItems))).ToArray();
            MultiContentsItems = Array.Empty<ShinryouCheckResultMultiContentsItem>();
        }

        public ShinryouCheckResult(
            string title,
            IEnumerable<ShinryouCheckResultSingleContentItem> singleContentItems,
            IEnumerable<ShinryouCheckResultMultiContentsItem> multiContentsItems)
        {
            Title = title;
            SingleContentItems = (singleContentItems ?? throw new ArgumentNullException(nameof(singleContentItems))).ToArray();
            MultiContentsItems = (multiContentsItems ?? throw new ArgumentNullException(nameof(multiContentsItems))).ToArray();
        }

        public string Title { get; }

        public IReadOnlyList<ShinryouCheckResultSingleContentItem> SingleContentItems { get; }

        public IReadOnlyList<ShinryouCheckResultMultiContentsItem> MultiContentsItems { get; }

    }

    public sealed class ShinryouCheckResultSingleContentItem
    {
        public ShinryouCheckResultSingleContentItem(string title, string content)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }

        public string Title { get; }
        public string Content { get; }
    }

    public sealed class ShinryouCheckResultMultiContentsItem
    {
        public ShinryouCheckResultMultiContentsItem(string title, IEnumerable<string> contents)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Contents = (contents ?? throw new ArgumentNullException(nameof(contents))).ToArray();
        }

        public string Title { get; }
        public IReadOnlyList<string> Contents { get; }
    }
}
