using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorGame : Game {

    public float GameEndLingerTime = 5.0f;
    public float FadeTime = 10.0f;
    
    public string loseText = "You have died.";
    public string winText = "You have escaped the mine.";
    protected MenuScript _ms;

    public override void Awake()
    {
        base.Awake();
        _ms = FindObjectOfType<MenuScript>();
    }

    protected virtual void Update()
    {
        if(!_ms)
        {
            _ms = FindObjectOfType<MenuScript>();
        }
    }

    //Returns false if game doesn't end
    public virtual bool EndGame(bool victory)
    {
        LOG("Game ending in " + GameEndLingerTime + " seconds.");
        if(_ms)
        {
            if (victory)
            {
                _ms.SetGameLargeText(true, winText);
            }
            else
            {
                _ms.SetGameLargeText(true, loseText);
            }

            ImageEffectManager cam = FindObjectOfType<ImageEffectManager>();
            if(cam)
            {
                cam.CrossFadeBlack(true, FadeTime);
            }
            StartCoroutine(LingerBeforeSceneChange());
            return true;
        }
        return false;
    }

    protected virtual IEnumerator LingerBeforeSceneChange()
    {
        float countDown = GameEndLingerTime;
        while(countDown > 0)
        {
            countDown -= Time.deltaTime;
            yield return null;
        }
        
        if(_ms)
        {
            _ms.ReturnToMainMenu();
        }
    }
}
