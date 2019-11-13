using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask groundLayer;

    Rigidbody2D PlayerRB;
    BoxCollider2D PlayerBox;

    private float maxspeed;
    private float jumpvelocity;
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private float dashHor;
    private float dashVert;
    private bool dashing;
    private bool dashAvailable;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        PlayerBox = GetComponent<BoxCollider2D>();
        maxspeed = 6f;
        jumpvelocity = 10f;
        dashing = false;
        dashAvailable = false;
        dashSpeed = 20f;
        dashHor = 0f;
        dashVert = 0f;
        dashTime = startDashTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dashing)
        {
            if (!dashAvailable)
            {
                if (IsGrounded())
                {
                    dashAvailable = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (dashAvailable)
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
        if (!dashing)
        {
            dashAvailable = false;
            dashHor = Input.GetAxisRaw("Horizontal");
            dashVert = Input.GetAxisRaw("Vertical");
            dashing = true;
        }
        if (dashHor == -1 && dashVert != 1)
        {
            PlayerRB.velocity = new Vector2(-1f * dashSpeed, 0f);
        }
        else if (dashHor == -1 && dashVert == 1)
        {
            PlayerRB.velocity = new Vector2(-1f * dashSpeed, 1f * dashSpeed);
        }
        else if (dashHor == 0 && dashVert == 1)
        {
            PlayerRB.velocity = new Vector2(0f, 1f * dashSpeed);
        }
        else if (dashHor == 1 && dashVert == 1)
        {
            PlayerRB.velocity = new Vector2(1f * dashSpeed, 1f * dashSpeed);
        }
        else if (dashHor == 1 && dashVert != 1)
        {
            PlayerRB.velocity = new Vector2(1f * dashSpeed, 0f);
        }
        if (dashTime <= 0)
        {
            dashHor = 0f;
            dashVert = 0f;
            dashTime = startDashTime;
            PlayerRB.velocity = new Vector2(0f, 0f);
            dashing = false;
        }
        else
        {
            dashTime -= Time.deltaTime;
        }
    }

    void Jump()
    {
        if (IsGrounded())
        {
            Vector2 vel = PlayerRB.velocity;
            vel.y = jumpvelocity;
            PlayerRB.velocity = vel;
        }
    }

    bool IsGrounded()
    {
        Vector2 position = transform.position;
        float x = PlayerBox.size.x - 0.2f;
        Vector2 direction = Vector2.down;
        float distance = 0.6f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            return true;
        }
        position.x -= x/2;
        RaycastHit2D hit2 = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit2.collider != null)
        {
            return true;
        }
        position.x += x;
        RaycastHit2D hit3 = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit3.collider != null)
        {
            return true;
        }
        return false;
    }
}
