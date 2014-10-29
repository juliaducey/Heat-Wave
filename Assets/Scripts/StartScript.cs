using UnityEngine;
using System.Collections;

public class StartScript : MonoBehaviour {
    public GameObject timer;
	// Use this for initialization
	void Start () {

	}

    void Awake()
    {
        if (!GameObject.Find("Timer(Clone)"))
            Instantiate(timer, Vector3.zero, Quaternion.identity);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
