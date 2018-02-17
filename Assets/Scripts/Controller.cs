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
        moveTxt.text = "<color=black>Arrows keys</color> <size=20> - move robot</size>";
        camTxt.text = "<color=black>ASWD</color> <size=20>- rotate camera</size>";
        interactTxt.text = "<color=black>E</color> <size=20>- open doors and interact with items</size>";
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
            moveTxt.text = "<color=black>Left joystick</color> <size=20>- move robot</size>";
            camTxt.text = "<color=black>Right joystick</color> <size=20>- rotate camera</size>";
            interactTxt.text = "<color=black>A</color> <size=20>- open doors and interact with items</size>";

        }
        else
        {
            GetComponent<Image>().sprite = noController;
            moveTxt.text = "<color=black>Arrows keys</color><size=20> - move robot</size>";
            camTxt.text = "<color=black>ASWD</color> <size=20>- rotate camera</size>";
            interactTxt.text = "<color=black>E</color> <size=20>- open doors and interact with items</size>";

        }
    }
}
