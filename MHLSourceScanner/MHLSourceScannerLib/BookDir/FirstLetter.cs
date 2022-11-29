using MHLCommon.MHLBookDir;
using MHLCommon.ViewModels;
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

    public class FirstLetter : PathElement
    {
        #region [Fields]
        private ElementType typedItem;
        #endregion
        #region [Constructor]
        public FirstLetter(BookPathTypedItem typedItem, PathRowElement pathRowElement) : base(BookPathItem.FirstLetter, pathRowElement)
        {
            this.typedItem = new ElementType(typedItem);
            
            Source.Add(new ElementType(BookPathTypedItem.Author));
            Source.Add(new ElementType(BookPathTypedItem.SequenceName));
            Source.Add(new ElementType(BookPathTypedItem.Title));

            selectedItem = Source[1];
        }
        #endregion

        #region [Properties]
        protected override ElementType TypedItem
        {
            get => typedItem;
            set => typedItem = value;
        }
        protected override bool IsTyped => true;
        #endregion
        #region [Methods]
        #endregion
    }
}