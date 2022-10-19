using MHLCommon.MHLScanner;
using System.IO;

namespace MHLControls.MHLPickers
{
    public static class MHLAsk4Picker
    {
       static public void AskDirectory(IPicker<string> picker)
       {
           using (var folder = new System.Windows.Forms.FolderBrowserDialog())
           {
               folder.SelectedPath = picker.Value;
               System.Windows.Forms.DialogResult result = folder.ShowDialog();
               if (result == System.Windows.Forms.DialogResult.OK)
               {
                   picker.Value = folder.SelectedPath;
               }
           }
       }

        static public void AskFile(IPicker<string> picker)
        {
            using (var file = new System.Windows.Forms.OpenFileDialog())
            {
                file.FileName = picker.Value;
                file.InitialDirectory = Path.GetDirectoryName(picker.Value);

                System.Windows.Forms.DialogResult result = file.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    picker.Value = file.FileName;
                }
            }
        }
    }
}
