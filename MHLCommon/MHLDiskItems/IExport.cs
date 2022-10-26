namespace MHLCommon.MHLDiskItems
{
    public interface IExport
    {
        public bool CheckDestination();
        bool Export(IDiskItem diskItem);
    }
}