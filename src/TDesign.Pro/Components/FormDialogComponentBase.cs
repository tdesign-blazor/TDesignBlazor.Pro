using TDesign.Pro.Templates;

namespace TDesign.Pro.Components;

/// <summary>
/// 提供组件用于弹出表单对话框的基类。
/// </summary>
/// <typeparam name="TModel">表单模型的类型。</typeparam>
public abstract class FormDialogComponentBase<TModel>: ComponentBase where TModel:class,new ()
{
    /// <summary>
    /// 对话框服务。
    /// </summary>
    [Inject] IDialogService DialogService { get; set; }
    /// <summary>
    /// 表单的内容。
    /// </summary>
    [Parameter][EditorRequired] public RenderFragment<TModel>? ChildContent { get; set; }
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
    /// 当对话框关闭后要触发的事件。
    /// </summary>
    [Parameter] public EventCallback<Task<DialogResult>> OnDialogClosed { get; set; }

    /// <summary>
    /// 设置一个委托，是否显示对话框。
    /// </summary>
    [Parameter] public Func<bool> ShowDialog { get; set; } = () => true;

    [Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object> AdditionalAttributes { get; set; } = new Dictionary<string, object>();

    protected override void OnInitialized()
    {
        ModelProvider ??= () => Task.FromResult(new TModel());
    }

    /// <summary>
    /// 打开对话框。
    /// </summary>
    protected async Task OpenDialog()
    {
        if ( !ShowDialog() )
        {
            return;
        }

        var model = await ModelProvider!.Invoke();

        var parameters = new DialogParameters
        {
            ["Model"] = model,
            ["FormContent"] = ChildContent,
            ["DialogTitle"] = DialogTitle,
            ["OnSubmit"] = OnSubmit
        };
        var dialog = await DialogService.Open<FormDialogTemplate<TModel>>(parameters);
        await OnDialogClosed.InvokeAsync(dialog.Result);
    }
}
