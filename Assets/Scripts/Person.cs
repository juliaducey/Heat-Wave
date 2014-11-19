using UnityEngine;
using System.Collections;

public class Person : MonoBehaviour {

	public int id;
	public bool male;
	public bool drunk;
	public bool old;
	public int timeOutsideInSeconds = 0; // use our global timer, rather than updating on every frame
	public float timeTillFaintInSeconds;
	private float startTime;
	public int waterDrank = 0;
	public MetisScriptHandler scriptHandler;
    public float currenttime;
    public Temperature temperature;
	public float XMove = 0.08f;
	public float YMove = 0.0f;
	

	// Use this for initialization
	void Start () {
		this.male = Random.Range(0.0F, 1.0F) > .5;
		this.drunk = Random.Range(0.0F, 1.0F) > .75;
		this.old = Random.Range(0.0F, 1.0F) > .75;
        this.startTime = Time.time;
		// lose 10 seconds for every risk factor
		this.timeTillFaintInSeconds = 40 - 10 * (this.male.GetHashCode () + this.drunk.GetHashCode () + this.old.GetHashCode ()) + Random.Range (-10, 10);
        this.temperature = GameObject.Find("Temperature").GetComponent<Temperature>();

		//TODO: programmatically attach scripts to people
		//MetisScriptHandler handler = gameObject.GetComponent<MetisScriptHandler> ();
		//handler.Script = "blah blah blah";
	}
	
	// Update is called once per frame
	void Update () {
        float tempMultiplier = 1f;
        if (temperature.curTemp > 100)
            tempMultiplier = 1.5f;

        this.timeTillFaintInSeconds -= tempMultiplier * Time.deltaTime;
		if (this.timeTillFaintInSeconds < 0) {
			this.faint ();
		}

		//Should not hard code in edges of the scene. 
		//Todo(Gebhard): Figure out how to get right and left edge of sprite. P3
		float xPosition = gameObject.transform.position.x;
		if (xPosition > 30 || xPosition < -45) {
			XMove = -1 * XMove;
		}
		gameObject.transform.position = new Vector3 (xPosition + XMove, 
		                                             gameObject.transform.position.y + YMove, 
		                                             gameObject.transform.position.z);

	}

	void OnMouseDown() {
		// bring up dialogue
		scriptHandler.RunScript();
	}

	public void drinkWater () {
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
        GameObject gamestate = GameObject.Find("GameState(Clone)");
        gamestate.SendMessage("SomeoneFainted");
        Destroy(gameObject);
	}



}
