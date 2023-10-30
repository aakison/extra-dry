namespace ExtraDry.Server.Internal;

/// <summary>
/// Represents a token that can be used to help keep pages of results in a stable order when 
/// calling APIs.
/// </summary>
/// <remarks>
/// This class is kept internal as the actual contents shouldn't leak to consumers.  It's not 
/// security critical, but want to discourage any token hacking so that future versions aren't 
/// breaking changes.
/// </remarks>
internal class ContinuationToken {

    internal ContinuationToken() { }

    public ContinuationToken(string? filter, string? sort, int skip, int take)
    {
        Filter = filter ?? string.Empty;
        Sort = sort ?? string.Empty;
        Skip = ActualSkip(null, skip);
        Take = ActualTake(null, take);
    }

    /// <summary>
    /// Returns the next token for the page that follows this token.  The token's skip and take are 
    /// used unless overridden, in which case the skipOverride and the takeOverride are considered 
    /// as part of the current token, not the next token.
    /// </summary>
    public ContinuationToken Next(int skipOverride = -1, int takeOverride = -1)
    {
        var actualTake = ActualTake(this, takeOverride);
        var actualSkip = ActualSkip(this, skipOverride) + actualTake;
        var next = new ContinuationToken(Filter, Sort, actualSkip, actualTake);
        return next;
    }

    public string Filter { get; set; } = string.Empty;

    public string Sort { get; set; } = string.Empty;

    public int Skip { get; set; }

    public int Take { get; set; }

    public override string ToString()
    {
        using var memory = new MemoryStream(); 
        using var writer = new BinaryWriter(memory); 
        writer.Write(Filter);
        writer.Write(Sort);
        writer.Write(Skip);
        writer.Write(Take);
        var bytes = memory.ToArray();
        var base64 = Convert.ToBase64String(bytes);
        return base64;
    }

    public static ContinuationToken? FromString(string? token)
    {
        if(string.IsNullOrWhiteSpace(token)) {
            return default;
        }
        // One item cache to make this a lookup on token to help keep this internal.
        if(token == lastTokenString) {
            return lastToken;
        }
        try {
            var bytes = Convert.FromBase64String(token);
            using var memory = new MemoryStream(bytes);
            using var reader = new BinaryReader(memory);
            var result = new ContinuationToken {
                Filter = reader.ReadString(),
                Sort = reader.ReadString(),
                Skip = reader.ReadInt32(),
                Take = reader.ReadInt32(),
            };
            lastTokenString = token;
            lastToken = result;
            return result;
        }
        catch(FormatException ex) {
            throw new DryException("Invalid continuation token encoding.",
                $"Bad data paging request: {ex.Message}");
        }
        catch(EndOfStreamException ex) {
            throw new DryException("Invalid continuation token length",
                $"Bad data paging request: {ex.Message}");
        }
    }

    /// <summary>
    /// Returns the winning `take` amount where API call can override token, but not make it 0.
    /// </summary>
    internal static int ActualTake(ContinuationToken? token, int take)
    {
        if(take > 0) {
            return take;
        }
        else {
            return token?.Take ?? PageQuery.DefaultTake;
        }
    }

    /// <summary>
    /// Returns the winning `skip` amount where API call can override token (but both must agree to make it zero).
    /// </summary>
    internal static int ActualSkip(ContinuationToken? token, int skip) {
        if(skip > 0) {
            return skip;
        }
        else {
            return token?.Skip ?? 0;
        }
    }

    private static string lastTokenString = string.Empty;

    private static ContinuationToken? lastToken;

}
