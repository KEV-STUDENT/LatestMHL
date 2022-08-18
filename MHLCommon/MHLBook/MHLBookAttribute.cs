using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MHLCommon.MHLBook
{
    public abstract class MHLBookAttribute : IBookAttribute
    {
        #region [Fields]
        private XmlNode? _node = null;
        #endregion

        public XmlNode? Node { 
            get { return _node; } 
            set { 
                _node = value;
                LoadInformationFromXML();
            }
        }

        #region [IBookAttribute implementation]
        XmlNode? IBookAttribute.Node => Node;
        #endregion

        #region [Protected Methods]
        protected abstract void LoadInformationFromXML();
        #endregion
    }
}
