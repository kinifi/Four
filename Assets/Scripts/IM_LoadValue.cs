using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class IM_LoadValue : MonoBehaviour {
	
	public bool debugLogValue = false;
	public string XMLValueToLoad;
	public Text UITextToSet;

	private void Start () {
	
		//make sure everything is lowercase
		XMLValueToLoad = XMLValueToLoad.ToLower();
		if(PlayerPrefs.HasKey(XMLValueToLoad))
		{
			this.GetComponent<Text>().text = PlayerPrefs.GetString(XMLValueToLoad);
			debugValue("Loaded: " + XMLValueToLoad);
		}
		else
		{
			Debug.LogWarning(XMLValueToLoad + " Does not exist!");
			UITextToSet.text = "ERROR";
		}

	}

	/// <summary>
	/// Translation the specified xmlvalue.
	/// </summary>
	/// <param name="xmlvalue">XML tag you want a translation from</param>
	public static string Translation(string xmlvalue)
	{

		xmlvalue = xmlvalue.ToLower();
		if(PlayerPrefs.HasKey(xmlvalue))
		{
			xmlvalue = PlayerPrefs.GetString(xmlvalue);
			return xmlvalue;
		}
		else
		{
			return (xmlvalue + " Does not exist!");
		}
		

	}

	/// <summary>
	/// Debugs the value only if DebugLogValue is on
	/// </summary>
	/// <param name="log">Log.</param>
	private void debugValue(string log)
	{
		if(debugLogValue)
		{
			Debug.Log(log);
		}
	}

}
