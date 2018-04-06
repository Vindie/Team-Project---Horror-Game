using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socket : MonoBehaviour {

    public virtual bool HasItem
    {
        get { return equippedItem; }
    }

    protected Item equippedItem;
    protected Collider itemCol;
    protected Rigidbody itemRB;
    protected bool[] rbSettings = { true, false, true };

    protected virtual void Update()
    {
        if (HasItem)
        {
            equippedItem.transform.position = transform.position;
            equippedItem.transform.rotation = transform.rotation;
        }
    }

    public virtual bool Equip(Item item)
    {
        if (HasItem || !item)
        {
            return false;
        }

        equippedItem = item;

        //Check for and disable collision collider.
        Collider[] colliders = item.GetComponents<Collider>();
        foreach(Collider col in colliders)
        {
            if(!col.isTrigger)
            {
                itemCol = col;
            }
        }
        if(itemCol)
        {
            itemCol.enabled = false;
        }

        //Check for and disable rigidbody physics
        itemRB = item.GetComponent<Rigidbody>();
        if(itemRB)
        {
            rbSettings[0] = itemRB.detectCollisions;
            rbSettings[1] = itemRB.freezeRotation;
            rbSettings[2] = itemRB.useGravity;

            itemRB.detectCollisions = false;
            itemRB.freezeRotation = true;
            itemRB.useGravity = false;
        }
        
        return true;
    }

    public virtual bool Unequip()
    {
        if(!HasItem)
        {
            return false;
        }

        equippedItem = null;

        //Re-enable collision collider if it exists
        if (itemCol)
        {
            itemCol.enabled = true;
        }
        itemCol = null;

        //Re-enable rigidbody if it exists
        if (itemRB)
        {
            itemRB.detectCollisions = rbSettings[0] ;
            itemRB.freezeRotation = rbSettings[1];
            itemRB.useGravity = rbSettings[2];
        }

        return true;
    }

    public virtual bool UseItem(Actor user)
    {
        if(!HasItem)
        {
            return false;
        }

        return equippedItem.Use(user);
    }
}
