using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
public class LevelTemplate : ScriptableObject
{
    public int level;
    [PreviewField(60, ObjectFieldAlignment.Center)]
    public Sprite image;
    public Object scene;

    public Vector2 BoundingXY1;
    public Vector2 BoundingXY2;
    public Vector2 startingPosition;
    public bool lockCamera;
}
