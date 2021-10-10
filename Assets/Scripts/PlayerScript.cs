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
    }

    public bool isDying()
    {
        return timeToDeath > 0;
    }

    private bool cantMove()
    {
        return isLight || isDying();
    }

    private void FixedUpdate()
    {
        if (transitioning)
            transition();
        movePlayer();
    }

    private void transition()
    {
        // Transition Magic
        if (transitionDelay > 0)
        {
            transitionDelay -= 1 / 60f;
        }
        else
        {
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
                    loadNextLevel();
                    avatarBody.velocity = new Vector2(0, 0);
                }
            }
            leftCurtain.transform.localScale = new Vector3(100 * transitionTimer, 51, 1);
            rightCurtain.transform.localScale = new Vector3(100 * transitionTimer, 51, 1);
        }
    }

    private void loadNextLevel()
    {
        // Temporary Scene Change for now
        if (SceneManager.GetActiveScene().name.Equals("MasonTestScene2"))
            SceneManager.LoadScene("MasonTestScene");
        else
            SceneManager.LoadScene("MasonTestScene2");
    }

    private void movePlayer()
    {
        // Player Acceleration
        if (Input.GetKey(KeyCode.W) && !cantMove())
        {
            avatarBody.velocity = new Vector2(avatarBody.velocity.x, avatarBody.velocity.y + acceleration);
        }
        if (Input.GetKey(KeyCode.S) && !cantMove())
        {
            avatarBody.velocity = new Vector2(avatarBody.velocity.x, avatarBody.velocity.y - acceleration);
        }
        if (Input.GetKey(KeyCode.D) && !cantMove())
        {
            avatarBody.velocity = new Vector2(avatarBody.velocity.x + acceleration, avatarBody.velocity.y);
        }
        if (Input.GetKey(KeyCode.A) && !cantMove())
        {
            avatarBody.velocity = new Vector2(avatarBody.velocity.x - acceleration, avatarBody.velocity.y);
        }
        // Speed Cap
        if (avatarBody.velocity.magnitude > maxSpeed)
        {
            float magnitude = avatarBody.velocity.magnitude;
            float counterMagnitude = -1 * (magnitude - maxSpeed) / magnitude;
            Vector2 counterVelocity = new Vector2(avatarBody.velocity.x * counterMagnitude, avatarBody.velocity.y * counterMagnitude);
            avatarBody.velocity = new Vector2(avatarBody.velocity.x + counterVelocity.x, avatarBody.velocity.y + counterVelocity.y);
        }
        // Natural Deceleration
        // Horizontally
        if ((!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)) || cantMove())
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
        // Vertically
        if ((!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) || cantMove())
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

        if (isDying())
        {
            timeToDeath--;
            if (timeToDeath <= 0)
            {
                PlayerDeath();
            }
        }
    }

    public void newStartPosition(Vector2 position)
    {
        startposition = position;
        avatar.transform.position = startposition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Light") && !transitioning && !isDying())
        {
            timeToDeath = 60;
        }
        /*
        // Still Necessary? I don't think so...
        if (other.tag.Equals("Start"))
        {
            // Resetting the start position to the spawn whenever the player comes into contact
            startposition = other.transform.position;
        }
        */
        if (other.tag.Equals("Level Complete") && !isDying())
        {
            startTransition();
        }
    }

    private void startTransition()
    {
        transitioning = true;
        transitionTimer = 0;
        transitionClosing = false;
        transitionDelay = 1f;
        isLight = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Empty for now
    }

    public void PlayerDeath()
    {
        avatar.transform.position = startposition;
        avatarBody.velocity = new Vector2(0, 0);
        // Light on for a second maybe?
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
