using MHLCommon.BookDir;
using MHLCommon.MHLBookDir;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace MHLSourceScannerLib.BookDir
{
    public class PathRowVM : PathRowWithList<PathRowVM, PathRowElementVM>, IPathRowUI<PathRowVM, PathRowElementVM>
    {
        #region [Fields]       
        private ViewModel4PathRow? viewModel = null;
        #endregion

        #region [Constructors]       
        public PathRowVM() : this(false, null, null, null) { }
        public PathRowVM(PathRowVM? parent) : this(false, null, null, parent) { }
        [JsonConstructor]
        public PathRowVM(bool isFileName, ObservableCollection<PathRowVM> subRows, ObservableCollection<PathRowElementVM>? items) :
            this(isFileName, subRows, items, null){ }

        public PathRowVM(bool isFileName, ObservableCollection<PathRowVM> subRows, ObservableCollection<PathRowElementVM>? items, PathRowVM? parent) :
           base(isFileName, subRows, items, parent) { }
        #endregion

        #region [Properties]       
        [JsonIgnore]
        public ViewModel4PathRow ViewModel => viewModel;       
        #endregion

        #region [Methods]
        protected override void InitPathRow(PathRowVM? parent)
        {
            Parent = parent;

            if(Count == 0)
                Items.Add(new PathRowElementVM());

            viewModel = new ViewModel4PathRow(this);
        }
        #endregion

        #region [IPathRow Implementation] 
        bool IPathRowUI<PathRowVM, PathRowElementVM>.IsExpanded { get => IsExpanded; set => IsExpanded = value; }
        #endregion
    }
}
