using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class KillPlayerBlock : MonoBehaviour
{

    public GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            EventManager.EmitEvent("KILL");
            Debug.Log("EMIT KILL");
        }
    }


}
