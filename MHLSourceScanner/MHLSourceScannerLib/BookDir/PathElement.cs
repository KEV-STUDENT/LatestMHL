using MHLCommon.MHLBookDir;
using MHLResources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MHLSourceScannerLib.BookDir
{    public class PathElement : IPathElement<ElementType, ViewModel4PathElement>
    {
        #region [Fields]
        private readonly ElementType noneType = new ElementType(BookPathTypedItem.None);
        private readonly BookPathItem elementType;
        private readonly string name;

        protected ObservableCollection<ElementType> source;
        protected ElementType selectedItem;

        private ViewModel4PathElement viewModel;
        protected PathRowElement parent;
        #endregion

        #region [IPathElement implementation]
        string IPathElement<ElementType, ViewModel4PathElement>.Name => Name;
        BookPathItem IPathElement<ElementType, ViewModel4PathElement>.ElementType {
           get => ElementType;
        }
        bool IPathElement<ElementType, ViewModel4PathElement>.IsTyped => IsTyped;
        ElementType IPathElement<ElementType, ViewModel4PathElement>.TypedItem
        {
            get => TypedItem;
            set => TypedItem = value;
        }

        ViewModel4PathElement IPathElement<ElementType, ViewModel4PathElement>.ViewModel => ViewModel;
        ObservableCollection<ElementType> IPathElement<ElementType, ViewModel4PathElement>.Source => Source;
        ElementType IPathElement<ElementType, ViewModel4PathElement>.SelectedItem
        {
            get => SelectedItem;
            set => SelectedItem = value;
        }
        #endregion

        #region [Constructors]
        public PathElement(BookPathItem typeId, PathRowElement pathRowElement)
        {
            elementType = typeId;
            name = GetNameByTypeId();
            viewModel = new ViewModel4PathElement(this);
            source = new ObservableCollection<ElementType>();
            parent = pathRowElement;
        }
        #endregion

        #region [Properties]
        [JsonIgnore]
        public ViewModel4PathElement ViewModel => viewModel;
        [JsonIgnore]
        protected ObservableCollection<ElementType> Source => source;
        protected ElementType SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
            }
        }

        public virtual string Name => name;
        public BookPathItem ElementType => elementType;
        protected virtual bool IsTyped => elementType == BookPathItem.FirstLetter;
        public virtual ElementType TypedItem {
            get { return noneType; }
            set { throw new NotSupportedException(); }
        }
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
