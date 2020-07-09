using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp1.ViewModel
{
    class BaseHelper : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChnge([CallerMemberName] string propy = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propy));
        }
    }
}
