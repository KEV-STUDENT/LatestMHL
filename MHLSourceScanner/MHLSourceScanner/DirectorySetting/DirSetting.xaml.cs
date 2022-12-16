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
            _vm = new ViewModel4DirSetting();
            _vm.Close += () => {
                Close(); 
            };

            _vm.Run += () =>
            {
                SaveData2Json();
                Close();
            };

            InitializeComponent();

            LoadDataFromJson();
            DataContext = this;
        }

        private void LoadDataFromJson()
        {
            Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            RowConfigSection section = (RowConfigSection)cfg.Sections["RowStructure"];
            if (section != null)
            {
               var str = section.RowItems[0].StructureJson;
               try
               {
                    PathRowVM? row = JsonSerializer.Deserialize<PathRowVM>(str);
                    DirectoryTree.ViewModel.Source.Clear();
                    if (row != null)
                    {
                        DirectoryTree.ViewModel.Source.Add(row);
                        while ((row?.SubRows?.Count ?? 0) > 0)
                        {
                            if (row != null)
                            {
                                foreach (PathRowVM subRow in row.SubRows)
                                {
                                    subRow.Parent = row;
                                }
                                row = row.SubRows[0];
                            }
                        }
                        if (row != null)
                            row.ViewModel.IsSelected = true;
                    }
                }
                catch (Exception e)
                { 
                }
            }
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
