using MHLCommon;
using MHLCommon.MHLDiskItems;
using System.Collections.Concurrent;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Xml.XPath;

namespace MHLSourceOnDisk
{
    public class DiskItemVirtualGroup : DiskItem, IVirtualGroup
    {
        #region [Private Fields]
        private IDiskCollection item;
        private List<string> subList;
        #endregion

        #region [Constructors]
        public DiskItemVirtualGroup(IDiskCollection item, List<string> subList) :
            base(GetPath4Collection(item), GetName4List(subList))
        {
            this.item = item;
            this.subList = subList;
        }
        #endregion

        #region [Methods]
        static private string GetPath4Collection(IDiskCollection item)
        {
            string path2Item;
            if (item is IDiskItem diskItem)
            {
                path2Item = diskItem.Path2Item;
            }
            else
                path2Item = string.Empty;
            return path2Item;
        }
        static private string GetName4List(List<string> subList)
        {
            string name;

            if ((subList?.Count ?? 0) > 0)
                name = string.Format("{0}...{1}", subList[0], subList[subList.Count - 1]);
            else
                name = "Is Empty";
            return name;
        }
        #endregion

        #region [IDiskItemVirtualGroup implementation]
        IDiskCollection IVirtualGroup.ParentCollection => item;

        List<string> IVirtualGroup.ItemsNames => subList;
        #endregion

        #region [DiskItem implementation]
        public override bool ExportItem(ExpOptions exportOptions)
        {
            bool result;
            using (ZipArchive zipArchive = ZipFile.OpenRead(((IDiskItem)this).Path2Item))
            {
                ConcurrentBag<string> errorEnries = new ConcurrentBag<string>();

                Parallel.ForEach(subList, entryName =>
                {
                    bool exported = ExportEntry(exportOptions, entryName);
                    if (!exported)
                    {
                        errorEnries.Add(entryName);
                    }
                });

                result = errorEnries.Count == 0;
                if(!result)
                {
                    foreach(string entry in errorEnries)
                    {
                        System.Diagnostics.Debug.WriteLine(string.Concat("Not Exported: ", entry));
                    }
                }
            }
            return result;
        }

        private bool ExportEntry(ExpOptions exportOptions, string entry)
        {
            bool result = true;
            using (ZipArchive zipArchive = ZipFile.OpenRead(((IDiskItem)this).Path2Item))
            {
                result = ExportFile(exportOptions, zipArchive.GetEntry(entry));
            }
            return result;
        }

        private bool ExportFile(ExpOptions exportOptions, ZipArchiveEntry? file)
        {
            bool result = (file != null);
            string newFile;

            if(result)
            {
                try
                {
                    if (exportOptions.OverWriteFiles)
                        newFile = Path.Combine(exportOptions.PathDestination, file.Name);
                    else
                        newFile = MHLSourceOnDiskStatic.GetNewFileName(exportOptions.PathDestination, file.Name);

                    file.ExtractToFile(newFile, exportOptions.OverWriteFiles);
                    if (!File.Exists(newFile))
                    {
                        result = false;
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    result = false;
                }
            }

            return result;
        }
        #endregion

        #region [IDiskCollection implementation]
        int IDiskCollection.Count => subList.Count;

        bool IDiskCollection.IsVirtualGroupsUsed => false;

        IEnumerable<IDiskItem> IDiskCollection.GetChilds()
        {
            return item.GetChilds(subList);
        }

        IEnumerable<IDiskItem> IDiskCollection.GetChilds(List<string> subList)
        {
            return item.GetChilds(subList);
        }
        #endregion
    }
}