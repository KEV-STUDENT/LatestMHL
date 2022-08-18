using System.Xml;

namespace MHLCommon.MHLBook
{
    public class MHLKeyword : MHLBookAttribute<string>

    {
        #region [Fields]
        private string _keyword = string.Empty;
        #endregion

        #region [Properties]
        public string Keyword { get => _keyword; set => _keyword = value; }
        #endregion

        #region [Constructor]
        public MHLKeyword()
        {
        }
        #endregion

        protected override void LoadInformationFromXML()
        {
            Keyword = Node ?? string.Empty;
        }
   }
}