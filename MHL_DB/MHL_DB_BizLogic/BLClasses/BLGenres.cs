using MHL_DB_Model;
using MHLCommon.DataModels;
using MHLCommon;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;

namespace MHL_DB_BizLogic.BLClasses
{
    internal class BLGenres : BL4Entity<List<MHLGenre>, List<Genre>>
    {
        #region [Constructors]
        public BLGenres(DBModel dB, object? locker) : base(dB, locker) { }
        public BLGenres(DBModel dB) : base(dB) { }
        #endregion

        #region [Methods]
        //private List<Genre>? GetDBEntities4Filter(List<FB2Genres> filter)
        //{
        //    List<Genre>? genresDB = null;
        //    if (filter != null && filter.Any())
        //        if (Locker == null)
        //            genresDB = FilterData(filter);
        //        else
        //            lock (Locker)
        //                genresDB = FilterData(filter);

        //    return genresDB;
        //}

        //private List<Genre>? FilterData(IEnumerable<FB2Genres> filter)
        //{
        //    return DB.Genres
        //           .Where(g => filter.Contains(g.GenreVal)).Select(g => g).ToList();
        //}

        protected override List<Genre>? GetDBEntities4ListFromDiskItem(List<MHLGenre> attributes)
        {
            List<Genre>? result = null;

            if (attributes.Any())
            {
                List<FB2Genres> filter = attributes.GroupBy(g => g.Genre).Select(g => g.First().Genre).ToList();
                result = GetDBEntities4Filter(filter);
            }

            return result;
        }

        protected override List<Genre>? GetNewEntities4ListFromDiskItem(List<MHLGenre> attributes)
        {

            List<Genre>? result = null;
            IEnumerable<FB2Genres>? newGenres = null;

            if (attributes.Any())
            {
                IEnumerable<FB2Genres> genre4book = attributes.GroupBy(g => g.Genre).Select(g => g.First().Genre);
                List<Genre>? genresDB = GetDBEntities4Filter(genre4book);



                if ((genresDB?.Count ?? 0) == 0)
                    newGenres = genre4book;
                else
                {
                    IEnumerable<FB2Genres> genreFromDB = from gb in genresDB select gb.GenreVal;
                    if (genreFromDB.Any())
                        newGenres = genre4book.Except(genreFromDB);
                }
            }

            if (newGenres?.Any() ?? false)

                result = newGenres
                    .Select(g => new Genre()
                    {
                        GenreVal = g,
                        GenreCode = g.ToString()
                    }).ToList();

            return result;
        }

        protected override List<MHLGenre> List4DiskItems(List<IDiskItem> diskItems)
        {
            List<MHLGenre> result = new List<MHLGenre>();

            foreach (IDiskItem item in diskItems)
                if (item is IMHLBook book)
                    result.AddRange(book.Genres);

            return result;
        }

        protected override List<Genre>? FilterData<T3>(T3 filter)
        {
            List<Genre>? result = null;

            if (filter != null && filter is IEnumerable<FB2Genres> genres && (genres?.Any() ?? false))
                result = DB.Genres
                       .Where(g => genres.Contains(g.GenreVal)).Select(g => g).ToList();
            return result;
        }

        protected override bool CheckFilter<T3>(T3? filter) where T3 : default
        {
            return filter != null && filter is IEnumerable<FB2Genres> genres && (genres?.Any() ?? false);
        }
        #endregion
    }
}