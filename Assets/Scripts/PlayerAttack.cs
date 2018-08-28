using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour 
{
	[Header("Basic Stuff")]
	public GameObject player;
	public Transform weapon;
	public Transform[] attackPoints = new Transform[4];
	private Transform _returnPoint;
	private Transform _finalAttackPoint;
	public float attackTime;
	public int weaponDamage;
	public bool attacking;

	[Header("Keys")]
	public KeyCode attackKey;

	[Header("References")]
	[SerializeField] private MeshRenderer _mesh;
	[SerializeField] private PlayerScript _player;

	void Start ()
	{
		_mesh = gameObject.GetComponent<MeshRenderer>();
		_mesh.enabled = false;

		_player = player.GetComponent<PlayerScript>();
		Debug.Log("Component found");
	}

	void Update ()
	{
		CheckForDirection();
		CheckForAttack();

		if(Input.GetKeyDown(attackKey))
		{
			attacking = true;
		}
	}

	void CheckForDirection ()
	{
		if(Input.GetKeyDown(_player.moveUp))
		{
			_returnPoint = attackPoints[0];
			_finalAttackPoint = attackPoints[1];
		}

		else if(Input.GetKeyDown(_player.moveLeft))
		{
			_returnPoint = attackPoints[1];
			_finalAttackPoint = attackPoints[2];
		}

		else if(Input.GetKeyDown(_player.moveDown))
		{
			_returnPoint = attackPoints[2];
			_finalAttackPoint = attackPoints[3];
		}

		else if(Input.GetKeyDown(_player.moveDown))
		{
			_returnPoint = attackPoints[3];
			_finalAttackPoint = attackPoints[0];
		}

		else
		{
			_returnPoint = attackPoints[0];
			_finalAttackPoint = attackPoints[1];
		}
	}

	void CheckForAttack ()
	{
		if(attacking)
		{
			_mesh.enabled = true;
			
			float t = attackTime * Time.deltaTime;

			weapon.position = Vector3.MoveTowards(weapon.position, _finalAttackPoint.position, t);

			if(Vector3.Distance(weapon.position, _finalAttackPoint.position) < 0.1f)
			{
				_mesh.enabled = false;
				weapon.position = _returnPoint.position;
				attacking = false;
			}
		}
	}
}
