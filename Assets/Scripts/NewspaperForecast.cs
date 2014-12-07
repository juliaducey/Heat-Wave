using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewspaperForecast : MonoBehaviour {
    public Text text;
    public int fainted;
    public int inside;
    public int day;
    public GameState gamestate;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        gamestate = GameObject.Find("MainGameState").GetComponent<GameState>();
        day = gamestate.currentDay - 1;
        fainted = gamestate.numberOfPeopleFainted;
        inside = gamestate.numberOfPeopleInside;
//		text.text = "DAY " + day + " OVER\n" +
//			fainted + " FAINTED\n" +
//				inside + " WENT INSIDE\n" +
//				"3 DAY FORECAST\n" + (int)gamestate.forecast[0] + " " + (int)gamestate.forecast[1] + " " + (int)gamestate.forecast[2];
		text.text = (int)gamestate.forecast[0] + "   " + (int)gamestate.forecast[1] + "   " + (int)gamestate.forecast[2];

	}

	// Update is called once per frame
	void Update () {

	}
}
