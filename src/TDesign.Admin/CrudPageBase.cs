namespace TDesign.Admin;

/// <summary>
/// 表示具备 CRUD 功能页面的基类。
/// </summary>
public abstract class CrudPageBase:ComponentBase
{
    [Inject] protected INotificationService NotificationService { get; private set; }
    [Inject]protected IServiceProvider ServiceProvider { get; private set; }
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
