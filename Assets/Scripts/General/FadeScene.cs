using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeScene : MonoBehaviour 
{
	public Image fadeImage;
	public float fadeTime;
	public bool firstScene = false;

	void Start ()
	{
		if(firstScene)
		{
			fadeImage.color = new Color(0f, 0f, 0f, 1f);
			fadeImage.CrossFadeAlpha(0f, fadeTime / 2, false);
			firstScene = false;
		}
	}

	public IEnumerator ChangeScene (int buildInt)
	{
		fadeImage.CrossFadeAlpha(1f, fadeTime / 2, false);
		yield return new WaitForSeconds(fadeTime / 2);
		SceneManager.LoadScene(buildInt);
	}
}
