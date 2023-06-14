using TDesign.Pro.Abstractions;

namespace TDesign.Pro.Components;
partial class CrudDialogTable<TCreate, TUpdate, TDetail, TList, TSearch>
{
    /// <summary>
    /// 页面的标题。
    /// </summary>
    [ParameterApiDoc("页面显示的标题",Value = "无标题页面")]
    [Parameter] public string? PageTitle { get; set; } = "无标题页面";

    #region 搜索相关的参数
    /// <summary>
    /// 搜索表单部分的代码片段。
    /// </summary>
    [ParameterApiDoc("搜索表单部分的代码片段", Type = "RenderFragment<TSearch>?")]
    [Parameter]public RenderFragment<TSearch>? SearchFormContent { get; set; }
    /// <summary>
    /// 提交搜索行为的名称。默认是【查询】。
    /// </summary>
    [ParameterApiDoc("提交搜索行为的名称", Value = "查询")]
    [Parameter] public string? SearchSubmitActionName { get; set; } = "查询";
    /// <summary>
    /// 重置搜索表单行为的名称。默认是【重置】。
    /// </summary>
    [ParameterApiDoc("重置搜索表单行为的名称", Value = "重置")]
    [Parameter] public string? SearchResetActionName { get; set; } = "重置";

    /// <summary>
    /// 搜索表单部分查询功能的代码片段。默认是呈现提交按钮。
    /// </summary>
    [ParameterApiDoc("搜索表单部分查询功能的代码片段。默认是呈现提交按钮")]
    [Parameter]public RenderFragment? SearchFormSubmitContent { get; set; }
    /// <summary>
    ///搜索表单部分重置功能的代码片段。默认是呈现重置按钮。
    /// </summary>
    [ParameterApiDoc("搜索表单部分重置功能的代码片段。默认是呈现重置按钮")]
    [Parameter]public RenderFragment? SearchFormResetContent { get; set; }

    #endregion

    #region 表格相关的参数
    /// <summary>
    /// 设置获取列表数据源的委托。
    /// </summary>
    [ParameterApiDoc("获取列表数据源的委托", Required = true, Type = "Func<TSearch, Task<DataSource<TList>>>")]
    [Parameter][EditorRequired] public Func<TSearch, Task<DataSource<TList>>> ListDataSourceProvider { get; set; }

    /// <summary>
    /// 设置表格列的内容。
    /// </summary>
    [ParameterApiDoc("表格列的内容")]
    [Parameter]public RenderFragment? TableColumnContent { get; set; }

    /// <summary>
    /// 获取单个数据源的委托。该委托用于更新前获取数据或详情数据，输入参数为数据源的主键。
    /// </summary>
    [ParameterApiDoc("获取单个数据源的委托，用于更新前获取数据或详情数据，输入参数为数据源的主键。",Type = "Func<object, Task<TDetail?>>?")]
    [Parameter]public Func<object, Task<TDetail?>>? DetailDataSourceProvider { get; set; }

    /// <summary>
    /// 设置列表的主键，用于更新和删除操作。
    /// </summary>
    [ParameterApiDoc("用于更新和删除操作的列表主键字段，例如 () => context.Id", Required = true,Type = "Func<TList?, object>")]
    [Parameter][EditorRequired] public Func<TList?, object>? Key { get; set; }
    #endregion

    #region 创建相关的参数
    /// <summary>
    /// 设置创建操作的名称，默认是【创建】。
    /// </summary>
    [ParameterApiDoc("设置创建操作的名称，默认是【创建】。将显示在操作按钮、对话框标题上")]
    [Parameter] public string? CreateActionName { get; set; } = "创建";
    /// <summary>
    /// 创建操作的图标。
    /// </summary>
    [ParameterApiDoc("创建操作的图标",Value = "IconName.Add")]
    [Parameter] public object? CreateActionIcon { get; set; } = IconName.Add;
    /// <summary>
    /// 设置创建操作的表单内容。
    /// </summary>
    [ParameterApiDoc("创建操作的表单内容",Type = "RenderFragment<TCreate>?")]
    [Parameter]public RenderFragment<TCreate>? CreateFormContent { get; set; }
    /// <summary>
    /// 当表单的验证成功后，提交创建时触发的回调方法。
    /// </summary>
    [ParameterApiDoc("当表单的验证成功后，提交创建时触发的回调方法", Type = "EventCallback<TCreate>")]
    [Parameter]public EventCallback<TCreate> OnFormCreating { get; set; }
    /// <summary>
    /// 当创建对话框关闭后触发的事件。
    /// </summary>
    [ParameterApiDoc("当创建对话框关闭后触发的事件", Type = "EventCallback<DialogResult>")]
    [Parameter]public EventCallback<DialogResult> OnFormCreated { get; set; }
    /// <summary>
    /// 返回一个布尔值，表示创建权限的逻辑方法。
    /// </summary>
    /// <value>有权限，则返回 <c>true</c>；否则返回 <c>false</c>。</value>
    [ParameterApiDoc("创建权限的逻辑方法", Type = "Func<Task<bool>>")]
    [Parameter] public Func<Task<bool>> CreatePermissionProvider { get; set; } = () => Task.FromResult(true);
    #endregion

