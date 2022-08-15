using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MHLSourceOnDisk;
using MHLSourceScannerModelLib;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using MHLCommon;

namespace MHLSourceScannerLib
{
    /// <summary>
    /// Логика взаимодействия для SourceTree.xaml
    /// </summary>
    public partial class SourceTree : UserControl, IShower
    {
        public ShowerViewModel ViewModel { get; private set; }
        protected IShower shower;
        public SourceTree()
        {
            InitializeComponent();
            DiskItemShower shower = new DiskItemShower();
            shower.UpdateView = UpdateViewAction;
            //shower.CreateItem = CreateViewItem;
            shower.LoadCollection = LoadItemCollection;
            this.shower = shower;
            ViewModel = new ShowerViewModel();
            //DataContext = this;
            ShowSource.ItemsSource = new ObservableCollection<ITreeDiskItem>();
        }


        private void UpdateViewAction()
        {
            ShowSource.ItemsSource = SourceItems;
        }

        /*private void CreateViewItem1(IDiskItem item, ITreeItem parent)
        {
            Task.Factory.StartNew(()=>
            {
                System.Diagnostics.Debug.WriteLine("-=1-Start {0} =-", System.Threading.Thread.CurrentThread.ManagedThreadId);
                CreateViewItem(item, parent);
                System.Threading.Thread.Sleep(100000);
                System.Diagnostics.Debug.WriteLine("-=1-End {0} =-", System.Threading.Thread.CurrentThread.ManagedThreadId);
            });
            //System.Threading.Thread thread = new System.Threading.Thread(()=>CreateViewItem(diskItem, treeItem));
            //thread.Start();
        }*/


        private void CreateViewItem(IDiskItem item, ITreeDiskItem parent)
        {
            parent.AddDiskItem(item);
            //Dispatcher.BeginInvoke(new Action<IDiskItem, ITreeItem>((i, p) => p.AddDiskItem(i, p)), diskItem, treeItem);

            //Action act = new Action(() => treeItem.AddDiskItem(diskItem, treeItem));
            //Task t = Task.Factory.StartNew(Dispatcher.BeginInvoke(act);

            // Dispatcher.InvokeAsync(new Action(() => treeItem.AddDiskItem(diskItem, treeItem)));
            //treeItem.AddDiskItem(diskItem, treeItem);

        }

        private void LoadItemCollection(ITreeDiskItem parent)
        {
            parent.SourceItems.Clear();
            parent.LoadItemCollection();
           /* ObservableCollection<ITreeItem> collection = parent.LoadChildsCollection();
            foreach (var i in collection)
                parent.SourceItems.Add(i);*/

            /*System.Diagnostics.Debug.WriteLine("Thread 11 : {0}  Task : {1}", System.Threading.Thread.CurrentThread.ManagedThreadId, Task.CurrentId);

            Task<ObservableCollection<ITreeItem>> task = parent.LoadChildsCollectionAsync();
            task.ContinueWith((t) =>
                 {                
                     foreach (var i in t.Result)
                        Dispatcher.BeginInvoke(()=>parent.SourceItems.Add(i));
                 }
             );*/

            //task.Start();
            //task.Wait();

            //parent.LoadItemCollection();
            //Dispatcher.BeginInvoke(new Action<IDiskItem?, ITreeItem>((i, p) => p.LoadItemCollection(i, p)), source, treeItem);

            //Dispatcher.BeginInvoke(new Action(() => treeItem.LoadChilds()));


            //Dispatcher.InvokeAsync(new Action(() => treeItem.LoadItemCollection()));
            //System.Diagnostics.Debug.WriteLine("LoadItemCollection :" + source?.Name??String.Empty );
        }

        public ObservableCollection<ITreeDiskItem> SourceItems
        {
            get { return shower.SourceItems; }
        }


        ObservableCollection<ITreeDiskItem> IShower.SourceItems
        {
            get { return SourceItems; }
        }

        void IShower.UpdateView()
        {
            /*Task.Factory.StartNew(() =>
            {
                shower.UpdateView();
            }
                //Dispatcher.BeginInvoke(new Action(() => shower.UpdateView()))}
            );*/
            shower.UpdateView();
            //ShowSource.ItemsSource = shower.SourceItems;
        }

        void IShower.UpdateView(ITreeDiskItem treeItem)
        {

            /*Task.Factory.StartNew(() =>
            {
                shower.UpdateView(treeViewDiskItem);
            }
            );*/
            //Dispatcher.BeginInvoke(new Action(() => shower.UpdateView(treeViewDiskItem)));

            shower.UpdateView(treeItem);
            //ShowSource.ItemsSource = shower.SourceItems;
        }

        void IShower.AddDiskItem(IDiskItem diskItem, ITreeDiskItem treeItem)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                treeItem.AddDiskItem(diskItem);
            }));
        }

        void IShower.LoadItemCollection(ITreeDiskItem treeItem)
        {
            shower.LoadItemCollection(treeItem);
        }
    }
}
