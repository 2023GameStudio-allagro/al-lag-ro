using UnityEngine;
using System.IO;

public static class JsonLoader
{
	public static T LoadJsonData<T>(string path)
	{
		TextAsset jsonFile = Resources.Load<TextAsset>(path);
		Debug.Log(jsonFile);

		if(jsonFile == null) return default(T);
		string rawText = jsonFile.text;
		return JsonUtility.FromJson<T>(rawText) ?? default(T);
	}
}