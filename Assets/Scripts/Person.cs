using UnityEngine;
using System.Collections;

public class Person : MonoBehaviour {

	public bool male;
	public bool drunk;
	public bool old;
	public int timeOutsideInSeconds = 0; // use our global timer, rather than updating on every frame
	public int timeTillFaintInSeconds;
	private float startTime;
	public int waterDrank = 0;

	// Use this for initialization
	void Start () {
		this.male = Random.Range(0.0F, 1.0F) > .5;
		this.drunk = Random.Range(0.0F, 1.0F) > .75;
		this.old = Random.Range(0.0F, 1.0F) > .75;
		this.startTime = Time.time;
		// lose 10 seconds for every risk factor
		this.timeTillFaintInSeconds = 60 - 10 * (this.male.GetHashCode () + this.drunk.GetHashCode () + this.old.GetHashCode ()) + Random.Range (-10, 10);
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - this.startTime > this.timeTillFaintInSeconds) {
			this.faint ();
		}



	}

	void OnMouseDown() {
		// bring up dialogue
	}

	void drinkWater () {
		this.timeTillFaintInSeconds += 20; // value can be balanced later
		this.waterDrank++;
	}
	
	void goInside () {
		// remove from screen
		// add to global environment variable tracking the number of people who successfully went inside
	}

	void rejectAdvice () {
		// dialogue
	}

	void faint() {
		// update global variable tracking number of fainted people
		// destroy game object (or have them fall over)
	}



}
