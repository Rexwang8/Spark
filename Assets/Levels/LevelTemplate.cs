using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
public class LevelTemplate : ScriptableObject
{
    [TitleGroup("Base")]
    public int level;
    [PreviewField(60, ObjectFieldAlignment.Center)]
    public Sprite image;
    public string scenename;
    [ShowInInspector]
    [TextArea]
    private string desc;

    [TitleGroup("Level Info")]
    public Vector2 BoundingXY1;
    public Vector2 BoundingXY2;
    public Vector2 startingPosition;
    public bool lockCamera;

    public GameObject levelTerrain;

    [TitleGroup("Text")]
    public bool UsingUIText;
    [TextArea]
    public string UIText;
    public Vector2 UITextCoords;
}
