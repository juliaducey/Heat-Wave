using UnityEngine;
using System.Collections;

public class BirdSound : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!audio.isPlaying)
        {
            if (Random.Range(0f, 1f) < 0.001f)
                audio.Play();
        }

	}
}
