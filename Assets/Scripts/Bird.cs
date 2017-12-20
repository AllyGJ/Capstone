using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{

	public Transform spot1;
	public Transform spot2;
	public Transform spot3;

	public float speed;
	public float amp;

    public bool pauseFlying = false;

	private Vector3 startSpot3;
	private Vector3 tempPos;

	private bool setPos3 = true;


	void Start ()
	{
		setPos (1);
		startSpot3 = spot3.position;
       // print("hi: "+startSpot3);
		tempPos = new Vector3 (0f, spot3.position.y, spot3.position.z);
		//tempPos = startSpot3;
	}

	void Update ()
	{
		if (GameManager.instance.game1) {
            setPos(1);
            transform.GetComponent<Float>().setPos(spot1.position);
            transform.GetComponent<Float>().floating = true;

		} else if (GameManager.instance.game2) {
			setPos3 = true;
			setPos (2);
            transform.GetComponent<Float>().setPos(spot2.position);
            transform.GetComponent<Float>().floating = true;

		} else if (GameManager.instance.game3) {
            transform.GetComponent<Float>().floating = false;
			if (setPos3) {
				transform.position = startSpot3;
                transform.rotation = new Quaternion(0, 0, 0, 0);
				setPos3 = false;
			}
				
			if (GameManager.instance.startRunning) {
                if (pauseFlying)
                {
                    tempPos.x = transform.position.x;
                }
                else
                {
                    tempPos.x = Mathf.Sin(Time.fixedTime * speed) * amp;
                }
                tempPos.y = spot3.position.y;
                tempPos.z = spot3.position.z;
                transform.position = tempPos;

				if (transform.position.z >= 750f) {
					GameManager.instance.endMiniGame (true);
				}
			}
			
        }else{
            transform.GetComponent<Float>().floating = false;
        }
		
	}

	public void setPos (int spotNum)
	{
		if (spotNum == 1) {
			transform.position = spot1.position;
            transform.rotation = spot1.rotation;
		} else if (spotNum == 2) {
			transform.position = spot2.position;
			transform.rotation = spot2.rotation;
		} else if (spotNum == 3) {
			transform.position = spot3.position;
            transform.rotation = spot3.rotation;
		}
	}
		

}
