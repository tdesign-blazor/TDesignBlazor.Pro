using Microsoft.Extensions.DependencyInjection;

namespace TDesign.Pro;
/// <summary>
/// TDesignPro 的依赖注入扩展。
/// </summary>
public static class TDesignProDependencyInjectionExtensions
{
    /// <summary>
    /// 添加指定配置的 TDesign Pro 版本的服务。
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configure">自定义配置的委托。</param>
    /// <returns></returns>
    public static IServiceCollection AddTDesignPro(this IServiceCollection services, Action<TDesignProOptions>? configure = default)
    {
        var options = new TDesignProOptions();
        configure?.Invoke(options);
        services.AddSingleton(options);
        services.AddTDesign();
        return services;
    }
}
