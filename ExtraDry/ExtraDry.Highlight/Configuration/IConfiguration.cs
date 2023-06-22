using System.Collections.Generic;

namespace ExtraDry.Highlight
{
    public interface IConfiguration
    {
        IDictionary<string, Definition> Definitions { get; }
    }
}
