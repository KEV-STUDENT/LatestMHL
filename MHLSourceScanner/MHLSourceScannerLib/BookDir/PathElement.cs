using MHLCommon.MHLBookDir;
using MHLResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHLSourceScannerLib.BookDir
{    public class PathElement : IPathElement
    {
        #region [Fields]
        private readonly BookPathItem elementType;
        private readonly string name;
        #endregion

        #region [IPathElement implementation]
        string IPathElement.Name => Name;
        BookPathItem IPathElement.ElementType {
           get => ElementType;
        }

        bool IPathElement.IsTyped => IsTyped;
        #endregion

        #region [Constructors]
        public PathElement(BookPathItem typeId)
        {
            elementType = typeId;
            name = GetNameByTypeId();
        }       
        #endregion

        #region [Properties]
        public virtual string Name => name;
        public BookPathItem ElementType => elementType;
        public virtual bool IsTyped => false;
        #endregion

        #region [Methods]
        private string GetNameByTypeId()
        {
            switch (elementType)
            {
                case BookPathItem.Author:
                    return MHLResources.MHLResourcesManager.GetStringFromResources("PathElement_Author", "Author");
                case BookPathItem.FirstLetter:
                    return MHLResources.MHLResourcesManager.GetStringFromResources("PathElement_1stLetter", "First Letter from : ");
                case BookPathItem.SequenceName:
                    return MHLResources.MHLResourcesManager.GetStringFromResources("PathElement_SequenceName", "Sequence Name");
                case BookPathItem.SequenceNum:
                    return MHLResources.MHLResourcesManager.GetStringFromResources("PathElement_SequenceNum", "Number in Sequence");
                case BookPathItem.Title:
                    return MHLResources.MHLResourcesManager.GetStringFromResources("PathElement_Title", "Title");
            }
            throw new Exception("Unknown path element");
        }
        #endregion
    }
}
