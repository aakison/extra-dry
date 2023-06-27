using System;

namespace ExtraDry.Highlight;

public class Highlighter
{
    public Engine Engine { get; set; }

    public XmlConfiguration Configuration { get; set; }

    public Highlighter(Engine engine, XmlConfiguration configuration)
    {
        Engine = engine;
        Configuration = configuration;
    }

    public Highlighter(Engine engine)
        : this(engine, new DefaultConfiguration())
    {
    }

    public string Highlight(string definitionName, string input)
    {
        if (definitionName == null) {
            throw new ArgumentNullException(nameof(definitionName));
        }

        if (Configuration.Definitions.ContainsKey(definitionName)) {
            var definition = Configuration.Definitions[definitionName];
            return Engine.Highlight(definition, input);
        }
        else
        {
            Console.WriteLine($"Unable to find requested language definition '{definitionName}'");
        }

        return input;
    }
}
