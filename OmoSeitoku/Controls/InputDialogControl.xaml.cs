using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OmoSeitoku.Controls
{
    /// <summary>
    /// InputDialogControl.xaml の相互作用ロジック
    /// </summary>
    public partial class InputDialogControl : UserControl
    {
        public event EventHandler OKButtonClick;
        public event EventHandler CancelButtonClick;

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(InputDialogControl), new PropertyMetadata(""));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(InputDialogControl), new PropertyMetadata(""));

        public static readonly DependencyProperty MultiLineProperty =
            DependencyProperty.Register("MultiLine", typeof(bool), typeof(InputDialogControl),
                new FrameworkPropertyMetadata((d, e) => ((InputDialogControl)d).OnMultiLineChanged(e)));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public bool MultiLine
        {
            get { return (bool)GetValue(MultiLineProperty); }
            set { SetValue(MultiLineProperty, value); }
        }


        public InputDialogControl()
        {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            Dispatcher.BeginInvoke((Action)delegate
            {
                _textBox.Focus();
            },
            System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }

        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            OKButtonClick?.Invoke(this, EventArgs.Empty);
        }

        private void CANCEL_Button_Click(object sender, RoutedEventArgs e)
        {
            CancelButtonClick?.Invoke(this, EventArgs.Empty);
        }

        private void OnMultiLineChanged(DependencyPropertyChangedEventArgs e)
        {
            try
            {
                _textBox.AcceptsReturn = MultiLine;
            }
            catch (Exception)
            {
            }
        }
    }
}
