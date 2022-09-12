using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class Enemy : MonoBehaviour
{
    public enum dir {left, right }

    public dir cdir = dir.left;

    private Rigidbody2D rb;

    [Range(0, 20)]
    public float TurnTime = 2;
    private float ctime = 0;

    [Range(1, 20)]
    public float movespeed = 8;

    private GameObject player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        ctime = TurnTime / 2;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EventManager.EmitEvent("KILL");
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement;
        if(cdir == dir.left)
        {
            movement = Vector2.left * Time.deltaTime * 100 * movespeed;
        }
        else
        {
            movement = Vector2.right * Time.deltaTime * 100 * movespeed;
        }
        
        rb.AddForce(movement);
    }

    private void Update()
    {
        ctime += Time.deltaTime;
        if(ctime > TurnTime)
        {
            ctime = 0;
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
            if(cdir == dir.left)
            {
                cdir = dir.right;
            }
            else
            {
                cdir = dir.left;
            }
        }

        

    }
}
