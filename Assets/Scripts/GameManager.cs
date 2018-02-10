using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif
using Invector.CharacterController;
using System;

public class GameManager : MonoBehaviour
{

	public static GameManager instance = null;

    [Header("Settings")]
    public bool macBuild;
	public bool usingController;
	public bool musicOn;
	public float musicVolume;

	[Header ("Cameras")]
	public string currentCam;
	public Camera canvasCam;
	public Camera playerCam;
	public Camera movieCam;
	public Camera miniGame1;
	public Camera miniGame2;
	public Camera miniGame3;
    public Camera picViewingCam;

    [Header("Game Objects")]
    public GameObject player;
	public GameObject bird;
    public GameObject critter;
	public GameObject pitchfork;
	public GameObject arrow;

	public Transform pitchforkStart;
	public Transform startPoint;
    public Transform camResetPos;

	public GameObject settingsButton;
    public GameObject gearFrame;
	public Canvas gameItems;
	public GameObject videoCanvas;

    public GameObject upButton;
    public GameObject downButton;
    public GameObject picLights;

	public GameObject[] storyItems;
	public GameObject currItem;

	public Material[] regularMats;
	public Material[] glowMats;

	public int overallScore;

    [Header("Door sounds")]
    public AudioSource[] doorSounds;

    [Header("Booleans")]
    public bool isNextDay = false;

	public bool game1 = false;
	public bool game2 = false;
	public bool game3 = false;

	public bool startRunning = false;

	private int currItemIndex = 0;

	private ButtonMash buttonMash;
	private Trajectory trajectory;

	private Transform camPosRot;

    private Vector3 newPicCamPos;

	void Awake ()
	{
		if (instance == null)
			instance = this;
		//else if (instance != this)
			//Destroy (gameObject);
        

		//DontDestroyOnLoad (gameObject);

		buttonMash = GetComponent<ButtonMash> ();
		trajectory = GetComponent<Trajectory> ();

        newPicCamPos = picViewingCam.transform.position;

		camPosRot = playerCam.transform;

		musicOn = true;
		musicVolume = 0.5f;
		reset ();

	}

    void Update()
    {
        if(picViewingCam.enabled){
            if(Input.GetKeyDown("joystick button 5") || Input.GetKeyDown("up")){
                goUp(true);
            }
            else if(Input.GetKeyDown("joystick button 6") || Input.GetKeyDown("down")){
                goUp(false);
            }

            if(Input.GetKeyDown("escape") || Input.GetKeyDown("joystick button 17") || Input.GetKeyDown("joystick button 1")){
                print("2");
                useCamera("player");
            }
        }

        if (picViewingCam.transform.position != newPicCamPos)
        {
            picViewingCam.transform.position = Vector3.Lerp(picViewingCam.transform.position, newPicCamPos, Time.deltaTime * 2f);
        }

    }

    void stopDoorSounds()
    {
        foreach (AudioSource source in doorSounds)
        {
            source.Stop();
        }
        SoundManager.instance.stopRockingChair();
    }


	public void movePlayer (bool val)
	{

        if (val)
        {
            player.GetComponent<Player>().removeConstraints();
            player.GetComponent<vThirdPersonInput>().moving = true;
        }
        else player.GetComponent<vThirdPersonInput>().moving = false;
        
		//player.GetComponent<vThirdPersonInput> ().enabled = val;

		
	}


	public IEnumerator startMiniGame1 ()
	{
        stopDoorSounds();
        player.GetComponent<Interactables>().showText(false);
		game1 = true;
        movePlayer(false);
		bird.SetActive (true);
        showHideUIElements(false);
		while (videoCanvas.GetComponent<Video> ().started == true) {
			yield return new WaitForSeconds (0.1f);
		}

        movePlayer(false);

		player.GetComponent<Player> ().setPos (1);

		gameItems.worldCamera = miniGame1;
		buttonMash.reset ();
	
		yield return new WaitForSeconds (2f);
		buttonMash.beginButtonMash = true;
	
	}

