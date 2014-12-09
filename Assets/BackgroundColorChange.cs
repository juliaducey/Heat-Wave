using UnityEngine;
using System.Collections;

public class BackgroundColorChange : MonoBehaviour {

	public SpriteRenderer Background;
	Timer Timer;

	public Color DawnColor;
	public Color NightColor;

	private const float TOTAL_MINS = 60*16f;

	void Start()
	{
		Timer = (Timer) GameObject.FindObjectOfType<Timer>();
	}

	// Update is called once per frame
	void Update () 
	{
		var time = Timer.GetTotalMinutes ();

		if (time < TOTAL_MINS/4)
		{
			Background.color = DawnColor*(1-time/(TOTAL_MINS/4)) + Color.white*(time/(TOTAL_MINS/4));
		}
		else if (time > TOTAL_MINS*3/4)
		{
			time -= TOTAL_MINS*3/4;
			Background.color = NightColor*(time/(TOTAL_MINS/4)) + Color.white*(1-time/(TOTAL_MINS/4));
		}
	}
}
