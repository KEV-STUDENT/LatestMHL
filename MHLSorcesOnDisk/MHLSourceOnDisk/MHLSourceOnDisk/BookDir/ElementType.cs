using MHLCommon.MHLBookDir;

namespace MHLSourceOnDisk.BookDir
{
    public struct ElementType : IElementType
    {
        public ElementType(BookPathTypedItem typeID)
        {
            TypeID = typeID;
        }
        public BookPathTypedItem TypeID { get; }
    }
}
