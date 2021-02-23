using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp1.ViewModels.Base
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool Set<T>(ref T _field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(_field, value)) return false;
            _field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
