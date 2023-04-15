using MHL_DB_Model;
using MHLCommon.MHLBook;
using Microsoft.IdentityModel.Tokens;

namespace MHL_DB_BizLogic.BLClasses
{
    public class BLSequence : BL4Entity<string, Sequence4Book, string>
    {
       #region [Constructors]
        public BLSequence(DBModel dB) : this(dB, null) { }
        public BLSequence(DBModel dB, object? locker) : base(dB, locker)
        {
            FilterDuplicatedAttrubute = (List<string> attributes) =>
            {
                return attributes
                     .Where(a => !a.IsNullOrEmpty())
                     .GroupBy(a => a.Trim().ToLower())
                     .Select(a => a.First()).ToList();
            };
        }
        #endregion

        #region [Methods]
        protected override bool AddInDB(List<Sequence4Book>? newEntitys)
        {
            bool ret = !newEntitys.IsNullOrEmpty();

            if (ret)
                DB.Sequence4Books.AddRange(newEntitys);

            return ret;
        }

        protected override List<Sequence4Book> CheckInDB(IEnumerable<string> filter)
        {
            return  DB.Sequence4Books
                   .Where(a => a.Name != null && filter.Contains(a.Name.Trim().ToLower()))
                   .Select(a => a)
                   .ToList();
        }

        protected override string ConvertAttribute(string a)
        {
            return a.Trim().ToLower();
        }

        protected override Sequence4Book ConvertAttribute2DBEntity(string a)
        {
            return new Sequence4Book()
            {
                Name = a.Trim()
            };
        }

        protected override IEnumerable<string>? ConvertAttributes2ComparedList(List<string> attributes)
        {
            return attributes
              .Where(ab => !ab.IsNullOrEmpty())
              .GroupBy(a => ConvertAttribute(a))
              .Select(a => a.Key);
        }

        protected override string ConvertDBEntity(Sequence4Book a)
        {
            return a.Name.Trim().ToLower();
        }

        protected override void GetAttributesFromBook(List<string> list, IMHLBook book)
        {
            if (list != null && !(book?.SequenceAndNumber?.Name.IsNullOrEmpty() ?? false))
                list.Add(book?.SequenceAndNumber?.Name ?? string.Empty);
        }
        #endregion
    }
}