using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public static class UIExtensions {

	#region Getters and Setters

	public static Vector2 GetPosition(this Graphic self)
	{
		return self.rectTransform.rect.position;
	}

	public static void SetPosition(this Graphic self, Vector2 position)
	{
		self.rectTransform.position = new Vector3(position.x, position.y, self.rectTransform.position.z);
	}
	
	public static void SetPosition(this Graphic self, float x, float y)
	{
		self.SetPosition (new Vector2(x, y));
	}

	public static float GetWidth(this Graphic self)
	{
		return self.rectTransform.rect.width;
	}

	public static void SetWidth(this Graphic self, float width)
	{
		self.rectTransform.sizeDelta = new Vector2(width, self.rectTransform.rect.height);
	}

	public static float GetHeight(this Graphic self)
	{
		return self.rectTransform.rect.height;
	}
	
	public static void SetHeight(this Graphic self, float height)
	{
		self.rectTransform.sizeDelta = new Vector2(self.rectTransform.rect.width, height);
	}

	public static void SetAlpha(this Graphic self, float alpha)
	{
		self.canvasRenderer.SetAlpha (alpha);
	}

	public static float GetAlpha(this Graphic self)
	{
		return self.canvasRenderer.GetAlpha ();
	}

	#endregion
	#region Tweens

	public static WaitForSeconds FadeIn(this Graphic self, float seconds, Action postFadeCallback)
	{
		self.StartCoroutine (self.GetComponent<CanvasRenderer>().FadeTo(1, seconds, postFadeCallback));
		return new WaitForSeconds(seconds);
	}
	
	public static WaitForSeconds FadeOut(this Graphic self, float seconds, Action postFadeCallback)
	{
		self.StartCoroutine (self.GetComponent<CanvasRenderer>().FadeTo(0, seconds, postFadeCallback));
		return new WaitForSeconds(seconds);
	}
	
	public static WaitForSeconds FadeIn(this Graphic self, float seconds)
	{
		return self.FadeIn (seconds, null);
	}
	
	public static WaitForSeconds FadeOut(this Graphic self, float seconds)
	{
		return self.FadeOut (seconds, null);
	}

	public static WaitForSeconds FadeIn(this CanvasRenderer self, float seconds, Action postFadeCallback)
	{
		self.GetComponent<Graphic>().StartCoroutine (self.FadeTo(1, seconds, postFadeCallback));
		return new WaitForSeconds(seconds);
	}
	
	public static WaitForSeconds FadeOut(this CanvasRenderer self, float seconds, Action postFadeCallback)
	{
		self.GetComponent<Graphic>().StartCoroutine (self.FadeTo(0, seconds, postFadeCallback));
		return new WaitForSeconds(seconds);
	}
	
	public static WaitForSeconds FadeIn(this CanvasRenderer self, float seconds)
	{
		return self.FadeIn (seconds, null);
	}
	
	public static WaitForSeconds FadeOut(this CanvasRenderer self, float seconds)
	{
		return self.FadeOut (seconds, null);
	}
	
	private static IEnumerator FadeTo(this CanvasRenderer self, float newAlpha, float fadeSeconds, Action postFadeCallback)
	{
		float alpha = self.GetAlpha ();
		
		for(float t = 0.0f; true; t += Time.deltaTime / fadeSeconds)
		{
			// If it's close enough to done that we don't really care anymore
			if(Mathf.Abs(newAlpha - self.GetAlpha ()) <= 0.05f)
			{
				self.SetAlpha(newAlpha);
				if (postFadeCallback != null)
				{
					postFadeCallback();
				}
				break;
			}
			// Otherwise just continue fading
			else
			{
				self.SetAlpha(Mathf.Lerp (alpha, newAlpha, t));
				yield return null;
			}
		}
		yield break;
	}
	
	public static WaitForSeconds Move(this Graphic self, Vector2 targetPosition, float moveTime, Action callback)
	{
		self.StartCoroutine (self.MoveTo (targetPosition, moveTime, callback));
		return new WaitForSeconds(moveTime);
	}
	
	public static WaitForSeconds Move(this Graphic self, Vector2 targetPosition, float moveTime)
	{
		return self.Move (targetPosition, moveTime, null);
	}
	
	public static WaitForSeconds MoveRelative(this Graphic self, Vector2 delta, float moveTime, Action callback)
	{
		Vector2 targetPosition = new Vector2(self.GetPosition ().x + delta.x, self.GetPosition ().y + delta.y);
		return self.Move (targetPosition, moveTime, callback);
	}
	
	public static WaitForSeconds MoveRelative(this Graphic self, Vector2 delta, float moveTime)
	{
		return self.MoveRelative (delta, moveTime, null);
	}
	
	private static IEnumerator MoveTo(this Graphic self, Vector2 targetPosition, float moveTime, Action callback)
	{
		Vector2 startingPosition = self.GetPosition ();

		for(float t = 0.0f; true; t += Time.deltaTime / moveTime)
		{
			// If it's close enough to done that we don't really care anymore
			if(Vector2.Distance(targetPosition, new Vector2(self.GetPosition().x, self.GetPosition().y)) <= 0.05f)
			{
				// Set position to final value
				if (callback != null)
				{
					callback.Invoke ();
				}
				self.SetPosition (targetPosition);
				break;
			}
			// Otherwise just continue moving
			else
			{
				Vector2 newPosition = Vector2.Lerp(startingPosition, targetPosition, t);
				self.SetPosition (newPosition);
				yield return null;
			}
		}
		yield break;
	}

	#endregion
}