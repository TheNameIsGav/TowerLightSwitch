using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPanel : MonoBehaviour {
    public void PlayGame()
    {
        SceneManager.LoadScene("Level 1");
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);
    }
}
