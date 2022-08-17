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
        private XmlDocument? _xDoc;
        private XmlNamespaceManager? _namespaceManager;
        private string? _title = null;
        private List<IBookAttribute>? _authors = null;

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
                    _authors = LoadAuthors();
                return _authors;
            }
        }
        #endregion

        #region [Private Methods]
        private List<IBookAttribute> LoadAuthors()
        {
            List<IBookAttribute> authors = new List<IBookAttribute>();

            XmlNodeList? nodeList = GetNodeList("//fb:description/fb:title-info/fb:author");
           
            if((nodeList?.Count ?? 0) == 0)
                nodeList = GetNodeList("//description/title-info/author");

            if (nodeList != null)
            {
                
                foreach (XmlNode author in nodeList)
                {
                    MHLAuthor a;
                    System.Diagnostics.Debug.WriteLine(author.HasChildNodes);
                    if (author.HasChildNodes)
                    {
                        a = new MHLAuthor();
                        foreach (XmlNode n in author.ChildNodes)
                        {
                            switch (n.LocalName)
                            {
                                case "first-name":
                                    a.FirstName = n.InnerText;
                                    break;
                                case "middle-name":
                                    a.MiddleName = n.InnerText;
                                    break;
                                case "last-name":
                                    a.LastName = n.InnerText;
                                    break;
                            }
                        }
                        authors.Add(a);
                    }
                }
            }
            return authors;
        }

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
        #endregion

        #region [Protected Methods]
        #endregion
    }
}