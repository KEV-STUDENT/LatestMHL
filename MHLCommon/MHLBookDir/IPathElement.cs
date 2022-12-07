using MHLCommon.ViewModels;
using System.Collections.ObjectModel;

namespace MHLCommon.MHLBookDir
{
    public interface IPathElement
    {
        bool IsTyped { get; }
        string Name { get; }
        BookPathItem ElementType { get; }
        BookPathTypedItem ElementItemType { get; }
    }

    public interface IPathElement<T>  : IPathElement
    {
        ObservableCollection<T> Source { get; }
        T SelectedItem { get; set; }
    }


    public interface IPathElement<T1, T2> : IPathElement<T1>
    {
        T2 ViewModel { get; }
    }
}