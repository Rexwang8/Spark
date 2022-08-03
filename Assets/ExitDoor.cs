using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{

    private GameObject player;
    public Object MainMenuLevel;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            EventManager.EmitEvent("FINISHLEVEL");
            Static.maxBeatenLevel = Static.levelTemplate.level;
            Debug.Log($"Start this level: {Static.currentSelectedlevel}");
            SceneManager.LoadScene(MainMenuLevel.name);
            Debug.Log("EMIT FINISHLEVEL" + Static.levelTemplate.level);
        }
    }


}
