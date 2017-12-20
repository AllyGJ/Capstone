using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pitchfork : MonoBehaviour
{
    bool holding = false;

    public GameObject grabber;
    public GameObject playerHand;

    private Rigidbody rb;


    private string interact;

    public Transform game3Pos;

    private bool throwing = false;
    private Vector3 velocity = Vector3.forward;



    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        reset();
    }

    // Update is called once per frame
    void Update()
    {
        checkController();

        if(GameManager.instance.game3){
            //constraintsOn(false);

            if (!throwing)
            {
                this.transform.position = game3Pos.position;
                this.transform.rotation = game3Pos.rotation;
            }
            else{

                //Here's where the pitchfork needs to move forward and hit the bird /
                // completely miss the bird if the value passed in to "throwAtBird" is low. 

                var birdPos = GameManager.instance.bird.transform.position;
                Vector3 distance = birdPos - transform.position;


                transform.position = Vector3.Lerp(game3Pos.position,
                                                  birdPos, 0.5f);

                //rb.AddRelativeForce((distance + velocity) * 5f);
                //rb.rotation = Quaternion.LookRotation(rb.velocity);

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
        
        throwing = true;



        if(value == 5){
            GameManager.instance.bird.GetComponent<Bird>().pauseFlying = true;
           // rb.velocity = velocity * 5f;

        }else if(value == 4){
            GameManager.instance.bird.GetComponent<Bird>().pauseFlying = true;
            //rb.velocity = velocity * 5f;
            
        }else if(value == 3){
            GameManager.instance.bird.GetComponent<Bird>().pauseFlying = true;
            //rb.velocity = velocity * 5f;
        }
        else{
           // rb.velocity = velocity * 5f;
            
        }
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

    private void constraintsOn(bool val)
    {
        if (val)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else{
            rb.constraints = RigidbodyConstraints.None;
        }  
    }

    public void reset()
    {
        constraintsOn(true);
    }
}
