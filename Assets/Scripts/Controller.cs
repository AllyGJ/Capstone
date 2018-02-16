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

    void Start(){
        moveTxt.text = "<color=#00F448FF>Arrows keys</color> - move robot";
        camTxt.text = "<color=#00F448FF>ASWD</color> - rotate camera";
        interactTxt.text = "<color=#00F448FF>E</color> - open doors and interact with items";
    }

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
            moveTxt.text = "<color=#00F448FF>Left joystick</color> - move robot";
            camTxt.text = "<color=#00F448FF>Right joystick</color> - rotate camera";
            interactTxt.text = "<color=#00F448FF>A</color> - open doors and interact with items";

        }
        else
        {
            GetComponent<Image>().sprite = noController;
            moveTxt.text = "<color=#00F448FF>Arrows keys</color> - move robot";
            camTxt.text = "<color=#00F448FF>ASWD</color> - rotate camera";
            interactTxt.text = "<color=#00F448FF>E</color> - open doors and interact with items";

        }
    }
}
