using MHLSourceScanner.Configurations.RowFolder;
using MHLSourceScannerLib.BookDir;
using System;
using System.Configuration;
using System.Text.Json;
using System.Windows;

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
            _vm = new ViewModel4DirSetting(this);
            InitializeComponent();
            _vm.LoadDataFromConfig();
            DataContext = this;
        }

        private void SaveData2Json()
        {
            PathRowVM? row = DirectoryTree.ViewModel.Source[0];

            string jsonString = JsonSerializer.Serialize<PathRowVM?>(row);


            Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            RowConfigSection section = (RowConfigSection)cfg.Sections["RowStructure"];
            if (section != null)
            {
                section.RowItems[0].StructureJson = jsonString;
                cfg.Save();
            }
        }
    }
}
