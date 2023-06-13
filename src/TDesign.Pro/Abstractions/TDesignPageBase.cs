using Microsoft.Extensions.DependencyInjection;

namespace TDesign.Pro;

/// <summary>
/// 表示页面基类。
/// </summary>
public abstract class TDesignPageBase : ComponentBase
{
    /// <summary>
    /// <see cref="IServiceProvider"/> 实例。
    /// </summary>
    [Inject] protected IServiceProvider ServiceProvider { get; set; }
    /// <summary>
    /// 获取消息通知服务。
    /// </summary>
    protected INotificationService NotificationService => ServiceProvider.GetRequiredService<INotificationService>();
    /// <summary>
    /// 获取对话框服务。
    /// </summary>
    protected IDialogService DialogService => ServiceProvider.GetRequiredService<IDialogService>();
}
