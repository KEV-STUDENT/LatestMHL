using MHLCommon.DataModels;
using MHLCommon.MHLBook;
using MHLSourceOnDisk;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MHLSourceOnDiskTest
{
    [TestClass]
    public class IBookTest
    {
        [TestMethod]
        [DataRow(@"F:\1\test\426096.fb2")]
        public void Authors(string pathFile) 
        {
            MHLCommon.MHLBook.IMHLBook book = new DiskItemFileFB2(pathFile);
            Assert.AreNotEqual(0, book.Authors.Count);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\426096.fb2")]
        public void Author_IAuthor(string pathFile)
        {
            MHLCommon.MHLBook.IMHLBook book = new DiskItemFileFB2(pathFile);
            Assert.IsInstanceOfType(book.Authors[0], typeof(IAuthor));
        }

        [TestMethod]
        [DataRow(@"F:\1\test\426096.fb2")]
        public void Genres(string pathFile)
        {
            MHLCommon.MHLBook.IMHLBook book = new DiskItemFileFB2(pathFile);
            Assert.AreNotEqual(0, book.Genres.Count);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\426096.fb2")]
        public void Author_IGenre(string pathFile)
        {
            MHLCommon.MHLBook.IMHLBook book = new DiskItemFileFB2(pathFile);
            Assert.IsInstanceOfType(book.Genres[0], typeof(IGenre));
        }


        [TestMethod]
        [DataRow(@"F:\1\test\426096.fb2")]
        public void Keywords(string pathFile)
        {
            MHLCommon.MHLBook.IMHLBook book = new DiskItemFileFB2(pathFile);
            foreach(var key in book.Keywords)
            {
                System.Diagnostics.Debug.WriteLine(key.Node);
            }
            Assert.AreNotEqual(0, book.Keywords.Count);
        }

        [TestMethod]
        [DataRow(@"F:\1\test\426096.fb2")]
        public void Author_IKeword(string pathFile)
        {
            MHLCommon.MHLBook.IMHLBook book = new DiskItemFileFB2(pathFile);
            Assert.IsInstanceOfType(book.Keywords[0], typeof(IKeyword));
        }

        [TestMethod]
        [DataRow(@"F:\1\test\426096.fb2")]
        public void Annotation(string pathFile)
        {
            MHLCommon.MHLBook.IMHLBook book = new DiskItemFileFB2(pathFile);
            
            System.Diagnostics.Debug.WriteLine(book.Annotation);

            Assert.AreNotEqual(string.Empty, book.Annotation);
        }
    }
}
