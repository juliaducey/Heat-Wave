﻿using UnityEngine;
using System.Collections;

public class NewsScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ContinueGame()
    {
        Application.LoadLevel("MainScene");
    }
}
