using UnityEngine;
using System.Collections;

public class BackgroundColorChange : MonoBehaviour {

	public SpriteRenderer Background;
	public Timer Timer;

	public Color DawnColor;
	public Color NightColor;

	private const float TOTAL_MINS = 60*16;
	
	// Update is called once per frame
	void Update () 
	{
		var time = Timer.GetTotalMinutes ();

		if (time < TOTAL_MINS/4)
		{
			Background.color = DawnColor*(1-time/TOTAL_MINS*1/4) + Color.white*(time/TOTAL_MINS*1/4);
		}
		else if (time > TOTAL_MINS*3/4)
		{
			time = time - TOTAL_MINS*3/4;
			Background.color = NightColor*(time-TOTAL_MINS*1/4) + Color.white*(1-time/TOTAL_MINS*1/4);
		}
	}
}
