using UnityEngine;
using System.Collections;
using System;

public static class dfControlExtensions {

	public static WaitForSeconds FadeIn(this dfControl self, float seconds, TweenStatus<float> status, Action postFadeCallback)
	{
		self.StartCoroutine (self.FadeTo(1, seconds, status, postFadeCallback));
		return new WaitForSeconds(seconds);
	}
	
	public static WaitForSeconds FadeOut(this dfControl self, float seconds, TweenStatus<float> status, Action postFadeCallback)
	{
		self.StartCoroutine (self.FadeTo(0, seconds, status, postFadeCallback));
		return new WaitForSeconds(seconds);
	}

	public static WaitForSeconds FadeIn(this dfControl self, float seconds)
	{
		return self.FadeIn (seconds, null, null);
	}
	
	public static WaitForSeconds FadeOut(this dfControl self, float seconds)
	{
		return self.FadeOut (seconds, null, null);
	}
	
	private static IEnumerator FadeTo(this dfControl self, float newAlpha, float fadeSeconds, TweenStatus<float> status, Action postFadeCallback)
	{
		float alpha = self.Opacity;
		
		if (status == null)
			status = new TweenStatus<float>();

		status.StartState = alpha;
		status.EndState = newAlpha;
		status.Active = true;

		for(float t = 0.0f; true; t += Time.deltaTime / fadeSeconds)
		{
			// If it's close enough to done that we don't really care anymore
			if(Mathf.Abs(newAlpha - self.Opacity) <= 0.05f || status.Finished)
			{
				self.Opacity = newAlpha;
				if (postFadeCallback != null)
				{
					postFadeCallback();
				}
				status.Finish ();
				status.Active = false;
				break;
			}
			// Otherwise just continue fading
			else
			{
				self.Opacity = Mathf.Lerp (alpha, newAlpha, t);
				yield return null;
			}
		}
		yield break;
	}

	public static WaitForSeconds Move(this dfControl self, Vector2 targetPosition, float moveTime, TweenStatus<Vector2> status, Action callback)
	{
		self.StartCoroutine (self.MoveTo (targetPosition, moveTime, status, callback));
		return new WaitForSeconds(moveTime);
	}

	public static WaitForSeconds Move(this dfControl self, Vector2 targetPosition, float moveTime)
	{
		return self.Move (targetPosition, moveTime, null, null);
	}
	
	public static WaitForSeconds MoveRelative(this dfControl self, Vector2 delta, float moveTime, TweenStatus<Vector2> status, Action callback)
	{
		Vector2 targetPosition = new Vector2(self.Position.x + delta.x, self.Position.y + delta.y);
		return self.Move (targetPosition, moveTime, status, callback);
	}

	public static WaitForSeconds MoveRelative(this dfControl self, Vector2 delta, float moveTime)
	{
		return self.MoveRelative (delta, moveTime, null, null);
	}

	private static IEnumerator MoveTo(this dfControl self, Vector2 targetPosition, float moveTime, TweenStatus<Vector2> status, Action callback)
	{
		Vector2 startingPosition = new Vector2(self.Position.x, self.Position.y);

		if (status == null)
			status = new TweenStatus<Vector2>();

		status.StartState = startingPosition;
		status.EndState = targetPosition;
		status.Active = true;

		for(float t = 0.0f; true; t += Time.deltaTime / moveTime)
		{
			// If it's close enough to done that we don't really care anymore
			if(Vector2.Distance(targetPosition, new Vector2(self.Position.x, self.Position.y)) <= 0.05f || status.Finished)
			{
				// Set position to final value
				if (callback != null)
				{
					callback.Invoke ();
				}
				self.Position = new Vector3(targetPosition.x, targetPosition.y, self.Position.z);
				status.Finish();
				status.Active = false;
				break;
			}
			// Otherwise just continue moving
			else
			{
				Vector2 newPosition = Vector2.Lerp(startingPosition, targetPosition, t);
				self.Position = new Vector3(newPosition.x, newPosition.y, self.Position.z);
				yield return null;
			}
		}
		yield break;
	}
}

public class TweenStatus<T>
{
	public T StartState { get; set; }
	public T EndState { get; set; }
	public bool Active { get; set; }
	public string fuck = "no";
	public float time=0;

	bool _finished;
	public bool Finished
	{
		get { return _finished; }
	}

	public void Finish()
	{
		_finished = true;
	}
}