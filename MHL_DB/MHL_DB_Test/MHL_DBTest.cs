using MHL_DB_SQLite;
using MHL_DB_Model;
using Microsoft.EntityFrameworkCore;

namespace MHL_DB_Test
{
    [TestClass]
    public class MHL_DBTest
    {
        [TestMethod]
        [DataRow(@"F:\1\test\DestinationDB\DB4Test.sqlite")]
        [DataRow(@"F:\1\test\DestinationDB\DB4Test1.sqlite")]
        public void DataModel_CreateifNotExist(string dBFile)
        {
            if (File.Exists(dBFile))
                File.Delete(dBFile);

            using (var dB = new DBModel(dBFile))
            {
                dB.Database.Migrate();
            }

            Assert.IsTrue(File.Exists(dBFile));
        }


        [TestMethod]
        [DataRow(@"F:\1\test\DestinationDB\DB4Test.sqlite")]
        public void DataModel_DBModel_Authors(string dBFile)
        {
            List<Author>? autors = null;
            using (var dB = new DBModel(dBFile))
            {
                autors = dB.Authors.ToList();
            }
            Assert.IsNotNull(autors);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\DestinationDB\DB4Test.sqlite")]
        public void DataModel_DBModel_Books(string dBFile)
        {
            List<Book>? books = null;
            using (var dB = new DBModel(dBFile))
            {
                books = dB.Books.ToList();
            }
            Assert.IsNotNull(books);
        }
    }
}