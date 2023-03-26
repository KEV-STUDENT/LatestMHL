using MHL_DB_BizLogic.BLClasses;
using MHL_DB_Model;
using MHLCommon;
using MHLCommon.DataModels;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static System.Reflection.Metadata.BlobBuilder;


namespace MHL_DB_BizLogic.SQLite
{
    static public class Bizlogic4DB
    {
        static private IBLEntity<List<T1>, List<T2>>? GetEntityBL<T1, T2>(DBModel dB)
        {
            IBLEntity<List<T1>, List<T2>>? bl = null;

            Type dbType = typeof(T2);

            if (dbType == typeof(Genre))
                bl = new BLGenres(dB) as IBLEntity<List<T1>, List<T2>>;
            else if (dbType == typeof(Keyword4Book))
                bl = new BLKeywords(dB) as IBLEntity<List<T1>, List<T2>>;
            else if (dbType == typeof(Author))
                bl = new BLAuthors(dB) as IBLEntity<List<T1>, List<T2>>;

            return bl;
        }

        static private bool SetNewValue2DataBase<T>(DBModel dB, List<T> newValues)
        {
            bool res = true;

            if (newValues is List<Genre> genres && !genres.IsNullOrEmpty())
                dB.Genres.AddRange(genres);
            else if (newValues is List<Keyword4Book> keywords && !keywords.IsNullOrEmpty())
                dB.Keyword4Books.AddRange(keywords);
            else if (newValues is List<Author> authors && !authors.IsNullOrEmpty())
                dB.Authors.AddRange(authors);
            else
                res = false;

            return res;
        }

        static public int Export_Entity<T1, T2>(DBModel dB, List<T1>? inList, out List<T2>? outList)
        {
            outList = null;

            if (!(inList?.Any() ?? false))
                return -1;

            int res = -1;
            IBLEntity<List<T1>, List<T2>>? bl = GetEntityBL<T1, T2>(dB);
            if (bl != null)
            {
                List<T2>? newValues = bl.GetNewEntities4ListFromDiskItem(inList);
                res = newValues?.Count ?? 0;

                if (res != 0 && SetNewValue2DataBase(dB, newValues))
                    dB.SaveChanges();

                outList = bl.GetDBEntities4ListFromDiskItem(inList);
            }
            return res;
        }

        static public int Export_Genres(DBModel dB, List<MHLGenre>? genres, out List<Genre>? genresDB)
        {
            genresDB = null;
            if (!(genres?.Any() ?? false))
                return -1;

            int res = -1;
            BLGenres blG = new BLGenres(dB);

            if (blG is IBLEntity<List<MHLGenre>, List<Genre>> bl && bl != null)
            {
                List<Genre>? newGenres = bl.GetNewEntities4ListFromDiskItem(genres);


                res = newGenres?.Count ?? 0;
                if (res != 0)
                {
                    dB.Genres.AddRange(newGenres);
                    dB.SaveChanges();
                }

                genresDB = bl.GetDBEntities4ListFromDiskItem(genres);
            }
            System.Diagnostics.Debug.WriteLine(genresDB?.Count ?? -1);

            return res;
        }

        //static public int Export_Keywords(DBModel dB, List<MHLKeyword>? keywords, out List<Keyword4Book>? keywordDB)
        //{
        //    keywordDB = null;
        //    if (keywords == null || keywords.Any())
        //        return -1;

        //    int res = -1;
        //    BLKeywords blK = new BLKeywords(dB);

        //    if (blK is IBLEntity<List<MHLKeyword>, List<Keyword4Book>> bl && bl != null)
        //    {
        //        List<Keyword4Book>? newValues = bl.GetDBEntities4ListFromDiskItem(keywords);
        //        res = newValues?.Count ?? 0;
        //        if (res != 0)
        //        {
        //            dB.Keyword4Books.AddRange(newValues);
        //            dB.SaveChanges();
        //        }

        //        keywordDB = bl.GetDBEntities4ListFromDiskItem(keywords);
        //    }

        //    return res;
        //}

