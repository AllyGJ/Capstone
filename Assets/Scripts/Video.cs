﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent (typeof (AudioSource))]

public class Video : MonoBehaviour
{
	public MovieTexture movie;

	// Use this for initialization
	void Start ()
	{
		GetComponent<Renderer> ().material.mainTexture = movie;
		movie.Play ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
