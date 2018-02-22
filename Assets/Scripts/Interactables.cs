using System.Collections;
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

    //private bool firstPitchfork = false;
    //private bool firstGame1 = false;
    //private bool firstDoor = false;
    //private bool secondDoor = false;
    //private bool firstGame2 = false;
    //private bool firstGame3 = true;


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

        if (other.tag == "photos")
        {
            itemTxt.text = "Family photos. " + buttonTxt + " to view photos.";
        }

        if(other.tag == "poster")
        {
            itemTxt.text = "AIRRaT Poster. Master used to work there.";
        }

        if(other.tag == "bed")
        {
            itemTxt.text = "Master sleeps here.";
        }

        if (other.tag == "dinnerTable")
        {
            itemTxt.text = "Master and family eat here";
        }

        //MINIGAME1
		if (firstGame1 && other.tag == "miniGame1" && GameManager.instance.currItem.gameObject.name.ToString () == "Door1") {
			GameManager.instance.setNextVideo ();
			GameManager.instance.playVideo ("miniGame1");
			StartCoroutine (GameManager.instance.startMiniGame1 ());
            SoundManager.instance.switchTo("game");
			firstGame1 = false;
		}

        //CUTSCENE TO NEXT DAY
		if (firstDoor && other.tag == "cutscene" && GameManager.instance.currItem.gameObject.name.ToString () == "Door2") {
			
			GameManager.instance.setNextVideo ();
			GameManager.instance.playVideo ("player");
			StartCoroutine (GameManager.instance.waitForVideo (true));

			firstDoor = false;
		}

        //MINIGAME2
		if (GameManager.instance.isNextDay && !firstDoor && secondDoor && other.tag == "miniGame2"
		    && GameManager.instance.currItem.gameObject.name.ToString () == "Door2") {
            SoundManager.instance.stopCritterAtDoor();
            SoundManager.instance.switchTo("game");
			GameManager.instance.setNextVideo ();
			GameManager.instance.playVideo ("miniGame2");
			StartCoroutine (GameManager.instance.startMiniGame2 ());

			secondDoor = false;
		}

        //MINIGAME3
		if (firstGame3 && !firstGame1 && !firstDoor && !secondDoor && other.tag == "miniGame1"
		    && GameManager.instance.currItem.gameObject.name.ToString () == "Door1") {
			GameManager.instance.setNextVideo ();
			GameManager.instance.playVideo ("miniGame3");
			StartCoroutine (GameManager.instance.startMiniGame3 ());
            SoundManager.instance.switchTo("game");
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
                
                StartCoroutine(other.GetComponent<Pitchfork>().Pickup());

				if (firstPitchfork) {
					GameManager.instance.resetMats ();
					GameManager.instance.nextItem ();
					firstPitchfork = false;
				}
			}
		}

        if(other.tag == "photos"){
            if(Input.GetKeyDown(interact)){
                GameManager.instance.useCamera("pics");
            }
        }


	}

	private IEnumerator openAndCloseDoor (GameObject door)
	{
		if (door.GetComponent<Door> ().RotationPending == false) {
            door.GetComponent<AudioSource>().Play();
			StartCoroutine (door.GetComponent<Door> ().Move ());
			yield return new WaitForSeconds (3f);
			StartCoroutine (door.GetComponent<Door> ().Move ());
		}
	}

	void OnTriggerExit (Collider other)
	{
		itemTxt.text = "";
	}

    public void showText(bool val)
    {
        itemTxt.enabled = val;
    }


	public void reset ()
	{
		firstPitchfork = true;
		firstGame1 = true;
		firstDoor = true;
		secondDoor = true;
		firstGame2 = true;
		firstGame3 = true;

        showText(true);
	}

	private void checkController ()
	{
		if (GameManager.instance.usingController) {
            if (GameManager.instance.macBuild)
            {
                interact = "joystick button 16";
            }else{
                interact = "joystick button 0";
            }
            buttonTxt = "Press <color=#00F448FF>A</color>";
		} else {
			interact = "e";
            buttonTxt = "Press <color=#00F448FF>E</color>";
		}

	}

}
