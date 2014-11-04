using UnityEngine;
using System.Collections;

public class MetisDFFadeView : dfViewComponent {

	dfTextureSprite _black;
	dfPanel _dialogueBG;
	public string DialogueBackgroundPanel = "Dialogue Background";

	protected override void OnAwake()
	{
		_black = _panel.GetComponentInChildren<dfTextureSprite>();
		_dialogueBG = FindPanelWithName (DialogueBackgroundPanel);
		var texture = new Texture2D(1, 1);
		texture.SetPixel (0, 0, Color.black);
		texture.Apply ();
		_black.Texture = texture;
		_black.Width = (int) MetisDFScreenView.BASE_RESOLUTION.x;
		_black.Height = (int) MetisDFScreenView.BASE_RESOLUTION.y;
	}

	public void Show(float seconds)
	{
		_panel.FadeIn (seconds);
		_dialogueBG.FadeOut (seconds*2);
	}

	public void Hide(float seconds)
	{
		_panel.FadeOut (seconds);
		_dialogueBG.FadeIn (seconds*2);
	}

}
