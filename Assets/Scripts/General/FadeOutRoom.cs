using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutRoom : MonoBehaviour 
{
	public float startDist;
	public float endDist;
	private float _dist;
	public GameObject[] objToFade;
	public Transform player;
	[SerializeField] private bool _canFade;
	[SerializeField] private Color _gizmoColor;

	void Start ()
	{
		_dist = Mathf.Abs(startDist - endDist);
		Debug.Log("FadeOutRoom.cs | Dist b/w points = " + _dist.ToString());
	}

	void Update ()
	{
		float currentDist = Vector3.Distance(player.position, transform.position);
		if(currentDist < startDist && currentDist > endDist)
		{
			foreach(GameObject fObj in objToFade)
			{
				fObj.SetActive(true);
				MeshRenderer rend = fObj.GetComponentInChildren<MeshRenderer>();
			 	Color newColor = rend.material.color;
             	newColor.a -= Time.deltaTime;
             	rend.material.color = newColor;
             	gameObject.GetComponent<MeshRenderer>().material = mat;
				//rend.material.color = new Color(1f, 1f, 1f, _dist / (startDist - currentDist));

				if(rend.material.color.a <= 0.1f)
				{
					continue;
				} 
			}
		}

		else if(currentDist <= endDist)
		{
			foreach(GameObject fObj2 in objToFade)
			{
				Renderer rend = fObj2.GetComponent<Renderer>();
				rend.material.color = new Color(1f, 1f, 1f, 1f);
			}
		}
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = _gizmoColor;
		Gizmos.DrawSphere(transform.position, startDist);
		Gizmos.DrawSphere(transform.position, endDist);
		Gizmos.DrawLine(transform.position, player.position);
	}
}
