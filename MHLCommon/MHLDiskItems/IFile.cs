namespace MHLCommon.MHLDiskItems
{
    public interface IFile
    {
        IDiskCollection? Parent { get; }
        bool IsZipEntity { get; }
    }
}