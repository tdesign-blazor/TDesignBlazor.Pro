namespace TDesign.Admin.Templates;
partial class CrudDialogTable<TCreate, TUpdate, TDetail, TList, TListFilter>
{
    /// <summary>
    /// 页面的标题。
    /// </summary>
    [Parameter] public string? PageTitle { get; set; } = "未标题页面";

    #region 搜索相关的参数
    /// <summary>
    /// 列表过滤的表单任意内容。
    /// </summary>
    [Parameter]public RenderFragment<TListFilter>? ListFilterFormContent { get; set; }

    /// <summary>
    /// 列表过滤表单的查询提交任意内容。默认是提交按钮。
    /// </summary>
    [Parameter]public RenderFragment? ListFilterFormSubmitContent { get; set; }
    /// <summary>
    ///列表过滤表单的重置任意内容。默认是重置按钮。
    /// </summary>
    [Parameter]public RenderFragment? ListFilterFormResetContent { get; set; }
    
    #endregion

    #region 表格相关的参数
    /// <summary>
    /// 设置获取列表数据源的委托。
    /// </summary>
    [Parameter][EditorRequired] public Func<TListFilter, Task<DataSource<TList>>> ListDataSourceProvider { get; set; }

    [Parameter]public RenderFragment? TableColumnContent { get; set; }

    /// <summary>
    /// 获取单个数据源的委托。该委托用于更新前获取数据或详情数据。
    /// </summary>
    [Parameter]public Func<object, Task<TDetail?>>? DetailDataSourceProvider { get; set; }

    /// <summary>
    /// 设置列表的主键，用于更新和删除操作。
    /// </summary>
    [Parameter][EditorRequired] public Func<TList?, object>? Key { get; set; }
    #endregion

    #region 创建相关的参数
    /// <summary>
    /// 设置创建操作的名称，默认是【创建】。
    /// </summary>
    [Parameter] public string? CreateActionName { get; set; } = "创建";
    /// <summary>
    /// 设置创建操作的表单内容。
    /// </summary>
    [Parameter]public RenderFragment<TCreate>? CreateFormContent { get; set; }
    /// <summary>
    /// 当表单的验证成功后，提交创建时触发的回调方法。
    /// </summary>
    [Parameter]public EventCallback<TCreate> OnFormCreating { get; set; }
    /// <summary>
    /// 当创建对话框关闭后触发的事件。
    /// </summary>
    [Parameter]public EventCallback<DialogResult> OnFormCreated { get; set; }
    /// <summary>
    /// 返回一个布尔值，表示创建权限的逻辑方法。
    /// </summary>
    /// <value>有权限，则返回 <c>true</c>；否则返回 <c>false</c>。</value>
    [Parameter] public Func<bool> CreatePermissionProvider { get; set; } = () => true;
    [Parameter] public object? CreateIcon { get; set; } = IconName.Add;
    #endregion

    #region 更新相关的参数
    /// <summary>
    /// 设置编辑操作的名称，默认是【编辑】。
    /// </summary>
    [Parameter] public string? EditActionName { get; set; } = "编辑";
    /// <summary>
    /// 设置编辑操作的表单内容。
    /// </summary>
    [Parameter]public RenderFragment<TUpdate>? EditFormContent { get; set; }
    /// <summary>
    /// 设置编辑操作的图标，默认是 <see cref="IconName.Edit"/>。
    /// </summary>
    [Parameter] public object? EditIcon { get; set; } = IconName.Edit;
    /// <summary>
    /// 编辑操作的任意内容。
    /// </summary>
    [Parameter]public RenderFragment<TList?>? EditOperationContent { get; set; }
    /// <summary>
    /// 设置一个回调方法，当编辑表单提交更新操作时触发的事件。
    /// </summary>
    [Parameter] public EventCallback<TUpdate> OnFormUpdating { get; set; }
    /// <summary>
    /// 设置一个回调方法，当编辑表单窗口关闭后触发的事件。
    /// </summary>
    [Parameter] public EventCallback<DialogResult> OnFormUpdated { get; set; } 
    /// <summary>
    /// 返回一个布尔值，表示编辑权限的逻辑方法。
    /// </summary>
    /// <value>有权限，则返回 <c>true</c>；否则返回 <c>false</c>。</value>
    [Parameter] public Func<bool> EditPermissionProvider { get; set; } = () => true;

    /// <summary>
    /// 设置 <typeparamref name="TDetail"/> 映射到 <typeparamref name="TUpdate"/> 模型的方法。
    /// </summary>
    [Parameter] public Func<TDetail?, TUpdate?> MapDetailToUpdateProvider { get; set; } = (detail) => detail as TUpdate;
    #endregion

    #region 删除相关的参数
    /// <summary>
    /// 返回一个布尔值，表示删除权限的逻辑方法。
    /// </summary>
    /// <value>有权限，则返回 <c>true</c>；否则返回 <c>false</c>。</value>
    [Parameter] public Func<bool> DeletePermissionProvider { get; set; } = () => true;
    /// <summary>
    /// 设置删除操作的任意内容。
    /// </summary>
    [Parameter]public RenderFragment<TList>? DeleteOperationContent { get; set; }
    /// <summary>
    /// 当点击确定后的删除方法。
    /// </summary>
    [Parameter]public EventCallback<TList> OnConfirmDeleting { get; set; }
    /// <summary>
    /// 设置删除操作的图标，默认是 <see cref="IconName.Delete"/>。
    /// </summary>
    [Parameter] public object? DeleteIcon { get; set; } = IconName.Delete;
    /// <summary>
    /// 设置删除操作的名称，默认是【删除】。
    /// </summary>
    [Parameter] public string? DeleteActionName { get; set; } = "删除";

    /// <summary>
    /// 设置删除提示的消息。
    /// </summary>
    [Parameter] public Func<TList, string>? DeleteMessageProvider { get; set; } = (item) => "你确定要删除吗";

    #endregion

    [Parameter]public Action<ICollection<RenderFragment?>>? ActionToolsProvider { get; set; }
}
