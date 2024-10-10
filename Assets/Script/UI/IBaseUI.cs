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
        /// 点击
        /// </summary>
        public virtual void OnClick()
        {

        }

        /// <summary>
        /// 鼠标进入
        /// </summary>
        public virtual void OnEnter()
        {

        }


        /// <summary>
        /// 鼠标离开
        /// </summary>
        public virtual void OnExit()
        {

        }
    }
}
