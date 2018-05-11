using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent (typeof (AudioSource))]
using UnityEngine.UI;

public class Video : MonoBehaviour
{
	public UnityEngine.Video.VideoClip[] movie;

    public UnityEngine.Video.VideoClip goodEnding;
    public UnityEngine.Video.VideoClip badEnding;

	public int curVideo = 0;

	public Text text;

	public bool started = false;
	public bool canSkip = true;
	private string backCam = "lastCam";


    private UnityEngine.Video.VideoPlayer vc;

	// Use this for initialization
	void Start ()
	{
        vc = GetComponent<UnityEngine.Video.VideoPlayer>();
		//GetComponent<Renderer> ().material.mainTexture = movie [0];
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (canSkip)
			skip ();
		else
			text.text = "";

        //print(vc.time);
        if (started && ((int)vc.time >= (int)vc.clip.length)) {
           // SoundManager.instance.muteAll(false);

           // print("3");
			vc.Stop ();
            GameManager.instance.useCamera (backCam);

			started = false;
		}
		
	}

	public void setNextVideo ()
	{
		curVideo++;
        //GetComponent<Renderer> ().material.mainTexture = movie [curVideo];
        vc.clip = movie[curVideo];
	}

    public void setGoodEnding(bool val)
    {
        if (val) vc.clip = goodEnding;
        else vc.clip = badEnding;
    }

	public void playVideo (string backCam)
	{
        //SoundManager.instance.muteAll(true);
       // print("4");
       // print("video length: " + vc.clip.length);
        GameManager.instance.useCamera ("movie");
		vc.Play ();
		started = true;
		this.backCam = backCam;
	}

	private void skip ()
    {
		if (started) {
			if (GameManager.instance.usingController) {
                text.text = "Press <color=#00F448FF>A</color> to skip";
                if (Input.GetKeyDown ("joystick button 16") || Input.GetKeyDown("joystick button 0")) {
					vc.Stop ();
                    GameManager.instance.useCamera (backCam);
					started = false;
				}
			} else {
                text.text = "Press <color=#00F448FF>space</color> to skip";
				if (Input.GetKeyDown ("space")) {
					vc.Stop ();
                   // print("5");
                    GameManager.instance.useCamera (backCam);
					started = false;
				}
			}

		}
	}

	public void reset ()
	{
		curVideo = 0;
        //GetComponent<Renderer> ().material.mainTexture = movie [curVideo];

        if(vc == null) vc = GetComponent<UnityEngine.Video.VideoPlayer>();
        vc.clip = movie[curVideo];
	}
}
