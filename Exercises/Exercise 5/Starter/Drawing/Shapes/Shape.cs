using System.Drawing;
using System.Xml.Serialization;

namespace Shapes
{
    [XmlInclude(typeof(Circle))]
    [XmlInclude(typeof(Rectangle))]
    [XmlInclude(typeof(Triangle))]
    public class Shape
    {
        public Position Location { get; set; }
        [XmlIgnore]
        public Color Color { get; set; } = Color.Black;
        public string HtmlColor
        {
            get
            {
                return ColorTranslator.ToHtml(Color);
            }
            set
            {
                Color = ColorTranslator.FromHtml(value);
            }
        }
        public int LineWidth { get; set; } = 5;
        public bool IsSelected { get; set; } = false;

        public virtual void Draw(IDevice device)
        {

        }
    }
}