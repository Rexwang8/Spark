using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using DTT.Utils.Extensions;
using UnityEngine.SceneManagement;

public class ESCController : MonoBehaviour
{
    GameObject settingsChild;
    GameObject dimChild;
    GameObject ContinueObject;
    GameObject ExitObject;
    GameObject EscMenuWrapper;
    public string startScene;
    public GameObject statichelper;

    private Vector2 inputDirection;
    private bool currentInput = false;

    private bool gotInput = false;
    private enum alldir { none, left, right, up, down }
    private alldir currentdir = alldir.none;

    private void Awake()
    {
        //-1 level
        EscMenuWrapper = transform.GetChild(0).gameObject;

        //-2 level
        dimChild = EscMenuWrapper.transform.GetChild(0).gameObject;
        settingsChild = EscMenuWrapper.transform.GetChild(1).gameObject;
        //-3 level
        ContinueObject = settingsChild.transform.GetChild(0).gameObject;
        ExitObject = settingsChild.transform.GetChild(1).gameObject;


        //Ini
        ContinueObject.GetComponent<TMP_Text>().text = "Continue Game".Bold();
        EscMenuWrapper.SetActive(true);
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

         if (Static.showingESC == Static.enumMenuState.main)
        {
            //select either continue or exit game

            if (Static.mainESC == Static.ESCMenuMainState.exit)
            {
                Debug.Log("Return to level");
                Static.showingESC = Static.enumMenuState.hidden;
                statichelper.GetComponent<StaticHelper>().destroySparks();
                SceneManager.LoadScene(startScene);
            }
            else if (Static.mainESC == Static.ESCMenuMainState.cont)
            {
                //Continue, unpause
                Debug.Log("Application Unpause");
                Static.showingESC = Static.enumMenuState.hidden;
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
        DoNavigateEscapeMenu(currentdir);
    }

    private void DoNavigateEscapeMenu(alldir dir)
    {
        //only work when esc menu is showing
        if (Static.showingESC != Static.enumMenuState.main)
        {
            return;
        }



        if (dir == alldir.down || dir == alldir.up)
        {

            if (Static.mainESC == Static.ESCMenuMainState.exit)
            {
                Static.mainESC = Static.ESCMenuMainState.cont;
            }
            else
            {
                Static.mainESC = Static.ESCMenuMainState.exit;
            }

            if (Static.mainESC == Static.ESCMenuMainState.exit)
            {
                //Static.mainESC = Static.ESCMenuMainState.exit;
                ExitObject.GetComponent<TMP_Text>().text = "Exit Game".Bold();
                ContinueObject.GetComponent<TMP_Text>().text = "Continue Game";


            }
            else if (Static.mainESC == Static.ESCMenuMainState.cont)
            {
                //Static.mainESC = Static.ESCMenuMainState.cont;
                ExitObject.GetComponent<TMP_Text>().text = "Exit Game";
                ContinueObject.GetComponent<TMP_Text>().text = "Continue Game".Bold();
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
        if (currentInput && !gotInput)
        {
            CalculateUIInput(inputDirection);
            gotInput = true;
        }

        //show/hide esc menu
        if (Static.showingESC == Static.enumMenuState.hidden)
        {
            settingsChild.SetActive(false);
            dimChild.SetActive(false);
            Time.timeScale = 1;
            Static.gamePaused = false;
        }
        else if (Static.showingESC == Static.enumMenuState.main)
        {
            settingsChild.SetActive(true);
            dimChild.SetActive(true);
            Time.timeScale = 0;
            Static.gamePaused = true;
        }
    }
}
