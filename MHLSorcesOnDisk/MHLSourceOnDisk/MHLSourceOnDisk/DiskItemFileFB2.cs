using System.Xml;
using System.IO.Compression;
using System.Text;
using System.Diagnostics;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;

namespace MHLSourceOnDisk
{
    public class DiskItemFileFB2 : DiskItemFile, IBook
    {
        private const string ATTR_MAIN_PATH = "//fb:description/fb:title-info/fb:";
        private const string ATTR_SECOND_PATH = "//description/title-info/";

        private XmlDocument? _xDoc;
        private XmlNamespaceManager? _namespaceManager;
        private string? _title = null;
        private List<IBookAttribute>? _authors = null;
        private List<IBookAttribute>? _genres = null;

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

        List<IBookAttribute> IBook.Authors
        {
            get
            {
                if (_authors == null)
                    _authors = GetBookAttributes<MHLAuthor>("author");
                return _authors;
            }
        }

        List<IBookAttribute> IBook.Genres
        {
            get
            {
                if (_genres == null)
                    _genres = GetBookAttributes<MHLGenre>("genre");
                return _genres;
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

            if (file?.Parent is DiskItemFileZip)
            {
                using (ZipArchive archive = ZipFile.Open(item.Path2Item, ZipArchiveMode.Read))
                {
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

        private XmlNodeList? GetNodeList(string nodes)
        {
            return XDoc?.DocumentElement?.SelectNodes(nodes, NamespaceManager);
        }

        private List<IBookAttribute> GetBookAttributes<T>(string attributeName) where T : MHLBookAttribute, new()
        {
            List<IBookAttribute> res = new List<IBookAttribute>();

            XmlNodeList? nodeList = GetNodeList(string.Concat(ATTR_MAIN_PATH, attributeName));

            if ((nodeList?.Count ?? 0) == 0)
                nodeList = GetNodeList(string.Concat(ATTR_SECOND_PATH, attributeName));

            if (nodeList != null)
            {
                foreach (XmlNode node in nodeList)
                {
                    res.Add(new T() { Node = node });
                }
            }
            return res;
        }
        #endregion

        #region [Protected Methods]
        #endregion
    }
}