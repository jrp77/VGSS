using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour 
{
	[Header("General UI")]
	public Image[] healthImages;
	public Sprite healthIcon;
	public PlayerScript playerStats;

	[Header("PauseMenu")]
	public KeyCode pauseButton;
	public Image pauseMenu;
	[SerializeField] private bool _paused;

	void Start ()
	{
		pauseMenu.gameObject.SetActive(false);
	}

	void Update ()
	{
		if(Input.GetKeyDown(pauseButton))
		{
			_paused = togglePause();
			
			if(!_paused)
			{
				pauseMenu.gameObject.SetActive(false);
			}

			else
			{
				pauseMenu.gameObject.SetActive(true);
			}
		}

		switch (playerStats.health)
		{
			case 0:
				Debug.Log("PlayerDied");
			break;

			case 1:
				healthImages[0].sprite = healthIcon;
				healthImages[1].sprite = null;
				healthImages[1].CrossFadeAlpha(0f, .01f, false);
				healthImages[2].sprite = null;
				healthImages[2].CrossFadeAlpha(0f, .01f, false);
			break;

			case 2:
				healthImages[0].sprite = healthIcon;
				healthImages[1].sprite = healthIcon;
				healthImages[2].sprite = null;
				healthImages[2].CrossFadeAlpha(0f, .01f, false);
			break;

			case 3:
				foreach(Image healthImg in healthImages)
				{
					healthImg.sprite = healthIcon;
				}
			break;
		}
	}

	bool togglePause ()
	{
		if(Time.timeScale == 0f)
		{
			Time.timeScale = 1f;
			return(false);
		}

		else
		{
			Time.timeScale = 0f;
			return(true);
		}
	}

	public void TriggerPauseWithButton (bool yes)
	{
		if(yes)
		{
			pauseMenu.gameObject.SetActive(false);
			Time.timeScale = 1f;
		}

		else
		{
			pauseMenu.gameObject.SetActive(true);
			Time.timeScale = 0f;
		}
	}

	public void QuitGame (bool yes)
	{
		/*

		#if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
         #else
         Application.Quit();
         #endif

		*/

		if(yes)
		{
			Application.Quit();
		}
	}
}
