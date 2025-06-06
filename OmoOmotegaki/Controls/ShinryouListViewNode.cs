using System.ComponentModel;

namespace OmoOmotegaki.Controls
{
    public abstract class ShinryouListViewNode : INotifyPropertyChanged
    {
        protected ShinryouListViewNode(ShinryouListViewGroupSource parent, int gyoBangou)
        {
            Parent = parent;
            行番号 = gyoBangou;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public ShinryouListViewGroupSource Parent { get; private set; }

        public int 行番号 { get; private set; }

        public virtual bool IsSelected
        {
            get => __property_IsSelected;
            set
            {
                if (__property_IsSelected != value)
                {
                    __property_IsSelected = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                }
            }
        }
        private bool __property_IsSelected;

        public bool IsGroupNode => Parent == null;
    }
}
