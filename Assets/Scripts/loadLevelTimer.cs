using UnityEngine;
using System.Collections;

public class loadLevelTimer : MonoBehaviour {

	public string m_levelToLoad;
	public float m_Timer = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		m_Timer -= Time.deltaTime;

		if(m_Timer < 0)
		{
			Application.LoadLevel(m_levelToLoad);
		}

	}
}
