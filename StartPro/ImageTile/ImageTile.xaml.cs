using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using DependencyPropertyGenerator;
using StartPro.Api;

namespace StartPro.Tile;

[DependencyProperty<string>("ImagePath", DefaultValue = "")]
[DependencyProperty<Stretch>("Stretch", DefaultValue = Stretch.Uniform)]
public partial class ImageTile : TileBase, IEditable<ImageTile>
{
    public ImageTile( )
    {
        Grid root = Content as Grid;
        InitializeComponent( );
        userControl.Content = null;
        border.Child = MainImage;

        Utils.AppendContexts(ContextMenu, contextMenu);
        Content = root;
    }

    public IEditor<ImageTile> Editor => new CreateImage(this);

    public override void ReadAttributes(XmlNode node)
    {
        base.ReadAttributes(node);
        ImagePath = node.FromAttribute("ImagePath", string.Empty);
        Stretch = node.FromAttribute("Stretch", Stretch.Uniform);
    }

    public override void WriteAttributes(ref XmlElement element)
    {
        base.WriteAttributes(ref element);
        element.SetAttribute("Type", "ImageTile");
        element.SetAttribute("ImagePath", ImagePath);
        element.SetAttribute("Stretch", ((int) Stretch).ToString( ));
    }

    private void EditTile(object o, RoutedEventArgs e)
    {
        (this as IEditable<ImageTile>).Edit(Owner);
    }

    partial void OnImagePathChanged(string newValue)
    {
        MainImage.Source = new BitmapImage(new Uri(newValue));
    }
}
