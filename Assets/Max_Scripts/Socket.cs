using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socket : MonoBehaviour {

    protected Item equippedItem;
    protected Transform oldParent;
    protected Rigidbody itemRB;

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

        itemRB = item.GetComponent<Rigidbody>();
        if(itemRB)
        {
            itemRB.detectCollisions = false;
            itemRB.useGravity = false;
        }
        
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

        if (itemRB)
        {
            itemRB.detectCollisions = true;
            itemRB.useGravity = true;
        }
        itemRB = null;
        
        return true;
    }
}
