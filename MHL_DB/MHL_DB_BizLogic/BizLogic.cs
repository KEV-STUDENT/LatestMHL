using MHL_DB_BizLogic.BLClasses;
using MHL_DB_Model;
using MHL_DB_MSSqlServer;
using MHLCommon;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;

namespace MHL_DB_BizLogic
{
    static internal class BizLogic
    {
        private static object _locker = new object();

        internal static IEnumerable<(string Key, IDiskItem DiskItem, IMHLBook MHLBook)>? CheckBooksInDB(DBModel dB, List<IDiskItem> books)
        {
            IEnumerable<(string Key, IDiskItem DiskItem, IMHLBook MHLBook)> booksList = null;
            if ((books?.Count ?? 0) == 0)
                return booksList;

            booksList = books.GroupBy(b => string.Format("{0}|{1}", b.Path2Item.Trim().ToUpper(), b.Name.Trim().ToUpper()))            //IEnumerable < (string, IDiskItem DiskItem, IMHLBook MHLBook ) >  booksList = diskItems.GroupBy(b => string.Format("{0}|{1}", b.Path2Item.Trim().ToUpper(), b.Name.Trim().ToUpper()))
                    .Select(b => (b.Key, DiskItem: b.First(), MHLBook: (IMHLBook)b.First()));

            var booksKeys = booksList.Select(b => b.Key);

            IEnumerable<string> bookDB;
            lock (_locker)
                bookDB = dB.Books
                .Where(b => booksKeys.Contains(b.Path2File.ToUpper() + "|" + b.EntityInZIP.ToUpper()))
                .Select(b => b.Path2File.ToUpper() + "|" + b.EntityInZIP.ToUpper()).ToList();

            if (bookDB.Any())
            {
                booksKeys = booksKeys.Except(bookDB);
                booksList = from b in booksList join db in booksKeys on b.Key equals db select b;
            }

            return booksList;
        }


        internal async static Task<List<T>?> GetNewEntityRange4BooksListAsync<T>(DBModel dB, List<IDiskItem> diskItems)
        {
            return await Task.Run(() => { return GetNewEntityRange4BooksList<T>(dB, diskItems); });
        }

        internal static List<T>? GetNewEntityRange4BooksList<T>(DBModel dB, List<IDiskItem> diskItems)
        {
            Type listType = typeof(T);

            if (listType == typeof(Author)) { } //return GetNewAuthorsRange4AuthorsList(dB, AuthorList4DiskItems(diskItems)) as List<T>;
            else if (listType == typeof(Genre))
                return GetNewGenresRange4GenresList(dB, GenreList4DiskItems(diskItems)) as List<T>;
            else if (listType == typeof(Keyword4Book))
                return GetNewKeywordsRange4GenresList(dB, KeywordList4DiskItems(diskItems)) as List<T>;

            return null;
        }
        #region [Author]
        //private static List<MHLAuthor> AuthorList4DiskItems(List<IDiskItem> diskItems)
        //{
        //    List<MHLAuthor> result = new List<MHLAuthor>();

        //    foreach (var item in diskItems)
        //        if (item is IMHLBook book)
        //            result.AddRange(book.Authors);

        //    return result;
        //}

        //internal static List<Author>? GetNewAuthorsRange4AuthorsList(DBModel dB, List<MHLAuthor> authors)
        //{
        //    List<Author>? authorsRes = null;
        //    IEnumerable<MHLAuthor>? newAuthors = null;

        //    if (authors != null && authors.Any())
        //    {
        //        authors = authors
        //            .Where(ab => !(string.IsNullOrEmpty(ab.FirstName) && string.IsNullOrEmpty(ab.LastName) && string.IsNullOrEmpty(ab.MiddleName)))
        //            .GroupBy(a => string.Format("{0}|{1}|{2}",
        //                    a.FirstName?.Trim() ?? string.Empty,
        //                    a.LastName?.Trim() ?? string.Empty,
        //                    a.MiddleName?.Trim() ?? string.Empty).ToLower())
        //            .Select(a => a.First()).ToList();

        //        IEnumerable<string> authors4book =
        //            from ab in authors
        //            select string.Format("{0}|{1}|{2}",
        //                ab.FirstName?.Trim() ?? string.Empty,
        //                ab.LastName?.Trim() ?? string.Empty,
        //                ab.MiddleName?.Trim() ?? string.Empty).ToLower();

        //        List<Author>? authorsDB = GetAuthors4AuthorsList(dB, authors4book);

        //        if ((authorsDB?.Count ?? 0) == 0)
        //        {
        //            newAuthors = authors
        //                .Where(ab => !(string.IsNullOrEmpty(ab.FirstName) && string.IsNullOrEmpty(ab.LastName) && string.IsNullOrEmpty(ab.MiddleName)))
        //                .GroupBy(a => string.Format("{0}|{1}|{2}",
        //                    a.FirstName?.Trim() ?? string.Empty,
        //                    a.LastName?.Trim() ?? string.Empty,
        //                    a.MiddleName?.Trim() ?? string.Empty).ToLower())
        //                .Select(a => a.First());
        //        }
        //        else
        //        {
        //            var newList = authors4book.Except(
        //                from ab in authorsDB
        //                select string.Format("{0}|{1}|{2}",
        //                    ab.FirstName?.Trim().ToLower() ?? string.Empty,
        //                    ab.LastName?.Trim().ToLower() ?? string.Empty,
        //                    ab.MiddleName?.Trim().ToLower() ?? string.Empty));

