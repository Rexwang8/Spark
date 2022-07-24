using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
public class LevelTemplate : ScriptableObject
{
    public int level;
    [PreviewField(30, ObjectFieldAlignment.Center)]
    public Sprite image;
}
