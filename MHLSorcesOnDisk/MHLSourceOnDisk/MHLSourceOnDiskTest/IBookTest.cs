using MHLCommon.MHLBook;
using MHLSourceOnDisk;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MHLSourceOnDiskTest
{
    [TestClass]
    public class IBookTest
    {
        protected string pathFile = @"F:\1\test\426096.fb2";
        protected string pathZip = @"F:\1\test\fb2-495000-500999.zip";

        [TestMethod]
        [DataRow(@"F:\1\test\426096.fb2")]
        public void Authors(string pathFile) 
        {
            IBook book = new DiskItemFileFB2(pathFile);
            Assert.AreNotEqual(0, book.Authors.Count);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\426096.fb2")]
        public void Genres(string pathFile)
        {
            IBook book = new DiskItemFileFB2(pathFile);
            Assert.AreNotEqual(0, book.Genres.Count);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\426096.fb2")]
        public void Keywords(string pathFile)
        {
            IBook book = new DiskItemFileFB2(pathFile);
            foreach(var key in book.Keywords)
            {
                System.Diagnostics.Debug.WriteLine(key.Node);
            }
            Assert.AreNotEqual(0, book.Keywords.Count);
        }


        [TestMethod]
        [DataRow(@"F:\1\test\426096.fb2")]
        public void Annotation(string pathFile)
        {
            IBook book = new DiskItemFileFB2(pathFile);
            
            System.Diagnostics.Debug.WriteLine(book.Annotation);

            Assert.AreNotEqual(string.Empty, book.Annotation);
        }
    }
}
