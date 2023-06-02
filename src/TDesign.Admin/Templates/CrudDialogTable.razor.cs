using TDesign.Admin.Components;

namespace TDesign.Admin.Templates;

/// <summary>
/// 具备 CRUD 功能的表格，将使用 <see cref="TDialog"/> 对话框来打开表单。
/// </summary>
/// <typeparam name="TCreate">创建模型的类型。</typeparam>
/// <typeparam name="TUpdate">更新模型的类型。</typeparam>
/// <typeparam name="TDetail">详情模型的类型。</typeparam>
/// <typeparam name="TList">列表模型的类型。</typeparam>
/// <typeparam name="TListFilter">列表过滤的类型。</typeparam>
partial class CrudDialogTable<TCreate, TUpdate, TDetail, TList, TListFilter>
    where TCreate : class, new()
    where TUpdate : class, new()
    where TList : class, new()
    where TDetail : class, new()
    where TListFilter : class, new()
{
    /// <summary>
    /// 创建对话框的标题。
    /// </summary>
    internal string? CreateDialogTitle => $"{CreateActionName}{PageTitle}";
    /// <summary>
    /// 删除对话框的标题。
    /// </summary>
    internal string? DeleteDialogTitle => $"{DeleteActionName}{PageTitle}";
    /// <summary>
    /// 编辑对话框的标题。
    /// </summary>
     string? EditDialogTitle => $"{EditActionName}{PageTitle}";
     TListFilter FilterModel { get; set; } = new();
     DataSource<TList> TableDataSource { get; set; }
     TDetail? DetailDataSource { get; set; }

    TListFilter FielterModel { get; set; } = new();
    List<RenderFragment?> ActionTools { get; set; } = new();

    protected override void OnInitialized()
    {
        if ( Key is null )
        {
            throw new ArgumentNullException(nameof(Key));
        }

        EditOperationContent = rowValue =>
                            builder => builder.Component<FormDialogLink<TUpdate>>()
                                            .Attribute(nameof(FormDialogLink<TUpdate>.DialogTitle), EditDialogTitle)
                                            .Attribute(nameof(FormDialogLink<TUpdate>.Text), EditActionName)
                                            .Attribute(nameof(FormDialogLink<TUpdate>.IconName), EditActionIcon)
                                            .Attribute(nameof(FormDialogLink<TUpdate>.OnSubmit), OnFormUpdating)
                                            .Attribute(nameof(FormDialogLink<TUpdate>.OnDialogClosed), HtmlHelper.Instance.Callback().Create<Task<DialogResult>>(this, CloseUpdateForm))
                                            .Attribute(nameof(FormDialogLink<TUpdate>.ModelProvider), () => GetDetailData(rowValue, Key))
                                            .Attribute(nameof(FormDialogLink<TUpdate>.ChildContent), (RenderFragment<TUpdate>)(model => async builder =>
                                            {
                                                var detail = await GetDetailData(rowValue, Key);
                                                var update = MapDetailToUpdateProvider(detail);
                                                builder.AddContent(0, EditFormContent?.Invoke(update!));
                                            }))
                                            .Close();

        DeleteOperationContent = rowValue =>
                            builder => builder.Component<TLink>()
                                            .Attribute(nameof(TLink.Hover), LinkHover.Underline)
                                            .Attribute(nameof(TLink.Theme), Theme.Danger)
                                            .Attribute("onclick",HtmlHelper.Instance.Callback().Create(this,()=> ShowDialogAndCofirmToDelete(rowValue)))
                                            .ChildContent(builder => builder.Component<TIcon>(DeleteActionIcon is not null).Attribute(nameof(TIcon.Name), DeleteActionIcon).Close().Content(DeleteActionName))
                                            .Close();

        ListFilterFormSubmitContent = builder => builder.Component<TButton>()
                                                        .Attribute(nameof(TButton.Theme),Theme.Primary)
                                                        .Attribute(nameof(TButton.HtmlType),ButtonHtmlType.Submit)
                                                        .Attribute(nameof(TButton.Block),true)
                                                        .ChildContent("查询")
                                                        .Close();

        ListFilterFormResetContent = builder => builder.Component<TButton>()
                                                        .Attribute(nameof(TButton.Theme), Theme.Default)
                                                        .Attribute(nameof(TButton.HtmlType), ButtonHtmlType.Reset)
                                                        .Attribute(nameof(TButton.Block),true)
                                                        .ChildContent("重置")
                                                        .Close();
    }

    protected override void OnParametersSet()
    {
        InitialActionTools();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if ( firstRender )
        {
            await GetListData();
        }
    }
    async Task GetListData()
    {
        TableDataSource = await ListDataSourceProvider.Invoke(FilterModel);
        StateHasChanged();
    }

    Task<TDetail?> GetDetailData(TList? item, Func<TList?, object> key)
    {
        if ( DetailDataSourceProvider is null )
        {
            throw new InvalidOperationException($"只有设置了 {nameof(DetailDataSourceProvider)} 才可以获取详情的操作");
        }

        return DetailDataSourceProvider.Invoke(key(item));
    }

    async Task RefreshListData(Task<DialogResult> result)
    {
        var resultData = await result;
        if ( !resultData.Cancelled )
        {
            await GetListData();
        }
    }

    Task CloseUpdateForm(Task<DialogResult> result)
    {
        return RefreshListData(result).ContinueWith(t => OnFormUpdated);
    }

    Task CloseCreateForm(Task<DialogResult> result)
    {
        return RefreshListData(result).ContinueWith(t => OnFormCreated);
    }

    async Task ShowDialogAndCofirmToDelete(TList item)
    {
        var deleteMessage = DeleteMessageProvider?.Invoke(item);

        var dialog = await DialogService.Open<TDesign.Templates.ConfirmationDialogTemplate>(deleteMessage, DeleteDialogTitle, IconName.InfoCircle, Theme.Primary);
        var result = await dialog.Result;
        if ( !result.Cancelled )
        {
            await OnConfirmDeleting!.InvokeAsync(item);
            await GetListData();
        }
    }

    void InitialActionTools()
    {
        ActionTools.Add(BuildCreateActionButton());

        ActionToolsProvider?.Invoke(ActionTools);


        RenderFragment? BuildCreateActionButton()
        => builder => builder.Component<FormDialogButton<TCreate>>(CreatePermissionProvider())
                                .Attribute(nameof(FormDialogButton<TCreate>.DialogTitle), CreateDialogTitle)
                                .Attribute("title", CreateActionName)
                                .Attribute(nameof(FormDialogButton<TCreate>.ButtonContent), (RenderFragment)(content => content.Component<TIcon>().Attribute(nameof(TIcon.Name), CreateActionIcon).Close()))
                                .Attribute(nameof(FormDialogButton<TCreate>.OnSubmit), OnFormCreating)
                                .Attribute(nameof(FormDialogButton<TCreate>.OnDialogClosed), HtmlHelper.Instance.Callback().Create<Task<DialogResult>>(this, CloseCreateForm))
                                .ChildContent(CreateFormContent?.Invoke(new()))
                            .Close();
    }
}
