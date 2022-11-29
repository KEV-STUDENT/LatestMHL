using MHLCommon.MHLBookDir;
using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHLSourceScannerLib.BookDir
{
    public class ViewModel4PathRow : ViewModel
    {
        #region [Fields]
        private IPathRow<PathRowElement> pathRow;
        #endregion

        #region [Constructors]
        public ViewModel4PathRow(IPathRow<PathRowElement> item)
        {
            pathRow= item;            
        }
        #endregion

        #region [Properies]
        public int Count => pathRow.Count;
        #endregion

        #region [Indexer]
        public ViewModel4PathRowElement this[int i]
        {
            get
            {
                return (i >= pathRow.Count ? pathRow[Count].ViewModel : pathRow[i].ViewModel);
            }
        }
        #endregion      
    }
}
