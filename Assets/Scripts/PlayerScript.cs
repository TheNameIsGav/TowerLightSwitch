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
    public float acceleration;
    private Vector2 velocity;
    private int timeToDeath = -1;
    // Hitboxes of Snake people do not include head

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(instance);
    }

    void Start()
    {
        avatarBody = transform.GetComponent<Rigidbody2D>();
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

        if (avatarBody.velocity.magnitude > 30 || (avatarBody.velocity.magnitude < .1 && isLight))
        {
            LightSwap();
            if (isLight)
                cam.backgroundColor = new Color(254,254,254); 
            else
                cam.backgroundColor = new Color(0, 0, 0);
        }
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
            timeToDeath = 10;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {

    }

    public void PlayerDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
