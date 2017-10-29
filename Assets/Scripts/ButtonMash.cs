﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ButtonMash : MonoBehaviour
{
	public bool beginButtonMash;
	public Sprite[] xboxBtns;
	public Sprite[] keyBtns;

	public GameObject ButtonPressTimer;
	public Image LoadingBar;
	public Image CenterBtn;

	private Sprite[] btns;
	private string[] keys;

	public string[] xboxKeys;
	public string[] keyKeys;

	public bool userInputMatches = false;

	private int numButtons = 15;
	public int curButton = 0;
	public int correct = 0;



	void Start ()
	{
		btns = new Sprite[4];
		keys = new String[4];

		CenterBtn.GetComponent<Image> ();
		LoadingBar.GetComponent<Image> ();

		checkController ();

		randomlyPickBtn ();
		showButton (false);
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		checkController ();

		if (beginButtonMash) {
			//GameManager.instance.player.GetComponent <Player> ().enabled = false;
			showButton (true);

			if (curButton <= numButtons) {
				fillBar ();

				checkForUserInput ();

				if (userInputMatches && LoadingBar.fillAmount < 1) {
					matchedButton ();
				} 

			} else {
				//shown all buttons, check how many correct
				if (correct == numButtons) {
					print ("perfect score!");
				} else {
					float percent = ((float)correct / (float)numButtons) * 100;
					print ("percent correct = " + percent);
				}
				beginButtonMash = false;
			}
				

		} else {
			showButton (false);
			//GameManager.instance.player.GetComponent <Player> ().enabled = true;
			//GameManager.instance.endMiniGame ();
		}
		
	}

	void checkController ()
	{
		if (GameManager.instance.usingController) {
			UseArray (xboxBtns, xboxKeys);
		} else {
			UseArray (keyBtns, keyKeys);
		}
	}

	void UseArray (Sprite[] arr, string[] keys)
	{
		for (int i = 0; i < arr.Length; i++) {
			btns [i] = arr [i];
			this.keys [i] = keys [i];
		}
	}

	public void showButton (bool val)
	{
		ButtonPressTimer.SetActive (val);
	}

	void randomlyPickBtn ()
	{
		int rand = UnityEngine.Random.Range (0, 3);
		CenterBtn.sprite = btns [rand];
	}

	void checkForUserInput ()
	{
		Sprite curBtn = CenterBtn.sprite;
		string btn2Press = "";

		for (var i = 0; i < keys.Length; i++) {
			if (btns [i] == curBtn) {
				btn2Press = keys [i];
			}
		}
			
		if (Input.GetKeyDown (btn2Press)) {
			userInputMatches = true;
		} 
	}


	void fillBar ()
	{
		LoadingBar.fillAmount += Time.deltaTime * 0.3f;
	
		if (LoadingBar.fillAmount >= 0.75f)
			LoadingBar.color = Color.red;
		if (LoadingBar.fillAmount == 1f) {
			if (!userInputMatches)
				curButton++;
			LoadingBar.fillAmount = 0f;
			LoadingBar.color = Color.blue;
			randomlyPickBtn ();
		}

	}

	void matchedButton ()
	{
		correct++;
		curButton++;
		//show animation of robot hitting bird
		LoadingBar.fillAmount = 0;
		randomlyPickBtn ();
		userInputMatches = false;
	}

	public void reset ()
	{
		curButton = 0;
		correct = 0;
		LoadingBar.fillAmount = 0f;
	}
}