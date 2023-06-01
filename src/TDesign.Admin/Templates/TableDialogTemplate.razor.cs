using System.Linq.Expressions;
using TDesign.Admin.Components;

namespace TDesign.Admin.Templates;
partial class TableDialogTemplate<TCreate, TUpdate, TDetail, TList, TListFilter> : ComponentBase
    where TCreate : class,new()
    where TUpdate : class,new()
    where TList : class, new()
    where TDetail : class,new()
    where TListFilter : class, new()
{
    internal DataSource<TList> TableDataSource { get; set; }
    internal TDetail? DetailDataSource { get; set; }

    protected override void OnInitialized()
    {
        if(Key is null )
        {
            throw new ArgumentNullException(nameof(Key));
        }

        EditOperationContent = rowValue => 
                            builder => builder.Component<FormDialogLink<TUpdate>>()
                                            .Attribute(nameof(FormDialogLink<TUpdate>.DialogTitle),$"{EditActionName}{PageTitle}")
                                            .Attribute(nameof(FormDialogLink<TUpdate>.Text),EditActionName)
                                            .Attribute(nameof(FormDialogLink<TUpdate>.IconName), EditIcon)
                                            .Attribute(nameof(FormDialogLink<TUpdate>.OnSubmit), OnFormUpdating)
                                            .Attribute(nameof(FormDialogLink<TUpdate>.OnDialogClosed), HtmlHelper.Instance.Callback().Create<Task<DialogResult>>(this, SubmittedUpdateForm))
                                            .Attribute(nameof(FormDialogLink<TUpdate>.ModelProvider),()=> GetDetailData(rowValue,Key))
                                            .Attribute(nameof(FormDialogLink < TUpdate >.ChildContent ),(RenderFragment<TUpdate>)(model=> async builder =>
                                            {
                                                var detail = await GetDetailData(rowValue, Key);
                                                var update = MapDetailToUpdateProvider(detail);
                                                builder.AddContent(0, EditFormContent?.Invoke(update!));
                                            }))
                                            .Close();
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
        TableDataSource = await ListDataSourceProvider.Invoke();
        StateHasChanged();
    }

    Task<TDetail?> GetDetailData(TList? item, Func<TList?,object> key)
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
    
    Task SubmittedUpdateForm(Task<DialogResult> result)
    {
        return RefreshListData(result).ContinueWith(t => OnFormUpdated);
    }

    Task SubmittedCreateForm(Task<DialogResult> result)
    {
        return RefreshListData(result).ContinueWith(t => OnFormCreated);
    }
}
