using MHL_DB_Model;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using Microsoft.IdentityModel.Tokens;

namespace MHL_DB_BizLogic.BLClasses
{
    internal class BLAuthors : BL4Entity<List<MHLAuthor>, List<Author>>
    {
        #region [Constructor]
        public BLAuthors(DBModel dB, object? locker) : base(dB, locker)
        {
            GetAttributesFromBook += (List<MHLAuthor> list, IMHLBook book) =>
            {
                if (list != null && (book?.Authors?.IsNullOrEmpty() ?? false))
                    list.AddRange(book.Authors);
            };
        }
        public BLAuthors(DBModel dB) : this(dB, null) { }
        #endregion

            #region [Methods]
        protected override List<Author>? GetDBEntities4ListFromDiskItem(List<MHLAuthor> attributes)
        {
            List<Author>? authorsDB = null;
            if (attributes != null && attributes.Any())
            {

                IEnumerable<string> authors4book =
                    from ab in attributes
                    select string.Format("{0}|{1}|{2}",
                        ab.FirstName?.Trim() ?? string.Empty,
                        ab.LastName?.Trim() ?? string.Empty,
                        ab.MiddleName?.Trim() ?? string.Empty).ToLower();

                authorsDB = GetDBEntities4Filter(authors4book);
            }

            return authorsDB;
        }

        protected override List<Author>? GetNewEntities4ListFromDiskItem(List<MHLAuthor> attributes)
        {
            List<Author>? authorsRes = null;
            IEnumerable<MHLAuthor>? newAuthors = null;

            if (attributes != null && attributes.Any())
            {
                attributes = attributes
                    .Where(ab => !(string.IsNullOrEmpty(ab.FirstName) && string.IsNullOrEmpty(ab.LastName) && string.IsNullOrEmpty(ab.MiddleName)))
                    .GroupBy(a => string.Format("{0}|{1}|{2}",
                            a.FirstName?.Trim() ?? string.Empty,
                            a.LastName?.Trim() ?? string.Empty,
                            a.MiddleName?.Trim() ?? string.Empty).ToLower())
                    .Select(a => a.First()).ToList();

                IEnumerable<string> authors4book =
                    from ab in attributes
                    select string.Format("{0}|{1}|{2}",
                        ab.FirstName?.Trim() ?? string.Empty,
                        ab.LastName?.Trim() ?? string.Empty,
                        ab.MiddleName?.Trim() ?? string.Empty).ToLower();

                List<Author>? authorsDB = GetDBEntities4Filter(authors4book);

                if ((authorsDB?.Count ?? 0) == 0)
                {
                    newAuthors = attributes
                        .Where(ab => !(string.IsNullOrEmpty(ab.FirstName) && string.IsNullOrEmpty(ab.LastName) && string.IsNullOrEmpty(ab.MiddleName)))
                        .GroupBy(a => string.Format("{0}|{1}|{2}",
                            a.FirstName?.Trim() ?? string.Empty,
                            a.LastName?.Trim() ?? string.Empty,
                            a.MiddleName?.Trim() ?? string.Empty).ToLower())
                        .Select(a => a.First());
                }
                else
                {
                    var newList = authors4book.Except(
                        from ab in authorsDB
                        select string.Format("{0}|{1}|{2}",
                            ab.FirstName?.Trim().ToLower() ?? string.Empty,
                            ab.LastName?.Trim().ToLower() ?? string.Empty,
                            ab.MiddleName?.Trim().ToLower() ?? string.Empty));

                    if (newList.Any())
                        newAuthors = (
                            from ab in attributes
                            where newList.Contains(
                                string.Format("{0}|{1}|{2}",
                                    ab.FirstName?.Trim() ?? string.Empty,
                                    ab.LastName?.Trim() ?? string.Empty,
                                    ab.MiddleName?.Trim() ?? string.Empty))
                            select ab)
                            .Distinct();
                }
            }

            if (newAuthors?.Any() ?? false)
                authorsRes = newAuthors
                    .Select(
                        author => new Author()
                        {
                            LastName = author.LastName?.Trim(),
                            FirstName = author.FirstName?.Trim(),
                            MiddleName = author.MiddleName?.Trim()
                        })
                    .ToList();

            return authorsRes;
        }

       protected List<MHLAuthor> List4DiskItems1(List<IDiskItem> diskItems)
        {
            List<MHLAuthor> result = new List<MHLAuthor>();

            foreach (IDiskItem item in diskItems)
                if (item is IMHLBook book)
                    result.AddRange(book.Authors);

            return result;
        }

        protected override List<Author>? FilterData<T3>(T3 filter)
        {
            List<Author>? result = null;

            if (filter != null && filter is IEnumerable<string> authors && (authors?.Any() ?? false))
                result = DB.Authors
                       .Where(a => authors.Contains(
                           a.FirstName.ToLower() + "|" + a.LastName.ToLower() + "|" + a.MiddleName.ToLower()))
                       .Select(a => a)
                       .ToList();

            return result;
        }
        #endregion
    }
}
