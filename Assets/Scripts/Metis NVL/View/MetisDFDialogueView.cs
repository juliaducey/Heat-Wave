using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class MetisDFDialogueView : dfViewComponent {

	string _name = "";
	string _dialogue = "";
	string _dialogueSuffix = "";
	float _nextTime = 0f;
	int _currentDialogueIndex = 0;
	List<TagInfo> _tags = new List<TagInfo>();
	Dictionary<string, string> _matchingTags = new Dictionary<string, string>();
	const int DISPLAY_ALL_TEXT = -1;
	
	public float SpeakerNameTransitionTime = 0.1f;
	public int SpeakerNameMoveDelta = 20;
	
	dfRichTextLabel _dialogueBox;
	dfLabel _speakerBox;
	dfPanel _dialoguePanel;
	dfPanel _speakerPanel;
	
	Vector2 _speakerPanelLeft;
	Vector2 _speakerPanelRight;
	Vector3 _dialoguePosition;
	
	TweenStatus<float> _speakerFadeStatus = new TweenStatus<float>();
	TweenStatus<Vector2> _speakerMoveStatus = new TweenStatus<Vector2>();
	
	private const float TextSpeed = 0.01f;
	
	string _visibleDialogue = "";
	string VisibleDialogue
	{
		get { return _visibleDialogue; }
		set
		{
			_visibleDialogue = value;
			_dialogueBox.Text = value;
		}
	}
	
	private struct TagInfo
	{
		public int StartPosition;
		public int EndPosition;
		public string OpeningTag;
		public string ClosingTag;
	}
	
	protected override void OnAwake()
	{
		_matchingTags.Add ("i", "</i>");
		_matchingTags.Add ("b", "</b>");
		_matchingTags.Add ("color", "</color>");
		_matchingTags.Add ("size", "</size>");
		_matchingTags.Add ("material", "</material>");
		_matchingTags.Add ("br", "<br/>");

		var panels = _panel.gameObject.GetComponentsInChildren<dfPanel>();
		foreach (var panel in panels)
		{
			if (panel.name.Contains ("Dialogue Panel"))
			{
				_dialoguePanel = panel;
			}
			else if (panel.name.Contains ("Speaker Panel"))
			{
				_speakerPanel = panel;
			}
		}
		_dialogueBox = _dialoguePanel.gameObject.GetComponentInChildren<dfRichTextLabel>();
        _speakerBox = _speakerPanel.gameObject.GetComponentInChildren<dfLabel>();
		_speakerPanelRight = new Vector2(_speakerPanel.Position.x, _speakerPanel.Position.y);
		_speakerPanelLeft = new Vector2(_speakerPanelRight.x - SpeakerNameMoveDelta, _speakerPanelRight.y);
        
		_dialoguePosition = new Vector2(_dialogueBox.Position.x, _dialogueBox.Position.y);
	}
	
	public int Height
	{
		get { return (int) _dialogueBox.Height; }
	}
	
	public int Width
	{
		get { return  (int) _dialogueBox.Width; }
	}
	
	public bool AllTextFinished
	{
		get { return _currentDialogueIndex == DISPLAY_ALL_TEXT; }
	}
	
	void Update()
	{
		if (_currentDialogueIndex != DISPLAY_ALL_TEXT && Time.time >= _nextTime)
		{
			IncrementDialogueIndex();
		}
	}
	
	private void IncrementDialogueIndex()
	{
		// Handle open and close tags that should be inserted at this index.
		for (int i = 0; i < _tags.Count; i++)
		{
			var tag = _tags[i];
			if (tag.StartPosition == _currentDialogueIndex)
			{
				// TODO this is a horrible hack
				if (tag.OpeningTag != "<br>")
					VisibleDialogue += tag.OpeningTag;
				_dialogueSuffix = tag.ClosingTag + _dialogueSuffix;
			}
			
			// Check for closing tags in reverse order.
			// Necessary to make it reappend the tags in the correct order, e.g. <b><i>text</i></b> works but <b><i>text</b></i> doesn't.
			tag = _tags[_tags.Count - i - 1];
			if (tag.EndPosition == _currentDialogueIndex)
			{
				int index = _dialogueSuffix.IndexOf (tag.ClosingTag);
				if (index != -1)
				{
					_dialogueSuffix = _dialogueSuffix.Remove (index, tag.ClosingTag.Length);
				}
				VisibleDialogue += tag.ClosingTag;
			}
		}
		
		// Add the next visible character to the displayed dialogue.
		
		if (_currentDialogueIndex < _dialogue.Length)
		{
			VisibleDialogue += _dialogue[_currentDialogueIndex];
			_nextTime = Time.time + TextSpeed;
			_currentDialogueIndex += 1;
		}
		else
		{
			_currentDialogueIndex = DISPLAY_ALL_TEXT;
		}
	}
	
	public WaitForSeconds FadeIn(float seconds)
	{
		_speakerPanel.Position = _speakerPanelLeft;
		_speakerPanel.Opacity = 0;
		return _dialoguePanel.FadeIn (seconds);
	}
	
	public WaitForSeconds FadeOut(float seconds)
	{
		HideSpeakerName();
		return _dialoguePanel.FadeOut (seconds);
	}
	
	public void ShowAllText()
	{
		while (!AllTextFinished)
		{
			IncrementDialogueIndex();
		}
	}
	
	public void ChangeDialogue(string name, string dialogue)
	{
		_currentDialogueIndex = 0;
		ChangeSpeakerName (name);
		_dialogue = dialogue;
		VisibleDialogue = "";
		ParseOutTags ();
	}
	
	private void ChangeSpeakerName(string newName)
	{
		if (newName == _name)
			return;
		
		if (_speakerPanel.Opacity == 0)
		{
			EnterSpeakerName(newName);
		}
		else
		{	
			ResetSpeakerStatus ();
			_speakerPanel.FadeOut (SpeakerNameTransitionTime, _speakerFadeStatus, null);
			_speakerPanel.Move (_speakerPanelLeft, SpeakerNameTransitionTime, _speakerMoveStatus, () => EnterSpeakerName (newName));
		}
	}
	
	private void EnterSpeakerName(string name)
	{
		_name = name;
		_speakerBox.Text = name;
		if (name != "")
		{
			ResetSpeakerStatus ();
			_speakerPanel.FadeIn (SpeakerNameTransitionTime, _speakerFadeStatus, null);
			_speakerPanel.Move(_speakerPanelRight, SpeakerNameTransitionTime, _speakerMoveStatus, null);
		}
	}
	
	private void HideSpeakerName()
	{
		ResetSpeakerStatus ();
		if (_speakerPanel.Opacity > 0)
			_speakerPanel.FadeOut (SpeakerNameTransitionTime, _speakerFadeStatus, null);
		_speakerPanel.Move (_speakerPanelLeft, SpeakerNameTransitionTime, _speakerMoveStatus, null);
	}
	
	private void ResetSpeakerStatus()
	{
		_speakerFadeStatus.Finish ();
		_speakerMoveStatus.Finish ();
		_speakerFadeStatus = new TweenStatus<float>();
		_speakerMoveStatus = new TweenStatus<Vector2>();
	}
	
	void ParseOutTags()
	{
		_tags.Clear ();
		List<Tuple<int, string>> openingTags = new List<Tuple<int, string>>();
		List<Tuple<int, string>> closingTags = new List<Tuple<int, string>>();
		
		// Parse out all the tags and keep track of their indices in the sentence.
		
		StringBuilder newDialogue = new StringBuilder();
		for (int i = 0; i < _dialogue.Length; i++)
		{
			// If this is the beginning of a tag, consume subsequent characters until the entire tag has been read.
			if (_dialogue[i] == '<')
			{
				string tag = "";
				int index = i;
				bool open = false;
				// Loop until we get to the end of the tag or the end of the input.
				while (i < _dialogue.Length)
				{
					// Add the current character to the tag.
					tag += _dialogue[i];
					
					// Add the completed tag to the dictionary.
					if (_dialogue[i] == '>')
					{
						var tagTuple = new Tuple<int, string>(newDialogue.Length, tag);
						// Keep track of whether this is an open tag or a close tag.
						if (tag.Contains ("/"))
						{
							closingTags.Add (tagTuple);
						}
						else
						{
							openingTags.Add (tagTuple);
						}
						
						break;
					}
					i++;
				}
			}
			
			// Otherwise, add the character to the dialogue to display.
			else
			{
				newDialogue.Append(_dialogue[i]);
			}
		}
		
		_dialogue = newDialogue.ToString ();
		
		// Construct the tag information by finding the closing tags that match the opening tags.
		
		foreach (var openTag in openingTags)
		{
			// Get the tag sans the open and close brackets.
			string tagName = openTag.Item2.Substring (1, openTag.Item2.Length - 2);
			// If there's an =, get rid of it and everything after it.
			tagName = tagName.Split ('=')[0];
			
			// Skip illegal tags.
			if (!_matchingTags.ContainsKey (tagName))
				continue;
			
			// Find the matching close tag.
			string match = _matchingTags[tagName];
			int indexToRemove = -1;
			for (int i = 0; i < closingTags.Count; i++)
			{
				var closeTag = closingTags[i];
				if (closeTag.Item2 == match)
				{
					_tags.Add (new TagInfo { StartPosition = openTag.Item1, 
						EndPosition = closeTag.Item1, OpeningTag = openTag.Item2, ClosingTag = closeTag.Item2 });
					indexToRemove = i;
					break;
				}
			}
			if (indexToRemove != -1)
			{
				closingTags.RemoveAt (indexToRemove);
			}
		}
		
	}

	public float MeasureDialogueWidth(string text)
	{
		return _dialogueBox.Font.MeasureText (text, _dialogueBox.FontSize, _dialogueBox.FontStyle).x;
	}
	
	public void Clear()
	{
		_name = "";
		_dialogue = "";
		_dialogueSuffix = "";
		_visibleDialogue = "";
		_speakerBox.Text = "";
		_dialogueBox.Text = "";
		_currentDialogueIndex = 0;
		_nextTime = 0f;
	}
}

public struct Tuple<T1, T2>
{
	public T1 Item1;
	public T2 Item2;
	
	public Tuple(T1 item1, T2 item2)
	{
		Item1 = item1;
		Item2 = item2;
	}
}

