namespace Inventory.Contracts;

public interface IModuleProvider : IDisposable
{
    IEnumerable<IModule>? GetModules(IEnumerable<string> assemblyNames);
}
