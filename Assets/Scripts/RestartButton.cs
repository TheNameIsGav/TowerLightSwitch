using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerScript.instance.newStartPosition(new Vector2(0f,0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   public void OnClick()
    {
        PlayerScript.instance.currLevel = 1;
        SceneManager.LoadScene("Level 1");
    }
}
