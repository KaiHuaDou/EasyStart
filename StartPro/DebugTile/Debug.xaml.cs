using System.Windows.Controls;
using System.Xml;

namespace StartPro.Tile;

public partial class DebugTile : TileBase
{
    public DebugTile( )
    {
        Grid root = Content as Grid;
        InitializeComponent( );
        userControl.Content = null;
        userControl = null;
        border.Child = MainGrid;

        Content = root;
    }

    public override void WriteAttributes(ref XmlElement element)
    {
        base.WriteAttributes(ref element);
        element.SetAttribute("Type", "__MFGM__");
    }
}
