using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour {
    public Sprite controller;
    public Sprite noController;

    public Text moveTxt;
    public Text camTxt;
    public Text interactTxt;


    public void toggleController()
    {
        GameManager.instance.toggleController();
        if (GameManager.instance.usingController)
        {
            GetComponent<Image>().sprite = controller;
            moveTxt.text = "Right joystick to move robot";
            camTxt.text = "Left joystick to rotate camera";
            interactTxt.text = "A to open doors and interact with items";

        }
        else
        {
            GetComponent<Image>().sprite = noController;
            moveTxt.text = "Arrows keys to move robot";
            camTxt.text = "ASWD to rotate camera";
            interactTxt.text = "E to open doors and interact with items";

        }
    }
}
