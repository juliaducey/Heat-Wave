using UniLua;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;

public static class CoreLibrary
{
	public const string LIB_NAME = "corelib";
	
	static LuaTable table;
	static List<NameFuncPair> methods = new List<NameFuncPair>();

	public static void AddMethod(NameFuncPair method)
	{
		methods.Add (method);
	}

	public static int OpenLib(ILuaState lua)
	{
		lua.L_NewLib(methods.ToArray ());
		return 1;
	}
}