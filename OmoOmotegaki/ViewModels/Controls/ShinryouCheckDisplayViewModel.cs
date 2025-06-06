using OmoOmotegaki.Data;
using OmoOmotegaki.Models;
using OmoSeitoku;
using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace OmoOmotegaki.ViewModels.Controls
{
    public sealed class ShinryouCheckDisplayViewModel : ViewModelBase
    {

        private readonly IShinryouDataCollectionSource _shinryouDataCollectionSource;

        public ShinryouCheckDisplayViewModel(IShinryouDataCollectionSource shinryouDataCollectionSource)
        {
            _shinryouDataCollectionSource = shinryouDataCollectionSource;
        }

        public event EventHandler DateRangeChanged;
        public event EventHandler ShinryouCheckResultsChanged;
        public event EventHandler InputNumbersChanged;

        #region Property

        #region DateRange

        public DateRange? DateRange
        {
            get { return __prop_DateRange; }
            set
            {
                if (SetProperty(ref __prop_DateRange, value))
                {
                    DateRangeChanged?.Invoke(this, EventArgs.Empty);
                    RaisePropertyChanged(nameof(DateRangeDisplay));
                }
            }
        }
        private DateRange? __prop_DateRange;

        public string DateRangeDisplay
        {
            get => DateRange.HasValue
                        ? $"{DateRange.Value.Min.ToShortDateString()} ～ {DateRange.Value.Max.ToShortDateString()}"
                        : "-";
        }

        #endregion

        #region ShinryouCheckResults

        public IReadOnlyList<ShinryouCheckResult> ShinryouCheckResults
        {
            get { return __prop_ShinryouCheckResults; }
            set
            {
                if (SetProperty(ref __prop_ShinryouCheckResults, value))
                {
                    ShinryouCheckResultsChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private IReadOnlyList<ShinryouCheckResult> __prop_ShinryouCheckResults = Array.Empty<ShinryouCheckResult>();

        #endregion

        #region InputNumbers

        public IReadOnlyList<int> InputNumbers
        {
            get { return __prop_InputNumbers; }
            private set
            {
                if (SetProperty(ref __prop_InputNumbers, value))
                {
                    InputNumbersChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private IReadOnlyList<int> __prop_InputNumbers;

        #endregion

        #region InputQuickCheckText

        public string InputQuickCheckText
        {
            get { return __prop_InputQuickCheckText; }
            set
            {
                if (SetProperty(ref __prop_InputQuickCheckText, value))
                {
                    if (string.IsNullOrWhiteSpace(value) || value == "0")
                    {
                        InputNumbers = null;
                        return;
                    }

                    MatchCollection matches = Regex.Matches(value, @"[0-9０-９]+");

                    if (matches.Count == 0)
                    {
                        ShinryouCheckResults = new[]
                        {
                            new ShinryouCheckResult("エラー: 数値のみ入力できます。"),
                        };
                        return;
                    }

                    InputNumbers = matches
                                    .Cast<Match>()
                                    .Select(m => ParseInt(m.Value))
                                    .ToArray();


                    static int ParseInt(string value)
                    {
                        char[] ar = value.ToCharArray();
                        for (int i = ar.Length - 1; i >= 0; i--)
                        {
                            char c = ar[i];

                            switch (c)
                            {
                                case '０': ar[i] = '0'; break;
                                case '１': ar[i] = '1'; break;
                                case '２': ar[i] = '2'; break;
                                case '３': ar[i] = '3'; break;
                                case '４': ar[i] = '4'; break;
                                case '５': ar[i] = '5'; break;
                                case '６': ar[i] = '6'; break;
                                case '７': ar[i] = '7'; break;
                                case '８': ar[i] = '8'; break;
                                case '９': ar[i] = '9'; break;
                            }
                        }

                        return int.Parse(new string(ar));
                    }
                }

                // 変更されたかどうかにかかわらず更新
                // （QuickCheckSettings の内容が変更されているか不明なため）
                RefreshCheckDisplay();
            }
        }
        private string __prop_InputQuickCheckText;

        #endregion

        #region QuickCheckSettings

        public ShinryouCheckRule QuickCheckSettings
        {
            get => __prop_QuickCheckSettings;
            private set
            {
                SetProperty(ref __prop_QuickCheckSettings, value);
            }
        }
        private ShinryouCheckRule __prop_QuickCheckSettings = new ShinryouCheckRule();

        #endregion

        #endregion

        public void ClearDisplay()
        {
            DateRange = default;
            ShinryouCheckResults = Array.Empty<ShinryouCheckResult>();
        }

        public void RefreshCheckDisplay()
        {
            RefreshDisplay(_shinryouDataCollectionSource);
        }

        public void RefreshDisplay(IShinryouDataCollectionSource shinryouDataCollectionSource)
        {
            if (shinryouDataCollectionSource is null) { throw new ArgumentNullException(nameof(shinryouDataCollectionSource)); }

            ClearDisplay();

            List<ShinryouCheckRule> checkRules = ClientShareSettings.Instance.ShinryouCheckSyochiList.ToList();

            // クイックチェックを追加
            if ((InputNumbers != null) && (0 < InputNumbers.Count))
            {
                foreach (var rule in InputNumbers
                    .Select(p => new ShinryouCheckRule(p)
                    {
                        RuleType = QuickCheckSettings.RuleType,
                        CheckAction = QuickCheckSettings.CheckAction,
                    })
                    .Reverse())
                {
                    checkRules.Insert(0, rule);
                }
            }

            IEnumerable<SinryouData> notFilteredShinryouDataCollection = 
                shinryouDataCollectionSource.ShinryouDataCollection?.GetNonFiltered();

            if (notFilteredShinryouDataCollection is null)
            {
                ShinryouCheckResults = new[] { new ShinryouCheckResult("エラー: データがありません。") };
                return;
            }

            DateRange = shinryouDataCollectionSource.CurrentDateRange;

            ShinryouCheckResults = checkRules
                .Select(checkRule => checkRule.CheckAction?.Check(checkRule.ShochiId, notFilteredShinryouDataCollection))
                .Where(res => (res is object) && !string.IsNullOrWhiteSpace(res.Title))
                .ToArray();
        }
    }
}
