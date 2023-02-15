using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StartPro
{
    public partial class AppClip
    {
        public static readonly DependencyProperty AppNameProperty;
        public static readonly DependencyProperty AppIconProperty;
        public static readonly DependencyProperty AppPathProperty;
        public static readonly DependencyProperty ClipSizeProperty;
        public static readonly DependencyProperty ClipColorProperty;
        public static readonly DependencyProperty ClipFontSizeProperty;
        public static readonly DependencyProperty ClipRadiusProperty;

        static AppClip( )
        {
            Type thisType = typeof(AppClip);
            PropertyMetadata appNameMeta = new PropertyMetadata("Application");
            PropertyMetadata appIconMeta = new PropertyMetadata(new BitmapImage());
            PropertyMetadata appPathMeta = new PropertyMetadata(Default.AppName, AppPathChanged);
            PropertyMetadata clipSizeMeta = new PropertyMetadata(ClipType.Medium, ClipSizeChanged);
            PropertyMetadata clipColorMeta = new PropertyMetadata(Default.Background);
            PropertyMetadata clipFontSizeMeta = new PropertyMetadata(Default.FontSize);
            PropertyMetadata clipRadiusMeta = new PropertyMetadata(Default.Radius);
            AppNameProperty = DependencyProperty.Register("AppName", typeof(string), thisType, appNameMeta);
            AppIconProperty = DependencyProperty.Register("SysIcon", typeof(ImageSource), thisType, appIconMeta);
            AppPathProperty = DependencyProperty.Register("AppPath", typeof(string), thisType, appPathMeta);
            ClipSizeProperty = DependencyProperty.Register("ClipSize", typeof(ClipType), thisType, clipSizeMeta);
            ClipColorProperty = DependencyProperty.Register("ClipColor", typeof(SolidColorBrush), thisType, clipColorMeta);
            ClipFontSizeProperty = DependencyProperty.Register("ClipFontSize", typeof(double), thisType, clipFontSizeMeta);
            ClipRadiusProperty = DependencyProperty.Register("ClipRadius", typeof(CornerRadius), thisType, clipRadiusMeta);
        }

        public string AppName
        {
            get { return (string) GetValue(AppNameProperty); }
            set { SetValue(AppNameProperty, value); }
        }
        public ImageSource AppIcon
        {
            get { return (ImageSource) GetValue(AppIconProperty); }
            set { SetValue(AppIconProperty, value); }
        }
        public string AppPath
        {
            get { return (string) GetValue(AppPathProperty); }
            set { SetValue(AppPathProperty, value); }
        }
        public ClipType ClipSize
        {
            get { return (ClipType) GetValue(ClipSizeProperty); }
            set { SetValue(ClipSizeProperty, value); }
        }
        public SolidColorBrush ClipColor
        {
            get { return (SolidColorBrush) GetValue(ClipColorProperty); }
            set { SetValue(ClipColorProperty, value); }
        }
        public double ClipFontSize
        {
            get { return (double) GetValue(ClipFontSizeProperty); }
            set { SetValue(ClipFontSizeProperty, value); }
        }
        public CornerRadius ClipRadius
        {
            get { return (CornerRadius) GetValue(ClipRadiusProperty); }
            set { SetValue(ClipRadiusProperty, value); }
        }
    }
}
