using UnityEngine;
using System.Collections;

public class Person : MonoBehaviour {

	public int id;
	public bool male;
	public bool drunk;
	public bool old;
	public bool inConversation;
	public bool fainting; // Whether or not person is currently fainting
	public int timeOutsideInSeconds = 0; // use our global timer, rather than updating on every frame
	public float timeTillFaintInSeconds;
	private float startTime;
	public int waterDrank = 0;
	public MetisScriptHandler scriptHandler;
    public float currenttime;
    public Temperature temperature;
    public GameState state;
	public int faintRotation = 1;
	// People burn for this long (seconds)
	private float fireTimeRemaining = 2.0f;
	// Flame property for prefab
	public Transform flame; 
	private Transform myFlame;

	public float XMove = 0f;
	public float YMove = 0f;
	public float distanceWalked = 0f;
	public float currentXPosition; 
	public float distanceNeedToTravelToPause = 0f;
	public float timeStoppedMinutes=0f;
	public float timePersonPausesInMinutes=0f;
	

	// Use this for initialization
	void Start () {
        this.state = GameObject.Find("GameState(Clone)").GetComponent<GameState>();
		this.male = Random.Range(0.0F, 1.0F) > .5;
		this.drunk = Random.Range(0.0F, 1.0F) > .75;
		this.old = Random.Range(0.0F, 1.0F) > .75;
        this.startTime = Time.time;
		// lose 10 seconds for every risk factor
		this.timeTillFaintInSeconds = 31 - 10 * (this.male.GetHashCode () + this.drunk.GetHashCode () + this.old.GetHashCode ()) + Random.Range (-10, 10);
        this.temperature = GameObject.Find("Temperature").GetComponent<Temperature>();

		this.distanceNeedToTravelToPause = Random.Range (10, 50);
		this.XMove = generateWalkingSpeed ();
		this.timePersonPausesInMinutes = Random.Range (10, 30);
		this.currentXPosition = this.transform.position.x;
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
		if (this.timeTillFaintInSeconds <= 0 && !fainting) {
			this.faint ();
		}

		//Should not hard code in edges of the scene. 
		//Don't move if fainting
		//Todo(Gebhard): Figure out how to get right and left edge of sprite. P3
		if (!inConversation && !fainting) {
			float xPosition = gameObject.transform.position.x;
			distanceWalked += currentXPosition - xPosition;
			currentXPosition = xPosition;
			if (Mathf.Abs(distanceWalked) > distanceNeedToTravelToPause) {
				timeStoppedMinutes +=  5 * Time.deltaTime;
				gameObject.transform.position = new Vector3 (xPosition,
				                                             gameObject.transform.position.y,
				                                             gameObject.transform.position.z);
				if (timeStoppedMinutes > timePersonPausesInMinutes){
					XMove = generateWalkingSpeed();
					distanceWalked = 0;
					timeStoppedMinutes = 0;
				}
			} else {
				// Todo(Gebhard): Figure out how to get right and left edge of sprite. 
				// If character is going to walk out of scene change direction
				if (xPosition > 30 || xPosition < -45) {
					XMove = -1 * XMove;
				}
				
				gameObject.transform.position = new Vector3 (xPosition + XMove, 
				                                             gameObject.transform.position.y + YMove, 
				                                             gameObject.transform.position.z);
			}
		} 
		else if (fainting) 
		{
			// Rotate till horizontal
			if (gameObject.transform.eulerAngles.z < 90)
			{
				gameObject.transform.Rotate(0, 0, faintRotation);
			} 
			else
			{
				if (fireTimeRemaining == 2.0)
				{
					myFlame = (Transform)Transform.Instantiate(flame, transform.position, Quaternion.identity);
				}
				if (fireTimeRemaining <= 0)
				{
					Destroy(myFlame.gameObject);
					Destroy (gameObject);
					// Destroy(gameObject);
				}

				fireTimeRemaining -= Time.deltaTime;
			}
		}

	}

	
	// Keeps generating a random number either forward or backward 
	// until in speed Range [-.1, -.03] or [.03, .1]
	float generateWalkingSpeed() {
		float speed = Random.Range (-.1f, .1f);
		while (Mathf.Abs(speed) < .03) {
			speed = Random.Range (-.1f, .1f);
		}
		return speed;
	}

	void OnMouseDown() {
		// bring up dialogue
        if (state.busy == false)
        {
            inConversation = true;
		    state.TalkToPerson (this);
		    scriptHandler.RunScript();
        }
	}

	public void drinkWater () {
		Debug.Log ("somebody drank water");
		this.timeTillFaintInSeconds += 20; // value can be balanced later
		this.waterDrank++;
	}

	public void setPersonToMoving() {
		inConversation = false;
	}

	
	public void goInside () {
		state.SomeoneWentInside ();
		Destroy (gameObject);
	}

	void rejectAdvice () {
		// dialogue
	}

	void faint() {
		// update global variable tracking number of fainted people
		// destroy game object (or have them fall over)
        this.state.SomeoneFainted();
        // Create animation
		fainting = true;
		// Destroy(gameObject);
	}

	public string GetAttributes()
	{
		string attributes = "";
		attributes += male ? "male" : "female";
		if (drunk)
		{
			attributes += ",drunk";
		}
		if (old)
		{
			attributes += ",old";
		}
		
		return attributes;
	}

}