        public static int Export_Sequences(DBModel dB, MHLSequenceNum? sequenceAndNumber, out Sequence4Book? sequence)
        {
            int res = -1;

            if (sequenceAndNumber == null)
            {
                sequence = null;
                return res;
            }

            sequence = dB.Sequence4Books
                           .Where(x => x.Name == sequenceAndNumber.Name.Trim())
                           .FirstOrDefault();

            if (sequence == null)
            {
                sequence = new Sequence4Book() { Name = sequenceAndNumber.Name.Trim() };
                dB.Sequence4Books.Add(sequence);

                dB.SaveChanges();
                res = 1;
            }
            else res = 0;

            return res;
        }

        public static int Export_Sequences(DBModel dB, List<string?>? sequenceFB2, out List<Sequence4Book>? sequenceDB)
        {
            sequenceDB = null;
            int res = -1;

            if (sequenceFB2 == null || !sequenceFB2.Any())
                return res;

            var seqUpper = sequenceFB2
                 .Where(s => !string.IsNullOrEmpty(s))
                 .GroupBy(s => s.Trim().ToUpper()).Select(k => k.Key);

            var sequence = dB.Sequence4Books.Where(s => seqUpper.Contains(s.Name.ToUpper()))
                .Select(s => s.Name.Trim().ToUpper()).ToList();

            List<string> sequenceAdd;
            if (sequence != null && sequence.Any())
            {
                sequenceAdd = seqUpper.Except(sequence).ToList();
                sequenceAdd = sequenceFB2
                    .Where(s => sequenceAdd.Contains((s ?? string.Empty).Trim().ToUpper()))
                    .Select(s => s ?? string.Empty).ToList();
            }
            else
            {
                sequenceAdd = sequenceFB2
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Select(s => s ?? string.Empty).ToList();
            }

            if (sequenceAdd.Any())
            {
                var addList = sequenceAdd.Select(s => new Sequence4Book() { Name = s.Trim() });
                dB.Sequence4Books.AddRange(addList);
                res = addList.Count();

                dB.SaveChanges();
            }
            else
                res = 0;

            sequenceDB = dB.Sequence4Books
                .Select(s => s).ToList().Where(s => seqUpper.Contains(s.Name.Trim().ToUpper())).ToList();

            return res;
        }

        public static int Export_Volumes(DBModel dB, MHLSequenceNum? sequenceAndNumber, out Volume? volume)
        {
            int res = -1;
            volume = null;

            if (sequenceAndNumber != null)
            {
                res = Export_Sequences(dB, sequenceAndNumber, out Sequence4Book? sequence);

                if (res > -1 && sequence != null)
                {

                    if (res == 0)
                    {
                        volume = dB.Volumes
                            .Where(x => x.Number == sequenceAndNumber.Number && x.SequenceId == sequence.Id)
                            .FirstOrDefault();
                    }

                    if (volume == null)
                    {
                        res = 1;
                        volume = new Volume()
                        {
                            Number = sequenceAndNumber.Number,
                            Sequence = sequence
                        };

                        dB.Volumes.Add(volume);
                        dB.SaveChanges();
                    }
                    else res = 0;
                }
            }
            return res;
        }

        /*public static int Export_Authors(DBModel dB, List<MHLAuthor>? authorsFB2, out List<Author>? authorsDB)
        {
            authorsDB = null;
            if (!(authorsFB2?.Any() ?? false))
                return -1;


            int res = -1;
            BLAuthors blA = new BLAuthors(dB);
            if (blA is IBLEntity<List<MHLAuthor>, List<Author>> bl && bl != null)
            {
                List<Author>? newAuthors = bl.GetNewEntities4ListFromDiskItem(authorsFB2);
                res = newAuthors?.Count() ?? 0;
                if (res != 0)
                {
                    dB.Authors.AddRange(newAuthors);
                    dB.SaveChanges();
                }
                authorsDB = bl.GetDBEntities4ListFromDiskItem(authorsFB2);
            }

            return res;
        }*/

        public static int Export_FB2(string fileSQlite, IDiskItem? fb2)
        {
            int res;
            using (DBModel dB = new DBModelSQLite(fileSQlite))
            {
                res = Export_FB2(dB, fb2);
                if (res > 0)
                    dB.SaveChanges();
            }
            return res;
        }

