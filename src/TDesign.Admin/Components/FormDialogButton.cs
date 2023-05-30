using Microsoft.AspNetCore.Components.Rendering;
using TDesign.Admin.Templates;

namespace TDesign.Admin.Components;

/// <summary>
/// 可以打开带有表单的对话框功能的按钮。
/// </summary>
/// <typeparam name="TModel">表单模型的类型。</typeparam>
public class FormDialogButton<TModel>:ComponentBase where TModel:class,new()
{
    /// <summary>
    /// 对话框服务。
    /// </summary>
    [Inject]IDialogService DialogService { get; set; }
    /// <summary>
    /// 表单的内容。
    /// </summary>
    [Parameter][EditorRequired] public RenderFragment<TModel>? FormContent { get; set; }
    /// <summary>
    /// 设置一个回调方法，当表单提交时触发。
    /// </summary>
    [Parameter][EditorRequired] public EventCallback<TModel> OnSubmit { get; set; }
    /// <summary>
    /// 对话框标题。
    /// </summary>
    [Parameter] public string? DialogTitle { get; set; }
    /// <summary>
    /// 提供一个创建模型的委托。比如更新操作，先查出数据再填充模型。
    /// </summary>
    [Parameter] public Func<Task<TModel>>? ModelProvider { get; set; }

    /// <summary>
    /// 设置按钮的任意内容。
    /// </summary>
    [Parameter]public RenderFragment? ButtonContent { get; set; }
    /// <summary>
    /// 设置按钮显示的文本。若设置了 <see cref="ButtonContent"/>，则被忽略。
    /// </summary>
    [Parameter]public string? ButtonText { get; set; }
    /// <summary>
    /// 按钮的主题。
    /// </summary>
    [Parameter] public Theme? ButtonTheme { get; set; } = Theme.Primary;
    /// <summary>
    /// 按钮的风格。
    /// </summary>
    [Parameter] public ButtonVarient ButtonVarient { get; set; } = ButtonVarient.Base;
    /// <summary>
    /// 当对话框关闭后要触发的事件。
    /// </summary>
    [Parameter]public EventCallback<Task<DialogResult>> OnDialogClosed { get; set; }
    protected override void OnInitialized()
    {
        ButtonContent ??= builder => builder.AddContent(0, ButtonText);
        ModelProvider ??= () => Task.FromResult(new TModel());
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.Component<TButton>()
            .Attribute(nameof(TButton.Varient), ButtonVarient)
            .Attribute(nameof(TButton.Theme), ButtonTheme)
            .Attribute(nameof(TButton.OnClick), HtmlHelper.Instance.Callback().Create<MouseEventArgs>(this, async () =>
            {
                var model = await ModelProvider!.Invoke();

                var parameters = new DialogParameters
                {
                    ["Model"] = model,
                    ["FormContent"] = FormContent,
                    ["DialogTitle"] = DialogTitle,
                    ["OnSubmit"] = OnSubmit
                };
                var dialog = await DialogService.Open<FormDialogTemplate<TModel>>(parameters);
                await OnDialogClosed.InvokeAsync(dialog.Result);
            }))
            .ChildContent(ButtonContent)
            .Close();
    }
}
