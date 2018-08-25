using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour 
{
	public int sceneIndex;
	public string playerTag;

	void OnTriggerEnter (Collider col)
	{
		if(col.gameObject.tag == playerTag)
		{
			SceneManager.LoadScene(sceneIndex);
		}
	}
}
