using UnityEngine;
using System.Collections;

public class MetisScriptHandler : MonoBehaviour 
{
	public TextAsset Script;
	public bool AutoStart = false;
	public bool AutoShowBG = false;

	private MetisLuaScriptController _scriptController;

	void Start()
	{
		_scriptController = (MetisLuaScriptController) GameObject.FindGameObjectWithTag(MetisSceneController.TAG).GetComponent<MetisLuaScriptController>();
	}

	// TODO: Putting this in Update instead of Start is a hack because other things need to initialize first in Start; not sure how to handle it yet
	void Update()
	{
		if (AutoStart)
		{
			AutoStart = false;
			RunScript();
		}
	}

	public void RunScript()
	{
		_scriptController.ExecuteScript(Script, AutoShowBG);
	}

}
