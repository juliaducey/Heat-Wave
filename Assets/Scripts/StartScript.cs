using UnityEngine;
using System.Collections;

public class StartScript : MonoBehaviour {
    public GameObject gamestate;
    public BGMusic bgm;
	// Use this for initialization
	void Start () {

	}

    void Awake()
    {
        if (!GameObject.Find("GameState(Clone)"))
            Instantiate(gamestate, Vector3.zero, Quaternion.identity);

		GameObject.Find("Instructions Text").SetActive(false);
		GameObject.Find("Back Button").SetActive(false);
        if (!GameObject.Find("BGM(Clone)"))
            Instantiate(bgm, Vector3.zero, Quaternion.identity);
    }
	// Update is called once per frame
	void Update () {
	
	}

    void StartGame()
    {
        Application.LoadLevel("MainScene");
    }
}