        //            if (newList.Any())
        //                newAuthors = (
        //                    from ab in authors
        //                    where newList.Contains(
        //                        string.Format("{0}|{1}|{2}",
        //                            ab.FirstName?.Trim() ?? string.Empty,
        //                            ab.LastName?.Trim() ?? string.Empty,
        //                            ab.MiddleName?.Trim() ?? string.Empty))
        //                    select ab)
        //                    .Distinct();
        //        }
        //    }

        //    if (newAuthors?.Any() ?? false)
        //        authorsRes = newAuthors
        //            .Select(
        //                author => new Author()
        //                {
        //                    LastName = author.LastName?.Trim(),
        //                    FirstName = author.FirstName?.Trim(),
        //                    MiddleName = author.MiddleName?.Trim()
        //                })
        //            .ToList();

        //    return authorsRes;
        //}

        //internal static List<Author>? GetAuthors4AuthorsList(DBModel dB, List<MHLAuthor> authors)
        //{
        //    List<Author>? authorsDB = null;
        //    if (authors != null && authors.Any())
        //    {

        //        IEnumerable<string> authors4book =
        //            from ab in authors
        //            select string.Format("{0}|{1}|{2}",
        //                ab.FirstName?.Trim() ?? string.Empty,
        //                ab.LastName?.Trim() ?? string.Empty,
        //                ab.MiddleName?.Trim() ?? string.Empty).ToLower();

        //        authorsDB = GetAuthors4AuthorsList(dB, authors4book);
        //    }

        //    return authorsDB;
        //}
        //private static List<Author>? GetAuthors4AuthorsList(DBModel dB, IEnumerable<string> authors4book)
        //{
        //    List<Author>? authorsDB = null;
        //    if (authors4book != null && authors4book.Any())
        //        lock (_locker)
        //            authorsDB = dB.Authors
        //           .Where(a => authors4book.Contains(
        //               a.FirstName.ToLower() + "|" + a.LastName.ToLower() + "|" + a.MiddleName.ToLower()))
        //           .Select(a => a)
        //           .ToList();

        //    return authorsDB;
        //}
        #endregion

        #region [Genre]       
        private static List<MHLGenre> GenreList4DiskItems(List<IDiskItem> diskItems)
        {
            List<MHLGenre> genres = new List<MHLGenre>();

            foreach (var item in diskItems)
                if (item is IMHLBook book)
                    genres.AddRange(book.Genres);

            return genres;
        }
        internal static List<Genre>? GetNewGenresRange4GenresList(DBModel dB, List<MHLGenre> genres)
        {
            List<Genre>? result = null;
            IEnumerable<FB2Genres>? newGenres = null;

            if (genres.Any())
            {
                IEnumerable<FB2Genres> genre4book = genres.GroupBy(g => g.Genre).Select(g => g.First().Genre);
                List<Genre>? genresDB = GetGenres4GenresList(dB, genre4book);

                if ((genresDB?.Count ?? 0) == 0)
                    newGenres = genre4book;
                else
                {
                    IEnumerable<FB2Genres> genreFromDB = from gb in genresDB select gb.GenreVal;
                    if (genreFromDB.Any())
                        newGenres = genre4book.Except(genreFromDB);
                }
            }

            if(newGenres?.Any()??false)

                result = newGenres
                    .Select(g => new Genre()
                    {
                        GenreVal = g,
                        GenreCode = g.ToString()
                    }).ToList();
              
            return result;
        }

        internal static List<Genre>? GetGenres4GenresList(DBModel dB, List<MHLGenre> genres)
        {
            List<Genre>? result = null;

            if (genres.Any())
            {
                IEnumerable<FB2Genres> genre4book = genres.GroupBy(g => g.Genre).Select(g => g.First().Genre);
                result = GetGenres4GenresList(dB, genre4book);
            }

            return result;
        }

        private static List<Genre>? GetGenres4GenresList(DBModel dB, IEnumerable<FB2Genres> genre4book)
        {
            List<Genre>? result = null;

            if (genre4book.Any())
                lock(_locker)
                    result = dB.Genres.Where(x => genre4book.Contains(x.GenreVal)).Select(x => x).ToList();

            return result;
        }
        #endregion

        #region [Keyword]
        private static List<MHLKeyword> KeywordList4DiskItems(List<IDiskItem> diskItems)
        {
            List<MHLKeyword> result = new List<MHLKeyword>();

            foreach (var item in diskItems)
                if (item is IMHLBook book)
                    result.AddRange(book.Keywords);

            return result;
        }

        private static List<Keyword4Book>? GetNewKeywordsRange4GenresList(DBModel dB, List<MHLKeyword> keywords)
        {
            List<Keyword4Book>? result = null;
            IEnumerable<MHLKeyword>? newKeywords = null;

            if(keywords.Any())
            { }

            if(newKeywords?.Any()??false)
            {
                //result = newKeywords.Select(x => x.Keyword).ToList();
            }
            return result;
        }
        #endregion
    }
}
