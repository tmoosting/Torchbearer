using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BeaconTouch : MonoBehaviour
{
    public GameObject Player;
    public AudioSource beaconSource;
    public AudioClip BeaconStartUp;
    public AudioClip BeamStartUp;
    public AudioClip BeamLoop;
    public AudioMixer mixer;
    private bool beamstarted = false;
    private float BeamVolume = -10f;
    private float BeaconVolume = -3f;
    private void Update()
    {
        if (!beaconSource.isPlaying && beamstarted)
        {
            beaconSource.clip = BeamLoop;
            beaconSource.Play();
        }
    }
    private void OnTriggerEnter2D(Collider2D other) //Checks for collisions with other colliders
    {
        if (other.gameObject.CompareTag("Player"))//is the colliding object the player?
        {
            Player.GetComponent<PlayerMovement>().endingAnimation += 1;
            Player.GetComponent<PlayerMovement>().PlayerAnimator.SetTrigger("End");
        }
    }

    public void WalkOffScreen()
    {
        if (Player.GetComponent<PlayerMovement>().endingAnimation == 3)
        {
            Player.GetComponent<PlayerMovement>().endingAnimation += 1;
        }
    }

    public void StartBeam()
    {
        beaconSource.Stop();
        mixer.SetFloat("BeaconVolume", BeamVolume);
        beaconSource.clip = BeamStartUp;
        beaconSource.Play();
        beamstarted = true;
    }

    public void StartBeacon ()
    {
        beaconSource.Stop();
        mixer.SetFloat("BeaconVolume", BeaconVolume);
        beaconSource.clip = BeaconStartUp;
        beaconSource.Play();
    }
}
