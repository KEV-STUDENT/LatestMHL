using MHL_DB_Model;
using MHLCommon;
using MHLCommon.MHLBook;
using Microsoft.IdentityModel.Tokens;

namespace MHL_DB_BizLogic.BLClasses
{
    public class BLGenres : BL4Entity<MHLGenre, Genre, FB2Genres>
    {
        #region [Constructors]
        public BLGenres(DBModel dB, object? locker) : base(dB, locker)
        {
            FilterDuplicatedAttrubute = (List<MHLGenre> attributes) =>
            {
                return attributes
                     .Where(ab => ab.Genre != FB2Genres.none)
                     .GroupBy(a => ConvertAttribute(a))
                     .Select(a => a.First()).ToList();
            };
        }
        public BLGenres(DBModel dB) : this(dB, null) { }
        #endregion

        #region [Methods]
        protected override List<Genre> CheckInDB(IEnumerable<FB2Genres> filter)
        {
            return DB.Genres
                   .Where(a => filter.Contains(a.GenreVal))
                   .Select(a => a)
                   .ToList();
        }

        protected override void GetAttributesFromBook(List<MHLGenre> list, IMHLBook book)
        {
            if (list != null && !(book?.Genres?.IsNullOrEmpty() ?? false))
                list.AddRange(book.Genres);
        }

        protected override IEnumerable<FB2Genres>? ConvertAttributes2ComparedList(List<MHLGenre> attributes)
        {
            return attributes
               .Where(ab => ab.Genre != FB2Genres.none)
               .GroupBy(a => ConvertAttribute(a))
               .Select(a => a.Key);
        }

        protected override Genre ConvertAttribute2DBEntity(MHLGenre a)
        {
            return new Genre()
            {
                GenreVal = a.Genre,
                GenreCode = a.Genre.ToString()
            };
        }

        protected override FB2Genres ConvertAttribute(MHLGenre a)
        {
            return a.Genre;
        }

        protected override FB2Genres ConvertDBEntity(Genre a)
        {
            return a.GenreVal;
        }

        protected override bool AddInDB(List<Genre>? newEntitys)
        {
            bool ret = !newEntitys.IsNullOrEmpty();

            if (ret)
                DB.Genres.AddRange(newEntitys);

            return ret;
        }
        #endregion
    }
}