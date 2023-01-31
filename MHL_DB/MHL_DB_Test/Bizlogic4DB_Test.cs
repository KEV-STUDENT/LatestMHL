using MHL_DB_Model;
using MHL_DB_SQLite;
using MHLCommon.MHLBook;
using MHLSourceOnDisk;

namespace MHL_DB_Test
{
    [TestClass]
    public class Bizlogic4DB_Test
    {
        private object locker = new object();

        #region [Genres]
        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\426096.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
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
                        List<Genre>? genresDB;
                        actual = dB.Genres.ToList().Count;
                        expected = Bizlogic4DB.Export_Genres(dB, book?.Genres, out genresDB);
                        CheckExpected(ref actual, ref expected, dB);
                        actual = dB.Genres.ToList().Count;
                    }
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
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
                        List<Genre>? genresDB;
                        if (Bizlogic4DB.Export_Genres(dB, book?.Genres, out genresDB) > 0)
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
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
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
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\426096.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
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
                        List<Keyword4Book>? keyword4BooksDB;
                        actual = dB.Keyword4Books.ToList().Count;
                        expected = Bizlogic4DB.Export_Keywords(dB, book?.Keywords, out keyword4BooksDB);
                        CheckExpected(ref actual, ref expected, dB);
                        actual = dB.Keyword4Books.ToList().Count;
                    }
                }
            }
            Assert.AreEqual(expected, actual);
        }

        private void CheckExpected(ref int actual, ref int expected, DBModel db)
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
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
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
                        List<Keyword4Book>? keyword4BooksDB;

                        if (Bizlogic4DB.Export_Keywords(dB, book?.Keywords, out keyword4BooksDB) > 0)
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
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        public void Export2SQLite_Keywords_Out(string pathFile, string fb2Name, string fileSQlite)
        {
            DiskItemFileFB2? itemFB2;
            int actual, expected;
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
                        Bizlogic4DB.Export_Keywords(dB, book?.Keywords, out keyword4BooksDB);
                    }
                }
            }
            expected = book?.Keywords?.Count ?? 0;
            actual = keyword4BooksDB?.Count ?? 0;

            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region [Sequences]
        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\426096.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
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
                        Sequence4Book? seq4Book;
                        actual = dB.Sequence4Books.ToList().Count;
                        expected = Bizlogic4DB.Export_Sequences(dB, book?.SequenceAndNumber, out seq4Book);
                        CheckExpected(ref actual, ref expected, dB);
                        actual = dB.Sequence4Books.ToList().Count;
                    }
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
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
                        Sequence4Book? seq4Book;
                        if (Bizlogic4DB.Export_Sequences(dB, book?.SequenceAndNumber, out seq4Book) > 0)
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
        #endregion

        #region [Volumes]
        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\426096.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
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
                        Volume? volume;
                        actual = dB.Volumes.ToList().Count;
                        expected = Bizlogic4DB.Export_Volumes(dB, book?.SequenceAndNumber, out volume);
                        CheckExpected(ref actual, ref expected, dB);
                        actual = dB.Volumes.ToList().Count;
                    }
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
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
                        Volume? volume;

                        if (Bizlogic4DB.Export_Volumes(dB, book?.SequenceAndNumber, out volume) > 0)
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
        #endregion

        #region [Authors]
        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\426096.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
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
                        List<Author>? authors;
                        actual = dB.Authors.ToList().Count;
                        expected = Bizlogic4DB.Export_Authors(dB, book?.Authors, out authors);
                        CheckExpected(ref actual, ref expected, dB);
                        actual = dB.Authors.ToList().Count;
                    }
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
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
                        List<Author>? authors;

                        if (Bizlogic4DB.Export_Authors(dB, book?.Authors, out authors) > 0)
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
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
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

        #region [Books]
        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\426096.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
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
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2.SQLite")]
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
        #endregion
    }
}
