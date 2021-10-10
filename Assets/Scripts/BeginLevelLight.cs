using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginLevelLight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(LightsOff());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LightsOff()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);
        for(int i = 0; i < 2; i++)
        {
            yield return wait;
        }
        gameObject.SetActive(false);
    }
}
