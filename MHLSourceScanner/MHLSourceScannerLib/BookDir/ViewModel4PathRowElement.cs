using MHLCommon.MHLBookDir;
using MHLCommon.MHLScanner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHLSourceScannerLib.BookDir
{
    public class ViewModel4PathRowElement
    {
        #region [Fields]
        private PathRowElement pathRowElement;
        #endregion


        #region [Constructors]
        public ViewModel4PathRowElement(PathRowElement pathRowElement)
        {
            this.pathRowElement = pathRowElement;
        }
        #endregion

        #region [Properties]
        public ObservableCollection<PathElement> Source => pathRowElement.Source;
        public PathElement SelectedItem
        {
            get => pathRowElement.SelectedItem; 
            set => pathRowElement.SelectedItem = value;
        }
        #endregion
    }
}
