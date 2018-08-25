using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour 
{
	[Range(0, 3)]
	public int damage;

	void OnTriggerEnter (Collider col)
	{
		if(col.gameObject.GetComponent<PlayerScript>())
		{
			PlayerScript play = col.gameObject.GetComponent<PlayerScript>();
			play.TakeDamage(damage);
		}
	}
}
