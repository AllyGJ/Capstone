using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Float : MonoBehaviour
{

	private Vector3 tempPos;

	public bool floating = false;
    public float speed;
    public float amp;
    public bool birdSounds = false;

    private int count = 0;

    void Start()
    {
        tempPos = new Vector3(0, 0, 0);
    }

	// Update is called once per frame
	void Update ()
	{
        
        if (floating) {

            if (birdSounds)
            {
                if (count == 0 || count % 300 == 0)
                    SoundManager.instance.playBirdFlap();

                count++;
            }

			tempPos.y = Mathf.Sin (Time.fixedTime * Mathf.PI * speed) * amp;
            //print(tempPos);
            tempPos.y += 1f;
			this.transform.position = tempPos;
		}
        else {
            SoundManager.instance.stopBirdFlap();
            count = 0;
        }
		
	}

	public void setPos (Vector3 pos)
	{
		tempPos = pos;
        floating = true;
	}
}
