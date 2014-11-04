using UnityEngine;
using System.Collections;

public class MetisFadeController : MetisViewComponentController 
{
	public float FadeTime = 0.5f;
	public MetisDFFadeView FadeView;

	[LuaMethodAttribute("fade_out")]
	public override void Show()
	{
		FadeView.Show(FadeTime);
		ScheduleWait (FadeTime*2);
	}
	
	[LuaMethodAttribute("fade_in")]
	public override void Hide()
	{
		FadeView.Hide(FadeTime);
		ScheduleWait (FadeTime*2);
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
