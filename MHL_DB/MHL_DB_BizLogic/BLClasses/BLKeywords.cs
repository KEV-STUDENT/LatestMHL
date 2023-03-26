using MHL_DB_Model;
using MHLCommon.MHLBook;
using Microsoft.IdentityModel.Tokens;

namespace MHL_DB_BizLogic.BLClasses
{
    internal class BLKeywords : BL4Entity<List<MHLKeyword>, List<Keyword4Book>>
    {
        #region [Constructors]
        public BLKeywords(DBModel dB) : this(dB, null) { }
        public BLKeywords(DBModel dB, object? locker) : base(dB, locker) 
        {
            GetAttributesFromBook += (List<MHLKeyword> list, IMHLBook book) =>
            {
                if (list != null && (book?.Keywords?.IsNullOrEmpty() ?? false))
                    list.AddRange(book.Keywords);
            };
         }
        #endregion

        #region [Methods]
        protected override List<Keyword4Book>? FilterData<T3>(T3 filter)
        {
            List<Keyword4Book>? result = null;

            if (filter != null && filter is IEnumerable<string> keywords && (keywords?.Any() ?? false))
                result = DB.Keyword4Books
                    .Where(k => keywords.Contains(k.Keyword))
                    .Select(k => k).ToList();

            return result;
        }

        protected override List<Keyword4Book>? GetDBEntities4ListFromDiskItem(List<MHLKeyword> attributes)
        {
            List<Keyword4Book>? result = null;
            if (attributes != null && attributes.Any())
            {
                IEnumerable<string> filter = attributes.GroupBy(k => k.Keyword.Trim().ToLower()).Select(k => k.Key);
                result = GetDBEntities4Filter(filter);
            }

            return result;
        }

        protected override List<Keyword4Book>? GetNewEntities4ListFromDiskItem(List<MHLKeyword> attributes)
        {
            List<Keyword4Book>? result = null;
            IEnumerable<MHLKeyword>? newValues = null;

            if (attributes.Any())
            {
                IEnumerable<string> list = attributes.GroupBy(k => k.Keyword.Trim().ToLower()).Select(k => k.Key);
                List<Keyword4Book>? keywordsDB = GetDBEntities4Filter(list);

                if (keywordsDB?.Any() ?? false)
                    newValues = attributes.Except(
                        attributes.GroupJoin(keywordsDB,
                            n => n.Keyword.Trim().ToLower(),
                            keywordDB => keywordDB.Keyword.Trim().ToLower(),
                        (n, keywordDB) => n));
                else
                    newValues = attributes;
            }

            if (newValues?.Any() ?? false)
                result = newValues
                    .Select(v => new Keyword4Book()
                    {
                        Keyword = v.Keyword.Trim()
                    }).ToList();

            return result;
        }
        #endregion
    }
}