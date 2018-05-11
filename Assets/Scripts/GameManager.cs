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
    public bool spacedOutItems = false;
    public bool usingController;
	public bool musicOn;
	public float musicVolume;

    [Header("House Cameras")]
    public Camera currentHouseCam;
    public GameObject[] houseCams;

    public GameObject[] afterGameCams;

    [Header("Cameras")]
    public string currentCam;

    public Camera initialCam;
    public Camera canvasCam;
    public Camera playerCam;
    public Camera movieCam;
    public Camera miniGame1;
    public Camera miniGame2;
    public Camera miniGame3;
    public Camera picViewingCam;

    [Header("Game Objects")]
    public GameObject player;
    public Animator playerAnim;
	public GameObject bird;
    public GameObject critter;
	public GameObject pitchfork;
	public GameObject arrow;
    public Animator rockingChairAnim;
    public GameObject blanket;

	public Transform pitchforkStart;
	public Transform startPoint;
    public Transform camResetPos;

	public GameObject settingsButton;
    public GameObject settingsMain;
    public GameObject gearFrame;
	public Canvas gameItems;
    [HideInInspector] public ImageOverlay imgOverlay;
	public GameObject videoCanvas;

    public GameObject upButton;
    public GameObject downButton;
    public GameObject picLights;

	public GameObject[] storyItems;
	public GameObject currItem;

	public Material[] regularMats;
	public Material[] glowMats;

    public Slider canvasVolume;

	public int overallScore;



    [Header("Door sounds")]
    public AudioSource[] doorSounds;

    [Header("Booleans")]

    public bool isNextDay = false;

	public bool game1 = false;
	public bool game2 = false;
	public bool game3 = false;

	public bool startRunning = false;

    public bool showingCanvas = true;

   /******************************************/

	private int currItemIndex = 0;

	private ButtonMash buttonMash;
	private Trajectory trajectory;

	private Transform camPosRot;

    private Vector3 newPicCamPos;

    private bool settingsOpen = false;





	void Awake ()
	{

		if (instance == null)
			instance = this;
        //else if (instance != this)
        //Destroy (gameObject);


        //DontDestroyOnLoad (gameObject);


        playerAnim = player.GetComponent<Animator>();
		buttonMash = GetComponent<ButtonMash> ();
		trajectory = GetComponent<Trajectory> ();

        newPicCamPos = picViewingCam.transform.position;

        //camPosRot = playerCam.transform;
        imgOverlay = gameItems.GetComponentInChildren<ImageOverlay>();

		musicOn = true;
		musicVolume = 0.5f;
		reset ();

	}

    void Update()
    {
        if (picViewingCam.enabled)
        {
            if (Input.GetKeyDown("joystick button 5") || Input.GetKeyDown("up") || Input.GetAxis("DPadY") > 0)
            {
                goUp(true);
            }
            else if (Input.GetKeyDown("joystick button 6") || Input.GetKeyDown("down") || Input.GetAxis("DPadY") < 0){
                goUp(false);
            }

            if(Input.GetKeyDown("escape") || Input.GetKeyDown("joystick button 17") || Input.GetKeyDown("joystick button 1")){
                print("2");
                useCamera("lastCam");
            }
        }

        if (picViewingCam.transform.position != newPicCamPos)
        {
            picViewingCam.transform.position = Vector3.Lerp(picViewingCam.transform.position, newPicCamPos, Time.deltaTime * 2f);
        }

        if(currentCam == "lastCam" || currentCam == "outside"){
            if(!usingController && Input.GetKeyDown("z")){
                if (settingsOpen) closeSettings();
                else openSettings();
            }else if(usingController){
                if((macBuild && Input.GetKeyDown("joystick button 9")) || (!macBuild && Input.GetKeyDown("joystick button 7"))){
                    if (settingsOpen) closeSettings();
                    else openSettings();
                }
            }
        }

        if(usingController){
            settingsButton.GetComponent<Button>().GetComponentInChildren<Text>().text = "start";
        }else{
            settingsButton.GetComponent<Button>().GetComponentInChildren<Text>().text = "z";
        }



    }

    public void stopShowingCanvas()
    {
        showingCanvas = false;
        canvasVolume.interactable = false;
    }

    public void openSettings()
    {
        settingsOpen = true;
        showElement(settingsMain);
        movePlayer(false);
    }
    public void closeSettings()
    {
        settingsOpen = false;
        hideElement(settingsMain);
        movePlayer(true);
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
        useCamera("miniGame1");
        SoundManager.instance.muteWalk(true);
        SoundManager.instance.stopRockingChair();
        rockingChairAnim.SetBool("rock", false);
        SoundManager.instance.stopBirdPeck();
        stopDoorSounds();
        player.GetComponent<Interactables>().showText(false);
		game1 = true;
        movePlayer(false);
		bird.SetActive (true);
        showHideUIElements(false);
		while (videoCanvas.GetComponent<Video> ().started == true) {
			yield return new WaitForSeconds (0.1f);
		}

        bird.GetComponent<Float>().birdSounds = true;
        movePlayer(false);

		player.GetComponent<Player> ().setPos (1);

        //pitchfork.transform.eulerAngles = new Vector3(0, 0, -100);
        //pitchfork.transform.rotation = new Quaternion(0, 0, -100, 0);

		gameItems.worldCamera = miniGame1;
		buttonMash.reset ();
	
		yield return new WaitForSeconds (2f);
		buttonMash.beginButtonMash = true;
	
	}

	public void endMiniGame (bool theEnd)
	{
       // print("end mini game");
        SoundManager.instance.muteWalk(false);
        SoundManager.instance.switchTo("norm");
        player.GetComponent<Interactables>().showText(true);
        player.transform.eulerAngles = new Vector3(0, 0, 0);
		buttonMash.beginButtonMash = false;
		trajectory.moveSlider = false;

        if (game1)
        {
            rockingChairAnim.SetBool("rock",true);
            SoundManager.instance.playRockingChair();
            currentHouseCam = afterGameCams[0].GetComponent<Camera>();
            bird.GetComponent<Float>().birdSounds = false;
            bird.GetComponent<Float>().floating = false;
        }
        else if(game2){
           
            rockingChairAnim.SetBool("rock", false);
            SoundManager.instance.stopRockingChair();
            //currentHouseCam = afterGameCams[1].GetComponent<Camera>();
            //bird.GetComponent<Float>().birdSounds = false;
            //bird.GetComponent<Float>().floating = false;
        }

        if(game3) playerAnim.SetTrigger("Game3PosDone");

        if(bird.activeSelf) bird.SetActive(false);

        game1 = false;
		game2 = false;
		game3 = false;
		startRunning = false;

        gameItems.worldCamera = currentHouseCam;

		if (!theEnd) {
			useCamera ("lastCam");
			movePlayer (true);
            showHideUIElements(true);

            if (currItemIndex != storyItems.Length - 1){
                if (spacedOutItems) StartCoroutine(waitAndSetNextItem(10f));
                else nextItem();
            }
				
		} else {
			print ("overall score = " + overallScore);
			videoCanvas.GetComponent<Video> ().canSkip = false;
            //setNextVideo ();

            if (overallScore >= 19f) {
                videoCanvas.GetComponent<Video>().setGoodEnding(true);
            }
            else videoCanvas.GetComponent<Video>().setGoodEnding(false);

			playVideo ("canvas");
			StartCoroutine (waitToReset ());
		}

	}

	public IEnumerator startMiniGame2 ()
	{
        SoundManager.instance.muteWalk(true);
        SoundManager.instance.stopRockingChair();
        rockingChairAnim.SetBool("rock", false);
        stopDoorSounds();
        player.GetComponent<Interactables>().showText(false);
		game2 = true;
		bird.SetActive (true);
        showHideUIElements(false);
		while (videoCanvas.GetComponent<Video> ().started == true) {
			yield return new WaitForSeconds (0.1f);
		}

        bird.GetComponent<Float>().birdSounds = true;

		movePlayer (false);
		player.GetComponent<Player> ().setPos (2);

		gameItems.worldCamera = miniGame2;
		buttonMash.reset ();

		yield return new WaitForSeconds (2f);
		buttonMash.beginButtonMash = true;
	}

	public IEnumerator startMiniGame3 ()
	{
        SoundManager.instance.muteWalk(true);
        SoundManager.instance.stopRockingChair();
        stopDoorSounds();
        rockingChairAnim.SetBool("rock", false);
        player.GetComponent<Interactables>().showText(false);
		game3 = true;
		bird.SetActive (true);
        critter.SetActive(true);
        showHideUIElements(false);
        //pitchfork.GetComponent<Pitchfork>().putDown();

        pitchfork.GetComponent<Pitchfork>().setPos3();

        playerAnim.SetTrigger("Game3Pos");
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

    public IEnumerator waitAndSetNextItem(float timeToWait){
        resetMats();
        yield return new WaitForSeconds(timeToWait);
        nextItem();
    }

    private void showHideUIElements(bool val)
    {
        settingsButton.SetActive(val);
        gearFrame.SetActive(val);
    }

    public IEnumerator waitForGameScene()
    {
        currentHouseCam = afterGameCams[1].GetComponent<Camera>();

        bird.GetComponent<Float>().floating = false;
        bird.GetComponent<Float>().birdSounds = false;
        bird.SetActive(false);

        blanket.SetActive(true);

        setNextVideo();
        playVideo("lastCam");

        while (videoCanvas.GetComponent<Video>().started == true)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.1f);
        endMiniGame(false);

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

        //playerCam.transform.position = camResetPos.position;
        //playerCam.transform.rotation = camResetPos.rotation;

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

        if (index == 1)
        {
            SoundManager.instance.playBirdPeck();
        }
		currItem.GetComponent<MeshRenderer> ().material = glowMats [index];
		//arrow.GetComponent<Float> ().setPos (new Vector3 (currItem.transform.position.x, currItem.transform.position.y + 2.5f, currItem.transform.position.z));

	}

    public void useCamera(string cam)
    {
        disableCams();
        player.GetComponent<Interactables>().itemTxt.enabled = true;
        picLights.SetActive(false);

        switch (cam)
        {
            case "canvas":
                canvasCam.enabled = true;
                break;
            case "outside":
                movePlayer(true);
                playerCam.enabled = true;
                player.GetComponent<vThirdPersonInput>().setCamera(playerCam);
                gameItems.worldCamera = playerCam;
                break;
            case "movie":
                movePlayer(false);
                movieCam.enabled = true;
                break;
            case "miniGame1":
                miniGame1.enabled = true;
                player.GetComponent<Interactables>().itemTxt.enabled = false;
                break;
            case "miniGame2":
                miniGame2.enabled = true;
                player.GetComponent<Interactables>().itemTxt.enabled = false;
                break;
            case "miniGame3":
                miniGame3.enabled = true;
                player.GetComponent<Interactables>().itemTxt.enabled = false;
                break;
            case "pics":
                player.GetComponent<Interactables>().itemTxt.enabled = false;
                picViewingCam.enabled = true;
                movePlayer(false);
                picLights.SetActive(true);
                break;
            case "lastCam":
                movePlayer(true);
                currentHouseCam.enabled = true;
                gameItems.worldCamera = currentHouseCam;
                SoundManager.instance.playRockingChair();
                player.GetComponent<vThirdPersonInput>().setCamera(currentHouseCam);
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


	public void showArrow (bool val)
	{
		arrow.SetActive (val);
	}

	public void nextItem ()
	{
      //  print("next item");
		currItemIndex++;
		setCurrItem (currItemIndex);
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

    public void toggleColorSelected(Text txt)
    {
        txt.color = new Color(0, 0.95f, 0.3f);
    }
    public void toggleColorDeselected(Text txt)
    {
        txt.color = Color.white;
    }

    public void toggleLineOn(GameObject line)
    {
        line.SetActive(true);
    }

    public void toggleLineOff(GameObject line)
    {
        line.SetActive(false);
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
        blanket.SetActive(false);
		musicOn = true;
		musicVolume = 0.5f;

        currItemIndex = 0;
		//currItemIndex = 4;
		setCurrItem (currItemIndex);

        currentHouseCam = initialCam;

		overallScore = 0;

        showingCanvas = true;
        canvasVolume.interactable = true;
		useCamera ("canvas");
		
        rockingChairAnim.SetBool("rock",true);

        picLights.SetActive(false);

        player.GetComponent<vThirdPersonInput>().setCamera(initialCam);

        showHideUIElements(true);
		bird.SetActive (false);
        critter.SetActive(false);

        bird.GetComponent<Bird>().reset();

        player.transform.position = startPoint.position;
        player.transform.rotation = startPoint.rotation;
		movePlayer (false);

		pitchfork.transform.parent = null;
		pitchfork.transform.position = pitchforkStart.position;
		pitchfork.transform.rotation = pitchforkStart.rotation;
        pitchfork.GetComponent<Pitchfork>().reset();

		player.GetComponent<Interactables> ().reset ();
		player.GetComponent<Player> ().resetSpot3 ();

		videoCanvas.GetComponent<Video> ().canSkip = true;
        videoCanvas.GetComponent<Video>().reset();

        GetComponent<GameManager>().enabled = true;

        settingsOpen = false;

        imgOverlay.CloseImgOverlay();


    }

	public void quit ()
	{
		Application.Quit ();
	}

}
