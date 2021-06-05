using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utility;

using World.Wrapper;

namespace World.Visual
{

    [RequireComponent(typeof(Square))]
    public class SquareOptimizer : MonoBehaviour
    {
        Square self;
        [SerializeField] bool hideOnStart = true;
        Utility.Logger logger;

        void Start()
        {
            self = GetComponent<Square>();
            logger = new Utility.Logger("SquareLog","SquareOptimizer");

            logger.Log("________________LOG START__________________________");

            if (hideOnStart)
            {
                logger.Log(self.name + ":HIDE BEGIN");
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
            logger.Log(self.name + ":" + self.structure.name);
            if (self.structure != null)
            {
                self.structure.Hide();
            }
            else
            {
                StartCoroutine(ThenHide());
            }
        }

        IEnumerator ThenHide()
        {
            logger.Log(self.name + ":HIDE WAIT");
            yield return new WaitWhile(() => self.structure == null);
            logger.Log(self.name + ":HIDDEN");
            Hide();
        }

    }

}