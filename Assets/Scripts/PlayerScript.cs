using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerScript : MonoBehaviour
{

    //Publics
    public Animator animator;
    public static PlayerScript instance;

    public SpriteRenderer leftCurtain;
    public SpriteRenderer rightCurtain;
    public SpriteRenderer avatar;

    public int lightScore;

    public float acceleration;
    public float maxSpeed;

    //Privates
    private bool isLight;
    private bool transitioning = false;
    private bool transitionClosing = true;

    private Light areaLight;

    private Rigidbody2D avatarBody;

    private Vector2 startposition;

    private int timeToDeath = -1;

    public int lightTime = 0;
    public string lightText = "";

    private float transitionDelay;
    private float transitionTimer;
    private int color = 0;

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
        areaLight = transform.GetComponent<Light>();
        leftCurtain.gameObject.SetActive(false);
        rightCurtain.gameObject.SetActive(false);
        //StartLightTimer();
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
        return isLight || isDying() || transitioning;
    }

    /// <summary>
    /// Updates the transition as necessary and moves the player
    /// </summary>
    private void FixedUpdate()
    {
        if (transitioning)
            transition();
        movePlayer();
        animator.SetInteger("color", color);
        animator.SetInteger("pSpeed", Mathf.Abs((int) avatarBody.velocity.magnitude));
        Debug.Log((int)avatarBody.velocity.x);
        if (((int)avatarBody.velocity.x) > 0){
            avatar.flipX = false;
        }
        else
        {
            avatar.flipX = true;
        }
        //Debug.Log((int)avatarBody.velocity.magnitude);
       
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
                isLight = false;
                if (transitionTimer <= 0)
                {
                    isLight = false;
                    transitioning = false;
                    leftCurtain.gameObject.SetActive(false);
                    rightCurtain.gameObject.SetActive(false);
                    StopAllCoroutines();
                    StartLightTimer();
                }
            }
            else
            {
                transitionTimer += 1 / 60f;
                if (transitionTimer >= 1)
                {
                    transitionClosing = true;
                    loadNextLevel();
                    isLight = false;
                    avatarBody.velocity = new Vector2(0, 0);
                }
            }
            leftCurtain.transform.localScale = new Vector3(60 * transitionTimer, 51, 1);
            rightCurtain.transform.localScale = new Vector3(60 * transitionTimer, 51, 1);
        }

    }

    /// <summary>
    /// loads the next level while transitioning, specifically once the curtains close
    /// </summary>
    private int currLevel = 1;
    private void loadNextLevel()
    {
        if (currLevel < 10) {
            if(lightScore < PlayerPrefs.GetInt("Level" + currLevel))
            {
                PlayerPrefs.SetInt("Level" + currLevel, lightScore);
            }
            currLevel++;
            SceneManager.LoadScene("Level " + currLevel);
            lightScore = 0;
        }
        else { SceneManager.LoadScene("GameOver"); isLight = true; }
    }

    /// <summary>
    /// Moves the player based off of the keys, and decelerates the player as necessary. 
    /// </summary>
    private void movePlayer()
    {
        // Player Acceleration
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && !cantMove())
        {
            avatarBody.velocity = new Vector2(avatarBody.velocity.x, avatarBody.velocity.y + acceleration);
        }
        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && !cantMove())
        {
            avatarBody.velocity = new Vector2(avatarBody.velocity.x, avatarBody.velocity.y - acceleration);
        }
        if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && !cantMove())
        {
            avatarBody.velocity = new Vector2(avatarBody.velocity.x + acceleration, avatarBody.velocity.y);
        }
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && !cantMove())
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
        if ((!(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            && !(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))) 
            || cantMove())
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
        if ((!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) 
            && !(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))) 
            || cantMove())
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
                avatar.color = new Color(255, 255, 255);
                PlayerDeath();
            }
        }
    }

    private void Update()
    {
        if (isLight) { areaLight.intensity = 1;
        } else { areaLight.intensity = -1; }
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

    //
    public void startPlayerDeath()
    {
        if (!cantMove())
        {
            timeToDeath = 10;
            avatar.color = new Color(255,0,0);
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
    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Light") && !transitioning && !isDying())
        {
            
        }
        if (other.tag.Equals("Level Complete") && !isDying())
        {
            startTransition();
        }
    }*/

    /// <summary>
    /// Called by the EndObject to trigger the end of a level
    /// </summary>
    public void OnFinish()
    {
        startTransition();
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
        StopCoroutine(iterateTime());
        isLight = true;
        leftCurtain.gameObject.SetActive(true);
        rightCurtain.gameObject.SetActive(true);
        leftCurtain.transform.position = new Vector2(-30, 0);
        rightCurtain.transform.position = new Vector2(30, 0);
    }

    //Gav's Function's

    /// <summary>
    /// Swaps the current state of Light
    /// </summary>
    public void LightSwap() { if (isLight) { lightScore++; } isLight = !isLight;  }

    /// <summary>
    /// Gets the current state of Light
    /// </summary>
    public bool GetLightState() { return isLight; }

    /// <summary>
    /// Function to handle the on and off of lights
    /// </summary>
   public void StartLightTimer()
    {
        StartCoroutine(iterateTime());
    }

    public void ColorUp()
    {
        color = ((color + 1) % 3);
    }
    public void ColorDown()
    {
        color = color - 1;
        if (color < 0)
        {
            color = 2;
        }
    }
    IEnumerator iterateTime()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);
        int count = isLight ? 5 : 10; //If we are light, then wait 5 seconds, if we are dark then wait 10 seconds
        for (int i = 0; i < count; i++)
        {
            if (count == 5)
            {
                lightText = "Time Till Dark: ";
                lightTime = 5 - i;
            }
            else
            {
                lightText = "Time Till Light: ";
                lightTime = 10 - i;
            }
            yield return wait;
        }
        LightSwap();
        StartLightTimer(); //This is super gross but w/e
    }

}
