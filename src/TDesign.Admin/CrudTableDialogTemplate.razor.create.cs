using Microsoft.AspNetCore.Components.Forms;

namespace TDesign.Admin;
partial class CrudTableDialogTemplate<TKey, TCreate, TUpdate, TList>
{
    #region 创建
    /// <summary>
    /// 创建对话框的标题。
    /// </summary>
    string? CreateDialogTitle => $"{CreateText}{PageTitle}";
    /// <summary>
    /// 表示创建时的对话框。
    /// </summary>
    TDialog? RefCreateDialog { get; set; }
    /// <summary>
    /// 创建模型。
    /// </summary>
    TCreate? CreateModel { get; set; } = new();
    /// <summary>
    /// 创建时的表单验证上下文。
    /// </summary>
    EditContext? CreateEditContext { get; set; }

    public IEnumerable<string?> CreateErrorMessages { get; private set; } = Enumerable.Empty<string?>();

    /// <summary>
    /// 打开创建对话框。
    /// </summary>
    public Task OpenCreateDialog()
    {
        if(RefCreateDialog is null )
        {
            throw new InvalidOperationException($"{nameof(RefCreateDialog)}的组件引用是 null");
        }

        return RefCreateDialog.Activate();
    }

    /// <summary>
    /// 关闭创建对话框。
    /// </summary>
    public Task? CloseCreateDialog() => RefCreateDialog?.Activate(false);

    public Task? Create()
    {
        if ( CreateEditContext is null )
        {
            throw new ArgumentNullException(nameof(CreateEditContext));
        }

        if ( CreateFuncProvider is null )
        {
            throw new InvalidOperationException($"必须要设置{nameof(CreateFuncProvider)}参数，以提供创建数据的逻辑方法");
        }

        var valid = CreateEditContext.Validate();
        if ( !valid )
        {
            CreateErrorMessages = CreateEditContext.GetValidationMessages();
            return Task.CompletedTask;
        }

        return CreateFuncProvider?.Invoke(CreateModel);
    }

    #endregion
}
