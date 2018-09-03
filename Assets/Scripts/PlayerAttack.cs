using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour 
{
	[Header("Basic Stuff")]
	public GameObject player;
	public Transform weapon;
	//public Transform[] directionPoints = new Transform[4];	//0 = up, 1 = down, 2 = left, 3 = right
	//public Transform returnPoint;
	//public Transform finalAttackPoint;
	public float attackTime;
	public int weaponDamage;
	public bool attacking;
	public string enemyTag;

	[Header("Keys")]
	public KeyCode attackKey;

	[Header("References")]
	[SerializeField] private MeshRenderer _mesh;

	void Start ()
	{
		_mesh = gameObject.GetComponent<MeshRenderer>();
		_mesh.enabled = false;
	}

	void Update ()
	{
		if(Input.GetKeyDown(attackKey))
		{
			attacking = true;
		}
	}

	void OnTriggerEnter (Collider col)
	{
		if(attacking)
		{
			if(col.gameObject.tag == enemyTag)
			{
				StartCoroutine("Attack", col.gameObject);
			}

			else
			{
				StartCoroutine("Attack", null);
			}
		}
	}

	IEnumerator Attack (GameObject _obj)
	{
		if(!_obj)
		{
			Debug.Log("Attacking, but there is no object in the collider");
			_mesh.enabled = true;
			yield return new WaitForSeconds(attackTime);
			_mesh.enabled = false;
		}

		else
		{
			bool _sent = false;

			Debug.Log("Attacking, enemy in range, ID="+_obj.name);
			_mesh.enabled = true;

			if(!_sent)
			{
				_obj.SendMessage("TakeDamage", weaponDamage);
				_sent = true;
			}

			else
			{
				Debug.Log("Redundant, already sent damage");
			}

			yield return new WaitForSeconds(attackTime);
			_mesh.enabled = false;
			_sent = false;
			attacking = false;		
		}
	}
}
