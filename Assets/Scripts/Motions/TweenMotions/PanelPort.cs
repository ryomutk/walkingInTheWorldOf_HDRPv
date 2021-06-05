using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;


[RequireComponent(typeof(PanelOpenMotion))]
public class PanelPort : MonoBehaviour
{
    static List<PanelPort> portList = new List<PanelPort>();
    [SerializeField] int _portID;
    PanelOpenMotion motion;
    public Image image { get; private set; }

    void Start()
    {
        motion = GetComponent<PanelOpenMotion>();
        image = transform.Find("Image").GetComponent<Image>();

        portList.Add(this);
    }

    public static Sequence ShowPanel(int id, Sprite sprite)
    {
        var instance = portList.Find(x => x._portID == id);

        return instance.ShowPanel(sprite);
    }

    public static Sequence HideAll(bool active = true)
    {
        Sequence seq = DOTween.Sequence();
        portList.ForEach(x => seq.Join(x.motion.Exit(activate:false)));

        if(active){
            seq.Play();
        }

        return seq;
    }

    Sequence HidePanel()
    {
        return motion.Exit();  
    }

    Sequence ShowPanel(Sprite sprite)
    {
        image.sprite = sprite;
        return motion.Enter();
    }


    void OnDestroy()
    {
        portList.Remove(this);
    }
}