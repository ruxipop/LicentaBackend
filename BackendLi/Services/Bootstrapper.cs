using System.Reflection;
using BackendLi.Entities.Attributes;

namespace BackendLi.Services;

public static class Bootstrapper
{
    private const string NamespacePrefix = "BackendLi";

    public static void ConfigureServices(IServiceCollection serviceCollection)
    {
        var assemblies = GetAssemblies();

        var serviceAttributes = assemblies
            .SelectMany(x => x.DefinedTypes)
            .SelectMany(x => x.GetCustomAttributes<ServiceAttribute>(false)
                .Select(y => new KeyValuePair<Type, ServiceAttribute>(x, y)))
            .ToList();
        foreach ((var type, var serviceAttribute) in serviceAttributes)
            serviceCollection.Add(new ServiceDescriptor(serviceAttribute.Type, type,
                serviceAttribute.ServiceLifetime));
    }

    private static IEnumerable<Assembly> GetAssemblies()
    {
        var assemblies = GetAssemblies(Assembly.GetEntryAssembly()).Distinct().ToList();
        return assemblies;
    }

    private static IEnumerable<Assembly> GetAssemblies(Assembly? assembly)
    {
        if (assembly != null)
        {
            foreach (var referencedAssembly in assembly.GetReferencedAssemblies()
                         .Where(x => x.Name != null && x.Name.StartsWith(NamespacePrefix)).Select(Assembly.Load)
                         .SelectMany(GetAssemblies))
                yield return referencedAssembly;
            yield return assembly;
        }
    }
}