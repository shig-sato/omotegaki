using System.Collections.Generic;
using System.Linq;

namespace OmoOmotegaki.Controls
{
    public sealed class ShinryouListViewDataSource
    {
        public int Count
        {
            get;
            private set;
        }
        public IEnumerable<ShinryouListViewGroupSource> Groups
        {
            get { return __property_Groups; }
            private set
            {
                __property_Groups = value;
                this.Count = value.Sum(group => group.Count());
            }
        }
        public IEnumerable<ShinryouListViewItemSource> Items
        {
            get
            {
                foreach (var group in this.Groups)
                {
                    foreach (var item in group)
                    {
                        yield return item;
                    }
                }
            }
        }

        #region Property Field

        private IEnumerable<ShinryouListViewGroupSource> __property_Groups;

        #endregion


        public ShinryouListViewDataSource(IEnumerable<ShinryouListViewGroupSource> groups)
        {
            Groups = groups;
        }


        public static ShinryouListViewDataSource Create(OmoSeitokuEreceipt.SER.ShinryouDataCollection data)
        {
            return new ShinryouListViewDataSource(
                ShinryouListViewGroupSource.CreateGroups(data));
        }
    }
}
