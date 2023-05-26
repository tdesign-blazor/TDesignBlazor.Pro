namespace TDesign.Admin;

partial class CrudTableDialogTemplate<TKey, TCreate, TUpdate, TList>
{
    /// <summary>
    /// 设置创建时的对话框内容。
    /// </summary>
    [Parameter]public RenderFragment<TCreate?>? CreateDialogContent { get; set; }
    /// <summary>
    /// 设置更新时的对话框内容。
    /// </summary>
    [Parameter]public RenderFragment<TUpdate?>? UpdateDialogContent { get; set; }

    [Parameter]public RenderFragment? ListContent { get; set; }

    #region 一般参数
    /// <summary>
    /// 页面的标题。
    /// </summary>
    [Parameter]public string? PageTitle { get; set; }
    [Parameter] public string? CreateText { get; set; } = "新增";
    [Parameter] public object? CreateIconName { get; set; } = IconName.AddRectangle;
    #endregion

    #region 对话框设置
    [Parameter] public string? CreateDialogCancelText { get; set; } = "取消";
    [Parameter] public string? CreateDialogSubmitText { get; set; } = "创建";
    [Parameter] public string? UpdateDialogTitle { get; set; } = "更新";
    [Parameter] public string? UpdateDialogCancelText { get; set; } = "取消";
    [Parameter] public string? UpdateDialogSubmitText { get; set; } = "保存";
    #endregion

    #region 行为参数
    /// <summary>
    /// 设置数据源提供器。
    /// </summary>
    [Parameter][EditorRequired]public Func<Task<DataSource<TList>>> DataSourceProvider { get; set; }
    /// <summary>
    /// 设置创建数据的函数。
    /// </summary>
    [Parameter]public Func<TCreate?,Task>? CreateFuncProvider { get; set; }
    #endregion
}
