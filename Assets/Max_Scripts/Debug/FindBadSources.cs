using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindBadSources : MonoBehaviour {

    List<AudioSource> badSources;
    public bool refreshSources = false;
    public bool giveSourceNames = false;
    public bool giveSourceCoords = false;
    public int numberOfBadSources;

    private void Start()
    {
        badSources = new List<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
		if(refreshSources)
        {
            UpdateList();
            refreshSources = false;
        }
        if(giveSourceNames)
        {
            LogSourceNames();
            giveSourceNames = false;
        }
        if(giveSourceCoords)
        {
            LogSourcePosition();
            giveSourceCoords = false;
        }

        numberOfBadSources = badSources.Count;
	}

    void UpdateList()
    {
        badSources.Clear();
        AudioSource[] allSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource src in allSources)
        {
            if (src.loop == false || src.spatialBlend != 1.0f)
            {
                badSources.Add(src);
            }
        }
    }

    void LogSourceNames()
    {
        foreach(AudioSource src in badSources)
        {
            Debug.Log("Name: " + src.name);
        }
    }

    void LogSourcePosition()
    {
        foreach (AudioSource src in badSources)
        {
            Debug.Log("Position: " + src.transform.position);
        }
    }
}
