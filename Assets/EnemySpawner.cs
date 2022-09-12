using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using TigerForge;

public class EnemySpawner : MonoBehaviour
{
    private bool isSpawned = false;
    private bool isRestrained = false;
    private bool isKilled = false;
    public GameObject enemyPrefab;

    public Color colactive;
    public Color colrestrained;
    private SpriteRenderer spr;

    public GameObject enemySpawned;

    [Range(7,20)]
    public float speed;

    [Range(1, 20)]
    public float TurnTIme;

    private GameObject[] sparks;

    // Start is called before the first frame update
    void Awake()
    {
        EventManager.StartListening("RESPAWN", CheckIfRespawn);
        EventManager.StartListening("RESPAWN", CheckRestrained);
        spr = GetComponent<SpriteRenderer>();
        spr.color = colactive;
        sparks = GameObject.FindGameObjectsWithTag("Sparks");
    }

    private void Start()
    {
        RespawnEnemy();
        CheckRestrained();
        DisplayRestrain();
    }

    // Update is called once per frame
    void CheckIfRespawn()
    {
            RespawnEnemy();
            CheckRestrained();
            DisplayRestrain();
    }



    void RespawnEnemy()
    {
        if(isSpawned)
        {
            Destroy(enemySpawned);
            isSpawned = false;
        }
        if(!isRestrained)
        {
            enemySpawned = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemySpawned.GetComponent<Enemy>().movespeed = speed;
            enemySpawned.GetComponent<Enemy>().TurnTime = TurnTIme;
            isKilled = false;
            isSpawned = true;
        }
       
    }

    

    private void CheckRestrained()
    {
        float mindist = 10000;
        sparks = GameObject.FindGameObjectsWithTag("Sparks");
        foreach (GameObject spark in sparks) //the line the error is pointing to
        {
            float dist = Vector2.Distance(this.transform.position, spark.transform.position);
            if(dist < mindist)
            {
                mindist = dist;
            }
        }
        if(mindist < 1f)
        {
            isRestrained = true;
        }
        else
        {
            isRestrained = false;
        }
        DisplayRestrain();
    }

    private void DisplayRestrain()
    {
        if(isRestrained)
        {
            spr.color = colrestrained;
        }
        else
        {
            spr.color = colactive;
        }
    }
}
