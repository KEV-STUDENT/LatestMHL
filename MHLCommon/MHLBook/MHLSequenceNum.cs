using MHLCommon.MHLBook;
using System.Xml;

namespace MHLCommon
{
    public class MHLSequenceNum : MHLBookAttribute<XmlNode>
    {
        #region [Fields]
        private string _name;
        private ushort _number;
        #endregion

        #region [Properties]
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public ushort Number
        {
            get => _number;
            set => _number = value;
        }
        #endregion

        #region [Methods]
        protected override void LoadInformationFromXML()
        {
            ushort number;
            Name = Node?.Attributes["name"]?.Value ?? string.Empty;
            ushort.TryParse( Node?.Attributes["number"]?.Value, out number);
            Number = number;
        }
        #endregion
    }
}