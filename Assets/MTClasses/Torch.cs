using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : Item
{
    public GameObject firePrefab;
    public bool startActive = true;
    public float lifeSpan;
    public int maxLifeSpan = 100;
    int fallingVelocity = 1000000;

    public bool getLit; //refers to if player has relit the torch after the torch died

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
        lifeSpan = maxLifeSpan * Time.deltaTime;
        getLit = false;

        if (startActive)
        {
            LightOn();
        }
        else
        {
            LightOff();
        }

    }

    public void Update()
    {
        if (lifeSpan == maxLifeSpan)
        {
            LightOn();
        }

        if(lifeSpan > 0)
        {
            torchDying();
        }
    }
    public void LightOn()
    {
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
        if (beingHeld || getLit)
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
            lifeSpan--;
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
