using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConstructionButton : MonoBehaviour {
    GameState state;
    public bool building = false;
    public float timeToBuild = 5f;
    public string item;
    GameObject working;

	public Text BuildingText;

	// Use this for initialization
	void Start () {
        this.state = GameObject.Find("MainGameState").GetComponent<GameState>();
		BuildingText.SetAlpha (0f);
	}
	
	// Update is called once per frame
	void Update () {
        if (building)
            this.timeToBuild -= Time.deltaTime;

        if (this.timeToBuild <= 0)
        {
            state.busy = false;
            building = false;
            if (item.Equals("umbrella"))
                state.AddUmbrella();
            else if (item.Equals("water"))
                state.AddWater();
            this.timeToBuild = 5;
            // working.gameObject.SetActive(false);
        }

	}

    void Awake()
    {
        //working = GameObject.Find("Working");
        //working.gameObject.SetActive(false);
    }

    public void Build()
    {
        if (state.busy == false)
        {
            if ((item.Equals("umbrella") && state.umbrellas != 5) || (item.Equals("water") && state.waters != 5))
            {
                state.busy = true;
                building = true;

				var t = timeToBuild/8;

				BuildingText.FadeIn(t, () => BuildingText.FadeOut (t, () => BuildingText.FadeIn 
				                   (t, () => BuildingText.FadeOut (t, () => BuildingText.FadeIn 
				                   (t, () => BuildingText.FadeOut(t, () => BuildingText.FadeIn (t, () => BuildingText.FadeOut (t))))))));
            }

        }

        //working.gameObject.SetActive(true);
    }
}
