using MHLCommon.MHLScanner;

namespace MHLCommon.ViewModels
{
    public interface ISelected
    {
        bool? IsSelected { get; set; }
        void SetSelecetdFromParent(ITreeItem? parent);
        void SetParentSelected(ITreeItem? parent, bool? value);
    }
}