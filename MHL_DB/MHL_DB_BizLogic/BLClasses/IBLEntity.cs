using MHL_DB_Model;
using MHLCommon;
using MHLCommon.MHLDiskItems;

namespace MHL_DB_BizLogic.BLClasses
{
    public interface IBLEntity<T1,T2>
    {
        //protected T1 List4DiskItems(List<IDiskItem> diskItems);     
        //public T2? GetNewEntities4ListFromDiskItem(T1 attributes);
        //public T2? GetDBEntities4ListFromDiskItem(T1 attributes);
        public T2? GetNewEntities4DiskItem(List<IDiskItem> diskItems);
        public Task<T2?> GetNewEntities4DiskItemAsync(List<IDiskItem> diskItems);
        public bool AddInDB(T2? newEntitys);
    }
}
