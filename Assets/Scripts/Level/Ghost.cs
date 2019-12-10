//This script creates an afterimage that is used when the player dashes. Should be used on the player gameobject
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{

    public float ghostDelay;//The delay it takes
    private float ghostDelaySeconds;//used to count down the delay
    public GameObject ghost; //Ghost prefab
    public bool makeGhost = false;//are we allowed to make a ghost?

    // Start is called before the first frame update
    void Start()
    {
        ghostDelaySeconds = ghostDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (makeGhost)//Are we currently dashing
        {
            if (ghostDelaySeconds > 0)//We can't make a ghost yet
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            else //generate ghost
            {
                GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation); //Create a ghost at our current player position
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite; //Get the sprite our player currently has
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;//Give ghost the sprite
                bool curFlip = GetComponent<SpriteRenderer>().flipX;//gets player flip
                currentGhost.GetComponent<SpriteRenderer>().flipX = curFlip;//sets ghost flip
                ghostDelaySeconds = ghostDelay;//Reset the delay
                Destroy(currentGhost, 1f);//destroy ghost after 1 second
            }
        }
    }
}
