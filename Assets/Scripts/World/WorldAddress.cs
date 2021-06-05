using UnityEngine;

namespace World
{

    /// <summary>
    /// 世界の住所をやり取りするデータ形式
    /// 入り口を原点として、右、上が正の
    /// </summary>
    public class WorldAddress
    {
        public Vector3Int coordinate { get; private set; }
        public string name
        {
            get
            {
                string ZName = "上";
                string XName = "右";

                if (coordinate.x < 0)
                {
                    XName = "左";
                }
                else if (coordinate.x == 0)
                {
                    XName = "心";
                }

                if (coordinate.z < 0)
                {
                    ZName = "下";
                }
                else if (coordinate.z == 0)
                {
                    ZName = "中";
                }

                return string.Format("第{2}層　{0}{1}世 {3}列{4}行 ", ZName, XName, coordinate.y, coordinate.x, coordinate.z);
            }
        }
        public WorldAddress(Vector3Int coords)
        {
            coordinate = coords;
        }
    }
}