using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UniLua;

public abstract class MetisViewComponentController : MonoBehaviour
{
	Func<float, Action, WaitToken> _wait;
	protected Compendium Compendium;

	void Awake()
	{
		foreach (var method in this.GetType ().GetMethods ())
		{
			foreach (var attribute in method.GetCustomAttributes (true))
			{
				if (attribute is LuaMethodAttribute)
				{
					var luaMethod = new LuaMethod(this, method);
					var nameFunc = new NameFuncPair(((LuaMethodAttribute) attribute).LuaMethodName, luaMethod.Invoke);
					CoreLibrary.AddMethod (nameFunc);
				}
			}
		}
		OnAwake();
	}


	protected virtual void OnAwake()
	{
		;
	}

	public void Initialize(Func<float, Action, WaitToken> wait)
	{
		_wait = wait;
		Compendium = (Compendium) GameObject.FindGameObjectWithTag (Compendium.TAG).GetComponentInChildren<Compendium>();
	}

	protected void ScheduleWait(float seconds)
	{
		_wait.Invoke (seconds, null);
	}

	protected void ScheduleWait(float seconds, Action callback)
	{
		_wait.Invoke (seconds, callback);
	}

	protected WaitToken ScheduleWaitForever()
	{
		var token = _wait.Invoke (0, null);
		token.StartWaitingForever ();
		return token;
	}

	public abstract void Show();
	public abstract void Hide();
	public abstract void Activate();
	public abstract void Deactivate();
}
