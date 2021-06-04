using UnityEngine;


/// <summary>
/// squareに入れられるobj。sqareを作るのに必要な情報へのインターフェイス
/// </summary>
public interface ISquareObj
{
    Vector3Int size{get;}
    Vector3Int sizeGrid{get;}
    
}