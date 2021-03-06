﻿using System.Collections;
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

    protected GameManager _gm;
    public int settingsMenuIndex = -1;

    //FPSPawn is set from FPS_Controller Start() function
    public FPS_Pawn FPSPawn;
    public float runtimeTimeScale;

    protected bool _isPaused = false;
    public bool IsPaused
    {
        get { return _isPaused; }
    }

    public Text gameLargeText;
    bool glt_active = false;
    float glt_activeTimer = 0.0f;
    public Text gameSmallText;
    bool gst_active = false;
    float gst_activeTimer = 0.0f;

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

        if (glt_activeTimer > 0.0f)
        {
            glt_activeTimer -= Time.deltaTime;
            if (glt_activeTimer <= 0.0f)
            {
                SetGameLargeText(false);
            }
        }
        if (gst_activeTimer > 0.0f)
        {
            gst_activeTimer -= Time.deltaTime;
            if(gst_activeTimer <= 0.0f)
            {
                SetGameSmallText(false);
            }
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
            if(activeMenuIndex == settingsMenuIndex)
            {
                //Settings menu is closing: save settings data
                if(!_gm)
                {
                    _gm = FindObjectOfType<GameManager>();
                }
                if(_gm)
                {
                    _gm.SaveSettings();
                }
            }
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

    //Reset game settings to defaults
    public void ResetSettings()
    {
        if (!_gm)
        {
            _gm = FindObjectOfType<GameManager>();
        }
        if (_gm)
        {
            _gm.gameSettings.ResetToDefaultSettings();
            Debug.Log("Settings reset to: " + _gm.gameSettings.Brightness + ", " + _gm.gameSettings.MouseSensitivity + ", " + _gm.gameSettings.Volume);
        }

        MenuSlider[] msCollection = FindObjectsOfType<MenuSlider>();
        foreach(MenuSlider ms in msCollection)
        {
            ms.SliderCheckValue();
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
    public void SetGameLargeText(bool setEnabled, string message = "", float time = 0.0f)
    {
        if(!gameLargeText) { return; }

        gameLargeText.text = message;

        glt_active = setEnabled;
        if(setEnabled && time > 0.0f)
        {
            glt_activeTimer = time;
        }
    }

    public void SetGameSmallText(bool setEnabled, string message = "", float time = 0.0f)
    {
        if (!gameSmallText) { return; }

        gameSmallText.text = message;

        gst_active = setEnabled;
        if (setEnabled && time > 0.0f)
        {
            gst_activeTimer = time;
        }
    }
}
