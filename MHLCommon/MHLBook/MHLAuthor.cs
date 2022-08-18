using System.Xml;

namespace MHLCommon.MHLBook
{
    public class MHLAuthor : MHLBookAttribute<XmlNode>
    {
        #region [Fields]
        private string _lastname = string.Empty;
        private string _firstname = string.Empty;
        private string _middleame = string.Empty;
        #endregion

        #region [Constructor]
        public MHLAuthor()
        {
        }
        #endregion

        #region [Properties]
        public string FirstName { get => _firstname; set => _firstname = value; }
        public string MiddleName { get => _middleame; set => _middleame = value; }
        public string LastName { get => _lastname; set => _lastname = value; }
        #endregion

        #region [Protected Methods]
        protected override void LoadInformationFromXML()
        {
            if (Node?.HasChildNodes ?? false)
            {
                foreach (XmlNode n in Node.ChildNodes)
                {
                    switch (n.LocalName)
                    {
                        case "first-name":
                            FirstName = n.InnerText;
                            break;
                        case "middle-name":
                            MiddleName = n.InnerText;
                            break;
                        case "last-name":
                            LastName = n.InnerText;
                            break;
                    }
                }
            }
            else
            {
                LastName = string.Empty;
                FirstName = string.Empty;
                MiddleName = string.Empty;
            }
        }
        #endregion
    }
}