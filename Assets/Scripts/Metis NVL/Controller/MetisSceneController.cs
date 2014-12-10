using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UniLua;

public class MetisSceneController : MonoBehaviour 
{
	AudioSource _bgmSource;
	AudioSource _sfxSource;
	Dictionary<string, MetisViewController> _views = new Dictionary<string, MetisViewController>();
	List<MetisViewController> _activeViews = new List<MetisViewController>();
	MetisViewController _currentView;
	List<IWaitListener> _listeners = new List<IWaitListener>();

	public static readonly string TAG = "SceneController";

	public string bullshit;

	bool _waiting;
	public bool Waiting 
	{ 
		get { return _waiting; }
		private set
		{
			if (value == false)
			{
                lock (_listeners)
                {
					var listeners = _listeners.ToArray ();
                    foreach (var listener in listeners)
                    {
                        listener.OnStopWaiting();
                    }
                }
			}
			_waiting = value;
		}
	}

	List<WaitToken> _waitTokens = new List<WaitToken>();

	void Awake()
	{
		Action method = () => ShowView("Dialogue");
		var luaMethod = new LuaMethod(this, method.Method);
		var nameFuncPair = new NameFuncPair("show_view", luaMethod.Invoke);
		CoreLibrary.AddMethod (nameFuncPair);

		_bgmSource = gameObject.AddComponent<AudioSource>();
		_bgmSource.playOnAwake = true;
		_bgmSource.loop = true;
		
		_sfxSource = gameObject.AddComponent<AudioSource>();
		_sfxSource.playOnAwake = true;
		_sfxSource.loop = false;

		var views = GameObject.FindObjectsOfType<MetisViewController>();
		foreach (var v in views)
		{
			var view = (MetisViewController) v;
			_views.Add(view.ViewName, view);
			view.Initialize (Wait);
		}

		var script = gameObject.AddComponent<MetisLuaScriptController>();
		script.bullshit = bullshit;
	}

	void Update()
	{
		if (_waitTokens.Count == 0)
			return;

		var shortestWait = _waitTokens[0];
		if (shortestWait.Done ())
		{
			shortestWait.InvokeCallback ();
			_waitTokens.RemoveAt (0);
			if (_waitTokens.Count == 0)
			{
				Waiting = false;
			}
		}
	}

	/// <summary>
	/// Registers an object to listen for wait status changes.
	/// </summary>
	/// <param name="listener"></param>
	public void RegisterWaitListener(IWaitListener listener)
	{
		_listeners.Add(listener);
	}
	
	/// <summary>
	/// Unregisters an object to stop listening for wait status changes.
	/// </summary>
	/// <param name="listener"></param>
	public void UnregisterWaitListener(IWaitListener listener)
	{
		_listeners.Remove(listener);
	}
	
	WaitToken Wait(float seconds, Action callback)
	{
		var token = new WaitToken(seconds, callback);
		_waitTokens.Add (token);
		_waitTokens = _waitTokens.OrderBy (waitToken => waitToken.EndTime).ToList ();
		Waiting = true;

		return token;
	}
	
	public void ShowView(string viewName)
	{
		// TODO: messy; refactor
		if (_currentView != null)
			_currentView.Deactivate();
		_currentView = _views[viewName];
		_activeViews.Remove (_currentView); // make sure it doesn't get put in twice
		_activeViews.Add(_currentView);
		
		_currentView.Show();
	}

	public void RemoveTopView()
	{
		// TODO: messy; refactor
		if (_currentView == null)
			return;

		_currentView.Hide();
		_activeViews.Remove(_currentView);
		if (_activeViews.Count > 0)
		{
			_currentView = _activeViews[_activeViews.Count - 1];
			_currentView.Activate();

		}
		else
		{
			_currentView = null;
		}
	}
}


