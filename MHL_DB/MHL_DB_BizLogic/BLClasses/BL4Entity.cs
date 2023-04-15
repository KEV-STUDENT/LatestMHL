using MHL_DB_Model;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using Microsoft.IdentityModel.Tokens;

namespace MHL_DB_BizLogic.BLClasses
{
    public abstract class BL4Entity<T1, T2, T3> : IBLEntity<List<T1>, List<T2>>
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
        List<T2>? IBLEntity<List<T1>, List<T2>>.GetNewEntities4DiskItem(List<IDiskItem> diskItems)
        {
            return GetNewEntities4DiskItem(diskItems);
        }

        async Task<List<T2>?> IBLEntity<List<T1>, List<T2>>.GetNewEntities4DiskItemAsync(List<IDiskItem> diskItems)
        {
            return await GetNewEntities4DiskItemAsync(diskItems);
        }

        bool IBLEntity<List<T1>, List<T2>>.AddInDB(List<T2>? newEntitys)
        {
           return AddInDB(newEntitys);
        }
        #endregion

        #region [Delegates]
        protected Func<List<T1>, List<T1>>? FilterDuplicatedAttrubute;
        #endregion

        #region [Properties]
        protected DBModel DB => _dB;
        protected object? Locker => _locker;
        #endregion

        #region [Methods]
        protected abstract List<T2> CheckInDB(IEnumerable<T3> filter);
        protected abstract void GetAttributesFromBook(List<T1> list, IMHLBook book);
        protected abstract IEnumerable<T3>? ConvertAttributes2ComparedList(List<T1> attributes);
        protected abstract T2 ConvertAttribute2DBEntity(T1 a);
        protected abstract T3 ConvertAttribute(T1 a);
        protected abstract T3 ConvertDBEntity(T2 a);
        protected abstract bool AddInDB(List<T2>? newEntitys);

        protected virtual List<T1>? ExceptFromAttributes(IEnumerable<T3> compared, List<T1> attributes, List<T2> allreadyInDB)
        {
            List<T1>? newAttributes = null;

            IEnumerable<T3> newList = compared.Except(allreadyInDB.Select(a => ConvertDBEntity(a)));
            if (newList.Any())
                newAttributes = attributes.Where(a => newList.Contains(ConvertAttribute(a))).ToList();



            return newAttributes;
        }

        protected virtual List<T2>? GetNewEntities4ListFromDiskItem(List<T1> attributes)
        {
            List<T2>? result = null;
            System.Diagnostics.Debug.WriteLine("-==-");
            if (!attributes.IsNullOrEmpty())
            {
                List<T1> uniqAttributes = FilterDuplicatedAttrubute?.Invoke(attributes) ?? attributes;
                System.Diagnostics.Debug.WriteLine("UNIQUE:{0}", uniqAttributes.Count);
                IEnumerable<T3>? comparedList = ConvertAttributes2ComparedList(uniqAttributes);
                System.Diagnostics.Debug.WriteLine("Compared:{0}", comparedList.Count());
                if (comparedList != null && comparedList.Any())
                {
                    List<T2>? allreadyInDB = GetDBEntities4Filter(comparedList);
                    System.Diagnostics.Debug.WriteLine("IN DB:{0}", allreadyInDB.Count);
                    List<T1>? attributes4Add = null;

                    if (allreadyInDB == null || allreadyInDB.Count == 0)
                    {
                        attributes4Add = uniqAttributes;
                    }
                    else
                    {
                        attributes4Add = ExceptFromAttributes(comparedList, uniqAttributes, allreadyInDB);
                    }

                    if (attributes4Add!=null && attributes4Add.Count > 0)
                        result = attributes4Add.Select(a=>ConvertAttribute2DBEntity(a)).ToList();
                }   
            }
            return result;
        }

        protected List<T1> List4DiskItems(List<IDiskItem> diskItems)
        {
            List<T1> result = new List<T1>();


            foreach (IDiskItem item in diskItems)
                if (item is IMHLBook book)
                    GetAttributesFromBook(result, book);

            return result;
        }

        private List<T2>? GetDBEntities4Filter(IEnumerable<T3>? filter)
        {
            List<T2>? result = null;

            if (filter != null && filter.Any())
                if (Locker == null)
                    result = CheckInDB(filter);
                else
                    lock (Locker)
                        result = CheckInDB(filter);

            return result;
        }
        public async Task<List<T2>?> GetNewEntities4DiskItemAsync(List<IDiskItem> diskItems)
        {
            return await Task.Run(() => { return GetNewEntities4DiskItem(diskItems); });
        }

        public List<T2>? GetNewEntities4DiskItem(List<IDiskItem> diskItems)
        {
            List<T1> list = List4DiskItems(diskItems);
            return GetNewEntities4ListFromDiskItem(list);
        }
        #endregion
    }
}
