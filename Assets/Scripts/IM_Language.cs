using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Text;
using System.IO;

public class IM_Language : MonoBehaviour {

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

	/// <summary>
	/// Sets the language. Only call after the object has confirmed languages setup.
	/// </summary>
	/// <param name="name">language name</param>
	public void setLanguage()
	{
		string name = "english";
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

	/// <summary>
	/// Setups the languages.
	/// </summary>
	private void setupLanguages()
	{
		defaultPath = Application.dataPath + "/" + "Languages" + "/";
		getFilesFromLanguageFolder();
		Invoke("setLanguage", 0.5f);
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

	private void LoadXML(string mapPath, string mapName)
	{
		
		mapPath = Application.dataPath + "/UserLevels/" +  mapName + "/" + mapName + ".xml";

		XmlReader reader = XmlReader.Create(mapPath);
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.Load(reader);

		#region per language
		XmlNodeList _english = xmlDoc.GetElementsByTagName("English");

		//Get the background Color
		Debug.Log(_english.Item(0).ChildNodes.Item(0).InnerText);
		//Get the Author name
		Debug.Log(_english.Item(0).ChildNodes.Item(1).InnerText);
		//get the author Note
		Debug.Log(_english.Item(0).ChildNodes.Item(2).InnerText);
		#endregion
		
		//////////////////////////////////////////
		/// make sure all the data we need is set here!
		/// //////////////////////////////////////

		reader.Close();
	}

}
