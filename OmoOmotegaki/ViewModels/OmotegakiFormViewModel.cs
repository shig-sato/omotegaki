using OmoOmotegaki.Data;
using OmoOmotegaki.Models;
using OmoSeitoku;
using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OmoOmotegaki.ViewModels
{
    public sealed class ShowMessageEventArgs : EventArgs
    {
        public string Message { get; }
        public bool IsError { get; }
        public bool IsLogging { get; }

        public ShowMessageEventArgs(string message, bool isError, bool isLogging)
        {
            this.Message = message ?? throw new ArgumentNullException(nameof(message));
            this.IsError = isError;
            this.IsLogging = isLogging;
        }
    }
    public sealed class KarteDataLoadedEventArgs : EventArgs
    {
        public KarteDataLoadedEventArgs()
        {
        }
    }

    public class OmotegakiFormViewModel : ViewModelBase, IShinryouDataCollectionSource
    {
        public OmotegakiFormViewModel()
        {
        }

        public event EventHandler<KarteDataLoadedEventArgs> KarteDataLoaded;
        public event EventHandler<ShowMessageEventArgs> ShowMessage;
        public event EventHandler CheckedShoshinKikanItemsChanged;

        private SinryouDataLoader _shinryouDataLoader;

        #region Properties

        public bool IsKarteLoading
        {
            get => __prop_IsKarteLoading;
            private set => SetProperty(ref __prop_IsKarteLoading, value);
        }
        private bool __prop_IsKarteLoading;

        public string StatusLabelカルテタイトル
        {
            get => __prop_StatusLabelカルテタイトル;
            set => SetProperty(ref __prop_StatusLabelカルテタイトル, value);
        }
        private string __prop_StatusLabelカルテタイトル;

        public string Title
        {
            get => __prop_Title;
            set => SetProperty(ref __prop_Title, value);
        }
        private string __prop_Title;

        public string TitleBase
        {
            get => __prop_TitleBase;
            set => SetProperty(ref __prop_TitleBase, value);
        }
        private string __prop_TitleBase;

        public DateRange? CurrentDateRange
        {
            get => __prop_CurrentDateRange;
            set => SetProperty(ref __prop_CurrentDateRange, value);
        }
        private DateRange? __prop_CurrentDateRange;

        public KarteId CurrentKarteId
        {
            get => __prop_CurrentKarteId;
            set => SetProperty(ref __prop_CurrentKarteId, value);
        }
        private KarteId __prop_CurrentKarteId;

        public KarteData CurrentKarteData
        {
            get => __prop_CurrentKarteData;
            set => SetProperty(ref __prop_CurrentKarteData, value);
        }
        private KarteData __prop_CurrentKarteData;

        public IReadOnlyList<ShoshinKikanItem> ShoshinKikanItems
        {
            get => __prop_ShoshinListItems;
            private set
            {
                var old = __prop_ShoshinListItems;

                if (SetProperty(ref __prop_ShoshinListItems, value))
                {
                    if (value?.Count > 0)
                    {
                        foreach (ShoshinKikanItem item in value)
                        {
                            item.IsCheckedChanged += ShoshinKikanItem_IsCheckedChanged;
                        }
                    }

                    if (old?.Count > 0)
                    {
                        foreach (ShoshinKikanItem item in old)
                        {
                            item.IsCheckedChanged -= ShoshinKikanItem_IsCheckedChanged;
                        }
                    }
                }
            }
        }

        private IReadOnlyList<ShoshinKikanItem> __prop_ShoshinListItems = Array.Empty<ShoshinKikanItem>();

        public ShinryouDataCollection ShinryouDataCollection
        {
            get => __prop_ShinryouDataCollection;
            private set => SetProperty(ref __prop_ShinryouDataCollection, value);
        }
        private ShinryouDataCollection __prop_ShinryouDataCollection;

        #endregion


        public void Clear()
        {
            ShinryouDataCollection = null;
            CurrentDateRange = null;
            CurrentKarteId = default;
            CurrentKarteData = null;
            ShoshinKikanItems = Array.Empty<ShoshinKikanItem>();
        }

        /// <summary>
        /// カルテ・診療情報を読み込む
        /// </summary>
        /// <param name="uncheckSyosinList">初診リストのチェック状態を復帰しない</param>
        public void LoadData(
            KarteId karteId,
            bool isRefresh,
            IReadOnlyList<DateRange2> targetKikans,
            SinryouDataLoader.診療統合種別 shinryouTougou,
            bool uncheckSyosinList)
        {
            try
            {
                IsKarteLoading = true;

                InternalLoadData(karteId, isRefresh, shinryouTougou, uncheckSyosinList, latestShoshinKikanOnly: false, targetKikans);
            }
            finally
            {
                IsKarteLoading = false;
            }
        }

        public void LoadDataLatestShoshinKikan(
            KarteId karteId,
            bool isRefresh,
            SinryouDataLoader.診療統合種別 shinryouTougou,
            bool uncheckSyosinList,
            out DateRange targetKikan)
        {
            try
            {
                IsKarteLoading = true;

                InternalLoadData(karteId, isRefresh, shinryouTougou, uncheckSyosinList, latestShoshinKikanOnly: true);
                targetKikan = (ShoshinKikanItems?.Count > 0)
                                ? new DateRange(ShoshinKikanItems.Last().Date, DateTime.MaxValue)
                                : DateRange.All;
            }
            finally
            {
                IsKarteLoading = false;
            }
        }

        private void InternalLoadData(
            KarteId karteId,
            bool isRefresh,
            SinryouDataLoader.診療統合種別 shinryouTougou,
            bool uncheckSyosinList,
            bool latestShoshinKikanOnly,
            IReadOnlyList<DateRange2> targetKikans = null)
        {
            // データクリア前に選択されている初診期間の開始日を取得しておく
            DateTime[] selectedDates =
                uncheckSyosinList
                    ? Array.Empty<DateTime>()
                    : ShoshinKikanItems
                            .Where(p => p.IsChecked)
                            .Select(p => p.Date)
                            .ToArray();

            Clear();

            // カルテデータ
            KarteData karteData;
            try
            {
                using var karteFs = new KarteFileStream(karteId.Shinryoujo);
                karteData = karteFs.GetKarteData(karteId.KarteNumber);
            }
            catch (Exception ex)
            {
                ShowMessage?.Invoke(this, new ShowMessageEventArgs(ex.Message, true, true));

                return;
            }

            CurrentKarteId = karteId;
            CurrentKarteData = karteData;


            // 診療データローダー
            if (_shinryouDataLoader != null &&
                _shinryouDataLoader.KarteId == karteId)
            {
                // 読込設定が前回と同じ場合 前回のローダーを使用する
            }
            else
            {
                // 読込設定が前回と異なる場合 ローダーを新たに作成する

                try
                {
                    using var shinryouFs = new ShinryouFileStream(karteId.Shinryoujo);
                    _shinryouDataLoader = shinryouFs.GetSinryouDataLoader(karteId);
                }
                catch (Exception ex)
                {
                    _shinryouDataLoader = null;

                    ShowMessage?.Invoke(this, new ShowMessageEventArgs(ex.Message, true, true));
                }
            }

            // 初診リスト
            if (_shinryouDataLoader != null)
            {
                ShoshinKikanItem[] shoshinKikanItems = _shinryouDataLoader.Get初診日リスト()
                                                            .Select(p => new ShoshinKikanItem(p))
                                                            .ToArray();

                if (latestShoshinKikanOnly)
                {
                    // 初診リスト最新の期間のみチェック
                    DateRange2[] tempLatestShoshinKikan = new[] { DateRange2.Infinite };
                    bool isLatest = true;
                    foreach (ShoshinKikanItem item in shoshinKikanItems.Reverse())
                    {
                        item.IsChecked = isLatest;

                        if (isLatest)
                        {
                            tempLatestShoshinKikan[0] = new DateRange2(item.Date, null);
                            isLatest = false;
                        }
                    }

                    targetKikans = tempLatestShoshinKikan;
                }
                else if (isRefresh)
                {
                    // リスト更新前のチェックを復帰
                    if (0 < selectedDates.Length)
                    {
                        foreach (ShoshinKikanItem item in shoshinKikanItems)
                        {
                            if (selectedDates.Contains(item.Date))
                            {
                                // チェックの復帰
                                item.IsChecked = true;
                            }
                        }
                    }
                }
                else
                {
                    // すべてチェック
                    foreach (var item in shoshinKikanItems)
                    {
                        item.IsChecked = true;
                    }
                }

                ShoshinKikanItems = shoshinKikanItems;
            }

            // 診療データ
            if (_shinryouDataLoader != null)
            {
                // 処置履歴リストアイテム追加
                IEnumerable<SinryouData> shinryouDataCollection = (targetKikans is null)
                    ? _shinryouDataLoader.GetShinryouData(DateRange.All, shinryouTougou)
                    : targetKikans
                        .Select(p => p.ToDateRange())
                        .SelectMany(kikan => _shinryouDataLoader.GetShinryouData(kikan, shinryouTougou));

                ShinryouDataCollection = new ShinryouDataCollection(shinryouDataCollection);


                if (ShinryouDataCollection.Count == 0)
                {
                    CurrentDateRange = null;
                }
                else
                {
                    CurrentDateRange = new DateRange(
                        ShinryouDataCollection.First().診療日,
                        ShinryouDataCollection.Last().診療日);
                }
            }
        }

        public void UpdateTitle()
        {
            var sb = new StringBuilder();

            if (CurrentKarteId != default)
            {
                sb.Append(CurrentKarteData?.Get氏名(false)).Replace(" ", "").Replace("　", "")
                    .Append('[')
                    .Append(CurrentKarteId.ToString())
                    .Append("] ");
            }

            StatusLabelカルテタイトル = sb.ToString();

            sb.Append(global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder)
                .Append(" - ")
                .Append(TitleBase);

            Title = sb.ToString();
        }

        internal List<DateRange2> GetKikanBySyosinList()
        {
            (ShoshinKikanItem Item, int Index)[] checkedItems = ShoshinKikanItems
                .Select((p, index) => (Item: p, Index: index))
                .Where(p => p.Item.IsChecked)
                .ToArray();

            if (ShoshinKikanItems.Count == checkedItems.Length)
            {
                return new List<DateRange2> { DateRange2.Infinite };
            }
            else if (ShoshinKikanItems.Count == 0)
            {
                return new List<DateRange2>();
            }

            var result = new List<DateRange2>();

            foreach ((ShoshinKikanItem startDateItem, int checkedItemIndex) in checkedItems)
            {
                var kikan = new DateRange2(min: startDateItem.Date, max: null);
                int nextItemIndex = checkedItemIndex + 1;

                if (nextItemIndex < ShoshinKikanItems.Count)
                {
                    // 選択した期間から1つ次の期間の前日までを期間範囲とする
                    ShoshinKikanItem endDateItem = ShoshinKikanItems[nextItemIndex];
                    kikan.Max = endDateItem.Date.AddDays(-1);
                }

                result.Add(kikan);
            }
            return result;
        }

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(sender, e);

            switch (e.PropertyName)
            {
                case nameof(TitleBase):
                    UpdateTitle();
                    break;
                case nameof(CurrentKarteData):
                    if (!(CurrentKarteData is null))
                    {
                        KarteDataLoaded?.Invoke(this, new KarteDataLoadedEventArgs());
                    }
                    UpdateTitle();
                    break;
            }
        }


        private void ShoshinKikanItem_IsCheckedChanged(object sender, EventArgs e)
        {
            CheckedShoshinKikanItemsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

}
