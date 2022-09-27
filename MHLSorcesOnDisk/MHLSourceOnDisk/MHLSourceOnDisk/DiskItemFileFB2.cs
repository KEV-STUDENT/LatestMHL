using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace MHLSourceOnDisk
{
    public class DiskItemFileFB2 : DiskItemFile, IBook
    {
        private const string ATTR_MAIN_PATH = "//fb:description/fb:title-info/fb:";
        private const string ATTR_SECOND_PATH = "//description/title-info/";
        private const string BINARY = "binary";

        private XmlDocument? _xDoc;
        private XmlNamespaceManager? _namespaceManager;
        private string? _title = null;
        private string? _annotation = null;
        private string? _cover = null;
        private List<IBookAttribute<XmlNode>>? _authors = null;
        private List<IBookAttribute<XmlNode>>? _genres = null;
        private List<IBookAttribute<String>>? _keywords = null;

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
                    _title = GetBookAttribute("book-title[1]");
                return _title;
            }
        }

        List<IBookAttribute<XmlNode>> IBook.Authors
        {
            get
            {
                if (_authors == null)
                    _authors = GetBookAttributes<MHLAuthor>("author");
                return _authors;
            }
        }

        List<IBookAttribute<XmlNode>> IBook.Genres
        {
            get
            {
                if (_genres == null)
                    _genres = GetBookAttributes<MHLGenre>("genre");
                return _genres;
            }

        }

        List<IBookAttribute<string>> IBook.Keywords
        {
            get
            {
                if (_keywords == null)
                    _keywords = GetBookAttributesFromList<MHLKeyword>(GetBookAttribute("keywords[1]"));

                return _keywords;
            }
        }

        string IBook.Annotation
        {
            get
            {
                if (_annotation == null)
                    _annotation = GetBookAttribute("annotation[1]");
                return _annotation;
            }
        }

        string IBook.Cover
        {
            get
            {
                if (_cover == null)
                {
                    XmlNodeList? nodeList = GetElementsByTagName(BINARY);
                    if (nodeList != null)
                    {
                        _cover = nodeList[0].InnerText;
                    }
                }
                return _cover??String.Empty;
            }
        }
        #endregion

        #region [Private Methods]
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
            Debug.WriteLine(node);
            XmlNode? book = XDoc?.DocumentElement?.SelectSingleNode(node, NamespaceManager);
            return book?.InnerText;
        }

        private XmlNodeList? GetNodeList(string nodes)
        {
            return XDoc?.DocumentElement?.SelectNodes(nodes, NamespaceManager);
        }

        private XmlNodeList? GetElementsByTagName(string tagName)
        {
            return XDoc?.GetElementsByTagName(tagName);
        }

        private List<IBookAttribute<XmlNode>> GetBookAttributes<T>(string attributeName)
            where T : MHLBookAttribute<XmlNode>, new()
        {
            List<IBookAttribute<XmlNode>> res = new List<IBookAttribute<XmlNode>>();

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

        private string GetBookAttribute(string attributeName)
        {
            string? title = GetNode(string.Concat(ATTR_MAIN_PATH, attributeName))
                   ?? GetNode(string.Concat(ATTR_SECOND_PATH, attributeName));


            return title ?? string.Empty;
        }

        private List<IBookAttribute<string>> GetBookAttributesFromList<T>(string attributeList) where T : MHLBookAttribute<string>, new()
        {
            List<IBookAttribute<string>> res = new List<IBookAttribute<string>>();

            if (!String.IsNullOrEmpty(attributeList))
            {
                foreach (string keyword in attributeList.Split(","))
                {
                    res.Add(new T() { Node = keyword });
                }
            }

            return res;
        }
        #endregion

        #region [Protected Methods]
        #endregion
    }
}