using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPanel : MonoBehaviour {
    public SpriteRenderer player;
    public Light pLight;
    public void PlayGame()
    {
        player.enabled = true;
        pLight.enabled = true;
        PlayerScript.instance.StartLightTimer();
        SceneManager.LoadScene("Level 1");
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);
    }
}