        public static int Export_FB2(DBModel dB, IDiskItem? fb2)
        {
            int res = -1;
            if (fb2 == null)
                return res;

            if (fb2 is IMHLBook book)
            {

                bool isInDB = dB.Books.Where(x => x.Path2File == fb2.Path2Item && x.EntityInZIP == fb2.Name).Any();
                if (!isInDB)
                {
                    int save = (Export_Entity(dB, book.Authors, out List<Author>? authors) > 0 ? 1 : 0) +
                        (Export_Entity(dB, book.Genres, out List<Genre>? genres) > 0 ? 1 : 0) +
                        (Export_Entity(dB, book.Keywords, out List<Keyword4Book>? keywords) > 0 ? 1 : 0) +
                        (Export_Volumes(dB, book.SequenceAndNumber, out Volume? volume) > 0 ? 1 : 0) +
                        (Export_Publishers(dB, book.Publisher, out Publisher? publisher) > 0 ? 1 : 0);


                    BookFileExtends fileExtends = BookFileExtends.None;
                    if (fb2 is IFile file)
                    {
                        fileExtends = file.IsZipEntity ? BookFileExtends.ZIP : BookFileExtends.FB2;
                    }

                    dB.Books.Add(
                        new Book()
                        {
                            Path2File = fb2.Path2Item,
                            EntityInZIP = fb2.Name,
                            Extends = fileExtends,
                            Title = book.Title,
                            Cover = book.Cover,
                            Annotation = book.Annotation,
                            Authors = (authors?.Count ?? 0) > 0 ? authors : null,
                            Genres = (genres?.Count ?? 0) > 0 ? genres : null,
                            Keywords = (keywords?.Count ?? 0) > 0 ? keywords : null,
                            Volume = volume,
                            Year = book.Year,
                            Publisher = publisher,
                        }
                        );
                    dB.SaveChanges();
                    res = 1;
                }
                else res = 0;

            }
            return res;
        }

        public static int Export_Publishers(DBModel dB, IPublisher? publisherFB2, out Publisher? publisherDB)
        {
            int res = -1;
            publisherDB = null;

            if (publisherFB2 != null)
            {
                publisherDB = dB.Publishers
                    .Where(p => p.City == publisherFB2.City && p.Name == publisherFB2.Name)
                    .Select(p => p).FirstOrDefault();

                if (publisherDB == null)
                {
                    res = 1;
                    publisherDB = new Publisher()
                    {
                        City = publisherFB2.City,
                        Name = publisherFB2.Name,
                    };
                    dB.Publishers.Add(publisherDB);
                    dB.SaveChanges();
                }
                else res = 0;
            }

            return res;
        }

