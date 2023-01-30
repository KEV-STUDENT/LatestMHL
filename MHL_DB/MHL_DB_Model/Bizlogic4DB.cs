using MHL_DB_SQLite;
using MHLCommon;
using MHLCommon.MHLBook;

namespace MHL_DB_Model
{
    static public class Bizlogic4DB
    {
        static public int Export_Genres(DBModel dB, List<MHLGenre> genres)
        {
            var genre4book = (from gb in genres select gb.Genre).Distinct();
            var genreFromDB = dB.Genres.Where(x => genre4book.Contains(x.GenreVal)).Select(x => x.GenreVal);
            var newGenres = (from gb in genre4book where !genreFromDB.Contains(gb) select gb).Distinct();

            Parallel.ForEach(newGenres, (fb2Genre) =>
            {
                dB.Genres.Add(new Genre()
                {
                    GenreVal = fb2Genre,
                    GenreCode = fb2Genre.ToString()
                });
            });

            return newGenres.Count();
        }

        static public int Export_Keywords(DBModel dB, List<MHLKeyword> keywords)
        {
            var kewords4book = (from kb in keywords select kb.Keyword.Trim().ToUpper()).Distinct();
            var keywordsFromDB =
                dB.Keyword4Books.Where(x => kewords4book.Contains(x.Keyword.Trim().ToUpper()))
                    .Select(x => x.Keyword.Trim().ToUpper());
            var newKeywords = (from kb in kewords4book where !keywordsFromDB.Contains(kb) select kb).Distinct();

            Parallel.ForEach(newKeywords, (keyword) =>
            {
                dB.Keyword4Books.Add(new Keyword4Book()
                {
                    Keyword = keyword,
                });

            });
            return newKeywords.Count();
        }

        public static int Export_Sequences(DBModel dB, MHLSequenceNum? sequenceAndNumber, out Sequence4Book? sequence)
        {
            int res = -1;

            if (sequenceAndNumber != null)
            {

                string newSequence = sequenceAndNumber.Name.ToUpper();

                sequence = dB.Sequence4Books
                           .Where(x => x.Name.ToUpper().Contains(newSequence))
                           .FirstOrDefault();

                res = (sequence == null ? 1 : 0);

                if (res == 1)
                {
                    sequence = new Sequence4Book() { Name = sequenceAndNumber.Name };
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

                System.Diagnostics.Debug.WriteLine(res);
                System.Diagnostics.Debug.WriteLine(sequence != null);

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
    }
}