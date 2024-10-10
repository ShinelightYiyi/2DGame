using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;


namespace GameFrameWork
{
    
    /// <summary>
    /// ���ڱ�������
    /// </summary>
    public class PlayerPrefsSave  
    {
        private static PlayerPrefsSave instance;
        public static PlayerPrefsSave Instance { get=>instance ?? (instance = new PlayerPrefsSave());}  



        /// <summary>
        /// �洢����
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void SaveData(string key,object data)
        {
            var json = JsonUtility.ToJson(data);  
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }


        /// <summary>
        /// ��ȡ����  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T LoadData<T>(string key)
        {
            try
            {
                var json = PlayerPrefs.GetString(key);
                var o = JsonUtility.FromJson<T>(json);
                return o;
            }
            catch(System.Exception e)
            {
                Debug.LogError($"Failed to load data to {key}. \n{e}");
                return default(T);
            }

        }
    }


    /// <summary>
    /// ���ڱ�������
    /// </summary>
    public class JsonSave
    {
        private static JsonSave instance;
        public static JsonSave Instance { get => instance ?? (instance = new JsonSave());}  


        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="dataName"></param>
        /// <param name="data"></param>
        public void SaveData(string dataName,object data)
        {
            string path = Path.Combine(Application.persistentDataPath, dataName);  
            var json = JsonUtility.ToJson(data);

            try
            {
                File.WriteAllText(path, json);
            }
            catch(System.Exception e)
            {
                Debug.LogError($"Failed to save data to {path}. \n{e}");
            }
        }


        /// <summary>
        /// ��������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataName"></param>
        /// <returns></returns>
        public T LoadData<T>(string dataName)
        {
            string path = Path.Combine(Application.persistentDataPath, dataName);
            try
            {
                var json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<T>(json);
                return data;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load data to {path}. \n{e}");
                return default(T);
            }
        }


        /// <summary>
        /// ɾ���浵
        /// </summary>
        /// <param name="dataName"></param>
        public void DeletSaveData(string dataName)
        {
            var path = Path.Combine(Application.persistentDataPath,dataName);
            if (path != null) File.Delete(path);
            else Debug.LogWarning($"�浵 {dataName} ������");
        }
    }
    


}
