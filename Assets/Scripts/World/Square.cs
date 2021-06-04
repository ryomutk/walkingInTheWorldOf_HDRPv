using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Structureの親。立体グリッドにフィットする直方体
public class Square : MonoBehaviour
{
    public Structure structure { get; private set; }

    /// <summary>
    /// 原点グリッドの座標。
    /// </summary>
    /// <value></value>
    public Vector3Int coordinate { get; private set; }

    public WorldAddress address { get; private set; }

    /// <summary>
    /// 全体の大きさ(単位はグリッド)。今のところ回転は反映されていない。
    /// </summaｚ
    /// <value></value>
    public Vector3Int scale { get; private set; }

    /// <summary>
    /// 一マスのサイズ
    /// </summary>
    /// <returns></returns>
    static public Vector3Int gridsize = new Vector3Int(9, 9, 9);

    /// <summary>
    /// インスタンスのスケールが反映されたサイズ
    /// </summary>
    Vector3Int scaledSize { get { return Vector3Int.Scale(gridsize, scale); } }

    /// <summary>
    /// square座標系の左下（わかってくれ）のローカル座標
    /// </summary>
    /// <param name="Vector3.one"></param>
    /// <returns></returns>
    Vector3Int origin { get { return -(gridsize - Vector3Int.one) / 2; } }


    public new BoxCollider collider{get;private set;}


    [Sirenix.OdinInspector.Button]
    /// <param name="strPref">structureのprefab</param>
    /// <param name="originCoords">原点グリッドの座標</param>
    /// <param name="rotateEuler">何度回転するか</param>
    /// <param name="setRandom">建物をSquare内でランダム配置するか</param>
    public void SetStructure(Structure structureInstance, Vector3Int originCoords, Vector3Int rotateEuler, bool setRandom = true)
    {
        if (collider == null)
        {
            collider = GetComponent<BoxCollider>();
        }

        coordinate = originCoords;

        address = new WorldAddress(coordinate);
        this.scale = structureInstance.sizeGrid;

        //接触してるだけなら反応しないようにするために、0.1四方だけ引いている
        collider.size = scaledSize - Vector3.one*0.1f;

        //これは原点グリッドの中心座標。全体ではない。
        Vector3 pos = new Vector3();

        pos = Vector3Int.Scale(originCoords, gridsize) - origin;

        transform.position = pos;

        collider.center = Vector3.Scale((Vector3)gridsize / 2, scale - Vector3Int.one);

        var str = structureInstance;
        str.transform.SetParent(transform);

        if (!str.mapped)
        {
            str.Remap();
        }


        if (setRandom)
        {
            var gap = scaledSize - str.size;

            gap.x = Random.Range(0, (int)Mathf.Round(gap.x));
            gap.y = Random.Range(0, (int)Mathf.Round(gap.y));
            gap.z = Random.Range(0, (int)Mathf.Round(gap.z));

            gap += origin;

            str.transform.localPosition = gap;
        }
        else
        {
            str.transform.localPosition = Vector3Int.zero;
        }

        transform.eulerAngles = rotateEuler;
        structure = str;
    }

    bool eraseTrigger
    {
        get
        {
            if (_eraseTrigger)
            {
                _eraseTrigger = false;
                return true;
            }
            return false;
        }
    }
    bool _eraseTrigger = false;
    public void EraseIfBug()
    {
        _eraseTrigger = true;
    }

    void OncollisionStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Square"))
        {

            if (eraseTrigger)
            {
                //SquareBuilder.instance.RemoveSquare(this);
                Debug.Log("悪い子。"+name+" scale:" + scale + "coordinate:" + coordinate + "rotate:" + transform.eulerAngles+"with"+other.name);
            }
        }
    }

}
