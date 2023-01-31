using MHL_DB_SQLite;
using MHLCommon;
using MHLCommon.DataModels;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;

namespace MHL_DB_Model
{
    static public class Bizlogic4DB
    {
        static public int Export_Genres(DBModel dB, List<MHLGenre>? genres, out List<Genre>? genresDB)
        {
            genresDB = null;
            if (genres == null)
                return -1;

            var genre4book = (from gb in genres select gb.Genre).Distinct();

            genresDB = dB.Genres.Where(x => genre4book.Contains(x.GenreVal)).Select(x => x).ToList();

            IEnumerable<FB2Genres> newGenres;
            if (genresDB.Count == 0)
                newGenres = genre4book;
            else
            {
                var genreFromDB = (from gb in genresDB select gb.GenreVal).Distinct();
                newGenres = (from gb in genre4book where !genreFromDB.Contains(gb) select gb).Distinct();
            }

            if (newGenres.Any())
            {
                foreach(var fb2Genre in  newGenres)
                {
                    dB.Genres.Add(new Genre()
                    {
                        GenreVal = fb2Genre,
                        GenreCode = fb2Genre.ToString()
                    });
                }

                genresDB = dB.Genres.Where(x => genre4book.Contains(x.GenreVal)).Select(x => x).ToList();

                System.Diagnostics.Debug.WriteLine("{0} <-> {1}", newGenres.Count(), genresDB.Count);
            }
            return newGenres.Count();
        }

        static public int Export_Keywords(DBModel dB, List<MHLKeyword>? keywords, out List<Keyword4Book>? keywordDB)
        {
            keywordDB = null;
            if (keywords == null)
                return -1;

            foreach (var keyword in keywords)
            {
                System.Diagnostics.Debug.WriteLine(keyword.Keyword);
            }

            var kewords4book = (from kb in keywords select kb.Keyword.Trim()).Distinct();

            keywordDB = dB.Keyword4Books.Where(x => kewords4book.Contains(x.Keyword))
                    .Select(x => x).ToList();


            IEnumerable<string> newKeywords;
            if (keywordDB.Count == 0)
            {
                newKeywords = kewords4book;
            }
            else
            {
                var keywordsFromDB = (from kb in keywordDB select kb.Keyword.Trim()).Distinct();
                newKeywords = (from kb in kewords4book where !keywordsFromDB.Contains(kb) select kb).Distinct();
            }

            if (newKeywords.Any())
            {
                foreach(var keyword in newKeywords)
                {
                    dB.Keyword4Books.Add(new Keyword4Book()
                    {
                        Keyword = keyword.Trim(),
                    });
                }

                keywordDB = dB.Keyword4Books.Where(x => kewords4book.Contains(x.Keyword))
                        .Select(x => x).ToList();
               
                System.Diagnostics.Debug.WriteLine("=======================================");
                foreach (var keyword in keywordDB)
                {
                    System.Diagnostics.Debug.WriteLine(keyword.Keyword);
                }
                System.Diagnostics.Debug.WriteLine("=======================================");
                foreach (var keyword in newKeywords)
                {
                    System.Diagnostics.Debug.WriteLine(keyword);
                }

                System.Diagnostics.Debug.WriteLine("{0} <-> {1}", newKeywords.Count(), keywordDB.Count);
            }
            return newKeywords.Count();
        }

        public static int Export_Sequences(DBModel dB, MHLSequenceNum? sequenceAndNumber, out Sequence4Book? sequence)
        {
            int res = -1;

            if (sequenceAndNumber != null)
            {
                sequence = dB.Sequence4Books
                           .Where(x => x.Name == sequenceAndNumber.Name.Trim())
                           .FirstOrDefault();

                res = (sequence == null ? 1 : 0);

                if (res == 1)
                {
                    sequence = new Sequence4Book() { Name = sequenceAndNumber.Name.Trim() };
                    dB.Sequence4Books.Add(sequence);
                }
            }
            else sequence = null;

            return res;
        }

