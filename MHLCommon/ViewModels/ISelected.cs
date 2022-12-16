using MHLCommon.MHLScanner;

namespace MHLCommon.ViewModels
{
    public interface ISelected
    {
        bool? IsSelected { get; set; }
        bool IsExported { get; set; }

        void SetSelecetdFromParent(ITreeItem? parent);
        void SetParentSelected(ITreeItem? parent, bool? value);
        void SetExportedFromParent(ITreeItem? parent);
        void SetParentExported(ITreeItem? parent, bool value);
    }
}