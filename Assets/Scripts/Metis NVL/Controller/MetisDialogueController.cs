using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Threading;

public class MetisDialogueController : MetisViewComponentController {

	public MetisDFDialogueView TextBox;
	
	Action _done = () => {};
	
	List<string> _textQueue = new List<string>();
	string _name = "";
	public int NumberOfLines = 4;
	
	public float FadeTime = 0.5f;
	
	bool _blocking = false;

	void Update()
	{
		// TODO refactor input handling out into somewhere else
		if (!_blocking && (Input.GetMouseButtonDown(0) || Input.GetKeyDown (KeyCode.Return) 
		                   || Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl))
		    # if UNITY_WP8
		    || (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Ended)
		    #endif
		    )
		{
			if (TextBox.AllTextFinished)
			{
				if (_textQueue.Count > 0)
				{
					TextBox.ChangeDialogue(_name, _textQueue[0]);
					_textQueue.RemoveAt(0);
				}
				else
				{
					_done.Invoke();
				}
			}
			else
			{
				TextBox.ShowAllText();
			}
		}
		
	}

	[LuaMethod("say")]
	public void ShowDialogue(string dialogue)
	{
		// Parse out name
		string[] parsedDialogue = dialogue.Split(new char[]{':'});
		string name = string.Empty;
		string text = string.Empty;

		if (parsedDialogue.Length > 1)
		{
			name = parsedDialogue[0];
			text = parsedDialogue[1];
		}
		else
		{
			text = parsedDialogue[0];
		}

		// TODO support speaker names with _'s as spaces

		var waitToken = ScheduleWaitForever ();
		_done = () => waitToken.FinishWaiting ();
		_textQueue = SplitIntoTextBoxes(text);
		_name = name;
		
		TextBox.ChangeDialogue(name, _textQueue[0]);
		
		_textQueue.RemoveAt(0);
	}

	/*
	/// <summary>
	/// Runs a scene in a coroutine. Used because Lua libraries must be static classes and can't call coroutines.
	/// </summary>
	/// <param name="action">Action.</param>
	public void HandleScene(Action action)
	{

	}*/
	
	List<string> SplitIntoTextBoxes(string text)
	{
		List<string> output = new List<string>();
		List<string> lines = SplitIntoLines (text);
		int i = 0;
		while(i < lines.Count)
		{
			var box = "";
			for (int j = 0; j < NumberOfLines; j++)
			{
				if (i >= lines.Count)
					break;
				box += lines[i];
				i++;
			}
			output.Add (box);
		}
		return output;
	}
	
	List<string> SplitIntoLines(string text)
	{
		List<string> output = new List<string>();
		if (TextBox.MeasureDialogueWidth(text) < TextBox.Width)
		{
			output.Add (text);
			return output;
		}
		
		List<string> current = new List<string>();
		List<string> rest = new List<string>(text.Split ());
		// Add words one at a time and check the size of the dialogue label until it's too high
		while (rest.Count > 0)
		{
			string next = rest[0];
			var currentString = String.Join (" ", current.ToArray ());
			string nextString = currentString + " " + next;
			// TODO: This is a hack fix because the MeasureText method is returning wrong information
			if (TextBox.MeasureDialogueWidth(nextString) > TextBox.Width - 100)
			{
				output.Add (currentString + " <br><br/>");
				current.Clear ();
			}
			else
			{
				current.Add (next);
				rest.RemoveAt (0);
			}
		}
		output.Add (String.Join (" ", current.ToArray ()));
		return output;
	}

	[LuaMethod("wait")]
	public void Wait(double seconds)
	{
		ScheduleWait ((float)seconds);
	}
	
	public void FadeIn(float seconds)
	{
		TextBox.FadeIn (seconds);
	}
	
	public void FadeOut(float seconds)
	{
		TextBox.FadeOut (seconds);
	}

	[LuaMethodAttribute("clear_textbox")]
	public void Clear()
	{
		TextBox.Clear ();
	}


	[LuaMethod("show_textbox")]
	public override void Show()
	{
		FadeIn (FadeTime);
		ScheduleWait(FadeTime);
	}

	[LuaMethodAttribute("hide_textbox")]
	public override void Hide()
	{
		FadeOut (FadeTime);
		ScheduleWait(FadeTime);
	}

	public override void Activate()
	{
		_blocking = false;
	}
	
	public override void Deactivate()
	{
		_blocking = true;
	}


}
