using OmoSeitokuEreceipt.SER;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace OmoOmotegaki.Controls
{
    public partial class SyochiRirekiList : UserControl
    {
        public delegate void ItemDateEventHandler(object sender, ItemDateEventArgs e);

        //public event EventHandler<ErrorMessageEventArgs> ErrorMessage;
        //public event ItemDateEventHandler ItemStartDateClick;
        //public event ItemDateEventHandler ItemEndDateClick;

        private ShinryouDataCollection _data;

        // Property

        public PointF ScrollPercent
        {
            get
            {
                var p = _shinryouListView.ScrollPercent;
                return new PointF((float)p.X, (float)p.Y);
            }
        }

        // Constructor

        public SyochiRirekiList()
        {
            InitializeComponent();

            _shinryouListView.KeyDown += (_, e) =>
            {
                var evt = new KeyEventArgs(
                    (Keys)System.Windows.Input.KeyInterop.VirtualKeyFromKey(e.Key));

                OnKeyDown(evt);

                e.Handled = evt.Handled;
            };
        }

        // Method

        public void EditEndDate(int receiptIndex, DateTime date)
        {
            throw new NotImplementedException();
        }

        public void EditStartDate(int receiptIndex, DateTime date)
        {
            throw new NotImplementedException();
        }

        public void ScrollListTo(DateTime date)
        {
            _shinryouListView.ScrollTo(date);
        }

        public void ScrollListTo(double horizontalPercent, double verticalPercent)
        {
            _shinryouListView.ScrollTo(horizontalPercent, verticalPercent);
        }

        public void ScrollListToTop()
        {
            _shinryouListView.ScrollToTop();
        }

        public void ScrollListToBottom()
        {
            _shinryouListView.ScrollToBottom();
        }

        public void SetData(ShinryouDataCollection data)
        {
            _data = data;

            if (data != null)
            {
                _data.CollectionChanged += Internal_data_CollectionChanged;
            }

            Internal_data_CollectionChanged(this, null);
        }

        private void Internal_data_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ShinryouListViewDataSource dataSource = (_data is null)
                                                        ? null
                                                        : ShinryouListViewDataSource.Create(_data);

            _shinryouListView.SetDataSource(dataSource);
        }

        #region クラス

        public sealed class ErrorMessageEventArgs : EventArgs
        {
            public string Message { get; }

            public ErrorMessageEventArgs(string message)
            {
                Message = message;
            }
        }

        public sealed class ItemDateEventArgs : EventArgs
        {
            public int Index { get; }
            public DateTime Date { get; }

            public ItemDateEventArgs(int index, DateTime date)
            {
                Index = index;
                Date = date;
            }
        }

        #endregion クラス
    }
}