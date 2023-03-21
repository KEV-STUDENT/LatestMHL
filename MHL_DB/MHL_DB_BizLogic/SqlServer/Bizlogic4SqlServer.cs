using MHLCommon.MHLDiskItems;
using MHL_DB_MSSqlServer;
using MHLCommon.MHLBook;
using System.Globalization;
using System.Collections.Generic;
using MHL_DB_Model;
using System.Threading.Tasks;
using MHL_DB_BizLogic.BLClasses;
using MHLCommon;

namespace MHL_DB_BizLogic.SqlServer
{
    static public class Bizlogic4SqlServer
    {
        private static object _locker = new object();
        public static async Task<int> Export_FB2List_Async(DBModelMSSqlServer dB, List<IDiskItem> books)
        {
            int res = 0;
            IEnumerable<(string Key, IDiskItem DiskItem, IMHLBook MHLBook)>? booksList = BizLogic.CheckBooksInDB(dB, books);

            if (booksList?.Any() ?? false)
            {
                List<IMHLBook> mhlBooks = booksList.Select(b => b.MHLBook).ToList();
                BLAuthors blA = new BLAuthors(dB, _locker);
                Task<List<Author>?> ta = blA.GetNewEntities4DiskItemAsync(books);
                BLGenres blG = new BLGenres(dB, _locker);
                Task<List<Genre>?> tg = blG.GetNewEntities4DiskItemAsync(books);

                await Task.WhenAll(ta, tg);

                List<Task> tasks = new List<Task>();
                if ((ta.Result?.Count ?? 0) > 0)
                {
                    //System.Diagnostics.Debug.WriteLine(ta.Result.Count());
                    res += ta.Result?.Count ?? 0;
                    tasks.Add(dB.Authors.AddRangeAsync(ta.Result));
                }

                if ((tg.Result?.Count ?? 0) > 0)
                {
                    res += tg.Result?.Count ?? 0;
                    tasks.Add(dB.Genres.AddRangeAsync(tg.Result));
                }


                if (res > 0)
                {
                    await Task.WhenAll(tasks);
                    return await dB.SaveChangesAsync();
                }
            }

            return res;
        }

        public static int Export_FB2List(DBModelMSSqlServer dB, List<IDiskItem> books)
        {
            int res = 0;
            IEnumerable<(string Key, IDiskItem DiskItem, IMHLBook MHLBook)>? booksList = BizLogic.CheckBooksInDB(dB, books);

            if (booksList?.Any() ?? false)
            {
                BLAuthors blA = new BLAuthors(dB, _locker);
                Task<List<Author>?> ta = blA.GetNewEntities4DiskItemAsync(books);


                Task<List<Genre>?> tg = BizLogic.GetNewEntityRange4BooksListAsync<Genre>(dB, books);
                Task<List<Keyword4Book>?> tk = BizLogic.GetNewEntityRange4BooksListAsync<Keyword4Book>(dB, books); ;

                Task.WaitAll(ta, tg, tk);

                List<Task> tasks = new List<Task>();
                if ((ta.Result?.Count() ?? 0) > 0)
                {
                    System.Diagnostics.Debug.WriteLine(ta.Result.Count());
                    res += ta.Result.Count();
                    tasks.Add(dB.Authors.AddRangeAsync(ta.Result));
                }

                if ((tg.Result?.Count() ?? 0) > 0)
                {
                    res += tg.Result.Count();
                    tasks.Add(dB.Genres.AddRangeAsync(tg.Result));
                }

                if (res > 0)
                {
                    Task.WaitAll(tasks.ToArray());
                    dB.SaveChanges();
                    res = -1;
                }
            }
            return res;
        }
    }
}
