using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {
    float health;
    GameState state;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
        this.health = 50f;
        this.state = GameObject.Find("MainGameState").GetComponent<GameState>();
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Application.loadedLevelName == "MainScene")
        {
            if (!GetComponent<SpriteRenderer>().enabled)
                GetComponent<SpriteRenderer>().enabled = true;
            health -= Time.deltaTime;

            if (health < 0)
            {
                state.RemoveWater();
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
