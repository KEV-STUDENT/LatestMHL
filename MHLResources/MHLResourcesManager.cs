using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Windows.Media.Imaging;
using System.IO;
using System.Reflection;

namespace MHLResources
{
    public static class MHLResourcesManager
    {
        public static BitmapImage GetImageFromResources(string resName)
        {
            BitmapImage bitmapImage = new BitmapImage();
            ResourceManager resourceManager = new ResourceManager("MHLResources.Resource", Assembly.GetExecutingAssembly());

            Bitmap? df = resourceManager.GetObject(resName) as Bitmap;
            if (df != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    df.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ms.Position = 0;
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = ms;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                }
            }
            return bitmapImage;
        }
    }
}
