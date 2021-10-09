using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    //Publics
    public static PlayerScript instance;

    //Privates
    private bool isLight;
    private Light areaLight;
    private int lightScore;

    // Player Things
    public SpriteRenderer avatar;
    private Rigidbody2D avatarBody;
    public float acceleration;
    private Vector2 velocity;
    // Hitboxes of Snake people do not include head

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(instance);
    }

    void Start()
    {
        avatarBody = transform.GetComponent<Rigidbody2D>();
        areaLight = transform.GetComponent<Light>();
        StartLightTimer();
    }

    private void FixedUpdate()
    {
        bool accelerated = false;
        if (Input.GetKey(KeyCode.W))
        {
            avatarBody.velocity = new Vector2(avatarBody.velocity.x, avatarBody.velocity.y + acceleration);
            accelerated = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            avatarBody.velocity = new Vector2(avatarBody.velocity.x, avatarBody.velocity.y - acceleration);
            accelerated = true;
        }
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            if (System.Math.Abs(avatarBody.velocity.y) < acceleration)
            {
                avatarBody.velocity = new Vector2(avatarBody.velocity.x, 0);
            } else if (avatarBody.velocity.y > 0)
            {
                avatarBody.velocity = new Vector2(avatarBody.velocity.x, avatarBody.velocity.y - acceleration);
            } else
            {
                avatarBody.velocity = new Vector2(avatarBody.velocity.x, avatarBody.velocity.y + acceleration);
            }
        }

        accelerated = false;
        if (Input.GetKey(KeyCode.D))
        {
            avatarBody.velocity = new Vector2(avatarBody.velocity.x + acceleration, avatarBody.velocity.y);
            accelerated = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            avatarBody.velocity = new Vector2(avatarBody.velocity.x - acceleration, avatarBody.velocity.y);
            accelerated = true;
        }
        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            if (System.Math.Abs(avatarBody.velocity.x) < acceleration)
            {
                avatarBody.velocity = new Vector2(0, avatarBody.velocity.y);
            }
            else if (avatarBody.velocity.x > 0)
            {
                avatarBody.velocity = new Vector2(avatarBody.velocity.x - acceleration, avatarBody.velocity.y);
            }
            else
            {
                avatarBody.velocity = new Vector2(avatarBody.velocity.x + acceleration, avatarBody.velocity.y);
            }
        }
    }

    private void Update()
    {
        if (isLight) { areaLight.intensity = 1;
        } else { areaLight.intensity = -1; }
    }

    public void PlayerDeath()
    {
        //TODO
        Debug.Log("Player Died");
        // This comment is a test to make sure I got the branches working
    }

    //Gav's Function's

    /// <summary>
    /// Swaps the current state of Light
    /// </summary>
    public void LightSwap() { isLight = !isLight; }

    /// <summary>
    /// Gets the current state of Light
    /// </summary>
    public bool GetLightState() { return isLight; }

    /// <summary>
    /// Function to handle the on and off of lights
    /// </summary>
    void StartLightTimer()
    {
        StartCoroutine(iterateTime());
    }

    IEnumerator iterateTime()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);
        int count = isLight ? 5 : 10; //If we are light, then wait 5 seconds, if we are dark then wait 10 seconds
        for (int i = 0; i < count; i++)
        {
            yield return wait;
        }
        LightSwap();
        StartLightTimer(); //This is super gross but w/e
    }

}
