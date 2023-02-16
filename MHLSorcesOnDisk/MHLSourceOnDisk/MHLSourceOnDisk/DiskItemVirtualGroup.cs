using MHL_DB_Model;
using MHL_DB_SQLite;
using MHLCommon.ExpDestinations;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using System.Collections.Concurrent;
using System.IO.Compression;
using System.Xml.XPath;

namespace MHLSourceOnDisk
{
    public class DiskItemVirtualGroup : DiskItemExported, IVirtualGroup
    {
        #region [Private Fields]
        private IDiskCollection item;
        private List<string> subList;
        private object locker = new object();
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
        private bool ExportItem2Dir(IExportDestination destination)
        {
            bool result = false;
            if (item is DiskItemFileZip zip)
            {
                using ZipArchive zipArchive = ZipFile.OpenRead(((IDiskItemExported)this).Path2Item);
                ConcurrentBag<string> errorEnries = new ConcurrentBag<string>();

                Parallel.ForEach(subList, entryName =>
                {
                    bool exported = false;
                    ZipArchiveEntry? entry = null;
                    lock (locker)
                    {
                        entry = (from zipEntry in zipArchive.Entries
                                 where zipEntry.Name == entryName
                                 select zipEntry).FirstOrDefault();
                    }

                    IDiskItem? diskItem = null;
                    if (entry != null)
                    {
                        lock (locker)
                        {
                            diskItem = DiskItemFabrick.GetDiskItem(zip, entry);
                        }
                    }

                    if (diskItem is IDiskItemExported itemExported)
                    {
                        Export2Dir exporter = new Export2Dir(Exporter.ExportOptions, itemExported);
                        exported = itemExported.ExportBooks(exporter);
                    }

                    if (!exported)
                        errorEnries.Add(entryName);
                });


                result = errorEnries.Count == 0;
            }
            return result;
        }

        private bool ExportItem2SQLite(ExpDestination2SQLite exp2SQLite)
        {

            bool ret = false;
            if (item is DiskItemFileZip zip)
            {
                BlockingCollection<IDiskItem> books = new BlockingCollection<IDiskItem>();
                using ZipArchive zipArchive = ZipFile.OpenRead(((IDiskItemExported)this).Path2Item);

                Parallel.ForEach(subList, entryName =>
                {                   
                    ZipArchiveEntry? entry = null;
                    lock (locker)
                        entry = (from zipEntry in zipArchive.Entries
                                 where zipEntry.Name == entryName
                                 select zipEntry).FirstOrDefault();

                    IDiskItem? diskItem = null;

                    if (entry != null)
                        lock (locker)
                            diskItem = DiskItemFabrick.GetDiskItem(zip, entry);


                    if ((diskItem != null) && (diskItem is DiskItemExported) && (diskItem is IMHLBook book))
                        books.TryAdd(diskItem);
                });

                using (DBModelSQLite dB = new DBModelSQLite(exp2SQLite.DestinationPath))
                {
                    ret = (Bizlogic4DB.Export_FB2List(dB, books.ToList()) > -1);
                }
            }
            return true;
        }
        #endregion

        #region [IDiskItemVirtualGroup implementation]
        IDiskCollection IVirtualGroup.ParentCollection => item;

        List<string> IVirtualGroup.ItemsNames => subList;
        #endregion

        #region [DiskItem implementation]
        public override bool ExportItem(IExportDestination destination)
        {
            if (destination is ExpDestination4Dir exp2Dir)
            {
                return ExportItem2Dir(exp2Dir);
            }
            else if (destination is ExpDestination2SQLite exp2SQLite)
            {
                return ExportItem2SQLite(exp2SQLite);
            }
            return false;
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

        IEnumerable<string> IDiskCollection.GetChildsNames()
        {
            return subList;
        }
        #endregion
    }
}