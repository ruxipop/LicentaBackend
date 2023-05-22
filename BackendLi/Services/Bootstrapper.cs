using System.Reflection;
using BackendLi.Entities.Attributes;

namespace BackendLi.Services;

public static class Bootstrapper
{
    private const string NamespacePrefix = "BackendLi";
    public static void ConfigureServices(IServiceCollection serviceCollection)
    {
        IEnumerable<Assembly> assemblies = GetAssemblies();
        
        List<KeyValuePair<Type, ServiceAttribute>> serviceAttributes = assemblies
            .SelectMany(x => x.DefinedTypes)
            .SelectMany(x =>x.GetCustomAttributes<ServiceAttribute>(false)
                .Select(y => new KeyValuePair<Type, ServiceAttribute>(x, y)))
            .ToList();
        foreach ((Type type, ServiceAttribute serviceAttribute) in serviceAttributes)
        {
            serviceCollection.Add(new ServiceDescriptor(serviceAttribute.Type, type,
                serviceAttribute.ServiceLifetime));
        }
    }
    private static IEnumerable<Assembly> GetAssemblies()
    {
        List<Assembly> assemblies = GetAssemblies(Assembly.GetEntryAssembly()).Distinct().ToList();
        return assemblies;
    }
    private static IEnumerable<Assembly> GetAssemblies(Assembly? assembly)
    {
        if (assembly != null)
        {
            foreach (Assembly referencedAssembly in assembly.GetReferencedAssemblies()
                         .Where(x => x.Name != null && x.Name.StartsWith(NamespacePrefix)).Select(Assembly.Load)
                         .SelectMany(GetAssemblies))
            {
                yield return referencedAssembly;
            }
            yield return assembly;
        }
    } 
}