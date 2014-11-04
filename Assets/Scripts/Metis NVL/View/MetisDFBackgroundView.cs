using UnityEngine;
using System.Collections;

public class MetisDFBackgroundView : dfViewComponent {

	private Compendium _compendium;
	private dfTextureSprite _bg;
	public float Opacity
	{
		get { return _panel.Opacity; }
	}

	protected override void OnAwake()
	{
		_bg = (dfTextureSprite) _panel.GetComponentInChildren<dfTextureSprite>();
	}

	void Start()
	{
		_panel.Width = MetisDFScreenView.BASE_RESOLUTION.x;
		_panel.Height = MetisDFScreenView.BASE_RESOLUTION.y;
	}

	public void ChangeBackground(Texture texture)
	{
		_bg.Texture = texture;
	}

	public void FadeIn(float seconds)
	{
		_panel.FadeIn (seconds);
	}

	public void FadeOut(float seconds)
	{
		_panel.FadeOut (seconds);
	}
}
