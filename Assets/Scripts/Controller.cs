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

    void Update()
    {
        if (GameManager.instance.usingController)
        {
            GetComponent<Image>().sprite = controller;
        }else{
            GetComponent<Image>().sprite = noController;

        }
    }

    public void toggleController()
    {
        GameManager.instance.toggleController();
        if (GameManager.instance.usingController)
        {
            GetComponent<Image>().sprite = controller;
            moveTxt.text = "Left joystick - move robot";
            camTxt.text = "Right joystick - rotate camera";
            interactTxt.text = "A - open doors and interact with items";

        }
        else
        {
            GetComponent<Image>().sprite = noController;
            moveTxt.text = "Arrows keys - move robot";
            camTxt.text = "ASWD - rotate camera";
            interactTxt.text = "E - open doors and interact with items";

        }
    }
}
