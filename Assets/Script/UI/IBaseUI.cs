using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFrameWork
{
    public abstract class IBaseUI : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnExit();
        }

        /// <summary>
        /// ���
        /// </summary>
        public virtual void OnClick()
        {

        }

        /// <summary>
        /// ������
        /// </summary>
        public virtual void OnEnter()
        {

        }


        /// <summary>
        /// ����뿪
        /// </summary>
        public virtual void OnExit()
        {

        }
    }
}
