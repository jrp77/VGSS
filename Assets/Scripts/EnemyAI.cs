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

	[Header("Play Mode Stats")]
	[SerializeField] private List<float> _distBtwPoints;
	[SerializeField] private bool _pointsMarked;
	[SerializeField] private bool _delay;
	[SerializeField] private int _nextPoint;

	[Header("Stats")]
	[SerializeField] private float _startTime;
	[SerializeField] private float _journeyLength;
	[SerializeField] private bool _chasing;

	void Start ()
	{
		nav = enemy.GetComponent<NavMeshAgent>();
		
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

		if(Vector3.Distance(enemy.position, player.position) <= recogDist)
		{
			_distBtwPoints.Clear();
			_pointsMarked = false;
			StopCoroutine("Patrol");
			StopCoroutine("ChangePoint");

			Vector3 _startPosition = enemy.position;

			_startTime = Time.time;
			_journeyLength = Vector3.Distance(enemy.position, player.position);
			
			float distCovrd = (Time.time - _startTime) * moveSpeed;
			float fracJourney = distCovrd / _journeyLength;
			transform.position = Vector3.Lerp(_startPosition, player.position, fracJourney);
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
}
