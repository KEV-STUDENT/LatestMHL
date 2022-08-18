using System.Xml;
namespace MHLCommon.MHLBook
{
    public class MHLGenre : MHLBookAttribute<XmlNode>

    {
        #region [Fields]
        private FB2Genres _genre = FB2Genres.none;
        #endregion

        #region [Properties]
        public FB2Genres Genre { get => _genre; set => _genre = value; }
        #endregion

        #region [Constructor]
        public MHLGenre()
        {
        }
        #endregion

        protected override void LoadInformationFromXML()
        {
            FB2Genres genre;

            if (string.IsNullOrEmpty(Node?.InnerText) || !Enum.TryParse<FB2Genres>(Node.InnerText, true, out genre))
                genre = FB2Genres.none;

            Genre = genre;
        }
    }
}