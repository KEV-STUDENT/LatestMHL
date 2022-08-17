using MHLCommon.MHLBook;
using MHLSourceOnDisk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHLSourceOnDiskTest
{
    [TestClass]
    public class IBookTest
    {
        protected string pathFile = @"F:\1\test\426096.fb2";
        protected string pathZip = @"F:\1\test\fb2-495000-500999.zip";

        [TestMethod]
        public void LoadAuthors_pathDirZip()
        {
            IBook book = new DiskItemFileFB2(pathFile);
            List<IBookAttribute> authors = book.LoadAuthors();

            Assert.IsNotNull(authors);
        }
    }
}
