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
        public static readonly DependencyProperty ClipMarginProperty;

        static AppClip( )
        {
            Type thisType = typeof(AppClip);
            PropertyMetadata appNameMeta = new PropertyMetadata("Application");
            PropertyMetadata appIconMeta = new PropertyMetadata(new BitmapImage());
            PropertyMetadata appPathMeta = new PropertyMetadata(Default.AppName, AppPathChanged);
            PropertyMetadata clipSizeMeta = new PropertyMetadata(ClipType.Medium);
            PropertyMetadata clipColorMeta = new PropertyMetadata(Default.Background);
            PropertyMetadata clipFontSizeMeta = new PropertyMetadata(Default.FontSize);
            PropertyMetadata clipRadiusMeta = new PropertyMetadata(Default.Radius);
            PropertyMetadata clipMarginMeta = new PropertyMetadata(Default.Margin);
            AppNameProperty = DependencyProperty.Register("AppName", typeof(string), thisType, appNameMeta);
            AppIconProperty = DependencyProperty.Register("SysIcon", typeof(ImageSource), thisType, appIconMeta);
            AppPathProperty = DependencyProperty.Register("AppPath", typeof(string), thisType, appPathMeta);
            ClipSizeProperty = DependencyProperty.Register("ClipSize", typeof(ClipType), thisType, clipSizeMeta);
            ClipColorProperty = DependencyProperty.Register("ClipColor", typeof(SolidColorBrush), thisType, clipColorMeta);
            ClipFontSizeProperty = DependencyProperty.Register("ClipFontSize", typeof(double), thisType, clipFontSizeMeta);
            ClipRadiusProperty = DependencyProperty.Register("ClipRadius", typeof(CornerRadius), thisType, clipRadiusMeta);
            ClipMarginProperty = DependencyProperty.Register("ClipMargin", typeof(Thickness), thisType, clipMarginMeta); ;
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
        public ClipType ClipSize
        {
            get { return (ClipType) GetValue(ClipSizeProperty); }
            set { SetValue(ClipSizeProperty, value); }
        }
        public SolidColorBrush ClipColor
        {
            get => (SolidColorBrush) GetValue(ClipColorProperty);
            set { SetValue(ClipColorProperty, value); }
        }
        public double ClipFontSize
        {
            get => (double) GetValue(ClipFontSizeProperty);
            set { SetValue(ClipFontSizeProperty, value); }
        }
        public CornerRadius ClipRadius
        {
            get => (CornerRadius) GetValue(ClipRadiusProperty);
            set { SetValue(ClipRadiusProperty, value); }
        }
        public Thickness ClipMargin
        {
            get => (Thickness) GetValue(ClipMarginProperty);
            set { SetValue(ClipMarginProperty, value);}
        }
    }
}
