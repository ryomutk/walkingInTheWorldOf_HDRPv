using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ロード順が決まってるクラスを複数保持し、
/// requireComponentsがすべてloadし終わってからロードされるように汁。
/// </summary>
public class LoadManager : Singleton<LoadManager>
{
    public void RegisterLoad(IRequireToLoad requireClass)
    {
        StartCoroutine(LoadRoutine(requireClass));
    }
    
    IEnumerator LoadRoutine(IRequireToLoad loadClass)
    {
        yield return 2;
        
        foreach (var iload in loadClass.requireComponentList)
        {
            yield return new WaitUntil(() => iload != null);
            yield return new WaitUntil(() => iload.loaded);
        }

        loadClass.StartLoad();
    }
}
