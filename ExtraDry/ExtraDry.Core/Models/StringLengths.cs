namespace ExtraDry.Core;

/// <summary>
/// Common string lengths for aligning the length of strings in the system.  Aligned lengths
/// improve interoperability and consistency across multiple services.  Lengths are given semenatic
/// meaning to facilitate understanding and consistency across the system.  Additionally, they are
/// used to inform form layout when using multiple columns.
/// </summary>
public static class StringLength
{
    /// <summary>
    /// Represent text that is typically a single word, such as a name or a code.  Receives a small
    /// form field element, e.g. a quarter of a page width.
    /// </summary>
    public const int Word = 25;

    /// <summary>
    /// Represents a short phrase or claus, such as a caption.  Receives a medium
    /// form field element, e.g. half of a page width.
    /// </summary>
    public const int Words = 50;

    /// <summary>
    /// Represents a full line of text, which may or may not be a complete sentence.  Receives a
    /// full width field element, e.g. a full page width.
    /// </summary>
    public const int Line = 100;

    /// <summary>
    /// Represents enough space for a full sentence.  Receives a full width field element like a 
    /// line, but with more room for longer sentences.
    /// </summary>
    public const int Sentence = 250;

    /// <summary>
    /// Represents enough space for a typical paragraph.  Receives a multi-line field element.
    /// </summary>
    public const int Paragraph = 1_000;

    /// <summary>
    /// Represents enough space for a full page of text.  Receives a multi-line field element.
    /// </summary>
    public const int Page = 5_000;

    /// <summary>
    /// Represents enough space for an extremely long, but not unlimited amount of text.  
    /// Receives a multi-line field element.
    /// </summary>
    public const int Book = 1_000_000;
}
