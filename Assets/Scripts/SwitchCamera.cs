using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour {

    [Header("Cameras")]
    public string currentCam;
    public Camera currentHouseCam;

    public Camera initialCam;
    public Camera canvasCam;
    public Camera playerCam;
    public Camera movieCam;
    public Camera miniGame1;
    public Camera miniGame2;
    public Camera miniGame3;
    public Camera picViewingCam;

    [Header("House Cameras")]
    public GameObject[] houseCams;

	// Use this for initialization
	void Start () {
      
        currentHouseCam = initialCam;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(!GameManager.instance.game1 && !GameManager.instance.game2 && !GameManager.instance.game3 )
        {
            if(other.tag == "houseCam"){
                currentHouseCam = other.gameObject.GetComponent<Camera>();
                useCamera("lastCam");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "House")
        {
            useCamera("outside");
        }
    }

    public void useCamera(string cam)
    {
        disableCams();

        switch (cam)
        {
            case "canvas":
                canvasCam.enabled = true;
                break;
            case "outside":
                playerCam.enabled = true;
                break;
            case "movie":
                movieCam.enabled = true;
                break;
            case "miniGame1":
                miniGame1.enabled = true;
                break;
            case "miniGame2":
                miniGame2.enabled = true;
                break;
            case "miniGame3":
                miniGame3.enabled = true;
                break;
            case "pics":
                picViewingCam.enabled = true;
                GameManager.instance.movePlayer(false);
                GameManager.instance.picLights.SetActive(true);
                break;
            case "lastCam":
                GameManager.instance.movePlayer(true);
                currentHouseCam.enabled = true;
                GameManager.instance.gameItems.worldCamera = currentHouseCam;
                SoundManager.instance.playRockingChair();
                break;
        }

        currentCam = cam;
    }

    private void disableCams()
    {
        canvasCam.enabled = false;
        playerCam.enabled = false;
        movieCam.enabled = false;
        miniGame1.enabled = false;
        miniGame2.enabled = false;
        miniGame3.enabled = false;
        picViewingCam.enabled = false;

        foreach (GameObject ob in houseCams)
        {
            ob.GetComponent<Camera>().enabled = false;
        }
    }
}
