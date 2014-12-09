using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseManager : MonoBehaviour {
	
	Canvas canvas;
	
	void Start()
	{
		canvas = GetComponent<Canvas>();
		canvas.enabled = false;
	}
	
	void Update()
	{

	}
	public void ToggleCanvas(){
		canvas.enabled = !canvas.enabled;
	}

	public void Pause()
	{
		canvas.enabled = !canvas.enabled;
		Time.timeScale = Time.timeScale == 0 ? 1 : 0;
	}
	
	public void Quit()
	{
		Application.LoadLevel ("EndScreen");
		
//		#if UNITY_EDITOR 
//		EditorApplication.isPlaying = false;
//		#else 
//		Application.Quit();
//		#endif
	}
}