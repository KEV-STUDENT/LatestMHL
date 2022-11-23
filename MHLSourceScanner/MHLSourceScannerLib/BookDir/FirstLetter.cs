using MHLCommon.MHLBookDir;
using MHLSourceScannerLib.BookDir;
using System;
using System.Xml.Linq;
using System.Xml.XPath;

namespace MHLSourceScannerLib
{
    public struct ElementType
    {
        public ElementType(BookPathTypedItem typeID)
        {
            TypeID = typeID;

            switch (TypeID)
            {
                case BookPathTypedItem.Author:
                    Name = MHLResources.MHLResourcesManager.GetStringFromResources("PathElement_Author", "Author");
                    break;
                case BookPathTypedItem.SequenceName:
                    Name = MHLResources.MHLResourcesManager.GetStringFromResources("PathElement_SequenceName", "Sequence Name");
                    break;
                case BookPathTypedItem.Title:
                    Name = MHLResources.MHLResourcesManager.GetStringFromResources("PathElement_Title", "Title");
                    break;
                default:
                    throw new Exception("Unknown path element");
            }
        }
        public string Name { get; }
        public BookPathTypedItem TypeID { get; }
    }

    public class FirstLetter : PathElement, IPathElementTyped<ElementType>
    {
        #region [Fields]
        private ElementType typedItem;
        #endregion
        #region [Constructor]
        public FirstLetter() : base(BookPathItem.FirstLetter)
        {
            typedItem = new ElementType(BookPathTypedItem.Author);
        }
        #endregion

        #region [Properties]
        public ElementType TypedItem
        {
            get => typedItem;
            set
            {
                typedItem = value;
            }
        }
        public override bool IsTyped => true;
        #endregion

        #region [IPathElementTyped Implemenation]
        ElementType IPathElementTyped<ElementType>.TypedItem
        {
            get => TypedItem;
            set => TypedItem = value;
        }
        #endregion

        #region [Methods]
        #endregion
    }
}