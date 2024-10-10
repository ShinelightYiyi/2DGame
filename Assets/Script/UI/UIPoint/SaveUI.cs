using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFrameWork
{
    public class SaveUI : IBaseUI
    {
        public override void OnEnter()
        {
            base.OnEnter();
            gameObject.transform.DOScale(1.1f, 0.1f);
        }


        public override void OnClick()
        {
            base.OnClick();
            gameObject.transform.DOScale(0.9f, 0.1f);
            EventCenter.Instance.EventTrigger("±£´æ");
        }

        public override void OnExit()
        {
            base.OnExit();
            gameObject.transform.DOScale(1f, 0.1f);
        }
    }
}
