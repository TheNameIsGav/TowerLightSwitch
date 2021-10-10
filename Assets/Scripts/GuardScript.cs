using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardScript : MonoBehaviour
{

    [SerializeField]
    private Quaternion ARotatePoint;
    [SerializeField]
    private Quaternion BRotatePoint;
    private bool rotateDirection = true;
    private float currRotation = 0;

    const float rotateIncrement = .01f; //Speed of rotation
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (rotateDirection) { transform.rotation = Quaternion.Lerp(ARotatePoint, BRotatePoint, currRotation += rotateIncrement); }
        else { transform.rotation = Quaternion.Lerp(BRotatePoint, ARotatePoint, currRotation += rotateIncrement); }
        
        if(currRotation >= 1)
        {
            rotateDirection = !rotateDirection;
            currRotation = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) { PlayerScript.instance.startPlayerDeath(); }
    }
}
