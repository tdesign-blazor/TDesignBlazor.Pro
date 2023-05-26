using Microsoft.AspNetCore.Components.Forms;

namespace TDesign.Admin;

/// <summary>
/// 具有 <see cref="TTable{TItem}"/> 列表渲染和 <see cref="TDialog"/> 弹出框组成的 CRUD 功能的模板。
/// </summary>
/// <typeparam name="TKey">主键的类型。</typeparam>
/// <typeparam name="TCreate">创建模型的类型。</typeparam>
/// <typeparam name="TUpdate">更新模型的类型。</typeparam>
/// <typeparam name="TList">列表模型的类型。</typeparam>
partial class CrudTableDialogTemplate<TKey, TCreate, TUpdate, TList>
    where TCreate : class,new()
    where TUpdate : class, new()
    where TList : class, new()
{

    /// <summary>
    /// 表示更新时的对话框。
    /// </summary>
    TDialog? RefUpdateDialog { get; set; }

    /// <summary>
    /// 更新模型。
    /// </summary>
    TUpdate? UpdateModel { get; set; } = new();

    /// <summary>
    /// 更新时的表单验证上下文。
    /// </summary>
    EditContext? UpdateEditContext { get; set; }

    DataSource<TList> DataSource { get; set; }

    protected override void OnInitialized()
    {
        if(DataSourceProvider is null )
        {
            throw new InvalidOperationException($"{nameof(DataSourceProvider)} 不能是空");
        }

        UpdateEditContext = new(UpdateModel);
        CreateEditContext = new(CreateModel);
    }

    protected override async Task OnInitializedAsync()
    {
        DataSource = await DataSourceProvider();
    }
}
