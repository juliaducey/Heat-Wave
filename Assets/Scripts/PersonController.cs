using UnityEngine;
using System.Collections;

public class PersonController : MetisViewComponentController {

	// TODO: All methods you want to expose to Lua go in this class.
	[LuaMethodAttribute("fainted")]
	public int Fainted()
	{
		Person talkingPerson = GameObject.Find ("MainGameState").GetComponent<GameState> ().GetTalkingPerson();
		var controller = GameObject.FindGameObjectWithTag(MetisSceneController.TAG).GetComponent<MetisLuaScriptController>();
		controller.PushOntoStack(talkingPerson.timeTillFaintInSeconds<0 ? 1 : 0);
		return 1;
	}

	[LuaMethodAttribute("attributes")]
	public int Attributes()
	{
		Person talkingPerson = GameObject.Find ("MainGameState").GetComponent<GameState> ().GetTalkingPerson();
		var controller = GameObject.FindGameObjectWithTag(MetisSceneController.TAG).GetComponent<MetisLuaScriptController>();
		controller.PushOntoStack(talkingPerson.GetAttributes());
		Debug.Log ("returned " + talkingPerson.GetAttributes ());
		return 1;
	}

	[LuaMethodAttribute("drink_water")]
	public void DrinkWater()
	{
		Person talkingPerson = GameObject.Find ("MainGameState").GetComponent<GameState> ().GetTalkingPerson();
		talkingPerson.drinkWater();
	}

	[LuaMethodAttribute("send_home")]
	public void SendHome()
	{
		Person talkingPerson = GameObject.Find ("MainGameState").GetComponent<GameState> ().GetTalkingPerson();
		talkingPerson.goInside();
	}

	[LuaMethodAttribute("test")]
	public void Test()
	{
		Debug.Log ("This does something.");
	}

	public static void PostScriptCall()
	{
		// TODO: Do something here. This is called after every script finishes.
		GameObject.Find ("MainGameState").GetComponent<GameState> ().FinishTalking();
	}

	// Don't worry about any of this; this is just because I'm using a hacky way to provide exposure to the scripting. -Julia
	#region hack
	public override void Show()
	{
	}

	public override void Hide()
	{
	}

	public override void Activate()
	{
	}

	public override void Deactivate()
	{
	}
	#endregion
}
