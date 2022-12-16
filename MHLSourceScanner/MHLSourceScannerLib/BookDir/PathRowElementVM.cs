using MHLCommon.MHLBookDir;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using MHLSourceOnDisk.BookDir;

namespace MHLSourceScannerLib.BookDir
{
    public class PathRowElementVM : PathRowElement, IPathRowElementUI<PathElementVM, PathElementVM, ViewModel4PathRowElement>
    {
        #region [Fields]
        private ViewModel4PathRowElement viewModel;
        private ObservableCollection<PathElementVM> source = new ObservableCollection<PathElementVM>();       
        #endregion

        #region [Constructors]
        public PathRowElementVM() : this(BookPathItem.Author, BookPathTypedItem.None) { }

        [JsonConstructor]
        public PathRowElementVM(BookPathItem selectedItemType, BookPathTypedItem selectedTypedItemType)
        {
            viewModel = new ViewModel4PathRowElement(this);
            source.Add(new PathElementVM(BookPathItem.Author));
            source.Add(new PathElementVM(BookPathItem.FirstLetter, BookPathTypedItem.Title));
            source.Add(new PathElementVM(BookPathItem.SequenceName));
            source.Add(new PathElementVM(BookPathItem.SequenceNum));
            source.Add(new PathElementVM(BookPathItem.Title));

            SelectedItem = Source[0];

            SelectedItemType = selectedItemType;
            SelectedTypedItemType = selectedTypedItemType;
        }
        #endregion

        #region [Properties]
        [JsonIgnore]
        public ViewModel4PathRowElement ViewModel => viewModel;
        [JsonIgnore]
        public ObservableCollection<PathElementVM> Source => source;
        public BookPathItem SelectedItemType
        {
            get=>SelectedItem.ElementType;
            set
            {
                foreach (PathElementVM item in source)
                {
                    if (item.ElementType == value)
                    {
                        SelectedItem = item;
                        return;
                    }
                }
            }
        }
        public BookPathTypedItem SelectedTypedItemType
        {
            get => SelectedItem.ElementItemType;
            set => SelectedItem.ElementItemType = value;
        }
        #endregion

        #region [IPathRowElement<PathElement, ViewModel4PathElement> Implementation]        
        IEnumerable<PathElementVM> IPathRowElementUI<PathElementVM, PathElementVM, ViewModel4PathRowElement>.Source => Source;
        ViewModel4PathRowElement IPathRowElementUI<PathElementVM, PathElementVM, ViewModel4PathRowElement>.ViewModel => ViewModel;

        PathElementVM IPathRowElement<PathElementVM>.SelectedItem => (PathElementVM)SelectedItem;

        void IPathRowElementUI<PathElementVM, PathElementVM, ViewModel4PathRowElement>.SetSelectedItem(PathElementVM selected)
        {
            SelectedItem = selected;
        }
        #endregion
    }
}
