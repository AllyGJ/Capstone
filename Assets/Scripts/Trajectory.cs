using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Security.AccessControl;

public class Trajectory : MonoBehaviour
{
	public GameObject trajectoryBar;
	public Slider traj;
	public Text trajText;
	public bool moveSlider = false;

	private string stopButton;
	private float throwValue;

	//Chances to hit bird
	private int numThrows = 5;

	private int curThrows = 0;

	private int score;

	void Start ()
	{
		checkController ();

		traj.value = 0;
		score = 0;

		showTrajBar (false);
	}


	void Update ()
	{
		checkController ();

		if (moveSlider) {
			showTrajBar (true);
			traj.value = Mathf.PingPong (Time.time, 1);

			if (Input.GetKeyDown (stopButton)) {
				curThrows++;
                print("pressed space");
				moveSlider = false;
				//throwValue = traj.value;

				int val = getCorrectScore (traj.value);
                print("val = " + val);
				score += val;

				//animation of robot throwing pitchfork at bird
				StartCoroutine (wait (val));

			}

		}
        else showTrajBar(false);
		
	}

	IEnumerator wait (int val)
	{
		if (curThrows == numThrows) {
            GameManager.instance.pitchfork.GetComponent<Pitchfork>().throwAtBird(val);
			yield return new WaitForSeconds (3f);
			showTrajBar (false);
			GameManager.instance.addToScore (score);
			GameManager.instance.endMiniGame (true);
		} else {
            GameManager.instance.pitchfork.GetComponent<Pitchfork>().throwAtBird(val);
			yield return new WaitForSeconds (3f);
			moveSlider = true;
            GameManager.instance.pitchfork.GetComponent<Pitchfork>().resetPos();
			//once animation over, start slider again
		}
	}

	void checkController ()
	{
		if (GameManager.instance.usingController) {
			stopButton = "joystick button 16";
			trajText.text = "HIT 'A' TO STOP";
		} else {
			stopButton = "space";
			trajText.text = "HIT 'SPACEBAR' TO STOP";
		}
	}

	public void showTrajBar (bool val)
	{
		trajectoryBar.SetActive (val);
	}

	private int getCorrectScore (float val)
	{
		//val = 0, return 0
		//val = 1, return 0
		//val > 0 and val < 1, return val*10

		if (val == 0f || val == 1f) {
			return 0;
		} else if (val > 0.5f) {
			return (int)((1f - val) * 10);
		} else
			return (int)(val * 10);
		
	}


	public void reset ()
	{
		traj.value = 0;
		showTrajBar (false);
		moveSlider = false;
		curThrows = 0;
		score = 0;
	}

}
