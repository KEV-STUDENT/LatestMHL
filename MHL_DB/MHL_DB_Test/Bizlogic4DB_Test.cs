using MHL_DB_Model;
using MHL_DB_SQLite;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using MHLSourceOnDisk;
using MHL_DB_BizLogic.SQLite;

namespace MHL_DB_Test
{
    [TestClass]
    public class Bizlogic4DB_Test
    {
        private object locker = new object();

        #region [Genres]
        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2", @"F:\1\test\destinationDB\ExportFB2_G1.SQLite")]
        [DataRow(@"F:\1\test\426096.fb2", "", @"F:\1\test\destinationDB\ExportFB2_G1.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2_G1.SQLite")]
        public void Export2SQLite_Genres(string pathFile, string fb2Name, string fileSQlite)
        {
            DiskItemFileFB2? itemFB2;
            int actual = -1, expected = -2;
            bool result;

            if (string.IsNullOrEmpty(fb2Name))
                itemFB2 = DiskItemFabrick.GetDiskItem(pathFile) as DiskItemFileFB2;
            else
                itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathFile, fb2Name) as DiskItemFileFB2;

            IMHLBook? book = itemFB2;
            result = book != null;
            if (result)
            {
                lock (locker)
                {
                    using (DBModel dB = new DBModelSQLite(fileSQlite))
                    {
                        actual = dB.Genres.ToList().Count;
                        expected = Bizlogic4DB.Export_Genres(dB, book?.Genres, out List<Genre>? genresDB);
                        CheckExpected(ref actual, ref expected, dB);
                        actual = dB.Genres.ToList().Count;
                    }
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2_G2.SQLite")]
        public void Export2SQLite_Genres_Duplicated(string pathFile, string fb2Name, string fileSQlite)
        {
            DiskItemFileFB2? itemFB2;
            int actual = -1, expected = -2;
            bool result;

            if (string.IsNullOrEmpty(fb2Name))
                itemFB2 = DiskItemFabrick.GetDiskItem(pathFile) as DiskItemFileFB2;
            else
                itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathFile, fb2Name) as DiskItemFileFB2;

            IMHLBook? book = itemFB2;
            result = book != null;
            if (result)
            {
                lock (locker)
                {
                    using (DBModel dB = new DBModelSQLite(fileSQlite))
                    {
                        if (Bizlogic4DB.Export_Genres(dB, book?.Genres, out List<Genre>? genresDB) > 0)
                        {
                            dB.SaveChanges();
                        }
                        expected = dB.Genres.ToList().Count;

                        if (Bizlogic4DB.Export_Genres(dB, book?.Genres, out genresDB) > 0)
                        {
                            dB.SaveChanges();
                        }
                        actual = dB.Genres.ToList().Count;
                    }
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2_G3.SQLite")]
        public void Export2SQLite_Genres_Out(string pathFile, string fb2Name, string fileSQlite)
        {
            DiskItemFileFB2? itemFB2;
            int actual, expected;
            bool result;

            if (string.IsNullOrEmpty(fb2Name))
                itemFB2 = DiskItemFabrick.GetDiskItem(pathFile) as DiskItemFileFB2;
            else
                itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathFile, fb2Name) as DiskItemFileFB2;

            IMHLBook? book = itemFB2;
            List<Genre>? genresDB = null;

            result = book != null;
            if (result)
            {
                lock (locker)
                {
                    using (DBModel dB = new DBModelSQLite(fileSQlite))
                    {
                        Bizlogic4DB.Export_Genres(dB, book?.Genres, out genresDB);
                        {
                            dB.SaveChanges();
                        }
                    }
                }
            }

            expected = book?.Genres?.Count ?? 0;
            actual = genresDB?.Count ?? 0;

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region [Keywords]
        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2", @"F:\1\test\destinationDB\ExportFB2_K1.SQLite")]
        [DataRow(@"F:\1\test\426096.fb2", "", @"F:\1\test\destinationDB\ExportFB2_K1.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2_K1.SQLite")]
        public void Export2SQLite_Keywords(string pathFile, string fb2Name, string fileSQlite)
        {
            DiskItemFileFB2? itemFB2;
            int actual = -1, expected = -2;
            bool result;

            if (string.IsNullOrEmpty(fb2Name))
                itemFB2 = DiskItemFabrick.GetDiskItem(pathFile) as DiskItemFileFB2;
            else
                itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathFile, fb2Name) as DiskItemFileFB2;

            IMHLBook? book = itemFB2;
            result = book != null;
            if (result)
            {
                lock (locker)
                {
                    using (DBModel dB = new DBModelSQLite(fileSQlite))
                    {
                        actual = dB.Keyword4Books.ToList().Count;
                        expected = Bizlogic4DB.Export_Keywords(dB, book?.Keywords, out List<Keyword4Book>? keyword4BooksDB);
                        CheckExpected(ref actual, ref expected, dB);
                        actual = dB.Keyword4Books.ToList().Count;
                    }
                }
            }
            Assert.AreEqual(expected, actual);
        }

        private static void CheckExpected(ref int actual, ref int expected, DBModel db)
        {
            if (expected > 0)
            {
                expected += actual;
                db.SaveChanges();
            }
            else if (expected < 1)
                expected = actual;
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2_K2.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2_K2.SQLite")]
        public void Export2SQLite_Keywords_Duplicated(string pathFile, string fb2Name, string fileSQlite)
        {
            DiskItemFileFB2? itemFB2;
            int actual = -1, expected = -2;
            bool result;

            if (string.IsNullOrEmpty(fb2Name))
                itemFB2 = DiskItemFabrick.GetDiskItem(pathFile) as DiskItemFileFB2;
            else
                itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathFile, fb2Name) as DiskItemFileFB2;

            IMHLBook? book = itemFB2;
            result = book != null;
            if (result)
            {
                lock (locker)
                {
                    using (DBModel dB = new DBModelSQLite(fileSQlite))
                    {

                        if (Bizlogic4DB.Export_Keywords(dB, book?.Keywords, out List<Keyword4Book>? keyword4BooksDB) > 0)
                        {
                            dB.SaveChanges();
                        }
                        expected = dB.Keyword4Books.ToList().Count;

                        if (Bizlogic4DB.Export_Keywords(dB, book?.Keywords, out keyword4BooksDB) > 0)
                        {
                            dB.SaveChanges();
                        }
                        actual = dB.Keyword4Books.ToList().Count;
                    }
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2_K3.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2_K3.SQLite")]
        public void Export2SQLite_Keywords_Out(string pathFile, string fb2Name, string fileSQlite)
        {
            DiskItemFileFB2? itemFB2;
            int actual, expected = 0;
            bool result;

            if (string.IsNullOrEmpty(fb2Name))
                itemFB2 = DiskItemFabrick.GetDiskItem(pathFile) as DiskItemFileFB2;
            else
                itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathFile, fb2Name) as DiskItemFileFB2;

            IMHLBook? book = itemFB2;
            List<Keyword4Book>? keyword4BooksDB = null;

            result = book != null;
            if (result)
            {
                lock (locker)
                {
                    using (DBModel dB = new DBModelSQLite(fileSQlite))
                    {
                        if (Bizlogic4DB.Export_Keywords(dB, book?.Keywords, out keyword4BooksDB) > 0)
                        {
                            expected = book?.Keywords?.Count ?? 0;
                            dB.SaveChanges();
                        }
                    }
                }
            }
            actual = keyword4BooksDB?.Count ?? 0;
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region [Sequences]
        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2", @"F:\1\test\destinationDB\ExportFB2_S1.SQLite")]
        [DataRow(@"F:\1\test\426096.fb2", "", @"F:\1\test\destinationDB\ExportFB2_S1.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2_S1.SQLite")]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2_S1.SQLite")]
        public void Export2SQLite_Sequences(string pathFile, string fb2Name, string fileSQlite)
        {
            DiskItemFileFB2? itemFB2;
            int actual = -1, expected = -2;
            bool result;

            if (string.IsNullOrEmpty(fb2Name))
                itemFB2 = DiskItemFabrick.GetDiskItem(pathFile) as DiskItemFileFB2;
            else
                itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathFile, fb2Name) as DiskItemFileFB2;

            IMHLBook? book = itemFB2;
            result = book != null;
            if (result)
            {
                lock (locker)
                {
                    using (DBModel dB = new DBModelSQLite(fileSQlite))
                    {
                        actual = dB.Sequence4Books.ToList().Count;
                        expected = Bizlogic4DB.Export_Sequences(dB, book?.SequenceAndNumber, out Sequence4Book? seq4Book);
                        CheckExpected(ref actual, ref expected, dB);
                        actual = dB.Sequence4Books.ToList().Count;
                    }
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2_S2.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2_S2.SQLite")]
        public void Export2SQLite_Sequences_Duplicated(string pathFile, string fb2Name, string fileSQlite)
        {
            DiskItemFileFB2? itemFB2;
            int actual = -1, expected = -2;
            bool result;

            if (string.IsNullOrEmpty(fb2Name))
                itemFB2 = DiskItemFabrick.GetDiskItem(pathFile) as DiskItemFileFB2;
            else
                itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathFile, fb2Name) as DiskItemFileFB2;

            IMHLBook? book = itemFB2;
            result = book != null;
            if (result)
            {
                lock (locker)
                {
                    using (DBModel dB = new DBModelSQLite(fileSQlite))
                    {
                        if (Bizlogic4DB.Export_Sequences(dB, book?.SequenceAndNumber, out Sequence4Book? seq4Book) > 0)
                        {
                            lock (locker)
                                dB.SaveChanges();
                        }
                        expected = dB.Sequence4Books.ToList().Count;

                        if (Bizlogic4DB.Export_Sequences(dB, book?.SequenceAndNumber, out seq4Book) > 0)
                        {
                            lock (locker)
                                dB.SaveChanges();
                        }
                        actual = dB.Sequence4Books.ToList().Count;
                    }
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(new string?[] { "Test1", "Test2", "Test3", "ЗЕМЛЯ ФАРАОНОВ" },
            new string?[] { "test2", "TEST3", "Test4", "земля фараонов" },
            @"F:\1\test\destinationDB\ExportFB2_S3.SQLite")]
        public void Export2SQLite_Export_Sequences(string?[] sequence1, string?[] sequence2, string pathDestination)
        {
            int actual = -1, expected = -2;

            if (File.Exists(pathDestination))
                File.Delete(pathDestination);

            lock (locker)
            {
                using DBModel dB = new DBModelSQLite(pathDestination);
               
                expected = Bizlogic4DB.Export_Sequences(dB, sequence1.ToList(), out List<Sequence4Book>? list1);
                dB.SaveChanges();

                /*foreach (var s in list1)
                    System.Diagnostics.Debug.WriteLine(s.Name);
                System.Diagnostics.Debug.WriteLine("======================= =================");*/

                expected += Bizlogic4DB.Export_Sequences(dB, sequence2.ToList(), out List<Sequence4Book>? list2);
                dB.SaveChanges();
                /*foreach (var s in list2)
                    System.Diagnostics.Debug.WriteLine(s.Name);*/
                actual = dB.Sequence4Books.ToList().Count;
            }

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region [Volumes]
        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2", @"F:\1\test\destinationDB\ExportFB2_V1.SQLite")]
        [DataRow(@"F:\1\test\426096.fb2", "", @"F:\1\test\destinationDB\ExportFB2_V1.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2_V1.SQLite")]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2_V1.SQLite")]
        public void Export2SQLite_Volumes(string pathFile, string fb2Name, string fileSQlite)
        {
            DiskItemFileFB2? itemFB2;
            int actual = -1, expected = -2;
            bool result;

            if (string.IsNullOrEmpty(fb2Name))
                itemFB2 = DiskItemFabrick.GetDiskItem(pathFile) as DiskItemFileFB2;
            else
                itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathFile, fb2Name) as DiskItemFileFB2;

            IMHLBook? book = itemFB2;
            result = book != null;
            if (result)
            {
                lock (locker)
                {
                    using (DBModel dB = new DBModelSQLite(fileSQlite))
                    {
                        actual = dB.Volumes.ToList().Count;
                        expected = Bizlogic4DB.Export_Volumes(dB, book?.SequenceAndNumber, out Volume? volume);
                        CheckExpected(ref actual, ref expected, dB);
                        actual = dB.Volumes.ToList().Count;
                    }
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2_V2.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2_V2.SQLite")]
        public void Export2SQLite_Volumes_Duplicated(string pathFile, string fb2Name, string fileSQlite)
        {
            DiskItemFileFB2? itemFB2;
            int actual = -1, expected = -2;
            bool result;

            if (string.IsNullOrEmpty(fb2Name))
                itemFB2 = DiskItemFabrick.GetDiskItem(pathFile) as DiskItemFileFB2;
            else
                itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathFile, fb2Name) as DiskItemFileFB2;

            IMHLBook? book = itemFB2;
            result = book != null;
            if (result)
            {
                lock (locker)
                {
                    using (DBModel dB = new DBModelSQLite(fileSQlite))
                    {

                        if (Bizlogic4DB.Export_Volumes(dB, book?.SequenceAndNumber, out Volume? volume) > 0)
                        {
                            lock (locker)
                                dB.SaveChanges();
                        }
                        expected = dB.Volumes.ToList().Count;

                        if (Bizlogic4DB.Export_Volumes(dB, book?.SequenceAndNumber, out volume) > 0)
                        {
                            lock (locker)
                                dB.SaveChanges();
                        }
                        actual = dB.Volumes.ToList().Count;
                    }
                }
            }
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip",
            new string[] { "426026.fb2", "426026.fb2", "426026.fb2", "426026.fb2" },
            @"F:\1\test\destinationDB\ExportFB2_V3_1.SQLite",
            false)]

        [DataRow(@"F:\1\test\fb2-426000-433000.zip",
            new string[] { "426026.fb2", "426026.fb2", "426026.fb2", "426026.fb2" },
            @"F:\1\test\destinationDB\ExportFB2_V3_2.SQLite",
            true)]
        public void Export2SQLite_Export_Volumes4BookList(string zipFile, string[] fb2Name, string fileSQlite, bool changeNum)
        {
            int actual = -1, expected = -2;

            if (File.Exists(fileSQlite))
                File.Delete(fileSQlite);

            List<IMHLBook> books = new List<IMHLBook>();
            ushort i = 0;
            foreach (var entity in fb2Name)
            {
                if (MHLSourceOnDiskStatic.GetBookFromZip(zipFile, entity) is IMHLBook book)
                {
                    book.SequenceAndNumber.Number = (ushort)(book.SequenceAndNumber.Number + i);
                    i++;
                    books.Add(book);
                }
            }

            lock (locker)
            {
                using (DBModel dB = new DBModelSQLite(fileSQlite))
                {
                    expected = Bizlogic4DB.Export_Volumes4BookList(dB, books, out List<Volume>? list);
                    dB.SaveChanges();
                    actual = list?.Count ?? 0;

                    if (changeNum)
                        foreach (var b in books)
                        {
                            b.SequenceAndNumber.Number = (ushort)(b.SequenceAndNumber.Number + i);
                            i++;
                        }

                    expected += Bizlogic4DB.Export_Volumes4BookList(dB, books, out list);
                    dB.SaveChanges();
                    if (changeNum)
                        actual += list?.Count ?? 0;
                }
            }
            System.Diagnostics.Debug.WriteLine("actual : {0}, expected : {1}", actual, expected);
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region [Authors]
        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2", @"F:\1\test\destinationDB\ExportFB2_A1.SQLite")]
        [DataRow(@"F:\1\test\426096.fb2", "", @"F:\1\test\destinationDB\ExportFB2_A1.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2_A1.SQLite")]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2_A1.SQLite")]
        public void Export2SQLite_Authors(string pathFile, string fb2Name, string fileSQlite)
        {
            DiskItemFileFB2? itemFB2;
            int actual = -1, expected = -2;
            bool result;

            if (string.IsNullOrEmpty(fb2Name))
                itemFB2 = DiskItemFabrick.GetDiskItem(pathFile) as DiskItemFileFB2;
            else
                itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathFile, fb2Name) as DiskItemFileFB2;

            IMHLBook? book = itemFB2;
            result = book != null;
            if (result)
            {
                lock (locker)
                {
                    using (DBModel dB = new DBModelSQLite(fileSQlite))
                    {
                        actual = dB.Authors.ToList().Count;
                        expected = Bizlogic4DB.Export_Authors(dB, book?.Authors, out List<Author>? authors);
                        CheckExpected(ref actual, ref expected, dB);
                        actual = dB.Authors.ToList().Count;
                    }
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2_A2.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2_A2.SQLite")]
        public void Export2SQLite_Authors_Duplicated(string pathFile, string fb2Name, string fileSQlite)
        {
            DiskItemFileFB2? itemFB2;
            int actual = -1, expected = -2;
            bool result;

            if (string.IsNullOrEmpty(fb2Name))
                itemFB2 = DiskItemFabrick.GetDiskItem(pathFile) as DiskItemFileFB2;
            else
                itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathFile, fb2Name) as DiskItemFileFB2;

            IMHLBook? book = itemFB2;
            result = book != null;
            if (result)
            {
                lock (locker)
                {
                    using (DBModel dB = new DBModelSQLite(fileSQlite))
                    {

                        if (Bizlogic4DB.Export_Authors(dB, book?.Authors, out List<Author>? authors) > 0)
                        {
                            lock (locker)
                                dB.SaveChanges();
                        }
                        expected = dB.Authors.ToList().Count;

                        if (Bizlogic4DB.Export_Authors(dB, book?.Authors, out authors) > 0)
                        {
                            lock (locker)
                                dB.SaveChanges();
                        }
                        actual = dB.Authors.ToList().Count;
                    }
                }
            }
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2_A3.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2_A3.SQLite")]
        public void Export2SQLite_Authors_Out(string pathFile, string fb2Name, string fileSQlite)
        {
            DiskItemFileFB2? itemFB2;
            int actual, expected;
            bool result;

            if (string.IsNullOrEmpty(fb2Name))
                itemFB2 = DiskItemFabrick.GetDiskItem(pathFile) as DiskItemFileFB2;
            else
                itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathFile, fb2Name) as DiskItemFileFB2;

            IMHLBook? book = itemFB2;
            List<Author>? authors = null;

            result = book != null;
            if (result)
            {
                lock (locker)
                {
                    using (DBModel dB = new DBModelSQLite(fileSQlite))
                    {
                        if (Bizlogic4DB.Export_Authors(dB, book?.Authors, out authors) > 0)
                        {
                            dB.SaveChanges();
                        }
                    }
                }
            }

            actual = book?.Authors?.Count ?? 0;
            expected = authors?.Count ?? 0;

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region [Publisher]
        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2", @"F:\1\test\destinationDB\ExportFB2_P1.SQLite")]
        [DataRow(@"F:\1\test\426096.fb2", "", @"F:\1\test\destinationDB\ExportFB2_P1.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2_P1.SQLite")]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2_P1.SQLite")]
        public void Export2SQLite_Publisher(string pathFile, string fb2Name, string fileSQlite)
        {
            DiskItemFileFB2? itemFB2;
            int actual = -1, expected = -2;
            bool result;

            if (string.IsNullOrEmpty(fb2Name))
                itemFB2 = DiskItemFabrick.GetDiskItem(pathFile) as DiskItemFileFB2;
            else
                itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathFile, fb2Name) as DiskItemFileFB2;

            IMHLBook? book = itemFB2;
            result = book != null;
            if (result)
            {
                lock (locker)
                {
                    using (DBModel dB = new DBModelSQLite(fileSQlite))
                    {
                        actual = dB.Publishers.ToList().Count;
                        expected = Bizlogic4DB.Export_Publishers(dB, book?.Publisher, out Publisher? publisher);
                        CheckExpected(ref actual, ref expected, dB);
                        actual = dB.Publishers.ToList().Count;
                    }
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2_P2.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2_P2.SQLite")]
        public void Export2SQLite_Publisher_Duplicated(string pathFile, string fb2Name, string fileSQlite)
        {
            DiskItemFileFB2? itemFB2;
            int actual = -1, expected = -2;
            bool result;

            if (string.IsNullOrEmpty(fb2Name))
                itemFB2 = DiskItemFabrick.GetDiskItem(pathFile) as DiskItemFileFB2;
            else
                itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathFile, fb2Name) as DiskItemFileFB2;

            IMHLBook? book = itemFB2;
            result = book != null;
            if (result)
            {
                lock (locker)
                {
                    using (DBModel dB = new DBModelSQLite(fileSQlite))
                    {
                        if (Bizlogic4DB.Export_Publishers(dB, book?.Publisher, out Publisher? publisher) > 0)
                        {
                            lock (locker)
                                dB.SaveChanges();
                        }
                        expected = dB.Publishers.ToList().Count;

                        if (Bizlogic4DB.Export_Publishers(dB, book?.Publisher, out publisher) > 0)
                        {
                            lock (locker)
                                dB.SaveChanges();
                        }
                        actual = dB.Publishers.ToList().Count;
                    }
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip",
            new string[] { "426026.fb2", "426026.fb2", "426026.fb2", "426026.fb2" },
            @"F:\1\test\destinationDB\ExportFB2_P3_1.SQLite",
            false)]

        [DataRow(@"F:\1\test\fb2-426000-433000.zip",
            new string[] { "426026.fb2", "426026.fb2", "426026.fb2", "426026.fb2" },
            @"F:\1\test\destinationDB\ExportFB2_P3_2.SQLite",
            true)]
        public void Export2SQLite_Export_Publishers4BookList(string zipFile, string[] fb2Name, string fileSQlite, bool changeNum)
        {
            int actual = -1, expected = -2;

            if (File.Exists(fileSQlite))
                File.Delete(fileSQlite);

            List<IMHLBook> books = new List<IMHLBook>();
            ushort i = 0;
            foreach (var entity in fb2Name)
            {
                if (MHLSourceOnDiskStatic.GetBookFromZip(zipFile, entity) is IMHLBook book)
                {
                    if (book.Publisher != null)
                    {
                        book.Publisher.Name += i;
                        i++;

                        System.Diagnostics.Debug.WriteLine("{0} : {1}",
                            book.Publisher.Name,
                            book.Publisher.City);
                    }
                    books.Add(book);
                }
            }

            lock (locker)
            {
                using (DBModel dB = new DBModelSQLite(fileSQlite))
                {
                    expected = Bizlogic4DB.Export_Publishers4BookList(dB, books, out List<Publisher>? list);
                    dB.SaveChanges();
                    actual = list?  .Count ?? 0;


                    foreach (var book in books)
                        if (book.Publisher != null)
                        {
                            book.Publisher.Name = book.Publisher.Name.ToUpper();
                            if (changeNum)
                            {
                                book.Publisher.Name += i;
                                i++;
                            }
                            System.Diagnostics.Debug.WriteLine("{0} : {1}",
                            book.Publisher.Name,
                            book.Publisher.City);
                        }

                    expected += Bizlogic4DB.Export_Publishers4BookList(dB, books, out list);
                    dB.SaveChanges();
                    if (changeNum)
                        actual += list?.Count ?? 0;
                }
            }
            System.Diagnostics.Debug.WriteLine("actual : {0}, expected : {1}", actual, expected);
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region [Books]
        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2", @"F:\1\test\destinationDB\ExportFB2_B1.SQLite")]
        [DataRow(@"F:\1\test\426096.fb2", "", @"F:\1\test\destinationDB\ExportFB2_B1.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2_B1.SQLite")]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2_B1.SQLite")]
        public void Export2SQLite_Book(string pathFile, string fb2Name, string fileSQlite)
        {
            DiskItemFileFB2? itemFB2;
            int actual = -1, expected = -2;

            if (string.IsNullOrEmpty(fb2Name))
                itemFB2 = DiskItemFabrick.GetDiskItem(pathFile) as DiskItemFileFB2;
            else
                itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathFile, fb2Name) as DiskItemFileFB2;

            lock (locker)
            {
                using (DBModel dB = new DBModelSQLite(fileSQlite))
                {
                    actual = dB.Books.ToList().Count;
                    expected = Bizlogic4DB.Export_FB2(dB, itemFB2);
                    CheckExpected(ref actual, ref expected, dB);
                    actual = dB.Books.ToList().Count;
                }
            }

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2_B2.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2_B2.SQLite")]
        public void Export2SQLite_Books_Duplicated(string pathFile, string fb2Name, string fileSQlite)
        {
            DiskItemFileFB2? itemFB2;
            int actual = -1, expected = -2;

            if (string.IsNullOrEmpty(fb2Name))
                itemFB2 = DiskItemFabrick.GetDiskItem(pathFile) as DiskItemFileFB2;
            else
                itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathFile, fb2Name) as DiskItemFileFB2;

            lock (locker)
            {
                Bizlogic4DB.Export_FB2(fileSQlite, itemFB2);
                using (DBModel dB = new DBModelSQLite(fileSQlite))
                {
                    expected = dB.Books.ToList().Count;
                }

                Bizlogic4DB.Export_FB2(fileSQlite, itemFB2);
                using (DBModel dB = new DBModelSQLite(fileSQlite))
                {
                    actual = dB.Books.ToList().Count;
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip",
          new string[] { "426026.fb2", "426026.fb2", "426026.fb2", "426026.fb2" },
          @"F:\1\test\destinationDB\ExportFB2_B3.SQLite")]
        public void Export2SQLite_Export_FB2List(string zipFile, string[] fb2Name, string fileSQlite)
        {
            int actual = -1, expected = -2;

            if (File.Exists(fileSQlite))
                File.Delete(fileSQlite);

            List<IDiskItem> books = new List<IDiskItem>();
            foreach (var entity in fb2Name)
            {
                IDiskItem? book = MHLSourceOnDiskStatic.GetItemFromZip(zipFile, entity);
                if (book != null)
                {
                    books.Add(book);
                }
            }

            lock (locker)
            {
                using (DBModel dB = new DBModelSQLite(fileSQlite))
                {
                    expected = Bizlogic4DB.Export_FB2List(dB, books);
                    dB.SaveChanges();

                    expected += Bizlogic4DB.Export_FB2List(dB, books);
                    dB.SaveChanges();

                    actual = dB.Books.Count();
                }
            }
            System.Diagnostics.Debug.WriteLine("actual : {0}, expected : {1}", actual, expected);
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
