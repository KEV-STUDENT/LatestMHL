using MHLCommon.DataModels;
using System.Xml;

namespace MHLCommon.MHLBook
{

    public class MHLAuthor : MHLBookAttribute<XmlNode>, IAuthor
    {
        #region [Fields]
        private string? _lastname;
        private string? _firstname;
        private string? _middleame;
        #endregion

        #region [Constructor]
        public MHLAuthor()
        {
        }
        #endregion

        #region [Properties]
        public string? FirstName { get => _firstname ?? String.Empty; set => _firstname = value; }
        public string? MiddleName { get => _middleame ?? String.Empty; set => _middleame = value; }
        public string? LastName { get => _lastname ?? String.Empty; set => _lastname = value; }
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