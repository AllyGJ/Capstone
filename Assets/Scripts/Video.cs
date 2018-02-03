using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent (typeof (AudioSource))]
using UnityEngine.UI;

public class Video : MonoBehaviour
{
	public MovieTexture[] movie;
	public int curVideo = 0;

	public Text text;

	public bool started = false;
	public bool canSkip = true;
	private string backCam = "player";

	// Use this for initialization
	void Start ()
	{
		GetComponent<Renderer> ().material.mainTexture = movie [0];
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (canSkip)
			skip ();
		else
			text.text = "";

		if (started && !movie [curVideo].isPlaying) {
            SoundManager.instance.muteAll(false);

            print("3");
			movie [curVideo].Stop ();
			GameManager.instance.useCamera (backCam);

			started = false;
		}
		
	}

	public void setNextVideo ()
	{
		curVideo++;
		GetComponent<Renderer> ().material.mainTexture = movie [curVideo];
	}

	public void playVideo (string backCam)
	{
        //SoundManager.instance.muteAll(true);
       // print("4");
		GameManager.instance.useCamera ("movie");
		movie [curVideo].Play ();
		started = true;
		this.backCam = backCam;
	}

	private void skip ()
    {
		if (started) {
			if (GameManager.instance.usingController) {
				text.text = "Press 'A' to skip";
                if (Input.GetKeyDown ("joystick button 16") || Input.GetKeyDown("joystick button 0")) {
					movie [curVideo].Stop ();
					GameManager.instance.useCamera (backCam);
					started = false;
				}
			} else {
				text.text = "Press 'space' to skip";
				if (Input.GetKeyDown ("space")) {
					movie [curVideo].Stop ();
                    print("5");
					GameManager.instance.useCamera (backCam);
					started = false;
				}
			}

		}
	}

	public void reset ()
	{
		curVideo = 0;
		GetComponent<Renderer> ().material.mainTexture = movie [curVideo];
	}
}
