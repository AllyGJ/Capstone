using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance = null;

    [Header("Sources")]
    public AudioSource robot;
    public AudioSource birdPeck;
    public AudioSource birdFlap;
    //public AudioSource critter;
    public AudioSource critterAtDoor;
    public AudioSource pickupFork;
    public AudioSource backgroundMusic;
    public AudioSource miniGameMusic;
    public AudioSource wrong;
    public AudioSource rockingChair;
    public AudioSource pitchforkWhip;

    //[Header("Sound clips")]
    //public AudioClip roboWalkHouse;
    //public AudioClip roboRunHouse;

    //public AudioClip roboWalkDirt;
    //public AudioClip roboRunDirt;

    //public AudioClip doorOpenClose;

    //public AudioClip scratchAtDoor;

    //public AudioClip pickupPitchfork;
    //public AudioClip throwPitchfork;

    //public AudioClip wrong;

    //public AudioClip birdFlap;

    //public AudioClip rockingChair;

     

	
	void Awake () {
        if(instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
	}

    public void playRobot()
    {
        print("paying robot sound");
        robot.Play();
    }

    public void playPickup()
    {
        pickupFork.Play();
    }

    public void playBirdFlap()
    {
        birdFlap.Play();
    }
    public void playBirdPeck()
    {
        birdPeck.Play();
    }
    public void playWrong()
    {
        wrong.Play();
    }
    public void playWhip()
    {
        pitchforkWhip.Play();
    }

    public void playCritterAtDoor()
    {
        critterAtDoor.Play();
        backgroundMusic.volume = 0.3f;
        critterAtDoor.volume = 1f;

    }
    public void stopCritterAtDoor()
    {
        critterAtDoor.Stop();
    }
    public void playRockingChair()
    {
        rockingChair.Play();
    }
    public void stopRockingChair()
    {
        rockingChair.Stop();
    }
    public void stopBirdFlap()
    {
        birdFlap.Stop();
    }
    public void stopBirdPeck()
    {
        birdPeck.Stop();
    }

    public void setVolume(float value)
    {
        backgroundMusic.volume = Mathf.Clamp(value, 0, 0.6f);
        miniGameMusic.volume = Mathf.Clamp(value, 0, 0.9f);

        robot.volume = value;
        birdFlap.volume = value;
        birdPeck.volume = value;
        pickupFork.volume = value;
        critterAtDoor.volume = value;
        rockingChair.volume = value;
        wrong.volume = value;
        pitchforkWhip.volume = value;
       
    }

    public void muteAll(bool val)
    {
        birdPeck.mute = val;
        birdFlap.mute = val;
        backgroundMusic.mute = val;
        robot.mute = val;
        critterAtDoor.mute = val;
        rockingChair.mute = val;
        wrong.mute = val;
        pitchforkWhip.mute = val;
        miniGameMusic.mute = val;
    }

    public void switchTo(string musicString)
    {
        if(musicString == "norm")
        {
            miniGameMusic.Stop();
            backgroundMusic.Play();
        }
        else if(musicString == "game"){
            backgroundMusic.Stop();
            miniGameMusic.Play();
        }
    }

    public void stopRobot(){
        robot.Stop();
    }

    public void stopMusic()
    {
        backgroundMusic.Stop();
    }
	
	
}
