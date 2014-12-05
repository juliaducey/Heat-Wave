using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {
    float health;
    GameState state;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
        this.health = 50f;
        this.state = GameObject.Find("GameState(Clone)").GetComponent<GameState>();
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Application.loadedLevelName == "MainScene" && state.hot)
        {
            health -= Time.deltaTime;

            if (health < 0)
            {
                state.RemoveWater();
                Destroy(gameObject);
            }
        }

        if (Application.loadedLevelName == "StartScene")
            Destroy(gameObject);
	}
}
