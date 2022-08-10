using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "LevelPage", menuName = "ScriptableObjects/LevelPage", order = 1)]
public class LevelPageTemplate : ScriptableObject
{
    public int page;
    public LevelTemplate[] levels;
}
