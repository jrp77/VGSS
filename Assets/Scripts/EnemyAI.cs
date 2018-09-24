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
	public int attackDamage;
	public float attackTime;
	public int health;

	[Header("Player Detection")]
	public Transform player;
	public float recogDist;

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
	private bool _attacking;

	[Header("Play Mode Stats")]
	[SerializeField] private List<float> _distBtwPoints;
	[SerializeField] private bool _pointsMarked;
	[SerializeField] private bool _delay;
	[SerializeField] private int _nextPoint;

	[Header("Stats")]
	[SerializeField] private float _startTime;
	[SerializeField] private float _journeyLength;
	[SerializeField] private bool _chasing;

	void OnDrawGizmos ()
	{
		Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
		Gizmos.DrawSphere(transform.position, recogDist);
		Gizmos.DrawSphere(transform.position, attackRange);
	}

	void Start ()
	{
		nav = enemy.GetComponent<NavMeshAgent>();

		_attacking = false;
		
		if(!enemy)
		{
			enemy = gameObject.GetComponent<Transform>();
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
		Debug.Log("Next point= " + _nextPoint.ToString());

		if(_nextPoint < 0)
		{
			_nextPoint = 0;
		}
		
		Debug.Log("Health = " + health.ToString());

		if(Vector3.Distance(enemy.position, patrolPoints[_nextPoint].position) < 0.4f)
		{
			StopCoroutine("Patrol");
			StartCoroutine("ChangePoint");
		}

		if(_nextPoint == patrolPoints.Length && !moving)
		{
			_nextPoint = 0;

			StartCoroutine("Patrol", _nextPoint);
		}

		if(Vector3.Distance(enemy.position, player.position) <= recogDist && Vector3.Distance(enemy.position, player.position) >= attackRange)
		{
			patrolling = false;
			chasing = true;
			_pointsMarked = false;
			_distBtwPoints.Clear();

			nav.isStopped = false;
			nav.SetDestination(player.position);
		}

		else if(Vector3.Distance(enemy.position, player.position) <= attackRange && !_attacking)
		{
			_attacking = true;

			nav.isStopped = true;
			StartCoroutine("AttackPlayer");
		}

		else if(Vector3.Distance(enemy.position, player.position) >= recogDist && chasing)
		{
			patrolling = true;
			chasing = false;

			nav.isStopped = false;
			StartCoroutine("Patrol", _nextPoint);
		}

		else
		{
			if(_pointsMarked)
			{
				StartCoroutine("Patrol", _nextPoint);
			}

			else
			{
				int nxt = nextPoint();
				StartCoroutine("Patrol", nxt);
			}
		}

		if(health <= 0)
		{
			Destroy(gameObject);
		}
	}

	int nextPoint ()
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

			int fIndex = _distBtwPoints.IndexOf(minDist);
			_nextPoint = fIndex;
		}

		return _nextPoint;
	}

	/* Reworking to solve for a variable

	void MarkPoints ()
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

			int fIndex = _distBtwPoints.IndexOf(minDist);
			int minPoint = fIndex;
			_nextPoint = minPoint;
			
			StartCoroutine("Patrol", _nextPoint);
		}
	}

	*/

	IEnumerator Patrol (int point)
	{
		moving = true;
		nav.SetDestination(patrolPoints[point].position);
		yield return null;
	}

	IEnumerator ChangePoint ()
	{
		if(_nextPoint < patrolPoints.Length)
		{
			_nextPoint += 1;
		}

		if(_nextPoint == patrolPoints.Length)
		{
			_nextPoint = 0;
		}

		moving = false;
		Debug.Log("Waiting for next point");
		yield return new WaitForSeconds(nextPointDelay);
		StartCoroutine("Patrol", _nextPoint);
	}
	
	public void TakeDamage (int damage)
	{
		health -= damage;
	}

	IEnumerator AttackPlayer ()
	{
		_attacking = true;
		player.transform.parent.SendMessage("TakeDamage", attackDamage);
		yield return new WaitForSeconds(attackTime);
		_attacking = false;
	}
}