        public static int Export_FB2List(DBModel dB, List<IDiskItem> diskItems)
        {
            int res = 0;
            IEnumerable<(string Key, IDiskItem DiskItem, IMHLBook MHLBook)>? booksList = BizLogic.CheckBooksInDB(dB, diskItems);

            if (booksList?.Any() ?? false)
            {
                List<IMHLBook> books = booksList.Select(b => b.MHLBook).ToList();

                BLAuthors blA = new BLAuthors(dB);
                List<Author>? authors = blA.GetNewEntities4DiskItem(diskItems);

                BLGenres blG = new BLGenres(dB);
                List<Genre>? genres = blG.GetNewEntities4DiskItem(diskItems);

                int authorsCnt, genresCnt, keywordsCnt, volumesCnt, publichersCnt;

                //authorsCnt = Export_Authors4BookList(dB, books, out List<Author>? authors);
                //if (authorsCnt > -1)
                authorsCnt = authors?.Count ?? 0;
                genresCnt = genres?.Count ?? 0;

                //genresCnt = Export_Genres4BookList(dB, books, out List<Genre>? genres);
                //if (genresCnt > -1)
                //    genresCnt = genres?.Count ?? 0;

                keywordsCnt = Export_Keywords4BookList(dB, books, out List<Keyword4Book>? keywords);
                if (keywordsCnt > -1)
                    keywordsCnt = keywords?.Count ?? 0;

                volumesCnt = Export_Volumes4BookList(dB, books, out List<Volume>? volumes);
                if (volumesCnt > -1)
                    volumesCnt = volumes?.Count ?? 0;

                publichersCnt = Export_Publishers4BookList(dB, books, out List<Publisher>? publishers);
                if (publichersCnt > -1)
                    publichersCnt = publishers?.Count ?? 0;

                Book book;
                foreach (var b in booksList)
                {
                    book = new Book()
                    {
                        Path2File = b.DiskItem.Path2Item,
                        EntityInZIP = b.DiskItem.Name,
                        Title = b.MHLBook.Title,
                        Annotation = b.MHLBook.Annotation,
                        Year = b.MHLBook.Year,
                        Cover = b.MHLBook.Cover
                    };

                    if (volumesCnt > 0)
                        book.Volume = volumes?
                            .Where(v => v.Sequence.Name.ToUpper() == (b.MHLBook?.SequenceAndNumber?.Name?.ToUpper() ?? string.Empty))
                            .FirstOrDefault();

                    book.Publisher = publishers?
                        .Where(p => p.Name.ToUpper() == (b.MHLBook?.Publisher?.Name?.ToUpper() ?? string.Empty) &&
                            p.City.ToUpper() == (b.MHLBook?.Publisher?.City?.ToUpper() ?? string.Empty))
                        .FirstOrDefault();

                    if (authorsCnt > 0)
                        book.Authors = authors?
                            .Join(b.MHLBook.Authors,
                            a => string.Format("{0}|{1}|{2}",
                                     a.FirstName?.Trim() ?? string.Empty,
                                     a.LastName?.Trim() ?? string.Empty,
                                     a.MiddleName?.Trim() ?? string.Empty).ToUpper(),
                            ba => string.Format("{0}|{1}|{2}",
                                     ba.FirstName?.Trim() ?? string.Empty,
                                     ba.LastName?.Trim() ?? string.Empty,
                                     ba.MiddleName?.Trim() ?? string.Empty).ToUpper(),
                            (a, ab) => a).ToList();

                    if (genresCnt > 0)
                        book.Genres = genres?
                               .Join(b.MHLBook.Genres,
                                g => g.GenreVal,
                                bg => bg.Genre, (g, bg) => g).ToList();

                    if (keywordsCnt > 0)
                        book.Keywords = keywords?
                                .Join(b.MHLBook.Keywords,
                                k => k.Keyword.ToUpper(),
                                bk => bk.Keyword.ToUpper(),
                                (k, bk) => k).ToList();

                    dB.Books.Add(book);
                }
                dB.SaveChanges();
                res = booksList.Count();
            }
            return res;
        }

        public static int Export_Publishers4BookList(DBModel dB, List<IMHLBook> books, out List<Publisher>? publishers)
        {
            publishers = null;
            int res = -1;

            if (books == null || !books.Any())
                return res;

            var listPublishers = books
                .Where(b => b.Publisher != null)
                .GroupBy(b => string.Format("{0}|{1}",
                    b.Publisher.Name.Trim(), b.Publisher.City.Trim()).ToLower())
                .Select(b => new { b.Key, b.First().Publisher });

            IEnumerable<string> list = listPublishers.Select(l => l.Key);

            List<string> listDB = dB.Publishers
                    .Where(p => list.Contains(p.Name.Trim().ToLower() + "|" + p.City.Trim().ToLower()))
                    .Select(p => (p.Name.Trim() + "|" + p.City.Trim()).ToLower()).ToList();

            IEnumerable<string> list4Add;
            if (listDB.Any())
                list4Add = list.Except(listDB);
            else
                list4Add = list;

            if (list4Add.Any())
            {
                var list4DB = list4Add.Join(
                    listPublishers,
                    l => l,
                    p => p.Key,
                    (l, p) => new Publisher() { City = p.Publisher.City, Name = p.Publisher.Name });

                dB.Publishers.AddRange(list4DB);
                dB.SaveChanges();

                res = list4DB.Count();
            }
            else
                res = 0;

            publishers = dB.Publishers
                   .Where(p => list.Contains(p.Name.ToLower() + "|" + p.City.ToLower()))
                   .Select(p => p).ToList();

            return res;
        }

