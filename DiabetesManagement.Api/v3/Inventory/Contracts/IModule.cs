using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Inventory.Contracts;

public interface IModule
{
    public bool CanRun();
    public string Name { get; }
    IEnumerable<Assembly> Assemblies { get; }
    void RegisterServices(IServiceCollection services);
}
