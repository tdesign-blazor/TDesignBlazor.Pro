namespace TDesign.Admin.Abstractions;

/// <summary>
/// 表示具备 CRUD 功能页面的基类。
/// </summary>
public abstract class CrudPageBase : TDesignPageBase
{
    [Parameter]public RenderFragment<object?>? CreateActionContent { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        SetDefaultCreateContent();
    }


    object? CreateModel { get; set; }

    #region 默认值
    void SetDefaultCreateContent()
    {
        CreateActionContent = value => builder => builder.Component<TButton>()
                                                            .Attribute(nameof(TButton.Theme),Theme.Primary)
                                                            //.Attribute(nameof(TButton.OnClick),HtmlHelper.Instance.Callback().Create())
                                                            
                                                        .Close();
    }
    #endregion
}

public abstract class CrudPageBase<TKey, TCreate, TUpdate, TList> : CrudPageBase
    where TCreate : class
    where TUpdate : class
    where TList : class
{
    protected abstract Task CreateAsync(TCreate model);
    protected abstract Task UpdateAsync(TKey key, TUpdate model);
    protected abstract Task DeleteAsync(TKey key);
}
