using MHLCommon;
using MHLCommon.ViewModels;
using MHLSourceOnDisk.BookDir;
using MHLSourceScanner.Configurations.DestinationFolder;
using MHLSourceScanner.Configurations.RowFolder;
using MHLSourceScannerLib.BookDir;
using System;
using System.Configuration;
using System.Text.Json;
using System.Windows.Forms;

namespace MHLSourceScanner.DirectorySetting
{
    internal class Model4DirSetting
    {
        #region[Methods]
        internal void AddRow2Parent(PathRowVM? selectedRow)
        {
            if (selectedRow != null)
            {
                PathRowVM newRow = new PathRowVM(selectedRow);

                newRow.IsFileName = selectedRow.IsFileName;
                selectedRow.SubRows.Add(newRow);
                selectedRow.ViewModel.IsFileName = false;
                selectedRow.ViewModel.IsExpanded = true;
                selectedRow.ViewModel.OnPropertyChanged("IsEnabled");
                newRow.ViewModel.IsSelected = true;
            }
        }

        internal bool CanAddRow2Parent(PathRowVM? selectedRow)
        {
            return (selectedRow?.SubRows?.Count ?? 0) == 0;
        }

        internal bool CanDeleteFromParent(PathRowVM? selectedRow)
        {
            return (selectedRow != null);
        }

        internal void DeleteRowFromParent(PathRowVM? selectedRow)
        {
            if (selectedRow != null)
            {
                if (selectedRow.Parent is PathRowVM row)
                {
                    row.SubRows.Remove(selectedRow);
                    row.ViewModel.IsFileName = selectedRow.IsFileName;
                    row.ViewModel.OnPropertyChanged("IsEnabled");
                }
            }
        }

        internal void LoadConfigurations(IVM4DirSetting vm)
        {
            Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            RowConfigSection section = (RowConfigSection)cfg.Sections["RowStructure"];
            if (section != null)
            {
                var str = section.RowItems[0].StructureJson;
                try
                {
                    PathRowVM? row = MHLCommonStatic.GetRowFromJson<PathRowVM>(str);
                    if (row != null)
                        vm.UpdatePathRowTree(row);
                }
                catch (Exception)
                {
                }
            }
        }

        internal void SaveConfigurations(PathRowVM row)
        {
             string jsonString = JsonSerializer.Serialize<PathRowVM?>(row);
             Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
             RowConfigSection section = (RowConfigSection)cfg.Sections["RowStructure"];
             if (section != null)
             {
                 section.RowItems[0].StructureJson = jsonString;
                 cfg.Save();
             }
        }
        #endregion
    }
}
