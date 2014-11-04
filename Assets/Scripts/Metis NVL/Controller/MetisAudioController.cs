using UnityEngine;
using System.Collections;

public class MetisAudioController : MetisViewComponentController 
{
	AudioSource BGM;
	AudioSource SFX;

    void Start()
    {
        var audioSources = GetComponents<AudioSource>();
        if (audioSources.Length < 2)
        {
            Debug.LogError("Audio controller needs at least 2 audio sources!");
        }
        BGM = audioSources[0];
        SFX = audioSources[1];
    }

	public override void Show()
	{
		;
	}
	
	public override void Hide()
	{
		;
	}
	
	public override void Activate()
	{
		;
	}
	
	public override void Deactivate()
	{
		;
	}

	[LuaMethod("music")]
	public void PlayBGM(string name)
	{
		StopCoroutine("HandleFadeOutBGM");
		BGM.volume = 1;
        BGM.clip = Compendium.GetBGM(name);
        BGM.Play();
	}

	[LuaMethod("sfx")]
	public void PlaySFX(string name)
	{
        SFX.clip = Compendium.GetSFX(name);
		SFX.Play();
	}

	[LuaMethod("fade_out_music")]
	public void FadeOutBGM(double seconds)
	{
        //StartCoroutine(HandleFadeOutBGM((float)seconds));
	}

    IEnumerator HandleFadeOutBGM(float seconds)
    {
        float fTimeCounter = 0f;

        while (!(Mathf.Approximately(fTimeCounter, seconds)))
        {
            fTimeCounter = Mathf.Clamp01(fTimeCounter + Time.deltaTime);
            BGM.volume = 1f - fTimeCounter;
            yield return new WaitForSeconds(0.02f);
        }
        StopCoroutine("HandleFadeOutBGM");
    }
}
