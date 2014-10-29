using UnityEngine;
using System.Collections;

public class Person : MonoBehaviour {

	public bool male;
	public bool drunk;
	public bool old;
	public int timeOutside = 0; // use our global timer, rather than updating on every frame
	public int timeTillFaint; // possibly calculate from bool values
	//private float startTime;

	// Use this for initialization
	void Start () {
		this.male = Random.Range(0.0F, 1.0F) > .5;
		this.drunk = Random.Range(0.0F, 1.0F) > .75;
		this.old = Random.Range(0.0F, 1.0F) > .75;
		//this.startTime = Time.time;
		// one turn for each risk factor + a random int from 0 to 3
		this.timeTillFaint = 3 - (this.male.GetHashCode() + this.drunk.GetHashCode() + this.old.GetHashCode()) + (int)Random.Range (0, 3);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
