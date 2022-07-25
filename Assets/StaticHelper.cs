using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTT.Utils.Extensions;
using UnityEngine.InputSystem;

public class StaticHelper : MonoBehaviour
{
    public LevelTemplate startLevelTemplate;
    private void Awake()
    {
        Static.levelTemplate = startLevelTemplate;
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
}
