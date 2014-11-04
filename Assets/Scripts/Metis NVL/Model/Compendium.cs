using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Registry of characters and BGs to appear in a Metis NVL scene.
/// </summary>
public class Compendium : MonoBehaviour
{
	public List<CharacterCompendiumRecord> Characters;
	public List<CompendiumRecord> BGs;
	public List<SoundCompendiumRecord> BGMs;
	public List<SoundCompendiumRecord> SFXs;

	public static readonly string TAG = "Compendium";

	/// <summary>
	/// Gets the character image with the given alias.
	/// </summary>
	/// <returns>The character.</returns>
	/// <param name="alias">Alias.</param>
	public Texture GetCharacter(string alias)
	{
		return Characters.First(entry => entry.Name == alias).DefaultImage;
	}

	/// <summary>
	/// Gets the character image with the given alias and tag.
	/// </summary>
	/// <returns>The character.</returns>
	/// <param name="alias">Alias.</param>
	public Texture GetCharacter(string alias, string tag)
	{
		if (tag == "normal")
		{
			return Characters.First(entry => entry.Name == alias).DefaultImage;
		}
		else
		{
			return Characters.First(entry => entry.Name == alias).Expressions.First (entry => entry.Name == tag).Image;
		}
	}

	/// <summary>
	/// Gets the BG image with the given alias.
	/// </summary>
	/// <returns>The character.</returns>
	/// <param name="alias">Alias.</param>
	public Texture GetBG(string alias)
	{
		return BGs.First(entry => entry.Name == alias).Image;
	}

	public AudioClip GetBGM(string alias)
	{
		return BGMs.First(entry => entry.Name == alias).Sound;
	}

	public AudioClip GetSFX(string alias)
	{
		return SFXs.First(entry => entry.Name == alias).Sound;
	}
}

[System.Serializable]
public class CompendiumRecord
{
	public Texture Image;
	public string Name;
}

[System.Serializable]
public class CharacterCompendiumRecord
{
	public string Name;
	public Texture DefaultImage;
	public List<CompendiumRecord> Expressions;
}

[System.Serializable]
public class SoundCompendiumRecord
{
	public AudioClip Sound;
	public string Name;
}
