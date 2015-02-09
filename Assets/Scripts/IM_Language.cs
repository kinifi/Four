using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Text;
using System.IO;

public class IM_Language : MonoBehaviour {

	public bool debugXMLValues = false;

	//Create a List of Languages to store for future use
	public List<Languages> _languages = new List<Languages>();
	public int numLanguagesFound;
	public string currentLanguage;
	private string defaultPath;

	//Language Class that holds all the information of each language
	public class Languages 
	{
		public string Name;
		public string filePath;
		public string title;
		public string playButton;
		public string optionsButton;
		public string quitButton;
	}

	// Use this for initialization
	void Start () {
	
		//Don't destory this GameObject so we can reference it throughout our game
		DontDestroyOnLoad(this.gameObject);

		//Call to start the loading of languages process
		setupLanguages();

	}

	#region Public Methods for Consumption

	/// <summary>
	/// Sets the language. Only call after the object has confirmed languages setup.
	/// </summary>
	/// <param name="name">language name</param>
	public void setLanguage(string name = "english")
	{

		//Debug.Log("starting to Set Language");

		if(numLanguagesFound >= 1)
		{

			for (int i = 0; i < _languages.Count; i++) 
			{
				//Debug.Log(_languages[i].Name.ToLower() + " | " + name.ToLower());
				if(_languages[i].Name.ToLower() == name.ToLower())
				{
					currentLanguage = name;
					Debug.Log("Language Set: " + name);
					break;
				}
				else
				{
					if(i == _languages.Count)
					{
						Debug.LogWarning("Language passed does not exist");
					}
				}
			}

		}
		else
		{
			currentLanguage = "english";
			Debug.Log("Only one language. Setting to English(default)");
		}
	
	}

	#endregion

	/// <summary>
	/// Setups the languages.
	/// </summary>
	private void setupLanguages()
	{
		//set the default language path
		defaultPath = Application.dataPath + "/" + "Languages" + "/";
		getFilesFromLanguageFolder();
		//call to set the language. Defaults to English
		setLanguage("swedish");
		//Uncomment for testing purposes
		//setLanguage("Swedish");
	}

	/// <summary>
	/// Gets the files from languages folder.
	/// </summary>
	/// <returns>The files from language folder.</returns>
	private void getFilesFromLanguageFolder()
	{
		checkLanguageFolderExists();
		//get all the files ending with .xml
		string[] filePaths = Directory.GetFiles(defaultPath, "*.xml");
		//set the number of files we found
		//TODO: This could be used to show loading progress if checked against the length of _languages.count
		numLanguagesFound = filePaths.Length;

		//Make sure we found at least one language or throw an error
		if(numLanguagesFound >= 1)
		{
			//let us know how many languages we found!
			Debug.Log("Found " + numLanguagesFound + " languages.");

			//get the name and file path of each language file found!
			foreach (string languageName in filePaths) 
			{

				//create a new language
				Languages _newLanguage = new Languages();
				//set the language name
				_newLanguage.Name = Path.GetFileNameWithoutExtension(languageName);
				//Debug.Log("Name: " + _newLanguage.Name);
				//set the language file path
				_newLanguage.filePath = languageName;
				//Debug.Log("File Path: " + _newLanguage.filePath);
				//add the file to the languages List
				_languages.Add(_newLanguage);
				//let us know we got a new language
				//Debug.Log("Found and Added: " + languageName);

				//Load XML: FilePath, Name of File, Language Class Object
				LoadXML(_newLanguage.filePath, _newLanguage.Name, _newLanguage);


			}

			Debug.Log("Completed Gathering All Languages");
		}
		else
		{
			Debug.LogError("No Languages Found in the Languages Folder!");
		}
	}

