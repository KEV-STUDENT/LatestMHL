using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MHLCommon.MHLScanner;

namespace MHLCommon.MHLBookDir
{
    public interface IPathRowElement<T1, T2>
    {
        T2 ViewModel { get; }
        ObservableCollection<T1> Source { get; }
        T1 SelectedItem { get; set; }
}
}
