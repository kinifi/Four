using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Text;
using System.IO;

public class IM_SaveData : MonoBehaviour {

	// Use this for initialization
	void Start () {

		saveNewLanguageXML("kinifi", "kinifis title", "play!", "options!!", "quit!!");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	private static void saveNewLanguageXML(string newLanguageName, string newTitle, string newPlayButton, string newOptionsButton, string newQuitButton)
	{
		//set the path 
		string path = Application.dataPath + "/Languages/" + newLanguageName + ".xml";

		/*
		<Language>
			<Name>English</Name>
				<filePath></filePath>
				<title>Data Heroes</title>
				<playButton>Play</playButton>
				<optionsButton>Options</optionsButton>
				<quitButton>Quit</quitButton>
		</Language>
		*/

		//Create the Language Root
		XmlDocument xmlDoc = new XmlDocument();
		XmlElement elmRoot = xmlDoc.CreateElement("Level");
		xmlDoc.AppendChild(elmRoot);

		//Create the Language Field
		XmlElement languageParentNode = xmlDoc.CreateElement("Language");

		//create a nodes for all the data needed
		XmlElement languageName = xmlDoc.CreateElement("Name");
		XmlElement filePath = xmlDoc.CreateElement("filePath");
		XmlElement title = xmlDoc.CreateElement("title");
		XmlElement playButton = xmlDoc.CreateElement("playButton");
		XmlElement optionsButton = xmlDoc.CreateElement("optionsButton");
		XmlElement quitButton = xmlDoc.CreateElement("quitButton");

		//set all the data
		languageName.InnerText = newLanguageName;
		filePath.InnerText = path + "";
		title.InnerText = newTitle;
		playButton.InnerText = newPlayButton;
		optionsButton.InnerText = newOptionsButton;
		quitButton.InnerText = newQuitButton;

		//organize all the data
		elmRoot.AppendChild(languageParentNode);
		languageParentNode.AppendChild(languageName);
		languageParentNode.AppendChild(filePath);
		languageParentNode.AppendChild(title);
		languageParentNode.AppendChild(playButton);
		languageParentNode.AppendChild(optionsButton);
		languageParentNode.AppendChild(quitButton);
		
		
		StreamWriter outStream = System.IO.File.CreateText(path);
		xmlDoc.Save(outStream);
		outStream.Close();
		Debug.Log("Finished Save: " + Time.realtimeSinceStartup);
	}


}