    #region 更新相关的参数
    /// <summary>
    /// 设置编辑操作的名称，默认是【编辑】。
    /// </summary>
    [ParameterApiDoc("更新操作的名称，默认是【编辑】。将显示在操作按钮、对话框标题上")]
    [Parameter] public string? UpdateActionName { get; set; } = "编辑";
    /// <summary>
    /// 设置编辑操作的图标，默认是 <see cref="IconName.Edit"/>。
    /// </summary>
    [ParameterApiDoc("更新操作的图标", Value = "IconName.Edit")]
    [Parameter] public object? UpdateActionIcon { get; set; } = IconName.Edit;
    /// <summary>
    /// 设置编辑操作的表单内容。
    /// </summary>
    [ParameterApiDoc("更新操作的表单内容", Type = "RenderFragment<TUpdate>?")]
    [Parameter]public RenderFragment<TUpdate>? UpdateFormContent { get; set; }
    /// <summary>
    /// 编辑操作的代码片段。
    /// </summary>
    [ParameterApiDoc("更新操作的代码片段，使用 context 可以获得当前行对象，可自定义编辑的操作代码片段", Type = "RenderFragment<TList?>?")]
    [Parameter]public RenderFragment<TList?>? UpdateOperationContent { get; set; }
    /// <summary>
    /// 设置一个回调方法，当编辑表单提交更新操作时触发的事件。
    /// </summary>
    [ParameterApiDoc("当编辑表单提交更新操作时触发的事件", Type = "EventCallback<TUpdate>")]
    [Parameter] public EventCallback<TUpdate> OnFormUpdating { get; set; }
    /// <summary>
    /// 设置一个回调方法，当编辑表单窗口关闭后触发的事件。
    /// </summary>
    [ParameterApiDoc("当编辑表单窗口关闭后触发的事件", Type = "EventCallback<DialogResult>")]
    [Parameter] public EventCallback<DialogResult> OnFormUpdated { get; set; }
    /// <summary>
    /// 返回一个布尔值，表示更新权限的逻辑方法。
    /// </summary>
    /// <value>有权限，则返回 <c>true</c>；否则返回 <c>false</c>。</value>
    [ParameterApiDoc("更新权限的逻辑方法", Type = "Func<Task<bool>>")]
    [Parameter] public Func<Task<bool>> UpdatePermissionProvider { get; set; } = () => Task.FromResult(true);

    /// <summary>
    /// 设置 <typeparamref name="TDetail"/> 映射到 <typeparamref name="TUpdate"/> 模型的方法。
    /// </summary>
    [ParameterApiDoc("从 TDetail 到 TUpdate 实体的映射逻辑，默认 TDetail as TUpdate，如果 TDetail 和 TUpdate 不是一个对象，会返回 null", Type = "Func<TDetail?, TUpdate?>")]
    [Parameter] public Func<TDetail?, TUpdate?> MapDetailToUpdateProvider { get; set; } = (detail) => detail as TUpdate;
    #endregion

    #region 删除相关的参数
    /// <summary>
    /// 设置删除操作的名称，默认是【删除】。
    /// </summary>
    [ParameterApiDoc("删除操作的名称，默认是【删除】。将显示在操作按钮、对话框标题上")]
    [Parameter] public string? DeleteActionName { get; set; } = "删除";
    /// <summary>
    /// 设置删除操作的图标，默认是 <see cref="IconName.Delete"/>。
    /// </summary>
    [ParameterApiDoc("删除操作的图标", Value = "IconName.Delete")]
    [Parameter] public object? DeleteActionIcon { get; set; } = IconName.Delete;
    /// <summary>
    /// 返回一个布尔值，表示删除权限的逻辑方法。
    /// </summary>
    /// <value>有权限，则返回 <c>true</c>；否则返回 <c>false</c>。</value>
    [ParameterApiDoc("删除权限的逻辑方法", Type = "Func<Task<bool>>")]
    [Parameter] public Func<Task<bool>> DeletePermissionProvider { get; set; } = () => Task.FromResult(true);
    /// <summary>
    /// 设置删除操作的任意内容。
    /// </summary>
    [ParameterApiDoc("删除操作的代码片段，使用 context 可以获得当前行对象，可自定义编辑的操作代码片段", Type = "RenderFragment<TList?>?")]
    [Parameter]public RenderFragment<TList>? DeleteOperationContent { get; set; }
    /// <summary>
    /// 设置当点击【确定】后触发的回调方法。
    /// </summary>
    [ParameterApiDoc("当点击【确定】后触发的回调方法", Type = "EventCallback<TList>")]
    [Parameter]public EventCallback<TList> OnConfirmDeleting { get; set; }

    /// <summary>
    /// 设置删除提示的消息。
    /// </summary>
    [Parameter] public Func<TList, string>? DeleteMessageProvider { get; set; } = (item) => "你确定要删除吗";

    #endregion

    /// <summary>
    /// 表示行为工具栏的集合提供器。
    /// </summary>
    [ParameterApiDoc("定义行为工具栏的UI内容集合。默认根据 CreatePermissionProvider 增加【创建】操作按钮", Type = "Action<ICollection<RenderFragment?>>?")]
    [Parameter]public Action<ICollection<RenderFragment?>>? ActionToolsProvider { get; set; }
}
