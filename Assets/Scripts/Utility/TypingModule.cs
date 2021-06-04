using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class TypingModule : MonoBehaviour,IModule
{
    Text text;
    public ModuleState state { get; private set; }
    [SerializeField] float TypeIntervalSec = 0.5f;
    //trueで文字を一瞬で表示するようにする
    [SerializeField] bool showInstance = false; 
    string thinking = "‥";

    //文字列を一気に表示する場合の変換法則。
    Dictionary<string, string> convertRule = new Dictionary<string, string>{{"‥","..."}};

    void Awake()
    {
        text = GetComponent<Text>();
        Initialize();
    }

    void Initialize()
    {
        text.text = "";
        state = ModuleState.ready;
    }

    [Button]
    public bool Skip()
    {
        if (state == ModuleState.working)
        {
            //StartCoroutine(Hayamawashi());
            state = ModuleState.compleate;
            return true;
        }
        return false;
    }

    [Button]
    public bool Type(string message)
    {
        StartCoroutine(TypeMotion(message));
        return true;
    }



    /*
    IEnumerator Hayamawashi()
    {
        var tmp = TypeIntervalSec;
        TypeIntervalSec = 0;
        yield return new WaitUntil(() => state == ModuleState.ready);
        TypeIntervalSec = tmp;
    }
    */



    IEnumerator TypeMotion(string message)
    {
        text.text = "";
        yield return new WaitUntil(() => state == ModuleState.ready); //これがあることで、Enterで文字だけ先走ってしまわないようにする。
        state = ModuleState.working;
        //string wordTyped = "";
        for (int i = 0; i < message.Length; i++)
        {
            yield return new WaitForSeconds(TypeIntervalSec);
            if (message[i].ToString() == thinking)
            {
                yield return StartCoroutine(TypeThinking());
            }
            else
            {
                text.text = text.text.Insert(text.text.Length, message[i].ToString());
            }

            //Stateがcompleateになったら全文表示して終了。
            //TypeThinkingも終わるので、elseでつないでいない。
            if (state == ModuleState.compleate || showInstance)
            {
                text.text = Convert(message);
                break;
            }

        }
        state = ModuleState.ready;
    }

    string Convert(string message){
        for(int i = 0;i < message.Length;i++){
            foreach(KeyValuePair<string,string> c in convertRule){
                message = message.Replace(c.Key,c.Value);
            }
        }

        return message;
    }


    IEnumerator TypeThinking()
    {
        var tmpInterval = TypeIntervalSec * 5;
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(tmpInterval);
            text.text = text.text.Insert(text.text.Length, "・");
            if(state == ModuleState.compleate){
                yield break;
            }
        }
    }
}
