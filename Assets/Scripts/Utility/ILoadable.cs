using System.Collections.Generic;

namespace Utility
{
    public interface IRequireToLoad : ILoad
    {
        List<ILoad> requireComponentList { get; }
        void StartLoad();
    }

    public interface ILoad
    {
        bool loaded { get; }
    }
}