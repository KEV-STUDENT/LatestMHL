using MHLCommon;
using MHLCommon.MHLScanner;
using MHLControls.MHLPickers;
using MHLSourceScannerLib;
using MHLUIElements;
using System.ComponentModel;
using System.Windows;
using MHLResources;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using System;
using MHLCommon.ViewModels;

namespace MHLSourceScanner.DirectorySettings
{
    /// <summary>
    /// Логика взаимодействия для DirectorySetting.xaml
    /// </summary>
    /// 
    public partial class DirSetting : Window
    {
        private ViewModel4DirSetting _vm;
        public ViewModel4DirSetting ViewModel
        {
            get { return _vm; }
        }
        public DirSetting()
        {
            _vm = new ViewModel4DirSetting();
            _vm.Close += () => { Close(); };

            InitializeComponent();

            DataContext = this;
        }
    }
}
