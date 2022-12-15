using System.Collections;
using System.IO.Compression;
using System.Runtime.Serialization;
using MHLCommon;
using MHLCommon.MHLDiskItems;

namespace MHLSourceOnDisk
{
    public class DiskItemFileZip : DiskItemFile, IDiskCollection
    {
        private enum VirtualGroups
        {
            NotChecked = 0,
            VirtualGroupsNotUsed = 1,
            VirtualGroupsUsed = 2
        }

        #region [Fields]
        private int count = -1;
        private VirtualGroups virtualFlag = VirtualGroups.NotChecked;
        private List<String> files = new List<string>();
        #endregion

        #region [Constructors]
        public DiskItemFileZip(string path) : base(path)
        {
        }

        public DiskItemFileZip(DiskItemFileZip item, string fullName) : base(item, fullName)
        {
        }
        #endregion

        #region [Methods]
        protected override void Initialize()
        {
            count = 0;
            virtualFlag = VirtualGroups.VirtualGroupsNotUsed;

            IDiskItem item = this;
            using (ZipArchive zipArchive = ZipFile.OpenRead(item.Path2Item))
            {
                if (zipArchive.Entries.Count > IDiskCollection.MaxItemsInVirtualGroup)
                {
                    count = (int)System.Math.Ceiling((decimal)zipArchive.Entries.Count / IDiskCollection.MaxItemsInVirtualGroup);
                    virtualFlag = VirtualGroups.VirtualGroupsUsed;
                }
                else
                {
                    count = zipArchive.Entries.Count;
                    virtualFlag = VirtualGroups.VirtualGroupsNotUsed;
                }
                files.Clear();
                foreach(ZipArchiveEntry entry in zipArchive.Entries)
                {
                    files.Add(entry.FullName);
                }
            }
        }

        private bool CheckExportDirectory4Files(string pathDestination)
        {
            bool ret = true;
            foreach (string file in files)
            {
                if (File.Exists(Path.Combine(pathDestination, file)))
                {
                    ret = false;
                    break;
                }
            }
            return ret;
        }
        #endregion

        #region [DiskItem Implementation]
        public override bool ExportItem(ExpOptions exportOptions)
        {
            bool result = true;
            try
            {
                using (ZipArchive zipArchive = ZipFile.OpenRead(Path2Item))
                {
                    if (exportOptions.OverWriteFiles || CheckExportDirectory4Files(exportOptions.PathDestination))
                        zipArchive.ExtractToDirectory(exportOptions.PathDestination, exportOptions.OverWriteFiles);
                    else
                    {
                        foreach (string file in files)
                        {
                            ExtractArchiveEntryToDirectorySeparately(exportOptions.PathDestination, file, zipArchive.GetEntry(file));
                        }
                    }
                }

            }
            catch (IOException ie)
            {
                System.Diagnostics.Debug.WriteLine(ie.Message);
                System.Diagnostics.Debug.WriteLine(ie.Data.Count);
                foreach (DictionaryEntry de in ie.Data)
                    System.Diagnostics.Debug.WriteLine("    Key: {0,-20}      Value: {1}",
                                      "'" + de.Key.ToString() + "'", de.Value);
                result = false;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                result = false;
            }
            return result;
        }

        private void ExtractArchiveEntryToDirectorySeparately(string pathDestination, string file, ZipArchiveEntry? zipArchiveEntry)
        {
            string newFile;
            if (zipArchiveEntry != null)
            {
                newFile = MHLSourceOnDiskStatic.GetNewFileName(pathDestination, file);
                zipArchiveEntry.ExtractToFile(newFile);
            }
        }
        #endregion

        #region [IDiskCollection implementation]
        int IDiskCollection.Count
        {
            get
            {
                return count;
            }
        }

        bool IDiskCollection.IsVirtualGroupsUsed
        {
            get
            {
                return (virtualFlag == VirtualGroups.VirtualGroupsUsed);
            }
        }

        IEnumerable<IDiskItem> IDiskCollection.GetChilds()
        {
            IDiskItem item = this;
            IDiskCollection diskCollection = this;

            using ZipArchive zipArchive = ZipFile.OpenRead(item.Path2Item);
            {
                if (diskCollection.IsVirtualGroupsUsed)
                {
                    int j = 0;
                    int i;
                    List<string> subList = new List<string>();

                    List<string> list = (
                        from file in zipArchive.Entries
                        select file.Name).ToList();

                    list.Sort(MHLCommonStatic.CompareStringByLength);

                    while (j < list.Count)
                    {
                        subList = new List<string>();
                        for (i = 0; i < IDiskCollection.MaxItemsInVirtualGroup; i++)
                        {
                            if (j < list.Count)
                            {
                                subList.Add(list[j]);
                            }
                            else break;
                            j++;
                        }

                        yield return DiskItemFabrick.GetDiskItem(this, subList);
                    }
                }
                else
                {
                    foreach (ZipArchiveEntry file in zipArchive.Entries)
                    {
                        yield return DiskItemFabrick.GetDiskItem(this, file);
                    }
                }
            }

        }

        IEnumerable<IDiskItem> IDiskCollection.GetChilds(List<string> subList)
        {
            IDiskItem item = this;
            using ZipArchive zipArchive = ZipFile.OpenRead(item.Path2Item);
            {
                ZipArchiveEntry? file = null;
                foreach (string entryName in subList)
                {
                    file = zipArchive.GetEntry(entryName);
                    if(file != null)
                        yield return DiskItemFabrick.GetDiskItem(this, file);
                }
            }
        }

        IEnumerable<string> IDiskCollection.GetChildsNames()
        {
            return files;
        }
        #endregion
    }
}
