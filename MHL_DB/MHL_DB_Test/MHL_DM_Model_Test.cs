using MHL_DB_Model;
using MHLCommon.DataModels;

namespace MHL_DB_Test
{
    [TestClass]
	public class MHL_DM_Model_Test
	{
		[TestMethod]
		public void MHL_DM_Model_Author_IAuthor()
		{
			IAuthorDB author = new Author();
			Assert.IsInstanceOfType(author, typeof(IAuthor));
		}

		[TestMethod]
		public void MHL_DM_Model_Book_IBook()
		{
			IBookDB book = new Book();
			Assert.IsInstanceOfType(book, typeof(IBook<Author, Genre, Keyword4Book>));
		}

		[TestMethod]
		public void MHL_DM_Model_Genre4Book_IGenre()
		{
			IGenreDB g = new Genre();
			Assert.IsInstanceOfType(g, typeof(IGenre));
		}

        [TestMethod]
        public void MHL_DM_Model_Keyword4Book_IKeyword()
        {
            IKeywordDB g = new Keyword4Book();
            Assert.IsInstanceOfType(g, typeof(IKeyword));
        }

        [TestMethod]
        public void MHL_DM_Model_Sequence_ISequence()
        {
            ISequenceDB g = new Sequence4Book();
            Assert.IsInstanceOfType(g, typeof(ICommonSequence));
        }

        [TestMethod]
        public void MHL_DM_Model_Volume_IVolume()
        {
            IVolumeDB g = new Volume();
            Assert.IsInstanceOfType(g, typeof(IVolume));
        }

    }
}