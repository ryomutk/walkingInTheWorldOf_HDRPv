using UnityEngine;
using DG.Tweening;

public class ItemSettor : Singleton<ItemSettor>
{
    GraphicFadeMotion titleFade;
    GraphicFadeMotion nameFade;
    TMPro.TMP_Text text;
    [SerializeField] float delay = 0.6f;

    void Start()
    {
        titleFade = transform.Find("Title").GetComponent<GraphicFadeMotion>();
        nameFade = transform.Find("Name").GetComponent<GraphicFadeMotion>();
        text = nameFade.GetComponent<TMPro.TMP_Text>();
    }

    public Sequence SetItem(string name)
    {
        text.text = name;
        var seq = titleFade.Enter()
        .Join(nameFade.Enter().SetDelay(delay));

        return seq;
    }

    public Sequence HideItem(){
        var seq = titleFade.Exit()
        .Join(nameFade.Exit());

        return seq;
    }
}