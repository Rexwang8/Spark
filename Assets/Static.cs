using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Sirenix.OdinInspector;
using TMPro;

public static class Static
{
    public static bool debugMode = false;

    public enum enumMenuState { hidden, main, settings }
    public static enumMenuState showingESC = enumMenuState.hidden;
    public enum enumMainState { start, levelselect }
    public static enumMainState currentMainState = enumMainState.start;
    public enum ESCMenuMainState { cont, main, exit}
    public static ESCMenuMainState mainESC = ESCMenuMainState.cont;

    public static int maxBeatenLevel = 5;
    public static int lastLevelAccessed = 0;
    public static int currentSelectedlevel;

    //clevel level template
    public static LevelTemplate CurrentLevelLevelTemplate;
    public static bool gamePaused = false;
    public static int sparkid = 0;
    public static GameObject[] sparkscripts;
    public static int MaxSparks = 30;

    public static bool disablePlayerMovement = false;
    public static int numKeysObtained = 0;

    //EVENTS
    /* FINISHLEVEL - emit when level is finished right when door is touched
     * BURSTLIGHT - emit when you need lights to burst
     * AUDIOSTART - emit to start music
     * AUDIOEND - emit to end music
     * 
     * 
     * 
     */
}
