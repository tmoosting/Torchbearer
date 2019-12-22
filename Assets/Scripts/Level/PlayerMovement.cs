//This is the controller for player movement. It enables the player to walk, jump, dash, and change animations. Should be used on the player gameobject
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask groundLayer; //uses the Ground layer to see if the player is standing on ground
    public LayerMask hazardLayer; //uses the Hazard layer so players cant avoid hazards by holding down jump
    public Ghost ghost; //the Ghost Script, used to create afterimages when dashing
    public GameObject cineMachine;  //Cinemachine
    public GameObject camObj;       //Main camera, used together with cinemachine to fix the camera when respawning

    //Player Components
    Rigidbody2D PlayerRB;
    BoxCollider2D PlayerBox;
    SpriteRenderer PlayerSprite;
    public Animator PlayerAnimator;
    public Animator BeaconAnimator;
    public Animator LitAnimator;
    public Animator NotLitAnimator;
    //Player Audio Components
    public AudioSource PlayerAudio;
    public AudioSource BGM;
    public AudioClip PlayerStep;
    public AudioClip PlayerJump;
    public AudioClip PlayerDash;
    public AudioClip PlayerDamage;
    public AudioClip PlayerCast;
    public AudioClip LevelCleared;
    public AudioClip LevelFailed;
    public float PitchRange = 0.2f; //Maximum deviation from starting pitch
    private float OriginalPitch; //.9 is recommended
    //Cinemachine components
    CinemachineFramingTransposer CineTransposer;
    CinemachineVirtualCamera vcam;
    //Variables for basic movement

    public float maxspeed = 11f; //Player max running speed
    public float jumpvelocity = 14f; // Player initial jump velocity
    private bool inControl = true; //Can the player control his character? (used for damage and cutscenes)
    private float WaitControlTimer = 0f;//Timer till player regains control
    private bool invincible = false; //Can the player be hurt right now?
    private float invincibleTimer = 0f; //timer for invincibility
    private float flashTime = 0.2f; //Time between sprite color flashes during invincibility
    private float flashTimer = 0f; //timer for flashes
    public Color flashColor; //Sprite color while flashing
    public Color normalColor; //sprite color when normal
    private float knockbackSpeed = 20f;//knockback from damage
    private bool respawn = false;
    private bool dying = false;
    private bool deathHandled = false;
    public int endingAnimation = 0;//0 not active, 1 waittillbeaconhit, 2 castanimation, 3 wait, 4 walkoffscreen

    public bool isFalling = false; //For animation, is the player falling?
    private bool isJumping = false;
    private float lowestY = -30f; //Under what y value does the player respawn
    private float yThreshold = 11.0f; //Velocity threshold for jumping
    private float negyThreshold = -3.0f; //Velocity threshold for falling
    private float walkingyThreshold = 0.1f;
    private float fallDamageThreshold = 25f;
    private float fallY;
    //Variables for dashing
    public float dashSpeed = 25f; //Velocity during dash
    public float startDashTime = 0.2f;//Dash duration
    private float dashTime;//Keeps track of leftover dash duration
    private float dashHor = 0f; //Dash horizontal direction
    private float dashVert = 0f; //Dash vertical direction
    private bool dashing = false; //Is the player currently dashing?
    public bool dashAvailable = false; //Can the player dash? (dash resets when on ground)
    public bool freeDash = false; //Can the player infinitely dash?
    public bool inDashPath = false;
    public float dashAngle = 1f;
    private bool DashPathActivated = false;
    private bool wasInPath = false;
    private bool hasPlayed = false;
  
    private int PlayerLives = 3; // Player lives
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyContainer;
    public Image dashtracker;
    public Sprite fullDash;
    public Slider ghostSlider;
    private float timeSpent = 0f;
    public float maxTime = 10f;
    public bool withinTime = true;

    private Vector3 spawnPoint;//The players starting point


    // Start is called before the first frame update
    void Start()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        PlayerBox = GetComponent<BoxCollider2D>();
        PlayerSprite = GetComponent<SpriteRenderer>();
        PlayerAnimator = GetComponent<Animator>();

        vcam = cineMachine.GetComponent<CinemachineVirtualCamera>();
        CineTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();

        dashTime = startDashTime; //Sets first dash to have full duration
        spawnPoint = transform.position; //Gets original player position as spawnpoint
        OriginalPitch = PlayerAudio.pitch;//Saves the original pitch
        SceneController.Instance.levelFullyLoaded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dying && !deathHandled && PlayerLives == 0)
        {
            Die();
        }
        if (!dying)
        {
            if (endingAnimation == 0 && withinTime)
            {
                if (timeSpent >= maxTime)
                {
                    withinTime = false;
                }
                else
                {
                    ghostSlider.value = timeSpent / maxTime;
                    timeSpent += Time.deltaTime;
                }
            }
            if (dashAvailable || freeDash)
            {
                dashtracker.sprite = fullDash;
            }
            else if (!dashAvailable && !freeDash)
            {
                dashtracker.sprite = emptyContainer;
            }
            if (transform.position.y < lowestY) //If the player is under the lowest y limit
            {
                Die();
            }
            if (invincible)
            {
                invincibleTimer -= Time.deltaTime;
                if (invincibleTimer <= 0)
                {
                    invincible = false;
                    PlayerSprite.color = normalColor;
                }
                else
                {
                    flashTimer -= Time.deltaTime;
                    if (flashTimer < 0)
                    {
                        if (PlayerSprite.color == normalColor)
                        {
                            PlayerSprite.color = flashColor;
                        }
                        else
                        {
                            PlayerSprite.color = normalColor;
                        }
                        flashTimer = flashTime;
                    }
                }
            }
            if (inControl) //is the player in control of his character
            {
                if (!dashing)//If the player is not currently dashing
                {
                    if (PlayerRB.velocity.y < yThreshold && PlayerRB.velocity.y > walkingyThreshold)
                    {
                        isJumping = false;
                    }
                    if (IsGrounded(0.8f)) //If the player is currently standing on the ground
                    {
                        if (!dashAvailable && IsGrounded(0.1f))
                        {
                            dashAvailable = true;
                        }
                        if (isFalling)
                        {
                            if (fallY - transform.position.y > fallDamageThreshold)
                            {
                                takeDamage(Vector2.zero);
                            }
                            isFalling = false;
                            PlayerAnimator.SetBool("Falling", false);
                        }
                    }
                    else if (PlayerRB.velocity.y < negyThreshold) //If the player is falling
                    {
                        if (!isFalling)
                        {
                            isFalling = true;
                            fallY = transform.position.y;
                        }
                        isJumping = false;
                        PlayerAnimator.SetBool("Jump", false);
                        PlayerAnimator.SetBool("Falling", true);
                    }
                    else if (PlayerRB.velocity.y > yThreshold)//If the player is jumping
                    {
                        isFalling = false;
                        isJumping = true;
                        PlayerAnimator.SetBool("Jump", true);
                        PlayerAnimator.SetBool("Falling", false);
                    }
                    if (!isJumping && !isFalling && IsGrounded(0.1f))
                    {
                        Vector2 vel = PlayerRB.velocity;
                        if (vel.y > walkingyThreshold)
                        {
                            vel.y = 1f;
                            PlayerRB.velocity = vel;
                        }
                    }
                    if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Z)) && (dashAvailable || freeDash) && endingAnimation == 0)//If the player presses down the space key and can dash
                    {
                        Dash();
                    }
                    else
                    {
                        float horizontal = Input.GetAxis("Horizontal"); //Gets a float between -1.0f and 1.0f depending on keypress
                        float vertical = Input.GetAxisRaw("Vertical"); //Gets a float that is exactly -1.0f, 0f or 1.0f depending on keypress
                        Vector2 vel = PlayerRB.velocity; //Get current player velocity
                        if (endingAnimation == 1)
                        {
                            horizontal = 0.5f;
                        }
                        else if (endingAnimation == 2 || endingAnimation == 3)
                        {
                            horizontal = 0f;
                        }
                        else if (endingAnimation == 4)
                        {
                            horizontal = 1f;
                            LitAnimator.SetTrigger("Lit");
                            if (!hasPlayed)
                            {
                                hasPlayed = true;
                                BGM.Stop();
                                BGM.clip = LevelCleared;
                                BGM.loop = false;
                                BGM.Play();
                            }
                            
                        }
                        vel.x = horizontal * maxspeed; //Alter horizontal velocity
                        PlayerAnimator.SetFloat("Speed", Mathf.Abs(horizontal * maxspeed));
                        if (horizontal != 0 && IsGrounded(0.1f))//If the player is moving and on the ground, play the footstep sound
                        {
                            PlayerAnimator.SetBool("Jump", false);
                            if (PlayerAudio.clip == PlayerStep)
                            {
                                if (PlayerAudio.isPlaying == false)
                                {
                                    PlayerAudio.pitch = Random.Range(OriginalPitch - PitchRange, OriginalPitch + PitchRange);
                                    PlayerAudio.Play();
                                }
                            }
                            else
                            {
                                if (PlayerAudio.isPlaying == false)
                                {
                                    PlayerAudio.clip = PlayerStep;
                                    PlayerAudio.pitch = Random.Range(OriginalPitch - PitchRange, OriginalPitch + PitchRange);
                                    PlayerAudio.Play();
                                }
                                else
                                {
                                    PlayerAudio.Stop();
                                    PlayerAudio.clip = PlayerStep;
                                    PlayerAudio.pitch = Random.Range(OriginalPitch - PitchRange, OriginalPitch + PitchRange);
                                    PlayerAudio.Play();
                                }
                            }


                        }
                        if (horizontal < 0) //Used to flip sprite based on movement direction
                        {
                            PlayerSprite.flipX = true;
                        }
                        else if (horizontal > 0)
                        {
                            PlayerSprite.flipX = false;
                        }
                        PlayerRB.velocity = vel; //Updates player velocity
                        if (vertical == 1 && endingAnimation == 0) //If the up arrow is pressed
                        {
                            Jump();
                        }
                    }
                }
                else //If we're currently dashing
                {
                    Dash();
                }
            }
            else
            {
                WaitControlTimer -= Time.deltaTime;
                if (WaitControlTimer <= 0 && !dying)
                {
                    inControl = true;
                    PlayerRB.velocity = Vector2.zero;
                }
            }
        }
        else
        {
            if (PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName("DeathEnd") && !deathHandled)
            {
                deathHandled = true;
                DeathHandler();
            }
        }
    }

    void Dash() //Function that causes the player to dash in a certain direction for a given amount of time at a given speed
    {
        if (!dashing)
        {
            dashHor = Input.GetAxisRaw("Horizontal");
            dashVert = Input.GetAxisRaw("Vertical");
        }
        if (!(dashHor == 0 && dashVert == 0)) //If a direction has been given
        {
            if (!dashing) //If it's the first iteration, initiate a dash, create afterimage and change the animation
            {
                dashAvailable = false;
                if (!freeDash)
                {
                    dashtracker.sprite = emptyContainer;
                }
                isFalling = false;
                dashing = true;
                PlayerAnimator.SetBool("Jump", false);
                PlayerAnimator.SetBool("Falling", false);
                PlayerAnimator.SetBool("Dashing", true);
                ghost.makeGhost = true;
                PlayerAudio.Stop();
                PlayerAudio.clip = PlayerDash;
                PlayerAudio.pitch = Random.Range(OriginalPitch - PitchRange, OriginalPitch + PitchRange);
                PlayerAudio.Play();
            }
            float pythDash = Mathf.Sqrt(dashSpeed * dashSpeed + dashSpeed * dashSpeed); //Used to make the distance of dashes on 1 axis the same as diagonal dashes
            if (dashHor == -1 && dashVert == 0) //Dash left
            {
                PlayerRB.velocity = new Vector2(-1f * pythDash, 0f);
                if (inDashPath && dashAngle == 180)
                {
                    DashPathActivated = true;
                    wasInPath = true;
                }
            }
            else if (dashHor == -1 && dashVert == 1)//Dash top left
            {
                PlayerRB.velocity = new Vector2(-1f * dashSpeed, 1f * dashSpeed);
                if (inDashPath && dashAngle == 135)
                {
                    DashPathActivated = true;
                    wasInPath = true;
                }
            }
            else if (dashHor == 0 && dashVert == 1)//Dash up
            {
                PlayerRB.velocity = new Vector2(0f, 1f * pythDash);
                if (inDashPath && dashAngle == 90)
                {
                    DashPathActivated = true;
                    wasInPath = true;
                }
            }
            else if (dashHor == 1 && dashVert == 1)//Dash top right
            {
                PlayerRB.velocity = new Vector2(1f * dashSpeed, 1f * dashSpeed);
                if (inDashPath && dashAngle == 45)
                {
                    DashPathActivated = true;
                    wasInPath = true;
                }
            }
            else if (dashHor == 1 && dashVert == 0)//Dash right
            {
                PlayerRB.velocity = new Vector2(1f * pythDash, 0f);
                if (inDashPath && dashAngle == 0)
                {
                    DashPathActivated = true;
                    wasInPath = true;
                }
            }
            else if (dashHor == 1 && dashVert == -1)//Dash bottom right
            {
                PlayerRB.velocity = new Vector2(1f * dashSpeed, -1f * dashSpeed);
                if (inDashPath && dashAngle == 315)
                {
                    DashPathActivated = true;
                    wasInPath = true;
                }
            }
            else if (dashHor == 0 && dashVert == -1)//Dash bottom
            {
                PlayerRB.velocity = new Vector2(0f, -1f * pythDash);
                if (inDashPath && dashAngle == 270)
                {
                    DashPathActivated = true;
                    wasInPath = true;
                }
            }
            else if (dashHor == -1 && dashVert == -1)//Dash bottom left
            {
                PlayerRB.velocity = new Vector2(-1f * dashSpeed, -1f * dashSpeed);
                if (inDashPath && dashAngle == 225)
                {
                    DashPathActivated = true;
                    wasInPath = true;
                }
            }
            if (!inDashPath)
            {
                DashPathActivated = false;
            }
            if (dashTime <= 0)//If the dash is done
            {
                if (DashPathActivated)
                {
                    dashTime = startDashTime;
                    PlayerAudio.Stop();
                    PlayerAudio.clip = PlayerDash;
                    PlayerAudio.pitch = Random.Range(OriginalPitch - PitchRange, OriginalPitch + PitchRange);
                    PlayerAudio.Play();
                }
                else
                {
                    dashHor = 0f;
                    dashVert = 0f;
                    dashTime = startDashTime;
                    dashAngle = 1f;
                    if (!wasInPath)
                    {
                        PlayerRB.velocity = new Vector2(0f, 0f); //Reset velocity
                    }
                    else
                    {
                        PlayerRB.velocity = new Vector2(dashHor * dashSpeed, dashVert * dashSpeed);
                    }
                    wasInPath = false;
                    dashing = false;
                    PlayerAnimator.SetBool("Dashing", false);
                    ghost.makeGhost = false;
                }
            }
            else //If the dash is not yet done
            {
                dashTime -= Time.deltaTime;
            }
        }
    }

    void Jump() //Function to make the player jump
    {
        if (IsGrounded(0.3f))//If the player is standing on the ground
        {
            dashAvailable = true;
            PlayerAnimator.SetBool("Jump", true);
            Vector2 vel = PlayerRB.velocity;
            vel.y = jumpvelocity; //Give the player vertical velocity
            PlayerRB.velocity = vel;
            PlayerAudio.Stop();
            PlayerAudio.clip = PlayerJump;
            PlayerAudio.pitch = Random.Range(OriginalPitch - PitchRange, OriginalPitch + PitchRange);
            PlayerAudio.Play();
        }
    }

    bool IsGrounded(float offset)//Checks if the player is standing on the ground, by checking for the groundlayer with raycasts on 3 points below the player.
    {
        Vector2 position = transform.position; //Gets player position
        position.y -= 1.40f;
        float x = (PlayerBox.bounds.size.x / 2);//Gets half the width of the player boxcollider
        Vector2 direction = Vector2.down;//(0,-1)
        float distance = 0.1f + offset;//Distance the raycast travels

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer); //Raycast in the middle of the player box collider
        if (hit.collider != null)//If ground layer has been hit
        {
            return true;
        }
        position.x -= x;
        hit = Physics2D.Raycast(position, direction, distance, groundLayer);//Raycast at the left edge of the player box collider
        if (hit.collider != null)
        {
            return true;
        }
        position.x += x*2;
        hit = Physics2D.Raycast(position, direction, distance, groundLayer);//Raycast at the right edge of the player box collider
        if (hit.collider != null)
        {
            return true;
        }
        if (invincible)
        {
            position = transform.position;
            position.y -= 1.40f;
            hit = Physics2D.Raycast(position, direction, distance, hazardLayer); //Raycast in the middle of the player box collider
            if (hit.collider != null)//If ground layer has been hit
            {
                return true;
            }
            position.x -= x;
            hit = Physics2D.Raycast(position, direction, distance, hazardLayer);//Raycast at the left edge of the player box collider
            if (hit.collider != null)
            {
                return true;
            }
            position.x += x * 2;
            hit = Physics2D.Raycast(position, direction, distance, hazardLayer);//Raycast at the right edge of the player box collider
            if (hit.collider != null)
            {
                return true;
            }
        }
        return false;
    }

    void Respawn ()//Function to respawn the player to his starting position
    {
        PlayerRB.bodyType = RigidbodyType2D.Dynamic;
        vcam.enabled = false;//Disable the CineMachine
        Vector2 Vel = PlayerRB.velocity;
        Vel.x = 0f;
        Vel.y = 0f;
        PlayerRB.velocity = Vel;

        transform.position = spawnPoint;//Reset the player's position
        camObj.GetComponent<Transform>().position = spawnPoint;//Move the camera's position
        vcam.PreviousStateIsValid = false;//Let's the CineMachine know its previous position should be ignored when calculating
        vcam.enabled = true;//Enable the CineMachine
        PlayerAnimator.SetTrigger("Respawn");
        dashHor = 0f;
        dashVert = 0f;
        dashTime = startDashTime;
        dashing = false;
        isFalling = false;
        isJumping = false;
        ghost.makeGhost = false;
    }
    void Die()
    {
        BGM.Stop();
        BGM.clip = LevelFailed;
        BGM.loop = false;
        BGM.Play();
        PlayerLives = 0;
        foreach (Image heart in hearts)
        {
            heart.sprite = emptyContainer;
        }
        inControl = false;
        dying = true;
        NotLitAnimator.SetTrigger("NotLit");
        PlayerAnimator.SetBool("Dashing", false);
        PlayerAnimator.SetBool("Falling", false);
        PlayerAnimator.SetBool("Jump", false);
        ghost.makeGhost = false;
        PlayerAnimator.SetTrigger("Dying");
        PlayerRB.bodyType = RigidbodyType2D.Static;
    }

    void DeathHandler()
    { 
        if (!respawn)
        {
            SceneController.Instance.EndLevel(false, false);
        }
        else
        {
            foreach (Image heart in hearts)
            {
                heart.sprite = fullHeart;
            }
            PlayerLives = 3;
            inControl = true;
            dying = false;
            Respawn();
        }
    }

    public void takeDamage(Vector2 direction)
    {
        if (!invincible)
        {
            PlayerLives -= 1;
            if (PlayerLives == 0)
            {
                hearts[PlayerLives].sprite = emptyContainer;
                PlayerAudio.Stop();
                PlayerAudio.clip = PlayerDamage;
                PlayerAudio.pitch = Random.Range(OriginalPitch - PitchRange, OriginalPitch + PitchRange);
                PlayerAudio.Play();
                Die();
            }
            else
            {
                if (PlayerLives >= 0)
                {
                    hearts[PlayerLives].sprite = emptyContainer;
                }
                dashHor = 0f;
                dashVert = 0f;
                dashTime = startDashTime;
                dashing = false;
                PlayerAnimator.SetBool("Dashing", false);
                PlayerAnimator.SetBool("Falling", false);
                PlayerAnimator.SetBool("Jump", false);
                ghost.makeGhost = false;
                inControl = false;
                invincible = true;
                invincibleTimer = 1f;
                WaitControlTimer = 0.2f;
                flashTimer = flashTime;
                direction.Normalize();
                direction.x *= knockbackSpeed;
                direction.y *= knockbackSpeed;
                PlayerRB.velocity = direction;
                PlayerAnimator.SetTrigger("DamageTaken");
                PlayerAudio.Stop();
                PlayerAudio.clip = PlayerDamage;
                PlayerAudio.pitch = Random.Range(OriginalPitch - PitchRange, OriginalPitch + PitchRange);
                PlayerAudio.Play();
            }
        }
    }
    public void castEnd()
    {
        endingAnimation++;
        vcam.enabled = false;//Disable the CineMachine
        BeaconAnimator.SetTrigger("TurnOn");
    }

    public void LevelEnd()
    {
        PlayerRB.velocity = new Vector2(0f, 0f);
        endingAnimation++;
        inControl = false; 
    }

    public void castSound()
    {
        PlayerAudio.Stop();
        PlayerAudio.clip =PlayerCast;
        PlayerAudio.Play();
    }
}
