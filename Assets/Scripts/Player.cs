﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Security.Cryptography;



public class Player : MonoBehaviour
{
	public Rigidbody rbody;

	public Transform spot1;
	public Transform spot2;
	public Transform spot3;

    public Transform resetSpot;


	void Start ()
	{
		rbody = GetComponent<Rigidbody> ();
		resetSpot3 ();
	}


	void Update ()
	{
		if (GameManager.instance.game3 && GameManager.instance.startRunning) {

			Vector3 movement = spot3.position;
			movement.z += 0.2f;

			spot3.position = movement;
			transform.position = spot3.position;
		}
	}

	public void setPos (int spotNum)
	{
        rbody.constraints = RigidbodyConstraints.FreezeAll;
		
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

	public void removeConstraints ()
	{
		rbody.constraints = RigidbodyConstraints.FreezeRotationX |
		RigidbodyConstraints.FreezeRotationY |
		RigidbodyConstraints.FreezeRotationZ;
	}

	public void resetSpot3 ()
    {
        spot3.position = resetSpot.position;
	}
}
