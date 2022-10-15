using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{

    private GameObject player;
    public string MainMenuLevel;

    private float time = 2.5f;
    private bool isExiting = false;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(isExiting)
            {
                return;
            }
            EventManager.EmitEvent("FINISHLEVEL");
            EventManager.EmitEvent("BURSTLIGHT");
            EventManager.EmitEvent("AUDIOEND");

            if (Static.maxBeatenLevel < Static.levelTemplate.level)
            {
                Static.maxBeatenLevel = Static.levelTemplate.level;
            }

            //Debug.Log($"Start this level: {Static.currentSelectedlevel}");
            StartCoroutine(WaitDoor());
            isExiting = true;
         //   SceneManager.LoadScene(MainMenuLevel);
        }
    }

    IEnumerator WaitDoor()
    {
        //Print the time of when the function is first called.
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        Static.disablePlayerMovement = true;
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(MainMenuLevel);
        //After we have waited 5 seconds print the time again.
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}
