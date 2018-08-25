using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour 
{
	[Header("Basic Variables")]
	public Transform enemy;
	public float moveSpeed;
	public float attackRange;

	[Header("Player Detection")]
	public Transform player;
	public string playerTag;
	[SerializeField] private bool _playerInArea;

	[Header("Patrol")]
	public float patrolDelay;
	public Transform[] patrolPoints;
	public float patrolSpeed;
	public bool chasing;
	public bool patrolling;
	public bool moving;
	public float minDist;
	public float nextPointDelay;
	private NavMeshAgent nav;

	[Header("Play Mode Stats")]
	[SerializeField] private List<float> _distBtwPoints;
	[SerializeField] private bool _pointsMarked;
	[SerializeField] private bool _delay;
	[SerializeField] private int _nextPoint;

	void Start ()
	{
		nav = enemy.GetComponent<NavMeshAgent>();
		
		if(!enemy)
		{
			enemy = gameObject.GetComponent<Transform>();
		}

		if(!player)
		{
			player = GameObject.FindGameObjectWithTag(playerTag).GetComponent<Transform>();
		}

		_pointsMarked = false;
		_delay = false;
		patrolling = true;
		moving = false;

		minDist = 50;

		foreach(Transform p in patrolPoints)
		{
			p.Translate(0f, enemy.position.y, 0f);
		}
	}

	void Update ()
	{
		if(_playerInArea)
		{
			MoveToPlayer();
		}

		if(!_delay)
		{
			StartCoroutine("Patrol");
		}

		if(chasing)
		{
			_distBtwPoints.Clear();
			_pointsMarked = false;
			minDist = 50;
		}

		if(enemy.position.x == patrolPoints[_nextPoint].position.x || enemy.position.z == patrolPoints[_nextPoint].position.z)
		{
			moving = false;
			_nextPoint++;
			StartCoroutine("GoToPoint", _nextPoint);
		}

		if(_nextPoint == patrolPoints.Length && !moving)
		{
			_nextPoint = 0;
			StartCoroutine("GoToPoint", _nextPoint);
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
			StartCoroutine("DelayPatrol");
		}
	}

	void MoveToPlayer ()
	{
		chasing = true;
		patrolling = false;

		float step = moveSpeed * Time.deltaTime;

		if(Vector3.Distance(enemy.position, player.position) > attackRange)
		{
			enemy.position = Vector3.MoveTowards(enemy.position, player.position, step);
		}
	}

	IEnumerator DelayPatrol ()
	{
		chasing = false;
		_delay = true;
		Debug.Log("Chase stopped, delaying until patrol");
		yield return new WaitForSeconds(patrolDelay);
		_delay = false;
		patrolling = true;
		StartCoroutine("Patrol");
	}

	IEnumerator Patrol ()
	{
		//find the closest point and go to that point
		if(patrolling)
		{
			if(!_pointsMarked)
			{
				for(int i = 0; i < patrolPoints.Length; i++)
				{
					float tempDist = Vector3.Distance(enemy.position, patrolPoints[i].position);
					_distBtwPoints.Add(tempDist);
				}

				for(int i2 = 0; i2 < _distBtwPoints.Count; i2++)
				{
					float tempMin = _distBtwPoints[i2];

					if(tempMin < minDist)
					{
						minDist = tempMin;
					}
				}

				_pointsMarked = true;
			}
			
			int fIndex = _distBtwPoints.IndexOf(minDist);
			int currentPoint = fIndex;

			NavMeshAgent nav = enemy.gameObject.GetComponent<NavMeshAgent>();
			_nextPoint = currentPoint;
			nav.destination = patrolPoints[currentPoint].position;
			moving = true;

			//if(enemy.position.x == patrolPoints[currentPoint].position.x || enemy.position.z == patrolPoints[currentPoint].position.z )
			//{
			//	moving = false;
			//	Debug.Log("Waiting to go to next point");
			//	yield return new WaitForSeconds (nextPointDelay);
			//	currentPoint++;
			//	_nextPoint = currentPoint;
			//	nav.SetDestination(patrolPoints[currentPoint].position);
			//	moving = true;
			//}

			//if(currentPoint == patrolPoints.Length && !moving)
			//{
			//	currentPoint = 0;
			//	_nextPoint = currentPoint;
			//	Debug.Log("Cycle completed, waiting to go to next point");
			//	yield return new WaitForSeconds (nextPointDelay);
			//	nav.SetDestination(patrolPoints[currentPoint].position);
			//}
		}
		yield return null;
	}

	IEnumerator GoToPoint (int pointIndex)
	{
		moving = false;
		Debug.Log("Waiting to go to next point");
		yield return new WaitForSeconds (nextPointDelay);
		nav.SetDestination(patrolPoints[pointIndex].position);
		moving = true;
	}
}
