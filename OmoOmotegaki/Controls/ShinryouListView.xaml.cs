using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace OmoOmotegaki.Controls
{
    /// <summary>
    /// ShinryouListView.xaml の相互作用ロジック
    /// </summary>
    public partial class ShinryouListView : UserControl
    {
        private readonly NodeTreeSelectionBehavior _selectionBehavior;

        private readonly OmoSeitoku.Threadings.Throttle<ShinryouListView, double, double> _scrollToThrottle =
            new OmoSeitoku.Threadings.Throttle<ShinryouListView, double, double>(
                TimeSpan.FromMilliseconds(200),
                (ShinryouListView me, double horizontalPercent, double verticalPercent) => me.InternalScrollTo(horizontalPercent, verticalPercent));

        private ShinryouListViewDataSource _dataSource;

        #region Constructor

        public ShinryouListView()
        {
            InitializeComponent();

            // 仮想リストでのスクロールを可能にする為のビヘイビアを取得
            _selectionBehavior = (NodeTreeSelectionBehavior)Interaction.GetBehaviors(_listView)
                .FirstOrDefault(b => b is NodeTreeSelectionBehavior);
        }

        #endregion Constructor

        #region Property

        public Point ScrollPercent
        {
            get
            {
                AutomationPeer peer = UIElementAutomationPeer.CreatePeerForElement(_listView);
                IScrollProvider scrollProvider = (IScrollProvider)peer.GetPattern(PatternInterface.Scroll);
                return new Point(
                    scrollProvider.HorizontalScrollPercent,
                    scrollProvider.VerticalScrollPercent);
            }
        }

        #endregion Property

        public bool ScrollTo(DateTime date, bool selectItem = true, bool nearIfNotExists = true)
        {
            if (_dataSource is null) return false;

            ShinryouListViewItemSource itemSource = _dataSource.Items.FirstOrDefault(p => (p.診療日 == date));
            if (nearIfNotExists && (itemSource is null))
            {
                foreach (var item in _dataSource.Items)
                {
                    if (date < item.診療日) break;
                    itemSource = item;
                }
            }
            if (itemSource is null) return false;
            ScrollTo(itemSource, selectItem);
            return true;
        }

        public void ScrollTo(ShinryouListViewItemSource itemSource, bool selectItem = true)
        {
            if (itemSource is null) throw new ArgumentNullException("itemSource");
            if (_dataSource is null) return;
            try
            {
                _selectionBehavior.SelectedItem = itemSource;
                _selectionBehavior.SelectedItem.IsSelected = selectItem;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void ScrollTo(double horizontalPercent, double verticalPercent)
        {
            _scrollToThrottle.Signal(this, horizontalPercent, verticalPercent);
        }

        public void ScrollToBottom()
        {
            ScrollTo(ScrollPatternIdentifiers.NoScroll, 100d);
        }

        public void ScrollToBottomItem(bool selectLastItem)
        {
            if (_dataSource is null) return;

            if (_dataSource.Items.LastOrDefault() is ShinryouListViewItemSource item)
            {
                ScrollTo(item, selectLastItem);
            }
        }

        public void ScrollToTop()
        {
            ScrollTo(ScrollPatternIdentifiers.NoScroll, 0d);
        }

        public void ScrollToTopItem(bool selectFirstItem)
        {
            if (_dataSource is null) return;

            if (_dataSource.Items.FirstOrDefault() is ShinryouListViewItemSource item)
            {
                ScrollTo(item, selectFirstItem);
            }
        }

        public void SetDataSource(ShinryouListViewDataSource dataSource)
        {
            _dataSource = dataSource;

            if (dataSource is null)
            {
                _listView.ItemsSource = null;
                _txtItemsCount.Text = "0";
            }
            else
            {
                _listView.ItemsSource = dataSource.Groups;
                _txtItemsCount.Text = dataSource.Count.ToString();
            }
        }

        private void InternalScrollTo(double horizontalPercent, double verticalPercent)
        {
            if (_dataSource is null) return;

            // SetScrollPercent
            AutomationPeer peer = UIElementAutomationPeer.CreatePeerForElement(_listView);
            IScrollProvider scrollProvider = (IScrollProvider)peer.GetPattern(PatternInterface.Scroll);

            if (((horizontalPercent == ScrollPatternIdentifiers.NoScroll) || scrollProvider.HorizontallyScrollable) &&
                ((verticalPercent == ScrollPatternIdentifiers.NoScroll) || scrollProvider.VerticallyScrollable))
            {
                scrollProvider.SetScrollPercent(horizontalPercent, verticalPercent);
            }
        }

        private void ListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Handled) return;

            e.Handled = true;

            Control senderControl = (Control)sender;
            UIElement parent = (UIElement)senderControl.Parent;
            parent.RaiseEvent(
                new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = MouseWheelEvent,
                    Source = senderControl,
                });
        }

        private void TreeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
            {
                //here you would probably want to include code that is called by your
                //mouse down event handler.
                e.Handled = true;
            }
        }
    }

    public sealed class PixelBasedScrollingBehavior
    {
        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(PixelBasedScrollingBehavior), new UIPropertyMetadata(false, HandleIsEnabledChanged));

        private static void HandleIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is VirtualizingStackPanel vsp))
            {
                return;
            }

            PropertyInfo propIsPixelBased = typeof(VirtualizingStackPanel).GetProperty("IsPixelBased", BindingFlags.NonPublic | BindingFlags.Instance);

            if (propIsPixelBased is null)
            {
                throw new InvalidOperationException("Pixel-based scrolling behaviour hack no longer works!");
            }

            if ((bool)e.NewValue)
            {
                propIsPixelBased.SetValue(vsp, true, new object[0]);
            }
            else
            {
                propIsPixelBased.SetValue(vsp, false, new object[0]);
            }
        }
    }

    public sealed class NodeTreeSelectionBehavior : Behavior<TreeView>
    {
        public ShinryouListViewNode SelectedItem
        {
            get { return (ShinryouListViewNode)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(ShinryouListViewNode), typeof(NodeTreeSelectionBehavior),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));

        private static void OnSelectedItemChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is ShinryouListViewNode newNode)) return;

            NodeTreeSelectionBehavior behavior = (NodeTreeSelectionBehavior)dependencyObject;
            TreeView tree = behavior.AssociatedObject;

            var nodeDynasty = new List<ShinryouListViewNode> { newNode };

            ShinryouListViewGroupSource parent = newNode.Parent;
            while (parent != null)
            {
                nodeDynasty.Insert(0, parent);
                parent = parent.Parent;
            }

            ItemsControl currentParent = (ItemsControl)tree;

            foreach (ShinryouListViewNode node in nodeDynasty)
            {
                // first try the easy way
                if (!(currentParent.ItemContainerGenerator.ContainerFromItem(node) is TreeViewItem newParent))
                {
                    // if this failed, it's probably because of virtualization, and we will have to do it the hard way.
                    // this code is influenced by TreeViewItem.ExpandRecursive decompiled code, and the MSDN sample at http://code.msdn.microsoft.com/Changing-selection-in-a-6a6242c8/sourcecode?fileId=18862&pathId=753647475
                    // see also the question at http://stackoverflow.com/q/183636/46635
                    currentParent.ApplyTemplate();
                    if (currentParent.Template.FindName("ItemsHost", currentParent) is ItemsPresenter itemsPresenter)
                    {
                        itemsPresenter.ApplyTemplate();
                    }
                    else
                    {
                        currentParent.UpdateLayout();
                    }

                    VirtualizingPanel virtualizingPanel = GetItemsHost(currentParent) as VirtualizingPanel;
                    CallEnsureGenerator(virtualizingPanel);
                    int index = currentParent.Items.IndexOf(node);
                    if (index < 0)
                    {
                        throw new InvalidOperationException($"Node '{node}' cannot be found in container");
                    }
                    CallBringIndexIntoView(virtualizingPanel, index);
                    newParent = currentParent.ItemContainerGenerator.ContainerFromIndex(index) as TreeViewItem;
                }

                if (newParent is null)
                {
                    throw new InvalidOperationException($"Tree view item cannot be found or created for node '{node}'");
                }

                if (node == newNode)
                {
                    //newParent.IsSelected = true;
                    //newParent.BringIntoView(new Rect(newParent.RenderSize));
                    newParent.BringIntoView();
                    break;
                }

                newParent.IsExpanded = true;
                currentParent = newParent;
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = e.NewValue as ShinryouListViewNode;
        }

        #region Functions to get internal members using reflection

        // Some functionality we need is hidden in internal members, so we use reflection to get them

        #region ItemsControl.ItemsHost

        private static readonly PropertyInfo ItemsHostPropertyInfo = typeof(ItemsControl).GetProperty("ItemsHost", BindingFlags.Instance | BindingFlags.NonPublic);

        private static Panel GetItemsHost(ItemsControl itemsControl)
        {
            //Debug.Assert(itemsControl != null);
            return ItemsHostPropertyInfo.GetValue(itemsControl, null) as Panel;
        }

        #endregion ItemsControl.ItemsHost

        #region Panel.EnsureGenerator

        private static readonly MethodInfo EnsureGeneratorMethodInfo = typeof(Panel).GetMethod("EnsureGenerator", BindingFlags.Instance | BindingFlags.NonPublic);

        private static void CallEnsureGenerator(Panel panel)
        {
            //Debug.Assert(panel != null);
            EnsureGeneratorMethodInfo.Invoke(panel, null);
        }

        #endregion Panel.EnsureGenerator

        #region VirtualizingPanel.BringIndexIntoView

        private static readonly MethodInfo BringIndexIntoViewMethodInfo = typeof(VirtualizingPanel).GetMethod("BringIndexIntoView", BindingFlags.Instance | BindingFlags.NonPublic);

        private static void CallBringIndexIntoView(VirtualizingPanel virtualizingPanel, int index)
        {
            //Debug.Assert(virtualizingPanel != null);
            BringIndexIntoViewMethodInfo.Invoke(virtualizingPanel, new object[] { index });
        }

        #endregion VirtualizingPanel.BringIndexIntoView

        #endregion Functions to get internal members using reflection
    }
}