using OmoEReceLib.ERObjects;
using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OmoOmotegaki.Controls
{
    public sealed class ShinryouListViewItemSource : ShinryouListViewNode
    {
        #region Constructor

        public ShinryouListViewItemSource(ShinryouListViewGroupSource parent, SinryouData source)
            : base(parent, source.行番号)
        {
            if (source == null) throw new ArgumentNullException("source");

            診療日 = source.診療日;
            病名リスト = source.病名;
            処置リスト = source.処置
                        .Where(s => s.SyochiId < 900)
                        .Select(s => s.Name + " x " + s.Kaisuu)
                        .ToArray();
            歯式 = source.歯式;
        }

        #endregion


        #region Model Property

        public DateTime 診療日 { get; set; }
        public IReadOnlyList<string> 病名リスト { get; set; }
        public IReadOnlyList<string> 処置リスト { get; set; }
        public ER歯式 歯式 { get; set; }

        #endregion


        // Method

        public override bool Equals(object obj)
        {
            var other = obj as ShinryouListViewItemSource;
            if (other == null) return false;

            return (診療日 == other.診療日)
                && ((病名リスト == other.病名リスト) || 病名リスト.SequenceEqual(other.病名リスト))
                && ((処置リスト == other.処置リスト) || 処置リスト.SequenceEqual(other.処置リスト))
                && (歯式 == other.歯式);
        }
        public override int GetHashCode()
        {
            int byoumeiHashCode = 0;
            int syochiHashCode = 0;

            if (病名リスト != null)
            {
                var byo = (System.Collections.IStructuralEquatable)病名リスト;
                byoumeiHashCode = byo.GetHashCode(EqualityComparer<string>.Default);
            }
            if (処置リスト != null)
            {
                var syo = (System.Collections.IStructuralEquatable)処置リスト;
                syochiHashCode = syo.GetHashCode(EqualityComparer<string>.Default);
            }

            return 診療日.GetHashCode()
                 ^ byoumeiHashCode
                 ^ syochiHashCode
                 ^ 歯式.GetHashCode();
        }
    }
}
