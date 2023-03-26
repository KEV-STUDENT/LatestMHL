using MHL_DB_Model;
using MHLCommon;
using MHLCommon.MHLBook;
using Microsoft.IdentityModel.Tokens;

namespace MHL_DB_BizLogic.BLClasses
{
    internal class BLGenres : BL4Entity<List<MHLGenre>, List<Genre>>
    {
        #region [Constructors]
        public BLGenres(DBModel dB, object? locker) : base(dB, locker) 
        {
            GetAttributesFromBook += (List<MHLGenre> list, IMHLBook book) =>
            {
                if (list != null && (book?.Genres.IsNullOrEmpty() ?? false))
                    list.AddRange(book.Genres);
            };
        }
        public BLGenres(DBModel dB) : this(dB, null) { }
        #endregion

        #region [Methods]
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

                if (genresDB?.Any() ?? false)
                    newGenres = genre4book.Except(from gb in genresDB select gb.GenreVal);
                else
                    newGenres = genre4book;
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

        protected override List<Genre>? FilterData<T3>(T3 filter)
        {
            List<Genre>? result = null;

            if (filter != null && filter is IEnumerable<FB2Genres> genres && (genres?.Any() ?? false))
                result = DB.Genres
                       .Where(g => genres.Contains(g.GenreVal)).Select(g => g).ToList();
            return result;
        }
        #endregion
    }
}