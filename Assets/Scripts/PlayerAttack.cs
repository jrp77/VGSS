using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour 
{
	[Header("Basic Stuff")]
	public GameObject player;
	public Transform weapon;
	public Transform[] directionPoints = new Transform[4];	//0 = up, 1 = down, 2 = left, 3 = right
	public Transform returnPoint;
	public Transform finalAttackPoint;
	public float attackTime;
	public int weaponDamage;
	public bool attacking;

	[Header("Keys")]
	public KeyCode attackKey;

	[Header("References")]
	[SerializeField] private MeshRenderer _mesh;
	[SerializeField] private PlayerScript _player;

	[Header("Direction Moving")]
	[SerializeField] private bool _up;
	[SerializeField] private bool _down;
	[SerializeField] private bool _left;
	[SerializeField] private bool _right;

	void Start ()
	{
		_mesh = gameObject.GetComponent<MeshRenderer>();
		_mesh.enabled = false;

		_player = player.GetComponent<PlayerScript>();
		Debug.Log("Component found");
	}

	void Update ()
	{
		CheckForAttack();
		CheckForDirection();

		if(Input.GetKeyDown(attackKey))
		{
			attacking = true;
		}
	}

	void CheckForDirection ()
	{
		if(Input.GetKeyDown(_player.moveUp))
		{
			Vector3 upRot = Vector3.RotateTowards(_player.transform.position, directionPoints[0].position, 5, 0);
			_player.transform.rotation = Quaternion.LookRotation(upRot);
		}

		if(Input.GetKeyDown(_player.moveDown))
		{
			Vector3 downRot = Vector3.RotateTowards(_player.transform.position, directionPoints[1].position, 5, 0);
			_player.transform.rotation = Quaternion.LookRotation(downRot);
		}
	}

	void CheckForAttack ()
	{
		if(attacking)
		{
			_mesh.enabled = true;
			
			float t = attackTime * Time.deltaTime;

			weapon.position = Vector3.MoveTowards(weapon.position, finalAttackPoint.position, t);

			if(Vector3.Distance(weapon.position, finalAttackPoint.position) < 0.1f)
			{
				_mesh.enabled = false;
				weapon.position = returnPoint.position;
				attacking = false;
			}
		}
	}
}
