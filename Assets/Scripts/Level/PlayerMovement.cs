using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask groundLayer;

    Rigidbody2D PlayerRB;
    BoxCollider2D PlayerBox;

    float maxspeed;
    float jumpvelocity;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        PlayerBox = GetComponent<BoxCollider2D>();
        maxspeed = 6f;
        jumpvelocity = 10f;
    }

    // Update is called once per frame
    void Update()
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
