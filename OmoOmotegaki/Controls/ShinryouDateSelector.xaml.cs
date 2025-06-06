using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OmoOmotegaki.Controls
{
    /// <summary>
    /// ShinryouDateSelector.xaml の相互作用ロジック
    /// </summary>
    public partial class ShinryouDateSelector : UserControl
    {
        #region Constructor

        public ShinryouDateSelector()
        {
            InitializeComponent();
        }

        #endregion Constructor

        public event EventHandler ItemActivated = delegate { };

        public event EventHandler SelectedItemChanged = delegate { };

        public ItemData SelectedItem => (ItemData)_listView.SelectedItem;

        #region Event Handler

        private void ListView_KeyDown(object sender, KeyEventArgs e)
        {
            if ((_listView.SelectedItem != null) && (e.Key == Key.Enter))
            {
                ItemActivated(this, EventArgs.Empty);
            }
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_listView.SelectedItem != null)
            {
                ItemActivated(this, EventArgs.Empty);
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItemChanged(this, EventArgs.Empty);
        }

        private bool _suspendExpanderEvent;

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            // グループが開かれたら他のグループを非表示にする。
            if (_suspendExpanderEvent) return;
            try
            {
                _suspendExpanderEvent = true;
                foreach (Expander expander in GetChildrenOfType<Expander>(_listView)
                                                    .Where(expander => expander != sender))
                {
                    expander.IsExpanded = false;
                    expander.Visibility = Visibility.Collapsed;
                }
            }
            finally
            {
                _suspendExpanderEvent = false;
            }

        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            // グループが閉じられたら他のグループを表示する。
            if (_suspendExpanderEvent) return;
            try
            {
                _suspendExpanderEvent = true;
                foreach (Expander expander in GetChildrenOfType<Expander>(_listView)
                                                    .Where(expander => expander != sender))
                {
                    expander.Visibility = Visibility.Visible;
                }
            }
            finally
            {
                _suspendExpanderEvent = false;
            }
        }

        #endregion Event Handler

        #region Helper

        private static IEnumerable<T> GetChildrenOfType<T>(Visual element)
            where T : Visual
        {
            if (element is null) { throw new ArgumentNullException(nameof(element)); }
            if (element is FrameworkElement fe) fe.ApplyTemplate();
            int length = VisualTreeHelper.GetChildrenCount(element);
            foreach (Visual child in Enumerable.Range(0, length)
                                                .Select(i => VisualTreeHelper.GetChild(element, i))
                                                .OfType<Visual>())
            {
                if (child is T foundElement)
                {
                    yield return foundElement;
                }
                else
                {
                    foreach (T item in GetChildrenOfType<T>(child))
                    {
                        yield return item;
                    }
                }
            }
        }

        #endregion

        public sealed class ItemData : IEqualityComparer<ItemData>, IEquatable<ItemData>
        {
            public ItemData(SinryouData data)
            {
                Date = data.診療日.ToString("MM/dd");
                Year = data.診療日.Year;
                SinryouData = data;
            }

            public string Date { get; }
            public int Year { get; }
            public SinryouData SinryouData { get; }

            #region Equals

            public override bool Equals(object obj)
            {
                return (obj is ItemData other) && Equals(other);
            }

            public override int GetHashCode()
            {
                var hashCode = 1243058077;
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Date);
                hashCode = hashCode * -1521134295 + Year.GetHashCode();
                return hashCode;
            }

            public bool Equals(ItemData other)
            {
                return !(other is null) && (Year == other.Year) && (Date == other.Date);
            }

            public bool Equals(ItemData x, ItemData y) => ReferenceEquals(x, y) || (x?.Equals(y) ?? false);

            public int GetHashCode(ItemData obj) => obj.GetHashCode();

            public override string ToString() => string.Concat(Year.ToString(), "/", Date);

            public static bool operator ==(ItemData x, ItemData y) => ReferenceEquals(x, y) || (x?.Equals(y) ?? false);

            public static bool operator !=(ItemData x, ItemData y) => !(x == y);

            #endregion Equals
        }
    }
}