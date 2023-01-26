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
			Assert.IsInstanceOfType(book, typeof(IBook<Author, Genre4Book, Keyword4Book>));
		}

		[TestMethod]
		public void MHL_DM_Model_Genre4Book_IGenre()
		{
			IGenreDB g = new Genre4Book();
			Assert.IsInstanceOfType(g, typeof(IGenre));
		}
	}
}