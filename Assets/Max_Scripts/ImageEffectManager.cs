using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageEffectManager : MonoBehaviour {

    public Material[] MaterialList;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(MaterialList.Length == 0)
        {
            Graphics.Blit(source, destination);
        }
        
        foreach(Material m in MaterialList)
        {
            Graphics.Blit(source, destination, m);
        }
    }
}
