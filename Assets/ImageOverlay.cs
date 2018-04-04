using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageOverlay : MonoBehaviour {

    public GameObject news1;
    public GameObject masterAsKid;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(this.gameObject.activeSelf)
        {
            if (Input.GetKeyDown("escape") || Input.GetKeyDown("joystick button 17") || Input.GetKeyDown("joystick button 1"))
            {
                CloseImgOverlay();
            }
        }
	}

    public void ShowPaper()
    {
        this.gameObject.SetActive(true);
        news1.SetActive(true);
        masterAsKid.SetActive(false);
    }

    public void ShowPortrait()
    {
        this.gameObject.SetActive(true);
        news1.SetActive(false);
        masterAsKid.SetActive(true);   
    }

    public void CloseImgOverlay()
    {
        this.gameObject.SetActive(false);
    }
}
