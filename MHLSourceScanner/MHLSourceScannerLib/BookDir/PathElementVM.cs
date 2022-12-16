using MHLCommon.MHLBookDir;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MHLSourceOnDisk.BookDir;

namespace MHLSourceScannerLib.BookDir
{
    public class PathElementVM : PathElement, IPathElementUI<ElementTypeUI, ElementTypeUI, ViewModel4PathElement>
    {
        #region [Fields]
        private readonly ElementTypeUI noneType = new ElementTypeUI(BookPathTypedItem.None);
        private readonly string name;
        protected ObservableCollection<ElementTypeUI> source;
        protected ElementTypeUI selectedItem;
        private readonly ViewModel4PathElement viewModel;
        #endregion

        #region [IPathElement implementation]
        string IPathElementUI<ElementTypeUI, ElementTypeUI, ViewModel4PathElement>.Name => Name;
        ElementTypeUI IPathElementUI<ElementTypeUI, ElementTypeUI, ViewModel4PathElement>.SelectedItem
        {
            get => SelectedItem;
        }
        ViewModel4PathElement IPathElementUI<ElementTypeUI, ElementTypeUI, ViewModel4PathElement>.ViewModel => ViewModel;
        void IPathElementUI<ElementTypeUI, ElementTypeUI, ViewModel4PathElement>.SetSelectedItem(ElementTypeUI selectedItem)
        {
            SelectedItem = selectedItem;
        }
        IEnumerable<ElementTypeUI> IPathElementUI<ElementTypeUI, ElementTypeUI, ViewModel4PathElement>.Source => Source;
        #endregion

        #region [Constructors]
        public PathElementVM(BookPathItem typeId) : this(typeId, BookPathTypedItem.None) { }
        public PathElementVM(BookPathItem elementType, BookPathTypedItem elementItemType):base(elementType, elementItemType)
        {           
            viewModel = new ViewModel4PathElement(this);
            name = GetNameByTypeId();
        }
        #endregion

        #region [Properties]
        public ObservableCollection<ElementTypeUI> Source
        {
            get
            {
                if (source == null)
                    source = new ObservableCollection<ElementTypeUI>();
                return source;
            }
        }
        public ViewModel4PathElement ViewModel => viewModel;
        public string Name => name;
        public ElementTypeUI SelectedItem
        {
            get => selectedItem;
            set => selectedItem = value;
        }
        public override BookPathTypedItem ElementItemType
        {
            get => SelectedItem.TypeID;
            set
            {
                foreach (ElementTypeUI item in Source)
                {
                    if (item.TypeID == value)
                    {
                        SelectedItem = item;
                        return;
                    }
                }
                SelectedItem = noneType;
            }
        }
        #endregion

        #region [Methods]
        private string GetNameByTypeId()
        {
            return ElementType switch
            {
                BookPathItem.Author => MHLResources.MHLResourcesManager.GetStringFromResources("PathElement_Author", "Author"),
                BookPathItem.FirstLetter => MHLResources.MHLResourcesManager.GetStringFromResources("PathElement_1stLetter", "First Letter from : "),
                BookPathItem.SequenceName => MHLResources.MHLResourcesManager.GetStringFromResources("PathElement_SequenceName", "Sequence Name"),
                BookPathItem.SequenceNum => MHLResources.MHLResourcesManager.GetStringFromResources("PathElement_SequenceNum", "Number in Sequence"),
                BookPathItem.Title => MHLResources.MHLResourcesManager.GetStringFromResources("PathElement_Title", "Title"),
                _ => throw new Exception("Unknown path element"),
            };
        }

        protected override void LoadSource()
        {
            if (ElementType == BookPathItem.FirstLetter)
            {
                Source.Add(new ElementTypeUI(BookPathTypedItem.Author));
                Source.Add(new ElementTypeUI(BookPathTypedItem.SequenceName));
                Source.Add(new ElementTypeUI(BookPathTypedItem.Title));
            }
        }
        #endregion
    }
}
