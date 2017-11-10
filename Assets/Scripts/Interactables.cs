﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Interactables : MonoBehaviour
{
	public Text itemTxt;

	private string buttonTxt;
	private string interact;

	private bool firstPitchfork = true;
	private bool firstGame1 = true;
	private bool firstDoor = true;
	private bool secondDoor = true;
	private bool firstGame2 = true;
	private bool firstGame3 = true;

	void Update ()
	{
		
		checkController ();
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Interactable") {
			itemTxt.text = other.gameObject.name.ToString ().ToUpper () + ". . . ";

			if (other.gameObject.name.ToString () == "Pitchfork") {
				itemTxt.text += buttonTxt + " to pick up.";
			}

			if (other.gameObject.name.ToString () == "scrapheap") {
				
			}
		}

		if (other.tag == "Door") {
			itemTxt.text = "Open door? " + buttonTxt;
			other.GetComponent<Door> ().Move ();
		}

		if (firstGame1 && other.tag == "miniGame1" && GameManager.instance.currItem.gameObject.name.ToString () == "Door1") {
			GameManager.instance.setNextVideo ();
			//StartCoroutine (GameManager.instance.playVideo ("miniGame1"));
			GameManager.instance.playVideo ("miniGame1");
			StartCoroutine (GameManager.instance.startMiniGame1 ());
			firstGame1 = false;
		}

		if (firstDoor && other.tag == "cutscene" && GameManager.instance.currItem.gameObject.name.ToString () == "Door2") {
			
			GameManager.instance.setNextVideo ();
			//StartCoroutine (GameManager.instance.playVideo ("player"));
			GameManager.instance.playVideo ("player");
			StartCoroutine (GameManager.instance.waitForVideo (true));

			firstDoor = false;
		}

		if (GameManager.instance.isNextDay && !firstDoor && secondDoor && other.tag == "miniGame2"
		    && GameManager.instance.currItem.gameObject.name.ToString () == "Door2") {

			GameManager.instance.setNextVideo ();
			GameManager.instance.playVideo ("miniGame2");
			StartCoroutine (GameManager.instance.startMiniGame2 ());

			secondDoor = false;
		}

		if (firstGame3 && !firstGame1 && !firstDoor && !secondDoor && other.tag == "miniGame1"
		    && GameManager.instance.currItem.gameObject.name.ToString () == "Door1") {
			GameManager.instance.setNextVideo ();
			//StartCoroutine (GameManager.instance.playVideo ("miniGame1"));
			GameManager.instance.playVideo ("miniGame3");
			StartCoroutine (GameManager.instance.startMiniGame3 ());
			firstGame3 = false;
		}

	}

	void OnTriggerStay (Collider other)
	{
		if (other.tag == "Door") {

			if (Input.GetKeyDown (interact)) {

				StartCoroutine (openAndCloseDoor (other.gameObject));
				
			}
		}

		if (other.gameObject.name.ToString () == "Pitchfork") {
			if (Input.GetKeyDown (interact)) {
				other.GetComponent<Pitchfork> ().pickup ();

				if (firstPitchfork) {
					GameManager.instance.resetMats ();
					GameManager.instance.nextItem ();
					firstPitchfork = false;
				}
			}
		}


	}

	private IEnumerator openAndCloseDoor (GameObject door)
	{
		if (door.GetComponent<Door> ().RotationPending == false) {
			StartCoroutine (door.GetComponent<Door> ().Move ());
			yield return new WaitForSeconds (5f);
			StartCoroutine (door.GetComponent<Door> ().Move ());
		}
	}

	void OnTriggerExit (Collider other)
	{
		itemTxt.text = "";
	}


	public void reset ()
	{
		firstPitchfork = true;
		firstGame1 = true;
		firstDoor = true;
		secondDoor = true;
		firstGame2 = true;
		firstGame3 = true;
	}

	private void checkController ()
	{
		if (GameManager.instance.usingController) {
			interact = "joystick button 16";
			buttonTxt = "Press A";
		} else {
			interact = "e";
			buttonTxt = "Press E";
		}

	}

}
