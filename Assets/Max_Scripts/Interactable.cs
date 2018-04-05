using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : Actor {

    public bool IgnoresInteraction = false;
    public bool LogInteractEvents = true;

    public virtual bool InteractWith(Actor source, Controller instigator = null, string verb = "uses")
    {
        if(IgnoresInteraction)
        {
            return true;
        }

        if(instigator)
        {
            INTERACTLOG(instigator.name + " " + verb + " " + ActorName);
        }
        else
        {
            INTERACTLOG("Someone " + verb + " " + ActorName);
        }

        return ProcessInteraction(source, instigator);
    }

    /// <summary>
    /// This is where interactions are procesed. Called by InteractWith
    /// This is the main function to override in inheriting classes.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="instigator"></param>
    /// <returns>Return true if interaction was a success.</returns>
    protected virtual bool ProcessInteraction(Actor source, Controller instigator)
    {
        return true;
    }

    public virtual void INTERACTLOG(string s)
    {
        if(LogInteractEvents)
        {
            Debug.Log(s);
        }
    }
}
