using MHLCommon.MHLBookDir;
using MHLCommon.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHLSourceScannerLib.BookDir
{
    public class ViewModel4PathElement : ViewModel
    {
        #region [Fields]
        private IPathElement<ElementType, ViewModel4PathElement> pathElement;
        #endregion

        #region [Constructors]
        public ViewModel4PathElement(PathElement pathElement)
        {
            this.pathElement = pathElement;
        }
        #endregion

        #region [Properties]
        public ObservableCollection<ElementType> Source => pathElement.Source;
        public ElementType SelectedItem
        {
            get => pathElement.SelectedItem;
            set => pathElement.SelectedItem = value;
        }

        public bool IsTyped => pathElement.IsTyped;
        public string Name => pathElement.Name;
        #endregion
    }
}
