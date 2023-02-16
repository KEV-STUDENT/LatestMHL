using MHLCommon;
using MHLCommon.DataModels;
using MHLCommon.ExpDestinations;
using MHLCommon.MHLBook;
using MHLCommon.MHLDiskItems;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Xml;
using MHL_DB_SQLite;
using MHL_DB_Model;

namespace MHLSourceOnDisk
{
    public class DiskItemFileFB2 : DiskItemFile, IMHLBook
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
        private const string NAMESPACE = "fb";

        private readonly string[] NAMESPACEURL = new string[2] {
            "http://www.gribuser.ru/xml/fictionbook/2.0",
            "http://www.gribuser.ru/xml/fictionbook/2.1"};
        #endregion

        #region [Fields]
        private XmlDocument? _xDoc;
        private XmlNamespaceManager? _namespaceManager;
        private string? _title = null;
        private string? _annotation = null;
        private string? _cover = null;
        private List<MHLAuthor>? _authors = null;
        private List<MHLGenre>? _genres = null;
        private List<MHLKeyword>? _keywords = null;
        private List<MHLSequenceNum>? _sequenceAndNumber = null;
        private bool _xDocLoaded = false;
        private byte _nameSpaceUrl = 0;
        private int? _year = null;
        private IPublisher? _publisher = null;
        #endregion

        #region [Properies]

