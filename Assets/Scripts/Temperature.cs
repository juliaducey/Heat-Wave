using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temperature : MonoBehaviour {
	public float baseTemp;
    public float trueTemp;
    public float curTemp;
    public GameState state;
	public Timer timerObject;
	public int currentHour = 0;
	public Text text; 
	public int numberOfPeopleDead = 0;
	public int numberOfPeopleInside = 0;
	
	// Use this for initialization
	void Start () {
		text = GetComponent <Text> ();
        state = GameObject.Find("MainGameState").GetComponent<GameState>();
        timerObject = GameObject.Find("Timer").GetComponent<Timer>();
        state.PopulateScene();

        baseTemp = state.forecast[(timerObject.day - 1) % 7];

        curTemp = -1f * System.Math.Abs(timerObject.hours * .5f - 6) + baseTemp - state.umbrellas;
        text.text = "Temperature: " + curTemp.ToString();
        updateTemperature();
		
	}
	
	// Update is called once per frame
	void Update () {
		updateTemperature ();
	}
	
	public float getTemperature() {
		return curTemp;
	}
	
	public void updateTemperature() {
        if (timerObject.hours > currentHour)
        {
            currentHour = timerObject.hours;
        }
        curTemp = baseTemp + 2 - (.05f * (currentHour - 16) * (currentHour - 16)) - state.umbrellas;
	    text.text = "Temperature: " + curTemp.ToString();

	}
	
	public void updateNumberOfPeopleDead(int numberFainted) {
		numberOfPeopleDead += numberFainted;
	}
	
	public void updateNumberOfPeopleInside(int numberThatWentInside) {
		numberOfPeopleInside += numberThatWentInside;
	}
}
