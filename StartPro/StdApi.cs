using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StartPro
{
    public static class StdApi
    {
        public static ImageSource Get(string path)
        {
            try
            {
                MemoryStream stream = new MemoryStream( );
                Icon.ExtractAssociatedIcon(path).Save(stream);
                return new BitmapImage { StreamSource = stream };
            }
            catch { return null; }
        }
    }
}
