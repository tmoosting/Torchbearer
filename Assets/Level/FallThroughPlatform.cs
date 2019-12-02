//This code makes it possible to fall through certain platforms by holding down the down arrow key. 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallThroughPlatform : MonoBehaviour
{

    private PlatformEffector2D effector;
    public float waitTime;//Time needed to hold down the arrow key to fall through a platform

    // Start is called before the first frame update
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
        waitTime = 0.5f; 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.DownArrow)) //If the down arrow key is released
        {
            waitTime = 0.5f;//reset the waiting time
        }

        if (Input.GetKey(KeyCode.DownArrow))//While the down arrow is being held down
        {
            if (waitTime <= 0)//If the arrow key has been held down for waitTime amount of seconds
            {
                effector.rotationalOffset = 180f;//Turn around the 1-way platform collider to fall through it
                waitTime = 0.5f;// Reset the waiting time
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }

        if (Input.GetKey(KeyCode.UpArrow))//If the player jumps
        {
            effector.rotationalOffset = 0;//Reset the 1-way platform collider so you can jump through the bottom again
        }
    }
}
