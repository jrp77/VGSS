using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour 
{
	public string playerTag;
	public int sceneToLoad;
	[SerializeField] private FadeScene _fadeScene;

	void Start ()
	{
		if(!_fadeScene)
		{
			_fadeScene = GameObject.Find("_GM").GetComponent<FadeScene>();
		}
	}

	void OnTriggerEnter (Collider col)
	{
		if(col.gameObject.tag == playerTag)
		{
			_fadeScene.StartCoroutine("ChangeScene", sceneToLoad);
		}
	}
}
