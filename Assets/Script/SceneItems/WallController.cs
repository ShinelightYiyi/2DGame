using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameFrameWork
{
    public class WallController : MonoBehaviour
    {
        [SerializeField] string levelName;
        [SerializeField] WallName wallName;


        [Header("��ֱ")] [SerializeField] float Up,Down;

        private void Start()
        {
            EventCenter.Instance.AddEventListener(levelName, () => StartTheLevel());
        }

        private void StartTheLevel()
        {
            if(gameObject != null)
            {
                switch (wallName)
                {
                    case WallName.Horizontal:
                        EventCenter.Instance.AddEventListener<bool>("��ֱ�ƶ�", (o) => HorizontalMove(o));
                        break;
                    case WallName.Vertecial:
                        EventCenter.Instance.AddEventListener<int>("ˮƽ�ƶ�", (o) => VerticalMove(o));
                        break;
                }
            }
            else
            {
                return;
            }
        }    

        private int moveIndex = 0;

        [Header("ˮƽ")][SerializeField] Transform Right, Left,Local;
        private void VerticalMove(int o)
        {
            moveIndex += o;
            if(moveIndex == 1)
            {
                gameObject.transform.DOMove(Right.position, 0.2f);
            }
            else if(moveIndex == 0)
            {
                gameObject.transform.DOMove(Local.position, 0.2f);
            //    Debug.LogWarning("��ԭ");
            }
            else if (moveIndex == -1)
            {
                gameObject.transform.DOMove(Left.position, 0.2f);
            }
        }


        /// <summary>
        /// tureΪ���ϣ�falseΪ����  
        /// </summary>
        /// <param name="o"></param>
        private void HorizontalMove(bool o)
        {
            if(gameObject != null)
            {
                if (o)
                {
                    gameObject.transform.DOMove(gameObject.transform.position + new Vector3(0,Up), 0.2f);
                }
                else if (!o)
                {
                    gameObject.transform.DOMove(gameObject.transform.position + new Vector3(0,Down), 0.2f);
                    Debug.LogWarning("�ƶ�");
                }
            }
            else
            {
                return;
            }
        }

        private void OnDestroy()
        {
            gameObject.transform.DOKill();
        }

    }


    public enum WallName
    {
        Horizontal,Vertecial
    }
}
