//This is the controller for player movement. It enables the player to walk, jump, dash, and change animations. Should be used on the player gameobject
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask groundLayer; //uses the Ground layer to see if the player is standing on ground
    public Ghost ghost; //the Ghost Script, used to create afterimages when dashing
    public GameObject cineMachine;  //Cinemachine
    public GameObject camObj;       //Main camera, used together with cinemachine to fix the camera when respawning

    //Player Components
    Rigidbody2D PlayerRB;
    BoxCollider2D PlayerBox;
    SpriteRenderer PlayerSprite;
    Animator PlayerAnimator;
    //Cinemachine components
    CinemachineFramingTransposer CineTransposer;
    CinemachineVirtualCamera vcam;
    //Variables for basic movement
    private float maxspeed = 6f; //Player max running speed
    private float jumpvelocity = 10f; // Player initial jump velocity
    private bool isFalling = false; //For animation, is the player falling?
    private float lowestY = -30f; //Under what y value does the player respawn
    public float yThreshold = 3.0f; //Velocity threshold for falling (negative) and jumping (positive)
    //Variables for dashing
    public float dashSpeed = 20f; //Velocity during dash
    public float startDashTime = 0.2f;//Dash duration
    private float dashTime;//Keeps track of leftover dash duration
    private float dashHor = 0f; //Dash horizontal direction
    private float dashVert = 0f; //Dash vertical direction
    private bool dashing = false; //Is the player currently dashing?
    public bool dashAvailable = false; //Can the player dash? (dash resets when on ground)
    public bool freeDash = false; //Can the player infinitely dash?

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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < lowestY) //If the player is under the lowest y limit
        {
            Respawn();
        }
        if (!dashing)//If the player is not currently dashing
        {
            if (!dashAvailable || isFalling)//If the player has no dash available or if the player is currently falling
            {
                if (IsGrounded()) //If the player is currently standing on the ground
                {
                    dashAvailable = true;
                    isFalling = false;
                    PlayerAnimator.SetBool("Falling", false);
                }
            }
            if (PlayerRB.velocity.y < -yThreshold) //If the player is falling
            {
                isFalling = true;
                PlayerAnimator.SetBool("Jump", false);
                PlayerAnimator.SetBool("Falling", true);
            }
            else if (PlayerRB.velocity.y > yThreshold)//If the player is jumping
            {
                isFalling = false;
                PlayerAnimator.SetBool("Jump", true);
                PlayerAnimator.SetBool("Falling", false);
            } 
            if (Input.GetKeyDown(KeyCode.Space))//If the player presses down the space key
            {
                if (dashAvailable  || freeDash) // If the player can use a dash or has unlimited dashes
                {
                    Dash();
                }
            }
            else
            {
                float horizontal = Input.GetAxis("Horizontal"); //Gets a float between -1.0f and 1.0f depending on keypress
                float vertical = Input.GetAxisRaw("Vertical"); //Gets a float that is exactly -1.0f, 0f or 1.0f depending on keypress
                Vector2 vel = PlayerRB.velocity; //Get current player velocity
                vel.x = horizontal * maxspeed; //Alter horizontal velocity
                PlayerAnimator.SetFloat("Speed", Mathf.Abs(horizontal * maxspeed));
                if (horizontal < 0) //Used to flip sprite based on movement direction
                {
                    PlayerSprite.flipX = true;
                }
                else if (horizontal > 0)
                {
                    PlayerSprite.flipX = false;
                }
                PlayerRB.velocity = vel; //Updates player velocity
                if (vertical == 1) //If the up arrow is pressed
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

    void Dash() //Function that causes the player to dash in a certain direction for a given amount of time at a given speed
    {
        if (!(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)) //If a direction has been given
        {
            if (!dashing) //If it's the first iteration, initiate a dash, create afterimage and change the animation
            {
                dashAvailable = false;
                dashHor = Input.GetAxisRaw("Horizontal");
                dashVert = Input.GetAxisRaw("Vertical");
                dashing = true;
                PlayerAnimator.SetBool("Jump", false);
                PlayerAnimator.SetBool("Falling", false);
                PlayerAnimator.SetBool("Dashing", true);
                ghost.makeGhost = true;
            }
            float pythDash = Mathf.Sqrt(dashSpeed * dashSpeed + dashSpeed * dashSpeed); //Used to make the distance of dashes on 1 axis the same as diagonal dashes
            if (dashHor == -1 && dashVert == 0) //Dash left
            {
                PlayerRB.velocity = new Vector2(-1f * pythDash, 0f);
            }
            else if (dashHor == -1 && dashVert == 1)//Dash top left
            {
                PlayerRB.velocity = new Vector2(-1f * dashSpeed, 1f * dashSpeed);
            }
            else if (dashHor == 0 && dashVert == 1)//Dash up
            {
                PlayerRB.velocity = new Vector2(0f, 1f * pythDash);
            }
            else if (dashHor == 1 && dashVert == 1)//Dash top right
            {
                PlayerRB.velocity = new Vector2(1f * dashSpeed, 1f * dashSpeed);
            }
            else if (dashHor == 1 && dashVert == 0)//Dash right
            {
                PlayerRB.velocity = new Vector2(1f * pythDash, 0f);
            }
            else if (dashHor == 1 && dashVert == -1)//Dash bottom right
            {
                PlayerRB.velocity = new Vector2(1f * dashSpeed, -1f * dashSpeed);
            }
            else if (dashHor == 0 && dashVert == -1)//Dash bottom
            {
                PlayerRB.velocity = new Vector2(0f, -1f * pythDash);
            }
            else if (dashHor == -1 && dashVert == -1)//Dash bottom left
            {
                PlayerRB.velocity = new Vector2(-1f * dashSpeed, -1f * dashSpeed);
            }
            if (dashTime <= 0)//If the dash is done
            {
                dashHor = 0f;
                dashVert = 0f;
                dashTime = startDashTime;
                PlayerRB.velocity = new Vector2(0f, 0f); //Reset velocity
                dashing = false;
                PlayerAnimator.SetBool("Dashing", false);
                ghost.makeGhost = false;
            }
            else //If the dash is not yet done
            {
                dashTime -= Time.deltaTime;
            }
        }
    }

    void Jump() //Function to make the player jump
    {
        if (IsGrounded())//If the player is standing on the ground
        {
            dashAvailable = true;
            PlayerAnimator.SetBool("Jump", true);
            Vector2 vel = PlayerRB.velocity;
            vel.y = jumpvelocity; //Give the player vertical velocity
            PlayerRB.velocity = vel;
        }
    }

    bool IsGrounded()//Checks if the player is standing on the ground, by checking for the groundlayer with raycasts on 3 points below the player.
    {
        Vector2 position = transform.position; //Gets player position
        float x = PlayerBox.bounds.size.x / 2;//Gets half the width of the player boxcollider
        Vector2 direction = Vector2.down;//(0,-1)
        float distance = 1.5f;//Distance the raycast travels

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer); //Raycast in the middle of the player box collider
        if (hit.collider != null)//If ground layer has been hit
        {
            return true;
        }
        position.x -= x;
        RaycastHit2D hit2 = Physics2D.Raycast(position, direction, distance, groundLayer);//Raycast at the left edge of the player box collider
        if (hit2.collider != null)
        {
            return true;
        }
        position.x += x*2;
        RaycastHit2D hit3 = Physics2D.Raycast(position, direction, distance, groundLayer);//Raycast at the right edge of the player box collider
        if (hit3.collider != null)
        {
            return true;
        }
        return false;
    }

    void Respawn ()//Function to respawn the player to his starting position
    {
        vcam.enabled = false;//Disable the CineMachine
        transform.position = spawnPoint;//Reset the player's position
        camObj.GetComponent<Transform>().position = spawnPoint;//Move the camera's position
        vcam.PreviousStateIsValid = false;//Let's the CineMachine know its previous position should be ignored when calculating
        vcam.enabled = true;//Enable the CineMachine
    }
}
