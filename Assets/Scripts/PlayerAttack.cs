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
	[SerializeField] private bool _enemyInArea;

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
			StartCoroutine("Attack");
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
				StartCoroutine("Attack", col.gameObject);
			}
		}
	}

	IEnumerator Attack (GameObject _obj, bool _isEnemy)
	{
		if(!_obj)
		{
			_mesh.enabled = true;
			Debug.Log("No enemy in range, attacking anyways");
			yield return new WaitForSeconds(attackTime);
			_mesh.enabled = false;
		}

		else
		{
			_mesh.enabled = true;
			Debug.Log("Enemy in range, ID=" + _obj.name + ", attacking now and dealing " + weaponDamage.ToString());
			_obj.SendMessage("TakeDamage", weaponDamage);
			yield return new WaitForSeconds(attackTime);
			_mesh.enabled = false; 
		}
	}
}
