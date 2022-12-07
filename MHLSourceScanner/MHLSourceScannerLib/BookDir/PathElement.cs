using MHLCommon.MHLBookDir;
using System;
using System.Collections.ObjectModel;

namespace MHLSourceScannerLib.BookDir
{
    public class PathElement : IPathElement<ElementType, ViewModel4PathElement>
    {
        #region [Fields]
        private readonly ElementType noneType = new ElementType(BookPathTypedItem.None);
        private BookPathItem elementType;
        private readonly string name;

        protected ObservableCollection<ElementType> source;
        protected ElementType selectedItem;

        private ViewModel4PathElement viewModel;
        protected PathRowElement? parent;
        #endregion

        #region [IPathElement implementation]
        string IPathElement.Name => Name;
        BookPathItem IPathElement.ElementType {
           get => ElementType;
        }
        bool IPathElement.IsTyped => IsTyped;
        ObservableCollection<ElementType> IPathElement<ElementType>.Source => Source;
        ElementType IPathElement<ElementType>.SelectedItem
        {
            get => SelectedItem;
            set => SelectedItem = value;
        }
        ViewModel4PathElement IPathElement<ElementType, ViewModel4PathElement>.ViewModel => ViewModel;
        #endregion

        #region [Constructors]
        public PathElement(BookPathItem typeId, PathRowElement? pathRowElement) : this(typeId, BookPathTypedItem.None, pathRowElement) { }
        public PathElement(BookPathItem elementType, BookPathTypedItem elementItemType, PathRowElement? pathRowElement)
        {
            source = new ObservableCollection<ElementType>();
            ElementType = elementType;
            LoadSource();
            ElementItemType = elementItemType;

            viewModel = new ViewModel4PathElement(this);
            name = GetNameByTypeId();
            parent = pathRowElement;
        }
        #endregion

        #region [Properties]
        protected ObservableCollection<ElementType> Source => source;
        protected bool IsTyped => ElementType == BookPathItem.FirstLetter;
        public ViewModel4PathElement ViewModel => viewModel;
        public string Name => name;       
        public ElementType SelectedItem
        {
            get => selectedItem;
            set => selectedItem = value;
        }
        public BookPathTypedItem ElementItemType
        {
            get => SelectedItem.TypeID;
            set
            {
                foreach(ElementType item in Source)
                {
                    if(item.TypeID == value)
                    {
                        SelectedItem = item;
                        return;
                    }
                }
                SelectedItem = noneType;
            }
        }

        public BookPathItem ElementType
        {
            get => elementType;
            set => elementType = value;
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

        protected void LoadSource()
        {
            if (ElementType == BookPathItem.FirstLetter)
            {
                Source.Add(new ElementType(BookPathTypedItem.Author));
                Source.Add(new ElementType(BookPathTypedItem.SequenceName));
                Source.Add(new ElementType(BookPathTypedItem.Title));
            }
        }
        #endregion
    }
}
