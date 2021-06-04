using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Square))]
public class SquareOptimizer : MonoBehaviour
{
    Square self;
    [SerializeField] bool hideOnStart = true;
    System.Action<string> log;

    void Start()
    {
        self = GetComponent<Square>();
        log = (x) => LogWriter.Log(x,"SquareLog");

        log("________________LOG START__________________________");

        if (hideOnStart)
        {
            log(self.name + ":HIDE BEGIN");
            Hide();
        }
    }

    public SquareOptimizer(Square square)
    {
        self = GetComponent<Square>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Show();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Hide();
        }
    }

    public void Show()
    {
        self.structure.Show();
    }

    public void Hide()
    {
        log(self.name +":"+ self.structure.name);
        if (self.structure != null)
        {
            self.structure.Hide();
        }
        else{
            StartCoroutine(ThenHide());
        }
    }

    IEnumerator ThenHide()
    {
        log(self.name + ":HIDE WAIT");
        yield return new WaitWhile(()=>self.structure == null);
        log(self.name + ":HIDDEN");
        Hide();
    }

}
