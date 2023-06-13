using Microsoft.AspNetCore.Components.Forms;

namespace TDesign.Pro.Templates;

/// <summary>
/// 表示表单对话框模板。
/// </summary>
/// <typeparam name="TModel">表单模型的类型。</typeparam>
partial class FormDialogTemplate<TModel> where TModel : class,new()
{
    /// <summary>
    /// 对话框上下文。
    /// </summary>
    [CascadingParameter]DialogContext Context { get; set; }
    /// <summary>
    /// 表单模型。
    /// </summary>
    TModel Model { get; set; } = new();
    /// <summary>
    /// 对话框标题。
    /// </summary>
    string? Title { get; set; }
    /// <summary>
    /// 表单的内容。
    /// </summary>
    RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 提交事件。
    /// </summary>
    EventCallback<TModel> OnSubmit { get; set; }

    EditContext _fixedEditContext;

    protected override void OnInitialized()
    {
        Model = Context.Parameters.Get<TModel>("Model");
        Title = Context.Parameters.Get<string>("DialogTitle");
        OnSubmit = Context.Parameters.Get<EventCallback<TModel>>("OnSubmit");
        var content= Context.Parameters.Get<RenderFragment<TModel>>("FormContent");
        ChildContent = builder => builder.AddContent(0, content, Model);
        _fixedEditContext = new(Model);
    }

    /// <summary>
    /// 关闭对话框。
    /// </summary>
    /// <returns></returns>
    public Task Close() => Context.Cancel();

    /// <summary>
    /// 验证表单并提交。
    /// </summary>
    public Task Submit()
    {
        var valid = _fixedEditContext.Validate();
        if ( !valid )
        {
            return Task.CompletedTask;
        }
        OnSubmit.InvokeAsync(Model);
        return Context.Confirm(Model);
    }
}
