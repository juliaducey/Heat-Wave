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
