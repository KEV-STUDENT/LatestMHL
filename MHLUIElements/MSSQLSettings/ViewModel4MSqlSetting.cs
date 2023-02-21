using MHLControls.ViewModels4Forms;
using MHLCommon.ViewModels;
using System.Windows.Input;

namespace MHLUIElements.MSSQLSettings
{
    internal class ViewModel4MSSqlSetting : VMEditForm, IVMSettings
    {
        #region [Fields]
        private MSSQLServerSettings _view;
        #endregion

        #region [Constructor]
        public ViewModel4MSSqlSetting(MSSQLServerSettings view)
        {
            _view = view;
            Close += () =>
            {
                _view.Close();
            };

            Run += () =>
            {
                _view.SettingsControl.ViewModel.SaveData();
                _view.Close();
            };
        }
        #endregion

        #region[IVMSettings]
        ICommand IVMSettings.CloseCommand { get => CloseCommand; set => CloseCommand = value; }
        ICommand IVMSettings.RunCommand { get => RunCommand; set => RunCommand = value; }

        void IVMSettings.LoadDataFromConfig()
        {
            _view.SettingsControl.ViewModel.LoadData();
        }
        #endregion
    }
}