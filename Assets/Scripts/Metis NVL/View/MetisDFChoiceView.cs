using UnityEngine;
using System.Collections;
using System;

public class MetisDFChoiceView : dfViewComponent {

	string _text;
	int _choiceNumber;
	Action<int> _clickCallback;
	float clickTime;
	
	public void Initialize(string text, int choiceNumber, Action<int> clickCallback)
	{
		clickTime = Time.time + MetisChoiceController.FadeTime;
		dfGUIManager manager = null;
		foreach (var m in dfGUIManager.ActiveManagers)
		{
			manager = m;
		}
		_panel.GetComponentInChildren<dfButton>().Text = text;
		_choiceNumber = choiceNumber;
		_clickCallback = clickCallback;
		var binding = (dfEventBinding) _panel.gameObject.GetComponentInChildren<dfEventBinding>();
		binding.DataTarget.Component = this;
		binding.DataTarget.MemberName = "OnClick";
		binding.Bind ();
	}
	
	public void OnClick()
	{
		if (Time.time < clickTime)
			return;

		if (_clickCallback != null)
		{
			//gameObject.audio.Play ();
			_clickCallback.Invoke(_choiceNumber);
		}
	}

	public void FadeIn(float time)
	{
		_panel.FadeIn (time);
	}

	public void FadeOut(float time)
	{
		_panel.FadeOut (time);
	}
}
