using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameState : MonoBehaviour {
	public int currentDay = 1;
	public Text text; 
	public int numberOfPeopleDead = 0;
	public int numberOfPeopleInside = 0;
	
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);

		
	}
	
	// Update is called once per frame
	void Update () {
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
        Application.LoadLevel("NewspaperScene");
    }


	
	public void updateNumberOfPeopleDead(int numberFainted) {
		numberOfPeopleDead += numberFainted;
	}
	
	public void updateNumberOfPeopleInside(int numberThatWentInside) {
		numberOfPeopleInside += numberThatWentInside;
	}

    public void LoadMainScene()
    {
        Application.LoadLevel("MainScene");
    }
}
