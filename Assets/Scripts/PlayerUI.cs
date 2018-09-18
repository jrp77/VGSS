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
			break;

			case 2:
				int health2 = playerStats.health;
				health2 -= 1;
				for(int i = 0; i < health2; i++)
				{
					healthImages[i].sprite = healthIcon;
				}
			break;

			case 3:
				int health3 = playerStats.health;
				for(int i = 0; i < health3; i++)
				{
					healthImages[i].sprite = healthIcon;
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
}
