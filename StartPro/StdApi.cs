using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StartPro
{
    public static class StdApi
    {
        public static ImageSource GetIcon(string path)
        {
            try
            {
                Icon icon = Icon.ExtractAssociatedIcon(path);
                ImageSource image = Imaging.CreateBitmapSourceFromHIcon(
                    icon.Handle, Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions( ));
                return image;
            }
            catch
            {
                return new BitmapImage( );
            }
        }
    }
}
