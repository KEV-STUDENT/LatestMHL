using MHLCommon.ViewModels;
using System.Windows;

namespace MHLSourceScanner.DirectorySettings
{
    /// <summary>
    /// Логика взаимодействия для DirectorySetting.xaml
    /// </summary>
    /// 
    public partial class DirSetting : Window
    {
        private IVM4DirSetting _vm;
        public IVM4DirSetting ViewModel
        {
            get { return _vm; }
        }
        public DirSetting()
        {
            _vm = new ViewModel4DirSetting(this);
            InitializeComponent();
            _vm.LoadDataFromConfig();
            DataContext = this;
        }       
    }
}
