using MHLCommon;
using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using MHLCommon.MHLDiskItems;
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
using System.Collections.Concurrent;
using System.Windows.Media;
using System.Reflection.Metadata;

namespace MHLSourceScanner
{
    internal class Model4Scanner
    {
        internal async Task ExportSelectedData2DirAsync(ObservableCollection<ITreeItem>? source, string destinationPath)
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

            await ExportSelectedItemsAsync(source, expOptions, ExportEnum.Directory);
        }

        internal async Task ExportSelectedData2SQLiteAsync(ObservableCollection<ITreeItem>? source, string fileSQlite)
        {
            if (source == null)
                return;
            
            await Task.Run(
                () => {
                        ExpOptions expOptions = new ExpOptions(fileSQlite);
                        ExportSelectedItems(source, expOptions, ExportEnum.SQLite);
                });


            //ExportSelectedData2SQLite(source, fileSQlite);

            //ExpOptions expOptions = new ExpOptions(fileSQlite);
            //ExportSelectedItems(source, expOptions, ExportEnum.SQLite);
        }

        internal Task ExportSelectedData2SQLite(ObservableCollection<ITreeItem>? source, string fileSQlite)
        {
            return Task.Run(
                ()=> {
                    if (source != null)
                    {
                        ExpOptions expOptions = new ExpOptions(fileSQlite);
                        ExportSelectedItems(source, expOptions, ExportEnum.SQLite);
                    }
                });
        }

        internal void ExportSelectedItems(ObservableCollection<ITreeItem> collection, ExpOptions expOptions, ExportEnum exportType)
        {
            foreach(ITreeItem item in collection)
            { 
                bool? continueExport = CheckItem4Export(item);

                if (continueExport ?? true)
                {
                    if (item is TreeDiskItem diskItem)
                    {
                        if ((continueExport ?? false) && (diskItem.Source != null))
                        {
                            IExport? exporter;
                            switch (exportType)
                            {
                                case ExportEnum.SQLite:
                                    exporter = new Export2SQLite(expOptions);
                                    break;
                                default:
                                    exporter = null; break;
                            }

                            if (exporter != null)
                                diskItem.ExportItemAsync(exporter).Wait();
                        }
                        else
                        {
                            ExportSelectedItems(diskItem.SourceItems, expOptions, exportType);
                        }
                    }
                }
            }
        }

        internal async Task ExportSelectedItemsAsync(ObservableCollection<ITreeItem> collection, ExpOptions expOptions, ExportEnum exportType)
        {
            await Parallel.ForEachAsync<ITreeItem>(collection, async (item, cancelToken) =>
            {
                bool? continueExport = CheckItem4Export(item);

                if (continueExport ?? true)
                {
                    if (item is TreeDiskItem diskItem)
                    {
                        if ((continueExport ?? false) && (diskItem.Source != null))
                        {
                            IExport? exporter;
                            switch (exportType)
                            {
                                case ExportEnum.Directory:
                                    exporter = new Export2Dir(expOptions, diskItem.Source);
                                    break;
                                default:
                                    exporter = null; break;
                            }

                            if (exporter != null)
                                await diskItem.ExportItemAsync(exporter);
                        }
                        else
                        {
                            await ExportSelectedItemsAsync(diskItem.SourceItems, expOptions, exportType);
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

        internal void SaveConfigurations(string sourcePath, ExportEnum type, string destinationDir, string destinationDB)
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
                destinationSection.FolderItems[0].PathType = (int)type;
                destinationSection.FolderItems[0].Path = destinationDir;
                destinationSection.FolderItems[0].Path4SQLite = destinationDB;
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

        internal void LoadConfigurations(IVM4Scanner vm)
        {
            Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            SourceConfigSection section = (SourceConfigSection)cfg.Sections["SourceFolders"];

            if (section != null && section.FolderItems.Count > 0)
                vm.SourcePath = section.FolderItems[0].Path;

            DestinationConfigSection destinationConfigSection = (DestinationConfigSection)cfg.Sections["DestinationFolders"];
            if (destinationConfigSection != null && destinationConfigSection.FolderItems.Count > 0)
            {
                vm.ExportType = (ExportEnum)destinationConfigSection.FolderItems[0].PathType;
                vm.DestinationPath = destinationConfigSection.FolderItems[0].Path;
                vm.DestinationDB = destinationConfigSection.FolderItems[0].Path4SQLite;
            }
        }
    }
}
