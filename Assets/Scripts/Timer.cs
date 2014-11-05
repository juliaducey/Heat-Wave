using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour {
	public int day, hours, minutes;
	public bool state;
	float seconds;
	int speed;
	Text text;


	// Use this for initialization
	void Start () {
		hours   = 0;
		minutes = 0;
		seconds = 0;
		day     = 1;
		speed   = 100;
        state = true;
		text = GetComponent <Text> ();
		updateTimerUI ();
	}

	// Update is called once per frame
	void Update () {
        if (state)
        {
            if (seconds == 0 && minutes == 0 && hours == 0)
            {
                day = GameObject.Find("GameState(Clone)").GetComponent<GameState>().currentDay;
            }
			seconds += Time.deltaTime * speed;
			
			if (seconds >= 1) {
				minutes += 1;
				seconds = 0;
			}
			if (minutes >= 60) {
				hours += 1;
				minutes = 0;
			}
			
			if (hours >= 24) {
				hours = 0;
                day += 1;
                GameObject gamestate = GameObject.Find("GameState(Clone)");
                gamestate.SendMessage("IncrementDay");
			}
        }
		updateTimerUI ();
	
	}

    public void StartTimer()
    {
        state = true;
    }

	public void updateTimerUI(){
		if (minutes < 10) {
			text.text = "Day " + day + "  Time: " + hours + ":0" + minutes;
		} else {
			text.text = "Day " + day + "  Time: " + hours + ":" + minutes;
		}
	}

    public void StopTimer()
    {
        state = false;
    }

    public float GetTime()
    {
        return seconds;
    }

    public void SetTime(float t)
    {
        seconds = t;
    }

    public void SetDay(int daynumber)
    {
        day = daynumber;
    }
}
