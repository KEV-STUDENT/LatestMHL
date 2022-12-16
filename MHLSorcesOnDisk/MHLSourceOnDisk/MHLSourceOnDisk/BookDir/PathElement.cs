using MHLCommon.MHLBookDir;
namespace MHLSourceOnDisk.BookDir
{
    public class PathElement : IPathElement
    {
        #region [Fields]
        private BookPathItem elementType;
        private BookPathTypedItem elementItemType;
        #endregion

        #region [Constructors]
        public PathElement(BookPathItem typeId) : this(typeId, BookPathTypedItem.None) { }
        public PathElement(BookPathItem elementType, BookPathTypedItem elementItemType)
        {
            ElementType = elementType;
            LoadSource();
            ElementItemType = elementItemType;
        }
        #endregion

        #region [Properties]
        public bool IsTyped => ElementType == BookPathItem.FirstLetter;        
        public virtual BookPathTypedItem ElementItemType
        {
            get => elementItemType;
            set => elementItemType = value;
        }

        public BookPathItem ElementType
        {
            get => elementType;
            set => elementType = value;
        }
        #endregion

        #region [IPathElement implementation]
        BookPathItem IPathElement.ElementType => ElementType;
        bool IPathElement.IsTyped => IsTyped;
        #endregion

        #region [Methods]
        protected virtual void LoadSource()
        {
        }
        #endregion
    }
}
