using UnityEngine;

//道路作成に必要な道のセット
public interface IRoadSet
{
    RoadBase turnRight{get;}
    RoadBase turnLeft{get;}


    RoadBase GetSlice(RoadType type);
}