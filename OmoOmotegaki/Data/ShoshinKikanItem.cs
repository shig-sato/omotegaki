using System;

namespace OmoOmotegaki.Data
{
    public sealed class ShoshinKikanItem : ViewModels.ViewModelBase
    {
        public ShoshinKikanItem(DateTime date)
        {
            Date = date;
        }

        public event EventHandler IsCheckedChanged;

        public string DisplayName
        {
            get
            {
                string format;
                try
                {
                    var era = OmoEReceLib.ERDateTime.GetEraYear(this.Date, true).Replace(" ", "");
                    format = string.Format("({0}) yyyy-MM-dd ～", era);
                }
                catch (Exception)
                {
                    format = "yyyy-MM-dd ～";
                }
                return this.Date.ToString(format);
            }
        }

        public DateTime Date
        {
            get => __prop_Date;
            set => SetProperty(ref __prop_Date, value);
        }
        private DateTime __prop_Date;

        public bool IsChecked
        {
            get => __prop_IsChecked;
            set
            {
                if (SetProperty(ref __prop_IsChecked, value))
                {
                    IsCheckedChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private bool __prop_IsChecked;
    }
}
