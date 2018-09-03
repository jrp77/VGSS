using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour 
{
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
}
