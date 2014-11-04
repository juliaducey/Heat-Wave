using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class LuaTable : MonoBehaviour
{
	public enum EntryType { Integer, Double, String, Flag }

	// Name must be unique. Used to identify tables when saved.
	public string ID;

	[System.Serializable]
	public class LuaTableEntry
	{
		public string Key;
		public EntryType Type;
		public object Value;
	}

	List<LuaTableEntry> _entries = new List<LuaTableEntry>();

	/// <summary>
	/// Returns a copy of this table's entries.
	/// </summary>
	/// <value>The entries.</value>
	public List<LuaTableEntry> Entries
	{
		get
		{
			return new List<LuaTableEntry>(_entries);
		}
	}

	/// <summary>
	/// Adds a new entry, or replaces the existing one if one with the same key already exists.
	/// </summary>
	/// <param name="newEntry">New entry.</param>
	public void SetEntry(LuaTableEntry newEntry)
	{
		LuaTableEntry existingEntry = _entries.FirstOrDefault (entry => entry.Key == newEntry.Key);
		if (existingEntry != default(LuaTableEntry))
		{
			existingEntry.Value = newEntry.Value;
			existingEntry.Type = newEntry.Type;
		}
		else
		{
			_entries.Add (newEntry);
		}
	}

	public void RemoveEntry(LuaTableEntry entityToRemove)
	{
		_entries.Remove (entityToRemove);
	}

	public LuaTableEntry GetByKey(string key)
	{
		return _entries.First(entry => entry.Key == key);
	}

	/// <summary>
	/// Saves this table to PlayerPrefs.
	/// </summary>
	public void Save()
	{
		StringBuilder indices = new StringBuilder ();

		// Save entries
		foreach (var entry in _entries)
		{
			switch(entry.Type)
			{
				case EntryType.Double:
					// Might cause loss of precision?
					PlayerPrefs.SetFloat(Prefix + "Double" + entry.Key, (float)entry.Value);
					indices.Append("Double" + entry.Key + ",");
					break;
				case EntryType.Integer:
					PlayerPrefs.SetInt(Prefix + "Integer" + entry.Key, (int)entry.Value);
					indices.Append("Integer" + entry.Key + ",");
					break;
				case EntryType.String:
					PlayerPrefs.SetString(Prefix + "String" + entry.Key, (string)entry.Value);
					indices.Append("String" + entry.Key + ",");
					break;
				case EntryType.Flag:
					PlayerPrefsX.SetBool(Prefix + "Flag" + entry.Key, (bool)entry.Value);
					indices.Append("Flag" + entry.Key + ",");
					break;
			}
		}

		// Save header
		// This is used so that, when loading again, you know which indices belong with this table.
		PlayerPrefs.SetString(Prefix + "Indices", indices.ToString());
	}

	/// <summary>
	/// Repopulate fields from PlayerPrefs based on Name.
	/// </summary>
	public void Load()
	{
		// Get list of indices
		string rawIndices = PlayerPrefs.GetString(Prefix + "Indices");

		// Remove prefix and parse out indices
		//List<string> indices = rawIndices.Substring ((Prefix + "Indices").Length).Split (new char[]{','}).ToList ();
		List<string> indices = rawIndices.Split (new char[]{','}).ToList ();

		// Load each entry
		foreach (var index in indices)
		{
			//Debug.LogError (index);
			// Parse out type and load entry
			if (index.StartsWith ("Integer"))
			{
				string key = Prefix + index;
				SetEntry(new LuaTableEntry(){Key=key.Substring((Prefix + "Integer").Length), Value=PlayerPrefs.GetInt(key), Type=EntryType.Integer});
			}
			else if (index.StartsWith ("String"))
			{
				string key = Prefix + index;
				SetEntry(new LuaTableEntry(){Key=key.Substring((Prefix + "String").Length), Value=PlayerPrefs.GetString(key), Type=EntryType.String});
			}
			else if (index.StartsWith ("Flag"))
			{
				string key = Prefix + index;
				SetEntry(new LuaTableEntry(){Key=key.Substring((Prefix + "Flag").Length), Value=PlayerPrefsX.GetBool(key), Type=EntryType.Flag});
			}
			else if (index.StartsWith ("Double"))
			{
				string key = Prefix + index;
				SetEntry(new LuaTableEntry(){Key=key.Substring((Prefix + "Double").Length), Value=PlayerPrefs.GetFloat(key), Type=EntryType.Double});
			}
		}
	}

	/// <summary>
	/// Gets the prefix used to indicate PlayerPrefs values used for this table.
	/// </summary>
	/// <value>The prefix.</value>
	string Prefix
	{
		get
		{
			return ID + "!";
		}
	}
}
