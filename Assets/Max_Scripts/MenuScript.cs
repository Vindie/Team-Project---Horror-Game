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

    protected bool _isPaused = false;
    public bool isPaused
    {
        get { return _isPaused; }
    }

    public Text gameText;

    private void Start()
    {
        ChangeMenuTo(StartingMenu);
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
        Debug.Log("Previous:" + previousMenuIndex + ", Active:" + activeMenuIndex + ", New:" + newMenuIndex);
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
        _isPaused = false;
    }

    public void TogglePause()
    {
        if (PauseMenuExists)
        {
            if (activeMenuIndex != 1)
            {
                ChangeMenuTo(1);
                _isPaused = true;
            }
            else
            {
                ChangeMenuTo(previousMenuIndex);
                _isPaused = false;
            }
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Scene_Menu");
    }
    //
    //
    //GAME MESSAGE FUNCTIONALITY
    //
    //
    public void SetGameText(bool setEnabled, string message)
    {
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
