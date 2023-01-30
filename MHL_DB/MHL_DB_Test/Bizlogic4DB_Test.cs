using MHL_DB_Model;
using MHL_DB_SQLite;
using MHLCommon.MHLBook;
using MHLSourceOnDisk;

namespace MHL_DB_Test
{
    [TestClass]
    public class Bizlogic4DB_Test
    {
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
                using (DBModel dB = new DBModelSQLite(fileSQlite))
                {
                    expected = dB.Genres.ToList().Count + Bizlogic4DB.Export_Genres(dB, book.Genres);
                    dB.SaveChanges();
                    actual = dB.Genres.ToList().Count;
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
                using (DBModel dB = new DBModelSQLite(fileSQlite))
                {
                    Bizlogic4DB.Export_Genres(dB, book.Genres);
                    dB.SaveChanges();
                    expected = dB.Genres.ToList().Count;

                    Bizlogic4DB.Export_Genres(dB, book.Genres);
                    dB.SaveChanges();
                    actual = dB.Genres.ToList().Count;
                }
            }
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
                using (DBModel dB = new DBModelSQLite(fileSQlite))
                {
                    expected = dB.Keyword4Books.ToList().Count + Bizlogic4DB.Export_Keywords(dB, book.Keywords);
                    dB.SaveChanges();
                    actual = dB.Keyword4Books.ToList().Count;
                }
            }
            Assert.AreEqual(expected, actual);
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
                using (DBModel dB = new DBModelSQLite(fileSQlite))
                {
                    Bizlogic4DB.Export_Keywords(dB, book.Keywords);
                    dB.SaveChanges();
                    expected = dB.Keyword4Books.ToList().Count;

                    Bizlogic4DB.Export_Keywords(dB, book.Keywords);
                    dB.SaveChanges();
                    actual = dB.Keyword4Books.ToList().Count;
                }
            }
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
                using (DBModel dB = new DBModelSQLite(fileSQlite))
                {
                    int i = dB.Sequence4Books.ToList().Count;
                    Sequence4Book? seq4Book;
                    expected = Bizlogic4DB.Export_Sequences(dB, book?.SequenceAndNumber, out seq4Book);
                    if(expected > -1)
                        expected += i;
                    else
                        expected = i;

                    dB.SaveChanges();

                    actual = dB.Sequence4Books.ToList().Count;
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
                using (DBModel dB = new DBModelSQLite(fileSQlite))
                {
                    Sequence4Book? seq4Book;
                    Bizlogic4DB.Export_Sequences(dB, book.SequenceAndNumber, out seq4Book);
                    dB.SaveChanges();
                    expected = dB.Sequence4Books.ToList().Count;

                    Bizlogic4DB.Export_Sequences(dB, book.SequenceAndNumber, out seq4Book);
                    dB.SaveChanges();
                    actual = dB.Sequence4Books.ToList().Count;
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
                using (DBModel dB = new DBModelSQLite(fileSQlite))
                {
                    int i = dB.Volumes.ToList().Count;
                    Volume? volume;

                    expected = Bizlogic4DB.Export_Volumes(dB, book?.SequenceAndNumber, out volume);
                    if (expected > -1)
                        expected += i;
                    else
                        expected = i;

                    dB.SaveChanges();

                    actual = dB.Volumes.ToList().Count;
                }
            }
            System.Diagnostics.Debug.WriteLine("{0}-{1}", expected, actual);
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
                using (DBModel dB = new DBModelSQLite(fileSQlite))
                {
                    Volume? volume;

                    Bizlogic4DB.Export_Volumes(dB, book.SequenceAndNumber, out volume);
                    dB.SaveChanges();
                    expected = dB.Volumes.ToList().Count;

                    Bizlogic4DB.Export_Volumes(dB, book.SequenceAndNumber, out volume);
                    dB.SaveChanges();
                    actual = dB.Volumes.ToList().Count;
                }
            }
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
