using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance = null;

    [Header("Sources")]
    public AudioSource robot;
    public AudioSource bird;
    public AudioSource critter;
    public AudioSource effects;
    public AudioSource music;

    [Header("Sound clips")]
    public AudioClip roboWalkHouse;
    public AudioClip roboRunHouse;

    public AudioClip roboWalkDirt;
    public AudioClip roboRunDirt;

    public AudioClip doorOpenClose;

    public AudioClip scratchAtDoor;

    public AudioClip pickupPitchfork;
    public AudioClip throwPitchfork;

    public AudioClip wrong;

    public AudioClip birdFlap;

    public AudioClip rockingChair;

     

	
	void Awake () {
        if(instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
	}

    public void playRobot(AudioClip clip, bool loop)
    {
        robot.clip = clip;
        robot.loop = loop;
        robot.Play();
    }

    public void playBird(AudioClip clip, bool loop)
    {
        bird.clip = clip;
        bird.loop = loop;
        bird.Play();
    }

    public void playSingle(AudioClip clip, bool loop){
        effects.clip = clip;
        effects.loop = loop;
        effects.Play();
    }

    public void playBackgroundMusic(AudioClip clip){
        music.clip = clip;
        music.loop = true;
        music.Play();
    }

    public void setVolume(float value){
        effects.volume = value;
        music.volume = value;
        robot.volume = value;
    }

    public void muteAll(bool val)
    {
        effects.mute = val;
        music.mute = val;
        robot.mute = val;
    }

    public void stopRobotSound(){
        robot.Stop();
    }

    public void stopEffectSound()
    {
        effects.Stop();
    }

    public void stopMusic()
    {
        music.Stop();
    }
	
	
}
