using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicHealthScript : MonoBehaviour 
{
	public int health;

	void Update ()
	{
		if(health <= 0)
		{
			Destroy(gameObject);
		}
	}

	public void TakeDamage(int damage)
	{
		health -= damage;
	}
}
