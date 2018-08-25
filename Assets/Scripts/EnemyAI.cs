using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour 
{
	[Header("Basic Variables")]
	public Rigidbody enemy;
	public float moveSpeed;
	public float attackRange;

	[Header("Player Detection")]
	public Transform player;
	public string playerTag;
	[SerializeField] private bool _playerInArea;

	void Start ()
	{
		if(!enemy)
		{
			enemy = gameObject.GetComponent<Rigidbody>();
		}

		if(!player)
		{
			player = GameObject.FindGameObjectWithTag(playerTag).GetComponent<Transform>();
		}
	}

	void Update ()
	{
		if(_playerInArea)
		{
			MoveToPlayer();
		}

		else
		{
			Patrol();
		}
	}

	void OnTriggerEnter (Collider col)
	{
		if(col.gameObject.tag == playerTag)
		{
			_playerInArea = true;
		}
	}

	void OnTriggerExit (Collider col)
	{
		if(col.gameObject.tag == playerTag)
		{
			_playerInArea = false;
		}
	}

	void MoveToPlayer ()
	{
		Transform trans = gameObject.GetComponent<Transform>();
		trans.LookAt(player.position);

		if(Vector3.Distance(trans.position, player.position) > attackRange)
		{
			float step = moveSpeed * Time.deltaTime;
			enemy.velocity = Vector3.MoveTowards(trans.position, player.position, step);
		}
	}

	void Patrol ()
	{

	}
}
