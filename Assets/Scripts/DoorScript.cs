using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour 
{
	public string sceneName;
	public string playerTag;

	void OnTriggerEnter (Collider col)
	{
		if(col.gameObject.tag == playerTag)
		{
			FadeScene fScene = GameObject.Find("_GM").GetComponent<FadeScene>();
			fScene.Fade(true, sceneName);
		}
	}
}
