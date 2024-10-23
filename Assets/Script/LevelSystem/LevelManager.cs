using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�ؿ��ؿ���ÿ����һ�ش洢��ǰ�ؿ�λ�ã��ؿ����ȡ��һλ����Ϣ

namespace GameFrameWork 
{
    public class LevelManager : MonoBehaviour
    {
        bool canStart = true;

        [SerializeField] Transform SavePosition;

        [SerializeField] GameObject LevelEdge;

        

        [SerializeField] GameObject LastCamera, TheCamera;


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player" && canStart)
            {
                EventCenter.Instance.EventTrigger(gameObject.name);
                PlayerData.Instance.SaveData(SavePosition.position);
                canStart = false;
                StartLevel();
            }
        }


        private void StartLevel()
        {
            TheCamera.SetActive(true);
            LevelEdge.SetActive(true);  
            LastCamera.SetActive(false);
        }



    }
}
