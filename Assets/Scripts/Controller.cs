using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour {
    public Sprite controller;
    public Sprite noController;

   

   

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

        }
        else
        {
            GetComponent<Image>().sprite = noController;
          

        }
    }
}
