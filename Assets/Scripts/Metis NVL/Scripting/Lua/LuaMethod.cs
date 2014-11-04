using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniLua;
using System.Reflection;

public class LuaMethod
{
	object _instance;
	MetisSceneController _scene;
	MethodInfo _method;

	public LuaMethod(object instance, MethodInfo method)
	{
		_instance = instance;
		_method = method;
		_scene = (MetisSceneController) GameObject.FindGameObjectWithTag (MetisSceneController.TAG).GetComponent<MetisSceneController>();
	}

	public int Invoke(ILuaState lua)
	{
		var methodParams = _method.GetParameters ();
		var parameters = new List<object>();
		for (int i = 0; i < methodParams.Length; i++)
		{
			var type = methodParams[i].ParameterType;
			if (type.Equals (typeof(string)))
			{
				parameters.Add (lua.L_CheckString (i+1));
			}
			else if (type.Equals (typeof(int)))
			{
				parameters.Add (lua.L_CheckInteger (i+1));
			}
			else if (type.Equals (typeof(double)))
			{
				parameters.Add (lua.L_CheckNumber (i+1));
			}
			else
			{
				throw new UnityException ("Unsupported Lua parameter type "+type.ToString ()+" in method "+_method.Name);
			}
		}

		IWaitListener waitListener = new LuaWaitListener();
		
		UnityThreadHelper.Dispatcher.Dispatch(() => 
		{
			_scene.RegisterWaitListener(waitListener);
			_method.Invoke (_instance, parameters.ToArray ());
			if (!_scene.Waiting)
			{
				waitListener.OnStopWaiting ();
			}
		});
		
		waitListener.OnStartWaiting();
		_scene.UnregisterWaitListener(waitListener);
		
		return 1;
	}
}
