using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;


namespace GameFrameWork
{
    public class RampController : MonoBehaviour
    {
        [SerializeField] GameObject rampGo;
        [SerializeField] RampName rampName;


        private bool canDown = true;
        private bool boxDown = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Box" && canDown)
            {
                rampGo.transform.DOLocalMove(new Vector3(0, -0.5f, 3f), 0.1f);
                canDown = false;
                boxDown = true;

                MoveMachine();

            }
            else if (collision.tag == "Player" && canDown)
            {
                rampGo.transform.DOLocalMove(new Vector3(0, -0.1f, 3f), 0.1f);
                canDown = false;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Box" && !canDown)
            {
                rampGo.transform.DOLocalMove(new Vector3(0, 0f, 3f), 0.1f);
                canDown = true;
                boxDown = false;
                ReturMachine();
            }
            else if (collision.tag == "Player" && !canDown && !boxDown)
            {
                rampGo.transform.DOLocalMove(new Vector3(0, 0f, 3f), 0.1f);
                canDown = true;
            }

        }

        private void MoveMachine()
        {
            switch(rampName)
            {
                case RampName.Up:
                    EventCenter.Instance.EventTrigger<bool>("垂直移动", true); 
                    break;
                case RampName.Down:
                    EventCenter.Instance.EventTrigger<bool>("垂直移动", false);
                    break;
                case RampName.Right:
                    EventCenter.Instance.EventTrigger<int>("水平移动", 1);
                    break;
                case RampName.Left:
                    EventCenter.Instance.EventTrigger<int>("水平移动", -1);
                    break;
            }
        }

        private void ReturMachine()
        {
            switch (rampName)
            {
                case RampName.Up:
                    EventCenter.Instance.EventTrigger<bool>("垂直移动", false);
                    break;
                case RampName.Down:
                    EventCenter.Instance.EventTrigger<bool>("垂直移动", true);
                    break;
                case RampName.Right:
                    EventCenter.Instance.EventTrigger<int>("水平移动", -1);
                    break;
                case RampName.Left:
                    EventCenter.Instance.EventTrigger<int>("水平移动", 1);
                    break;
            }
        }

        private void OnDestroy()
        {
            EventCenter.Instance.Clear();
            rampGo.transform.DOKill();
        }

    }

    


    public enum RampName
    {
        Up,
        Down, 
        Left, 
        Right
    }
}