        public static int Export_Volumes(DBModel dB, MHLSequenceNum? sequenceAndNumber, out Volume? volume)
        {
            int res = -1;
            volume = null;

            if (sequenceAndNumber != null)
            {
                Sequence4Book? sequence = null;
                res = Export_Sequences(dB, sequenceAndNumber, out sequence);

                if ((res > -1) && (sequence != null))
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
                    }
                    else res = 0;
                }
            }
            return res;
        }

        public static int Export_Authors(DBModel dB, List<MHLAuthor>? authorsFB2, out List<Author>? authorsDB)
        {
            int res = -1;
            authorsDB = null;

            if (authorsFB2 != null)
            {
                var authors4book = (
                    from ab in authorsFB2
                    select string.Format("{0}|{1}|{2}",
                        ab.FirstName?.Trim() ?? string.Empty,
                        ab.LastName?.Trim() ?? string.Empty,
                        ab.MiddleName?.Trim() ?? string.Empty)).Distinct();


                authorsDB = dB.Authors
                    .Where(a => authors4book.Contains(a.FirstName + "|" + a.LastName + "|" + a.MiddleName))
                    .Select(a => a)
                    .ToList();

                List<MHLAuthor>? newAuthors;

                if (authorsDB.Count == 0)
                {
                    newAuthors = authorsFB2;
                }
                else
                {
                    var authorsFromDB = (
                        from ab in authorsDB
                        select string.Format("{0}|{1}|{2}",
                            ab.FirstName?.Trim() ?? string.Empty,
                            ab.LastName?.Trim() ?? string.Empty,
                            ab.MiddleName?.Trim() ?? string.Empty)).Distinct();

                    newAuthors = (from ab in authorsFB2
                                  where !authorsFromDB.Contains(
                     string.Format("{0}|{1}|{2}",
                        ab.FirstName?.Trim() ?? string.Empty,
                        ab.LastName?.Trim() ?? string.Empty,
                        ab.MiddleName?.Trim() ?? string.Empty))
                                  select ab).Distinct().ToList();
                }

                res = newAuthors?.Count ?? 0;

                if (res != 0)
                {
                    foreach(var author in newAuthors)
                    {
                        dB.Authors.Add(new Author()
                        {
                            LastName = author.LastName?.Trim(),
                            FirstName = author.FirstName?.Trim(),
                            MiddleName = author.MiddleName?.Trim()
                        });
                    }

                    authorsDB = dB.Authors
                        .Where(a => authors4book.Contains(a.FirstName + "|" + a.LastName + "|" + a.MiddleName))
                        .Select(a => a)
                        .ToList();
                }
            }

            return res;
        }
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

            if(fb2 is IMHLBook book)
            {

                bool isInDB = dB.Books.Where(x=> x.Path2File == fb2.Path2Item && x.EntityInZIP == fb2.Name).Any();
                if (!isInDB)
                {
                    List<Author>? authors;
                    List<Genre>? genres;
                    List<Keyword4Book>? keywords;
                    Volume? volume;

                    int save = (Export_Authors(dB, book.Authors, out authors) > 0 ? 1 : 0) +
                        (Export_Genres(dB, book.Genres, out genres) > 0 ? 1 : 0) +
                        (Export_Keywords(dB, book.Keywords, out keywords) > 0 ? 1 : 0) +
                        (Export_Volumes(dB, book.SequenceAndNumber, out volume) > 0 ? 1 : 0);


                    BookFileExtends fileExtends = BookFileExtends.None;
                    if (fb2 is IFile file)
                    {
                        fileExtends = (file.IsZipEntity ? BookFileExtends.ZIP : BookFileExtends.FB2);
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
                        }
                        );
                    res = 1;
                }
                else res = 0;

            }
            return res;
        }
    }
}