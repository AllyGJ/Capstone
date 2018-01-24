using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pitchfork : MonoBehaviour
{
    bool holding = false;

    public GameObject grabber;
    public GameObject playerHand;
    public Transform game3Pos;


    private string interact;

    private bool throwing = false;

    private Vector3 p0;     //start position
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;     //end position

    private float t = 0;

    private void Awake()
    {
        p0 = game3Pos.position;
        p1 = new Vector3(p0.x, p0.y + 1f, p0.z + 1f);
        //p1 = new Vector3(0, 0, 0);
        //p2 = new Vector3(0, 0, 0);
        p3 = new Vector3(0, 0, 0);

        reset();
    }

    // Update is called once per frame
    public void Update()
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

                t = Mathf.Clamp(t + Time.deltaTime, 0, 1);
                //linear
                //Vector3 bezCurve = (p0 + t * (p3 - p0)) * 0.001f;

                //quadratic
                Vector3 bezCurve = ((1 - t)*(1 - t) * p0 + 2*(1 - t)*t*p1+t*t*p3) * 0.001f;
                bezCurve.z += 0.2f;

                transform.localPosition += bezCurve;



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
        p3 = GameManager.instance.bird.transform.localPosition;
        throwing = true;

        //Do different things for different values
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
            if (GameManager.instance.macBuild)
            {
                interact = "joystick button 17"; //B
            }else{
                interact = "joystick button 1";
            }

		} else {
			interact = "r";
		}

	}

    //private void constraintsOn(bool val)
    //{
    //    if (val)
    //    {
    //        rb.constraints = RigidbodyConstraints.FreezeAll;
    //    }
    //    else{
    //        rb.constraints = RigidbodyConstraints.None;
    //    }  
    //}

    public void reset()
    {
       // constraintsOn(true);
    }
}
