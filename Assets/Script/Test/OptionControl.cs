using GameFrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionControl : MonoBehaviour
{
    private bool isOption;
    [SerializeField] GameObject Panel;

    private void Awake()
    {
        isOption = false;
    }


    private void Update()
    {
        StartOption();
    }


    private void StartOption()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !isOption)
        {
            isOption = true;
            EventCenter.Instance.EventTrigger<bool>("控制输入", false);
            Time.timeScale = 0f;
            Panel.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isOption)
        {
            isOption= false;
            EventCenter.Instance.EventTrigger<bool>("控制输入", true);
            Time.timeScale = 1f;
            Panel.SetActive(false);
        }
    }
}
