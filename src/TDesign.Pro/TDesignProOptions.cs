namespace TDesign.Pro;

/// <summary>
/// TDeisng 专业版的配置项。
/// </summary>
public class TDesignProOptions
{
    /// <summary>
    /// 获取或设置应用程序的名称。
    /// </summary>
    public string? AppName { get; set; } = "TDesignPro";
    /// <summary>
    /// 获取或设置 LOGO Url 地址。
    /// </summary>
    public string? LogoUrl { get; set; }
    /// <summary>
    /// 获取或设置是否为深色主题。
    /// </summary>
    public bool Dark { get; set; }
}
