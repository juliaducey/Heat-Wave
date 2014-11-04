using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MetisDFPortraitView : dfViewComponent 
{
	public string LeftPortraitName = "Left Portrait";
	public string RightPortraitName = "Right Portrait";
	dfTextureSprite _leftPortrait;
	dfTextureSprite _rightPortrait;
	public float PictureMoveDelta = 20;

	Vector2 _leftPictureOut;
	Vector2 _leftPictureIn;
	Vector2 _rightPictureOut;
	Vector2 _rightPictureIn;
	// TODO use a non-hack fix
	string _leftTag;
	string _rightTag;
	Dictionary<string, Texture> pictures = new Dictionary<string, Texture>();

	Compendium Compendium;

	// TODO this should be handled in the controller maybe??
	public float PictureTransitionTime = 0.5f;
	public float ExpressionChangeTime = 0.1f;

	protected override void OnAwake()
	{
		Compendium = GameObject.FindGameObjectWithTag(Compendium.TAG).GetComponent<Compendium>();
		var portraits = _panel.GetComponentsInChildren<dfTextureSprite>();
		foreach (var portrait in portraits)
		{
			if (portrait.name == LeftPortraitName)
			{
				_leftPortrait = portrait;
			}
			else if (portrait.name == RightPortraitName)
			{
				_rightPortrait = portrait;
			}
		}

		_leftPictureOut = new Vector2(_leftPortrait.Position.x, _leftPortrait.Position.y);
		_rightPictureOut = new Vector2(_rightPortrait.Position.x, _rightPortrait.Position.y);
		_leftPictureIn = _leftPictureOut + new Vector2(PictureMoveDelta, 0);
		_rightPictureIn = _rightPictureOut + new Vector2(-1*PictureMoveDelta, 0);
	}

	public float ShowPicture(string tag, string picAlias)
	{
		Texture newTexture = null;
		if (string.IsNullOrEmpty(picAlias))
		{
			newTexture = Compendium.GetCharacter(tag);
		}
		else
		{
			newTexture = Compendium.GetCharacter(tag, picAlias);
		}
		
		// Don't ease out if a picture with that tag is already on screen; just cross-fade instead
		if (pictures.ContainsKey(tag))
		{
			return ReplacePicture(tag, newTexture);
		}
		else
		{
			if (_leftPortrait.Opacity < 1)
			{
				SetPicture(_leftPortrait, newTexture);
				pictures.Add(tag, newTexture);
				_leftTag = tag;
				_leftPortrait.name = tag;
				EasePictureIn (_leftPortrait);
				return PictureTransitionTime;
			}
			else if (_rightPortrait.Opacity < 1)
			{
				SetPicture(_rightPortrait, newTexture);
				pictures.Add(tag, newTexture);
				_rightTag = tag;
				_rightPortrait.name = tag;
				EasePictureIn(_rightPortrait);
				return PictureTransitionTime;
			}
			else
			{
				Debug.LogError ("Too many pictures onscreen!");
				return 0;
			}
		}
	}

	void EasePictureIn(dfTextureSprite picture)
	{
		EasePictureIn (picture, null);
	}
	
	void EasePictureOut(dfTextureSprite picture)
	{
		EasePictureOut (picture, null);
	}
	
	void EasePictureIn(dfTextureSprite picture, Action callback)
	{
		if (picture.name == _leftTag) // if the picture is on the left
		{
			picture.Move ( _leftPictureIn, PictureTransitionTime);
		}
		else // if the picture is on the right
		{
			picture.Move (_rightPictureIn, PictureTransitionTime);
		}
		picture.FadeIn (PictureTransitionTime, null, callback);
	}
	
	void EasePictureOut(dfTextureSprite picture, Action callback)
	{
		if (picture.name == _leftTag) // if the picture is on the left
		{
			picture.Move (_leftPictureOut, PictureTransitionTime);
		}
		else // if the picture is on the right
		{
			picture.Move (_rightPictureOut, PictureTransitionTime);
		}
		picture.FadeOut (PictureTransitionTime, null, callback);
	}
	
	public void HidePicture(string tag)
	{
		ExitCharacter (tag);
	}
	
	float ReplacePicture(string tag, Texture newTexture)
	{
		dfTextureSprite pictureToReplace = null;
		if (_leftPortrait.Texture == pictures[tag])
		{
			_leftTag = tag;
			pictureToReplace = _leftPortrait;
			_leftPortrait.name = tag;
		}
		else if (_rightPortrait.Texture == pictures[tag])
		{
			_rightTag = tag;
			pictureToReplace = _rightPortrait;
			_rightPortrait.name = tag;
		}
		else
		{
			Debug.LogError("No matching picture for: " + tag);
		}
		
		pictureToReplace.FadeOut (ExpressionChangeTime, null, delegate()
		                {
			// TODO refactor this hack
			if (pictureToReplace == _leftPortrait)
			{
				_leftTag = tag;
			}
			else
			{
				_rightTag = tag;
			}
			SetPicture(pictureToReplace, newTexture);
			pictures[tag] = newTexture;
			pictureToReplace.FadeIn (ExpressionChangeTime);
		});

		return ExpressionChangeTime*2;
	}
	
	private void SetPicture(dfTextureSprite picture, Texture texture)
	{
		//picture.Width = (int) (texture.width);
		//picture.Height = (int) (texture.height);
		picture.Texture = texture;
	}
	
	public void ExitCharacter(string name)
	{
		if (_leftTag == name)
		{
			EasePictureOut(_leftPortrait);
		}
		else if (_rightTag == name)
		{
			EasePictureOut(_rightPortrait);
		}
		pictures.Remove (name);
	}
	
	public void ExitAll()
	{
		EasePictureOut (_leftPortrait);
		EasePictureOut (_rightPortrait);
		pictures.Clear ();
	}

	public void FadeIn()
	{
		_panel.FadeIn (PictureTransitionTime);
	}

	public void FadeOut()
	{
		ExitAll ();
		_panel.FadeOut (PictureTransitionTime);
	}
}
