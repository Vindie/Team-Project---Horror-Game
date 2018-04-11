using System.Collections.Generic;

public class Modifier
{
    public Modifier(float v, uint k)
    {
        value = v;
        key = k;
    }

    public float value;

    public uint key;
}

public class ModifierTable {

    private List<KeyValuePair<Actor, Modifier>> modifierTable;

    protected uint keyCounter;

    public ModifierTable()
    {
        modifierTable = new List<KeyValuePair<Actor, Modifier>>();
        keyCounter = 0;
    }

    public virtual int Add(float newValue, Actor source)
    {
        modifierTable.Add(new KeyValuePair<Actor, Modifier>(source, new Modifier(newValue, keyCounter)));
        keyCounter++;
        return (int)keyCounter - 1;
    }

    public virtual void Remove(Actor source)
    {
        for(int i = 0; i < modifierTable.Count; i++)
        {
            if(modifierTable[i].Key == source)
            {
                modifierTable.RemoveAt(i);
            }
        }
    }

    public virtual void Remove(Actor source, int key)
    {
        for (int i = 0; i < modifierTable.Count; i++)
        {
            if (modifierTable[i].Key == source && modifierTable[i].Value.key == key)
            {
                modifierTable.RemoveAt(i);
            }
        }
    }

    public virtual bool KeyIsActive(int key)
    {
        if(key < 0 || key > keyCounter) //Ignore trivial cases
        {
            return false;
        }

        for(int i = 0; i < modifierTable.Count; i++)
        {
            if(modifierTable[i].Value.key == key)
            {
                return true;
            }
        }

        return false;
    }

    public virtual Actor[] GetModifyingActors()
    {
        List<Actor> l = new List<Actor>();

        for(int i = 0; i < modifierTable.Count; i++)
        {
            l.Add(modifierTable[i].Key);
        }

        return l.ToArray();
    }

    public virtual float Sum()
    {
        float sum = 0.0f;

        for (int i = 0; i < modifierTable.Count; i++)
        {
            sum += modifierTable[i].Value.value;
        }

        return sum;
    }

    public virtual float Product()
    {
        float product = 1.0f;

        for (int i = 0; i < modifierTable.Count; i++)
        {
            product *= modifierTable[i].Value.value;
        }

        return product;
    }

    public virtual float Max()
    {
        if(modifierTable.Count == 0)
        {
            return float.NaN;
        }

        float max = modifierTable[0].Value.value;

        for (int i = 1; i < modifierTable.Count; i++)
        {
            float temp = modifierTable[i].Value.value;
            if (temp > max)
            {
                max = temp;
            }
        }

        return max;
    }

    public virtual float Min()
    {
        if (modifierTable.Count == 0)
        {
            return float.NaN;
        }

        float min = modifierTable[0].Value.value;

        for (int i = 1; i < modifierTable.Count; i++)
        {
            float temp = modifierTable[i].Value.value;
            if (temp < min)
            {
                min = temp;
            }
        }

        return min;
    }

    public virtual float Mean()
    {
        return Sum() / modifierTable.Count;
    }
}
