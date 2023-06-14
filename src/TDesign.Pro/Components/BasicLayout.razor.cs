using TDesign.Pro.Abstractions;

namespace TDesign.Pro.Components;

/// <summary>
/// 表示基础的布局样式。使用传统的上左右的布局。
/// </summary>
public partial class BasicLayout
{
    /// <summary>
    /// 获取注入的配置项。
    /// </summary>
    [Inject]TDesignProOptions Options { get; set; }

    /// <summary>
    /// 设置顶部导航菜单的委托。
    /// </summary>
    [ParameterApiDoc("顶部导航菜单的委托", Type = "Func<Task<IEnumerable<MenuItem>>>")]
    [Parameter] public Func<Task<IEnumerable<MenuItem>>>? TopMenuItemsProvider { get; set; } = () => Task.FromResult(Enumerable.Empty<MenuItem>());
    /// <summary>
    /// 顶部导航菜单操作部分的任意内容。
    /// </summary>
    [ParameterApiDoc("顶部导航菜单操作部分的任意内容")]
    [Parameter] public RenderFragment? TopMenuOperationContent { get; set; }
    /// <summary>
    /// 设置边栏导航菜单的委托。
    /// </summary>
    [ParameterApiDoc("边栏导航菜单的委托",Type = "Func<Task<IEnumerable<MenuItem>>>")]
    [Parameter] public Func<Task<IEnumerable<MenuItem>>>? SideMenuItemsProvider { get; set; } = () => Task.FromResult(Enumerable.Empty<MenuItem>());
    /// <summary>
    /// 自定义系统的名称或 LOGO 部分的任意内容，不填写则使用注入服务时的配置。
    /// </summary>
    [ParameterApiDoc("自定义系统的名称或 LOGO 部分的任意内容，不填写则使用注入服务时的配置")]
    [Parameter]public RenderFragment? LogoContent { get; set; }
    /// <summary>
    /// 设置布局底部的任意代码片段。
    /// </summary>
    [ParameterApiDoc("布局底部的任意代码片段")]
    [Parameter]public RenderFragment? FooterContent { get; set; }
    /// <summary>
    /// 显示内容页的部分，一般使用 <see cref="LayoutComponentBase.Body"/> 参数。
    /// </summary>
    [ParameterApiDoc("显示内容页的部分，一般使用 @Body 参数")]
    [Parameter]public RenderFragment? BodyContent { get; set; }
    protected override void OnInitialized()
    {
        LogoContent ??= builder => builder.Element("img",condition: Options.LogoUrl.IsNotNullOrEmpty()).Attribute("src",Options.LogoUrl)
                                            .Element("h2").Style("margin-left:10px").Content(Options.AppName).Close()
                                            ;
    }

    RenderFragment? GetSubMenu(MenuItem menu)
        => builder => builder.Component<TSubMenu>(menu.Children.Any())
                            .Content(GetMenuItem(menu.Children))
                            .Close();
    RenderFragment? GetMenuItem(IEnumerable<MenuItem> menus)
        => builder =>
        {
            foreach ( var item in menus )
            {
                if ( item.IsGroup )
                {
                    builder.Component<TMenuItemGroup>().Attribute(m => m.Title, item.Name).Content(GetMenuItem(item.Children)).Close();
                }
                else
                {
                    builder.Component<TMenuItem>()
                            .Attribute(m => m.Link, item.Link)
                            .Attribute(m => m.IconPrefix, item.Icon)
                            .Attribute(m=>m.AdditionalAttributes,item.Attributes)
                            .Content(builder =>
                            {
                                if ( item.Children.Any() )
                                {
                                    builder.Content(GetSubMenu(item));
                                }
                                else
                                {
                                    builder.Content(item.Name);
                                }
                            })
                            .Close();
                }
            }
        };
}

/// <summary>
/// 表示导航菜单的项。
/// </summary>
public class MenuItem
{
    /// <summary>
    /// 获取或设置菜单显示的名称。
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// 获取或设置菜单的图标。
    /// </summary>
    public object? Icon { get; set; }
    /// <summary>
    /// 获取或设置菜单的导航链接。
    /// </summary>
    public string? Link { get; set; }

    /// <summary>
    /// 获取或设置一个布尔值，表示是否作为分组菜单。
    /// </summary>
    public bool IsGroup { get; set; }
    /// <summary>
    /// 菜单的其他特性。
    /// </summary>
    public IDictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();
    /// <summary>
    /// 获取或设置子菜单。
    /// </summary>
    public IEnumerable<MenuItem> Children { get; set; } = Enumerable.Empty<MenuItem>();
}