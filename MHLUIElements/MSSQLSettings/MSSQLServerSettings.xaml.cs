using MHLCommon.ViewModels;
using System.Windows;

namespace MHLUIElements.MSSQLSettings
{
    /// <summary>
    /// Логика взаимодействия для MSSQLServerSettings.xaml
    /// </summary>
    public partial class MSSQLServerSettings : Window
    {
        private IVMSettings _vm;

        public IVMSettings ViewModel
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
