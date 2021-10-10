using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{

    //Publics
    /// <summary>
    /// Making the player a singleton.
    /// </summary>
    public static PlayerScript instance;

    //Privates
    /// <summary>
    /// The boolean determines whether the map is being lit up.
    /// </summary>
    private bool isLight;

    // Level Transition
    /// <summary>
    /// Boolean for whether or not a transition is happening.
    /// </summary>
    private bool transitioning = false;
    /// <summary>
    /// Time until the curtains close.
    /// </summary>
    private float transitionDelay;
    /// <summary>
    /// Duration/transition of curtains and time progressing.
    /// </summary>
    private float transitionTimer;
    /// <summary>
    /// Boolean controlling whether the curtains are closing (true) or opening (false).
    /// </summary>
    private bool transitionClosing = true;
    /// <summary>
    /// Access to the left curtain
    /// </summary>
    public SpriteRenderer leftCurtain;
    /// <summary>
    /// Access to the right curtain
    /// </summary>
    public SpriteRenderer rightCurtain;

    // Player Things
    /// <summary>
    /// The sprite of the avatar
    /// </summary>
    public SpriteRenderer avatar;
    /// <summary>
    /// The rigibody for the player, most utilized for the velocity.
    /// </summary>
    private Rigidbody2D avatarBody;
    /// <summary>
    /// How much the player accelerates
    /// </summary>
    public float acceleration;
    /// <summary>
    /// Maximum speed the player can move
    /// </summary>
    public float maxSpeed;
    /// <summary>
    /// Timer until death, giving time for soundbyte.
    /// </summary>
    private int timeToDeath = -1;
    /// <summary>
    /// Vector Position where the player spawns and respawns
    /// </summary>
    private Vector2 startposition;

    /// <summary>
    /// Makes the player immortal and transdimensional.
    /// </summary>
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(instance);
    }

    /// <summary>
    /// Start function. Gets the rigibody
    /// </summary>
    void Start()
    {
        avatarBody = transform.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Checks if the player is dying via the death timer
    /// </summary>
    public bool isDying()
    {
        return timeToDeath > 0;
    }

    /// <summary>
    /// Checks if the light is on or the player is dying, meaning that the player can't move.
    /// </summary>
    private bool cantMove()
    {
        return isLight || isDying();
    }

    /// <summary>
    /// Updates the transition as necessary and moves the player
    /// </summary>
    private void FixedUpdate()
    {
        if (transitioning)
            transition();
        movePlayer();
    }

    /// <summary>
    /// Transition sequence updater, curtains, level loading, and other stuff
    /// </summary>
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

    /// <summary>
    /// Unfinished function that loads the next level while transitioning, specifically once the curtains close
    /// </summary>
    private void loadNextLevel()
    {
        // Temporary Scene Change for now
        if (SceneManager.GetActiveScene().name.Equals("MasonTestScene2"))
            SceneManager.LoadScene("MasonTestScene");
        else
            SceneManager.LoadScene("MasonTestScene2");
    }

    /// <summary>
    /// Moves the player based off of the keys, and decelerates the player as necessary. 
    /// </summary>
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

    /// <summary>
    /// It sets the spawn position of the pllayer in the new level and moves her there. 
    /// Called by the start tile upon activation.
    /// </summary>
    public void newStartPosition(Vector2 position)
    {
        startposition = position;
        avatar.transform.position = startposition;
    }

    /// <summary>
    /// Where the colisions happen. 2 Possibilities: 
    /// <para> 1. You touch a light. Death timer starts. </para>
    /// <para> 2. You reach the level goal. The startTransition function is called. </para>
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Light") && !transitioning && !isDying())
        {
            timeToDeath = 60;
        }
        if (other.tag.Equals("Level Complete") && !isDying())
        {
            startTransition();
        }
    }

    /// <summary>
    /// Starts the transition to the next level after the next level tile is collided with.
    /// </summary>
    private void startTransition()
    {
        transitioning = true;
        transitionTimer = 0;
        transitionClosing = false;
        transitionDelay = 1f;
        isLight = true;
    }

    /// <summary>
    /// This is the function called to 'kill' the player and respawn it after the death timer hits zero
    /// </summary>
    public void PlayerDeath()
    {
        avatar.transform.position = startposition;
        avatarBody.velocity = new Vector2(0, 0);
        timeToDeath = -1;
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