        public static int Export_Volumes4BookList(DBModel dB, List<IMHLBook>? books, out List<Volume>? volumes)
        {
            volumes = null;
            int res = -1;

            if (books == null || !books.Any())
                return res;

            var sequences = books
                .Where(b => !string.IsNullOrEmpty(b.SequenceAndNumber?.Name))
                .GroupBy(b => (b.SequenceAndNumber?.Name ?? string.Empty).Trim().ToUpper())
                .Select(g => g.First().SequenceAndNumber?.Name).ToList();

            if (Export_Sequences(dB, sequences, out List<Sequence4Book>? sequenceDB) >= 0)
            {
                var volumesAdded = books
                .Where(b => b.SequenceAndNumber != null)
                .GroupBy(b => string.Format("{0}|{1}", b.SequenceAndNumber?.Number ?? 0, (b.SequenceAndNumber?.Name ?? string.Empty).Trim().ToUpper()))
                .Select(g => new { g.Key, Value = g.First().SequenceAndNumber });

                IEnumerable<string> keysAdded = volumesAdded.Select(v => v.Key);

                var volumesSeq = dB.Volumes.Include(b => b.Sequence)
                    .GroupJoin(sequenceDB, v => v.SequenceId, s => s.Id, (v, s) => new { v.Number, v.Sequence.Name })
                    .ToList();

                IEnumerable<string> volumesDB = volumesSeq
                    .Join(volumesAdded,
                        vs => string.Format("{0}|{1}", vs.Number, vs.Name.Trim().ToUpper()),
                        va => va.Key,
                        (vs, va) => va.Key);

                IEnumerable<MHLSequenceNum?> volumesNew;
                if (volumesDB.Any())
                {
                    keysAdded = keysAdded.Except(volumesDB);
                    volumesNew = from v in volumesAdded
                                 where keysAdded.Contains(v.Key)
                                 select v.Value;
                }
                else
                    volumesNew = volumesAdded.Select(v => v.Value);

                if (volumesNew.Any())
                {
                    var list = volumesNew
                        .Join(sequenceDB,
                            v => v.Name.Trim().ToUpper(),
                            s => s.Name.Trim().ToUpper(),
                            (v, s) => new Volume() { Number = v.Number, SequenceId = s.Id });

                    dB.Volumes.AddRange(list);
                    dB.SaveChanges();

                    res = list.Count();
                }
                else
                    res = 0;

                volumes = dB.Volumes.Include(b => b.Sequence)
                    .GroupJoin(sequenceDB, v => v.SequenceId, s => s.Id, (v, s) => v).ToList();

                volumes = volumes
                            .Join(volumesAdded,
                                v => string.Format("{0}|{1}", v.Number, v.Sequence.Name.Trim().ToUpper()),
                                va => va.Key,
                                (v, va) => v).ToList();
            }
            return res;
        }

        private static int Export_Keywords4BookList(DBModel dB, List<IMHLBook> books, out List<Keyword4Book>? keywords)
        {
            List<MHLKeyword> keywordsAdded = new List<MHLKeyword>();
            foreach (var book in books)
            {
                keywordsAdded.AddRange(book.Keywords);
            }
            return Export_Entity(dB, keywordsAdded, out keywords);
        }

        //private static int Export_Genres4BookList(DBModel dB, List<IMHLBook> books, out List<Genre>? genres)
        //{
        //    List<MHLGenre> genresAdded = new List<MHLGenre>();
        //    foreach (var book in books)
        //    {
        //        genresAdded.AddRange(book.Genres);
        //    }
        //    return Export_Genres(dB, genresAdded, out genres);
        //}

        //private static int Export_Authors4BookList(DBModel dB, List<IMHLBook> books, out List<Author>? authors)
        //{
        //    List<MHLAuthor> authorsAdded = new List<MHLAuthor>();
        //    foreach (var book in books)
        //        authorsAdded.AddRange(book.Authors);

        //    return Export_Authors(dB, authorsAdded, out authors);
        //}
    }
}