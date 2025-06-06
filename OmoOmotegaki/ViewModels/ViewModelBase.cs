using Livet;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OmoOmotegaki.ViewModels
{
    public abstract class ViewModelBase : ViewModel
    {
        protected bool SetProperty<T>(ref T property, T newValue,
            [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(property, newValue))
            {
                return false;
            }
            else
            {
                property = newValue;
                RaisePropertyChanged(propertyName);
                return true;
            }
        }

        public ViewModelBase()
        {
            PropertyChanged += OnPropertyChanged;
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
