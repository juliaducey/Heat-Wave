using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class dfViewComponent : MonoBehaviour, IViewComponent
{
	public string PanelName;
	protected dfPanel _panel;

	void Awake()
	{
		_panel = FindPanelWithName (PanelName);
		if (_panel == null)
		{
			throw new UnityException("Daikon Forge panel "+PanelName+" not found.");
		}

		OnAwake ();
	}

	protected dfPanel FindPanelWithName(string name)
	{
		var manager = GameObject.FindGameObjectWithTag ("UIRoot");
		var panels = manager.gameObject.GetComponentsInChildren<dfPanel>();
		foreach (var panel in panels)
		{
			if (panel.name == name)
			{
				return panel;
			}
		}
		return null;
	}

	protected virtual void OnAwake()
	{
		;
	}
}
