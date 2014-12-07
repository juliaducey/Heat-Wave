using UnityEngine;
using System.Collections;

public class BGMusic : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake() {
        GameState state = GameObject.Find("MainGameState").GetComponent<GameState>();
        if (state.bgm)
        {
            Destroy(gameObject);
        }
        else
        {
            state.bgm = true;
        }
    }
}
