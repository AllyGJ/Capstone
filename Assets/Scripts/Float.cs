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
            if(count % 200 == 0) SoundManager.instance.playBird(SoundManager.instance.birdFlap, false);
			tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * 1f) * 0.08f;
			transform.position = tempPos;
		}
		
	}

	public void setPos (Vector3 pos)
	{
		tempPos = pos;
	}
}
