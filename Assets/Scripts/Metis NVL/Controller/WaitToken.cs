using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// TODO: this class is not very safe as it is right now; make it safer by hiding more of the functionality, preferably inside SceneController somehow
public class WaitToken
{
	public float EndTime { get; private set; }
	Action _callback;
	bool _doneWaiting;
	bool _waitForever;
	
	public WaitToken(float seconds, Action callback)
	{
		EndTime = Time.time + seconds;
		_callback = callback;
	}
	
	public bool Done()
	{
		if (_doneWaiting)
			return true;
		
		if (!_waitForever)
			_doneWaiting = Time.time >= EndTime;
		
		return _doneWaiting;
	}

	public void StartWaitingForever()
	{
		_waitForever = true;
		EndTime = Time.time;
	}
	
	public void FinishWaiting()
	{
		_doneWaiting = true;
	}
	
	public void InvokeCallback()
	{
		if (_callback != null)
			_callback.Invoke ();
	}
}