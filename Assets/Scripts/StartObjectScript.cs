using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartObjectScript : MonoBehaviour
{
    private SpriteRenderer sprite;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        PlayerScript.instance.newStartPosition(sprite.transform.position);
    }
}