	/// <summary>
	/// Checks the language folder exists. If it doesn't exist. It creates one
	/// </summary>
	private void checkLanguageFolderExists()
	{
		//check to see if the directory exists. If it doesn't then create one
		if(!File.Exists(defaultPath))
		{
			//create the directory called Languages
			//NOTE: THIS SHOULD BE CREATED ANYWAYS OR WE WONT HAVE ENGLISH
			DirectoryInfo di = Directory.CreateDirectory(Application.dataPath + "/" + "Languages");
			Debug.Log("Languages Directory Created");

		}
		else
		{
			Debug.Log("Languages Folder exists");

		}
	}

	private void saveXML(string path, string fileName, string currentLanguage)
	{
		//set the path 
		path = Application.dataPath + "/Languages/" + fileName + ".xml";


		
		XmlDocument xmlDoc = new XmlDocument();
		//save to the node of the current language
		//example
		//<English>
		//<Title> Data Heroeos </Title>
		//</English>

		XmlElement elmRoot = xmlDoc.CreateElement(currentLanguage);
		xmlDoc.AppendChild(elmRoot);
		
		XmlElement settings = xmlDoc.CreateElement("Settings");
		XmlElement authorName = xmlDoc.CreateElement("authorName");
		XmlElement authorNote = xmlDoc.CreateElement("authorNote");
		//authorName.InnerText = author_Name;
		//authorNote.InnerText = author_Note;
		elmRoot.AppendChild(settings);
		settings.AppendChild(authorName);
		settings.AppendChild(authorNote);
		
		
		StreamWriter outStream = System.IO.File.CreateText(path);
		xmlDoc.Save(outStream);
		outStream.Close();
		Debug.Log("Finished Save: " + Time.realtimeSinceStartup);
	}

	/// <summary>
	/// Loads the XML
	/// </summary>
	/// <param name="mapPath">Map path.</param>
	/// <param name="mapName">Map name.</param>
	/// <param name="_lang">Language from Class Object</param>
	/// 
	private void LoadXML(string mapPath, string mapName, Languages _lang)
	{

		XmlReader reader = XmlReader.Create(mapPath);
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.Load(reader);

		#region per language
		XmlNodeList _english = xmlDoc.GetElementsByTagName("Language");

		//the first number is the first node in the XML doc
		//Second number is the number of the item in the list. If it's the 4th from the top then you add 4 in it
		//Example There is a node called "Language" and you want to read the 5th item in that node
		//_english.Item(0).ChildNodes.Item(5).InnerText


		//Get Language Name even though we already have it
		DebugXML("Language Name: " + _english.Item(0).ChildNodes.Item(0).InnerText);

		//Get the Path even though we laready have it
		DebugXML("Path: " + _english.Item(0).ChildNodes.Item(1).InnerText);

		//Get Title translation
		DebugXML("Title: " + _english.Item(0).ChildNodes.Item(2).InnerText);
		_lang.title = _english.Item(0).ChildNodes.Item(2).InnerText;
		PlayerPrefs.SetString("title", _lang.title);

		//Get Play Button translation
		DebugXML(_english.Item(0).ChildNodes.Item(3).InnerText);
		_lang.playButton = _english.Item(0).ChildNodes.Item(3).InnerText;
		PlayerPrefs.SetString("playbutton", _lang.playButton);

		//Get the Options Button Translation
		DebugXML(_english.Item(0).ChildNodes.Item(4).InnerText);
		_lang.optionsButton = _english.Item(0).ChildNodes.Item(4).InnerText;
		PlayerPrefs.SetString("optionsbutton", _lang.optionsButton);

		//get the Quit Button Translation
		DebugXML(_english.Item(0).ChildNodes.Item(5).InnerText);
		_lang.quitButton = _english.Item(0).ChildNodes.Item(5).InnerText;
		PlayerPrefs.SetString("quitbutton", _lang.quitButton);

		#endregion
		
		Debug.Log("Done Loading: " + _lang.Name);

		reader.Close();
	}

	private void DebugXML(string log)
	{
		if(debugXMLValues)
		{
			Debug.Log(log);
		}
	}

}
