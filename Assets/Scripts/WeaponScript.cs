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
	public float weaponDamage;
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

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(transform.position, radiusOfImpact);
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
		Debug.Log("Finding enemies to hit...");
		Collider[] enem = Physics.OverlapSphere(transform.position, radiusOfImpact);
		Debug.Log(enem.Length.ToString() + " objects found!");

		if(_playerDir == PlayerDirection.Up)
		{
			foreach(Collider _temp in enem)
			{
				if(_temp.gameObject.transform.position.y <= transform.position.y + radiusOfImpact && _temp.gameObject.transform.position.y >= transform.position.y);
				{
					_enemList.Add(_temp.gameObject);
				}
			}

			Debug.Log("Found " + _enemList.Count.ToString() + " enemies! Sending attack message now...");

			StartCoroutine("Swing");
		}

		else if(_playerDir == PlayerDirection.Down)
		{
			foreach(Collider _temp in enem)
			{
				if(_temp.gameObject.transform.position.y <= transform.position.y - radiusOfImpact)
				{
					_enemList.Add(_temp.gameObject);
				}
			}

			Debug.Log("Found " + _enemList.Count.ToString() + " enemies! Sending attack message now...");

			StartCoroutine("Swing");
		}

		else if(_playerDir == PlayerDirection.Left)
		{
			foreach(Collider _temp in enem)
			{
				if(_temp.gameObject.transform.position.x <= transform.position.x - radiusOfImpact)
				{
					_enemList.Add(_temp.gameObject);
				}
			}

			Debug.Log("Found " + _enemList.Count.ToString() + " enemies! Sending attack message now...");

			StartCoroutine("Swing");
		}
		
		else if(_playerDir == PlayerDirection.Right)
		{
			foreach(Collider _temp in enem)
			{
				if(_temp.gameObject.transform.position.x <= transform.position.x + radiusOfImpact)
				{
					_enemList.Add(_temp.gameObject);
				}
			}

			Debug.Log("Found " + _enemList.Count.ToString() + " enemies! Sending attack message now...");

			StartCoroutine("Swing");
		}
	}

	IEnumerator Swing ()
	{
		foreach (GameObject enemies in _enemList)
		{
			enemies.SendMessage("TakeDamage", weaponDamage);
		}

		yield return new WaitForSeconds(attackTime);

		_enemList.Clear();
		_attacking = false;
	}
}
