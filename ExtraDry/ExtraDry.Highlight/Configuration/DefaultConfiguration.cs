using System.Xml.Linq;

namespace ExtraDry.Highlight;

public class DefaultConfiguration : XmlConfiguration
{
    public DefaultConfiguration() : base(XDocument.Parse(HardcodedDefinitions.Xml.TrimStart()))
    {
    }
}
