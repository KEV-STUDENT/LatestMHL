using MHL_DB_Model;
using MHLCommon;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;

namespace MHL_DB_BizLogic.BLClasses
{
    internal abstract class BL4Entity<T1, T2> : IBLEntity<T1, T2> where T1 : new()
    {
        private readonly DBModel _dB;
        private readonly object? _locker;

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

        protected event Action<T1, IMHLBook> GetAttributesFromBook;

        #region [Properties]
        protected DBModel DB => _dB;
        protected object? Locker => _locker;
        #endregion

        #region [Methods]
        protected abstract T2? GetDBEntities4ListFromDiskItem(T1 attributes);
        protected abstract T2? GetNewEntities4ListFromDiskItem(T1 attributes);
        protected abstract T2? FilterData<T3>(T3 filter);
         protected T1 List4DiskItems(List<IDiskItem> diskItems)
        {
            T1 result = new T1();
            if (GetAttributesFromBook != null)
                foreach (IDiskItem item in diskItems)
                    if (item is IMHLBook book)
                        GetAttributesFromBook(result, book);
            return result;
        }
        protected static bool CheckFilter<T>(T? filter)
        {
            bool result = false;
            
            if(filter != null)
                if(filter is IEnumerable<string> valuesStr)
                    result = valuesStr?.Any() ?? false;
                else if(filter is IEnumerable<FB2Genres> valuesGenre)
                    result = valuesGenre?.Any() ?? false;

            return result;
        }
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
