using GameFrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReStartLevel : MonoBehaviour
{
    private static ReStartLevel instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



}
