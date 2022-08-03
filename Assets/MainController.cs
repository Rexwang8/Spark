using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using DTT.Utils.Extensions;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    GameObject settingsChild;
    GameObject dimChild;
    GameObject mainmenuObject;
    GameObject ContinueObject;
    GameObject ExitObject;
    GameObject levelSelectChild;
    GameObject levelSelectWrapper;
    GameObject EscMenuWrapper;

    
    public List<GameObject> allLevels = new List<GameObject>();
    private int amountOfLevels;
    public LevelTemplate CurrentlySelectedLevel; 


    public GameObject startText;

    public Object nextScene;

    private Vector2 inputDirection;
    private bool currentInput = false;

    private bool gotInput = false;
    private enum alldir { none, left, right, up, down }
    private alldir currentdir = alldir.none;

    private void Awake()
    {
        //-1 level
        EscMenuWrapper = transform.GetChild(1).gameObject;
        levelSelectWrapper = transform.GetChild(0).gameObject;
        
        //-2 level
        dimChild = EscMenuWrapper.transform.GetChild(0).gameObject;
        settingsChild = EscMenuWrapper.transform.GetChild(1).gameObject;
        levelSelectChild = levelSelectWrapper.transform.GetChild(0).gameObject;
        //-3 level
        ContinueObject = settingsChild.transform.GetChild(0).gameObject;
        ExitObject = settingsChild.transform.GetChild(1).gameObject;
        mainmenuObject = settingsChild.transform.GetChild(2).gameObject;


        //Ini
        ContinueObject.GetComponent<TMP_Text>().text = "Continue Game".Bold();
        EscMenuWrapper.SetActive(true);
        levelSelectWrapper.SetActive(true);
        amountOfLevels = levelSelectChild.transform.childCount - 1;
        for (int i = 1; i < amountOfLevels + 1; i++)
        {
            allLevels.Add(levelSelectChild.transform.GetChild(i).gameObject);
        }
        Static.currentSelectedlevel = 1;
        CurrentlySelectedLevel = allLevels[0].GetComponent<LevelUi>().level;
        allLevels[Static.currentSelectedlevel - 1].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = $"Level {CurrentlySelectedLevel.level}".Bold();
    }
    public void OnEnter(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            TrySubmitButton();
        }

    }

    private void TrySubmitButton()
    {
        Debug.Log("ESC");
        if(Static.showingESC == Static.enumMenuState.hidden)
        {
            if (Static.currentMainState == Static.enumMainState.start)
            {
                //try to start game scene
                //Debug.Log("Trying scene start");
                // SceneManager.LoadScene(nextScene.name);
                Static.currentMainState = Static.enumMainState.levelselect;
            }
            else if(Static.currentMainState == Static.enumMainState.levelselect)
            {
                //start selected level
                Debug.Log($"Start this level: {CurrentlySelectedLevel.level}");
                Static.levelTemplate = CurrentlySelectedLevel;
                SceneManager.LoadScene(CurrentlySelectedLevel.scene.name);
            }
            
        }
        else if(Static.showingESC == Static.enumMenuState.main)
        {
            //select either continue or exit game
            
            if(Static.mainESC == Static.ESCMenuMainState.exit)
            {
                Debug.Log("Application Quit");
                Application.Quit();
            }
            else if(Static.mainESC == Static.ESCMenuMainState.cont)
            {
                //Continue, unpause
                Debug.Log("Application Unpause");
                Static.showingESC = Static.enumMenuState.hidden;
            }
            else
            {
                Debug.Log("Application Unpause and Main");
                Static.showingESC = Static.enumMenuState.hidden;
                Static.currentMainState = Static.enumMainState.start;
            }
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

    private void CalculateUIInput(Vector2 input)
    {
        //vertical input
        if(Mathf.Abs(input.x) < 0.3f && Mathf.Abs(input.y) > 0.3f)
        {
            if(input.y > 0.3f)
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
        DoNavigateEscapeMenu(currentdir);
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
            Static.currentSelectedlevel -= 1;
            if (Static.currentSelectedlevel <= 0 )
            {
                Static.currentSelectedlevel = 1;
            }
        }
        else
        {

            Static.currentSelectedlevel += 1;
            if (Static.currentSelectedlevel > allLevels.Count)
            {
                Static.currentSelectedlevel = allLevels.Count;
            }

            
            //LOGIC CHECK LOCKED LEVELS HERE -- TODO
            if(Static.currentSelectedlevel > Static.maxBeatenLevel + 1)
            {
                Static.currentSelectedlevel = Static.maxBeatenLevel;
            }
            Debug.Log("clevl" + Static.currentSelectedlevel + " maxlevl " + Static.maxBeatenLevel);
        }

        //Select level
        if(Static.currentSelectedlevel == prevlevel)
        {
            return;
        }
        CurrentlySelectedLevel = allLevels[Static.currentSelectedlevel - 1].GetComponent<LevelUi>().level;
        //MOVE UI TO MATCH LEVEL
        
        //Highlight/bold selected level
        if(Static.currentSelectedlevel > 1)
        {
            allLevels[Static.currentSelectedlevel - 2].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = $"Level {allLevels[Static.currentSelectedlevel - 2].GetComponent<LevelUi>().level.level}";
        }
        if (Static.currentSelectedlevel < allLevels.Count)
        {
            allLevels[Static.currentSelectedlevel].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = $"Level {allLevels[Static.currentSelectedlevel].GetComponent<LevelUi>().level.level}";
        }
        
        allLevels[Static.currentSelectedlevel - 1].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = $"Level {CurrentlySelectedLevel.level}".Bold();
        

    }

    private void DoNavigateEscapeMenu(alldir dir)
    {
        //only work when esc menu is showing
        if(Static.showingESC != Static.enumMenuState.main)
        {
            return;
        }



        if(dir == alldir.down || dir == alldir.up)
        {

            if(dir == alldir.up)
            {
                Static.mainESC = Static.mainESC.Previous();
            }
            else
            {
                Static.mainESC = Static.mainESC.Next();
            }

            if (Static.mainESC == Static.ESCMenuMainState.exit)
            {
                //Static.mainESC = Static.ESCMenuMainState.exit;
                ExitObject.GetComponent<TMP_Text>().text = "Exit Game".Bold();
                ContinueObject.GetComponent<TMP_Text>().text = "Continue Game";
                mainmenuObject.GetComponent<TMP_Text>().text = "Exit to Main Menu";
                
               
            }
            else if(Static.mainESC == Static.ESCMenuMainState.cont)
            {
                //Static.mainESC = Static.ESCMenuMainState.cont;
                ExitObject.GetComponent<TMP_Text>().text = "Exit Game";
                ContinueObject.GetComponent<TMP_Text>().text = "Continue Game".Bold();
                mainmenuObject.GetComponent<TMP_Text>().text = "Exit to Main Menu";
            }
            else
            {
                ExitObject.GetComponent<TMP_Text>().text = "Exit Game";
                ContinueObject.GetComponent<TMP_Text>().text = "Continue Game";
                mainmenuObject.GetComponent<TMP_Text>().text = "Exit to Main Menu".Bold();
            }


            if (Static.debugMode)
            {
                Debug.Log(Static.mainESC);
            }
        }
        
    }

    public void OnNav(InputAction.CallbackContext context)
    {

        inputDirection = context.ReadValue<Vector2>().normalized;
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovementInput();
        if(currentInput && !gotInput)
        {
            CalculateUIInput(inputDirection);
            gotInput = true;
        }

        //show/hide esc menu
        if (Static.showingESC == Static.enumMenuState.hidden)
        {
            settingsChild.SetActive(false);
            dimChild.SetActive(false);
        }
        else if(Static.showingESC == Static.enumMenuState.main)
        {
            settingsChild.SetActive(true);
            dimChild.SetActive(true);
        }

        if(Static.currentMainState == Static.enumMainState.levelselect)
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
}
