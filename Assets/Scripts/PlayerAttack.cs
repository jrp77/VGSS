using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour 
{
	[Header("Basic Stuff")]
	public Transform weapon;
	public float attackTime;
	public int weaponDamage;
	public bool attacking;
	public Transform finalAttackPosition;
	private float _rememberX;
	private float _rememberY;
	private MeshRenderer _mesh;

	[Header("Keys")]
	public KeyCode attackKey;

	void Start ()
	{
		_mesh = gameObject.GetComponent<MeshRenderer>();
		_mesh.enabled = false;
	}

	void Update ()
	{
		if(Input.GetKeyDown(attackKey) && !attacking)
		{
			attacking = true;
			StartCoroutine("Attack");
		}
	}

	IEnumerator Attack ()
	{
		//enable mesh renderer
		_rememberX = transform.position.x;
		_rememberY = transform.position.y;

		_mesh.enabled = true;

		//go from right to left in certain amount of time

		float t = 0f;
		t += Time.deltaTime / attackTime;
		transform.position = Vector3.Lerp(transform.position, finalAttackPosition.position, t);

		//when it gets to the other end, turn off mesh renderer/reset position/set "attacking" to false

		yield return new WaitForSeconds(t);
		_mesh.enabled = false;
		transform.Translate(finalAttackPosition.position.x - _rememberX, finalAttackPosition.position.y - _rememberY, 0f);
		attacking = false;
	}
}
