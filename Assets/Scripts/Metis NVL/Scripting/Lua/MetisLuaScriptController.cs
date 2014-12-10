using System;
using System.Threading;
using System.Collections;
using UnityEngine;
using UniLua;

public class MetisLuaScriptController : MonoBehaviour
{
	ILuaState 	Lua;
	Thread		LuaThread;    

	public LuaTable Table;
	public MetisDialogueController DialogueController;
	public MetisBackgroundController BackgroundController;
	public MetisSceneController SceneController;
	bool DisableMainCamera = false;

	public string bullshit;

	// Name of BG to automatically show, if any
	public string AutoShowBG = string.Empty;
	void Start()
	{
		DialogueController = transform.parent.GetComponentInChildren<MetisDialogueController>();
		BackgroundController = transform.parent.GetComponentInChildren<MetisBackgroundController>();
		SceneController = transform.parent.GetComponentInChildren<MetisSceneController>();

		UnityThreadHelper.EnsureHelper ();
		InitializeLuaVM ();
	}

	public void PushOntoStack(int number)
	{
		Lua.PushInteger (number);
	}

	public void PushOntoStack(string str)
	{
		Lua.PushString (str);
	}

	void InitializeLuaVM()
	{
		if( Lua == null )
		{
			Lua = LuaAPI.NewState();
			Lua.L_OpenLibs();
			
			// Load core library
			//CoreLibrary.Initialize(DialogueController, BackgroundController, SceneController);
			Lua.L_RequireF( CoreLibrary.LIB_NAME
						   , CoreLibrary.OpenLib
						   , false
						   );
			
			// Load test library
			TestLibrary.SetTable(Table);
			Lua.L_RequireF( TestLibrary.LIB_NAME
						   , TestLibrary.OpenLib
						   , false
						   );

			// Load VSA library
			//var status = Lua.L_DoString("require \"lib.vsa.core\"");//L_DoString(this.scriptCode);
		}
	}

	string scriptCode;
	bool shouldAutoShowBG;

	public void ExecuteScript(TextAsset script, bool shouldAutoShowBG)
	{
		this.scriptCode = script.text;
		this.shouldAutoShowBG = shouldAutoShowBG;
		UnityThreadHelper.CreateThread (() => HandleExecuteScript());
		//LuaThread = new Thread (new ThreadStart (HandleExecuteScript));
		//LuaThread.Start ();
	}

    Camera _mainCamera;

	void HandleExecuteScript()
	{
		if (DisableMainCamera)
		{
			UnityThreadHelper.Dispatcher.Dispatch(() => {
				// Disable main camera
				_mainCamera = Camera.main;
				_mainCamera.enabled = false;
			});
		}
		
		// Import header
		//ExecuteDoString ("require \"lib.vsa.core\"");
		//ExecuteDoString ("require \"lib.metis_nvl.core\"");

		ExecuteDoString (bullshit);

		//ExecuteDoString("sound(\"event_start\")");
		
		if (!string.IsNullOrEmpty(AutoShowBG) && shouldAutoShowBG)
		{
			ExecuteDoString("nvl.show_bg(\"" + AutoShowBG + "\")");
		}
		
		// Show dialogue box
		//if (shouldAutoShowBG)
		{
			ExecuteDoString("nvl.show_textbox();");
		}
		
		// Execute script
		ExecuteDoString(this.scriptCode);
		
		ExecuteDoString("return 1;");
		Lua.Pop(1);
		
		// Hide dialogue box
		// TODO Move this into a cleanup script that runs at the end of all scripts
		Lua.L_DoString("exit_all();");
		Lua.L_DoString("nvl.hide_textbox(); nvl.hide_bg();");
		Lua.L_DoString("nvl.clear_textbox();");
		UnityThreadHelper.Dispatcher.Dispatch(() => {
			SceneController.RemoveTopView ();
			PersonController.PostScriptCall();

			if (DisableMainCamera)
				_mainCamera.enabled = true;
		});
		
		return;
	}

	private void ExecuteDoString(string str)
	{
		//Debug.Log (System.IO.Directory.GetCurrentDirectory());
		//Debug.Log(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
		var status = Lua.L_DoString (str);
		if (status != ThreadStatus.LUA_OK)
		{
			throw new Exception(Lua.ToString(-1));
		}
	}

	private int StoreMethod( string name )
	{
		Lua.GetField( -1, name );
		if( !Lua.IsFunction( -1 ) )
		{
			throw new Exception( string.Format(
				"method {0} not found!", name ) );
		}
		return Lua.L_Ref( LuaDef.LUA_REGISTRYINDEX );
	}

	private void CallMethod( int funcRef )
	{
		Lua.RawGetI( LuaDef.LUA_REGISTRYINDEX, funcRef );

		// insert `traceback' function
		var b = Lua.GetTop();
		Lua.PushCSharpFunction( Traceback );
		Lua.Insert(b);

		var status = Lua.PCall( 0, 0, b );
		if( status != ThreadStatus.LUA_OK )
		{
			Debug.LogError( Lua.ToString(-1) );
		}

		// remove `traceback' function
		Lua.Remove(b);
	}

	private static int Traceback(ILuaState lua) {
		var msg = lua.ToString(1);
		if(msg != null) {
			lua.L_Traceback(lua, msg, 1);
		}
		// is there an error object?
		else if(!lua.IsNoneOrNil(1)) {
			// try its `tostring' metamethod
			if(!lua.L_CallMeta(1, "__tostring")) {
				lua.PushString("(no error message)");
			}
		}
		return 1;
	}
}
