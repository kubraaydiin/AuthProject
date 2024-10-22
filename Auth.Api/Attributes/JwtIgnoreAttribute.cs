namespace Auth.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class JwtIgnoreAttribute : Attribute
    {
    }
}
