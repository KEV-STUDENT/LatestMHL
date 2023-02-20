using MHLCommon.MHLBookDir;
using System.Windows.Input;

namespace MHLCommon.ViewModels
{
    public interface IVM4DirSetting : IVMSettings
    {
        ICommand AddRowCommand { get; set; }
        ICommand DeleteRowCommand { get; set; }

        void UpdatePathRowTree(IPathRow row);
    }
}