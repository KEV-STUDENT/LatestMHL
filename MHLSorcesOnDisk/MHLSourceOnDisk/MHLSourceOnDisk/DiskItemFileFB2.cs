using MHLCommon;
using System.Xml;
using System.IO.Compression;
using System.Text;
using System.Diagnostics;

namespace MHLSourceOnDisk
{
    public class DiskItemFileFB2 : DiskItemFile, IBook
    {
        private XmlDocument? _xDoc;
        private XmlNamespaceManager? _namespaceManager;
        private string? _title = null;

        private XmlNamespaceManager NamespaceManager
        {
            get
            {
                if (_namespaceManager == null)
                {
                    _namespaceManager = new XmlNamespaceManager(new NameTable());
                    _namespaceManager.AddNamespace("fb", "http://www.gribuser.ru/xml/fictionbook/2.0");
                }
                return _namespaceManager;

            }
        }

        private XmlDocument? XDoc
        {
            get
            {
                if (_xDoc == null)
                    _xDoc = GetXmlDocument();
                return _xDoc;
            }
        }

        #region [Constructors]
        public DiskItemFileFB2(string path) : base(path)
        {
        }

        public DiskItemFileFB2(DiskItemFileZip item, string fullName) : base(item, fullName)
        {
            System.Diagnostics.Debug.WriteLine(item != null);
            System.Diagnostics.Debug.WriteLine(item.Name);
            System.Diagnostics.Debug.WriteLine((item is DiskItemFileZip));
        }
        #endregion

        #region [IBook implementation]
        string IBook.Title
        {
            get
            {
                if (_title == null)
                    _title = GetTitle();
                return _title;
            }
        }
        #endregion

        #region [Private Methods]
        private string GetTitle()
        {
            string? title = GetNode("//fb:description/fb:title-info/fb:book-title[1]")
                   ?? GetNode("//description/title-info/book-title[1]");


            return title ?? string.Empty;
        }
        private XmlDocument GetXmlDocument()
        {
            XmlDocument xDoc = new();
            IDiskItem item = this;

            IDiskItemFile file = this;

            System.Diagnostics.Debug.WriteLine(file.Parent);
           //System.Diagnostics.Debug.WriteLine(((IDiskItem)file.Parent).Name);

            if (file.Parent != null && (file.Parent is DiskItemFileZip zip))
            {
                using (ZipArchive archive = ZipFile.Open(item.Path2Item, ZipArchiveMode.Read))
                {
                    System.Diagnostics.Debug.WriteLine(item.Name);
                    ZipArchiveEntry? entry = archive.GetEntry(item.Name);
                    using (Stream? st = entry?.Open())
                    {
                        if (st != null)
                        {
                            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                            xDoc.Load(st);
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(item.Path2Item))
                {
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    xDoc.Load(item.Path2Item);
                }
            }
            return xDoc;
        }

        private string? GetNode(string node)
        {
            XmlNode? book = XDoc?.DocumentElement?.SelectSingleNode(node, NamespaceManager);
            return book?.InnerText;
        }
        #endregion

        #region [Protected Methods]
        #endregion
    }
}