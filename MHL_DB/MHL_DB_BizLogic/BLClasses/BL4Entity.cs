using MHL_DB_Model;
using MHLCommon;
using MHLCommon.MHLDiskItems;

namespace MHL_DB_BizLogic.BLClasses
{
    internal abstract class BL4Entity<T1, T2> : IBLEntity<T1, T2>
    {
        private DBModel _dB;
        private object? _locker;

        #region [Constructor]
        public BL4Entity(DBModel dB, object? locker)
        {
            _dB = dB;
            _locker = locker;
        }

        public BL4Entity(DBModel dB) : this(dB, null) { }
        #endregion

        #region [IBLEntity]
        T2? IBLEntity<T1, T2>.GetDBEntities4ListFromDiskItem(T1 attributes)
        {
            return GetDBEntities4ListFromDiskItem(attributes);
        }

        T2? IBLEntity<T1, T2>.GetNewEntities4ListFromDiskItem(T1 attributes)
        {
            return GetNewEntities4ListFromDiskItem(attributes);
        }

        T1 IBLEntity<T1, T2>.List4DiskItems(List<IDiskItem> diskItems)
        {
            return List4DiskItems(diskItems);
        }

        T2? IBLEntity<T1, T2>.GetNewEntities4DiskItem(List<IDiskItem> diskItems)
        {
            return GetNewEntities4DiskItem(diskItems);
        }

        async Task<T2?> IBLEntity<T1, T2>.GetNewEntities4DiskItemAsync(List<IDiskItem> diskItems)
        {
            return await GetNewEntities4DiskItemAsync(diskItems);
        }
        #endregion

        #region [Properties]
        protected DBModel DB => _dB;
        protected object? Locker => _locker;
        #endregion

        #region [Methods]
        protected abstract T2? GetDBEntities4ListFromDiskItem(T1 attributes);
        protected abstract T1 List4DiskItems(List<IDiskItem> diskItems);
        protected abstract T2? GetNewEntities4ListFromDiskItem(T1 attributes);
        protected abstract T2? FilterData<T3>(T3 filter);
        protected abstract bool CheckFilter<T3>(T3? filter);
        protected T2? GetDBEntities4Filter<T3>(T3 filter)
        {
            T2? result = default;
            if (CheckFilter(filter))
                if (Locker == null)
                    result = FilterData(filter);
                else
                    lock (Locker)
                        result = FilterData(filter);
            return result;
        }

        public async Task<T2?> GetNewEntities4DiskItemAsync(List<IDiskItem> diskItems)
        {
            return await Task.Run(() => { return GetNewEntities4DiskItem(diskItems); });
        }

        public T2? GetNewEntities4DiskItem(List<IDiskItem> diskItems)
        {
            var list = List4DiskItems(diskItems);
            return GetNewEntities4ListFromDiskItem(list);
        }
        #endregion
    }
}
