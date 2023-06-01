namespace TDesign.Admin.Components;

/// <summary>
/// 可以打开带有表单的对话框功能的 <see cref="TLink"/> 组件。
/// </summary>
/// <typeparam name="TModel">表单模型的类型。</typeparam>
public class FormDialogLink<TModel> : FormDialogComponentBase<TModel> where TModel : class, new()
{
    /// <summary>
    /// 超链接的文本。
    /// </summary>
    [Parameter]public string? Text { get; set; }
    /// <summary>
    /// 文字的尺寸。
    /// </summary>
    [Parameter]public Size Size { get; set; } = Size.Medium;
    /// <summary>
    /// 链接文字的颜色。
    /// </summary>
    [Parameter] public Theme Theme { get; set; } = Theme.Primary;
    /// <summary>
    /// 是否具备下划线效果。
    /// </summary>
    [Parameter]public bool Underline { get; set; }
    /// <summary>
    /// 鼠标悬停文字的效果。
    /// </summary>
    [Parameter] public LinkHover Hover { get; set; } = LinkHover.Underline;
    /// <summary>
    /// 禁用状态。
    /// </summary>
    [Parameter]public bool Disabled { get; set; }
    /// <summary>
    /// 图标名称。
    /// </summary>
    [Parameter]public object? IconName { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.Component<TLink>()
            .Attribute(nameof(TLink.Size),Size)
            .Attribute(nameof(TLink.Theme),Theme)
            .Attribute(nameof(TLink.Underline),Underline)
            .Attribute(nameof(TLink.Disabled),Disabled)
            .Attribute(nameof(TLink.Hover),Hover)
            .Attribute("onclick", HtmlHelper.Instance.Callback().Create<MouseEventArgs>(this, OpenDialog))
            .ChildContent(content => content.Component<TIcon>(IconName is not null).Attribute(nameof(TIcon.Name),IconName).Close().Content(Text))
            .Close();
    }
}
