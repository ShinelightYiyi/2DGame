using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//关卡重开：每进入一关存储当前关卡位置，重开则读取这一位置信息

namespace GameFrameWork 
{
    public class LevelManager : MonoBehaviour
    {
        bool canStart = true;



        [SerializeField] GameObject LastLevel;

        [SerializeField] GameObject LastCamera, TheCamera;


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player" && canStart)
            {
                EventCenter.Instance.EventTrigger(gameObject.name);
                canStart = false;
                StartLevel();
            }
        }


        private void StartLevel()
        {
            TheCamera.SetActive(true);
            LastCamera.SetActive(false);
        }



    }
}
