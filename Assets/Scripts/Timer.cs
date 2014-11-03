using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
	public int day, hours, minutes;
	public bool state;
	float seconds;
	int speed;

	// Use this for initialization
	void Start () {
		hours   = 0;
		minutes = 0;
		seconds = 0;
		day     = 1;
		speed   = 100;
        state = true;
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        if (state)
        {
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
			}
        }
	
	}

    public void StartTimer()
    {
        state = true;
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
}
