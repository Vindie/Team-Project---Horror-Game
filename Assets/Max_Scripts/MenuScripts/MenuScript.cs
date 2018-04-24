using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    protected int activeMenuIndex = -1;
    protected int previousMenuIndex = -1;
    public GameObject[] MenuScreens;
    public int StartingMenu = 0;

    public bool PauseMenuExists = false;

    public FPS_Pawn FPSPawn;
    public float runtimeTimeScale;

    protected bool _isPaused = false;
    public bool IsPaused
    {
        get { return _isPaused; }
    }

    public Text gameText;

    private void Start()
    {
        foreach(GameObject menu in MenuScreens)
        {
            if(menu)
            {
                menu.SetActive(false);
            }
        }
        ChangeMenuTo(StartingMenu);
        runtimeTimeScale = Time.timeScale;
    }

    //MAIN MENU FUNCTIONALITY
    //
    //
    public void StartGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    //Close splash screen
    public void CloseSplash()
    {
        if(activeMenuIndex == 0)
        {
            ChangeMenuTo(1);
        }
    }

    //Navigate between menus.
    public void ChangeMenuTo(int newMenuIndex)
    {
        //Debug.Log("Previous:" + previousMenuIndex + ", Active:" + activeMenuIndex + ", New:" + newMenuIndex);
        if (newMenuIndex != activeMenuIndex)
        {
            if (0 <= activeMenuIndex && activeMenuIndex < MenuScreens.Length)
            {
                if (MenuScreens[activeMenuIndex])
                {
                    MenuScreens[activeMenuIndex].SetActive(false);
                }
            }
            if (0 <= newMenuIndex && newMenuIndex < MenuScreens.Length)
            {
                if (MenuScreens[newMenuIndex])
                {
                    MenuScreens[newMenuIndex].SetActive(true);
                }
            }

            previousMenuIndex = activeMenuIndex;
            activeMenuIndex = newMenuIndex;
        }
    }

    //Navigate to previous menu
    public void BackToPreviousMenu()
    {
        ChangeMenuTo(previousMenuIndex);
    }

    //Also used in pause menu
    public void QuitGame()
    {
        Application.Quit();
    }
    //
    //
    //PAUSE MENU FUNCTIONALITY
    //
    //
    public void ResumeGame()
    {
        ChangeMenuTo(0);
        Time.timeScale = runtimeTimeScale;
        _isPaused = false;

        if(FPSPawn)
        {
            FPSPawn.SetCursorLock(true);
        }
    }

    public void TogglePause()
    {
        if (PauseMenuExists)
        {
            if (activeMenuIndex != 1)
            {
                ChangeMenuTo(1);
                Time.timeScale = 0.0f;
                _isPaused = true;
            }
            else
            {
                ChangeMenuTo(0);
                Time.timeScale = runtimeTimeScale;
                _isPaused = false;
            }
        }
    }

    public void ReturnToMainMenu()
    {
        if(FPSPawn)
        {
            FPSPawn.SetCursorLock(false);
        }

        Time.timeScale = runtimeTimeScale;
        SceneManager.LoadScene("Scene_Menu");
    }
    //
    //
    //GAME MESSAGE FUNCTIONALITY
    //
    //
    public void SetGameText(bool setEnabled, string message)
    {
        //Make this not use ChangeMenuTo, but also not interrupt paused games.
        if(!gameText)
        {
            return;
        }

        gameText.text = message;
        if(setEnabled)
        {
            ChangeMenuTo(2);
        }
        else if(activeMenuIndex == 2)
        {
            ChangeMenuTo(previousMenuIndex);
        }
    }
}
