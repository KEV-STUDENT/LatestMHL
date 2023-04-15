using MHL_DB_Model;
using MHLCommon.MHLBook;
using Microsoft.IdentityModel.Tokens;

namespace MHL_DB_BizLogic.BLClasses
{
    public class BLAuthors : BL4Entity<MHLAuthor, Author, string>
    {
        #region [Constructor]
        public BLAuthors(DBModel dB, object? locker) : base(dB, locker)
        {
            FilterDuplicatedAttrubute = (List<MHLAuthor> attributes) =>
            {
                return attributes
                     .Where(ab => !IsEmptyAttribute(ab))
                     .GroupBy(a => ConvertAttribute(a))
                     .Select(a => a.First()).ToList();
            };
        }
        public BLAuthors(DBModel dB) : this(dB, null) { }
        #endregion

        #region [Methods]
        protected override void GetAttributesFromBook(List<MHLAuthor> list, IMHLBook book)
        {
          if (list != null && !(book?.Authors?.IsNullOrEmpty() ?? true))
                list.AddRange(book.Authors);
        }

        protected override List<Author> CheckInDB(IEnumerable<string> filter)
        {
            return DB.Authors.Where(a => a.FirstName != null && a.LastName != null && a.MiddleName != null &&
                        filter.Contains(
                            a.FirstName.ToLower() + "|" +
                            a.LastName.ToLower() + "|" +
                            a.MiddleName.ToLower()
                            )).ToList();
        }

        protected override IEnumerable<string>? ConvertAttributes2ComparedList(List<MHLAuthor> attributes)
        {
            return attributes
                .Where(ab => !IsEmptyAttribute(ab))
                .GroupBy(a => ConvertAttribute(a))
                .Select(a => a.Key);
        }

        protected override Author ConvertAttribute2DBEntity(MHLAuthor author)
        {
            return new Author()
            {
                LastName = author.LastName?.Trim(),
                FirstName = author.FirstName?.Trim(),
                MiddleName = author.MiddleName?.Trim()
            };
        }

        protected override string ConvertAttribute(MHLAuthor a)
        {
            return string.Format("{0}|{1}|{2}",
                        a.FirstName?.Trim() ?? string.Empty,
                        a.LastName?.Trim() ?? string.Empty,
                        a.MiddleName?.Trim() ?? string.Empty).ToLower();
        }

        protected override string ConvertDBEntity(Author a)
        {
            return (a?.FirstName?.ToLower() ?? string.Empty) + "|" +
                (a?.LastName?.ToLower() ?? string.Empty) + "|" +
                (a?.MiddleName?.ToLower() ?? string.Empty);
        }

        private static bool IsEmptyAttribute(MHLAuthor a)
        {
            return a.FirstName.IsNullOrEmpty() && a.LastName.IsNullOrEmpty() && a.MiddleName.IsNullOrEmpty();
        }

        protected override bool AddInDB(List<Author>? newEntitys)
        {
            bool ret = !newEntitys.IsNullOrEmpty();

            if(ret)
                DB.Authors.AddRange(newEntitys);

            return ret;
        }
        #endregion
    }
}
