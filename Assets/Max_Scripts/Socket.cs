using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socket : MonoBehaviour {

    protected Item equippedItem;
    protected Transform oldParent;

    public virtual bool HasItem
    {
        get { return equippedItem; }
    }

	public virtual bool Equip(Item item)
    {
        if (HasItem)
        {
            return false;
        }

        oldParent = item.transform.parent;
        item.transform.parent = transform;
        equippedItem = item;
        return true;
    }

    public virtual bool Unequip(bool returnToOriginalParent = false)
    {
        if(!HasItem)
        {
            return false;
        }

        if(returnToOriginalParent)
        {
            equippedItem.transform.parent = oldParent;
        }
        else
        {
            equippedItem.transform.parent = null;
        }

        equippedItem = null;
        oldParent = null;
        return true;
    }
}
