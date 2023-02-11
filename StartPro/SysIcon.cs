using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StartPro
{
    public static class SysIcon
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
