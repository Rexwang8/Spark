using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUi : MonoBehaviour
{
    public LevelTemplate level;
    GameObject lvtxt;
    GameObject lvimg;

    private void Awake()
    {
        lvimg = transform.GetChild(0).gameObject;
        lvtxt = transform.GetChild(1).gameObject;

        lvimg.GetComponent<Image>().sprite = level.image;
        lvtxt.GetComponent<TMP_Text>().text = $"Level {level.level}";

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
