using System.IO.Compression;
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

        private int count = -1;
        private VirtualGroups virtualFlag = VirtualGroups.NotChecked;
        public DiskItemFileZip(string path) : base(path)
        {
        }

        public DiskItemFileZip(DiskItemFileZip item, string fullName) : base(item, fullName)
        {
        }

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
            }
        }

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
    }
}
