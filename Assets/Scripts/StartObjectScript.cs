using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartObjectScript : MonoBehaviour
{
    public int par;
    private void Awake()
    {
        PlayerScript.instance.newStartPosition(transform.position);
        GameUI.instance.NewPar(par);
    }
}
