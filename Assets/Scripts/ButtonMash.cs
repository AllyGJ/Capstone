using System.Collections;
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

	public Sprite[] btns;
	private string[] keys;


	public string[] xboxKeys; //Mac
	public string[] keyKeys; //keyboard

    public string[] xboxKeysWin;//Windows

	public bool userInputMatches = false;
    public bool userInputWrong = false;

	private int numButtons = 8;
	public int curButton = 0;
	public int correct = 0;

    private bool pressedButton = false;

    private bool canChooseBtn = true;

	void Start ()
	{
		btns = new Sprite[4];
		keys = new String[4];

		CenterBtn.GetComponent<Image> ();
		LoadingBar.GetComponent<Image> ();

		//checkController ();

		//randomlyPickBtn ();
		showButton (false);
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		checkController ();

		if (beginButtonMash) {

            if (canChooseBtn)
            {
                randomlyPickBtn();
                canChooseBtn = false;
            }

			if (curButton < numButtons) {
                showButton(true);
				fillBar ();

				checkForUserInput ();


                if (userInputMatches && LoadingBar.fillAmount < 1)
                {
                    StartCoroutine(matchedButton());
                }
                else if (userInputWrong)
                {
                    StartCoroutine(pressedWrongButton());
                }

                //need to add case when user doesn't press anything


            } else {
				//shown all buttons, check how many correct
//				if (correct == numButtons) {
//					print ("perfect score!");
//				} else {
//					float percent = ((float)correct / (float)numButtons) * 100;
//					print ("percent correct = " + percent);
//				}
				print (correct);
				GameManager.instance.addToScore (correct - 1);
				beginButtonMash = false;

                if (GameManager.instance.game2)
                {
                    StartCoroutine(GameManager.instance.waitForGameScene());
                }
                else
                {
                    GameManager.instance.endMiniGame(false);
                }
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
            if (GameManager.instance.macBuild)
            {
                UseArray(xboxBtns, xboxKeys);
            }
            else{
                UseArray(xboxBtns, xboxKeysWin);
            }
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
		int rand = UnityEngine.Random.Range (0,3);
//        print("rand: "+rand);
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

        pressedButton = false;
        if (Input.anyKeyDown)
        {
            pressedButton = true;
            if (Input.GetKeyDown(btn2Press))
            {
                userInputMatches = true;
            }

            else
            {
                userInputWrong = true;
                LoadingBar.color = Color.red;
            }
        }
        //else{
        //    print("didn't touch any key");
        //}
	}


	void fillBar ()
	{
		LoadingBar.fillAmount += Time.deltaTime * 0.5f;
	
		if (LoadingBar.fillAmount >= 0.75f)
			LoadingBar.color = Color.red;
		
        if (LoadingBar.fillAmount == 1f) {
            if (!pressedButton)
                userInputWrong = true;
		}

	}
    IEnumerator pressedWrongButton()
    {
        beginButtonMash = false;
        correct--;
        curButton++;
        SoundManager.instance.playWrong();

        float r = UnityEngine.Random.Range(0f, 1f);
        if (r == 1f) GameManager.instance.playerAnim.SetTrigger("BigD");
        else GameManager.instance.playerAnim.SetTrigger("LittleD");

        yield return new WaitForSeconds(2f);

        LoadingBar.fillAmount = 0;
        LoadingBar.color = Color.blue;
       // randomlyPickBtn();
        userInputWrong = false;
        beginButtonMash = true;
        canChooseBtn = true;
    }
    IEnumerator matchedButton ()
	{
        beginButtonMash = false;
		correct++;
		curButton++;
        SoundManager.instance.playWhip();

        //show animation of robot hitting bird
        GameManager.instance.playerAnim.SetTrigger("Stab");

        yield return new WaitForSeconds(2f);

		LoadingBar.fillAmount = 0;
        LoadingBar.color = Color.blue;
		//randomlyPickBtn ();
		userInputMatches = false;
        beginButtonMash = true;
        canChooseBtn = true;
	}

	public void reset ()
	{
		curButton = 0;
		correct = 0;
		LoadingBar.fillAmount = 0f;
        LoadingBar.color = Color.blue;
		userInputMatches = false;
        canChooseBtn = true;
	}
}