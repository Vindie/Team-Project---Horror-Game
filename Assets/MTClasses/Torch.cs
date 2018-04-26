using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : Item
{
    public GameObject firePrefab;
    public bool startActive = true;
    public float lifeSpan;
    public int maxLifeSpan = 100;
    public float lifeKeptPercentOnRelight = 0.75f;
    int fallingVelocity = 1000000;

    public bool getLit; //refers to if player has relit the torch after the torch died
    protected bool _hasBeenHeld = false;

    /*
     * Lifespan for torch
     * starts to decrease when grabbed by player
     * or when relit
     * when dropped torch goes out (detriment to lifespan)
     * if relit lifespan = maxLifeSpan
     * */

    public bool LightActive
    {
        get { return _lightActive; }
    }
    protected bool _lightActive;

	protected override void Start ()
    {
        if (startActive)
        {
            LightOn();
        }
        else
        {
            LightOff();
        }

        getLit = false;
    }

    public void FixedUpdate()
    {
        if(lifeSpan > 0)
        {
            torchDying();
        }
        else if(_lightActive)
        {
            LightOff();
        }

        if(beingHeld)
        {
            _hasBeenHeld = true;
        }
    }
    public void LightOn()
    {
        lifeSpan = maxLifeSpan;
        maxLifeSpan = (int)(maxLifeSpan * lifeKeptPercentOnRelight);

        firePrefab.SetActive(true);
        _lightActive = true;
    }

    public void LightOff()
    {
        firePrefab.SetActive(false);
        _lightActive = false;
    }

    public bool lifeSpanDecreasingCheck()
    {
        if ((_hasBeenHeld || getLit) && _lightActive)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void torchDying()
    {
        if(lifeSpanDecreasingCheck())
        {
            lifeSpan -= Time.fixedDeltaTime;
        }
    }

    /*
     *     private void OnCollisionEnter(Collision col)
    {
        //print("Being Held: " + beingHeld);
        //print(lifeSpanDecreasingCheck());
        if(lifeSpanDecreasingCheck())
        {
            if( col.relativeVelocity.y >= fallingVelocity)
            {
                LightOff();
            }
        }
    }
     * */


}
