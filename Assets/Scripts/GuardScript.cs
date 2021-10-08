using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPlayerDetect()
    {
        //TODO - Functioanlity for detecting the player
        PlayerScript.instance.PlayerDeath();
    }
}
