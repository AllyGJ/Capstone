using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance = null;

    [Header("Sources")]
    public AudioSource effects;
    public AudioSource music;

    [Header("Sound clips")]
    public AudioClip roboWalkHouse;
    public AudioClip roboRunHouse;

    public AudioClip roboWalkDirt;
    public AudioClip roboRunDirt;

    public AudioClip doorOpenClose;

    public AudioClip scratchAtDoor;

     

	
	void Start () {
        if(instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        
	}

    public void playSingle(AudioClip clip){
        effects.clip = clip;
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
    }

    public void muteAll(bool val)
    {
        effects.mute = val;
        music.mute = val;
    }
	
	
}
