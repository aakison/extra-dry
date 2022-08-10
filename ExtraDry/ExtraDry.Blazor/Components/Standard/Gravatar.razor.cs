#nullable enable

using System.Text;

namespace ExtraDry.Blazor;

/// <summary>
/// Represents a gravatar to easily create a image from an e-mail address (if the user uses gravatar).
/// </summary>
public partial class Gravatar : ComponentBase {

    /// <summary>
    /// The e-mail address for this gravatar, also displayed as 'alt' text unles `HideEmail` is enabled.
    /// </summary>
    [Parameter, EditorRequired]
    public string? Email { get; set; }

    /// <summary>
    /// The size of the gravatar requested from the server, if not supplied the system provides default size.
    /// </summary>
    [Parameter]
    public int? Size { get; set; }

    /// <summary>
    /// Hides the e-mail address on the page to provide some slight protection if gravatar on public site.
    /// However, this is not a secure way of hiding the e-mail; if confidentiality is an issue, collect and store avatars on site.
    /// </summary>
    [Parameter]
    public bool HideEmail { get; set; }

    private string DisplayEmail => HideEmail ? "User Gravatar" : Email ?? "";

    private string Url => ToGravatarUrl(Email, Size);

    public static string ToGravatarUrl(string? email, int? size)
    {
        var hash = ToGravatarHash(email);
        var sizeUrl = size.HasValue ? $"&s={size.Value}" : "";
        return $"https://www.gravatar.com/avatar/{hash}?d=mp{sizeUrl}";
    }

    public static string ToGravatarHash(string? email)
    {
        if(string.IsNullOrWhiteSpace(email)) {
            return "00000000000000000000000000000000";
        }
        var encoder = new UTF8Encoding();
        using var md5 = new Internal.MD5();
        var hashedBytes = md5.ComputeHash(encoder.GetBytes(email.ToLowerInvariant()));
        var hash = string.Join("", hashedBytes.Select(e => e.ToString("X2")));
        return hash.ToLowerInvariant();
    }
}
