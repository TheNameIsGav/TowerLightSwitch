using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainScript : MonoBehaviour
{
    public static CurtainScript instance;
    public SpriteRenderer avatar;
    private void Awake()
    {
        avatar = this.GetComponent<SpriteRenderer>();
        instance = this;
        DontDestroyOnLoad(instance);
    }
}
