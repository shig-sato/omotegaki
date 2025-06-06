using OmoEReceLib.ERObjects;
using OmoSeitokuEreceipt.ER.Controls;
using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace OmoOmotegaki.Controls
{
    public partial class ToothSelector : UserControl
    {
        public event EventHandler<HaButtonClickEventArgs> HaButtonClick = delegate { };
        public event EventHandler CheckedHaButton歯種Changed = delegate { };


        private ER歯式 _displaySisiki;
        private SortedDictionary<DateTime, SinryouData> _dict;


        // Property

        public IEnumerable<string> CheckedHaButton歯種
        {
            get { return _sisikiFilterControl1.Checked歯種; }
            set { _sisikiFilterControl1.Checked歯種 = value; }
        }
        public bool IsMultiSelect
        {
            get { return _sisikiFilterControl1.IsMultiSelect; }
            set { _sisikiFilterControl1.IsMultiSelect = value; }
        }
        private ShinryouDataCollection Data
        {
            get { return __property_Data; }
            set
            {
                __property_Data = value;
                if (value != null)
                {
                    value.CollectionChanged += (s, e) => this.OnDataChanged(e);
                }
                this.OnDataChanged(EventArgs.Empty);
            }
        }
        private ShinryouDataCollection __property_Data;


        // Constructor

        public ToothSelector()
        {
            InitializeComponent();


            _displaySisiki = new ER歯式();
            _dict = new SortedDictionary<DateTime, SinryouData>();


            _sisikiFilterControl1.Checked歯種Changed += delegate
            {
                this.CheckedHaButton歯種Changed(this, EventArgs.Empty);
            };
        }


        // Method

        public void ClickClearButton()
        {
            _sisikiFilterControl1.PerformClickClearButton();
        }
        public void SetData(ShinryouDataCollection data)
        {
            this.Data = data;
        }

        protected void OnDataChanged(EventArgs e)
        {
            this.UpdateDictAndDisplatSisiki();
            this.UpdateDisplaySisikiControl();
        }

        private void UpdateDisplaySisikiControl()
        {
            _sisikiFilterControl1.SemiHidden歯種 = from ha in _displaySisiki.Inverse() select ha.歯種;
        }
        private void UpdateDictAndDisplatSisiki()
        {
            _dict.Clear();

            var haSet = new HashSet<string>();

            if (this.Data != null)
            {
                foreach (SinryouData item in this.Data)
                {
                    if (item.歯式 != null)
                    {
                        foreach (var hasyu in from ha in item.歯式
                                              where !haSet.Contains(ha.歯種)
                                              select ha.歯種)
                        {
                            haSet.Add(hasyu);
                        }
                    }
                    if (!_dict.ContainsKey(item.診療日))
                    {
                        _dict.Add(item.診療日, item);
                    }
                }
            }

            _displaySisiki = new ER歯式
            {
                haSet.Select(hasyu => new ER歯式単位(hasyu)).ToArray()
            };
        }


        // Event Handler

        void haButton_Click(object sender, MouseEventArgs e)
        {
            var haButton = (HaToggleButton)sender;

            var ev = new HaButtonClickEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta, haButton.歯種);
            this.HaButtonClick(this, ev);
        }

    }


    public sealed class HaButtonClickEventArgs : MouseEventArgs
    {
        public readonly string 歯種;

        public HaButtonClickEventArgs(MouseButtons button, int clicks, int x, int y, int delta, string hasyu)
            : base(button, clicks, x, y, delta)
        {
            this.歯種 = hasyu;
        }
    }
}