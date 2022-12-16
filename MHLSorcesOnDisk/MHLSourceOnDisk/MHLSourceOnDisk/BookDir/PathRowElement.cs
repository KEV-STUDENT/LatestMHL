using MHLCommon.MHLBookDir;
using MHLCommon.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MHLSourceOnDisk.BookDir
{
    public class PathRowElement : IPathRowElement<IPathElement>
    {
        #region [Fields]
        private PathElement selectedItem;
        #endregion

        #region [Properties]
        [JsonIgnore]
        public PathElement SelectedItem
        {
            get => selectedItem;
            set => selectedItem = value;
        }
        #endregion

        #region[IPathRowElement implementation]
        IPathElement IPathRowElement<IPathElement>.SelectedItem
        {
            get => SelectedItem;
        }
        #endregion

        #region [Constructors]
        public PathRowElement() : this(BookPathItem.Author, BookPathTypedItem.None) { }

        [JsonConstructor]
        public PathRowElement(BookPathItem selectedItemType, BookPathTypedItem selectedTypedItemType)
        {
            selectedItem = new PathElement(selectedItemType, selectedTypedItemType);
        }
        #endregion
    }
}
