using MHL_DB_Model;
using MHLCommon.MHLBook;
using Microsoft.IdentityModel.Tokens;

namespace MHL_DB_BizLogic.BLClasses
{
    public class BLKeywords : BL4Entity<MHLKeyword, Keyword4Book, string>
    {
        #region [Constructors]
        public BLKeywords(DBModel dB) : this(dB, null) { }
        public BLKeywords(DBModel dB, object? locker) : base(dB, locker)
        {
            FilterDuplicatedAttrubute = (List<MHLKeyword> attributes) =>
            {
                return attributes
                     .Where(a => !a.Keyword.IsNullOrEmpty())
                     .GroupBy(a => a.Keyword.Trim().ToLower())
                     .Select(a => a.First()).ToList();
            };
        }
        #endregion

        #region [Methods]
        protected override bool AddInDB(List<Keyword4Book>? newEntitys)
        {
            bool ret = !newEntitys.IsNullOrEmpty();

            if (ret)
                DB.Keyword4Books.AddRange(newEntitys);

            return ret;
        }
       
        protected override List<Keyword4Book> CheckInDB(IEnumerable<string> filter)
        {
            return DB.Keyword4Books
                   .Where(a => a.Keyword != null && filter.Contains(a.Keyword.Trim().ToLower()))
                   .Select(a => a)
                   .ToList();
        }

        protected override string ConvertAttribute(MHLKeyword a)
        {
            return a.Keyword.Trim().ToLower();
        }

        protected override Keyword4Book ConvertAttribute2DBEntity(MHLKeyword a)
        {
            return new Keyword4Book()
            {
                Keyword = a.Keyword.Trim()
            };
        }

        protected override IEnumerable<string>? ConvertAttributes2ComparedList(List<MHLKeyword> attributes)
        {
            return attributes
               .Where(ab => !ab.Keyword.IsNullOrEmpty())
               .GroupBy(a => ConvertAttribute(a))
               .Select(a => a.Key);
        }

        protected override string ConvertDBEntity(Keyword4Book a)
        {
            return a.Keyword.Trim().ToLower();
        }

        protected override void GetAttributesFromBook(List<MHLKeyword> list, IMHLBook book)
        {
            if (list != null && !(book?.Keywords?.IsNullOrEmpty() ?? false))
                list.AddRange(book.Keywords);
        }
        #endregion
    }
}