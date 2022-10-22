using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class ExitDoor : MonoBehaviour
{

    private GameObject player;
    public string MainMenuLevel;

    private float time = 2.2f;
    private bool isExiting = false;

    private Color unlockcol = new Color(0.95f, 0.95f, 0.75f);
    private Color lockcol = new Color(0.8f, 0.3f, 0.3f);
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        
        if(Static.CurrentLevelLevelTemplate.usesKeys)
        {
            transform.GetChild(0).gameObject.GetComponent<Light2D>().color = lockcol;
        }
        else
        {
            transform.GetChild(0).gameObject.GetComponent<Light2D>().color = unlockcol;
        }
        
    }

    private void Update()
    {
        
        if (Static.CurrentLevelLevelTemplate.usesKeys && Static.CurrentLevelLevelTemplate.numKeys != Static.numKeysObtained)
        {
            transform.GetChild(0).gameObject.GetComponent<Light2D>().color = lockcol;
        }
        else
        {
            transform.GetChild(0).gameObject.GetComponent<Light2D>().color = unlockcol;
        }
        
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(isExiting)
            {
                return;
            }

            //check if keys
            if(Static.CurrentLevelLevelTemplate.usesKeys && Static.CurrentLevelLevelTemplate.numKeys != Static.numKeysObtained) 
            {
                return;
            }
            EventManager.EmitEvent("FINISHLEVEL");
            EventManager.EmitEvent("BURSTLIGHT");
            EventManager.EmitEvent("AUDIOEND");

            if (Static.maxBeatenLevel < Static.CurrentLevelLevelTemplate.level)
            {
                Static.maxBeatenLevel = Static.CurrentLevelLevelTemplate.level;
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
