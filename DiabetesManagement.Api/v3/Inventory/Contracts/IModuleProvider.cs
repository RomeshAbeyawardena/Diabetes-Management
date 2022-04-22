namespace Inventory.Contracts;

public interface IModuleProvider
{
    IEnumerable<IModule>? GetModules(IEnumerable<string> assemblyNames);
}
