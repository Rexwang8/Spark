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
    GameObject EscMenuWrapper;
    GameObject FlameObject;


    public GameObject startText;

    private Vector2 inputDirection;
    private bool currentInput = false;

    private bool gotInput = false;
    private enum alldir { none, left, right, up, down }
    private alldir currentdir = alldir.none;

    private void Awake()
    {
        //-1 level
        EscMenuWrapper = transform.GetChild(1).gameObject;

        
        //-2 level
        dimChild = EscMenuWrapper.transform.GetChild(0).gameObject;
        settingsChild = EscMenuWrapper.transform.GetChild(1).gameObject;

        //-3 level
        ContinueObject = settingsChild.transform.GetChild(0).gameObject;
        ExitObject = settingsChild.transform.GetChild(1).gameObject;
        mainmenuObject = settingsChild.transform.GetChild(2).gameObject;
        FlameObject = settingsChild.transform.GetChild(3).gameObject;

        //Ini
        ContinueObject.GetComponent<TMP_Text>().text = "Continue Game".Bold();
        FlameObject.transform.localPosition = new Vector3(-180, 20, 0);
        EscMenuWrapper.SetActive(true);

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
                FlameObject.transform.localPosition = new Vector3(-180, -200, 0);


            }
            else if(Static.mainESC == Static.ESCMenuMainState.cont)
            {
                //Static.mainESC = Static.ESCMenuMainState.cont;
                ExitObject.GetComponent<TMP_Text>().text = "Exit Game";
                ContinueObject.GetComponent<TMP_Text>().text = "Continue Game".Bold();
                mainmenuObject.GetComponent<TMP_Text>().text = "Exit to Main Menu";
                FlameObject.transform.localPosition = new Vector3(-180, 20, 0);
            }
            else
            {
                ExitObject.GetComponent<TMP_Text>().text = "Exit Game";
                ContinueObject.GetComponent<TMP_Text>().text = "Continue Game";
                mainmenuObject.GetComponent<TMP_Text>().text = "Exit to Main Menu".Bold();
                FlameObject.transform.localPosition = new Vector3(-180, -90, 0);
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
        if (Static.showingESC == Static.enumMenuState.hidden)
        {
            if (Static.currentMainState == Static.enumMainState.start)
            {
                //try to start game scene

                Static.currentMainState = Static.enumMainState.levelselect;
            }
            else if (Static.currentMainState == Static.enumMainState.levelselect)
            {
                //start selected level
                Debug.Log($"Start this level: { Static.CurrentLevelLevelTemplate.level}");

                Static.lastLevelAccessed = Static.CurrentLevelLevelTemplate.level;


                SceneManager.LoadScene(Static.CurrentLevelLevelTemplate.scenename);
            }

        }
        else if (Static.showingESC == Static.enumMenuState.main)
        {
            //select either continue or exit game

            if (Static.mainESC == Static.ESCMenuMainState.exit)
            {
                Debug.Log("Application Quit");
                Application.Quit();
            }
            else if (Static.mainESC == Static.ESCMenuMainState.cont)
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
}
