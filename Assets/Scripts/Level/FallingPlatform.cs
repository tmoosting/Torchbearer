using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public AudioSource platformSource;
    public Animator platformAnim;
    public Rigidbody2D platformRB;
    public SpriteRenderer platformSprite;
    public BoxCollider2D platformGround;
    public GameObject triggers;
    private int stage = 0;//0 = originalpos, 1 = shake, 2 = falling, 3 = respawning
    private float timer = 0;
    Vector2 originalPos;
    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (stage == 2)
        {
            timer += Time.deltaTime;
            Vector2 vel = platformRB.velocity;
            vel.y = -5f;
            platformRB.velocity = vel;
            if (timer >= 1f)
            {
                Color platcolor = platformSprite.color;
                float alpha = 1f - (timer - 1f);
                if (alpha < 0f)
                {
                    alpha = 0f;
                }
                platcolor.a = alpha;
                platformSprite.color = platcolor;
            }
            if (timer >= 2f)
            {
                timer = 0f;
                platformRB.velocity = Vector2.zero;
                triggers.layer = 0;
                transform.position = originalPos;
                platformGround.enabled = false;
                stage++;
            }
        }
        if (stage == 3)
        {
            timer += Time.deltaTime;
            if (timer >= 5.0f)
            {
                Color platcolor = platformSprite.color;
                float alpha = 2*(timer - 5f);
                if (alpha > 1f)
                {
                    alpha = 1f;
                    platformSource.Stop();
                    platformGround.enabled = true;
                    triggers.layer = 9;
                    stage = 0;
                    timer = 0f;
                }
                platcolor.a = alpha;
                platformSprite.color = platcolor;
            }
        }

    }

    private void OnTriggerStay2D(Collider2D other) //Checks for collisions with other colliders
    {
        if (other.gameObject.CompareTag("Player"))//is the colliding object the player?
        {
            if (stage == 0)
            {
                platformAnim.SetTrigger("Shake");
                stage++;
                platformSource.Play();
            }
        }
    }

    public void StartFall()
    {
        if (stage == 1)
        {
            platformAnim.SetTrigger("StopShake");
            stage++;
        }
    }
}
