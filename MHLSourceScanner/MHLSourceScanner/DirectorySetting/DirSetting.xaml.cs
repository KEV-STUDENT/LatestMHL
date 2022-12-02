using System.Windows;
using System;
using System.Text.Json;
using MHLUIElements;
using MHLSourceScannerLib.BookDir;

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
            _vm.Close += () => {
                SaveData2Json();
                Close(); 
            };

            InitializeComponent();

            DataContext = this;
        }

        private void SaveData2Json()
        {
            string fileName = @"F:\1\WeatherForecast.json";
            PathRow? row = DirectoryTree.ViewModel.Source[0] as PathRow; 

            string jsonString = JsonSerializer.Serialize<PathRow?>(row);

            PathRow? row2 = JsonSerializer.Deserialize<PathRow?>(jsonString);
            // File.WriteAllText(fileName, jsonString);
            // Console.WriteLine(File.ReadAllText(fileName));
        }
    }
}
