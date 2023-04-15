﻿using MHL_DB_BizLogic.BLClasses;
using MHL_DB_Model;
using MHLCommon.DataModels;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using MHLSourceOnDisk;


namespace MHL_DB_Test
{
    [TestClass]
    public class BLAuthorsTest
    {
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
                expected = book?.Authors?.Count ?? 0;

                using (DBModel dB = new DBModelSQLite(fileSQlite))
                {
                    BLAuthors blA = new BLAuthors(dB);
                    List<Author>? ta = blA.GetNewEntities4DiskItem(new List<IDiskItem>(1) { itemFB2 });

                    actual = ta?.Count ?? 0;
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2_A2.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2_A2.SQLite")]
        public void Export2SQLite_Authors_DuplicatedBooks(string pathFile, string fb2Name, string fileSQlite)
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
                expected = book?.Authors?.Count ?? 0;

                using (DBModel dB = new DBModelSQLite(fileSQlite))
                {
                    BLAuthors blA = new BLAuthors(dB);
                    List<Author>? ta = blA.GetNewEntities4DiskItem(new List<IDiskItem>(2) { itemFB2, itemFB2 });

                    actual = ta?.Count ?? 0;
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2", @"F:\1\test\destinationDB\ExportFB2_A1.SQLite")]
        [DataRow(@"F:\1\test\426096.fb2", "", @"F:\1\test\destinationDB\ExportFB2_A1.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2_A1.SQLite")]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2_A1.SQLite")]
        public void Export2SQLite_Authors_DuplicatedWithDB(string pathFile, string fb2Name, string fileSQlite)
        {
            DiskItemFileFB2? itemFB2;
            int actual = -1, expected = -2;
            bool result = false;

            if (string.IsNullOrEmpty(fb2Name))
                itemFB2 = DiskItemFabrick.GetDiskItem(pathFile) as DiskItemFileFB2;
            else
                itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathFile, fb2Name) as DiskItemFileFB2;

            if (itemFB2 != null)
            {
                IMHLBook? book = itemFB2;
                result = book != null;
            }

            if (result)
            {
                expected = 0;

                using (DBModel dB = new DBModelSQLite(fileSQlite))
                {
                    IBLEntity<List<MHLAuthor>,List<Author>> bl = new BLAuthors(dB);
                    List<Author>? ta = bl.GetNewEntities4DiskItem(new List<IDiskItem>(1) { itemFB2 });

                    if(bl.AddInDB(ta))                    
                        dB.SaveChanges();

                    ta = bl.GetNewEntities4DiskItem(new List<IDiskItem>(1) { itemFB2 });
                    actual = ta?.Count ?? 0;
                }
            }
            Assert.AreEqual(expected, actual);
        }
    }
}
