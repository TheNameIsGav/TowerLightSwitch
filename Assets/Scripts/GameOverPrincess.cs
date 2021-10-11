using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPrincess : MonoBehaviour
{
    private SpriteRenderer avatar;
    void Awake()
    {
        avatar = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public void Hide()
    {
        avatar.sortingOrder = 0;
    }

    // Update is called once per frame
    public void Show()
    {
        avatar.sortingOrder = 3;
    }
}
