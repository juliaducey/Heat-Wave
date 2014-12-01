using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameState : MonoBehaviour {
	public int currentDay = 1;
	public Text text; 
	public int numberOfPeopleFainted = 0;
	public int numberOfPeopleInside = 0;
    public float[] forecast;
	public Person person;
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
	
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
        this.busy = false;
        forecast = new float[] { Random.Range(80.0f, 120.0f), Random.Range(80.0f, 120.0f), Random.Range(80.0f, 120.0f)};

		int numPeople = 5; //TODO: figure out specifics of random people
		for (int i=1; i<=numPeople; i++)
		{
			Person newPerson = (Person) Instantiate(person, new Vector3(Random.Range(-45, 30), 10F, (float) (-i)), Quaternion.identity);
			newPerson.transform.localScale = new Vector3(8, 8, 0);
			newPerson.name = "Person " + i;
			newPerson.id = i;
		}

		for (int i=1; i<=2; i++)
		{
			Person newPerson = (Person) Instantiate(person, new Vector3(Random.Range(-45, 30), -9f, (float) (-i)), Quaternion.identity);
			newPerson.transform.localScale = new Vector3(8, 8, 0);
			newPerson.name = "Person " + i;
			newPerson.id = i;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if (numberOfPeopleFainted > 15 ) {
			Application.LoadLevel("endScreen");
		}
	}
	
    // tells the Timer function what day it is
	public void GetDay() {
        Timer timer = GameObject.Find("Timer").GetComponent<Timer>();
        timer.SendMessage("SetDay", currentDay);
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
