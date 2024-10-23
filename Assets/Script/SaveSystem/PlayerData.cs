using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameFrameWork
{
    public class PlayerData 
    {
        private static PlayerData instance;
        public static PlayerData Instance { get=>instance ?? (instance = new PlayerData()); }  

        private static GameObject player;

        [System.Serializable]
        class truePlayerData
        {
            public Vector2 playerPosition;
        }

        public void GetPlayer()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        public void SaveData(Vector2 position)
        {
            truePlayerData data = new truePlayerData();
            data.playerPosition = position;
            JsonSave.Instance.SaveData("player.sav", data);
        }

        public void LoadData()
        {
            truePlayerData data = new truePlayerData();
            data = JsonSave.Instance.LoadData<truePlayerData>("player.sav");
            if (player == null)
            {
                Debug.LogWarning("Íæ¼Ò²»´æÔÚ");
                return;
            }
            player.transform.position = data.playerPosition;
        }




    }

}
