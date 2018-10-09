using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeScene : MonoBehaviour 
{
	public enum FadeMode {FadeIn, FadeOut};
	public FadeMode fadeM;
	public Image fadeImage;
	public float fadeTime;
	public bool isFading;
	public bool fadeComplete;
	public Animator anim;

	void Start ()
	{
		isFading = false;

		fadeImage.color = new Color(0f, 0f, 0f, 0f);
	}

	void Update ()
	{
		if(isFading)
		{
			if(fadeM == FadeMode.FadeIn)
			{
				StartCoroutine("FadeIn");
			}

			else if(fadeM == FadeMode.FadeOut)
			{
				StartCoroutine("FadeOut");
			}
		}

		else
		{
			Debug.Log("FadeScene Idle");
		}
	}

	public IEnumerator Fade (string sceneName)
	{
		Scene s = SceneManager.GetSceneByName(sceneName);

		anim.SetTrigger("beginFade");
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene(s.ToString());
	}
}