	public void endMiniGame (bool theEnd)
	{
        print("end mini game");
        SoundManager.instance.switchTo("norm");
        SoundManager.instance.playRockingChair();
        player.GetComponent<Interactables>().showText(true);
		buttonMash.beginButtonMash = false;
		trajectory.moveSlider = false;
		game1 = false;
		game2 = false;
		game3 = false;
		startRunning = false;
		bird.SetActive (false);
		gameItems.worldCamera = playerCam;

		if (!theEnd) {
			gameItems.worldCamera = playerCam;
            print("1");
			useCamera ("player");
			movePlayer (true);
            showHideUIElements(true);

			if (currItemIndex != storyItems.Length - 1)
				nextItem ();
		} else {
			print ("overall score = " + overallScore);
			videoCanvas.GetComponent<Video> ().canSkip = false;
			setNextVideo ();
			playVideo ("canvas");
			StartCoroutine (waitToReset ());
		}

	}

	public IEnumerator startMiniGame2 ()
	{
        stopDoorSounds();
        player.GetComponent<Interactables>().showText(false);
		game2 = true;
		bird.SetActive (true);
        showHideUIElements(false);
		while (videoCanvas.GetComponent<Video> ().started == true) {
			yield return new WaitForSeconds (0.1f);
		}

		movePlayer (false);
		player.GetComponent<Player> ().setPos (2);

		gameItems.worldCamera = miniGame2;
		buttonMash.reset ();

		yield return new WaitForSeconds (2f);
		buttonMash.beginButtonMash = true;
	}

	public IEnumerator startMiniGame3 ()
	{
        stopDoorSounds();
        player.GetComponent<Interactables>().showText(false);
		game3 = true;
		bird.SetActive (true);
        critter.SetActive(true);
        showHideUIElements(false);


        //pitchfork.GetComponent<Pitchfork>().setPos3();

		while (videoCanvas.GetComponent<Video> ().started == true) {
			yield return new WaitForSeconds (0.1f);
		}
			
        startRunning = true;

		movePlayer (false);
        player.GetComponent<Player>().resetSpot3();
		player.GetComponent<Player> ().setPos (3);

		gameItems.worldCamera = miniGame3;
		trajectory.reset ();

		yield return new WaitForSeconds (1f);
		trajectory.moveSlider = true;
	}

    private void showHideUIElements(bool val)
    {
        settingsButton.SetActive(val);
        gearFrame.SetActive(val);
    }

	public IEnumerator waitForVideo (bool nDay)
	{


		while (videoCanvas.GetComponent<Video> ().started == true) {
			yield return new WaitForSeconds (0.1f);
		}


		
		yield return new WaitForSeconds (0.1f);
		if (currItemIndex != storyItems.Length - 1)
			nextItem ();

		if (nDay)
			nextDay ();

	}

	public IEnumerator waitToReset ()
	{
		while (videoCanvas.GetComponent<Video> ().started == true) {
			yield return new WaitForSeconds (0.1f);
		}
		yield return new WaitForSeconds (0.1f);
		reset ();
	}

	public void nextDay ()
	{
		
		player.transform.position = startPoint.position;
		player.transform.rotation = startPoint.rotation;

        playerCam.transform.position = camResetPos.position;
        playerCam.transform.rotation = camResetPos.rotation;

		isNextDay = true;
        SoundManager.instance.playCritterAtDoor();

	}


	public void setNextVideo ()
	{
		videoCanvas.GetComponent<Video> ().setNextVideo ();
	}

	public void playVideo (string backCam)
	{
		
		videoCanvas.GetComponent<Video> ().playVideo (backCam);
		//yield return new WaitForSeconds (1f);
	}

	public void resetMats ()
	{
		for (int i = 0; i < storyItems.Length; i++) {
			storyItems [i].GetComponent<MeshRenderer> ().material = regularMats [i];
		}
	}

	public void setCurrItem (int index)
	{
		resetMats ();
		currItem = storyItems [index];
		currItem.GetComponent<MeshRenderer> ().material = glowMats [index];
		//arrow.GetComponent<Float> ().setPos (new Vector3 (currItem.transform.position.x, currItem.transform.position.y + 2.5f, currItem.transform.position.z));

	}


