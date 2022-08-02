using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{

    public GameObject light1;
    public GameObject light2;

    public void Awake()
    {
        light1.SetActive(false);
        light2.SetActive(true);
    }
    public void OnDebug(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            light1.SetActive(Static.debugMode);
            light2.SetActive(!Static.debugMode);
        }

    }
}
