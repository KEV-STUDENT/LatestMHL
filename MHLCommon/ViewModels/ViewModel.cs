using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
