using System.Windows;
using System.Windows.Controls;
using System.Xml;
using DependencyPropertyGenerator;
using StartPro.Api;

namespace StartPro.Tile;

[DependencyProperty<string>("Text", DefaultValue = "Text")]
[DependencyProperty<TextConfig>("TextConfig")]
[DependencyProperty<VerticalAlignment>("TextVerticalAlignment", DefaultValue = VerticalAlignment.Center)]
[DependencyProperty<HorizontalAlignment>("TextHorizontalAlignment", DefaultValue = HorizontalAlignment.Center)]

public partial class TextTile : TileBase, IEditable<TextTile>
{
    public TextTile( )
    {
        Grid root = Content as Grid;
        InitializeComponent( );
        userControl.Content = null; // 没有任何原因，非要有这一行才能工作。
        userControl = null;
        border.Child = MainText;

        Utils.AppendContexts(ContextMenu, contextMenu);
        Content = root;

        TextConfig = new TextConfig( ); // 是的，默认值要在这里初始化。
    }

    public IEditor<TextTile> Editor => new CreateText(this);

    public override void ReadAttributes(XmlNode node)
    {
        base.ReadAttributes(node);
        Text = node.FromAttribute("Text", string.Empty);
        if (node.ChildNodes[0] is not null)
            TextConfig.ReadAttributes(node);
        TextVerticalAlignment = node.FromAttribute("TextVerticalAlignment", VerticalAlignment.Center);
        TextHorizontalAlignment = node.FromAttribute("TextHorizontalAlignment", HorizontalAlignment.Center);
    }

    public override void WriteAttributes(ref XmlElement element)
    {
        base.WriteAttributes(ref element);
        element.SetAttribute("Type", "TextTile");
        element.SetAttribute("Text", Text);
        element.SetAttribute("TextVerticalAlignment", ((int) TextVerticalAlignment).ToString( ));
        element.SetAttribute("TextHorizontalAlignment", ((int) TextHorizontalAlignment).ToString( ));
        XmlElement textConfig = element.OwnerDocument.CreateElement("TextConfig");
        TextConfig.WriteAttributes(ref textConfig);
        element.AppendChild(textConfig);
    }

    private void EditTile(object o, RoutedEventArgs e)
    {
        (this as IEditable<TextTile>).Edit(Owner);
    }

    partial void OnTextChanged(string newValue)
    {
        TileTextShadow.Opacity = (!App.Settings.UIFlat && TextConfig.TextShadow) ? 0.4 : 0;
    }
}
