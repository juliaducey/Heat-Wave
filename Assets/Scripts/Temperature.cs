using UnityEngine;
using System.Collections;

public class Temperature : MonoBehaviour {
	public float temperature = 85.4f;
	public Timer timerObject;
	public int currentDay = 1;
	// Use this for initialization
	void Start () {
	
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
			Debug.Log(temperature);
		}
	}
}
