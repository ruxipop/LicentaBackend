namespace BackendLi.Entities.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ServiceAttribute : Attribute
{    public ServiceAttribute(Type type)
    {
        Type = type;
        ServiceLifetime = ServiceLifetime.Scoped;
    }

    public Type Type { get; }
    public ServiceLifetime ServiceLifetime { get; }
}