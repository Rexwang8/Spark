using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTT.Utils.Extensions;
using UnityEngine.InputSystem;

public class StaticHelper : MonoBehaviour
{
    public void ToggleDebugMode(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Static.debugMode = !Static.debugMode;
            Debug.Log("Switching Debug Mode".Bold());
        }

    }
}
