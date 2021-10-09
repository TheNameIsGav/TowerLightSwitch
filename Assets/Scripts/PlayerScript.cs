using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{

    //Publics
    public static PlayerScript instance;

    //Privates
    /*
     * The boolean determines whether the map is being lit up
     */
    private bool isLight;

    // Level Transition
    private bool transitioning = false;
    private float transitionDelay;
    private float transitionTimer;
    private bool transitionClosing = true;
    public SpriteRenderer leftCurtain;
    public SpriteRenderer rightCurtain;

    // Player Things
    public SpriteRenderer avatar;
    private Rigidbody2D avatarBody;
    public float acceleration;
    public float maxSpeed;
    private int timeToDeath = -1;
    private Vector2 startposition;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(instance);
    }

    void Start()
    {
        avatarBody = transform.GetComponent<Rigidbody2D>();
        startposition = GameObject.FindGameObjectWithTag("Start").transform.position;
        avatar.transform.position = startposition;
    }

    private void FixedUpdate()
    {
        // Transition Magic
        if (transitioning)
        {
            if (transitionDelay > 0)
            {
                transitionDelay -= 1 / 60f;
                Debug.Log("TransitionDelay: "+transitionDelay);
            }
            else
            {
                Debug.Log("TransitionTimer: " + transitionTimer);
                if (transitionClosing)
                {
                    transitionTimer -= 1 / 60f;
                    if (transitionTimer <= 0)
                    {
                        isLight = false;
                        transitioning = false;
                    }
                }
                else
                {
                    transitionTimer += 1 / 60f;
                    if (transitionTimer >= 1)
                    {
                        transitionClosing = true;
                        //Scene scene = SceneManager.GetSceneByName("MasonTestScene2");
                        // SceneManager.LoadScene("MasonTestScene2");
                        startposition = GameObject.FindGameObjectWithTag("Start").transform.position;
                        avatar.transform.position = startposition;
                        avatarBody.velocity = new Vector2(0, 0);
                    }
                }
                leftCurtain.transform.localScale = new Vector3(100 * transitionTimer, 51, 1);
                rightCurtain.transform.localScale = new Vector3(100 * transitionTimer, 51, 1);
            }
        }

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
            timeToDeath = 60;
        }
        if (other.tag.Equals("Start"))
        {
            startposition = other.transform.position;
        }
        if (other.tag.Equals("Level Complete"))
        {
            // Transition Sequence Begins
            transitioning = true;
            transitionTimer = 0;
            transitionClosing = false;
            transitionDelay = 1f;
            isLight = true;
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
        avatarBody.velocity = new Vector2(0, 0);
        // Light on second
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
