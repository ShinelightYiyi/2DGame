using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�ؿ��ؿ���ÿ����һ�ش洢��ǰ�ؿ�λ�ã��ؿ����ȡ��һλ����Ϣ

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
