using MHLCommon;
using MHLCommon.ExpDestinations;
using MHLSourceOnDisk;
using MHLSourceOnDisk.BookDir;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;
using System.IO;

namespace MHLSourceOnDiskTest
{
    [TestClass]
    public class Export2DirTest
    {
        [TestMethod]
        [DataRow(@"F:\1\test\426096.fb2", "", @"F:\1\test\destination",
            @"{""SubRows"":[{
                    ""SubRows"":[{
                        ""SubRows"":[],
                        ""Items"":[
                            {""SelectedItemType"":4,""SelectedTypedItemType"":0},
                            {""SelectedItemType"":2,""SelectedTypedItemType"":0},
                            { ""SelectedItemType"":1,""SelectedTypedItemType"":1}],""IsFileName"":true}],
                    ""Items"":[
                            {""SelectedItemType"":3,""SelectedTypedItemType"":0},
                            { ""SelectedItemType"":4,""SelectedTypedItemType"":0}],""IsFileName"":false}],
                ""Items"":[
                    {""SelectedItemType"":2,""SelectedTypedItemType"":0},
                    { ""SelectedItemType"":3,""SelectedTypedItemType"":0},
                    { ""SelectedItemType"":1,""SelectedTypedItemType"":3}],""IsFileName"":false}")]
        public void GetDestinationDir(string pathFile, string fb2Name, string pathDestination, string jsonStr)
        {
            string res = string.Empty;
            DiskItemFileFB2? itemFB2;


            if (string.IsNullOrEmpty(fb2Name))
                itemFB2 = DiskItemFabrick.GetDiskItem(pathFile) as DiskItemFileFB2;
            else
                itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathFile, fb2Name) as DiskItemFileFB2;

            if (itemFB2 != null)
            {
                PathRowDisk? row;
                if (string.IsNullOrEmpty(jsonStr))
                    row = null;
                else
                    row = JsonSerializer.Deserialize<PathRowDisk>(jsonStr);

                ExpOptions expOptions = new ExpOptions(pathDestination, row);
                Export2Dir exporter = new Export2Dir(expOptions);
                res = exporter.GetDestinationDir(itemFB2);
            }

            System.Diagnostics.Debug.WriteLine(res);

            Assert.IsFalse(string.IsNullOrEmpty(res));
        }

        [TestMethod]
        [DataRow(@"F:\1\test\426096.fb2", "", @"F:\1\test\destination",
               @"{""SubRows"":[{
                    ""SubRows"":[{
                        ""SubRows"":[],
                        ""Items"":[
                            {""SelectedItemType"":4,""SelectedTypedItemType"":0},
                            {""SelectedItemType"":2,""SelectedTypedItemType"":0},
                            { ""SelectedItemType"":1,""SelectedTypedItemType"":1}],""IsFileName"":true}],
                    ""Items"":[
                            {""SelectedItemType"":3,""SelectedTypedItemType"":0},
                            { ""SelectedItemType"":4,""SelectedTypedItemType"":0}],""IsFileName"":false}],
                ""Items"":[
                    {""SelectedItemType"":2,""SelectedTypedItemType"":0},
                    { ""SelectedItemType"":3,""SelectedTypedItemType"":0},
                    { ""SelectedItemType"":1,""SelectedTypedItemType"":3}],""IsFileName"":false}")]
        public void DestinationFullName(string pathFile, string fb2Name, string pathDestination, string jsonStr)
        {
            string comVal = string.Empty;
            string actVal = string.Empty;

            DiskItemFileFB2? itemFB2;

            if (string.IsNullOrEmpty(fb2Name))
                itemFB2 = DiskItemFabrick.GetDiskItem(pathFile) as DiskItemFileFB2;
            else
                itemFB2 = MHLSourceOnDiskStatic.GetBookFromZip(pathFile, fb2Name) as DiskItemFileFB2;

            if (itemFB2 != null)
            {
                PathRowDisk? row;
                if (string.IsNullOrEmpty(jsonStr))
                    row = null;
                else
                    row = JsonSerializer.Deserialize<PathRowDisk>(jsonStr);

                ExpOptions expOptions = new ExpOptions(pathDestination, row);
                Export2Dir exporter = new Export2Dir(expOptions, itemFB2);


                if (exporter.Destination is ExpDestinstions4Dir exp)
                {
                    comVal = Path.Combine(exp.DestinationPath, itemFB2.Name);
                    actVal = exp.DestinationFullName;
                }
            }

            System.Diagnostics.Debug.WriteLine("--------------------------");
            System.Diagnostics.Debug.WriteLine(comVal);
            System.Diagnostics.Debug.WriteLine(actVal);

            Assert.AreNotEqual(comVal,actVal);
        }

    }
}
