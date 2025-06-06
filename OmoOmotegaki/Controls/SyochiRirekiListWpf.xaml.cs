using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Controls;

namespace OmoOmotegaki.Controls
{
    /// <summary>
    /// SyochiRirekiListWpf.xaml の相互作用ロジック
    /// </summary>
    public partial class SyochiRirekiListWpf : UserControl
    {
        public delegate void ItemDateEventHandler(object sender, ItemDateEventArgs e);

        private ShinryouDataCollection _data;

        public SyochiRirekiListWpf()
        {
            InitializeComponent();
        }

        public event EventHandler<ErrorMessageEventArgs> ErrorMessage;

        public event ItemDateEventHandler ItemStartDateClick;

        public event ItemDateEventHandler ItemEndDateClick;

        #region Property

        public PointF ScrollPercent
        {
            get
            {
                var p = _shinryouListView.ScrollPercent;
                return new PointF((float)p.X, (float)p.Y);
            }
        }

        #endregion Property

        #region Method

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
            if (!(_data is null))
            {
                _data.CollectionChanged -= Internal_data_CollectionChanged;
            }

            _data = data;

            if (!(_data is null))
            {
                _data.CollectionChanged += Internal_data_CollectionChanged;
            }

            Internal_data_CollectionChanged(this, null);
        }

        private void RaiseError(string message)
        {
            ErrorMessage?.Invoke(this, new ErrorMessageEventArgs(message));
        }
        private void RaiseItemStartDateClick()
        {
            ItemStartDateClick?.Invoke(this, new ItemDateEventArgs(-1, default));
            throw new NotSupportedException();
        }
        private void RaiseItemEndDateClick()
        {
            ItemEndDateClick?.Invoke(this, new ItemDateEventArgs(-1, default));
            throw new NotSupportedException();
        }

        #endregion Method

        #region Event Helper

        private void Internal_data_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ShinryouListViewDataSource dataSource = _data is null
                                                        ? null
                                                        : ShinryouListViewDataSource.Create(_data);
            _shinryouListView.SetDataSource(dataSource);
        }

        #endregion Event Helper

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
            public int Index;
            public DateTime Date;

            public ItemDateEventArgs(int index, DateTime date)
                : base()
            {
                Index = index;
                Date = date;
            }
        }

        #endregion クラス
    }
}