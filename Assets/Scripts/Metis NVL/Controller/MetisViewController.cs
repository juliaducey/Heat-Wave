using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MetisViewController : MonoBehaviour
{
	public string ViewName;
	MetisViewComponentController[] _viewComponents;

	public void Initialize(Func<float, Action, WaitToken> wait)
	{
		_viewComponents = gameObject.GetComponents<MetisViewComponentController>();
		foreach (var view in _viewComponents)
		{
			view.Initialize(wait);
		}
	}

	public void Show()
	{
		foreach (var view in _viewComponents)
		{
			view.Show ();
		}
	}

	public void Hide()
	{
		foreach (var view in _viewComponents)
		{
			view.Hide ();
		}
	}

	public void Activate()
	{
		foreach (var view in _viewComponents)
		{
			view.Activate ();
		}
	}

	public void Deactivate()
	{
		foreach (var view in _viewComponents)
		{
			view.Deactivate ();
		}
	}
}
