using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageEffectManager : MonoBehaviour {

    public Material imageEffect;

    //public float setFadeSpeed = 1.0f;
    //public bool toggleScreenBlack = false;

    public float targetBloodiness = 0.0f;
    public float bloodySpeed = 5.0f;
    public Color BloodyScreenColor;
    public Color DefaultVignetteColor;

    protected float _timer;
    protected float _currentBloodiness = 0.0f;

    protected bool _screenIsBlack = false;
    protected float _screenFadeSpeed = 1.0f;
    protected float _currentFadeAmount = 1.0f;
    protected float _targetFadeAmount = 0.0f;


    private void Update()
    {
        /*if(toggleScreenBlack)
        {
            CrossFadeBlack(!_screenIsBlack, setFadeSpeed);
            toggleScreenBlack = false;
        }*/

        _currentBloodiness = Mathf.MoveTowards(_currentBloodiness, targetBloodiness, bloodySpeed * Time.deltaTime);
        _currentFadeAmount = Mathf.MoveTowards(_currentFadeAmount, _targetFadeAmount, _screenFadeSpeed * Time.deltaTime);
        
        Color VignetteColor = ColorLerp(DefaultVignetteColor, BloodyScreenColor, _currentBloodiness);
        Color FinalColor = ColorLerp(VignetteColor, new Color(0.0f, 0.0f, 0.0f, 0.0f), _currentFadeAmount);

        if (imageEffect)
        {
            imageEffect.SetColor("_VColor", FinalColor);
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(imageEffect) { Graphics.Blit(source, destination, imageEffect); }
        else            { Graphics.Blit(source, destination); }
    }

    protected Color ColorLerp(Color a, Color b, float t)
    {
        Color c;
        c.r = Mathf.Lerp(a.r, b.r, t);
        c.g = Mathf.Lerp(a.g, b.g, t);
        c.b = Mathf.Lerp(a.b, b.b, t);
        c.a = Mathf.Lerp(a.a, b.a, t);
        return c;
    }

    public void CrossFadeBlack(bool toBlack, float duration)
    {
        _screenFadeSpeed = duration;

        if(toBlack)
        {
            _targetFadeAmount = 1.0f;
        }
        else
        {
            _targetFadeAmount = 0.0f;
        }

        _screenIsBlack = toBlack;
    }
}
