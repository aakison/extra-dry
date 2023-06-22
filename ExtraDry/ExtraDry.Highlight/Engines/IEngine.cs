namespace ExtraDry.Highlight;

public interface IEngine
{
    string Highlight(Definition definition, string input);
}
