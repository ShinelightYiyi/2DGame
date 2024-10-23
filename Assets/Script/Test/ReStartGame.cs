using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFrameWork
{

    public class ReStartGame : MonoBehaviour
    {

        Animator bgAni;

        private void Start()
        {
            bgAni = GameObject.FindGameObjectWithTag("BG").GetComponent<Animator>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ReStart();
            }
        }

        void ReStart()
        {
            bgAni.SetBool("canIn", true);
            bgAni.SetBool("canOut", false);
            StartCoroutine(ReallyLoadSceneAsyn("TestScene"));
        }

        private IEnumerator ReallyLoadSceneAsyn(string scene)
        {

            AsyncOperation asy = SceneManager.LoadSceneAsync(scene);
            asy.allowSceneActivation = false;
            yield return new WaitForSeconds(1.5f);
            if(!asy.isDone)
            {
                if (asy.progress == 0.9f)
                {
                    asy.allowSceneActivation = true;
                    bgAni.SetBool("canIn", false);
                    bgAni.SetBool("canOut", true);
                }
                yield return null;
            }


        }


    }


        

}

