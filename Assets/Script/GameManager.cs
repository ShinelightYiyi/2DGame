using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameFrameWork
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            PlayerData.Instance.GetPlayer();
            EventCenter.Instance.AddEventListener("±£´æ",()=>PlayerData.Instance.SaveData());
            EventCenter.Instance.AddEventListener("¶ÁÈ¡",()=>PlayerData.Instance.LoadData());
        }
    }
}
