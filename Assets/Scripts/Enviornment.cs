using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enviornment : MonoBehaviour {
	public float temperature;
	public Timer timerObject;
	public int currentDay = 1;
	public Text text; 
	public int numberOfPeopleDead = 0;
	public int numberOfPeopleInside = 0;
	
	// Use this for initialization
	void Start () {
		text = GetComponent <Text> ();
		temperature = Random.Range(80.0f, 120.0f);
		text.text = "Temperature: " + temperature.ToString();
		
	}
	
	// Update is called once per frame
	void Update () {
		updateTemperature ();
	}
	
	public float getTemperature() {
		return temperature;
	}
	
	public void updateTemperature() {
		timerObject = GameObject.Find ("Timer").GetComponent<Timer> ();
		if (timerObject.day > currentDay) {
			currentDay = timerObject.day;
			temperature = Random.Range(80.0F, 120.0F);
			text.text = "Temperature: " + temperature.ToString();
		}
	}
	
	public void updateNumberOfPeopleDead(int numberFainted) {
		numberOfPeopleDead += numberFainted;
	}
	
	public void updateNumberOfPeopleInside(int numberThatWentInside) {
		numberOfPeopleInside += numberThatWentInside;
	}
}
