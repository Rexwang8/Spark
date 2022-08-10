using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using DTT.Utils.Extensions;
using UnityEngine.SceneManagement;

public class NavigateLevelSelect : MonoBehaviour
{

    private Vector2 inputDirection;
    private bool currentInput = false;

    private bool gotInput = false;
    private enum alldir { none, left, right, up, down }
    private alldir currentdir = alldir.none;

    public List<GameObject> allLevels = new List<GameObject>();
    private int amountOfLevels;

    GameObject levelSelectChild;
    GameObject levelSelectWrapper;
    public GameObject startText;

    private void Awake()
    {
        //-1 level
        levelSelectWrapper = transform.GetChild(0).gameObject;

        //-2 level
        levelSelectChild = levelSelectWrapper.transform.GetChild(0).gameObject;

        //Ini
        levelSelectWrapper.SetActive(true);
        amountOfLevels = 5;
        for (int i = 0; i < amountOfLevels; i++)
        {
            allLevels.Add(levelSelectChild.transform.GetChild(1).GetChild(i).gameObject);
        }
        for (int i = 0; i < amountOfLevels; i++)
        {
            allLevels.Add(levelSelectChild.transform.GetChild(2).GetChild(i).gameObject);
        }

        Static.currentSelectedlevel = Static.lastLevelAccessed;
        calcTemplateFromIdx();
        boldLevelTxt();
    }

    private void CalcLevelIdxDown()
    {

        Static.currentSelectedlevel -= 1;
        if (Static.currentSelectedlevel <= 0)
        {
            Static.currentSelectedlevel = 0;
        }
        boldLevelTxt();
    }
    private void CalcLevelIdxUp()
    {
        Static.currentSelectedlevel += 1;
        if (Static.currentSelectedlevel > allLevels.Count || Static.currentSelectedlevel > Static.maxBeatenLevel)
        {
            Static.currentSelectedlevel -= 1;
        }
        boldLevelTxt();
    }

    private void calcTemplateFromIdx()
    {
        Static.levelTemplate = allLevels[Static.currentSelectedlevel].GetComponent<LevelUi>().level;
    }


    private void CalculateUIInput(Vector2 input)
    {
        //vertical input
        if (Mathf.Abs(input.x) < 0.3f && Mathf.Abs(input.y) > 0.3f)
        {
            if (input.y > 0.3f)
            {
                currentdir = alldir.up;
            }
            else
            {
                currentdir = alldir.down;
            }
        }
        //horiz input
        else if (Mathf.Abs(input.y) < 0.3f && Mathf.Abs(input.x) > 0.3f)
        {
            if (input.x > 0.3f)
            {
                currentdir = alldir.right;
            }
            else
            {
                currentdir = alldir.left;
            }
        }

        if (Static.debugMode)
        {
            Debug.Log(currentdir);
        }

        DoNavigateLevelSelect(currentdir);
    }

    private void DoNavigateLevelSelect(alldir dir)
    {
        //only work when esc menu is hidden and on level select menu
        if (Static.showingESC != Static.enumMenuState.hidden || Static.currentMainState != Static.enumMainState.levelselect)
        {
            return;
        }
        int prevlevel = Static.currentSelectedlevel;

        //navigate
        if (dir == alldir.left)
        {
            CalcLevelIdxDown();
        }
        else
        {

            CalcLevelIdxUp();
        }

        //Select level
        if (Static.currentSelectedlevel == prevlevel)
        {
            return;
        }

        calcTemplateFromIdx();


    }
    private void boldLevelTxt()
    {
        if (Static.currentSelectedlevel >= 1)
        {
            allLevels[Static.currentSelectedlevel - 1].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = $"Level {allLevels[Static.currentSelectedlevel - 1].GetComponent<LevelUi>().level.level}";
        }
        if (Static.currentSelectedlevel < allLevels.Count)
        {
            allLevels[Static.currentSelectedlevel + 1].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = $"Level {allLevels[Static.currentSelectedlevel + 1].GetComponent<LevelUi>().level.level}";
        }

        allLevels[Static.currentSelectedlevel].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = $"Level {allLevels[Static.currentSelectedlevel].GetComponent<LevelUi>().level.level}".Bold();
    }
    public void OnNav(InputAction.CallbackContext context)
    {

        inputDirection = context.ReadValue<Vector2>().normalized;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Current: " + Static.currentSelectedlevel + "  Max" + Static.maxBeatenLevel);
        CalculateMovementInput();
        if (currentInput && !gotInput)
        {
            CalculateUIInput(inputDirection);
            gotInput = true;
        }


        if (Static.currentMainState == Static.enumMainState.levelselect)
        {
            startText.SetActive(false);
            levelSelectChild.SetActive(true);
        }
        else
        {
            startText.SetActive(true);
            levelSelectChild.SetActive(false);
        }
    }

    void CalculateMovementInput()
    {
        if (inputDirection == Vector2.zero)
        {
            currentInput = false;
            gotInput = false;
        }
        else if (inputDirection != Vector2.zero)
        {

            currentInput = true;
        }
    }
}
