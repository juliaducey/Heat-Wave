using UnityEngine;
using System.Collections;

public class Umbrella : MonoBehaviour {
    float health;
    Temperature temp;

	// Use this for initialization
	void Start () {
        this.health = 50f;
        temp = GameObject.Find("Temperature").GetComponent<Temperature>();
	}
	
	// Update is called once per frame
	void Update () {
        if (temp.curTemp > 100)
        {
            health -= Time.deltaTime;
        }

        if (health < 0)
        {
            GameState state = GameObject.Find("GameState(Clone)").GetComponent<GameState>();
            state.RemoveUmbrella();
            Destroy(gameObject);
        }

	}
}
