using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour 
{
    [Header("Movement")]
    public Rigidbody player;
    public float moveSpeed;
    public float sprintSpeed;
    public float accel;
    public float decel;
    private bool _canMove;

    [Header("Health")]
    public int health;
    private int _maxHealth;
    private bool _canTakeDmg;
    public float damageOffset;

    [Header("Damage Flicker")]
    public float flickerRate;
    public int flickerRepeat;

    [Header("Keys")]
    public KeyCode moveUp;
    public KeyCode moveDown;
    public KeyCode moveLeft;
    public KeyCode moveRight;
    public KeyCode sprint;

    [Header("Play Mode Stats")]
    [SerializeField] private Vector3 currentVelocity;
    [SerializeField] private int currentHealth;
    [SerializeField] private float xSpeed;
    [SerializeField] private float ySpeed;

    void Start ()
    {
        _canMove = true;

        _maxHealth = health;
        _canTakeDmg = true;

        flickerRate = (damageOffset / flickerRepeat) / 2;
    }

    void Update()
    {
        //Movement
        CheckForDiag();

        if(_canMove)
        {
            Move();
        }

        if(!_canMove)
        {
            Debug.Log("Can't move because 'canMove' = false");
        }

        //Health

        currentHealth = health;

        if(health <= 0)
        {
            Dead();
        }
    }

    void CheckForDiag ()
    {
        if(Input.GetKey(moveUp) && Input.GetKey(moveLeft))
        {
            _canMove = false;
        }

        else if(Input.GetKey(moveUp) && Input.GetKey(moveRight))
        {
            _canMove = false;
        }

        else if(Input.GetKey(moveUp) && Input.GetKey(moveDown))
        {
            _canMove = false;
        }

        else if(Input.GetKey(moveDown) && Input.GetKey(moveRight))
        {
            _canMove = false;
        }

        else if(Input.GetKey(moveDown) && Input.GetKey(moveLeft))
        {
            _canMove = false;
        }

        else{
        _canMove = true;
        }
    }
    
    void Move ()
    {
        if(Input.GetKeyUp(moveUp) || Input.GetKeyUp(moveDown) || Input.GetKeyUp(moveLeft) || Input.GetKeyUp(moveRight))
        {
            player.isKinematic = true;
        }

        else
        {
            player.isKinematic = false;
        }

        if(Input.GetKey(moveUp) && !player.isKinematic)
        {
            if(Input.GetKey(sprint))
            {
                
                player.velocity = new Vector3(0f, 0f, sprintSpeed);
            }

            else
            {
                player.velocity = new Vector3(0f, 0f, moveSpeed);
            }
        }
        else if(Input.GetKey(moveDown) && !player.isKinematic)
        {

            if(Input.GetKey(sprint))
            {
                player.velocity = new Vector3(0f, 0f, sprintSpeed);
            }

            else
            {
                player.velocity = new Vector3(0f, 0f, moveSpeed);
            }
        }
        else if(Input.GetKey(moveLeft) && !player.isKinematic)
        {
            if(Input.GetKey(sprint))
            {
                player.velocity = new Vector3(-sprintSpeed, 0f, 0f);
            }

            else
            {
                player.velocity = new Vector3(-moveSpeed, 0f, 0f);
            }
        }
            
        else if(Input.GetKey(moveRight) && !player.isKinematic)
        {
            if(Input.GetKey(sprint))
            {
                player.velocity = new Vector3(sprintSpeed, 0f, 0f);
            }

            else
            {
                player.velocity = new Vector3(moveSpeed, 0f, 0f);
            }
        }

        xSpeed = player.velocity.x;
        ySpeed = player.velocity.y;
    }

    public void TakeDamage(int dmg)
    {
        if(_canTakeDmg)
        {
            _canTakeDmg = false;
            StartCoroutine("Dmg", dmg);
            StartCoroutine("Flicker");
        }

        else if(!_canTakeDmg)
        {
            Debug.Log("Already being damaged");
        }
    }

    void Dead ()
    {
        Debug.Log("Player dead");
    }

    IEnumerator Dmg (int d)
    {
        int tempHealth = health - d;
        health = tempHealth;

        yield return new WaitForSeconds(damageOffset);
        _canTakeDmg = true;
    }

    IEnumerator Flicker ()
    {
        MeshRenderer rend = player.gameObject.GetComponent<MeshRenderer>();

        for(int i = 0; i < flickerRepeat; i++)
        {
            rend.enabled = false;
            yield return new WaitForSeconds(flickerRate);
            rend.enabled = true;
            yield return new WaitForSeconds(flickerRate);
        }
    }
}
