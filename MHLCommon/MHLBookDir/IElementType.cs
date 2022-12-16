using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHLCommon.MHLBookDir
{
    public interface IElementType
    {
        public BookPathTypedItem TypeID { get; }
    }
    public interface IElementTypeUI : IElementType
    {
        public string Name { get; }
    }
}
