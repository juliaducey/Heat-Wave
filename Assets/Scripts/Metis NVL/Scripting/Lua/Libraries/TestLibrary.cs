using UniLua;
using UnityEngine;
using System;

public static class TestLibrary
{
	public const string LIB_NAME = "testlib"; // 库的名称, 可以是任意字符串

	static LuaTable table;

	public static int OpenLib(ILuaState lua) // 库的初始化函数
	{
		var define = new NameFuncPair[]
		{
			new NameFuncPair("set_variable", SetVariable),
			new NameFuncPair("get_variable", GetVariable),
			new NameFuncPair("add", Add),
			new NameFuncPair("sub", Sub),
		};
		
		lua.L_NewLib(define);
		return 1;
	}

	public static int SetVariable(ILuaState lua)
	{
		// Param 1: Key
		// Param 2: Value
		// Param 3: Type
		var key = lua.L_CheckString (1);
		object val = null;
		var type = lua.L_CheckString (3);

		LuaTable.EntryType variableType = LuaTable.EntryType.Flag;
		switch (type)
		{
			case "Integer":
				val = lua.L_CheckInteger(2);
				variableType = LuaTable.EntryType.Integer;
				break;
			case "Double":
				val = lua.L_CheckNumber(2);
				variableType = LuaTable.EntryType.Double;
				break;
			case "String":
				val = lua.L_CheckString(2);
				variableType = LuaTable.EntryType.String;
				break;
			case "Flag":
				val = lua.L_CheckInteger(2);
				variableType = LuaTable.EntryType.Flag;
				if ((int)val == 0)
				{
					val = false;
				}
				else
				{
					val = true;
				}
				break;
			default:
				throw new Exception("Unrecognized Lua table entry type");
		}
		table.SetEntry (new LuaTable.LuaTableEntry (){Key=key, Value=val, Type=variableType});
		return 1;
	}

	public static int GetVariable(ILuaState lua)
	{
		// Param 1: Key
		var key = lua.L_CheckString (1);
		LuaTable.LuaTableEntry entry = table.GetByKey (key);

		switch (entry.Type)
		{
		case LuaTable.EntryType.Integer:
			lua.PushInteger( (int)entry.Value );
			break;
		case LuaTable.EntryType.Double:
			lua.PushNumber( (double)entry.Value );
			break;
		case LuaTable.EntryType.String:
			lua.PushString( (string)entry.Value );
			break;
		case LuaTable.EntryType.Flag:
			lua.PushBoolean( (bool)entry.Value );
			break;
		default:
			throw new Exception("Unrecognized Lua table entry type");
		}
		return 1;
	}
	
	public static int Add(ILuaState lua)
	{
		var a = lua.L_CheckNumber( 1 ); // 第一个参数
		var b = lua.L_CheckNumber( 2 ); // 第二个参数
		var c = a + b; // 执行加法操作
		lua.PushNumber( c ); // 将返回值入栈
		return 1; // 有一个返回值
	}
	
	public static int Sub(ILuaState lua)
	{
		var a = lua.L_CheckNumber( 1 ); // 第一个参数
		var b = lua.L_CheckNumber( 2 ); // 第二个参数
		var c = a - b; // 执行减法操作
		lua.PushNumber( c ); // 将返回值入栈
		return 1; // 有一个返回值
	}

	public static void SetTable(LuaTable newTable)
	{
		table = newTable;
	}
}