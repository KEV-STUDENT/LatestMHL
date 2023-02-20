using System.Windows.Input;

namespace MHLCommon.ViewModels
{
    public interface IVMSettings
    {
        void LoadDataFromConfig();
        public ICommand CloseCommand { get; set; }
        public ICommand RunCommand { get; set; }
    }
}