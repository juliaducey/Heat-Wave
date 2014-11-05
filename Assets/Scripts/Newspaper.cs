using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Newspaper : MonoBehaviour {
    public Text text;
    public int fainted;
    public int inside;
    public int day;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        GameState gamestate = GameObject.Find("GameState(Clone)").GetComponent<GameState>();
        day = gamestate.currentDay - 1;
        fainted = gamestate.numberOfPeopleFainted;
        inside = gamestate.numberOfPeopleInside;
	}

	// Update is called once per frame
	void Update () {
        text.text = "DAY " + day + " OVER\n" +
            fainted + " FAINTED\n" +
            inside + " WENT INSIDE";
	}
}
