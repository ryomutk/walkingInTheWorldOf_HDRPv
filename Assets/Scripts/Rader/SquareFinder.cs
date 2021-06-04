using UnityEngine;

/// <summary>
/// このゲーム内で必要な形でboxをcastせしもの
/// </summary>
public class SquareFinder
{
    SquareMetaData castSquare;

    public SquareFinder()
    {
        castSquare = new SquareMetaData();
        castSquare.MapSquare(Vector3Int.one, Vector3Int.zero, Vector3Int.zero);
    }

    /// <summary>
    /// 直線上にcastし、最初に見つけたのを返す。
    /// </summary>
    /// <param name="originCoords"></param>
    /// <param name="direction"></param>
    /// <param name="maxCount"></param>
    /// <returns></returns>
    public Square StraightFind(Vector3Int originCoords, Vector3Int direction, int maxCount = 10, Collider ignore = null)
    {
        int count = 0;
        Collider collider;

        do
        {
            count++;
            collider = Cast(originCoords + direction * count);


            if (count >= maxCount)
            {
                break;
            }
        }
        while (collider == null || collider == ignore);

        if (collider != null)
        {
            var sqr = collider.GetComponent<Square>();
            return sqr;
        }

        return null;
    }

    /// <summary>
    /// 指定したcoordinateの情報を取得
    /// </summary>
    /// <param name="coordinate"></param>
    /// <returns></returns>
    public Collider Cast(Vector3Int coordinate)
    {
        Collider[] hit = new Collider[1];

        castSquare.SetPosition(coordinate);

        Physics.OverlapBoxNonAlloc(castSquare.center, castSquare.halfExtent, hit, castSquare.rotation, LayerMask.GetMask("Square"));

        
        return hit[0];
    }

    /// <summary>
    /// メタスクエアの表示する範囲をチェック。
    /// </summary>
    /// <param name="metaSqr"></param>
    /// <returns></returns>
    public bool CheckSquare(IRectangularData metaSqr)
    {
        if (metaSqr.rotationEuler == Vector3.zero)
        {
            return !Physics.CheckBox(metaSqr.center, metaSqr.halfExtent);
        }
        else
        {
            return !Physics.CheckBox(metaSqr.center, metaSqr.halfExtent, metaSqr.rotation, LayerMask.GetMask("Square"));
        }
    }
}