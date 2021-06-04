using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 喧噪物のメタデータ
/// 建造物はそれが内接する最小の直方体のLocalPositionが最小の頂点が0,0,0であることを前提としている。
/// これを構成するブロックは、ここを介してしか編集できない。ようにするつもり
/// </summary>
public class Structure : MonoBehaviour, ISquareObj
{
    /// <summary>
    /// square上での出入口（接続先）の座標と向きを保持。
    /// </summary>
    public List<Doorway> doorwayList { get { return _doorwayList; } }
    public List<BuildBlock> blockList { get { return _blockList; } }
    public Vector3Int size { get { return _size; } }

    /// <summary>
    /// 必要なグリッドサイズ
    /// </summary>
    /// <value></value>
    public Vector3Int sizeGrid { get { return UnScale(size, Square.gridsize) + Vector3Int.one; } }
    public Vector3Int peak { get { return _peak; } }
    public Vector3Int deep { get { return _deep; } }
    public bool mapped { get { return _mapped; } }
    public Vector3 localCenter { get { return (Vector3)(peak + deep) / 2; } }




    [SerializeField] List<Doorway> _doorwayList;

    [SerializeField] List<BuildBlock> _blockList;
    [SerializeField] Vector3Int _size;
    [SerializeField] Vector3Int _peak;
    [SerializeField] Vector3Int _deep = new Vector3Int();
    [SerializeField] bool _mapped = false;

    /// <summary>
    /// Structureの情報を更新
    /// </summary>
[Sirenix.OdinInspector.Button]
    public virtual void Remap()
    {
        if (mapped)
        {
            doorwayList.Clear();
            blockList.Clear();
            Debug.LogWarning("multiMapped!");
        }


        GetComponentsInChildren<BuildBlock>(_blockList);
        foreach (var block in _blockList)
        {
            if (block.TryGetComponent<Doorway>(out Doorway doorway))
            {
                _doorwayList.Add(doorway);
            }
        }

        SetTip(x => x.transform.localPosition);

        _mapped = true;
    }

    void SetTip(System.Func<BuildBlock, Vector3> selector)
    {
        _peak.x = (int)blockList.Max(b => selector(b).x);
        _peak.y = (int)blockList.Max(b => selector(b).y);
        _peak.z = (int)blockList.Max(b => selector(b).z);

        _deep.x = (int)blockList.Min(b => selector(b).x);
        _deep.y = (int)blockList.Min(b => selector(b).y);
        _deep.z = (int)blockList.Min(b => selector(b).z);

        _size = peak - _deep + Vector3Int.one;
    }

    Vector3Int UnScale(Vector3Int a, Vector3Int b)
    {
        a.x /= b.x;
        a.y /= b.y;
        a.z /= b.z;

        return a;
    }

    public void Hide()
    {
        foreach (var block in blockList)
        {
            block.Hide();
        }
    }

    public void Show()
    {
        foreach (var block in blockList)
        {
            block.Show();
        }
    }

    public void Activate()
    {
        for(int i = 0;i < blockList.Count;i++)
        {
            blockList[i].Activate();
        }
    }

    public void Disactivate()
    {
        for (int i = 0; i < blockList.Count; i++)
        {
            blockList[i].Activate();
        }
    }


    /// <summary>
    /// ブロックの位置をそろえる
    /// </summary>
    [Sirenix.OdinInspector.Button]
    void SetNeat()
    {
        foreach (var block in blockList)
        {
            block.transform.localPosition = Vector3Int.RoundToInt(block.transform.localPosition);
            block.transform.eulerAngles = Vector3.zero;
        }
    }
}
