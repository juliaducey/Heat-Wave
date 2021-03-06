﻿using UnityEngine;
using System.Collections;

public class Umbrella : MonoBehaviour {
    float health;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
        this.health = 50f;
	}
	
	// Update is called once per frame
    // If MainScene, deplete umbrella health.  If health reaches 0, destroy umbrella and tell GameState about it.
	void Update () {
        if (Application.loadedLevelName == "MainScene")
          
        {
            if (!GetComponent<SpriteRenderer>().enabled)
                GetComponent<SpriteRenderer>().enabled = true;
            health -= Time.deltaTime;

            if (health < 0)
            {
                GameState state = GameObject.Find("MainGameState").GetComponent<GameState>();
                state.RemoveUmbrella();
                Destroy(gameObject);
            }
        }
        else
        {
            if (GetComponent<SpriteRenderer>().enabled)
                GetComponent<SpriteRenderer>().enabled = false;
        }
        if (Application.loadedLevelName == "StartScene")
            Destroy(gameObject);

	}
}