        public byte CurrentNameSpace
        {
            get => _nameSpaceUrl;
            set
            {
                if (value < NAMESPACE.Length)
                {
                    if (_namespaceManager != null)
                    {
                        ClearProperties();
                        _namespaceManager.RemoveNamespace(NAMESPACE, NAMESPACEURL[_nameSpaceUrl]);
                        _namespaceManager.AddNamespace(NAMESPACE, NAMESPACEURL[value]);
                    }
                    _nameSpaceUrl = value;
                }
            }
        }
        private XmlNamespaceManager NamespaceManager
        {
            get
            {
                if (_namespaceManager == null)
                {
                    _namespaceManager = new XmlNamespaceManager(new NameTable());
                    _namespaceManager.AddNamespace(NAMESPACE, NAMESPACEURL[_nameSpaceUrl]);
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
        string IBook<MHLAuthor, MHLGenre, MHLKeyword>.Title
        {
            get
            {
                _title ??= GetBookAttribute("book-title[1]");
                return _title;
            }
            set { _title = value; }
        }

        List<MHLAuthor> IBook<MHLAuthor, MHLGenre, MHLKeyword>.Authors
        {
            get
            {
                _authors ??= GetBookAttributes<MHLAuthor>("author");
                return _authors;
            }
            set { _authors = value; }
        }

        List<MHLGenre> IBook<MHLAuthor, MHLGenre, MHLKeyword>.Genres
        {
            get
            {
                _genres ??= GetBookAttributes<MHLGenre>("genre");
                return _genres;
            }
            set
            {
                _genres = value;
            }

        }

        List<MHLKeyword> IBook<MHLAuthor, MHLGenre, MHLKeyword>.Keywords
        {
            get
            {
                _keywords ??= GetBookAttributesFromList<MHLKeyword>(GetBookAttribute("keywords[1]"));

                return _keywords;
            }
            set { _keywords = value; }
        }

        string IBook<MHLAuthor, MHLGenre, MHLKeyword>.Annotation
        {
            get
            {
                _annotation ??= GetBookAttribute("annotation[1]");
                return _annotation;
            }
            set
            {
                _annotation = value;
            }
        }

        string IBook<MHLAuthor, MHLGenre, MHLKeyword>.Cover
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
            set { _cover = value; }
        }

        MHLSequenceNum? IMHLBook.SequenceAndNumber
        {
            get
            {
                _sequenceAndNumber ??= GetBookAttributes<MHLSequenceNum>("sequence");
                if ((_sequenceAndNumber?.Count ?? 0) > 0)
                    return _sequenceAndNumber[0];

                return null;
            }
        }

        int? IMHLBook.Year { 
            get{
                if (_year == null)
                {
                    string year = GetBookAttribute("year[1]");
                    int res;
                    if (int.TryParse(year, out res))
                        _year= res;
                 }
                return _year;
            } 
        }

        IPublisher? IMHLBook.Publisher
        {
            get
            {
                if (_publisher == null)
                {
                    string name = GetBookAttribute("publisher[1]");
                    string city = GetBookAttribute("city[1]");

                    if (!(string.IsNullOrEmpty(name) && string.IsNullOrEmpty(city)))
                        _publisher = new MHLPublisher(name, city);
                }
                return _publisher;
            }
        }
        #endregion

        #region [DiskItemExported Implementation]
        public override bool ExportItem(IExportDestination destination)
        {
            if (destination is ExpDestination4Dir exp2Dir)
            {
                return ExportItem2Dir(exp2Dir);
            }
            else if (destination is ExpDestination2SQLite exp2SQLite)
            {
                return ExportItem2SQLite(exp2SQLite);
            }
            return false;
        }
        #endregion

        #region [Methods]
        private bool ExportItem2SQLite(ExpDestination2SQLite exp2SQLite)
        {
            using (DBModelSQLite dB = new DBModelSQLite(exp2SQLite.DestinationPath))
            {
                if (Bizlogic4DB.Export_FB2(dB, this) > 0)
                    dB.SaveChanges();
            }
            return true;
        }
        
        private bool ExportItem2Dir(ExpDestination4Dir exp)
        {
            bool result;
            IFile file = this;
            string newFile;
            newFile = exp.DestinationFullName;
            try
            {

                if (file.Parent is DiskItemFileZip zip)
                    using (ZipArchive zipArchive = ZipFile.OpenRead(zip.Path2Item))
                    {
                        ZipArchiveEntry? fileInZip = zipArchive.GetEntry(this.Name);
                        fileInZip?.ExtractToFile(newFile, exp.OverWriteFiles);
                    }
                else
                    File.Copy(this.Path2Item, newFile, exp.OverWriteFiles);

                result = File.Exists(newFile);
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        private void ClearProperties()
        {
            _title = null;
        }
        private XmlDocument? GetXmlDocument()
        {
            XmlDocument? xDoc = new();
            IDiskItem item = this;
            IFile file = this;

            if (file?.Parent is DiskItemFileZip)
            {
                using ZipArchive archive = ZipFile.Open(item.Path2Item, ZipArchiveMode.Read);
                ZipArchiveEntry? entry = archive.GetEntry(item.Name);
                using Stream? st = entry?.Open();
                xDoc = LoadFromStream(st);
            }
            else
            {
                if (!string.IsNullOrEmpty(item.Path2Item))
                {
                    using Stream st = File.OpenRead(item.Path2Item);
                    xDoc = LoadFromStream(st);
                }
            }
            return xDoc;
        }

        private static XmlDocument? LoadFromStream(Stream? st)
        {
            XmlDocument? xDoc = null;
            string xml;
            if (st != null)
            {
                using var ms = new MemoryStream();
                Stream.Synchronized(st).CopyTo(ms);
                xml = GetStringFromBytes(ms.ToArray());
                xDoc = new XmlDocument();
                xDoc.LoadXml(xml);
            }
            return xDoc;
        }

        private static string GetStringFromBytes(byte[] byte4book)
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

                int bodyEnd = xml.LastIndexOf(BODY_END);
                if (bodyStart > 0 && bodyEnd > 0)
                {
                    xml = xml[..(bodyStart == 0 ? bodyEnd : bodyStart)] + (bodyStart > 0 ? ">" : "") + xml[(bodyEnd == 0 ? bodyStart : bodyEnd)..];
                }
            }
            return xml;
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

        private XmlNodeList? GetElementsByTagName(string tagName)
        {
            return XDoc?.GetElementsByTagName(tagName);
        }

        private List<T> GetBookAttributes<T>(string attributeName)
            where T : MHLBookAttribute<XmlNode>, new()
        {
            List<T> res = new List<T>();
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



            char c173 = (char)173;
            return (title ?? string.Empty).Replace(c173.ToString(), "");
        }

        private static List<T> GetBookAttributesFromList<T>(string attributeList) where T : MHLBookAttribute<string>, new()
        {
            List<T> res = new List<T>();

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