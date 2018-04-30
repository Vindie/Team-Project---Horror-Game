using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchFalling : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Torch t = other.gameObject.GetComponent<Torch>();
        if(t)
        {
            if (t.lifeSpanDecreasingCheck())
            {
                t.LightOff();
            }
        }
    }
}
