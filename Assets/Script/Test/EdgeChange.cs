using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EdgeChange : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera v_CameraA;
    [SerializeField] CinemachineConfiner v_Confiner;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("½ÇÉ«½øÈë");
            StartCoroutine(RealChangeEdge());
        }
    }


    private IEnumerator RealChangeEdge()
    {
        int o = 74;
        v_Confiner.m_Damping = 7.4f;
        v_Confiner.m_BoundingShape2D = gameObject.GetComponent<Collider2D>();
        yield return new WaitForSeconds(0.5f);

        while (o > 0)
        {
            if (v_Confiner.m_Damping <= 0)
            {
                v_Confiner.m_Damping = 0;
                break;
            }
            v_Confiner.m_Damping = Mathf.MoveTowards(v_Confiner.m_Damping, 0f, 0.1f);
            o--;
            yield return new WaitForSeconds(0.03f);
        }
    }

}
