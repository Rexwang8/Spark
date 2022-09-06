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

    // Start is called before the first frame update
    void Awake()
    {
        EventManager.StartListening("RESPAWN", CheckIfRespawn);
        spr = GetComponent<SpriteRenderer>();
        spr.color = colactive;
    }

    private void Start()
    {
        RespawnEnemy();
    }

    // Update is called once per frame
    void CheckIfRespawn()
    {
        if(true)
        {
            
            
            RespawnEnemy();
        }
    }

    void RespawnEnemy()
    {
        Debug.Log("Respawn Enemy");
        if(isSpawned)
        {
            Destroy(enemySpawned);
            isSpawned = false;
        }
        enemySpawned = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        isKilled = false;
        isSpawned = true;
    }

    

    void CheckRestrained()
    {

    }
}
