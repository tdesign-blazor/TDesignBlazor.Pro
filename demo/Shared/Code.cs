using Markdig;
using Microsoft.AspNetCore.Components;

namespace TDesign.Admin.Demo.Shared;
public static class Code
{
    public static MarkupString Create(string value)
    {
        var content = Markdown.ToHtml(value);
        return new MarkupString(content);
    }
    public const string BgRun = "background:#ccc";
}