	public void showArrow (bool val)
	{
		arrow.SetActive (val);
	}

	public void nextItem ()
	{
		currItemIndex++;
		setCurrItem (currItemIndex);
	}

	public void useCamera (string cam)
	{
        print("changed camera");
        disableCams();

		switch (cam) {
		case "canvas":
			canvasCam.enabled = true;
                //movePlayer(false);
			break;
		case "player":
			playerCam.enabled = true;
            movePlayer(true);
            picLights.SetActive(false);
                SoundManager.instance.playRockingChair();
			break;
		case "movie":
			movieCam.enabled = true;
                //movePlayer(false);
			break;
		case "miniGame1":
			miniGame1.enabled = true;
               // movePlayer(false);
			break;
		case "miniGame2":
			miniGame2.enabled = true;
                //movePlayer(false);
			break;
		case "miniGame3":
			miniGame3.enabled = true;
                //movePlayer(false);
			break;       
        case "pics":
            picViewingCam.enabled = true;
            movePlayer(false);
            picLights.SetActive(true);
            break;
		}

		currentCam = cam;
	}

	private void disableCams ()
	{
		canvasCam.enabled = false;
		playerCam.enabled = false;
		movieCam.enabled = false;
		miniGame1.enabled = false;
		miniGame2.enabled = false;
		miniGame3.enabled = false;
        picViewingCam.enabled = false;
	}

	public void toggleController ()
	{
        usingController = !usingController;
	}

	public void testing ()
	{
		print ("Clicking the item");
	}

	public void showElement (GameObject elem)
	{
		elem.SetActive (true);
	}

	public void hideElement (GameObject elem)
	{
		elem.SetActive (false);
	}

	public void toggleMusic ()
	{
        musicOn = !musicOn;
	}

	public void setMusicVolume (Slider slide)
	{
		musicVolume = slide.value;
        SoundManager.instance.setVolume(musicVolume);
	}

	public void addToScore (int val)
	{
		overallScore += val;
	}

    public void goUp(bool val)
    {
        newPicCamPos = picViewingCam.transform.position;

        if (val)
        {
            //move picViewingCam up if not up
            newPicCamPos.y = 0.85f;
            hideElement(upButton);
            showElement(downButton);
        }
        else
        {
            //move picViewingCam down if not down
            newPicCamPos.y = -0.08f;
            hideElement(downButton);
            showElement(upButton);

        }

    }



    /**************************************************************************/

	public void reset ()
	{
		musicOn = true;
		musicVolume = 0.5f;

        currItemIndex = 0;
		//currItemIndex = 4;
		setCurrItem (currItemIndex);

		overallScore = 0;

        disableCams();
		useCamera ("canvas");
		gameItems.worldCamera = playerCam;

        picLights.SetActive(false);

        playerCam.transform.position = camResetPos.position;
        playerCam.transform.rotation = camResetPos.rotation;

        showHideUIElements(true);
		bird.SetActive (false);
        critter.SetActive(false);

        bird.GetComponent<Bird>().reset();

        player.transform.position = startPoint.position;
        player.transform.rotation = startPoint.rotation;
		movePlayer (false);

        playerCam.transform.position = new Vector3(player.transform.position.x + 4f, player.transform.position.y, player.transform.position.z);
        playerCam.transform.eulerAngles = new Vector3(0, 90, 0);

		pitchfork.transform.parent = null;
		pitchfork.transform.position = pitchforkStart.position;
		pitchfork.transform.rotation = pitchforkStart.rotation;
        pitchfork.GetComponent<Pitchfork>().reset();

		player.GetComponent<Interactables> ().reset ();
		player.GetComponent<Player> ().resetSpot3 ();

		videoCanvas.GetComponent<Video> ().canSkip = true;

        GetComponent<GameManager>().enabled = true;
       // SoundManager.instance.setVolume(musicVolume);
       
	}

	public void quit ()
	{
		Application.Quit ();
	}

}
