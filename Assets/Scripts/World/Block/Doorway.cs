using UnityEngine;


public class Doorway : MonoBehaviour
{
    /// <summary>
    /// 空いてるワールド座標上の向き。
    /// CheckAvalableDirectionを呼ぶたびに更新される。
    /// 呼ばれる前はnullなので注意
    /// </summary>
    /// <value></value>
    public Vector3Int? avalableDirection { get { return _direction; } }
    protected Vector3Int? _direction = null;

    
    /// <summary>
    /// 空いているかを確認。
    /// </summary>
    /// <returns></returns>

    public virtual Vector3Int? CheckAvalableDirection()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.enabled = false;

        foreach (var direction in MathOfWorld.directions)
        {
            if(direction == Vector3Int.down || direction == Vector3Int.up)
            {
                continue;
            }
            else if (!Physics.Raycast(transform.position, direction, 1, LayerMask.GetMask("Block")))
            
            {
                _direction = direction;
                break;
            }
        }
        collider.enabled = true;

        if (_direction != null)
        {
            return _direction;
        }

        return null;
    }
}