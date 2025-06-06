using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace OmoOmotegaki.Controls
{
    /// <summary>
    /// ShoshinKikanList.xaml の相互作用ロジック
    /// </summary>
    public partial class ShoshinKikanList : UserControl
    {
        public ShoshinKikanList()
        {
            InitializeComponent();
        }

        public event EventHandler ItemsSourceChanged;
        public event EventHandler SelectedItemChanged;


        #region DP: ItemsSource
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
                nameof(ItemsSource), typeof(IEnumerable),
                typeof(ShoshinKikanList),
                new PropertyMetadata());
        #endregion


        #region DP: SelectedItem
        public object SelectedItem
        {
            get => (object)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
                nameof(SelectedItem), typeof(object),
                typeof(ShoshinKikanList),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        #endregion

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == SelectedItemProperty)
            {
                SelectedItemChanged?.Invoke(this, EventArgs.Empty);
            }
            else if (e.Property == ItemsSourceProperty)
            {
                ItemsSourceChanged?.Invoke(this, EventArgs.Empty);
            }
        }


    }
}
