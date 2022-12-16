using MHLCommon.ViewModels;
using System.Collections.ObjectModel;

namespace MHLCommon.MHLBookDir
{
    public interface IPathRowElement{}
    public interface IPathRowElement<out T> : IPathRowElement
        where T:IPathElement
    {
        T SelectedItem { get; }
    }

   public interface IPathRowElementUI<out T1,in T2, out T3> : IPathRowElement<T1>
        where T1 : IPathElement
        where T2 : IPathElement
        where T3 : ViewModel
    {
        IEnumerable<T1> Source { get; }      
        void SetSelectedItem(T2 selected);
        T3 ViewModel { get; }
    }
}
