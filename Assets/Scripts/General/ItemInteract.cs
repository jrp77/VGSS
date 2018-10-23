using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInteract : MonoBehaviour 
{
	public enum typeOfObj
	{
		Lamp,
		Door,
		MemeCube,
	};

	private typeOfObj _t;
	public float interactRange;
	public bool interacted = false;
	public KeyCode interactKey;
	public Image interactBox;
	public Text interactText;
	[SerializeField] private GameObject _parentBox;
	public Transform player;
	[SerializeField] private bool _interacting;
	
	[Header("MemeCube")]
	public Material objMat;
	public Material effectMat;

	void Start ()
	{
		_parentBox.SetActive(false);
		objMat = gameObject.GetComponent<Renderer>().material;
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = new Color(0f, 0f, 1f, 0.5f);
		Gizmos.DrawSphere(transform.position, interactRange);
	}

	void Update ()
	{
		Debug.Log(Vector3.Distance(player.position, gameObject.transform.position).ToString());

		if(Vector3.Distance(player.position, gameObject.transform.position) <= interactRange && !_interacting && !interacted)
		{
			_interacting = true;
			_parentBox.SetActive(true);
		}

		else if(Input.GetKeyDown(interactKey) && _interacting)
		{
			_interacting = false;
			interacted = true;
			_parentBox.SetActive(false);
			
			if(_t == typeOfObj.MemeCube)
			{
				objMat = effectMat;
			}
		}

		if(Vector3.Distance(player.position, gameObject.transform.position) > interactRange)
		{
			_interacting = false;
			_parentBox.SetActive(false);
		}

		if(_interacting)
		{
			interactText.text = "Press '" + interactKey.ToString() + "' to interact";
		}
	}

}
