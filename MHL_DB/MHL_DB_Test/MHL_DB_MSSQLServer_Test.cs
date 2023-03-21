using MHL_DB_BizLogic.SqlServer;
using MHL_DB_MSSqlServer;
using MHLCommon.ExpDestinations;
using MHLCommon.MHLDiskItems;
using MHLSourceOnDisk;

namespace MHL_DB_Test
{
    [TestClass]
    public class MHL_DB_MSSQLServer_Test
    {
        [TestMethod]
        [DataRow("DB4Test_1")]
        public void MHL_DB_MSSQLServer_CreateifNotExist(string dataBase)
        {
            bool res = false;
            ISQLServerSettings settings = new SQLServerSettings() { DataBase = dataBase };
            using (var dB = new DBModelMSSqlServer(settings))
            {
                res = dB.Database.EnsureDeleted();
            }
            Assert.IsTrue(res);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip",
             new string[] { "426020.fb2", "426021.fb2", "426022.fb2", "426023.fb2", "426024.fb2", "426025.fb2", "426026.fb2" },
            "DB4Test_2")]
        public void Export2SqlServer_BooksList(string zipFile, string[] fb2Name, string dataBase)
        {
            int actual = -1, expected = -2;

            List<IDiskItem> books = new List<IDiskItem>();
            ushort i = 0;
            foreach (var entity in fb2Name)
            {
                IDiskItem? book = MHLSourceOnDiskStatic.GetItemFromZip(zipFile, entity);
                if (book != null)
                {
                    books.Add(book);
                }
            }

            ISQLServerSettings settings = new SQLServerSettings() { DataBase = dataBase };
            using (var dB = new DBModelMSSqlServer(settings))
            {
                dB.Database.EnsureDeleted();
                dB.Database.EnsureCreated();

                expected = books.Count;

                actual = Bizlogic4SqlServer.Export_FB2List(dB, books);
            }

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\fb2-426000-433000.zip",
            new string[] { "426020.fb2", "426021.fb2", "426022.fb2", "426023.fb2", "426024.fb2", "426025.fb2", "426026.fb2" },
            "DB4Test_3")]
        public void Export2SqlServer_BooksList_Async(string zipFile, string[] fb2Name, string dataBase)
        {
            int actual = -1, expected = -2;

            List<IDiskItem> books = new List<IDiskItem>();
            ushort i = 0;
            foreach (var entity in fb2Name)
            {
                IDiskItem? book = MHLSourceOnDiskStatic.GetItemFromZip(zipFile, entity);
                if (book != null)
                {
                    books.Add(book);
                }
            }

            ISQLServerSettings settings = new SQLServerSettings() { DataBase = dataBase };
            using (var dB = new DBModelMSSqlServer(settings))
            {
                dB.Database.EnsureDeleted();
                dB.Database.EnsureCreated();

                expected = books.Count;
                Task<int> t = Bizlogic4SqlServer.Export_FB2List_Async(dB, books);
                t.Wait();
                actual = t.Result;
            }

            Assert.AreEqual(expected, actual);
        }
    }
}
