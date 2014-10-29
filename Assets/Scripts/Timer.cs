using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
    public float time;
    public bool state;
	// Use this for initialization
	void Start () {
        time = 0f;
        state = false;
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        if (state)
        {
            time += Time.deltaTime;
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
        return time;
    }

    public void SetTime(float t)
    {
        time = t;
    }
}
