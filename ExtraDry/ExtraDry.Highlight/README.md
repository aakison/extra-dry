# Extra DRY Highlight
A relatively simple and extensible syntax highlighter written in C#.  Adapted for use with Blazor
applications within the Extra DRY framework.  

## TL;DR
```csharp
var highlighter = new Highlighter(new HtmlEngine());
var highlightedCode = highlighter.Highlight("C#", csharpCode);
```

## Syntax definitions
The following is a list of all the definition names of syntaxes/languages that are supported out of the box;

- ASPX
- C
- C++
- C#
- COBOL
- Eiffel
- Fortran
- Haskell
- HTML
- Java
- JavaScript
- Mercury
- MSIL
- Pascal
- Perl
- PHP
- Python
- Ruby
- SQL
- Visual Basic
- VBScript
- VB.NET
- XML

## Output engines
Highlight supports the notion of an output engine which makes it possible to get the syntax highlighted result output in any format. Out of the box Highlight support HTML, XML and RTF output formats.

The HtmlEngine supports inline styles which can be enabled by setting the **UseCss** property to **true**;

```csharp
var highlighter = new Highlighter(new HtmlEngine { UseCss = true });
var highlightedCode = highlighter.Highlight("C#", csharpCode);
```

## Original Sources
The majority of this project originated with the GitHub project https://github.com/thomasjo/highlight
by @thomasjo.  This original project supported proper fonts and RTF output, both of which break
Blazor because of system dependencies on fonts and System.Drawing.  The portions that could not
be handled by Blazor were removed and equivelent replacements were authored as original work.  This
would represent a breaking change to the intent of the original package and was forked instead of
contributed to.

The original source is licensed under MIT which is included in this repo as Third-Party-LICENSE.md. 
The derived works are also MIT licensed.

