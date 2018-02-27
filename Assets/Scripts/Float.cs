using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Float : MonoBehaviour
{

	private Vector3 tempPos;

	public bool floating = false;
    public float speed;
    public float amp;

    private int count = 0;

    void Start()
    {
        tempPos = new Vector3(0, 0, 0);
    }

	// Update is called once per frame
	void Update ()
	{
        count++;
        if (floating) {
           if (count % 300 == 0)
                SoundManager.instance.playBirdFlap();                                                                 
			tempPos.y = Mathf.Sin (Time.fixedTime * Mathf.PI * speed) * amp;
            //print(tempPos);
            tempPos.y += 1f;
			this.transform.position = tempPos;
		}
        else {
            SoundManager.instance.stopBirdFlap();
        }
		
	}

	public void setPos (Vector3 pos)
	{
		tempPos = pos;
        floating = true;
	}
}
