using UnityEngine;
using System.Collections;

public class Restart : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void restart()
	{
        Destroy(GameObject.Find("MainGameState"));
		Application.LoadLevel ("StartScene");
		Time.timeScale = 1;
	}

}
