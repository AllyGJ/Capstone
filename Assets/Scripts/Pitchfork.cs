using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pitchfork : MonoBehaviour
{
    bool holding = false;

    public GameObject grabber;
    public GameObject playerHand;


    private string interact;

    public Transform game3Pos;

    private bool throwing = false;

    // Update is called once per frame
    void Update()
    {
        checkController();

        if(GameManager.instance.game3){
            if (!throwing)
            {
                this.transform.position = game3Pos.position;
                this.transform.rotation = game3Pos.rotation;
            }
            else{
                //transform.position = Vector3.Lerp(game3Pos.position, new Vector3(GameManager.instance.bird.transform.position.x / 2f,
                //                                                                 GameManager.instance.bird.transform.position.y * 2f,
                //                                                                 GameManager.instance.bird.transform.position.z / 2f), 0.5f);
                //transform.position = Vector3.Lerp(new Vector3(GameManager.instance.bird.transform.position.x / 2f,
                                                  //                               GameManager.instance.bird.transform.position.y * 2f,
                                                  //                               GameManager.instance.bird.transform.position.z / 2f),
                                                  //GameManager.instance.bird.transform.position, 0.5f);
                transform.position = Vector3.Lerp(game3Pos.position,
                                                  GameManager.instance.bird.transform.position, 0.5f);
            }
        }
    }

    public void pickup()
    {
        holding = true;
        this.transform.position = playerHand.transform.position;
        this.transform.parent = playerHand.transform;

    }

    public void putDown()
    {
        holding = false;
        this.transform.parent = null;
    }

    public void throwAtBird(int value){
        GameManager.instance.bird.GetComponent<Bird>().pauseFlying = true;
        throwing = true;
       



        //if(value == 5){

        //}else if(value == 4){
            
        //}else if(value == 3){
            
        //}
        //else{
            
        //}
    }
    public void resetPos(){
        throwing = false;
        GameManager.instance.bird.GetComponent<Bird>().pauseFlying = false;

        transform.position = game3Pos.position;
        transform.rotation = game3Pos.rotation;
    }

    public void setPos3()
    {
        this.transform.parent = null;
        this.transform.position = game3Pos.position;
        this.transform.rotation = game3Pos.rotation;
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
