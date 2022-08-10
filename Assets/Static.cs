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

    public static int maxBeatenLevel = 0;
    public static int lastLevelAccessed = 0;
    public static int currentSelectedlevel;
    public static LevelTemplate levelTemplate;
    public static bool gamePaused = false;
    public static int sparkid = 0;
    public static GameObject[] sparkscripts;
    public static int MaxSparks = 30;
}
