using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartObjectScript : MonoBehaviour
{
    private void Awake()
    {
        PlayerScript.instance.newStartPosition(transform.position);
    }
}
