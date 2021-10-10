using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    private int light_switches;
    private int par = 4;
    public TextMeshProUGUI light_switch_text;
    public TextMeshProUGUI par_main_text;
    //public Text light_switch_text;

    // Update is called once per frame

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Update()
    {
        par_main_text.text = "Par: " + par;
        light_switch_text.text = "Light Switches: " + PlayerScript.instance.lightScore;
        light_switches += 1;
    }
}
