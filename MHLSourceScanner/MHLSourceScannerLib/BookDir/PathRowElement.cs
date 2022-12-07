using MHLCommon.MHLBookDir;
using MHLCommon.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MHLSourceScannerLib.BookDir
{
    public class PathRowElement : IPathRowElement<PathElement, ViewModel4PathRowElement>
    {
        #region [Fields]
        private ViewModel4PathRowElement viewModel;

        private ObservableCollection<PathElement> source = new ObservableCollection<PathElement>();       

        private PathElement selectedItem;
        #endregion

        #region [Constructors]
        public PathRowElement() : this(BookPathItem.Author, BookPathTypedItem.None) { }

        [JsonConstructor]
        public PathRowElement(BookPathItem selectedItemType, BookPathTypedItem selectedTypedItemType)
        {
            viewModel = new ViewModel4PathRowElement(this);
            source.Add(new PathElement(BookPathItem.Author, this));
            source.Add(new PathElement(BookPathItem.FirstLetter, BookPathTypedItem.Title, this));
            source.Add(new PathElement(BookPathItem.SequenceName, this));
            source.Add(new PathElement(BookPathItem.SequenceNum, this));
            source.Add(new PathElement(BookPathItem.Title, this));

            selectedItem = Source[0];

            SelectedItemType = selectedItemType;
            SelectedTypedItemType = selectedTypedItemType;
        }
        #endregion

        #region [Properties]
        [JsonIgnore]
        public ViewModel4PathRowElement ViewModel => viewModel;
        [JsonIgnore]
        public ObservableCollection<PathElement> Source => source;
        [JsonIgnore]
        public PathElement SelectedItem
        {
            get => selectedItem;
            set => selectedItem = value;
        }

        public BookPathItem SelectedItemType
        {
            get=>selectedItem.ElementType;
            set
            {
                foreach (PathElement item in source)
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
            get => selectedItem.ElementItemType;
            set => selectedItem.ElementItemType = value;
        }
        #endregion

        #region [IPathRowElement<PathElement, ViewModel4PathElement> Implementation]
        ObservableCollection<PathElement> IPathRowElement<PathElement, ViewModel4PathRowElement>.Source => Source;

        ViewModel4PathRowElement IPathRowElement<PathElement, ViewModel4PathRowElement>.ViewModel => ViewModel;

        PathElement IPathRowElement<PathElement, ViewModel4PathRowElement>.SelectedItem
        {
            get => SelectedItem;
            set => SelectedItem = value;
        }
        #endregion
    }
}
