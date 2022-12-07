using MHLSourceScanner.Configurations.RowFolder;
using MHLSourceScannerLib.BookDir;
using System.Configuration;
using System.Text.Json;
using System.Windows;
using System;
using MHLSourceScanner.Configurations.SourceFolder;

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
                    PathRow row = JsonSerializer.Deserialize<PathRow>(str);
                    DirectoryTree.ViewModel.Source.Clear();
                    DirectoryTree.ViewModel.Source.Add(row);

                    while(row.SubRows.Count > 0)
                    {
                        foreach(PathRow subRow in row.SubRows) 
                        {
                            subRow.Parent = row;
                        }
                        row = row.SubRows[0];
                    }

                    row.ViewModel.IsSelected = true;
                }
                catch (Exception e)
                { 
                }
            }
        }
        private void SaveData2Json()
        {
            PathRow? row = DirectoryTree.ViewModel.Source[0] as PathRow;

            string jsonString = JsonSerializer.Serialize<PathRow?>(row);


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
