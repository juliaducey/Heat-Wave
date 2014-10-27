using UnityEngine;
using System.Collections;

public class Person : MonoBehaviour {

	public bool male;
	public bool drunk;
	public bool old;
	public int timeOutside = 0; // use our global timer, rather than updating on every frame
	public int timeTillFaint; // possibly calculate from bool values
	private float startTime;

	// Use this for initialization
	void Start () {
		Random random = new Random ();
		this.male = random.NextDouble () > .5;
		this.drunk = random.NextDouble () > .75;
		this.old = random.NextDouble () > .75;
		this.startTime = Time.time;
		// one turn for each risk factor + a random int from 0 to 3
		this.timeTillFaint = Convert.ToInt32 (this.male) + Convert.ToInt32 (this.drunk) + Convert.ToInt32 (this.old) + random.Next (0, 3);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
