using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FakeUp()
    {
        PlayerScript.instance.ColorUp();
    }

    public void FakeDown()
    {
        PlayerScript.instance.ColorDown();
    }
}
