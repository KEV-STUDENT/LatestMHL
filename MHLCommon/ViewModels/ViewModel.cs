using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MHLCommon.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        private PropertyChangedEventHandler? eventHandler;

        event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                eventHandler += value;
            }

            remove
            {
                eventHandler -= value;
            }
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (eventHandler != null)
                eventHandler(this, new PropertyChangedEventArgs(prop));
        }
    }
}
