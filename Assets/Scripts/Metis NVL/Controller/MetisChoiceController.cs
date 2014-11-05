using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;

public class MetisChoiceController : MetisViewComponentController 
{
	Action<int> _clickCallback;
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
		_clickCallback.Invoke(choiceNumber);
		_clickCallback = num => {};
		_waitForClick.FinishWaiting ();
	}

	[LuaMethodAttribute("menu1")]
	public int ShowMenuLua(string choice1)
	{
		return ShowMenuLua(1, choice1, null, null, null);
	}

	[LuaMethodAttribute("menu2")]
	public int ShowMenuLua(string choice1, string choice2)
	{
		return ShowMenuLua(2, choice1, choice2, null, null);
	}

	[LuaMethodAttribute("menu3")]
	public int ShowMenuLua(string choice1, string choice2, string choice3)
	{
		return ShowMenuLua(3, choice1, choice2, choice3, null);
	}
	
	[LuaMethodAttribute("menu4")]
	public int ShowMenuLua(string choice1, string choice2, string choice3, string choice4)
	{
		return ShowMenuLua(4, choice1, choice2, choice3, choice4);
	}

	// TODO having so many wrapper methods is weird refactor this
	
	public int ShowMenuLua(int numChoices, string choice1, string choice2, string choice3, string choice4)
	{
		_selectedChoice = 0;
		var scene = GameObject.FindGameObjectWithTag(MetisSceneController.TAG).GetComponent<MetisSceneController>();
		var viewName = this.GetComponent<MetisViewController>().ViewName;
		scene.ShowView (viewName);

		var _lock = new AutoResetEvent(false);

		Action<int> callback = delegate(int selected) 
		{
			_selectedChoice = selected;
			_lock.Set ();
		};

		switch (numChoices)
		{
		case 1:
			UnityThreadHelper.Dispatcher.Dispatch (() => ShowMenu(callback, choice1));
			break;
		case 2:
			UnityThreadHelper.Dispatcher.Dispatch (() => ShowMenu(callback, choice1, choice2));
			break;
		case 3:
			UnityThreadHelper.Dispatcher.Dispatch (() => ShowMenu(callback, choice1, choice2, choice3));
			break;
		case 4:
			UnityThreadHelper.Dispatcher.Dispatch (() => ShowMenu(callback, choice1, choice2, choice3, choice4));
			break;
		default:
			throw new ArgumentException("Illegal number of choices: must be 1-4, but "+numChoices+" was given");
		}

		_lock.WaitOne ();

		scene.RemoveTopView ();

		return _selectedChoice;
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
