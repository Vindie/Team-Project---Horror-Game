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

    public Text gameLargeText;
    bool glt_active = false;
    public Text gameSmallText;
    bool gst_active = false;

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

    private void Update()
    {
        if(gameLargeText)
        {
            if (glt_active && !_isPaused)   { gameLargeText.gameObject.SetActive(true); }
            else                            { gameLargeText.gameObject.SetActive(false); }
        }

        if(gameSmallText)
        {
            if (gst_active && !_isPaused)   { gameSmallText.gameObject.SetActive(true); }
            else                            { gameSmallText.gameObject.SetActive(false); }
        }
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
    public void SetGameLargeText(bool setEnabled, string message = "")
    {
        if(!gameLargeText) { return; }

        gameLargeText.text = message;

        glt_active = setEnabled;
    }

    public void SetGameSmallText(bool setEnabled, string message = "")
    {
        if (!gameSmallText) { return; }

        gameSmallText.text = message;

        gst_active = setEnabled;
    }
}
