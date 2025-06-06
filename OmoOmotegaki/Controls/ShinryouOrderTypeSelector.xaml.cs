using OmoSeitokuEreceipt.SER;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace OmoOmotegaki.Controls
{
    /// <summary>
    /// ShinryouOrderTypeSelector.xaml の相互作用ロジック
    /// </summary>
    public partial class ShinryouOrderTypeSelector : UserControl
    {
        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register(nameof(SelectedValue), typeof(ShinryouOrderType), typeof(ShinryouOrderTypeSelector),
                new PropertyMetadata((d, e) => ((ShinryouOrderTypeSelector)d).OnSelectedValueChanged(e)));

        public ShinryouOrderTypeSelector()
        {
            InitializeComponent();
        }

        public EventHandler SelectedValueChanged;

        public ShinryouOrderType SelectedValue
        {
            get { return (ShinryouOrderType)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        private void OnSelectedValueChanged(DependencyPropertyChangedEventArgs e)
        {
            ShinryouOrderType newValue = (ShinryouOrderType)e.NewValue;
            int newIndex = (int)newValue;
            _comboBox.SelectedIndex = newIndex;
            SelectedValueChanged?.Invoke(this, EventArgs.Empty);
        }

        private void _comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedValue = (ShinryouOrderType)_comboBox.SelectedIndex;
        }

        private sealed class MyItem
        {
            public string Text { get; }
            public BitmapImage Image { get; }

            public MyItem(string text, BitmapImage image)
            {
                this.Text = text;
                this.Image = image;
            }
        }
    }
}
