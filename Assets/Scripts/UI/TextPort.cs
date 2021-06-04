using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextPort : MonoBehaviour
{
    static List<TextPort> portList = new List<TextPort>();
    Text _text;
    [SerializeField]int _id;

    void Start()
    {
        portList.Add(this);
        _text = GetComponent<Text>();
    }

    static public void Display(int id, string text)
    {
        try
        {
            portList.Find(x => x._id == id)._text.text = text;
        }
        catch(System.NullReferenceException)
        {
            portList[0]._text.text = text;
        }
    }


    void OnDestroy()
    {
        portList.Remove(this);
    }
}