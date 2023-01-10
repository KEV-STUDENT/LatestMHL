using MHLCommon;
using MHLCommon.MHLDiskItems;
using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using MHLSourceOnDisk;
using MHLSourceOnDisk.BookDir;
using MHLSourceScanner.Configurations.DestinationFolder;
using MHLSourceScanner.Configurations.RowFolder;
using MHLSourceScanner.Configurations.SourceFolder;
using MHLSourceScannerLib;
using MHLSourceScannerModelLib;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MHLSourceScanner
{
    internal class Model4Scanner
    {
        internal async Task ExportSelectedDataAsync(ObservableCollection<ITreeItem>? source, string destinationPath)
        {
            if (source == null)
                return;
            
            PathRowDisk? row = null;
            Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            RowConfigSection section = (RowConfigSection)cfg.Sections["RowStructure"];
            if (section != null)
            {
                var str = section.RowItems[0].StructureJson;
                try
                {
                    row = MHLCommonStatic.GetRowFromJson<PathRowDisk>(str);
                }
                catch (Exception)
                {
                }
            }

            ExpOptions expOptions = new ExpOptions(destinationPath, false, row);
            
            await ExportSelectedItemsAsync(source, expOptions);
        }

        internal async Task ExportSelectedItemsAsync(ObservableCollection<ITreeItem> collection, ExpOptions expOptions)
        {
            await Parallel.ForEachAsync<ITreeItem>(collection, async (item, cancelToken) =>
            {
                bool? continueExport = CheckItem4Export(item);

                if (continueExport ?? true)
                {
                    if (item is TreeDiskItem diskItem)
                    {
                        if ((continueExport ?? false) &&(diskItem.Source != null))
                        {
                            Export2Dir exporter = new Export2Dir(expOptions, diskItem.Source);
                            await diskItem.ExportItemAsync(exporter);
                        }
                        else
                        {
                            await ExportSelectedItemsAsync(diskItem.SourceItems, expOptions);
                        }
                    }
                }
            });
        }

        private static bool? CheckItem4Export(ITreeItem item)
        {
            bool? result = item switch
            {
                TreeViewZip zip => zip.Selected,
                TreeViewVirtualGroup vg => vg.Selected,
                TreeViewFB2 fb2 => fb2.Selected ?? false,
                TreeViewDirectory dir => dir.ChildsLoaded && dir.SourceItems.Count > 0 ? null : false,
                _ => false,
            };
            return result;
        }

        internal void SaveConfigurations(string sourcePath, int type, string destination)
        {
            Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            SourceConfigSection section = (SourceConfigSection)cfg.Sections["SourceFolders"];
            if (section != null)
            {
                section.FolderItems[0].Path = sourcePath;
            }

            DestinationConfigSection destinationSection = (DestinationConfigSection)cfg.Sections["DestinationFolders"];
            if (destinationSection != null)
            {
                destinationSection.FolderItems[0].PathType = type;
                destinationSection.FolderItems[0].Path = destination;
            }
            cfg.Save();
        }

        internal void ChangeSourceDir(string? sourcePath, IShower shower)
        {
            if (shower != null)
            {
                ITreeDiskItem treeViewDiskItem = new TreeViewDirectory(sourcePath ?? string.Empty, shower, null);
                shower.Clear(treeViewDiskItem);
                shower.UpdateView(treeViewDiskItem);
            }
        }
    }
}
