using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour {

    public Sprite sound;
    public Sprite noSound;
    public GameObject volume;

    void Update()
    {
        if (GameManager.instance.musicOn)
        {
            GetComponent<Image>().sprite = sound;
            volume.SetActive(true);
        }
        else
        {
            GetComponent<Image>().sprite = noSound;
            volume.SetActive(false);
        }
    }

    public void toggleMusic()
    {
        GameManager.instance.toggleMusic();

       
        if (GameManager.instance.musicOn)
        {
            GetComponent<Image>().sprite = sound;
            volume.SetActive(true);
            SoundManager.instance.muteAll(false);
        }
        else
        {
            GetComponent<Image>().sprite = noSound;
            volume.SetActive(false);
            SoundManager.instance.muteAll(true);
        }
    }
}
