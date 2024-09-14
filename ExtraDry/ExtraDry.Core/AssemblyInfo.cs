using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ExtraDry.Tests")]
[assembly: InternalsVisibleTo("ExtraDry.Core.Tests")]

// MD5 Classes are internal, but used by the server and blazor projects.
// Can't rely on Microsoft MD5 as they suppress from Blazor.
[assembly: InternalsVisibleTo("ExtraDry.Blazor")]
[assembly: InternalsVisibleTo("ExtraDry.Server")]

