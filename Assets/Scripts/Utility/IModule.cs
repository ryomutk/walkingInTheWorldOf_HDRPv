
namespace ModulePattern
{
    /// <summary>
    /// こいつはModuleStateを持っている。
    /// </summary>
    public interface IModule
    {
        ModuleState state { get; }
    }
}