using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreUpdate : MonoBehaviour {
	public Text score_text;
	public GameState gamestate;
	// Use this for initialization
	void Start () {
		score_text = GetComponent<Text>();
		gamestate = GameObject.Find("MainGameState").GetComponent<GameState>();

		if (gamestate.score == 1) {
			score_text.text = "Score: " + gamestate.score + " hour";
		} else {
			score_text.text = "Score: " + gamestate.score + " hours";	
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
