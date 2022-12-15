using MHLCommon;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Xml;

namespace MHLSourceOnDisk
{
    public class DiskItemFileFB2 : DiskItemFile, IBook
    {
        #region [Consatants]
        private const string ATTR_MAIN_PATH = "//fb:description/fb:title-info/fb:";
        private const string ATTR_SECOND_PATH = "//description/title-info/";

        private const string SRC_MAIN_PATH = "//fb:description/fb:src-title-info/fb:";
        private const string SRC_SECOND_PATH = "//description/src-title-info/";

        private const string PUBLISH_MAIN_PATH = "//fb:description/fb:publish-info/fb:";
        private const string PUBLISH_SECOND_PATH = "//description/publish-info/";

        private const string BODY_START = "<body";
        private const string BODY_END = "</body>";

        private const string BINARY = "binary";

        private bool _xDocLoaded = false;

        #endregion

        #region [Fields]
        private XmlDocument? _xDoc;
        private XmlNamespaceManager? _namespaceManager;
        private string? _title = null;
        private string? _annotation = null;
        private string? _cover = null;
        private List<IBookAttribute<XmlNode>>? _authors = null;
        private List<IBookAttribute<XmlNode>>? _genres = null;
        private List<IBookAttribute<String>>? _keywords = null;
        private List<IBookAttribute<XmlNode>>? _sequenceAndNumber = null;
        #endregion

        #region [Properies]
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
                if (!_xDocLoaded)
                {
                    _xDocLoaded = true;
                    _xDoc = GetXmlDocument();
                }
                return _xDoc;
            }
        }
        #endregion

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
                        _cover = nodeList[0]?.InnerText;
                    }
                }
                return _cover ?? String.Empty;
            }
        }

        List<IBookAttribute<XmlNode>>? IBook.SequenceAndNumber
        {
            get
            {
                if (_sequenceAndNumber == null)
                {
                    _sequenceAndNumber = GetBookAttributes<MHLSequenceNum>("sequence");
                }
                return _sequenceAndNumber;
            }
        }
        #endregion

        #region [DiskItem Implementation]
        public override bool ExportItem(ExpOptions exportOptions)
        {
            bool result = true;
            IFile file = this;
            string entryName, newFile;

            entryName = ((IDiskItem)this).Name;

            if (exportOptions.OverWriteFiles)
                newFile = Path.Combine(exportOptions.PathDestination, entryName);
            else
                newFile = MHLSourceOnDiskStatic.GetNewFileName(exportOptions.PathDestination, entryName);

            try
            {

                if (file.Parent is DiskItemFileZip zip)
                    using (ZipArchive zipArchive = ZipFile.OpenRead(zip.Path2Item))
                    {
                        ZipArchiveEntry? fileInZip = zipArchive.GetEntry(entryName);

                        if (fileInZip != null)
                        {
                            fileInZip.ExtractToFile(newFile, exportOptions.OverWriteFiles);
                        }
                    }
                else
                    File.Copy(this.Path2Item, newFile, exportOptions.OverWriteFiles);

                result = File.Exists(newFile);                
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }
        #endregion

        #region [Methods]
        private XmlDocument? GetXmlDocument()
        {
            XmlDocument? xDoc = new();
            IDiskItem item = this;
            IFile file = this;

            if (file?.Parent is DiskItemFileZip)
            {
                using (ZipArchive archive = ZipFile.Open(item.Path2Item, ZipArchiveMode.Read))
                {
                    ZipArchiveEntry? entry = archive.GetEntry(item.Name);
                    using (Stream? st = entry?.Open())
                    {
                        xDoc = LoadFromStream(st);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(item.Path2Item))
                {
                    using (Stream st = File.OpenRead(item.Path2Item))
                    {
                        xDoc = LoadFromStream(st);
                    }
                }
            }
            return xDoc;
        }

        private XmlDocument? LoadFromStream(Stream? st)
        {
            XmlDocument? xDoc = null;
            string xml;
            if (st != null)
            {
                using (var ms = new MemoryStream())
                {
                    Stream.Synchronized(st).CopyTo(ms);
                    xml = GetStringFromBytes(ms.ToArray());
                    xDoc = new XmlDocument();
                    xDoc.LoadXml(xml);
                }
            }
            return xDoc;
        }

        private string GetStringFromBytes(byte[] byte4book)
        {
            int markerLength = DiskItemFabrick.CheckFileMarker(byte4book);
            if (markerLength > 0)
            {
                byte[] bookCopy = new byte[byte4book.Length - markerLength];
                Array.Copy(byte4book, markerLength, bookCopy, 0, bookCopy.Length);
                byte4book = bookCopy;
            }
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding = Encoding.GetEncoding("windows-1251");

            if (byte4book.Length > 37)
            {
                if ((byte4book[30] == 85 || byte4book[30] == 117) &&
                    (byte4book[31] == 84 || byte4book[31] == 116) &&
                    (byte4book[32] == 70 || byte4book[32] == 102) &&
                    byte4book[33] == 45 && byte4book[34] == 56 && byte4book[35] == 34 &&
                    byte4book[36] == 63 && byte4book[37] == 62)
                {
                    encoding = Encoding.UTF8;
                }
            }

            string xml = encoding.GetString(byte4book, 0, byte4book.Length);
            if (!string.IsNullOrEmpty(xml))
            {
                int bodyStart = xml.IndexOf(BODY_START);
                if (bodyStart > 0)
                    bodyStart += BODY_START.Length;

                int bodyEnd = xml.IndexOf(BODY_END);
                if (bodyStart > 0 && bodyEnd > 0)
                {
                    xml = xml[..(bodyStart == 0 ? bodyEnd : bodyStart)] + (bodyStart > 0 ? ">" : "") + xml[(bodyEnd == 0 ? bodyStart : bodyEnd)..];
                }
            }
            return xml;
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
            {
                nodeList = GetNodeList(string.Concat(ATTR_SECOND_PATH, attributeName));
                if ((nodeList?.Count ?? 0) == 0)
                {
                    nodeList = GetNodeList(string.Concat(SRC_MAIN_PATH, attributeName));
                    if ((nodeList?.Count ?? 0) == 0)
                    {
                        nodeList = GetNodeList(string.Concat(SRC_SECOND_PATH, attributeName));
                        if ((nodeList?.Count ?? 0) == 0)
                        {
                            nodeList = GetNodeList(string.Concat(PUBLISH_MAIN_PATH, attributeName));
                            if ((nodeList?.Count ?? 0) == 0)
                                nodeList = GetNodeList(string.Concat(PUBLISH_SECOND_PATH, attributeName));
                        }
                    }
                }
            }

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
                   ?? GetNode(string.Concat(ATTR_SECOND_PATH, attributeName))
                   ?? GetNode(string.Concat(SRC_MAIN_PATH, attributeName))
                   ?? GetNode(string.Concat(SRC_SECOND_PATH, attributeName))
                   ?? GetNode(string.Concat(PUBLISH_MAIN_PATH, attributeName))
                   ?? GetNode(string.Concat(PUBLISH_SECOND_PATH, attributeName));


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
    }
}