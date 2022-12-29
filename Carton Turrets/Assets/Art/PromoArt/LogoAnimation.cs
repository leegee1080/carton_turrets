using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LogoAnimation : MonoBehaviour
{
    [SerializeField]Image _img;

    [SerializeField]float _startDelay;
    [SerializeField]float _inTime;
    [SerializeField]float _totalTime;
    Tween _inWaveTween;
    [SerializeField]float _waveFilterAmount = 0.1f;
    [SerializeField]float _waveFilterMin = 0.05f;
    Tween _inAlphaTween;
    Tween _outAlphaTween;
    [SerializeField]float _alphaAmount = 0f;

    private void Start()
    {
        StartCoroutine(StartDelay());
        StartCoroutine(NextSceneCountDown());
        
    }

    private void PlayEffects()
    {
        _inWaveTween = DOTween.To (() => _waveFilterAmount,
        w => _waveFilterAmount = w, _waveFilterMin, _inTime);
        _inWaveTween.OnUpdate (UpdateWaveStr);
        _inWaveTween.SetUpdate(true);

        _inAlphaTween = DOTween.To (() => _alphaAmount,
        a => _alphaAmount = a, 1, _inTime);
        _inAlphaTween.OnUpdate (UpdateAlpha);
        _inAlphaTween.SetUpdate(true);
    }

    private void FadeEffects()
    {
        _outAlphaTween = DOTween.To (() => _alphaAmount,
        a => _alphaAmount = a, 0, _inTime);
        _outAlphaTween.OnUpdate (UpdateAlpha);
        _outAlphaTween.OnComplete(NewScene);
        _outAlphaTween.SetUpdate(true);
    }
    public void UpdateWaveStr()
    {
        _img.material.SetFloat("_RoundWaveStrength", _waveFilterAmount);
    }
    public void UpdateAlpha()
    {
        _img.color = new Color(1,1,1,_alphaAmount);
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSecondsRealtime(_startDelay);
        AudioController.singleton.PlaySound("promo_moist_drop");
        PlayEffects();
    }
    IEnumerator NextSceneCountDown()
    {
        yield return new WaitForSecondsRealtime(_totalTime);
        FadeEffects();
    }

    private void NewScene()
    {
        GlobalVolumeController.singleton.NewScene(1);
    }

}
