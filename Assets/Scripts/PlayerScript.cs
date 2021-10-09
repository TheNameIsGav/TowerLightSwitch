using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{

    //Publics
    public static PlayerScript instance;
    public Camera cam;

    //Privates
    /*
     * The boolean determines whether the map is being lit up
     */
    private bool isLight;

    // Player Things
    public SpriteRenderer avatar;
    private Rigidbody2D avatarBody;
    private Collider2D avaterTouch;
    public float acceleration;
    private float maxSpeed = 30;
    private Vector2 velocity;
    private int timeToDeath = -1;
    private Vector2 startposition;
    // Hitboxes of Snake people do not include head

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(instance);
    }

    void Start()
    {
        avatarBody = transform.GetComponent<Rigidbody2D>();
        //avaterTouch = transform.GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        // Player Acceleration
        if (Input.GetKey(KeyCode.W) && !isLight)
        {
            avatarBody.velocity = new Vector2(avatarBody.velocity.x, avatarBody.velocity.y + acceleration);
        }
        if (Input.GetKey(KeyCode.S) && !isLight)
        {
            avatarBody.velocity = new Vector2(avatarBody.velocity.x, avatarBody.velocity.y - acceleration);
        }
        if (Input.GetKey(KeyCode.D) && !isLight)
        {
            avatarBody.velocity = new Vector2(avatarBody.velocity.x + acceleration, avatarBody.velocity.y);
        }
        if (Input.GetKey(KeyCode.A) && !isLight)
        {
            avatarBody.velocity = new Vector2(avatarBody.velocity.x - acceleration, avatarBody.velocity.y);
        }
        // Player Deceleration
        if ((!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) || isLight)
        {
            if (System.Math.Abs(avatarBody.velocity.y) < acceleration)
            {
                avatarBody.velocity = new Vector2(avatarBody.velocity.x, 0);
            }
            else if (avatarBody.velocity.y > 0)
            {
                avatarBody.velocity = new Vector2(avatarBody.velocity.x, avatarBody.velocity.y - acceleration);
            }
            else
            {
                avatarBody.velocity = new Vector2(avatarBody.velocity.x, avatarBody.velocity.y + acceleration);
            }
        }
        // Speed Cap
        if (avatarBody.velocity.magnitude > maxSpeed)
        {
            // Debug.Log("Speeding: "+ avatarBody.velocity.magnitude);
            float magnitude = avatarBody.velocity.magnitude;
            float counterMagnitude = -1 * (magnitude - maxSpeed) / magnitude;
            // Debug.Log("$$$$$$$$: " + counterMagnitude);
            Vector2 counterVelocity = new Vector2(avatarBody.velocity.x * counterMagnitude, avatarBody.velocity.y * counterMagnitude);
            avatarBody.velocity = new Vector2(avatarBody.velocity.x + counterVelocity.x, avatarBody.velocity.y + counterVelocity.y);
            // Debug.Log("--------: " + avatarBody.velocity.magnitude);
        }
        // Natural Deceleration
        if ((!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) || isLight)
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

        // Light Test Apparatus
        /*
        if (avatarBody.velocity.magnitude > 30 || (avatarBody.velocity.magnitude < .1 && isLight))
        {
            LightSwap();
            if (isLight)
                cam.backgroundColor = new Color(254,254,254); 
            else
                cam.backgroundColor = new Color(0, 0, 0);
        }
        */

        if (timeToDeath > 0)
        {
            Debug.Log(timeToDeath);
            timeToDeath--;
            if (timeToDeath <= 0)
            {
                PlayerDeath();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Light"))
        {
            Debug.Log(other.ToString());
            timeToDeath = 100;
        }
        if (other.tag.Equals("Start"))
        {
            startposition = other.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Start"))
        {
            startposition = other.transform.position;
        }
    }

    public void PlayerDeath()
    {
        avatar.transform.position = startposition;
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

}
