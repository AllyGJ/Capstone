using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

#if UNITY_EDITOR
using UnityEditor;
#endif
using Invector.CharacterController;
using System;

public class GameManager : MonoBehaviour
{

	public static GameManager instance = null;

	[Header ("Settings")]
	public bool DEBUG;
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

	[Header ("Game Objects")]
	public GameObject player;
	public GameObject bird;
	public GameObject pitchfork;
	public GameObject arrow;

	public Transform pitchforkStart;
	public Transform startPoint;

	public GameObject settingsButton;
	public Canvas gameItems;
	public GameObject videoCanvas;

	public GameObject[] storyItems;
	public GameObject currItem;

	public Material[] regularMats;
	public Material[] glowMats;

	public int overallScore;

    [Header("UI objects")]
    public Button soundButton;
    public Sprite sound;
    public Sprite noSound;
    public GameObject volume;

    public Button controllerButton;
    public Sprite controller;
    public Sprite noController;

    public Text moveTxt;
    public Text camTxt;
    public Text interactTxt;


	[Header ("Booleans")]
	public bool isNextDay = false;

	public bool game1 = false;
	public bool game2 = false;
	public bool game3 = false;

	public bool startRunning = false;

	private int currItemIndex = 0;

	private ButtonMash buttonMash;
	private Trajectory trajectory;

	private Transform camPosRot;

	void Awake ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		//DontDestroyOnLoad (gameObject);

		buttonMash = GetComponent<ButtonMash> ();
		trajectory = GetComponent<Trajectory> ();

		//default
		//usingController = false;

//		if (DEBUG)
//			useCamera ("player");
//		else


//		player.GetComponent<vThirdPersonController> ().enabled = false;
//		player.GetComponent<vThirdPersonInput> ().enabled = false;
//		playerCam.enabled = false;
		camPosRot = playerCam.transform;

		musicOn = true;
		musicVolume = 0.5f;
		reset ();

	}


	public void movePlayer (bool val)
	{
		player.GetComponent<vThirdPersonInput> ().enabled = val;

		if (val)
			player.GetComponent<Player> ().removeConstraints ();
	}

	public IEnumerator startMiniGame1 ()
	{
		game1 = true;
		bird.SetActive (true);
		settingsButton.SetActive (false);
		while (videoCanvas.GetComponent<Video> ().started == true) {
			yield return new WaitForSeconds (0.1f);
		}

		player.GetComponent<vThirdPersonInput> ().enabled = false;
		player.GetComponent<Player> ().setPos (1);

		gameItems.worldCamera = miniGame1;
		buttonMash.reset ();
	
		yield return new WaitForSeconds (2f);
		buttonMash.beginButtonMash = true;
	
	}

	public void endMiniGame (bool theEnd)
	{
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
			useCamera ("player");
			movePlayer (true);
			settingsButton.SetActive (true);

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
		game2 = true;
		bird.SetActive (true);
		settingsButton.SetActive (false);
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
		game3 = true;
		bird.SetActive (true);
		settingsButton.SetActive (false);
		while (videoCanvas.GetComponent<Video> ().started == true) {
			yield return new WaitForSeconds (0.1f);
		}
			
		startRunning = true;
	
		movePlayer (false);
		player.GetComponent<Player> ().setPos (3);

		gameItems.worldCamera = miniGame3;
		trajectory.reset ();

		yield return new WaitForSeconds (1f);
		trajectory.moveSlider = true;
	}

	public IEnumerator waitForVideo (bool nDay)
	{
		//print ("wait 1");
		while (videoCanvas.GetComponent<Video> ().started == true) {
			yield return new WaitForSeconds (0.1f);
		}
		//print ("wait 2");


		
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

		playerCam.transform.position = new Vector3 (14f, 0.15f, 0.3f);
		playerCam.transform.rotation = startPoint.rotation;

		isNextDay = true;

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
		disableCams ();
		switch (cam) {
		case "canvas":
			canvasCam.enabled = true;
			break;
		case "player":
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
	}

	public void toggleController ()
	{
		usingController = !usingController;
        if (usingController)
        {
            controllerButton.GetComponent<Image>().sprite = controller;
            moveTxt.text = "Right joystick to move robot";
            camTxt.text = "Left joystick to rotate camera";
            interactTxt.text = "A to open doors and interact with items";

        }
        else
        {
            controllerButton.GetComponent<Image>().sprite = noController;
            moveTxt.text = "Arrows keys to move robot";
            camTxt.text = "ASWD to rotate camera";
            interactTxt.text = "E to open doors and interact with items";

        }
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
        if(musicOn){
            soundButton.GetComponent<Image>().sprite = sound;
            volume.SetActive(true);
        }
        else{
            soundButton.GetComponent<Image>().sprite = noSound;
            volume.SetActive(false);
        }
	}

	public void setMusicVolume (Slider slide)
	{
		musicVolume = slide.value;
	}

	public void addToScore (int val)
	{
		overallScore += val;
	}

	public void reset ()
	{
		musicOn = true;
		musicVolume = 0.5f;

		currItemIndex = 0;
		setCurrItem (currItemIndex);

		overallScore = 0;

		useCamera ("canvas");
		gameItems.worldCamera = playerCam;

		playerCam.transform.position = camPosRot.position;
		playerCam.transform.rotation = new Quaternion (0, -90, 0, 0);

		settingsButton.SetActive (true);
		bird.SetActive (false);

		player.transform.position = startPoint.position;
		player.transform.rotation = startPoint.rotation;
		movePlayer (false);

		playerCam.transform.position = new Vector3 (14f, 0.15f, 0.3f);
		playerCam.transform.rotation = startPoint.rotation;

		pitchfork.transform.parent = null;
		pitchfork.transform.position = pitchforkStart.position;
		pitchfork.transform.rotation = pitchforkStart.rotation;

		player.GetComponent<Interactables> ().reset ();
		player.GetComponent<Player> ().resetSpot3 ();

		videoCanvas.GetComponent<Video> ().canSkip = true;
	}

	public void quit ()
	{
		Application.Quit ();
	}

}
