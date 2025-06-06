using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace OmoSeitokuEreceipt.ER.Controls
{
    /// <summary>
    /// SisikiFilterControl.xaml の相互作用ロジック
    /// </summary>
    public partial class SisikiFilterControl : UserControl
    {
        private const double SEMI_HIDDEN_OPACITY = 0.3;


        public event EventHandler Checked歯種Changed = delegate { };


        public bool IsMultiSelect
        {
            get
            {
                return _btnMultiSelect.IsChecked.HasValue
                    && _btnMultiSelect.IsChecked.Value;
            }
            set
            {
                _btnMultiSelect.IsChecked = value;
            }
        }


        /// <summary>{ 歯種 => 歯ボタン }</summary>
        private Dictionary<string, ToggleButton> _haButtons;
        private bool _suspendChecked歯種Changed;


        public IEnumerable<string> Checked歯種
        {
            get
            {
                return from kv in _haButtons
                       where (kv.Value.IsChecked.HasValue && kv.Value.IsChecked.Value)
                       select kv.Key;
            }
            set
            {
                _suspendChecked歯種Changed = true;
                try
                {
                    foreach (var btn in _haButtons.Values)
                    {
                        btn.IsChecked = false;
                    }
                    if (value != null)
                    {
                        foreach (string item in value)
                        {
                            _haButtons[item].IsChecked = true;
                        }
                    }
                }
                finally
                {
                    _suspendChecked歯種Changed = false;
                    this.OnChecked歯種Changed(EventArgs.Empty);
                }
            }
        }
        public IEnumerable<string> SemiHidden歯種
        {
            get
            {
                return from kv in _haButtons
                       where (kv.Value.Opacity == SEMI_HIDDEN_OPACITY)
                       select kv.Key;
            }
            set
            {
                foreach (var btn in _haButtons.Values)
                {
                    btn.Opacity = 1.0;
                }
                if (value != null)
                {
                    foreach (string hasyu in value)
                    {
                        _haButtons[hasyu].Opacity = SEMI_HIDDEN_OPACITY;
                    }
                }
            }
        }


        // Constructor

        public SisikiFilterControl()
        {
            InitializeComponent();

            _btnClear.Click += delegate
            {
                this.Checked歯種 = null;
            };

            _haButtons = new Dictionary<string, ToggleButton>();

            // Get HaButtons
            for (int bui = 1; bui <= 8; bui++)
            {
                bool isNyuusi = (5 <= bui);

                for (int ha = 1; ha <= 8; ha++)
                {
                    if (isNyuusi && (ha == 6)) break;
                    string hasyu = "10" + bui + ha;
                    var btn = (ToggleButton)this.FindName("_btn" + hasyu);
                    _haButtons.Add(hasyu, btn);

                    // setup button
                    btn.Checked += btn_CheckedChanged;
                    btn.Unchecked += btn_CheckedChanged;
                }
            }
        }


        // Method

        public void PerformClickClearButton()
        {
            _btnClear.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.LeftCtrl)
            {
                this.IsMultiSelect = true;
            }
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.Key == Key.LeftCtrl)
            {
                this.IsMultiSelect = false;
            }
        }

        private void OnChecked歯種Changed(EventArgs e)
        {
            this.Checked歯種Changed(this, e);
        }


        // Event Handler

        void btn_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (_suspendChecked歯種Changed) return;

            var senderButton = (ToggleButton)e.Source;
            bool senderChecked = (senderButton.IsChecked.HasValue && senderButton.IsChecked.Value);

            if (this.IsMultiSelect)
            {
                this.OnChecked歯種Changed(e);
            }
            else if (senderChecked)
            {
                // オンにされたボタン以外をオフにする
                this.Checked歯種 =
                    from kv in _haButtons
                    where (kv.Value == senderButton)
                    select kv.Key;
            }
            else
            {
                // すべてオフ
                this.Checked歯種 = null;
            }
        }
    }
}
