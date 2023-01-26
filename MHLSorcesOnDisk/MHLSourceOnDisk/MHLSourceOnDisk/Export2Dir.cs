using MHLCommon;
using MHLCommon.MHLBookDir;
using MHLCommon.MHLBook;
using MHLSourceOnDisk.BookDir;
using MHLCommon.MHLDiskItems;
using MHLCommon.ExpDestinations;

namespace MHLSourceOnDisk
{
    public class Export2Dir : ExportDiskItems
    {
        #region [Constructors]      
        public Export2Dir(ExpOptions expOptions) : base(expOptions) { }
        public Export2Dir(ExpOptions expOptions, IDiskItem diskItem) : base(expOptions, diskItem) { }
        #endregion

        #region [Methods]
        public override bool CheckDestination()
        {
            if (Destination is ExpDestinstions4Dir expDestinstions)
                return CheckCreateDir(expDestinstions.DestinationPath);
            else
                return false;
        }

        protected override IExportDestination CreateDestinantion(IDiskItem? diskItem)
        {
            string path, filename;

            filename = diskItem?.Name ?? string.Empty;

            if (diskItem is IMHLBook book)
            {
                path = GetDestinationDir(book);
                filename = GetFileName(book, filename);
            }
            else
                path = ExportOptions.PathDestination;

            return new ExpDestinstions4Dir(path, filename, ExportOptions.OverWriteFiles);
        }

        private string GetFileName(IMHLBook fb2, string filename)
        {
            string res;
            string prefix = string.Empty;
          
            if (ExportOptions.PathRow is PathRowDisk pathRow)
                do
                {
                    if (pathRow.IsFileName)
                    {
                        prefix = GetSubDirName(pathRow, fb2);
                        break;
                    }
                    else if (pathRow.SubRows.Count > 0)
                    {
                        pathRow = pathRow.SubRows[0];
                    }
                    else break;
                } while (true);

            if (!string.IsNullOrEmpty(prefix))
                res = string.Format("{0}_{1}", prefix, filename);
            else
                res = filename;

            return res;
        }

        public string GetDestinationDir(IMHLBook fb2)
        {
            string destinationDir = ExportOptions.PathDestination;
            string res;
           
            if (ExportOptions.PathRow is PathRowDisk pathRow)
                do
                {
                    if (pathRow.IsFileName)
                        break;

                    res = GetSubDirName(pathRow, fb2);

                    if (!string.IsNullOrEmpty(res))
                        destinationDir = Path.Combine(destinationDir, res);

                    if (pathRow.SubRows.Count > 0)
                        pathRow = pathRow.SubRows[0];
                    else
                        break;
                } while (true);

            return destinationDir;
        }

        private bool CheckCreateDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return Directory.Exists(dir);
        }
        private string GetSubDirName(IPathRow? row, IMHLBook fb2)
        {
            string res = string.Empty;
            string fl;
            if (row is PathRowDisk pathRow)
                for (int i = 0; i < pathRow.Count; i++)
                {
                    fl = string.Empty;
                    switch (pathRow[i].SelectedItemType)
                    {
                        case BookPathItem.Author:
                            if ((fb2?.Authors?.Count ?? 0) > 0 && (fb2.Authors[0] is MHLAuthor author))
                                fl = string.Format("{0}{1}{2}", author.LastName, author.FirstName, author.MiddleName);
                            break;
                        case BookPathItem.Title:
                            fl = fb2.Title;
                            break;
                        case BookPathItem.SequenceName:
                            if ((fb2?.SequenceAndNumber?.Count ?? 0) > 0 && (fb2.SequenceAndNumber[0] is MHLSequenceNum sequence))
                                fl = sequence.Name;
                            break;
                        case BookPathItem.SequenceNum:
                            if ((fb2?.SequenceAndNumber?.Count ?? 0) > 0 && (fb2.SequenceAndNumber[0] is MHLSequenceNum num))
                                fl = num.Number.ToString();
                            break;
                        case BookPathItem.FirstLetter:
                            fl = GetFirstLetter(fb2, pathRow[i].SelectedTypedItemType);       
                            break;
                        default: break;
                    }

                    if (string.IsNullOrEmpty(res))
                        res = fl;
                    else if (!string.IsNullOrEmpty(fl))
                        res = string.Format("{0}_{1}", res, fl);                   
                }

            if (!string.IsNullOrEmpty(res))
            {
                char[] invalidNameChars = Path.GetInvalidFileNameChars();
                foreach (char c in invalidNameChars)
                    res = res.Replace(c, ' ').Replace(" ", null);
            }
            return res;
        }

        private string GetFirstLetter(IMHLBook? fb2, BookPathTypedItem selectedTypedItemType)
        {
            string res = string.Empty;

            if (fb2 != null)
                switch (selectedTypedItemType)
                {
                    case BookPathTypedItem.SequenceName:
                        if ((fb2?.SequenceAndNumber?.Count ?? 0) > 0 && (fb2.SequenceAndNumber[0] is MHLSequenceNum sequence))
                            res = sequence.Name;
                        break;
                    case BookPathTypedItem.Title:
                        res = fb2.Title;
                        break;
                    case BookPathTypedItem.Author:
                        if ((fb2?.Authors?.Count ?? 0) > 0 && (fb2.Authors[0] is MHLAuthor author))
                            res = string.Format("{0}{1}{2}", author.LastName, author.FirstName, author.MiddleName);
                        break;
                    default: break;
                }

            if (!string.IsNullOrEmpty(res))
                res = res[..1];
            return res;
        }
        #endregion
    }
}