using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour 
{
	public enum PlayerDirection
	{
		Up,
		Down,
		Left,
		Right
	};

	[Header("General")]
	public float attackTime;
	public float radiusOfImpact;
	public KeyCode attackKey;
	public PlayerScript playerScript;
	

	[Header("Play Mode Stats")]
	[SerializeField] private List<GameObject> _enemList;
	[SerializeField] private PlayerDirection _playerDir;
	[SerializeField] private bool _attacking;

	void Start ()
	{
		string tempDir = PlayerPrefs.GetString("playerDir");

		if(tempDir == "Up")
			_playerDir = PlayerDirection.Up;
		else if(tempDir == "Down")
			_playerDir = PlayerDirection.Down;
		else if(tempDir == "Left")
			_playerDir = PlayerDirection.Left;
		else if(tempDir == "Right")
			_playerDir = PlayerDirection.Right;
	}

	void Update ()
	{
		_playerDir = findDirection(_playerDir);
		PlayerPrefs.SetString("playerDir", _playerDir.ToString());
		//Debug.Log(PlayerPrefs.GetString("playerDir"));

		if(Input.GetKeyDown(attackKey) && !_attacking)
		{
			_attacking = true;
			MarkEnemies();
		}
	}

	PlayerDirection findDirection (PlayerDirection dir)
	{
		if(Input.GetKeyDown(playerScript.moveUp))
			dir = PlayerDirection.Up;
		else if(Input.GetKeyDown(playerScript.moveDown))
			dir = PlayerDirection.Down;
		else if(Input.GetKeyDown(playerScript.moveLeft))
			dir = PlayerDirection.Left;
		else if(Input.GetKeyDown(playerScript.moveRight))
			dir = PlayerDirection.Right;

		return dir;
	}

	void MarkEnemies ()
	{
		Collider[] enem = Physics.OverlapSphere(transform.position, radiusOfImpact);

		for(int i = 0; i < enem.Length; i++)
		{
			_enemList.Add(enem[i].gameObject);
		}

		Swing();
	}

	void Swing ()
	{
		
	}
}
