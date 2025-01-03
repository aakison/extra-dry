namespace ExtraDry.Core.Internal;

internal static class HexStringHelper
{
    /// <summary>
    /// Polyfill for Convert.FromHexString, which is available in .net 5 onwards.
    /// </summary>
    public static byte[] GetBytesFromString(string hexstring)
    {
        if(string.IsNullOrEmpty(hexstring)) {
            return [];
        }
        if(hexstring.Length % 2 != 0) {
            throw new ArgumentException("Hex String must be an even number of characters");
        }
        var bytes = new byte[hexstring.Length / 2];
        for(var loop = 0; loop < hexstring.Length; loop += 2) {
            bytes[loop / 2] = GetByteFromchars(hexstring[loop], hexstring[loop + 1]);
        }
        return bytes;
    }

    private static byte GetByteFromchars(char a, char b)
    {
        var val1 = hex.IndexOf(char.ToUpperInvariant(a));
        var val2 = hex.IndexOf(char.ToUpperInvariant(b));
        return val1 < 0 || val2 < 0
            ? throw new ArgumentException("Invalid hex character")
            : Convert.ToByte((val1 * 16) + val2);
    }

    private const string hex = "0123456789ABCDEF";
}
