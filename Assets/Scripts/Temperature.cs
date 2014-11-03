using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Temperature : MonoBehaviour {
	public float temperature;
	public Timer timerObject;
	public int currentDay = 1;
	public Text text; 
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
}
