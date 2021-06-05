using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//プレイヤーがその上に乗ったことを感知する。
//現状BuildBlock以外が使うことは想定していない
public class PlayerDetector : DetectorBehaviour<Player>, IObserver<ModuleState>
{
    //すべてのインスタンスに栄光あれ
    static List<PlayerDetector> instanceList;
    //BlockのActive状況と同期
    [SerializeField] bool syncWithBlock = true;
    [SerializeField] int castIntervalTick = 2;
    BuildBlock selfBlock = null;
    Logger logger;
    Coroutine castRoutine;


    protected override void Start()
    {
        base.Start();

        logger = new Logger("CastLog", "PLAYER DETECTOR on:" + gameObject.name);

        instanceList.Add(this);

        if (syncWithBlock)
        {
            selfBlock = GetComponent<BuildBlock>();
            selfBlock.AddObserver(this);
            
            if (selfBlock == null)
            {
                logger.LogError("BLOCK NOT FOUND");
            }
        }
    }

    public bool OnNotice(ModuleState state)
    {
        if (state == ModuleState.working)
        {
            StartCoroutine(CastRoutine());
            return true;
        }
        else
        {
            StopAllCoroutines();
            return true;
        }
    }

    public bool StartCastRoutine()
    {
        if (castRoutine == null)
        {
            castRoutine = StartCoroutine(CastRoutine());
            return true;
        }

        logger.LogError("AREADY STARTED!");
        return false;
    }

    public bool StopCastRoutine()
    {
        if (castRoutine != null)
        {
            StopCoroutine(castRoutine);
            return true;
        }

        return false;
    }

    IEnumerator CastRoutine()
    {
        while (true)
        {
            for (int i = 0; i < castIntervalTick; i++)
            {
                yield return new WaitUntil(() => Clock.instance.Signal);
            }

            
        }
    }

    void OnDestroy()
    {
        instanceList.Remove(this);
    }
}