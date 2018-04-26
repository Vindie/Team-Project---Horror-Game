using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageEffectManager : MonoBehaviour {

    public Material imageEffect;

    public float targetBloodiness = 0.0f;
    public float bloodyLerpFactor = 0.2f;
    public Color BloodyScreenRGB;
    public Color DefaultVignetteColor;

    protected float _timer;
    [SerializeField]
    protected float _currentBloodiness = 0.0f;

    private void Update()
    {
        _currentBloodiness = Mathf.Lerp(targetBloodiness, _currentBloodiness, bloodyLerpFactor);
        if(imageEffect)
        {
            imageEffect.SetColor("Vignette Color", colorLerp(DefaultVignetteColor, BloodyScreenRGB, _currentBloodiness));
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(imageEffect) { Graphics.Blit(source, destination, imageEffect); }
        else            { Graphics.Blit(source, destination); }
    }

    protected Color colorLerp(Color a, Color b, float t)
    {
        Color c;
        c.r = Mathf.Lerp(a.r, b.r, t);
        c.g = Mathf.Lerp(a.g, b.g, t);
        c.b = Mathf.Lerp(a.b, b.b, t);
        c.a = Mathf.Lerp(a.a, b.a, t);
        return c;
    }
}
