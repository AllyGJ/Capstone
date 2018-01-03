using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Float : MonoBehaviour
{

	private Vector3 tempPos;

	public bool floating = false;

    private int count = 0;

	// Update is called once per frame
	void Update ()
	{
        count++;
        if (floating) {
            if (count % 100 == 0) SoundManager.instance.playBirdFlap();                                                                 
			tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * 1f) * 0.3f;
			transform.position = tempPos;
		}
        else {
            SoundManager.instance.stopBirdFlap();
        }
		
	}

	public void setPos (Vector3 pos)
	{
		tempPos = pos;
	}
}
