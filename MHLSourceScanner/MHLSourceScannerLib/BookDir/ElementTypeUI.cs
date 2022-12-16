using MHLCommon.MHLBookDir;
using System;
using System.Text.Json.Serialization;

namespace MHLSourceScannerLib
{
    public struct ElementTypeUI : IElementTypeUI
    {
        public ElementTypeUI(BookPathTypedItem typeID)
        {
            TypeID = typeID;

            switch (TypeID)
            {
                case BookPathTypedItem.Author:
                    Name = MHLResources.MHLResourcesManager.GetStringFromResources("PathElement_Author", "Author");
                    break;
                case BookPathTypedItem.SequenceName:
                    Name = MHLResources.MHLResourcesManager.GetStringFromResources("PathElement_SequenceName", "Sequence Name");
                    break;
                case BookPathTypedItem.Title:
                    Name = MHLResources.MHLResourcesManager.GetStringFromResources("PathElement_Title", "Title");
                    break;
                case BookPathTypedItem.None:
                    Name = String.Empty;
                    break;
                default:
                    throw new Exception("Unknown path element");
            }
        }
        [JsonIgnore]
        public string Name { get; }
        public BookPathTypedItem TypeID { get; }
    }   
}