﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Security.AccessControl;

public class Trajectory : MonoBehaviour
{

	public Slider traj;
	public Text trajText;

	private string stopButton;
	private bool moveSlider = false;
	private float throwValue;

	void Start ()
	{
		if (GameManager.instance.usingController) {
			stopButton = "joystick button 16";
			trajText.text = "HIT 'A' TO STOP";
		} else {
			stopButton = "space";
			trajText.text = "HIT 'SPACEBAR' TO STOP";
		}

		traj.value = 0;

		showTrajBar (false);
		startTraj (false);
	}


	void Update ()
	{

		if (moveSlider) {
			traj.value = Mathf.PingPong (Time.time, 1);

			if (Input.GetKeyDown (stopButton)) {
				startTraj (false);
				throwValue = traj.value;
			}

		}
		
	}

	public void startTraj (bool val)
	{
		moveSlider = val;
	}

	public void showTrajBar (bool val)
	{
		traj.enabled = val;
	}


}
