using MHLCommon.MHLBook;
using MHLCommon.MHLScanner;
using MHLCommon.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MHLSourceScannerLib;

namespace MHLSourceScanner
{
    public class ViewModel4Scanner : ViewModel
    {
        private bool _destinationIsDirectory = true;
        public bool DestinationIsDirectory
        {
            get { return _destinationIsDirectory; }
            set {
                _destinationIsDirectory = value;
                OnPropertyChanged("DestinationIsDBFile");
                OnPropertyChanged("DestinationIsDirectory");
            }
        }

        public bool DestinationIsDBFile
        {
            get { return !_destinationIsDirectory; }
            set { 
                _destinationIsDirectory = !value;
                OnPropertyChanged("DestinationIsDBFile");
                OnPropertyChanged("DestinationIsDirectory");
            }
        }


    }
}
