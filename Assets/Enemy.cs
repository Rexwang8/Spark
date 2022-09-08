using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class Enemy : MonoBehaviour
{
    public enum dir {left, right }

    public dir cdir = dir.left;

    private Rigidbody2D rb;

    [Range(1, 20)]
    public float movespeed = 8;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            EventManager.EmitEvent("KILL");
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = Vector2.left * Time.deltaTime * 100 * movespeed;
        rb.AddForce(movement);
        Debug.Log("moveing enemy" + movement);
    }
}
