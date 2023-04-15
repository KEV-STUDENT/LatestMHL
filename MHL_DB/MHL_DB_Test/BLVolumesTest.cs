using MHL_DB_BizLogic.BLClasses;
using MHL_DB_Model;
using MHLCommon;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using MHLSourceOnDisk;

namespace MHL_DB_Test
{
    [TestClass]
    public class BLVolumesTest
    {
        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2", @"F:\1\test\destinationDB\ExportFB2_V1.SQLite")]
        [DataRow(@"F:\1\test\426096.fb2", "", @"F:\1\test\destinationDB\ExportFB2_V1.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2_V1.SQLite")]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2_V1.SQLite")]
        public void Export2SQLite(string pathFile, string fb2Name, string fileSQlite)
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
                expected = book?.SequenceAndNumber == null ? 0 : 1;

                using (DBModel dB = new DBModelSQLite(fileSQlite))
                {
                    BLVolumes bl = new BLVolumes(dB);
                    List<Volume>? ta = bl.GetNewEntities4DiskItem(new List<IDiskItem>(1) { itemFB2 });

                    actual = ta?.Count ?? 0;
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2_V2.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2_V2.SQLite")]
        public void Export2SQLite_DuplicatedBooks(string pathFile, string fb2Name, string fileSQlite)
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
                expected = book?.SequenceAndNumber == null ? 0 : 1;

                using (DBModel dB = new DBModelSQLite(fileSQlite))
                {
                    BLVolumes bl = new BLVolumes(dB);
                    List<Volume>? ta = bl.GetNewEntities4DiskItem(new List<IDiskItem>(2) { itemFB2, itemFB2 });
                    foreach(var v in ta)
                    {
                        System.Diagnostics.Debug.WriteLine("{0} : {1}", v.Sequence.Name, v.Number);
                    }
                    actual = ta?.Count ?? 0;
                }
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-495000-500999.zip", "495000.fb2", @"F:\1\test\destinationDB\ExportFB2_V1.SQLite")]
        [DataRow(@"F:\1\test\426096.fb2", "", @"F:\1\test\destinationDB\ExportFB2_V1.SQLite")]
        [DataRow(@"F:\1\test\Davydov_Moskovit.454563.fb2", "", @"F:\1\test\destinationDB\ExportFB2_V1.SQLite")]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip", "426026.fb2", @"F:\1\test\destinationDB\ExportFB2_V1.SQLite")]
        public void Export2SQLite_DuplicatedWithDB(string pathFile, string fb2Name, string fileSQlite)
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
                expected = 0;

                using (DBModel dB = new DBModelSQLite(fileSQlite))
                {
                    IBLEntity<List<SequenceVolume>, List<Volume>> bl = new BLVolumes(dB);
                    List<Volume>? ta = bl.GetNewEntities4DiskItem(new List<IDiskItem>(1) { itemFB2 });

                    if ((ta?.Count ?? 0) > 0 && bl.AddInDB(ta))
                        dB.SaveChanges();

                    ta = bl.GetNewEntities4DiskItem(new List<IDiskItem>(1) { itemFB2 });
                    actual = ta?.Count ?? 0;
                }
            }
            Assert.AreEqual(expected, actual);
        }
    }
}