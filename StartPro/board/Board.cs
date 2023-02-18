using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StartPro
{
    public partial class Board
    {
        public static bool IsDrag { get; set; }

        public static readonly DependencyProperty AppNameProperty;
        public static readonly DependencyProperty AppIconProperty;
        public static readonly DependencyProperty AppPathProperty;
        public static readonly DependencyProperty BoardSizeProperty;
        public static readonly DependencyProperty BoardColorProperty;
        public static readonly DependencyProperty BoardFontSizeProperty;

        static Board( )
        {
            Type thisType = typeof(Board);
            PropertyMetadata appNameMeta = new PropertyMetadata("Application");
            PropertyMetadata appIconMeta = new PropertyMetadata(new BitmapImage( ));
            PropertyMetadata appPathMeta = new PropertyMetadata(Default.AppName, AppPathChanged);
            PropertyMetadata boardSizeMeta = new PropertyMetadata(BoardType.Medium);
            PropertyMetadata boardColorMeta = new PropertyMetadata(Default.Background);
            PropertyMetadata boardFontSizeMeta = new PropertyMetadata(Default.FontSize);
            AppNameProperty = DependencyProperty.Register("AppName", typeof(string), thisType, appNameMeta);
            AppIconProperty = DependencyProperty.Register("SysIcon", typeof(ImageSource), thisType, appIconMeta);
            AppPathProperty = DependencyProperty.Register("AppPath", typeof(string), thisType, appPathMeta);
            BoardSizeProperty = DependencyProperty.Register("BoardSize", typeof(BoardType), thisType, boardSizeMeta);
            BoardColorProperty = DependencyProperty.Register("BoardColor", typeof(SolidColorBrush), thisType, boardColorMeta);
            BoardFontSizeProperty = DependencyProperty.Register("BoardFontSize", typeof(double), thisType, boardFontSizeMeta);
        }

        public BoardType BoardSize
        {
            get { return (BoardType) GetValue(BoardSizeProperty); }
            set { SetValue(BoardSizeProperty, value); }
        }
        public SolidColorBrush BoardColor
        {
            get => (SolidColorBrush) GetValue(BoardColorProperty);
            set { SetValue(BoardColorProperty, value); }
        }
        public double BoardFontSize
        {
            get => (double) GetValue(BoardFontSizeProperty);
            set { SetValue(BoardFontSizeProperty, value); }
        }

        public string AppName
        {
            get => (string) GetValue(AppNameProperty);
            set { SetValue(AppNameProperty, value); }
        }
        public ImageSource AppIcon
        {
            get => (ImageSource) GetValue(AppIconProperty);
            set { SetValue(AppIconProperty, value); }
        }
        public string AppPath
        {
            get => (string) GetValue(AppPathProperty);
            set { SetValue(AppPathProperty, value); }
        }

    }
}
