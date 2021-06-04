using UnityEngine;

public class Player:MonoBehaviour
{
    static Vector3Int _size = new Vector3Int(1,3,1);
    public static Vector3Int size{get{return _size;}}
}