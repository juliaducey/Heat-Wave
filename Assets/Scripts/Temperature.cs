using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temperature : MonoBehaviour {
	public float baseTemp;
    public float curTemp;
	public Timer timerObject;
	public int currentHour = 0;
	public Text text; 
	public int numberOfPeopleDead = 0;
	public int numberOfPeopleInside = 0;
	
	// Use this for initialization
	void Start () {
		text = GetComponent <Text> ();
		baseTemp = Random.Range(80.0f, 120.0f);
        timerObject = GameObject.Find("Timer").GetComponent<Timer>();
        curTemp = -1f * System.Math.Abs(timerObject.hours * .5f - 6) + baseTemp;
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
            curTemp = -1f * System.Math.Abs(timerObject.hours * .5f - 6) + baseTemp;
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
