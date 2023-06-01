namespace TDesign.Admin.Abstractions;

public abstract class CrudPageBase<TKey, TCreate, TUpdate, TDetail, TList, TListFilter> : TDesignPageBase
    where TCreate : class
    where TUpdate : class
    where TList : class
{
    protected abstract Task CreateAsync(TCreate model);
    protected abstract Task UpdateAsync(TKey key, TUpdate model);
    protected abstract Task DeleteAsync(TKey key);
}
