using Markdig;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace TDesign.Pro.Demo.Shared;
public static class Code
{
    public static MarkupString Create(string value)
    {
        var pipeline = new MarkdownPipelineBuilder().UseSyntaxHighlighting().Build();
        var content = Markdown.ToHtml(value, pipeline);
        return new MarkupString(content);
    }
    public const string BgRun = "background:#ccc";

    public static IServiceCollection AddTDesignDemo(this IServiceCollection services)
    {
        return services.AddTDesignPro(configure =>
        {
            configure.AppName = "TDesign Pro 专业版";
            configure.Dark = true;
        });
    }
}
