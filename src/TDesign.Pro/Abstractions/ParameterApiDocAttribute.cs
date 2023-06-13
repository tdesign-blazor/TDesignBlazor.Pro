using System.Reflection;

namespace TDesign.Pro.Abstractions;
internal class ParameterApiDocAttribute:Attribute
{
    public ParameterApiDocAttribute(string? comment)
    {
        Comment = comment;
    }
    public string? Type { get; set; }
    public string? Comment { get; }
    public bool Required { get; set; }
    public object? Value { get; set; }
}

public class ApiDoc
{
    public static List<(string name,string type,bool requried,object? defaultValue,string? comment)> GetParameterApiDoc(Type componentType)
    {
        var list = new List<(string name, string type, bool requried, object? defaultValue, string? comment)>();

        foreach ( var parameter in componentType.GetProperties().Where(m => m.IsDefined(typeof(ParameterApiDocAttribute), false)) )
        {
            var name = parameter.Name;
            var attr = parameter.GetCustomAttribute<ParameterApiDocAttribute>();
            var type = attr.Type?? parameter.PropertyType.Name;
            var required = attr.Required;
            var value = attr.Value;
            var comment = attr.Comment;
            list.Add((name, type, required, value, comment));
        }


        return list;
    }
}