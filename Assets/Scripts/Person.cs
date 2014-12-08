using UnityEngine;
using System.Collections;

public class Person : MonoBehaviour {

	public int id;
	private bool male;
	private bool drunk;
	private bool old;
	private bool inConversation;
	private bool fainting; // Whether or not person is currently fainting
	private bool goingInside;
	private bool faintNotification;
	private bool checkNotification;
	private int timeOutsideInSeconds = 0; // use our global timer, rather than updating on every frame
	public float timeTillFaintInSeconds;
	private float timeTillGoInside;
	private float startTime;
	private int waterDrank = 0;
	public MetisScriptHandler scriptHandler;
    private float currenttime;
    private Temperature temperature;
    public GameState state;
	private int faintRotation = 1;
	// People burn for this long (seconds)
	private float fireTimeRemaining = 2.0f;
	// Flame property for prefab
	public Transform flame; 
	public Transform exclamation;
	public Transform checkmark;
	private Transform myFlame;
	private Transform myExclamation;
	private Transform myCheckmark;

	private float XMove = 0f;
	private float YMove = 0f;
	private float distanceWalked = 0f;
	private float currentXPosition; 
	private float distanceNeedToTravelToPause = 0f;
	private float timeStoppedMinutes = 0f;
	private float timePersonPausesInMinutes = 0f;
	

	// Use this for initialization
	void Start () {
        this.state = GameObject.Find("MainGameState").GetComponent<GameState>();
		this.male = Random.Range(0.0F, 1.0F) > .5;
		this.drunk = Random.Range(0.0F, 1.0F) > .75;
		this.old = Random.Range(0.0F, 1.0F) > .75;
        this.startTime = Time.time;
		// lose 10 seconds for every risk factor
		this.timeTillFaintInSeconds = 40 - 10 * (this.male.GetHashCode () + this.drunk.GetHashCode () + this.old.GetHashCode ()) + Random.Range (-10, 10);
        this.temperature = GameObject.Find("Temperature").GetComponent<Temperature>();

		this.distanceNeedToTravelToPause = Random.Range (10, 50);
		this.XMove = generateWalkingSpeed ();
		this.timePersonPausesInMinutes = Random.Range (10, 30);
		this.currentXPosition = this.transform.position.x;
		this.timeTillGoInside = Random.Range (0, 40);
		//TODO: programmatically attach scripts to people
		//MetisScriptHandler handler = gameObject.GetComponent<MetisScriptHandler> ();
		//handler.Script = "blah blah blah";
	}
	
	// Update is called once per frame
	void Update () {

		// game is paused
		if (Time.timeScale == 0) {
			return;
		}

        float tempMultiplier = 1f;

        if (temperature.curTemp > 100 && state.waters == 0)
            tempMultiplier = 1.5f;

        this.timeTillFaintInSeconds -= tempMultiplier * Time.deltaTime;
		this.timeTillGoInside -= Time.deltaTime;

		if ((this.timeTillGoInside <= 0) && !fainting && !goingInside && !inConversation) {
			this.goInside ();
		} else if ((this.timeTillFaintInSeconds <= 0) && !fainting && !goingInside && !inConversation) {
			this.faint ();
		} else if (goingInside) {
			if (!checkNotification)
			{
				myCheckmark = (Transform)Transform.Instantiate(checkmark, transform.position, Quaternion.identity);
				checkNotification = true;
			}
			myCheckmark.transform.position = new Vector3(transform.position.x - 2, transform.position.y + 4, transform.position.z);
			gameObject.transform.position = new Vector3 (gameObject.transform.position.x + 0.05f, 
			                                             gameObject.transform.position.y, 
			                                             gameObject.transform.position.z);
			if (gameObject.transform.position.x > 45) {
				if (myExclamation)
				{
					Destroy(myExclamation.gameObject);
				}
				if (myCheckmark) {
					Destroy (myCheckmark.gameObject);
				}
				Destroy(this.gameObject);
			}
		}

		//Should not hard code in edges of the scene. 
		//Don't move if fainting
		//Todo(Gebhard): Figure out how to get right and left edge of sprite. P3
		else if (!inConversation && !fainting) {
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
				if (xPosition > 33.5 || xPosition < -33.5) {
					XMove = -1 * XMove;
				}
				
				gameObject.transform.position = new Vector3 (xPosition + XMove, 
				                                             gameObject.transform.position.y + YMove, 
				                                             gameObject.transform.position.z);
			}

			// If close to fainting, start the wobble
			if (timeTillFaintInSeconds < 10)
			{
				if (!faintNotification)
				{
					myExclamation = (Transform)Transform.Instantiate(exclamation, transform.position, Quaternion.identity);
					faintNotification = true;
				}
				// myExclamation.transform.position = transform.position;
				myExclamation.transform.position = new Vector3(transform.position.x - 2, transform.position.y + 5, transform.position.z);
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
        if (state.busy == false && !goingInside && !fainting)
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
		this.goingInside = true;

		if (myExclamation) 
		{
			Destroy (myExclamation.gameObject);
		}
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
		// Destroy exclamation point
		Destroy (myExclamation.gameObject);
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
		if (timeTillFaintInSeconds < 15) 
		{
			attributes = "dying";
			//TODO: balance timings?
		}
		return attributes;
	}

}
