using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorGame : Game {

    public float GameEndLingerTime = 5.0f;
    public float FadeTime = 10.0f;

    public string loseText = "Y  o  u    h  a  v  e    d  i  e  d  .";   //A E S T H E T I C
    public string winText = "Y  o  u    h  a  v  e    e  s  c  a  p  e  d    t  h  e    m  i  n  e  .";

    public GameObject keyPrefab;

    protected MenuScript _ms;
    protected Key_Spawn[] _keyPotentialSpawns;

    public override void Awake()
    {
        base.Awake();
        _ms = FindObjectOfType<MenuScript>();
        RefreshKey_SpawnList();
        SpawnTheKeyAt(GetRandomKey_Spawn());
    }

    protected virtual void Update()
    {
        if (!_ms)
        {
            _ms = FindObjectOfType<MenuScript>();
        }
    }

    //Returns false if game doesn't end
    public virtual bool EndGame(bool victory)
    {
        LOG("Game ending in " + GameEndLingerTime + " seconds.");
        if (_ms)
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
            if (cam)
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
        while (countDown > 0)
        {
            countDown -= Time.deltaTime;
            yield return null;
        }

        if (_ms)
        {
            _ms.ReturnToMainMenu();
        }
    }

    #region Key Spawning
    protected void SpawnTheKeyAt(Transform keySpawnPosition)
    {
        if(!keyPrefab)
        {
            LOG_ERROR("HorrorGame: Missing Key Prefab");
            return;
        }

        GameObject key = Factory(keyPrefab, transform.position, transform.rotation);
        if(!key) { LOG_ERROR("HorrorGame: Key failed to spawn."); }
    }

    void RefreshKey_SpawnList()
    {
        _keyPotentialSpawns = FindObjectsOfType<Key_Spawn>();
    }

    Transform GetRandomKey_Spawn()
    {
        if (_keyPotentialSpawns == null) { RefreshKey_SpawnList(); }

        int index = (int)(Random.value * _keyPotentialSpawns.Length);

        if (_keyPotentialSpawns.Length == index) { index = 0; }
        
        return _keyPotentialSpawns[index].transform;
    }
    #endregion
}
