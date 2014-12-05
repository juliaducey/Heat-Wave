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
        state =  GameObject.Find("GameState(Clone)").GetComponent<GameState>();
        state.DrawUpgrades();
        baseTemp = state.forecast[0];
        timerObject = GameObject.Find("Timer").GetComponent<Timer>();
        curTemp = -1f * System.Math.Abs(timerObject.hours * .5f - 6) + baseTemp - state.umbrellas;
        text.text = "Temperature: " + curTemp.ToString();
		
	}
	
	// Update is called once per frame
	void Update () {
		updateTemperature ();
	}
	
	public float getTemperature() {
		return curTemp;
	}
	
	public void updateTemperature() {
		timerObject = GameObject.Find ("Timer").GetComponent<Timer> ();
		if (timerObject.hours > currentHour) {
			currentHour = timerObject.hours;
            curTemp = -1f * System.Math.Abs(timerObject.hours * .5f - 6) + baseTemp - state.umbrellas;
            if (curTemp > 100f  && !state.hot)
                state.hot = true;
            else if (curTemp <= 100f && state.hot)
                state.hot = false;
			text.text = "Temperature: " + curTemp.ToString();
		}
	}
	
	public void updateNumberOfPeopleDead(int numberFainted) {
		numberOfPeopleDead += numberFainted;
	}
	
	public void updateNumberOfPeopleInside(int numberThatWentInside) {
		numberOfPeopleInside += numberThatWentInside;
	}
}
