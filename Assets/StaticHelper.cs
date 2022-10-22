using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTT.Utils.Extensions;
using UnityEngine.InputSystem;
using TigerForge;

public class StaticHelper : MonoBehaviour
{
    public LevelTemplate startLevelTemplate;
    private void Awake()
    {
        if(Static.CurrentLevelLevelTemplate == null)
        {
            Static.CurrentLevelLevelTemplate = startLevelTemplate;
            Static.lastLevelAccessed = 1;
        }
        
    }
    public void ToggleDebugMode(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Static.debugMode = !Static.debugMode;
            Debug.Log("Switching Debug Mode".Bold());
        }

    }

    public void onMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ToggleMenuState();
            
        }
    }

    public void ToggleMenuState()
    {
        if(Static.showingESC == Static.enumMenuState.main)
        {
            Static.showingESC = Static.enumMenuState.hidden;
        }
        else
        {
            Static.showingESC = Static.enumMenuState.main;
        }
        Debug.Log($"Switching Esc Menu State: {Static.showingESC}".Bold());
    }

    public void getSparks()
    {
        Static.sparkscripts = GameObject.FindGameObjectsWithTag("Sparks");
    }

    public int getSparksCT()
    {
        return GameObject.FindGameObjectsWithTag("Sparks").Length;
    }


    public void checkSparks()
    {
        getSparks();
        for (int i = 0; i < Static.sparkscripts.Length; i++)
        {
            Static.sparkscripts[i].GetComponent<SparkCT>().checkSparks();
        }
    }

    public void destroySparks()
    {
        getSparks();
        for (int i = 0; i < Static.sparkscripts.Length; i++)
        {
            Static.sparkscripts[i].GetComponent<SparkCT>().destroySparks();
        }
    }
}
