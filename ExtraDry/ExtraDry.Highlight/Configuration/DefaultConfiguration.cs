using System.Xml.Linq;

namespace ExtraDry.Highlight;

public class DefaultConfiguration : XmlConfiguration
{
    public DefaultConfiguration()
    {
        XmlDocument = XDocument.Parse(Resources.DefaultDefinitions);
    }
}
