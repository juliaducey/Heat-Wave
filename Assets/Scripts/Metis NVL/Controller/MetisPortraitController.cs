using UnityEngine;
using System.Collections;

public class MetisPortraitController : MetisViewComponentController 
{
	public MetisDFPortraitView PortraitView;

	public override void Show()
	{
		PortraitView.FadeIn();
		ScheduleWait (PortraitView.PictureTransitionTime);
	}
	
	public override void Hide()
	{
		PortraitView.FadeOut();
		ScheduleWait (PortraitView.PictureTransitionTime);
	}
	
	public override void Activate()
	{
		;
	}
	
	public override void Deactivate()
	{
		;
	}

	[LuaMethod("show_portrait")]
	public void ShowPortrait(string name, string tag)
	{
		var wait = PortraitView.ShowPicture (name, tag);
		ScheduleWait (wait);
	}

	[LuaMethod("exit_portrait")]
	public void ExitPortrait(string name)
	{
		PortraitView.ExitCharacter (name);
		ScheduleWait (PortraitView.PictureTransitionTime);
	}

	[LuaMethod("exit_all")]
	public void ExitAll()
	{
		PortraitView.ExitAll ();
		ScheduleWait (PortraitView.PictureTransitionTime);
	}
}
