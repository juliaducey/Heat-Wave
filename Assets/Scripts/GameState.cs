using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameState : MonoBehaviour {
	public int currentDay = 1;
	public Text text; 
	public int numberOfPeopleFainted = 0;
	public int numberOfPeopleInside = 0;
	public int score = 0;
    public float[] forecast;
	public Person person1;
	public Person person2;
	public Person person3;
	public Person person4;
    public Umbrella umbrella;
    public UmbrellaSprite umbrellaSprite;
    public Water water;
    public WaterSprite waterSprite;
    public int waters;
    public int umbrellas;
    public bool busy;
	public Person[] allPeople; 
	public Person talkingPerson;
    public bool hot;
    public bool bgm;
	private Timer timer;
	
	// Use this for initialization
	void Start () {
		forecast = new float[] { Random.Range(80.0f, 120.0f), Random.Range(80.0f, 120.0f), Random.Range(80.0f, 120.0f)};
		this.busy = false;
	}
	
	// Update is called once per frame
	void Update () {
		// Hacky way to not update score when gamestate is passed to the end screen
		// 3 Represents the end screen load level
		if (Application.loadedLevel != 3) {
			SetScore ();
			if (numberOfPeopleFainted > 15 ) {
				Application.LoadLevel("endScreen");
			}
		}

	}
	
    // tells the Timer function what day it is
	public void GetDay() {
        timer.SendMessage("SetDay", currentDay);
	}

	public void SetScore() {
		Timer timer = GameObject.Find("Timer").GetComponent<Timer>();
		score = (timer.day - 1) * 24 + timer.hours;
	}
	
	// updates the currentDay
	public void SetDay(int daynumber) {
        currentDay = daynumber;
	}

    public void IncrementDay()
    {
        currentDay += 1;
        forecast[0] = forecast[1] + Random.Range(-2f, 2f);
        forecast[1] = forecast[2] + Random.Range(-2f, 2f);
        forecast[2] = Random.Range(80.0f, 120.0f);
        Application.LoadLevel("NewspaperScene");
    }

    public void AddUmbrella()
    {
        this.umbrellas += 1;
        Instantiate(umbrella, new Vector3((this.umbrellas - 8) * 5.0F, 1.0F, (float)(1)), Quaternion.identity);
        UmbrellaSprite newUmbrellaSprite = (UmbrellaSprite)Instantiate(umbrellaSprite, new Vector3((this.umbrellas - 8) * 5.0F, 1.0F, (float)(1)), Quaternion.identity);
        newUmbrellaSprite.transform.localScale = new Vector3(1, 1, 0);
    }

    public void RemoveUmbrella()
    {
        this.umbrellas -= 1;
    }

    public void AddWater()
    {
        this.waters += 1;
        Instantiate(water, new Vector3((this.umbrellas - 7) * 5.0F, 1.5F, (float)(1)), Quaternion.identity);
        WaterSprite newWaterSprite = (WaterSprite)Instantiate(waterSprite, new Vector3((this.umbrellas - 7.5f) * 5.0F, 1.5F, (float)(1)), Quaternion.identity);
        newWaterSprite.transform.localScale = new Vector3(1, 1, 0);
    }

    public void RemoveWater()
    {
        this.waters -= 1;
    }

	public void TalkToPerson(Person p)
	{
		talkingPerson = p;
	}

	public void FinishTalking()
	{
		talkingPerson = null;
	}

	public Person GetTalkingPerson()
	{
		return talkingPerson;
	}

	// Populate scene with people; more people on later days
	public void PopulateScene()
	{
		Person[] people = new Person[] {person1, person2, person3, person4};
		// Kinda hacky but whatever
		timer = GameObject.Find ("Timer").GetComponent<Timer> ();
		int day = timer.day;
		
		int numPeople = 1 + (3 * day) + Random.Range(0, 3); 
		for (int i=1; i<=numPeople/2; i++)
		{
			Person person = people[Random.Range(0, 4)];
			Person newPerson = (Person) Instantiate(person, new Vector3(Random.Range(-45, 30), 10F, (float) (-i)), Quaternion.identity);
			// Fixed prefab so scaling here isn't necessary
			// newPerson.transform.localScale = new Vector3(8, 8, 0);
			newPerson.name = "Person " + i;
			newPerson.id = i;
		}
		
		// Create people on bottom row.  Note that one person doesn't actually get created because of bounds on loops
		for (int i=1; i<=numPeople/2; i++)
		{
			Person person = people[Random.Range(0, 4)];
			Person newPerson = (Person) Instantiate(person, new Vector3(Random.Range(-45, 30), -9F, (float) (-i)), Quaternion.identity);
			// Fixed prefab so scaling here isn't necessary
			// newPerson.transform.localScale = new Vector3(8, 8, 0);
			newPerson.name = "Person " + i;
			newPerson.id = i;
		}
	}
	
	public void SomeoneWentInside() {
		numberOfPeopleInside += 1;
	}
	
	public void SomeoneFainted() {
		numberOfPeopleFainted += 1;
	}

    public void LoadMainScene()
    {
        Application.LoadLevel("MainScene");
    }

	public void Pause()
	{
		Time.timeScale = Time.timeScale == 0 ? 1 : 0;
	}
}
