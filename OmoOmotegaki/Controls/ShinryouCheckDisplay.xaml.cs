using OmoOmotegaki.ViewModels.Controls;
using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace OmoOmotegaki.Controls
{
    /// <summary>
    /// ShinryouCheckDisplay.xaml の相互作用ロジック
    /// </summary>
    public partial class ShinryouCheckDisplay : UserControl
    {
        public ShinryouCheckDisplay()
        {
            InitializeComponent();

            DataContextChanged += delegate
            {
                var vm = (ShinryouCheckDisplayViewModel)DataContext;

                _propertyGrid.SelectedObject = vm?.QuickCheckSettings;
            };
        }

        private void InputQuickCheckTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;

                var textBox = (TextBox)sender;
                var bndExp = BindingOperations.GetBindingExpression(textBox, TextBox.TextProperty);
                bndExp.UpdateSource();
            }
        }

        private void TextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            
            textBox.Tag = textBox.Text;

            Dispatcher.BeginInvoke(
                (Action)textBox.SelectAll,
                System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        private void TextBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;

            textBox.Text = (textBox.Tag as string) ?? string.Empty;
        }
    }
}
