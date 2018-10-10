using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour 
{
	public string sceneName;
	public string playerTag;

	private FadeScene _fScene;

	void Start ()
	{
		_fScene = GameObject.Find("_GM").GetComponent<FadeScene>();
	}

	void OnTriggerEnter (Collider col)
	{
		if(col.gameObject.tag == playerTag)
		{
			_fScene.StartCoroutine("Fade", sceneName);
		}
	}

	void Update ()
	{
		if(_fScene.fadeComplete)
		{
			SceneManager.LoadScene(sceneName);
		}
	}
}
