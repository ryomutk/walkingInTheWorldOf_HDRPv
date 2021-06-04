using System.Collections;
using UnityEngine;

public class RandomBuildBoy : MonoBehaviour
{
    /// <summary>
    /// 使うグリッド座標すべて
    /// </summary>
    /// <param name="Start("></param>
    /// <returns></returns>
    [SerializeField] Vector3Int Range = new Vector3Int(100, 100, 100);

    [SerializeField] bool setRandom = true;

    /// <summary>
    /// n回連続で失敗するまで施行を繰り返す
    /// </summary>
    [Sirenix.OdinInspector.Button]
    void StartFailBaseSet(int a)
    {
        StartCoroutine(FailBaseSet(a));
    }
    IEnumerator FailBaseSet(int failNum)
    {
        int count;
        bool endflag = false;
        while(!endflag)
        {
            count = 0;
            while(!RandomShoot())
            {
                
                Debug.Log("set");
                if(count > failNum)
                {
                    endflag = true;
                    break;
                }
                count++;
                yield return 5;
            }
            yield return 5;
        }

        Debug.Log("compleate!");
    }


    bool RandomShoot()
    {
        var max = StructureDB.instance.length;
        var strId = Random.Range(0, max);
        var euler = MathOfWorld.rotateEulers[Random.Range(0, 4)];
        Vector3Int coords = new Vector3Int();

        coords.x = Random.Range(-Range.x, Range.x);
        coords.y = Random.Range(0, Range.y);
        coords.z = Random.Range(-Range.z, Range.z);

        return SquareBuilder.instance.Build(strId,coords,euler,setRandom);
    }

}
