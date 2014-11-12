using UnityEngine;
using System.Collections;

public class PersonController : MetisViewComponentController {

	// TODO: All methods you want to expose to Lua go in this class.

	[LuaMethodAttribute("drinkWater")]
	public void DrinkWater(int id)
	{
		Person[] allPeople = FindObjectsOfType (typeof(Person)) as Person[];
		foreach (Person p in allPeople) 
		{
			if (p.id == id)
				p.drinkWater();
		}
		//TODO: actually call the person drinkWater method
	}

	[LuaMethodAttribute("test")]
	public void Test()
	{
		Debug.Log ("This does something.");
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
