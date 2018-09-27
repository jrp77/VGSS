using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeScene : MonoBehaviour 
{
	public Image fadeImage;
	public float fadeTime;

	public void Fade (bool fadeStatus, string sName)	//if true= fade in, false= fade out
	{
		if(fadeStatus)
		{
			fadeImage.CrossFadeAlpha(1f, fadeTime, false);
			SceneManager.LoadScene(sName);
		}

		else
		{
			fadeImage.CrossFadeAlpha(0f, fadeTime, false);
		}
	}
}
