using UnityEngine;
using System.Collections;

public class MetisBackgroundController : MetisViewComponentController 
{
	public float BackgroundFadeTime = 5f;
	public MetisDFBackgroundView BackgroundView;

	public override void Show()
	{
		BackgroundView.FadeIn (BackgroundFadeTime);
		ScheduleWait (BackgroundFadeTime);
	}

	[LuaMethodAttribute("hide_bg")]
	public override void Hide()
	{
		BackgroundView.FadeOut (BackgroundFadeTime);
		ScheduleWait (BackgroundFadeTime);
	}

	[LuaMethodAttribute("show_bg")]
	public void ShowBackground(string alias)
	{
		var bg = Compendium.GetBG (alias);
		BackgroundView.ChangeBackground (bg);
		Show ();
	}

	public override void Activate()
	{
		;
	}

	public override void Deactivate()
	{
		;
	}
}
