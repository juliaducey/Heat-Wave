using UnityEngine;
using System.Collections;

public class MetisDFScreenView : MonoBehaviour {
	
	public static readonly Vector2 BASE_RESOLUTION = new Vector2(1920, 1080);

	Texture2D _boxTexture;
	Rect _box1;
	Rect _box2;
	bool _drawBoxes = false;
	float _scaleFactor;

	GameObject UIRoot;

	void Awake()
	{
		UIRoot = GameObject.FindGameObjectWithTag("UIRoot");
		_boxTexture = new Texture2D(1, 1);
		_boxTexture.SetPixel (0, 0, Color.black);
		_boxTexture.Apply ();

		ScaleWindow (Screen.width, Screen.height);
	}
	
	void OnGUI()
	{
		if (_drawBoxes)
		{
			GUI.DrawTexture (_box1, _boxTexture);
			GUI.DrawTexture (_box2, _boxTexture);
		}
	}

	private void ScaleWindow(int screenWidth, int screenHeight)
	{
		var baseAspectRatio = BASE_RESOLUTION.x/BASE_RESOLUTION.y;
		var realAspectRatio = screenWidth/screenHeight;
		
		if (realAspectRatio < baseAspectRatio)
		{
			_scaleFactor = screenWidth / BASE_RESOLUTION.x;
			_drawBoxes = true;
			
			var viewHeight = BASE_RESOLUTION.y * _scaleFactor;
			var boxHeight = (int) (screenHeight - viewHeight)/2 + 1;
			_box1 = new Rect(0, 0, screenWidth, boxHeight);
			_box2 = new Rect(0, screenHeight - boxHeight, screenWidth, boxHeight);
		}
		else if (realAspectRatio > baseAspectRatio)
		{
			_scaleFactor = screenHeight / BASE_RESOLUTION.y;
			_drawBoxes = true;
			
			var viewWidth = BASE_RESOLUTION.x * _scaleFactor;
			var boxWidth = (int) (screenWidth - viewWidth)/2 + 1;
			_box1 = new Rect(0, 0, boxWidth, screenHeight);
			_box2 = new Rect(screenWidth - boxWidth, 0, boxWidth, screenHeight);
		}
		else
		{
			_scaleFactor = 1;
			_drawBoxes = false;
		}
		
		var gui = UIRoot.GetComponent<dfGUIManager>();
		gui.UIScale = _scaleFactor;
	}
}
