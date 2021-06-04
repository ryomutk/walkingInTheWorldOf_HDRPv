using UnityEngine;



public class SquareCollider:BoxCollider
{
    public Square square{get;private set;}

    public SquareCollider():base()
    {
        square = GetComponent<Square>();
    }
}