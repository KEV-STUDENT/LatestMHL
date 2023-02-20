using MHLControls.ViewModels4Forms;
using MHLCommon.ViewModels;
using System.Windows.Input;

namespace MHLUIElements.MSSQLSettings
{
    internal class ViewModel4MSSqlSetting : VMEditForm, IVM4MSSqlSetting
    {
        #region [Fields]
        private MSSQLServerSettings _view;
        private Model4MSSqlSetting _model;
        #endregion

        #region [Constructor]
        public ViewModel4MSSqlSetting(MSSQLServerSettings view)
        {
            _view = view;
            _model = new Model4MSSqlSetting();


            Close += () =>
            {
                _view.Close();
            };

            Run += () =>
            {
                _view.Close();
            };
        }
        #endregion

        #region[IVM4MSSqlSetting]
        ICommand IVMSettings.CloseCommand { get => CloseCommand; set => CloseCommand = value; }
        ICommand IVMSettings.RunCommand { get => RunCommand; set => RunCommand = value; }

        void IVMSettings.LoadDataFromConfig()
        {
            _view.SettingsControl.ViewModel.LoadData();
        }
        #endregion
    }
}