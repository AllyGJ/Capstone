using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pitchfork : MonoBehaviour
{
    bool holding = false;

    public GameObject grabber;


    private string interact;


    // Update is called once per frame
    void Update()
    {
        checkController();

        //		if (holding && !GameManager.instance.game1 && !GameManager.instance.game2 && !GameManager.instance.game3) {
        //			if (Input.GetKeyDown (interact)) {
        //				putDown ();
        //			}
        //		}
    }

    public void pickup()
    {
        holding = true;
        this.transform.parent = grabber.transform;
    }

    public void putDown()
    {
        holding = false;
        this.transform.parent = null;
    }

    public void throwAtBird(int value){
        GameManager.instance.bird.GetComponent<Bird>().pauseFlying = true;
        Vector3 dir = GameManager.instance.bird.transform.position;

        while (transform.position != dir)
        {
            Vector3 diff = dir - transform.position;
            transform.position += diff;
        }

        //if(value == 5){
        //    transform.position = transform.position + Vector3.forward * 2;
        //}else if(value == 4){
            
        //}else if(value == 3){
            
        //}
        //else{
            
        //}
    }
    public void resetPos(){
        GameManager.instance.bird.GetComponent<Bird>().pauseFlying = false;
        transform.position = new Vector3(GameManager.instance.player.transform.position.x + 1f, 
                                         GameManager.instance.player.transform.position.y, 
                                         GameManager.instance.player.transform.position.z);    
    }

	private void checkController ()
	{
		if (GameManager.instance.usingController) {
			interact = "joystick button 17"; //B

		} else {
			interact = "r";
		}

	}
}
