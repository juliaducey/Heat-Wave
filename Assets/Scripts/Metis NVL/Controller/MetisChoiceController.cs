using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;

public class MetisChoiceController : MetisViewComponentController 
{
	WaitToken _waitForClick;
	bool _active = false;
	int _selectedChoice = 0;

	public float FadeTime = 0.5f;
	List<MetisDFChoiceView> _choices = new List<MetisDFChoiceView>();

	protected override void OnAwake()
	{
		var choices = GameObject.FindObjectsOfType<MetisDFChoiceView>();
		foreach (var choice in choices)
		{
			_choices.Add ((MetisDFChoiceView) choice);
		}
		_choices = _choices.OrderBy(choice => choice.PanelName).ToList ();
	}

	void ChoiceClickCallback(int choiceNumber)
	{
		Hide ();
		_selectedChoice = choiceNumber;
		var scene = GameObject.FindGameObjectWithTag(MetisSceneController.TAG).GetComponent<MetisSceneController>();
		scene.RemoveTopView ();
		_waitForClick.FinishWaiting ();
	}

	// 	TODO having two ShowMenu methods is stupid combine them

	[LuaMethodAttribute("menu")]
	public void ShowMenuLua(int numChoices, string choice1, string choice2, string choice3, string choice4)
	{
		var scene = GameObject.FindGameObjectWithTag(MetisSceneController.TAG).GetComponent<MetisSceneController>();
		var viewName = this.GetComponent<MetisViewController>().ViewName;
		scene.ShowView (viewName);

		switch (numChoices)
		{
		case 1:
			ShowMenu(choice1);
			break;
		case 2:
			ShowMenu(choice1, choice2);
			break;
		case 3:
			ShowMenu(choice1, choice2, choice3);
			break;
		case 4:
			ShowMenu(choice1, choice2, choice3, choice4);
			break;
		default:
			throw new ArgumentException("Illegal number of choices: must be 1-4, but "+numChoices+" was given");
		}
	}

	[LuaMethodAttribute("menu_result")]
	public int GetMenuResult()
	{
		var controller = GameObject.FindGameObjectWithTag(MetisSceneController.TAG).GetComponent<MetisLuaScriptController>();
		controller.PushOntoStack(_selectedChoice);
		return 1;
	}

	public void ShowMenu(params string[] choices)
	{
		_selectedChoice = 0;
		_waitForClick = ScheduleWaitForever ();
		for (int i = 0; i < choices.Length; i++)
		{
			_choices[i].FadeIn (FadeTime);
			_choices[i].Initialize(choices[i], i+1, ChoiceClickCallback);
		}
	}

	public override void Show()
	{
		Activate ();
	}

	public override void Hide()
	{
		Deactivate ();
		foreach (var choice in _choices)
		{
			choice.FadeOut(FadeTime);
		}
	}

	public override void Activate()
	{
		_active = true;
	}

	public override void Deactivate()
	{
		_active = false;
	}

}
