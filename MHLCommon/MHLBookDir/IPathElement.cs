using MHLCommon.ViewModels;
using System.Collections.ObjectModel;

namespace MHLCommon.MHLBookDir
{
    public interface IPathElement<T1, T2>
    {
        bool IsTyped { get; }
        string Name { get; }
        BookPathItem ElementType { get; }
        T1 TypedItem { get; set; }
        ObservableCollection<T1> Source { get; }
        T1 SelectedItem { get; set; }
        T2 ViewModel { get; }
    }
}