using UnityEngine;
using System.Collections;
using System;

[AttributeUsage(AttributeTargets.Method)]
public class LuaMethodAttribute : System.Attribute 
{
	public readonly string LuaMethodName;

	public LuaMethodAttribute(string methodName)
	{
		LuaMethodName = methodName;
	}
}
