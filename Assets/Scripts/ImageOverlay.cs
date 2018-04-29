using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageOverlay : MonoBehaviour {

    public GameObject[] news;
    public GameObject[] blueprints;
    public GameObject masterAsKid;

    public GameObject[] curImgs;

    public Button right;
    public Button left;
    public Button exit;

    private int curIndex;

	// Use this for initialization
	void Start () {
        right.onClick.AddListener(()=>{
            NextImg(1);
        });

        left.onClick.AddListener(() => {
            NextImg(-1);
        });

        exit.onClick.AddListener(()=>{
            CloseImgOverlay();
        });

        //HideNews();
        //HidePrints();
        curIndex = 0;
        left.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if(this.gameObject.activeSelf)
        {
            GameManager.instance.movePlayer(false);

            if (Input.GetKeyDown("right") || Input.GetKeyDown("joystick button 8") || Input.GetAxis("DPadX") > 0) right.onClick.Invoke();
            if (Input.GetKeyDown("left") || Input.GetKeyDown("joystick button 7") || Input.GetAxis("DPadX") < 0) left.onClick.Invoke();

            if (Input.GetKeyDown("escape") || Input.GetKeyDown("joystick button 17") || Input.GetKeyDown("joystick button 1"))
            {
                CloseImgOverlay();
            }


        }
	}

    public void ShowPaper()
    {
        this.gameObject.SetActive(true);

        curImgs = new GameObject[news.Length];
        curImgs = news;

        HideCurImg();
        curImgs[curIndex].SetActive(true);
        right.gameObject.SetActive(true);

        masterAsKid.SetActive(false);
        HidePrints();
    }

    public void ShowPrints()
    {
        this.gameObject.SetActive(true);

        curImgs = new GameObject[blueprints.Length];
        curImgs = blueprints;

        HideCurImg();
        curImgs[curIndex].SetActive(true);
        right.gameObject.SetActive(true);

        HideNews();
        masterAsKid.SetActive(false);
    }

    private void HideNews()
    {
        foreach(GameObject ob in news){
            ob.SetActive(false);
        }
    }

    private void HidePrints()
    {
        foreach (GameObject ob in blueprints)
        {
            ob.SetActive(false);
        }
    }

    private void HideCurImg()
    {
        foreach (GameObject ob in curImgs)
        {
            ob.SetActive(false);
        }
    }

    public void ShowPortrait()
    {
        this.gameObject.SetActive(true);
        HideNews();
        HidePrints();
        left.gameObject.SetActive(false);
        right.gameObject.SetActive(false);
        masterAsKid.SetActive(true);   
    }

    public void CloseImgOverlay()
    {
        this.gameObject.SetActive(false);
        curIndex = 0;
        GameManager.instance.movePlayer(true);

        curImgs = new GameObject[0];
    }

    private void NextImg(int dir)
    {
        bool goRight = (curIndex + dir > curIndex) && (curIndex + dir <= curImgs.Length - 1);
        bool goLeft = (curIndex + dir < curIndex) && (curIndex + dir >= 0);

        if (goRight)
        {
            curIndex += dir;
            left.gameObject.SetActive(true);
            right.gameObject.SetActive(false);
        }
        else if (goLeft)
        {
            curIndex += dir;
            right.gameObject.SetActive(true);
            left.gameObject.SetActive(false);
        }
        else return;

        HideCurImg();
        curImgs[curIndex].SetActive(true);
    }
}
