using MHLCommon.MHLDiskItems;

namespace MHLSourceOnDisk
{
    public class DiskItemVirtualGroup : IDiskItemVirtualGroup
    {
        #region [Private Fields]
        private readonly string  path2Item;
        private readonly string name;
        private IDiskCollection item;
        private List<string> subList;
        #endregion

        #region [Constructors]
        public DiskItemVirtualGroup(IDiskCollection item, List<string> subList)
        {
            this.item = item;
            this.subList = subList;
            
            if (item is IDiskItem diskItem)
            {
                path2Item = diskItem.Path2Item;
            }else
                path2Item = string.Empty;

            if((subList?.Count??0) > 0)
                name = string.Format("{0}...{1}", subList[0], subList[subList.Count - 1]);
            else
                name = "Is Empty";
        }
        #endregion

        #region [IDiskItemVirtualGroup implementation]
        IDiskCollection IDiskItemVirtualGroup.ParentCollection => item;

        List<string> IDiskItemVirtualGroup.ItemsNames => subList;
        #endregion

        #region [IDiskItem implementation]
        string IDiskItem.Path2Item => path2Item;


        string IDiskItem.Name => name;

        bool IDiskItem.ExportBooks<T>(T exporter)
        {
            throw new NotImplementedException();
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