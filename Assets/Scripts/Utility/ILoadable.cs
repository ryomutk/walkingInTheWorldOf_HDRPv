using System.Collections.Generic;

public interface IRequireToLoad:ILoad
{
    List<ILoad> requireComponentList{get;}
    void StartLoad();
}

public interface ILoad
{
    bool loaded{get;}
}