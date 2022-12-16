using MHLCommon.ViewModels;
using System.Collections.ObjectModel;

namespace MHLCommon.MHLBookDir
{
    public interface IPathElement
    {
        bool IsTyped { get; }       
        BookPathItem ElementType { get; }
        BookPathTypedItem ElementItemType { get; }
    }

    public interface IPathElementUI<out T1, in T2, out T3> : IPathElement
        where T1 : IElementTypeUI
        where T2 : IElementTypeUI
        where T3 : ViewModel
    {
        string Name { get; }
        T3 ViewModel { get; }
        IEnumerable<T1> Source { get; }
        T1 SelectedItem { get; }
        void SetSelectedItem(T2 selectedItem);
    }
}