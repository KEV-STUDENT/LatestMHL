﻿using MHL_DB_BizLogic.SQLite;
using MHL_DB_SQLite;
using MHLCommon;
using MHLCommon.DataModels;
using MHLCommon.ExpDestinations;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using System.Collections;
using System.Collections.Concurrent;
using System.IO.Compression;
using System.Runtime.Intrinsics.X86;

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
        private object locker = new object();
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
                foreach (ZipArchiveEntry entry in zipArchive.Entries)
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
        private bool ExportItem2SQLite(ExpDestination2SQLite exp2SQLite)
        {
            bool ret = false;
            BlockingCollection<IDiskItem> books = new BlockingCollection<IDiskItem>();
            using (ZipArchive zipArchive = ZipFile.OpenRead(Path2Item))
            {
                IDiskItem diskItem;
                Parallel.ForEach(zipArchive.Entries, entry =>
                {
                    lock (locker)
                    {
                        diskItem = DiskItemFabrick.GetDiskItem(this, entry);
                    }
                    if ((diskItem != null) && (diskItem is DiskItemExported) && (diskItem is IMHLBook book))
                        books.TryAdd(diskItem);
                });
            }

            using (DBModelSQLite dB = new DBModelSQLite(exp2SQLite.DestinationPath))
            {
                ret = (Bizlogic4DB.Export_FB2List(dB, books.ToList()) > -1);
            }
            return true;
        }

        private bool ExportItem2Dir(IExportDestination destination)
        {
            bool result = true;
            if ((destination is ExpDestination4Dir exp) && (Exporter != null))
            {
                try
                {
                    using (ZipArchive zipArchive = ZipFile.OpenRead(Path2Item))
                    {
                        Parallel.ForEach(zipArchive.Entries, entry =>
                        {
                            IDiskItem? diskItem = null;
                            lock (locker)
                            {
                                diskItem = DiskItemFabrick.GetDiskItem(this, entry);
                            }
                            if ((diskItem != null) && (diskItem is DiskItemExported itemExported))
                            {
                                Export2Dir exporter = new Export2Dir(Exporter.ExportOptions, diskItem);
                                itemExported.ExportBooks(exporter);
                            }
                        });
                    }

                }
                catch (IOException ie)
                {
                    result = false;
                }
                catch (Exception e)
                {
                    result = false;
                }
            }
            else { result = false; }
            return result;
        }
        #endregion

        #region [DiskItemExported Implementation]
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
                    if (file != null)
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
