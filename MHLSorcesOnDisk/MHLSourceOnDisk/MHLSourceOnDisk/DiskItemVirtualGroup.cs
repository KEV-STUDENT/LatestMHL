using MHLCommon;
using MHLCommon.MHLDiskItems;
using System.IO.Compression;
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
        public DiskItemVirtualGroup(IDiskCollection item, List<string> subList):
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
            bool result = true;
            using(ZipArchive zipArchive = ZipFile.OpenRead(((IDiskItem)this).Path2Item))
            {
                ZipArchiveEntry? file = null;
                string newFile;

                foreach (string entryName in subList)
                {
                    file = zipArchive.GetEntry(entryName);
                    if (file != null)
                    {
                        try
                        {
                            newFile = Path.Combine(exportOptions.PathDestination, entryName);
                            //System.Diagnostics.Debug.WriteLine(newFile);
                            file.ExtractToFile(newFile, true);
                            if (!File.Exists(newFile))
                            {
                                System.Diagnostics.Debug.WriteLine("ERROR : " + newFile);
                                result = false;
                            }
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e.Message);
                            result = false;
                        }
                    }
                }
            }
            System.Diagnostics.Debug.WriteLine(result);
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