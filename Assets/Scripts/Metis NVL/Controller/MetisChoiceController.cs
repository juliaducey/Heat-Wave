using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class MetisChoiceController : MetisViewComponentController 
{
	Action<int> _clickCallback;
	WaitToken _waitForClick;
	bool _active = false;

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
		_clickCallback.Invoke(choiceNumber);
		_clickCallback = num => {};
		_waitForClick.FinishWaiting ();
	}

	[LuaMethodAttribute("menu")]
	public int ShowMenuLua(int numChoices, string choice1, string choice2, string choice3, string choice4)
	{
		var wait = ScheduleWaitForever();
		var scene = GameObject.FindGameObjectWithTag(MetisSceneController.TAG).GetComponent<MetisSceneController>();
		var viewName = this.GetComponent<MetisViewController>().ViewName;
		scene.ShowView (viewName);

		// TODO thread this so the method doesn't return until a choice has been selected

		Action<int> callback = delegate(int selectedChoice) 
		{ 
			wait.FinishWaiting (); 
			scene.RemoveTopView ();
		};

		switch (numChoices)
		{
		case 1:
			ShowMenu(callback, choice1);
			break;
		case 2:
			ShowMenu(callback, choice1, choice2);
			break;
		case 3:
			ShowMenu(callback, choice1, choice2, choice3);
			break;
		case 4:
			ShowMenu(callback, choice1, choice2, choice3, choice4);
			break;
		default:
			throw new ArgumentException("Illegal number of choices: must be 1-4, but "+numChoices+" was given");
		}

		// TODO this is wrong
		return 1;
	}

	public void ShowMenu(Action<int> callback, params string[] choices)
	{
		_waitForClick = ScheduleWaitForever ();
		_clickCallback = callback;
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
