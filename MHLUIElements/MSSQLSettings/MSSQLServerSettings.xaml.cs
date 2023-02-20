using MHLCommon.ViewModels;
using System.Windows;

namespace MHLUIElements.MSSQLSettings
{
    /// <summary>
    /// Логика взаимодействия для MSSQLServerSettings.xaml
    /// </summary>
    public partial class MSSQLServerSettings : Window
    {
        private IVM4MSSqlSetting _vm;

        public IVM4MSSqlSetting ViewModel
        {
            get { return _vm; }
        }

        public MSSQLServerSettings()
        {
            _vm = new ViewModel4MSSqlSetting(this);
            InitializeComponent();
            _vm.LoadDataFromConfig();
            DataContext = this;
        }
    }
}
