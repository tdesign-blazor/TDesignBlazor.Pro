namespace TDesign.Pro.Components;

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
                                            .Attribute(m => m.DialogTitle, EditDialogTitle)
                                            .Attribute(m => m.Text, EditActionName)
                                            .Attribute(m => m.IconName, EditActionIcon)
                                            .Attribute(m => m.OnSubmit, OnFormUpdating)
                                            .Attribute(m => m.OnDialogClosed, HtmlHelper.Instance.Callback().Create<Task<DialogResult>>(this, CloseUpdateForm))
                                            .Attribute(m => m.ModelProvider, async () =>
                                            {
                                                var detail = await GetDetailData(rowValue, Key);
                                                var update = MapDetailToUpdateProvider(detail);
                                                return update!;
                                            })
                                            .Attribute(m => m.ChildContent, model => async builder =>
                                            {
                                                var detail = await GetDetailData(rowValue, Key);
                                                var update = MapDetailToUpdateProvider(detail);
                                                builder.AddContent(0, EditFormContent?.Invoke(update!));
                                            })
                                            .Close();

        DeleteOperationContent = rowValue =>
                            builder => builder.Component<TLink>()
                                            .Attribute(m => m.Hover, LinkHover.Underline)
                                            .Attribute(m => m.Theme, Theme.Danger)
                                            .Attribute(m=>m.ChildContent, builder => builder.Component<TIcon>(DeleteActionIcon is not null).Attribute(nameof(TIcon.Name), DeleteActionIcon).Close().Content(DeleteActionName))
                                            .Attribute("onclick", HtmlHelper.Instance.Callback().Create(this, () => ShowDialogAndCofirmToDelete(rowValue)))
                                            .Close();

        ListFilterFormSubmitContent = builder => builder.Component<TButton>()
                                                        .Attribute(m => m.Theme,Theme.Primary)
                                                        .Attribute(m => m.HtmlType,ButtonHtmlType.Submit)
                                                        .Attribute(m => m.Block,true)
                                                        .Content("查询")
                                                        .Close();

        ListFilterFormResetContent = builder => builder.Component<TButton>()
                                                        .Attribute(m => m.Theme, Theme.Default)
                                                        .Attribute(m => m.HtmlType, ButtonHtmlType.Reset)
                                                        .Attribute(m => m.Block,true)
                                                        .Content("重置")
                                                        .Close();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if ( firstRender )
        {
            InitialActionTools();
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
        ActionTools ??= new();
        ActionTools.Add(BuildCreateActionButton());

        ActionToolsProvider?.Invoke(ActionTools);
        StateHasChanged();

        RenderFragment? BuildCreateActionButton()
        => builder => builder.Component<FormDialogButton<TCreate>>(CreatePermissionProvider())
                                .Attribute(m => m.DialogTitle, CreateDialogTitle)
                                .Attribute(m => m.ButtonContent, (RenderFragment)(content => content.Component<TIcon>().Attribute(m => m.Name, CreateActionIcon).Close()))
                                .Attribute(m=>m.OnSubmit, OnFormCreating)
                                .Attribute(m => m.OnDialogClosed, HtmlHelper.Instance.Callback().Create<Task<DialogResult>>(this, CloseCreateForm))
                                .Attribute(m => m.ChildContent, model => builder => builder.AddContent(0, CreateFormContent, model))
                                .Attribute("title", CreateActionName)
                            .Close();
    }
}
