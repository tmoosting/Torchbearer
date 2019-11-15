using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask groundLayer;
    public Ghost ghost;
    public GameObject cineMachine;
    public GameObject camObj;

    Rigidbody2D PlayerRB;
    BoxCollider2D PlayerBox;
    SpriteRenderer PlayerSprite;
    Animator PlayerAnimator;
    Transform PlayerTrans;
    
    CinemachineFramingTransposer CineTransposer;
    CinemachineVirtualCamera vcam;

    private float maxspeed = 6f;
    private float jumpvelocity = 10f;
    public float dashSpeed = 20f;
    public float startDashTime = 0.2f;
    private float dashHor = 0f;
    private float dashVert = 0f;
    private bool dashing = false;
    public bool dashAvailable = false;
    private bool isFalling = false;
    public bool freeDash = false;
    private float lowestY = -30f;

    private float dashTime;
    private Vector3 spawnPoint;
    private Vector3 cineSpawnPoint;


    // Start is called before the first frame update
    void Start()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        PlayerBox = GetComponent<BoxCollider2D>();
        PlayerSprite = GetComponent<SpriteRenderer>();
        PlayerAnimator = GetComponent<Animator>();
        PlayerTrans = GetComponent<Transform>();
        dashTime = startDashTime;
        spawnPoint = PlayerTrans.position;
        vcam = cineMachine.GetComponent<CinemachineVirtualCamera>();
        CineTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerTrans.position.y < lowestY)
        {
            vcam.enabled = false;
            PlayerTrans.position = spawnPoint;
            camObj.GetComponent<Transform>().position = spawnPoint;
            vcam.PreviousStateIsValid = false;
            vcam.enabled = true;
        }
        if (!dashing)
        {
            if (!dashAvailable || isFalling)
            {
                if (IsGrounded())
                {
                    dashAvailable = true;
                    isFalling = false;
                    PlayerAnimator.SetBool("Falling", false);
                }
            }
            if (PlayerRB.velocity.y < -0.01)
            {
                isFalling = true;
                PlayerAnimator.SetBool("Jump", false);
                PlayerAnimator.SetBool("Falling", true);
            }
            else if (PlayerRB.velocity.y > 0.01)
            {
                isFalling = false;
                PlayerAnimator.SetBool("Jump", true);
                PlayerAnimator.SetBool("Falling", false);
            } 
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (dashAvailable  || freeDash)
                {
                    Dash();
                }
            }
            else
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");
                Vector2 vel = PlayerRB.velocity;
                vel.x = horizontal * maxspeed;
                PlayerAnimator.SetFloat("Speed", Mathf.Abs(horizontal * maxspeed));
                if (horizontal < 0)
                {
                    PlayerSprite.flipX = true;
                }
                else if (horizontal > 0)
                {
                    PlayerSprite.flipX = false;
                }
                PlayerRB.velocity = vel;
                if (vertical == 1)
                {
                    Jump();
                }
            }
        }
        else
        {
            Dash();
        }
        


    }

    void Dash()
    {
        if (!(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0))
        {
            if (!dashing)
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
            float pythDash = Mathf.Sqrt(dashSpeed * dashSpeed + dashSpeed * dashSpeed);
            if (dashHor == -1 && dashVert == 0)
            {
                PlayerRB.velocity = new Vector2(-1f * pythDash, 0f);
            }
            else if (dashHor == -1 && dashVert == 1)
            {
                PlayerRB.velocity = new Vector2(-1f * dashSpeed, 1f * dashSpeed);
            }
            else if (dashHor == 0 && dashVert == 1)
            {
                PlayerRB.velocity = new Vector2(0f, 1f * pythDash);
            }
            else if (dashHor == 1 && dashVert == 1)
            {
                PlayerRB.velocity = new Vector2(1f * dashSpeed, 1f * dashSpeed);
            }
            else if (dashHor == 1 && dashVert == 0)
            {
                PlayerRB.velocity = new Vector2(1f * pythDash, 0f);
            }
            else if (dashHor == 1 && dashVert == -1)
            {
                PlayerRB.velocity = new Vector2(1f * dashSpeed, -1f * dashSpeed);
            }
            else if (dashHor == 0 && dashVert == -1)
            {
                PlayerRB.velocity = new Vector2(0f, -1f * pythDash);
            }
            else if (dashHor == -1 && dashVert == -1)
            {
                PlayerRB.velocity = new Vector2(-1f * dashSpeed, -1f * dashSpeed);
            }
            if (dashTime <= 0)
            {
                dashHor = 0f;
                dashVert = 0f;
                dashTime = startDashTime;
                PlayerRB.velocity = new Vector2(0f, 0f);
                dashing = false;
                PlayerAnimator.SetBool("Dashing", false);
                ghost.makeGhost = false;
            }
            else
            {
                dashTime -= Time.deltaTime;
            }
        }
    }

    void Jump()
    {
        if (IsGrounded())
        {
            dashAvailable = true;
            PlayerAnimator.SetBool("Jump", true);
            Vector2 vel = PlayerRB.velocity;
            vel.y = jumpvelocity;
            PlayerRB.velocity = vel;
        }
    }

    bool IsGrounded()
    {
        Vector2 position = transform.position;
        float x = PlayerBox.bounds.size.x/2 - 0.2f;
        Vector2 direction = Vector2.down;
        float distance = 1.5f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            return true;
        }
        position.x -= x;
        RaycastHit2D hit2 = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit2.collider != null)
        {
            return true;
        }
        position.x += x*2;
        RaycastHit2D hit3 = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit3.collider != null)
        {
            return true;
        }
        return false;
    }
}
